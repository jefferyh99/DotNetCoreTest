Dockerfile Image资源可以通过查找https://hub.docker.com/找到
发布：dotnet publish -c Release -o ../publish
构建：docker build -t dockertestdocker .
后台运行：docker run --name=dockertest1 -p 7777:80 -d  dockertestdocker
立即运行（可以观察是否有报错信息）：docker run --name=dockertest1 -p 7777:80  dockertestdocker
查看启动日志：docker logs dockertestdocker
验证访问：curl http：//localhost:7777

常用docker命令
docker images
docker ps
docker ps -a

官方命令：
https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-run?tabs=netcore21

dotnet run //直接运行.csproj中的项目，不是编译完的话，推荐这种方式。
dotnet run --urls="http://localhost:10000" //直接运行.csproj中的项目
dotnet a.dll --urls="http://localhost:10000"//运行编译后的dll，如果没有编译的话，可能一些配置项不会进去。