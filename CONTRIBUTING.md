# Backend
## Before you begin

### Generate a development certificate

Run the following commands from the repository root:

```bash
mkdir -p ./src/backend/temp/https
dotnet dev-certs https -ep ./src/backend/temp/https/aspnetapp.pfx -p Qwerty123
dotnet dev-certs https --trust
```

### Create directory for postgres volume
```bash
mkdir -p ./src/backend/temp/pgdata  
```

## Keycloak
Для преднастройки аттрибутов профиля тестовых пользователей можно вписать просто у пользователя свойства в аттрибуты. После добавления непобходимых полей в настройки профилей в админке, значения станут отображаться у пользователей в админке и в ответе api

### Добавление аттрибутов профилю
Заходим в админку в целевой `realm`. Заходим в Realm settings -> User Profile -> JSON Editor. Добавляем в массив `attributes` следующие элементы

```json
{
    "name": "company_name",
    "displayName": "Company name",
    "permissions": { "view": ["admin","user"], "edit": ["admin","user"] },
    "annotations": { "inputType": "text" }
},
{
    "name": "company_logo",
    "displayName": "Company logo URL",
    "permissions": { "view": ["admin","user"], "edit": ["admin","user"] },
    "annotations": { "inputType": "url" },
    "validations": {
    "pattern": { "pattern": "https?://.+", "error-message": "Must be a valid http(s) URL" }
    }
},
{
    "name": "avatar",
    "displayName": "Avatar URL",
    "permissions": { "view": ["admin","user"], "edit": ["admin","user"] },
    "annotations": { "inputType": "url" },
    "validations": {
    "pattern": { "pattern": "https?://.+", "error-message": "Must be a valid http(s) URL" }
    }
}
```

Нажимаем `Save`