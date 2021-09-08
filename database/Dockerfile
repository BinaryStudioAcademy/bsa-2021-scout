FROM mcr.microsoft.com/mssql/server:2017-latest
WORKDIR /usr/src/app

COPY . .

RUN sed -i 's/\r$//' import-data.sh
RUN sed -i 's/\r$//' entrypoint.sh
RUN sed -i 's/\r$//' secrets.csv
RUN sed -i 's/\r$//' setup.sql

RUN chmod +x import-data.sh

CMD /bin/bash ./entrypoint.sh