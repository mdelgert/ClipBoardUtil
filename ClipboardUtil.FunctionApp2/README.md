https://stackoverflow.com/questions/58765236/how-to-build-dockerfile-created-by-visual-studio-2019-from-command-line

docker build -t clipboardutilfunctionapp2 -f Dockerfile ..
docker run -d -p 8080:80 --name test1 clipboardutilfunctionapp2
docker run -d -p 8081:80 --name test2 clipboardutilfunctionapp2
docker inspect test1 | jq '.[0].NetworkSettings.Ports'
http://localhost:8080/api/swagger/ui
http://localhost:8081/api/swagger/ui