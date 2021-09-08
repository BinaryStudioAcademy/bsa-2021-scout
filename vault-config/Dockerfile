FROM mcr.microsoft.com/mssql/server:2017-latest AS config
WORKDIR /config

COPY . .

RUN sed -i 's/\r$//' config.sh
RUN sed -i 's/\r$//' secrets.csv
RUN sed -i 's/\r$//' setup.sql

CMD bash config.sh