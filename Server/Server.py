# codeing=utf8
import socket
import threading
import time
import sys
import json
import os

def socket_service():
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        s.bind(('127.0.0.1', 6666))
        s.listen(5)
    except socket.error as msg:
        print(msg)
        sys.exit(1)
    print('Waiting for connection ...')

    while 1:
        conn, addr = s.accept()
        t = threading.Thread(target=deal_data, args=(conn, addr))
        t.start()

def deal_data(conn, addr):
    print('Accept new connection from {}'.format(addr))
    while 1:
        data = conn.recv(1024)
        print('The data from {} is {}'.format(addr, data))
        #time.sleep(1)
        if data == 'exit' or not data:
            print('The connection of {} is close'.format(addr))
            break
        data2 = json.loads(data.decode())
        if (data2['Title'] == 'Login'):
            th = threading.Thread(target=LoginFunc, args=(conn, data2["UserName"], data2["Password"]))
            th.start()
        elif(data2['Title'] == 'UpdateUserInfo'):
            th = threading.Thread(target=UpdateUserInfoFunc, args=(conn, data2))
            th.start()
    conn.close()

def UpdateUserInfoFunc(conn, data):
    path = 'Acount/' + data['UserName'] + '.json'
    with open(path, 'r') as f:
        JsonData = json.load(f)
        JsonData["GameTimes"] = data["GameTimes"]
        JsonData["MaxLevelRecord"] = data["MaxLevelRecord"]
        data = JsonData
    with open(path, 'w') as f:
        json.dump(data, f)

def LoginFunc(conn, UserName, Password):
    path = 'Acount/' + UserName + '.json'
    if(os.path.exists(path)):
        with open(path, 'r') as f:
            JsonData = json.load(f)
            if(Password == JsonData["Password"]):
                data = [{'Title': 'UserInformation', 'UserName': UserName, 'GameTimes': JsonData["GameTimes"], 'MaxLevelRecord': JsonData["MaxLevelRecord"]}]
                data2 = json.dumps(data)
                conn.send(data2.encode())
            else:
                SendErrorMessage(conn, "用户名或密码错误！")
    else:
        SendErrorMessage(conn, "用户名或密码错误！")

def SendErrorMessage(conn, msg):
    data = [{'Title': 'Error', 'Message': msg}]
    data2 = json.dumps(data, ensure_ascii=False)
    conn.send(data2.encode(encoding='utf8'))

if __name__ == '__main__':
    socket_service()