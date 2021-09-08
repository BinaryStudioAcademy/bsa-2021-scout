sleep 15s

/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P MyBadPw123! -d master -i setup.sql

/opt/mssql-tools/bin/bcp Secrets in /usr/src/app/secrets.csv -S localhost -U sa -P MyBadPw123! -d HashiCorp -c -t ','