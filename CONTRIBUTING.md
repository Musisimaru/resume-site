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
```
mkdir -p ./src/backend/temp/pgdata  
```