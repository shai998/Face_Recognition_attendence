import DataBase as db
import ParseData as pd
import con_Server as cs
import datetime as dt

STU_ID = []
now = dt.datetime.now()
date = str(dt.date(now.year, now.month, now.day))

cs.Conn_Server()
cs.Send_to_Server("IN")

Query = cs.SELECT_ID("SELECT STU_ID FROM STUDENT")
STU_ID = pd.Parse_Data(Query)

for i in STU_ID:
    cs.UPDATE("UPDATE DAILY_ATTEND SET ATT_BOOL = 0, ATT_LATE = 0, ATT_TIME = NULL WHERE STU_ID = " + i + " AND ATT_DATE = '" + date + "';")

cs.DELETE("DELETE INOUT WHERE INOUT_ID >= 7 ")

cs.DisConn_Server()