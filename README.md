# ChatbotAI
This is an example project for ChatbotAI.

## WEB API - Chatbot_AI - ASP.NET Core 8.0
create local databse:
```bash
sqllocaldb create "Local"
```
open SQL Server Management Studio and connect to SQL server (localdb)\Local using Windows Authentication

Then create new database "chatbot_ai"

in PM console run command:
```bash
dotnet ef database update
```
If you want to use your own database, please update ConnectionStrings in appsettings.Development.json appropriately.

run project in IIS Express and copy url.

## UI - Chatbot_AI_UI - Angular 16
run in terminal following commands:
```bash
npm install
```
In **app.module.ts** replace ChatbotAIApiBaseUrl value with copied url

Then start application:
```bash
ng serve
```
