#!/bin/bash

docker build -t clipboardutilfunctionapp2 -f Dockerfile ..
docker run -d -p 8080:80 --name test1 clipboardutilfunctionapp2