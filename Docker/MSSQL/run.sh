#!/bin/bash

# Link: https://github.com/mdelgert/DevDocker/tree/main/MSSQL

# Note: Setup MSSQL instance

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password2024!" \
   -p 1433:1433 --name sql1 --hostname sql1 \
   -d mcr.microsoft.com/mssql/server:2022-latest

# docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password2024!" \
#    -p 1433:1433 --name sql1 --hostname sql1 \
#    -d mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04