import threading
import pygame

import speech_recognition as sr
import Worker_Thread as wt
import In_Program as ip
import DataBase as db


from konlpy.tag import Kkma
from gtts import gTTS

kkma = Kkma()

question = ("종료")
question_mrph = kkma.morphs(question) #형태소 분석
print(question_mrph)