# TaskManagerAPI

## Descrição
O **TaskManagerAPI** é uma API de TESTE  para gerenciamento de projetos e tarefas, desenvolvida em .NET 9 com suporte a Docker para facilitar a execução e o deploy. 
A API inclui funcionalidades como criação de projetos, gerenciamento de tarefas e documentação via Swagger.

---

## Executando o projeto no Docker

### Pré-requisitos
- Docker instalado ([Instruções de instalação](https://docs.docker.com/get-docker/))
- Docker Compose instalado ([Instruções de instalação](https://docs.docker.com/compose/install/))

### Passos para execução

1. Clone o repositório:     
      git clone https://github.com/lvmendes01/TaskManager.git
      cd TaskManager

2. Construa e inicie o container:
   docker-compose up --build

3. Acesse a API no navegador ou via ferramentas como Postman:
	https://localhost:7131/swagger

	---

## Comandos úteis

- **Parar os containers**: 
	- docker-compose down
- **Recompilar e reiniciar**:
	- docker-compose up --build
- **Acessar o container**:
	- docker exec -it taskmanagerapi /bin/bash


---

## Estrutura do Projeto

- **TaskManagerAPI**: API principal.
- **TaskManager.Tests**: Testes unitários para a API.
- **TaskManager.Models**: Camada de domínio com entidades e regras de negócio.
- **TaskManager.Data**: Implementação de persistência e acesso a dados pelo contexto do Entity Framework Core.

---

## Tecnologias Utilizadas

- .NET 9
- Docker
- Docker Compose
- Entity Framework Core
- Swagger para documentação da API
- Mysql como banco de dados

---

## Configuração do Docker

### Dockerfile
O `Dockerfile` está configurado para:
1. Restaurar dependências.
2. Compilar o projeto.
3. Publicar os arquivos necessários para execução.
4. Usar a imagem base do .NET Runtime para executar a aplicação.

### docker-compose.yml
O `docker-compose.yml` está configurado para:
- Construir a imagem do projeto.
- Mapear a porta `8080` do host para a porta `80` do container.
- Configurar a variável de ambiente `ASPNETCORE_ENVIRONMENT` como `Development`.

---

## Testando a API

1. Certifique-se de que o container está em execução:


---
---

## Configuração do Banco de Dados

O projeto utiliza MySQL como banco de dados. Certifique-se de configurar as seguintes variáveis de ambiente no `docker-compose.yml`:

- `MYSQL_ROOT_PASSWORD`: Senha do usuário root.
- `MYSQL_DATABASE`: Nome do banco de dados.
- `MYSQL_USER`: Nome do usuário.
- `MYSQL_PASSWORD`: Senha do usuário.

O banco de dados será inicializado automaticamente ao iniciar o container.

---
## Contribuindo

1. Faça um fork do repositório.

2. Crie uma branch para sua feature ou correção:
   git checkout -b minha-feature

3. Faça o commit das suas alterações:
   git commit -m "Minha nova feature"

4. Envie para o repositório remoto:
   git push origin minha-feature

5. Abra um Pull Request.

---
  



 --- sessão dedicada


 perguntaria para o PO visando o refinamento para futuras implementações ou melhorias?
 ## Perguntas para o PO
 - Quais são as prioridades para as próximas sprints?
 - Há alguma funcionalidade específica que você gostaria de ver implementada na API?
 - Existe algum feedback dos usuários que devemos considerar para melhorias?
 - Há planos para integrar a API com outras ferramentas ou serviços no futuro?

	
Na terceira fase, escreva no arquivo README.md em uma sessão dedicada o que você melhoraria no projeto, identificando possíveis pontos de melhoria, implementação de padrões, visão do projeto sobre arquitetura/cloud, etc.

- ## Melhorias no Projeto
- Implementação de Padrões
  - Adotar padrões de design como Repository e Unit of Work para melhor separação de preocupações.
  - Implementar injeção de dependência para facilitar testes e manutenção. *não implementado falta de tempo, mais algo que gosto bastante*
  - Utilizar DTOs (Data Transfer Objects) para transferir dados entre camadas, melhorando a segurança e a performance. 
  - Implementar autenticação e autorização usando JWT (JSON Web Tokens) para proteger endpoints sensíveis. *Teve um endpoint que foi solicitado apenas para o gerente, porem como nao foi solicitado autentificação, não foi possivel essa funcionalidade*