# 📝 TODO API

REST API for managing user tasks with access & refresh tokens.  
Refresh tokens are stored in httpOnly cookies.

---

## Features
- User registration, login, logout
- Access & refresh token authentication
- CRUD tasks
- Update username/password
- Delete user

---

## Authentication
- Access token: short-lived, send in header `Authorization: Bearer <token>`
- Refresh token: long-lived, in httpOnly cookie, used at `POST /api/auth/refresh`

---

## Endpoints
- GET    /api/users – get user info and tasks
- PATCH  /api/auth/update/username – update username
- PATCH  /api/auth/update/password – update password
- DELETE /api/users – delete user
- GET    /api/tasks – get user tasks
- POST   /api/tasks – create task
- PATCH  /api/tasks/{id} – update task
- DELETE /api/tasks/{id} – delete task
- POST   /api/auth/register – register
- POST   /api/auth/login – login, get tokens
- POST   /api/auth/logout – logout
- POST   /api/auth/refresh – refresh access token

---

## Example Request
GET /api/tasks  
Authorization: Bearer <access_token>

### Example Response
[
  { "id": 1, "title": "Buy milk", "completed": false },
  { "id": 2, "title": "Read book", "completed": true }
]

---

##DTO
all the DTOs you can find in the /ServerSol/Server/DTO folder

>TokenResposeDto use only for internal operations and not allow any andpoints

---

> Note: All protected endpoints require a valid access token. Refresh token used only via /auth/refresh.

---

##Deploy
You can deploy project on VPS/VDS or your local machine, using _docker-compose.yml_ in /ServerSol. Configurator environment variables for your DB and ports in docker compose file.

---

##DB
I use PostgreSQL in my case, but you can change db editing _appsettings.json_, download .net packages for your DB and change env. Also you need to change volume path in _docker-compose.yml_.

