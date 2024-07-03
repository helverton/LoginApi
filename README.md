## LoginApi usando Identity

## Projeto criado usando VS 2022
- ASP.NET Core Web Api

## NuGet Package
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.VisualStudio.Web.CodeGeneration.Design
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Tools
- Swashbuckle.AspNetCore.Filters

## Projeto 
O objetivo deste projeto era criar uma API usando .NET Core 8 com camada de autorização OAuth2 e com autenticação
que poderia usar cookies ou bearer token.
Aproveitando o estudo com Identity e EF também criei um seed de roles e usuários.
Usei um pouco de scaffold para gerar a estrutura de consumo de api em um model e também engenharia 
reversa para importar uma tabela do banco de dados como um model para o projeto. 
Ao final configurei alguns endpoints para obrigar a autenticação para consumo e também
configurei o swagger para usar como documentação e ferramenta de teste.

## Conteúdo
- Visual Studio 2022 
- C# 
- API
- .NET Core 8
- Swagger
- OAuth2 

## Referência
- [https://www.youtube.com/watch?v=S0RSsHKiD6Y]
- [https://www.youtube.com/watch?v=8J3nuUegtL4]
