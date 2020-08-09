import pymssql, cv2
import con_Server as cs
import Chatbot_cam as ct
import ParseData as pd
import socket
import sys

from datetime import datetime, time, date

#함수
#region 

# STU_ID   = 학번
# STU_NAME = 이름
# STU_DEP  = 학과
def Selecet_STUDENT():
    STU_NAME = []

    Query = ("SELECT * FROM STUDENT" )
    msg = cs.SELECT_STUDENT(Query)
    STU_NAME = pd.Parse_Data(msg)
    
    return STU_NAME

def Count_STUDNET():
    Query = ("SELECT COUNT(*) FROM STUDENT" )
    count_msg = cs.Count_Stu(Query)
    count = pd.Parse_Data(count_msg)

    return count

def Insert_STU_DATA(STU_ID, STU_NAME, STU_DEP):
    cs.INSERT("INSERT INTO STUDENT VALUES(" + STU_ID + ",'" + STU_NAME + "','" + STU_DEP + "');")


def Update_ATT_DATA(now, STU_ID):
    Attend_ = time(9, 10)
    ATT_TIME = time( now.hour, now.minute, now.second )
    ATT_DATE = date( now.year, now.month, now.day )

    if ATT_TIME <= Attend_:
        ATT_LATE = False
    else:
        ATT_LATE = True

    ATT_TIME = str(ATT_TIME)
    ATT_DATE = str(ATT_DATE)

    #학번과 날짜를통해 해당 날짜의 출결 정보를 받아온다
    Query = cs.SELECT_BD("SELECT ATT_BOOL, ATT_DATE FROM DAILY_ATTEND WHERE STU_ID = " + STU_ID + " AND ATT_DATE = '" + ATT_DATE + "';")
    ATT_BOOL_, ATT_DATE_ = pd.Parse_Data(Query)

    #날짜가 같고 해당인원이 출석이 아닌경우 출석처리를 한후에 출석시간, 지각여부 기입
    if ATT_DATE_ == ATT_DATE and ATT_BOOL_ == 'False':
        cs.UPDATE("UPDATE DAILY_ATTEND SET ATT_BOOL = 1, ATT_TIME = '" + ATT_TIME + "', ATT_LATE = '" + str(ATT_LATE) + "' WHERE STU_ID = '" + STU_ID + "' AND ATT_DATE = '" + ATT_DATE + "';")
        return True
    else:
        return False

def Update_IN_DATA(now, STU_NAME):
    STU_ID = []
    IN_TIME = str(time( now.hour, now.minute, now.second ))
    IN_DATE = str(date( now.year, now.month, now.day))

    #학생의 이름으로 학번을 받는다
    Query = cs.SELECT_ID("SELECT STU_ID FROM STUDENT WHERE STU_NAME = '" + STU_NAME + "';")
    STU_ID = pd.Parse_Data(Query)
    
    for i in STU_ID:
        cs.INSERT("INSERT INTO INOUT (STU_ID, INOUT_BOOL, IN_TIME, INOUT_DATE) VALUES (" + i + ", 1, '" + IN_TIME + "','" + IN_DATE + "') ;")
        Is_Update = Update_ATT_DATA(now, i)

    return Is_Update

def Update_OUT_DATA(now, STU_NAME):
    STU_ID = []
    OUT_TIME = str(time( now.hour, now.minute, now.second ))
    now.ToString("yyyy-MM-dd")
    OUT_DATE = str(date( now.year, now.month, now.day))

    #학생의 이름으로 학번을 받는다
    Query = cs.SELECT_ID("SELECT STU_ID FROM STUDENT WHERE STU_NAME = '" + STU_NAME + "';")
    STU_ID = pd.Parse_Data(Query)
    
    for i in STU_ID:
        cs.INSERT("INSERT INTO INOUT (STU_ID, INOUT_BOOL, OUT_TIME, INOUT_DATE) VALUES (" + i + ", 0, '" + OUT_TIME + "','" + OUT_DATE + "') ;")

#endregion        