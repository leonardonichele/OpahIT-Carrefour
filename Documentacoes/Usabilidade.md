## Para executar os projetos por favor siga a ordem indicada:

1. docker compose up (cria a instancia do RabbitMQ)
2. inicie o projeto GFICash com dotnet run
3. inice o projeto GFIReport com dotnet run

Acesso via Swagger protegido por key, rotas sem autenticacao, mas para uma implementacao futura, sugiro uso de Bearer Token JWT por autenticacao de usuário.
http://localhost:5286/swagger/index.html?key=9a2d9e278ec677450ed6c818fa21eae42f75da556db53a46f89385ef449bc7aa
GFICash API

http://localhost:5196/swagger/index.html?key=9a2d9e278ec677450ed6c818fa21eae42f75da556db53a46f89385ef449bc7aa
GFIReport API

RabbitMQ
http://localhost:15672
(admin/admin)

## Obs de melhoria: Unificar em um docker compose os projetos.