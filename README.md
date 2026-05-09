# WebApplication1 - ASP.NET Core MVC

Projeto MVC com cadastro de Usuários e Empresas, usando Entity Framework Core + SQL Server.

## Estrutura

```
WebApplication1/
├── Controllers/
│   ├── HomeController.cs
│   ├── UsuariosController.cs
│   └── EmpresasController.cs
├── Models/
│   ├── AppDbContext.cs
│   ├── Usuario.cs
│   ├── Empresa.cs
│   └── ErrorViewModel.cs
├── Views/
│   ├── Home/Index.cshtml
│   ├── Usuarios/Create.cshtml, Index.cshtml
│   ├── Empresas/Create.cshtml, Index.cshtml
│   └── Shared/_Layout.cshtml
├── Migrations/
│   └── CreateDatabase.sql
├── appsettings.json
├── Program.cs
└── WebApplication1.csproj
```

## Como rodar

### 1. Banco de dados

**Opção A — via script SQL:**
Abra o SQL Server Management Studio e execute o arquivo:
```
Migrations/CreateDatabase.sql
```

**Opção B — via migrations do EF Core (no Package Manager Console):**
```
Add-Migration InitialCreate
Update-Database
```

### 2. Connection String

Edite o `appsettings.json` com os dados do seu SQL Server:
```json
"defaultConnection": "Server=SEU_SERVIDOR;Database=WebApplication1Db;Trusted_Connection=True;"
```

### 3. Executar

Pressione **F5** no Visual Studio ou rode:
```
dotnet run
```

## Fluxo de navegação

- `/` → Tela inicial com escolha de cadastro
- `/Usuarios/Create` → Cadastro de usuário
- `/Empresas/Create` → Cadastro de empresa
- `/Usuarios` → Lista de usuários
- `/Empresas` → Lista de empresas
