#!/bin/bash

echo Linux Docker build

cd ../DockerTest

dotnet publish -c Release -o ../publish

cd ../publish

echo publish success

docker build -t DockerTestDocker .