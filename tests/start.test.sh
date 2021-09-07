#!/bin/bash

mkdir .env

echo "ASPNETCORE_ENVIRONMENT=Development
DATABASE_CONNECTION_STRING='Server=ats_database;Database=ATS_dev;User Id=sa;Password=MyBadPw123!;'
ELASTIC_CONNECTION_STRING='http://ats_elastic_server:9200'
MONGODB_CONNECTION_URI='mongodb://mongo:MyBadPass123!@ats_mongodb:27017'
MONGODB_DATABASE_NAME='ats_database'
SECRET_JWT_KEY='DD70E219DCF6408A7506EA0186D183AE'
FRONTEND_URL=''
MAIL_ADDRESS='fake'
MAIL_PASSWORD='fake'
MAIL_DISPLAY_NAME='Test'" > .env/ats_api.env

echo "SA_PASSWORD=MyBadPw123!
ACCEPT_EULA=Y" > .env/ats_database.env

echo "MONGO_INITDB_ROOT_USERNAME=mongo
MONGO_INITDB_ROOT_PASSWORD='MyBadPass123!'
MONGO_INITDB_DATABASE=ats_database" > .env/ats_mongodb.env

docker build -t ats_api -f backend/src/Dockerfile ./backend/src

docker-compose -f .docker/docker-compose.test.yml up -d

bash ./tests/scripts/wait.sh

sleep 15

cd tests && npm install && npm run test:api
