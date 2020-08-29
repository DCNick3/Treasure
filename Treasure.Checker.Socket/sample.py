#!/usr/bin/env python3

import random
import socket

s = socket.socket()
s.connect(('127.177.0.13', 56454))
f = s.makefile('rw')

cc = ["Движение Вверх", "Движение Влево", "Движение Вправо", "Движение Вниз"]

while True:
    c = random.choice(cc)
    f.write(c + '\n')
    f.flush()
    i = f.readline()
    if i == "Победа":
        break
