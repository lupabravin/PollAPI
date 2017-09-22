# PollAPI - README 
---
## Restaurando Banco de Dados  SQL Server 2014

- Realizar o download e instalar o [Microsoft SQL Server 2014 Express](https://www.microsoft.com/pt-br/download/details.aspx?id=42299)
- Abrir o **SQL Server Management Studio**
- Definir usu�rio e senha e selecionar a op��o de acesso do SQL Server (ao inv�s de acesso do Windows)
- No painel **Pesquisador de Objetos**, � esquerda, clicar com o bot�o direito em **Banco de Dados**
- Selecionar a op��o **Restaurar Banco de Dados**
- No painel **Origem**, selecionar o *RadioButton* **Dispositivo** e localizar o arquivo de backup **PollDB.bak** enviado junto ao projeto
- Aguardar a restaura��o da base de dados
---
## Configurando ConnectionString no projeto
- Abrir a solution PollAPI.sln utilizando o [Vistual Studio 2017 Community](https://www.visualstudio.com/pt-br/thank-you-downloading-visual-studio/?sku=Community&rel=15)
- Localizar e abrir o arquivo **Web.config** na pasta raiz do projeto por meio do *Solution Explorer* 
- Procurar a regi�o definida para a string de conex�o na linha 71, que estar� desta forma:

```xml
<connectionStrings>
    <add name="PollDBModel" connectionString="metadata=res://*/Models.PollDBModels.csdl|res://*/Models.PollDBModels.ssdl|res://*/Models.PollDBModels.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=TRAIRA\SQLEXPRESS;initial catalog=PollDB;user id=sa;password=1800dz10;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
</connectionStrings>
```

- Alterar as seguintes propriedades:
    - Propriedade *data source* com o endere�o do servidor ou inst�ncia SQL Server em que a API ser� hospedada;
    - Propriedades *user id* e *password* com o usu�rio e senha cadastrados no SQL Server;

---
## Executando a API no Visual Studio
- Com o projeto ainda aberto no **Visual Studio 2017 Community**, clicar no menu superior **Build** e selecionar a op��o **Build Solution**
- Selecionar configura��o desejada: **Debug** ou **Release**
 

----
#####  As requisi��es utilizadas para a realiza��o dos testes via Postman se encontram neste [link](https://documenter.getpostman.com/view/1436800/poll-api-requests/6tgWNAa) 


