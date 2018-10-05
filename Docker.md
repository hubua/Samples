# Docker (using PS)
* published DLLs should exist in the path specified as argument to COPY in `Dockerfile` (note that paths other then `obj/Docker/publish` are in `.dockerignore`);
* `cd` to location of `Dockerfile`;
* add image to local repository:
```
docker build -t docker-web-app:1.0 .
```

* ensure image added to Docker local repository:
```
docker images
```

* get information about image:
```
docker inspect {container_name_or_id}
```

* run image (ports defined as HOST:CONTAINER):
```
docker run -d -p 4000:4403 docker-web-app:1.0
```

* view running containers:
```
docker ps
```

* stop running container:
```
docker stop {container_name_or_id}
```

* clean up:
```
# delete containers
docker rm -f $(docker ps -a -q)
# delete images
docker rmi -f $(docker images -q)
```

* view logs
```
docker logs {container_id}
```

## Docker Compose
* Overview of Docker Compose https://docs.docker.com/compose/overview/
(Compose is a tool for defining and running multi-container Docker applications. With Compose, you use a YAML file to configure your application’s services. Then, with a single command, you create and start all the services from your configuration.)

* docker-compose creates a private network between services described in docker-compose.yml and assings hostnames equal to service names
```
docker-compose -f ".\docker-compose.nginx.yml" -p nginx_dwa_app up -d
# note: -p is optional
```

* Networking https://docs.docker.com/compose/networking/
* Deploying changes https://docs.docker.com/compose/production/
* Install compose https://docs.docker.com/compose/install/ (download with 'wget')


