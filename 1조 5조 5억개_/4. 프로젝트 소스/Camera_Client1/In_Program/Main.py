import face_detect as fd
import In_Program as ip
import con_Server as cs
import dlib, onnx, threading
import tensorflow.compat.v1 as tf
import onnxruntime as ort

from onnx_tf.backend import prepare
from imutils import face_utils

def Main(queue, msg_queue):
    #region init
    # load the model, create runtime session & get input variable name
    onnx_model = onnx.load('model/ultra_light_640.onnx')
    predictor = prepare(onnx_model)
    ort_session = ort.InferenceSession('model/ultra_light_640.onnx')
    input_name = ort_session.get_inputs()[0].name

    shape_predictor = dlib.shape_predictor('model/shape_predictor_68_face_landmarks.dat')
    fa = face_utils.facealigner.FaceAligner(shape_predictor, desiredFaceWidth=112, desiredLeftEye=(0.3, 0.3))
    #endregion
    cs.Conn_Server()

    try:
        with tf.Graph().as_default():
            with tf.Session() as sess:
                ip.In_Program_M(sess, ort_session, input_name, fa, queue, msg_queue)

        cs.DisConn_Server()
    except Exception as e:
        print(e)
        cs.DisConn_Server()