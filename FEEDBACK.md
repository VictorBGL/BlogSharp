# Feedback do Instrutor

#### 28/10/24 - Revisão Inicial - Eduardo Pires

## Pontos Positivos:

- Boa separação de responsabilidades.
- Arquitetura enxuta de acordo com a complexidade do projeto
- Demonstrou conhecimento em Identity e JWT
- Soube aplicar a validação de claims ou proprietario do post
- Mostrou entendimento do ecossistema de desenvolvimento em .NET

## Pontos Negativos:

- Nem todas funcionalidades (CRUD Posts e Commentários) estão implementadas
- A camada data faz muito mais coisas, poderia ser chamada de Core.
- A entidade autor é representada pela usuário, nomenclatura diferente do proposto.
- A validação de admin está sendo aplicada em momentos onde um user comum (proprietário do post) poderia ter acesso, não é o uso correto
- Existem maneiras mais elegantes de obter o usuário do identity e utilizar seus dados (ex id ou email).

## Sugestões:

- Evoluir o projeto para as necessidades solicitadas no escopo.

## Problemas:

- Não consegui executar todas as ações do projeto, pois nem tudo está implementado, mas a aplicação rodou.
