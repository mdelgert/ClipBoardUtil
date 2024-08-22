#!/bin/bash

# Link:  https://hub.docker.com/r/emqx/emqx

# Note: Cross platform runs on pi as well as macos and windows.

docker run -d --name emqx -p 18083:18083 -p 1883:1883 -p 8883:8883 -p 8083:8083 -p 8084:8084 emqx/emqx:latest