
docker rm sitepeinturebuild
docker rmi sitepeinturebuild

docker build -t sitepeinturebuild -f Dockerfile.build .
docker create --name sitepeinturebuild sitepeinturebuild

rm -Rf ./bin/Release/PublishOutput
docker cp sitepeinturebuild:/out ./bin/Release/PublishOutput

docker stop sitepeinture
docker rm sitepeinture
docker rmi sitepeinture

docker build -t sitepeinture .
docker run  --restart=always -e "ASPNETCORE_ENVIRONMENT=Production" --name=sitepeinture -d -p 6000:80 sitepeinture