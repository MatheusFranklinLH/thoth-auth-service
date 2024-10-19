Bom dia chat: hoje iremos construir um serviço de autenticação, eu nomeei esse serviço de Thoth, em referência ao deus egipcio Thoth, esse serviço vai ser responsável por persistir no banco dados de usuários e suas organizações, roles e permissões, vai ser um serviço mult tenant em que a entidade do tenant vai ser a organização, então todo usuário terá uma organização, usuários e roles serao um relacionamento nxn e assim como roles e permissões também. Também vai ser responsável por gerar tokens jwt bearer através do Microsoft Identity, os tokens terão informações das Roles, permissões, email e organização do usuário, inclusive para toda chamada tokenizada só dar acesso a dados da organização daquele usuário.

Eu quero que o foco desse serviço seja segurança, então quero salvar senhas criptografas, etc  e quero que você sempre me dê sugestões do que é o melhor para a segurança do sistema aqui.

Nós iremos usar o dotnet core na versão 8.0.403, as bibliotecas Entity Framework para integração com um banco de dados PostgreSQL, Flunt para fazer todas as validações de dados e xUnit para construir nossos testes de unidades.

Quero também já pensar em logs para o meu serviço, acredito que de inicio a gente pode criar uma interface para colocar logs na aplicação, mesmo que por enquanto a classe que implementa interface só colocar um Console.WriteLine, só que pro futuro talvez eu colocar para salvar em banco de dados ou algo do tipo, usar um serviço separado talvez.

Eu quero usar Domain Driven Design, eu pensei em uma divisão em Domain, Infrastructure e API já que não vai ser um serviço tão grande.

Eu quero que você tenha tudo isso em mente, e que você me auxilie em cada passo, desde criação de diretórios, comandos dotnet de terminal, melhores templates para usar em cada parte do serviço.

E vamos começar devagar e por partes, vamos fazer o Domain e criar as entidades primeiro, beleza?