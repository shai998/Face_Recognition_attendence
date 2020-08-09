# -*- coding: utf-8 -*-
import Main as m
import sys, datetime
from PyQt5.QtWidgets import *
from PyQt5 import uic, QtCore, QtGui
import threading
import queue as Queue
import cv2

#UI파일 연결
#단, UI파일은 Python 코드 파일과 같은 디렉토리에 위치해야한다.
form_class = uic.loadUiType("Camera_Widget.ui")[0]
q = Queue.Queue()
msg_q = Queue.Queue()
running = False
#스레드 선언
main_thread = threading.Thread(target=m.Main, args=(q, msg_q,))

class OwnImageWidget(QWidget):
    def __init__(self, parent=None):
        super(OwnImageWidget, self).__init__(parent)
        self.image = None

    def setImage(self, image):
        self.image = image
        sz = image.size()
        self.setMinimumSize(sz)
        self.update()

    def paintEvent(self, event):
        qp = QtGui.QPainter()
        qp.begin(self)
        if self.image:
            qp.drawImage(QtCore.QPoint(0, 0), self.image)
        qp.end()

'''
def Camera(cam, queue, width, height):
    global running
    capture = cv2.VideoCapture(cam)
    capture.set(cv2.CAP_PROP_FRAME_WIDTH, width)
    capture.set(cv2.CAP_PROP_FRAME_HEIGHT, height)
    record = False
    
    while(running):
        frame = {}        
        capture.grab()
        retval, img = capture.retrieve(0)
        frame["img"] = img
        
        if queue.qsize() < 10:
            queue.put(frame)
        else:
            print(queue.size())

    capture.release()
    cv2.destroyAllWindows() 
'''

#화면을 띄우는데 사용되는 Class 선언
class WindowClass(QMainWindow, form_class) :
    def __init__(self) :
        super().__init__()
        self.setupUi(self)
        #타이머를 사용하여 시작시 자동시작
        QtCore.QTimer.singleShot(100, self.start)

        #QWidget 크기 설정(카메라)
        self.window_width = self.ImgWidget.frameSize().width()
        self.window_height = self.ImgWidget.frameSize().height()
        self.ImgWidget = OwnImageWidget(self.ImgWidget)
  
        #프레임 업데이트 스레드
        self.timer = QtCore.QTimer(self)
        self.timer.timeout.connect(self.update_frame)
        self.timer.timeout.connect(self.update_msg)
        self.timer.start(1)
   
        self.chat_lbl1.setText("비트는 뭐하는 곳인가요?")
        self.chat_lbl2.setText("비트는 지금 몇명이 수료중인가요?")
        self.chat_lbl3.setText("비트는 현재 몇번째 기수가 수료중인가요?")
        self.end_lbl.setText("종료를 원하시면 [종료, 그만]을 말씀해주세요")

        #self.att_log.addItem('리스트위젯 테스트 문구')

    #메인 함수 시작스레드
    def start(self):
        main_thread.start()
        global running
        running = True

    #큐에 쌓이 프레임을 순차적으로 가져온다. 
    #시작할때 이 스레드가 시작되며 카메라가 시작되면 큐가 쌓이고 프레임을 보여주기 시작한다.
    def update_frame(self):
        if not q.empty():
            #self.startButton.setText('Camera is live')
            frame = q.get() #완성된 프레임을 해당 q에 넣어주고 꺼내오면됨


            #img = frame["img"]
            img = frame
            
            img_height, img_width, img_colors = img.shape
            scale_w = float(self.window_width) / float(img_width)
            scale_h = float(self.window_height) / float(img_height)
            scale = min([scale_w, scale_h])

            if scale == 0:
                scale = 1
            
            img = cv2.resize(img, None, fx=scale, fy=scale, interpolation = cv2.INTER_CUBIC)
            img = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
            height, width, bpc = img.shape
            bpl = bpc * width
            image = QtGui.QImage(img.data, width, height, bpl, QtGui.QImage.Format_RGB888)
            
            self.ImgWidget.setImage(image)

    def update_msg(self):
        if not msg_q.empty():
            msg = msg_q.get()
            self.att_log.addItem(msg)


    def closeEvent(self, event):
        global running
        running = False

if __name__ == "__main__" :
    #QApplication : 프로그램을 실행시켜주는 클래스
    app = QApplication(sys.argv)
    #WindowClass의 인스턴스 생성
    myWindow = WindowClass() 
    #프로그램 화면을 보여주는 코드
    myWindow.show()
    #프로그램을 이벤트루프로 진입시키는(프로그램을 작동시키는) 코드
    app.exec_()
