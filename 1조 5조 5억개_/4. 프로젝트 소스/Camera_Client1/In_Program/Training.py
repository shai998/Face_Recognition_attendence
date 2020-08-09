import cv2, dlib, onnx, pickle, os
import numpy as np
import tensorflow as tf
import face_detect as fd
import onnxruntime as ort

from PIL import Image
from imutils import face_utils
from onnx_tf.backend import prepare

p_path = os.path.abspath(os.pardir)
new_path_model = p_path + '/In_Program/model/'
new_path_face = p_path + '/In_Program/faces/'
new_path_embd = p_path + '/In_Program/embeddings/'

#region onnx init
# load the model, create runtime session & get input variable name
onnx_model = onnx.load(new_path_model + 'ultra_light_640.onnx')
predictor = prepare(onnx_model)

ort_session = ort.InferenceSession(new_path_model + 'ultra_light_640.onnx')
input_name = ort_session.get_inputs()[0].name
#endregion

shape_predictor = dlib.shape_predictor(new_path_model + 'shape_predictor_68_face_landmarks.dat')
fa = face_utils.facealigner.FaceAligner(shape_predictor, desiredFaceWidth=112, desiredLeftEye=(0.3, 0.3))

TRAINING_BASE = new_path_face + "/training/"
dirs = os.listdir(TRAINING_BASE)

# images and names for later use
images = []
names = []

for label in dirs:
    now_label = None
    #i = index number / fn = file_name
    for i, fn in enumerate(os.listdir(os.path.join(TRAINING_BASE, label))):
        print(f"start collecting faces from {label}'s data")
        cap = cv2.VideoCapture(os.path.join(TRAINING_BASE, label, fn))
        if label != now_label or now_label == None:
            frame_count = 0
        now_label = label
        while True:
            print(frame_count)
            # read video frame
            # ret = is read image?(T, F) / raw_img = image data
            ret, raw_img = cap.read()
            # process every 5 frames
            #if frame_count % 3 == 0 and raw_img is not None:
            if raw_img is not None:
                h, w, _ = raw_img.shape
                img = cv2.cvtColor(raw_img, cv2.COLOR_BGR2RGB)
                img = cv2.resize(img, (640, 480))
                img_mean = np.array([127, 127, 127])
                img = (img - img_mean) / 128
                img = np.transpose(img, [2, 0, 1])
                img = np.expand_dims(img, axis=0)
                img = img.astype(np.float32)

                confidences, boxes = ort_session.run(None, {input_name: img})
                boxes, labels, probs = fd.predict(w, h, confidences, boxes, 0.7)
                
                # if face detected
                if boxes.shape[0] > 0:
                    x1, y1, x2, y2 = boxes[0,:]
                    # convert to gray scale image
                    gray = cv2.cvtColor(raw_img, cv2.COLOR_BGR2GRAY)
                    # align and resize
                    aligned_face = fa.align(raw_img, gray, dlib.rectangle(left = x1, top=y1, right=x2, bottom=y2))
                    aligned_face = cv2.resize(aligned_face, (112,112))

                    # write to file
                    cv2.imwrite(new_path_face + f'tmp/{label}_{frame_count}.jpg', aligned_face)

                    img2 = Image.open(new_path_face + f'tmp/{label}_{frame_count}.jpg')
                    img2 = img2.convert("RGB")
                    img2 = img2.resize((112, 112))

                    aligned_face = aligned_face - 127.5
                    aligned_face = aligned_face * 0.0078125
                    images.append(aligned_face)
                    names.append(label)

            frame_count += 1
            # if video end
            if frame_count == cap.get(cv2.CAP_PROP_FRAME_COUNT):
                break

with tf.Graph().as_default():
    with tf.Session() as sess:
        print("loading checkpoint ...")
        saver = tf.train.import_meta_graph(new_path_model + 'mfn.ckpt.meta')
        saver.restore(sess, new_path_model + 'mfn.ckpt')
        
        images_placeholder = tf.get_default_graph().get_tensor_by_name("input:0")
        embeddings = tf.get_default_graph().get_tensor_by_name("embeddings:0")
        phase_train_placeholder = tf.get_default_graph().get_tensor_by_name("phase_train:0")

        # calc face embeddings
        feed_dict = { images_placeholder: images, phase_train_placeholder:False }
        embeds = sess.run(embeddings, feed_dict=feed_dict)
        print("saving embeddings")
        with open(new_path_embd + "embeddings.pkl", "wb") as f:
            pickle.dump((embeds, names), f)