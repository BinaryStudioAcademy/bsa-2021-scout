/opt/mssql-tools/bin/sqlcmd -S ats_database -U sa -P MyBadPw123! -d master -i setup.sql

apt-get update && apt-get install -y software-properties-common curl gnupg2 && \
	curl -fsSL https://apt.releases.hashicorp.com/gpg | apt-key add - && \
	apt-add-repository "deb [arch=amd64] https://apt.releases.hashicorp.com $(lsb_release -cs) main" && \
	apt-get update && apt-get install -y \
	vault && \
	setcap cap_ipc_lock= /usr/bin/vault

vault secrets enable -path='ats-api/database' database

vault write ats-api/database/config/ats-vault-db \
	 	plugin_name=mssql-database-plugin \
	 	connection_url='sqlserver://{{username}}:{{password}}@ats_database:1433' \
	 	allowed_roles="ats-api-role" \
	 	username="sa" \
	 	password="MyBadPw123!"

vault write ats-api/database/roles/ats-api-role \
    db_name=ats-vault-db \
    creation_statements="CREATE LOGIN [{{name}}] WITH PASSWORD = '{{password}}';\
				USE HashiCorp;\
				CREATE USER [{{name}}] FOR LOGIN [{{name}}];\
        GRANT SELECT,UPDATE,INSERT,DELETE TO [{{name}}];" \
    default_ttl="2m" \
    max_ttl="43800h"
