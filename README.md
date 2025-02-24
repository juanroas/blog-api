# 📌 Blog API - ASP.NET Core 🚀

Um **sistema de blog monolítico**, desenvolvido em **C# com ASP.NET Core**, que permite aos usuários autenticados **criar, editar e excluir postagens**.  
Qualquer visitante pode visualizar as postagens existentes.  
O sistema também possui **notificações em tempo real via WebSockets** sempre que uma nova postagem é criada.

---

## 🛠️ Tecnologias Utilizadas  

✔ **C# com ASP.NET Core**  
✔ **Entity Framework Core (MySQL)** para persistência de dados  
✔ **Autenticação com Identity + JWT**  
✔ **WebSockets para notificações em tempo real**  
✔ **Swagger** para documentação da API  

---

## 📂 Arquitetura do Projeto  

O projeto segue uma **arquitetura em camadas**, aplicando **SOLID**, especialmente **SRP (Single Responsibility Principle)** e **DIP (Dependency Inversion Principle)**.  

- **📌 Controllers** → Controlam as requisições HTTP.  
- **📌 Application (Services e DTOs)** → Contém regras de negócio e evita lógica nos controllers.  
- **📌 Domain (Models)** → Representa as entidades do sistema.  
- **📌 Infrastructure (Data e Services)** → Contém acesso ao banco de dados e serviços externos (WebSockets).
```
- /Blog
 ├── /Controllers
 │   ├── AuthController.cs
 │   ├── PostController.cs
 │
 ├── /Application
 │   ├── /Services
 │       ├── Interfaces
 │       │   ├── IAuthService.cs
 │       │   ├── IPostService.cs
 │       ├── Implementations
 │       │   ├── AuthService.cs
 │       │   ├── PostService.cs
 │   ├── /DTOs
 │       ├── Auth
 │       │   ├── UserRegisterDto.cs
 │       │   ├── UserLoginDto.cs
 │       ├── Post
 │       │   ├── PostCreateDto.cs
 │       │   ├── PostUpdateDto.cs
 │
 ├── /Domain
 │   ├── /Models
 │       ├── Post.cs
 │       ├── User.cs
 │       
 ├── /Infrastructure
 │   ├── /Data
 │       ├── MySqlBlogContext.cs
 │   ├── /Services
 │       ├── NotificationService.cs
 │
 ├── Program.cs
```

---

## 🚀 Como Executar o Projeto  

### **1️⃣ Clone o Repositório**
```bash
git clone https://github.com/juanroas/blog-api.git
cd blog-api
```

### **2️⃣ Configure a String de Conexão**
Edite o arquivo appsettings.json com suas credenciais do MySQL:
Exemplo: 
```
"ConnectionStrings": {
    "MySQLConnectionString": "Server=localhost;Database=blog_db;User=root;Password=admin123;"
}
```
### **3️⃣ Instale as Dependências**
```
dotnet restore
```

### **4️⃣ Execute as Migrations**
```
dotnet ef database update
```
### **5️⃣ Rode a API**
```
dotnet run
```
## **Para acessar a documentação Swagger:**
```
http://localhost:5000/swagger
```

---


## 📌 Endpoints Principais

### **🔹Autenticação**

✔ **POST /api/auth/register → Registrar usuário**

✔ **POST /api/auth/login → Fazer login e obter um token JWT**

### **🔹 Postagens**

✔ **GET /api/posts → Listar postagens**

✔ **POST /api/posts → Criar uma postagem (Autenticado)**

✔ **PUT /api/posts/{id} → Editar uma postagem (Somente dono)**

✔ **DELETE /api/posts/{id} → Excluir uma postagem (Somente dono)**

### **🔹 Notificações em Tempo Real**
Conecte-se via WebSockets para receber notificações quando um post for criado:
```
ws://localhost:5000/ws
```

---

## 🛡️ Segurança
🔐 **JWT (JSON Web Token) é utilizado para autenticação.**

🔐 **As senhas são armazenadas com ASP.NET Identity e hashing seguro.**

🔐 **Somente o autor do post pode editar ou excluir.**

---

## 📜 Licença
Este projeto é de código aberto e pode ser modificado conforme necessário.

📌 Autor: Juan Roas

🚀 Contribuições são bem-vindas!
