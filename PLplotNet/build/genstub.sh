#!/bin/bash

touch empty.c
gcc -shared -o libplplotd.so.12 empty.c    
gcc -Wl,--no-as-needed -shared -o libplplot.so -fPIC -L. -l:libplplotd.so.12
rm -f libplplotd.so.12
rm -f empty.c


