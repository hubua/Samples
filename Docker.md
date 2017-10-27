# PowerShell
* published DLLs should exist in the path specified as argument to COPY in `Dockerfile`;
** note that paths other then `obj/Docker/publish` are in `.dockerignore`;
* cd to location of `Dockerfile`;
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