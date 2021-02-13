# Simple .Net Core Web API With JWT Auth

This is a simple Web API with JWT authentication build with .Net Core.

## Installation

Use the [dotnet ef](https://docs.microsoft.com/tr-tr/ef/core/cli/dotnet) to create/update databases.\
\
First of all, you need to change the directory to BaseApi.

```bash
cd BaseApi
```

For create Sqlite database

```bash
dotnet ef database update --context SqliteDatabaseContext
```

For the MSSQL server use DatabaseContext. Before that don't forget to add the connection string to the appsettings.json file.

```bash
dotnet ef database update --context DatabaseContext
```

## JWT Configuration
In the appsettings.json file

```javascript
"TokenOptions": {
    "Audience": "localhost:5001",
    "Issuer": "localhost:5001",
    "AccessTokenExpiration": 1,
    "RefreshTokenExpiration": 60,
    "SecurityKey": "ENTER_ANY_STRING_FOR_JWT_SECURITY"
  },
```
Change Audience and Issuer with your domain on IIS. \
AccessTokenExpiration how many days later Access Token will be invalid. \
RefreshTokenExpiration how many days later Refresh Token Token will be invalid \
SecurityKey any string to secure tokens. It will not be invisible don't forget that.

## Usage

```bash
dotnet run
```

## Rest API

### * Add a new user. You might call login after that.

#### Request

`POST /api/user`
```
curl --location --request POST 'https://localhost:5001/api/user' \
--header 'Content-Type: application/json' \
--data-raw '{
    "Email":"username@blabla.com",
    "PassWord":"secretpassword",
    "FullName":"UserName and Surname"
}'
```
#### Response
```bash
{
    "id": 1,
    "email": "username@blabla.com",
    "fullName": "UserName and Surname",
    "passWord": "2034F6E32958647FDFF75D265B455EBF",
    "signDate": "2021-02-13T15:03:17.518819",
    "refreshToken": null,
    "refreshTokenExpirationDate": "0001-01-01T00:00:00"
}
```

### * Login with Email and Password. Gets new access and refresh tokens.

#### Request
`POST /api/auth/login`

```bash
curl --location --request POST 'https://localhost:5001/api/auth/login' \
--header 'Content-Type: application/json' \
--data-raw '{
    "Email":"username@blabla.com",
    "PassWord":"secretpassword"
}'
```
#### Response
```bash
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJqdGkiOiJmZTRiOGYxMi0wMmYwLTQ4MWQtYjI0Zi0zMWIyYmMyOWQ2NzYiLCJuYmYiOjE2MTMyMTgxNTIsImV4cCI6MTYxMzMwNDU1MiwiaXNzIjoibG9jYWxob3N0OjUwMDEiLCJhdWQiOiJsb2NhbGhvc3Q6NTAwMSJ9.x_qyX4Vg0KLVSGzlc5RrkzY56-pV8AubsIyBpx2kLtY",
    "expration": "2021-02-14T15:09:12.544927+03:00",
    "refreshToken": "wTaeTY3neZj4LxYm5EdQtHqkzXnO5CnjeIR1NsOnyLQ="
}
```
### * Get the logged in user

#### Request

`GET /api/user`

```bash
curl --location --request GET 'https://localhost:5001/api/user' \
--header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJqdGkiOiJmZTRiOGYxMi0wMmYwLTQ4MWQtYjI0Zi0zMWIyYmMyOWQ2NzYiLCJuYmYiOjE2MTMyMTgxNTIsImV4cCI6MTYxMzMwNDU1MiwiaXNzIjoibG9jYWxob3N0OjUwMDEiLCJhdWQiOiJsb2NhbGhvc3Q6NTAwMSJ9.x_qyX4Vg0KLVSGzlc5RrkzY56-pV8AubsIyBpx2kLtY'
```

#### Response
```javascript
{
    "id": 1,
    "email": "username@blabla.com",
    "fullName": "UserName and Surname",
    "passWord": "2034F6E32958647FDFF75D265B455EBF",
    "signDate": "2021-02-13T15:03:17.518819",
    "refreshToken": "wTaeTY3neZj4LxYm5EdQtHqkzXnO5CnjeIR1NsOnyLQ=",
    "refreshTokenExpirationDate": "2021-04-14T15:09:12.556034"
}
```
### * Update the user which is logged in.

#### Request

`PUT /api/user`

```bash
curl --location --request PUT 'https://localhost:5001/api/user' \
--header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJqdGkiOiIxNzhhYzE0NC0xMmE4LTQ4MDQtODlhYi1hODc0MzE4MGJhYWEiLCJuYmYiOjE2MTMyMDc4OTMsImV4cCI6MTYxMzI5NDI5MywiaXNzIjoibG9jYWxob3N0OjUwMDEiLCJhdWQiOiJsb2NhbGhvc3Q6NTAwMSJ9.HmE2EabHmHW0qFytdMpDwqBzC3e5cxlRUa7LrPKEfN0' \
--header 'Content-Type: application/json' \
--data-raw '{
    "Email":"username@blabla.com",
    "PassWord":"secretnewpassword"
}'
```

#### Response
```javascript
{
    "id": 1,
    "email": "username@blabla.com",
    "fullName": "UserName and Surname",
    "passWord": "AFE5C0DE4BE50DB289424EB081CA892B",
    "signDate": "2021-02-13T15:03:17.518819",
    "refreshToken": "wTaeTY3neZj4LxYm5EdQtHqkzXnO5CnjeIR1NsOnyLQ=",
    "refreshTokenExpirationDate": "2021-04-14T15:09:12.556034"
}
```

### * Get new access and refresh tokens.

#### Request

`POST /api/auth/refreshtokens`

```bash
curl --location --request POST 'https://localhost:5001/api/auth/refreshtokens' \
--header 'Content-Type: application/json' \
--data-raw '"wTaeTY3neZj4LxYm5EdQtHqkzXnO5CnjeIR1NsOnyLQ="'
```

#### Response
```javascript
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJqdGkiOiJjYTA0MmU3Ny05YTY4LTRmN2QtODk5MS1iZDlmZjljNjY5ZmYiLCJuYmYiOjE2MTMyMTkxNzEsImV4cCI6MTYxMzMwNTU3MSwiaXNzIjoibG9jYWxob3N0OjUwMDEiLCJhdWQiOiJsb2NhbGhvc3Q6NTAwMSJ9.PR4ds1iFCOS9RixZQu_hOlsd1D1p12HiMOPjfrDfvhE",
    "expration": "2021-02-14T15:26:11.698506+03:00",
    "refreshToken": "taT3PcF3w0Lvhf1et+vtLqjr9G83P+npSwmwBQDlrZQ="
}
```
### * Delete the refresh token. You would use this for logout the user.

#### Request

`POST /api/auth/revokerefreshtoken`
```bash
curl --location --request POST 'https://localhost:5001/api/auth/revokerefreshtoken' \
--header 'Content-Type: application/json' \
--data-raw '"taT3PcF3w0Lvhf1et+vtLqjr9G83P+npSwmwBQDlrZQ="'
```
#### Response

```javascript
{
    "token": null,
    "expration": "0001-01-01T00:00:00",
    "refreshToken": null
}
```

### * Delete the user which is logged in.

#### Request

`DELETE /api/user`
```bash
curl --location --request DELETE 'https://localhost:5001/api/user' \
--header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJqdGkiOiJmZTRiOGYxMi0wMmYwLTQ4MWQtYjI0Zi0zMWIyYmMyOWQ2NzYiLCJuYmYiOjE2MTMyMTgxNTIsImV4cCI6MTYxMzMwNDU1MiwiaXNzIjoibG9jYWxob3N0OjUwMDEiLCJhdWQiOiJsb2NhbGhvc3Q6NTAwMSJ9.x_qyX4Vg0KLVSGzlc5RrkzY56-pV8AubsIyBpx2kLtY'
```
#### Response
```javascript
{
    "id": 1,
    "email": "username@another.com",
    "fullName": "UserName and Surname",
    "passWord": "AFE5C0DE4BE50DB289424EB081CA892B",
    "signDate": "2021-02-13T15:03:17.518819",
    "refreshToken": null,
    "refreshTokenExpirationDate": "0001-01-01T00:00:00"
}
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)