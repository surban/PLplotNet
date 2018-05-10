#!/bin/bash

TARGET="libplplot.so.15"

touch empty.c
gcc -shared -o $TARGET empty.c    
gcc -Wl,--no-as-needed -shared -o libplplot.so -fPIC -L. -l:$TARGET
rm -f $TARGET
rm -f empty.c

