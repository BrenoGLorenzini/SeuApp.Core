Passo a passo para execução do app

Criar database mariaDB com nome "SeuAppDb" e alterar configurações de conexão nos arquivos:

  linha 37 do arquivo Program.cs
  appsettings.json
  linha 15 do arquivo DesignTimeDbContextFactory no Projeto SeuApp.Data

  obs: validar se a pasta "Migrations" está vazia, caso não esteja, apague o conteúdo da pasta.

  salve os arquivos
  
clique com o botão direito no Projeto "SeuApp.Data" e defina como projeto de inicialização.
clique em ferramentas bem ao topo da IDE, vá com o mouse para gerenciador de pacotes NuGet e clique em "Console do Gerenciador de Pacotes".
no topo a direita no console, selecione em Default project "SeuApp.Data" (caso a seleção não esteja em SeuApp.Data).

cole no powershell os comandos:

Add-Migration Initial

Update-Database

clique enter, note que irá criar um arquivo automatico dentro da pasta Migrations.
abra o banco de dados e verifique se foi criado as tabelas do database.

após isso, defina o projeto SeuApp.WinForms como projeto de inicialização.
clique em Compilação nas opções ao topo da tela, clique em limpar solução e após limpar (verificar no console) clique em REcompilar solução no mesmo caminho.
execute o app ou publique o executável em alguma pasta.
para publicar o executável, basta clicar com o botão direito do mouse no projeto SeuAPp.WinForms e ir em publicar.
