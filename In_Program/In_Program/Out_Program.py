import pickle, dlib, onnx, cv2, sys, os
import tensorflow.compat.v1 as tf
import face_detect as fd
import onnxruntime as ort
import numpy as np
import threading

from datetime import datetime, time, date
from onnx_tf.backend import prepare
from imutils import face_utils

#정확도
threshold = 0.4

#Dictionary init
count = {'unknown' : 0 }
namelist = []

# load distance
with open("embeddings/embeddings.pkl", "rb") as f:
    (saved_embeds, names) = pickle.load(f)

def threading_(Time_, name):
    out_t = threading.Thread(target = db.Update_OUT_DATA, args=(Time_, name))
    out_t.start()

def Out_Program_M(ort_session, input_name, fa,images_placeholder, phase_train_placeholder, sess, embeddings, conn, cur, video_capture):
    print("out 돌아욧")
    now = datetime.now()
    Today_ = str(date(now.year, now.month, now.day))
    Time_ = str(time(now.hour, now.minute, now.second))

    fps = video_capture.get(cv2.CAP_PROP_FPS)
    ret, frame = video_capture.read()
    frame = cv2.flip(frame, 1)

    # preprocess faces
    h, w, _ = frame.shape
    img = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    img = cv2.resize(img, (640, 480))
    img_mean = np.array([127, 127, 127])
    img = (img - img_mean) / 128
    img = np.transpose(img, [2, 0, 1])
    img = np.expand_dims(img, axis=0)
    img = img.astype(np.float32)

    # detect faces
    confidences, boxes = ort_session.run(None, {input_name: img})
    boxes, labels, probs = fd.predict(w, h, confidences, boxes, 0.7)

    # locate faces
    faces = []
    boxes[boxes<0] = 0
    for i in range(boxes.shape[0]):
        box = boxes[i, :]
        x1, y1, x2, y2 = box
        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        aligned_face = fa.align(frame, gray, dlib.rectangle(left = x1, top=y1, right=x2, bottom=y2))
        aligned_face = cv2.resize(aligned_face, (112,112))
        aligned_face = aligned_face - 127.5
        aligned_face = aligned_face * 0.0078125
        faces.append(aligned_face)

    # face embedding
    if len(faces)>0:
        predictions = []

        faces = np.array(faces)
        feed_dict = { images_placeholder: faces, phase_train_placeholder:False }
        embeds = sess.run(embeddings, feed_dict=feed_dict)
                
        # prediciton using distance
        for embedding in embeds:
            diff = np.subtract(saved_embeds, embedding)
            dist = np.sum(np.square(diff), 1)
            idx = np.argmin(dist)
            if dist[idx] < threshold:
                predictions.append(names[idx])     
                threading_(Time_, names[idx])
            else:
                predictions.append("unknown")

        # draw
        for i in range(boxes.shape[0]):
            box = boxes[i, :]
            text = f"{predictions[i]}"
            x1, y1, x2, y2 = box
            cv2.rectangle(frame, (x1, y1), (x2, y2), (80,18,236), 2)
            # Draw a label with a name below the face
            cv2.rectangle(frame, (x1, y2 - 20), (x2, y2), (80,18,236), cv2.FILLED)
            font = cv2.FONT_HERSHEY_DUPLEX
            cv2.putText(frame, text, (x1 + 6, y2 - 6), font, 0.3, (255, 255, 255), 1)

        return frame