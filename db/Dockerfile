FROM mysql:5.7.22

# Copie o arquivo migrate.sh do host para o contêiner
COPY db/migrate.sh /docker-entrypoint-initdb.d/migrate.sh

# Define as permissões do arquivo migrate.sh para torná-lo executável
RUN chmod +x /docker-entrypoint-initdb.d/migrate.sh

# Expõe a porta 3306 do contêiner
EXPOSE 3306