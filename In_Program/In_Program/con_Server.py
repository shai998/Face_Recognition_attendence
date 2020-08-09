import socket
import sys

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
#server_address = ('172.18.79.165', 1234)
server_address = ('203.234.10.63', 1234)

def Conn_Server():
    try:
        sock.connect(server_address)
    except(Exception):
        print(Exception + server_address[0])

def DisConn_Server():
    sock.shutdown(1)
    sock.close()

def Send_to_Server(message):
    # 메시지를 바이너리(byte)형식으로 변환한다.
    data = message.encode();
    # 메시지 길이를 구한다.
    length = len(data);
    # server로 little 엔디언 형식으로 데이터 길이를 전송한다.
    sock.sendall(length.to_bytes(4, byteorder="little"));
    # 데이터를 전송한다.
    sock.sendall(data);

def Recv_by_Server():
    # 데이터의 길이를 받는다
    data = sock.recv(4);
    # 데이터 길이는 little 엔디언 형식으로 int를 변환한다.
    length = int.from_bytes(data, "little");
    # 데이터 길이를 받는다.
    data = sock.recv(length);
    # 데이터를 수신한다.
    msg = data.decode();

    if "DBCHANGED" in msg:
        pass
    else:
        print('received "%s"' % msg)
        return  msg

def Count_Stu(Query):
    message = "COUNT_STU@" + Query

    Send_to_Server(message)

    msg = None
    msg = Recv_by_Server()

    return msg

def SELECT_STUDENT(Query):
    message = "SELECT_S@" + Query

    Send_to_Server(message)

    msg = None
    msg = Recv_by_Server()

    return msg

def SELECT_ID(Query):
    message = "SELECT_ID@" + Query

    Send_to_Server(message)

    msg = None
    msg = Recv_by_Server()

    return msg

def SELECT_BD(Query):
    message = "SELECT_BD@" + Query

    Send_to_Server(message)

    msg = None
    msg = Recv_by_Server()

    return msg

def UPDATE(Query):
    message = "UPDATE@" + Query

    Send_to_Server(message)

def INSERT(Query):
    message = "INSERT@" + Query

    Send_to_Server(message)

def DELETE(Query):
    message = "DELETE@" + Query

    Send_to_Server(message)