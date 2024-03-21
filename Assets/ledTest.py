from time import sleep

while True:
    sleep(0.01)
    f = open("motorOutput.txt", "r")
    n = f.read()
    print(n)
    