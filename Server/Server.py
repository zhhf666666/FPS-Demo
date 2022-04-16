# codeing=utf8
import socket
import threading
import time
import sys
import json

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
    conn.close()

def LoginFunc(conn, UserName, Password):
    if(UserName == "admin" and Password == "admin"):
        data = [{'Title': 'UserInformation', 'UserName': UserName, 'GameTimes': '0', 'MaxLevelRecord': '0'}]
        data2 = json.dumps(data)
        conn.send(data2.encode())
    else:
        data = [{'Title':'Error', 'Message': '用户名或密码错误！'}]
        data2 = json.dumps(data, ensure_ascii=False)
        conn.send(data2.encode(encoding='utf8'))

if __name__ == '__main__':
    socket_service()