# PollAPI - README 

## Restaurando Banco de Dados  SQL Server 2014

- Realizar o download e instalar o [Microsoft SQL Server 2014 Express](https://www.microsoft.com/pt-br/download/details.aspx?id=42299)
- Abrir o **SQL Server Management Studio**
- Definir usuário e senha e selecionar a opção de acesso do SQL Server (ao invés de acesso do Windows)
- No painel **Pesquisador de Objetos**, à esquerda, clicar com o botão direito em **Banco de Dados**
- Selecionar a opção **Restaurar Banco de Dados**
- No painel **Origem**, selecionar o *RadioButton* **Dispositivo** e localizar o arquivo de backup **PollDB.bak** que se encontra junto ao projeto
- Aguardar a restauração da base de dados

## Configurando ConnectionString no projeto
- Abrir a solution PollAPI.sln utilizando o [Vistual Studio 2017 Community](https://www.visualstudio.com/pt-br/thank-you-downloading-visual-studio/?sku=Community&rel=15)
- Localizar e abrir o arquivo **Web.config** na pasta raiz do projeto por meio do *Solution Explorer* 
- Procurar a região definida para a string de conexão na linha 72, que estará desta forma:

```xml
<connectionStrings>
    <add name="PollDB" connectionString="metadata=res://*/PollDB.csdl|res://*/PollDB.ssdl|res://*/PollDB.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=localhost;initial catalog=PollDB;persist security info=True;user id=sa;password=1800Dz10;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
</connectionStrings>
```

- Alterar as seguintes propriedades:
    - Propriedade *data source* com o endereço do servidor ou instância SQL Server em que a API será hospedada;
    - Propriedades *user id* e *password* com o usuário e senha cadastrados no SQL Server;


## Executando a API no Visual Studio
- Com o projeto ainda aberto no **Visual Studio 2017 Community**, clicar no menu superior **Build** e selecionar a opção **Build Solution**
- Selecionar configuração desejada: **Debug** ou **Release**
 

----
#####  As requisições utilizadas para a realização dos testes via Postman se encontram neste [link](https://documenter.getpostman.com/view/1436800/poll-api-requests/6tgWNAa) 
