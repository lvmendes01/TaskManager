# TaskManagerAPI

## Descri��o
O **TaskManagerAPI** � uma API de TESTE  para gerenciamento de projetos e tarefas, desenvolvida em .NET 9 com suporte a Docker para facilitar a execu��o e o deploy. 
A API inclui funcionalidades como cria��o de projetos, gerenciamento de tarefas e documenta��o via Swagger.

---

## Executando o projeto no Docker

### Pr�-requisitos
- Docker instalado ([Instru��es de instala��o](https://docs.docker.com/get-docker/))
- Docker Compose instalado ([Instru��es de instala��o](https://docs.docker.com/compose/install/))

### Passos para execu��o

1. Clone o reposit�rio:     
      git clone https://github.com/lvmendes01/TaskManager.git
      cd TaskManager

2. Construa e inicie o container:
   docker-compose up --build

3. Acesse a API no navegador ou via ferramentas como Postman:
	https://localhost:7131/swagger

	---

## Comandos �teis

- **Parar os containers**: 
	- docker-compose down
- **Recompilar e reiniciar**:
	- docker-compose up --build
- **Acessar o container**:
	- docker exec -it taskmanagerapi /bin/bash


---

## Estrutura do Projeto

- **TaskManagerAPI**: API principal.
- **TaskManager.Tests**: Testes unit�rios para a API.
- **TaskManager.Models**: Camada de dom�nio com entidades e regras de neg�cio.
- **TaskManager.Data**: Implementa��o de persist�ncia e acesso a dados pelo contexto do Entity Framework Core.

---

## Tecnologias Utilizadas

- .NET 9
- Docker
- Docker Compose
- Entity Framework Core
- Swagger para documenta��o da API
- Mysql como banco de dados

---

## Configura��o do Docker

### Dockerfile
O `Dockerfile` est� configurado para:
1. Restaurar depend�ncias.
2. Compilar o projeto.
3. Publicar os arquivos necess�rios para execu��o.
4. Usar a imagem base do .NET Runtime para executar a aplica��o.

### docker-compose.yml
O `docker-compose.yml` est� configurado para:
- Construir a imagem do projeto.
- Mapear a porta `8080` do host para a porta `80` do container.
- Configurar a vari�vel de ambiente `ASPNETCORE_ENVIRONMENT` como `Development`.

---

## Testando a API

1. Certifique-se de que o container est� em execu��o:


---
---

## Configura��o do Banco de Dados

O projeto utiliza MySQL como banco de dados. Certifique-se de configurar as seguintes vari�veis de ambiente no `docker-compose.yml`:

- `MYSQL_ROOT_PASSWORD`: Senha do usu�rio root.
- `MYSQL_DATABASE`: Nome do banco de dados.
- `MYSQL_USER`: Nome do usu�rio.
- `MYSQL_PASSWORD`: Senha do usu�rio.

O banco de dados ser� inicializado automaticamente ao iniciar o container.

---
## Contribuindo

1. Fa�a um fork do reposit�rio.

2. Crie uma branch para sua feature ou corre��o:
   git checkout -b minha-feature

3. Fa�a o commit das suas altera��es:
   git commit -m "Minha nova feature"

4. Envie para o reposit�rio remoto:
   git push origin minha-feature

5. Abra um Pull Request.

---
  



 --- sess�o dedicada


 perguntaria para o PO visando o refinamento para futuras implementa��es ou melhorias?
 ## Perguntas para o PO
 - Quais s�o as prioridades para as pr�ximas sprints?
 - H� alguma funcionalidade espec�fica que voc� gostaria de ver implementada na API?
 - Existe algum feedback dos usu�rios que devemos considerar para melhorias?
 - H� planos para integrar a API com outras ferramentas ou servi�os no futuro?

	
Na terceira fase, escreva no arquivo README.md em uma sess�o dedicada o que voc� melhoraria no projeto, identificando poss�veis pontos de melhoria, implementa��o de padr�es, vis�o do projeto sobre arquitetura/cloud, etc.

- ## Melhorias no Projeto
- Implementa��o de Padr�es
  - Adotar padr�es de design como Repository e Unit of Work para melhor separa��o de preocupa��es.
  - Implementar inje��o de depend�ncia para facilitar testes e manuten��o. *n�o implementado falta de tempo, mais algo que gosto bastante*
  - Utilizar DTOs (Data Transfer Objects) para transferir dados entre camadas, melhorando a seguran�a e a performance. 
  - Implementar autentica��o e autoriza��o usando JWT (JSON Web Tokens) para proteger endpoints sens�veis. *Teve um endpoint que foi solicitado apenas para o gerente, porem como nao foi solicitado autentifica��o, n�o foi possivel essa funcionalidade*