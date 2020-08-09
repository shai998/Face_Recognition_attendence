import pickle, dlib, onnx, cv2, sys, os, threading, queue

import tensorflow.compat.v1 as tf
import onnxruntime as ort
import face_detect as fd
import Chatbot_cam as ct
import DataBase as db
import numpy as np

from datetime import datetime, time, date
from onnx_tf.backend import prepare
from imutils import face_utils

# load distance
with open("embeddings/embeddings.pkl", "rb") as f:
    (saved_embeds, names) = pickle.load(f)

'''
def Timer_thread():
    timer = threading.timer(10, Timer_thread)
    timer.start()
'''

def Chat_Thread(queue):
    while True:
        data = queue.get()
        print(data)

        if data == "unknown":
            ct.chat_start(data)
        else:
            ct.ntc_vc(data)



def Add_Dictionary():
    count = {'unknown' : 0 }

    STU_NAME = []
    STU_NAME = db.Selecet_STUDENT()

    for i in STU_NAME:
        count[i] = 0

    return count


def In_Program_M(sess, ort_session, input_name, fa, frame_queue, msg_queue):
    count_ = 0
    bot_count = 0
    #iou_threshold = 0.475
    iou_threshold = 0.575
    chat_queue = queue.Queue()
    count = Add_Dictionary()
    saver = tf.train.import_meta_graph('model/mfn.ckpt.meta')
    saver.restore(sess, 'model/mfn.ckpt')

    images_placeholder = tf.get_default_graph().get_tensor_by_name("input:0")
    embeddings = tf.get_default_graph().get_tensor_by_name("embeddings:0")
    phase_train_placeholder = tf.get_default_graph().get_tensor_by_name("phase_train:0")
    embedding_size = embeddings.get_shape()[1]

    chat_t = threading.Thread(target=Chat_Thread, args=(chat_queue,))
    chat_t.start()
    #영상 캡쳐
    video_capture_0 = cv2.VideoCapture(0)  #내장 카메라
    #video_capture_0 = cv2.VideoCapture(1)   #외장 카메라
    video_capture_0.set(3, 5000)   #최대치로 하기위해 그냥 큰값 넣은것임
    video_capture_0.set(4, 5000)   #최대치로 하기위해 그냥 큰값 넣은것임

    while True:
        now = datetime.now()
        from Chatbot_cam import Talking
        talk = Talking
        fps = video_capture_0.get(cv2.CAP_PROP_FPS)
        ret, frame = video_capture_0.read()
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
                if dist[idx] < iou_threshold:
                    predictions.append(names[idx])
                    print(names[idx])
                    if count[names[idx]] == 10:
                        Is_Update = db.Update_IN_DATA(now, names[idx])
                        msg = (names[idx] + "님 입장하셨습니다. 시간 : " + str(time(now.hour, now.minute, now.second)))
                        msg_queue.put(msg)
                        count[names[idx]] = 0
                        if Is_Update == True:
                            chat_queue.put(names[idx])
                    count[names[idx]] += 1
                    
                else:
                    predictions.append("unknown")

                    if talk != True:
                        if count_ == 20:
                            print("대화시작")
                            chat_queue.put("unknown")
                            count_ = 0
                        count_ += 1
                    else:
                        continue
            # draw
            for i in range(boxes.shape[0]):
                box = boxes[i, :]
                text = f"{predictions[i]}"
                x1, y1, x2, y2 = box
                cv2.rectangle(frame, (x1, y1), (x2, y2), (80,18,236), 2)
                # Draw a label with a name below the face
                cv2.rectangle(frame, (x1, y2 - 20), (x2, y2), (80,18,236), cv2.FILLED)
                font = cv2.FONT_HERSHEY_DUPLEX
                cv2.putText(frame, text, (x1 + 6, y2 - 6), font, 0.7, (255, 255, 255), 1)

        frame_queue.put(frame)
        #해당 frame을 queue에 집어 넣어야한다.
        #cv2.imshow('Video', frame)
        

        # Hit 'q' on the keyboard to quit!
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break
    # Release handle to the webcam
    video_capture_0.release()
    cv2.destroyAllWindows()
