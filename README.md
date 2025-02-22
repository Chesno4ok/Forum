
# ChesnokForum

Это пет проект по созданию **fullstack приложения** "форум". В основе которого лежит **ASP.NET Core MVC** и **Blazor**, всё связано между собой с помощью **Aspire**.

## Стек технологий

### Backend:
- ![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-5C2D91?style=flat&logo=.net&logoColor=white) - Серверная часть приложения
- ![Redis](https://img.shields.io/badge/Redis-D12E27?style=flat&logo=redis&logoColor=white) - Кеширование
- ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?style=flat&logo=postgresql&logoColor=white) - Система управления базами данных

### Frontend:
- ![Blazor](https://img.shields.io/badge/Blazor-5A2A99?style=flat&logo=blazor&logoColor=white) - Фреймворк для разработки UI
- ![MudBlazor](https://img.shields.io/badge/MudBlazor-0078D4?style=flat&logo=blazor&logoColor=white) - UI Библиотека

## Как запустить

1. Перейдите в директорию:
   ```bash
   \ChesnokForum\AspireForum\AspireForum.AppHost
  

2. Запустите приложение:
   ```bash
   dotnet run
   

3. Перейдите в панель управления и наслаждайтесь проектом!

## Известные проблемы

- **Проблема:** Иногда backend встает на не тот порт, который указан на фронте.
  **Решение:** Попробуйте перезапустить. Если не получается, перейдите в:
  ```plaintext
  ChesnokForum\Forum.Frontend\Forum.Frontend\appsettings.json
  ```
  и укажите правильный путь к backend.

- **Проблема:** Иногда `MigrationService` запускается до того, как стартует контейнер с базой данных.
  **Решение:** Подождите, пока запустится контейнер, и перезапустите `MigrationService`.

---

## Ссылки на технологии:

- [ASP.NET Core](https://dotnet.microsoft.com/en-us/apps/aspnet)
- [Redis](https://redis.io/)
- [PostgreSQL](https://www.postgresql.org/)
- [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- [MudBlazor](https://mudblazor.com/)



Приятной работы! 🚀
