docker build -t clipboardutil-functionapp1 .
docker run -p 8081:80 clipboardutil-functionapp1

http://localhost:8081/api/swagger/ui