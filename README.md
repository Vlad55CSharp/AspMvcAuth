Пример использования ASP.NET Core Identity для аутентификации и авторизации пользователей в приложении ASP.NET Core MVC. 
Статьи, в которых рассматривается код проекта  
https://csharp.webdelphi.ru/autentifikaciya-i-avtorizaciya-v-asp-net-core-mvc-svoya-sistema-autentifikacii-polzovatelej/

https://csharp.webdelphi.ru/autentifikaciya-i-avtorizaciya-v-asp-net-core-mvc-upravlenie-rolyami-polzovatelej/

https://csharp.webdelphi.ru/autentifikaciya-i-avtorizaciya-v-asp-net-core-mvc-rabota-s-utverzhdeniyami-claims/

https://csharp.webdelphi.ru/autentifikaciya-i-avtorizaciya-v-asp-net-core-mvc-politiki-avtorizacii/

https://csharp.webdelphi.ru/autentifikaciya-i-avtorizaciya-v-asp-net-core-mvc-podtverzhdenie-adresa-elektronnoj-pochty/

Для того, чтобы запустить приложение, необходимо в файл appsettings.json или в secrets.json добавить следующие настройки:

{
  "EmailSender": {
    "Subject": "Email Confirmation",
    "From": "адрес_с_которого_отправляется_письмо",
    "Login": "логин_почты",
    "Password": "пароль_почты",
    "Host": "хост_почтового_сервера",
    "Port": порт (число),
    "EnableSSL": использовать или нет ssl (true/false)
  },
  "ConnectionStrings": {
    "DefaultConnection": "DataSource={файл_БД_SQLIte}"
  }
}
