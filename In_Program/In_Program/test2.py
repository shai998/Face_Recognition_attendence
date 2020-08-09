import queue as q
import threading

def Test_Thread(queue):
    count = 0
    while True:
        if(count == 100):
            break
        print(count)
        count += 1
        try:
            data = queue.get()
            print(data)
        except:
            pass

msg_q = q.Queue()

test_t = threading.Thread(target=Test_Thread, args=(msg_q,))
test_t.start()

msg_q.put('banana')
msg_q.put('banana')
msg_q.put('banana')
msg_q.put('banana')
msg_q.put('banana')
msg_q.put('banana')
msg_q.put('banana')
msg_q.put('banana')