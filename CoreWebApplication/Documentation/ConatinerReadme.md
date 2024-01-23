# Docker container of app
- Check docker is running
- `docker --version`

## Build image
- Cd to folder where project `Dockerfile` exists

```
docker build -t kmahaley/coreapp:1.0 -f Dockerfile .
```

- Check images
```
docker images
```
- Should have image as `kmahaley/coreapp:1.0`

## Run image as container

- Run container from newly build image
```
docker run -d --name app -p8080:80 kmahaley/coreapp:1.0
```

- - goto `http:localhost:8080/swagger/index.html`
  - swagger documentation of the app
-  http://localhost:8080/item 
  - list of items

## Push to Docker hub

- login and push image to docker hub
- Login
  - `docker login` => provide credentials
- Push
  - `docker push kmahaley/coreapp:1.0`
  - goto Docker hub and verify image is present 

## Create Azure Container instance

- Login to Azure portal
- Create Azure container instance
 - Select subscription, resource group 
 	- Create random container name
	- Select region
	- select image type from `other` docker hub `public type`, OS type Linux
	- Networking tab, provide random DNS name
	- Keep rest default and create resource
- Once resource is created, goto resource and copy FQDN eg.`app.example.com`
- goto web browser and type `htpp://app.example.com\swagger\index.html`, `http://app.example.com\item`
 