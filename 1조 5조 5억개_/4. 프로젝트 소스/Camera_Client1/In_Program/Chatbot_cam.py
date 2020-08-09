import threading
import pygame

import speech_recognition as sr
import In_Program as ip
import DataBase as db


from konlpy.tag import Kkma
from gtts import gTTS

kkma = Kkma()
Talking = False

def ntc_vc(name):
    if pygame.mixer.get_busy() == True:
        pygame.mixer.stop()
    result = name + "님 출근하셨습니다."

    tts = gTTS(text=result, lang='ko') #en
    voice_file = name + "notice.mp3"
    tts.save(voice_file)

    freq = 24000    # sampling rate, 44100(CD), 16000(Naver TTS), 24000(google TTS) = 말하는 속도 지정하기. 디폴트로하면 느림
    bitsize = -16   # signed 16 bit. support 8,-8,16,-16 = 16비트 사운드
    channels = 1    # 1 is mono, 2 is stereo = 모노 채널
    buffer = 2048   # number of samples (experiment to get right sound)

    # default : pygame.mixer.init(frequency=22050, size=-16, channels=2, buffer=4096)
    pygame.mixer.init(freq, bitsize, channels, buffer)
    pygame.mixer.music.load(voice_file) #재생할 mp3파일 지정(같은 폴더 안에 있어야 함)
    pygame.mixer.music.play()

    clock = pygame.time.Clock()
    while pygame.mixer.music.get_busy():
        clock.tick(30)
    pygame.mixer.quit()

def ans_vc(text,count):
    tts = gTTS(text=text, lang='ko') #en
    tts.save(str(count) + "answer.mp3")

    voice_file = str(count) + "answer.mp3"

    freq = 24000    # sampling rate, 44100(CD), 16000(Naver TTS), 24000(google TTS) = 말하는 속도 지정하기. 디폴트로하면 느림
    bitsize = -16   # signed 16 bit. support 8,-8,16,-16 = 16비트 사운드
    channels = 1    # 1 is mono, 2 is stereo = 모노 채널
    buffer = 2048   # number of samples (experiment to get right sound)

    # default : pygame.mixer.init(frequency=22050, size=-16, channels=2, buffer=4096)
    pygame.mixer.init(freq, bitsize, channels, buffer)
    pygame.mixer.music.load(voice_file) #재생할 mp3파일 지정(같은 폴더 안에 있어야 함)
    pygame.mixer.music.play()

    clock = pygame.time.Clock()
    while pygame.mixer.music.get_busy():
        clock.tick(30)
    pygame.mixer.quit()

def ans_list(question_mrph):
    result = None
    try:
        if ('그만' in question_mrph or '종료' in question_mrph):
            result = '대화를 종료합니다'

        elif ('여기' in question_mrph or '비트' in question_mrph):
            print("1번 들어옴")
            if ('무엇' in question_mrph or '뭐' in question_mrph or '뭐하' in question_mrph):
                print("2번 들어옴")
                result = '입학이 긍지가 되고 수료가 날개가 되는 상위1%전문가 양성을 위한 교육과정을 밟는 곳입니다'
            elif ('몇' in question_mrph):
                if ('명' in question_mrph or '인원' in question_mrph):
                    count = db.Count_STUDNET()
                    result = '현재 ' + count + ' 명이 수료중에 있습니다'
                    #result = '현재 24 명이 수료중에 있습니다'
                elif ('기' in question_mrph):
                    result = '현재 우송비트 고급과정은 30기입니다'
        else:
             result = ("질문을 모르겠어요")
    except Exception as e : 
        print(e)
        result = '질문 받을 수 있는 항목이 아닙니다' 

    return result

def chat_start(data):
    global Talking
    Talking = True
    question = None
    count = 0
    result = "어서오세요 우송 비트센터입니다. 저에게 질문을 해주세요. 질문은 화면 하단에있는 질문중 하나를 골라주세요."
    ans_vc(result,count)
    count = count + 1

    while True:
        r = sr.Recognizer()
        mic = sr.Microphone()
        with mic as source:
            print('Im Listening...')
            audio = r.listen(source)
        try:
            question = r.recognize_google(audio,language='ko-KR') #input()
        except:
            Talking = False
            break      
        else:
            print(question)
            question_mrph = kkma.morphs(question) #형태소 분석
            result = ans_list(question_mrph) # 대답고르기
            print(result) #대답 출력
            if result == '대화를 종료합니다' or '질문이 없으시면 전 다시 돌아갈게요':
                ans_vc(result,count) #대답 mp3화 및 재생
                Talking = False
                break
            count = count + 1
            if count >4 :
                count = 0
            ans_vc(result,count) #대답 mp3화 및 재생
