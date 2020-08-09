import face_detect as fd
import Out_Program as op
import DataBase as db

import dlib, onnx
import tensorflow.compat.v1 as tf
import onnxruntime as ort

from onnx_tf.backend import prepare
from imutils import face_utils


#region init
# load the model, create runtime session & get input variable name
onnx_model = onnx.load('model/ultra_light_640.onnx')
predictor = prepare(onnx_model)
ort_session = ort.InferenceSession('model/ultra_light_640.onnx')
input_name = ort_session.get_inputs()[0].name

shape_predictor = dlib.shape_predictor('model/shape_predictor_68_face_landmarks.dat')
fa = face_utils.facealigner.FaceAligner(shape_predictor, desiredFaceWidth=112, desiredLeftEye=(0.3, 0.3))
#endregion

db.Conn_Server()
try:
    with tf.Graph().as_default():
        with tf.Session() as sess:
            op.Out_Program_M(sess, ort_session, input_name, fa)

    db.DisConn_Server()
except Exception as e:
    print(e)
    db.DisConn_Server()
