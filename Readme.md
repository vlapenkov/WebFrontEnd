Пример работы с Docker
3 контейнера
WebFront
MyWebApi+SqlServer
Webapi2.Web+PostGresql
Identity server +  postgre + iodc
Refit

docker run --network=dockercompose751187152415958688_default  alpine wget -qO- WebApi2/api/products/getproducts

Важно!!!
Начало запуска  - http://local_host:5008/

Предварительно в hosts прописывается 192.168.0.106 local_host

192.168.0.106 host.docker.internal - не работает для is4, т.к. redirect c по oidc идет на localhost

26.01.2021 Сделал проброс access_token через refit  и авторизация проходит
03.02.2021 Samesite для chrome 
- ASPNETCORE_ENVIRONMENT=Development (ставим в docker-compose.override.yml) + другие переменные среды



