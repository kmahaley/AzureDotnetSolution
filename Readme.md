# Docker

## Commads

|Description| Sample cmd| example|
|-|-|-|
|find containers| |docker ps |
|find containers historic||docker ps -a|
|Pull and run image|docker run imageName -d|docker run redis -d|
|Port biniding|docker run imageName -d -phostPort:containerPort|docker run redis -d -p6379:6379|
|remove conatiner|docker rm containerId|docker rm 82f03602a050 |
|remove conatiner|docker rm containerName|docker rm redis |
|container log|docker log containerId| docker log 82f03602a050|
|container log|docker log containerName| docker log redis|
|container start|docker start containerName| docker start redis|
|container stop|docker stop containerName| docker stop redis|
|container stop|docker stop containerId| docker stop 82f03602a050|
|list images|docker images||
|create container with name|docker run -d -p6379:6379 --name containerName imageName:imageVersion|docker run -d -p6379:6379 --name redis redis:7.2|
|remove images|docker rmi imageName:imageVersion|docker rmi redis:7.2|
|inside container| docker exec -it containerName /bin/bash| docker exec -it redis /bin/bash|
|exit from inside container|exit||


