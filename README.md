# Quick And Simple Api Auth

[![Run Build and Test](https://github.com/kolosovpetro/QuickAndSimpleApiAuth/actions/workflows/run-build-and-test-dotnet.yml/badge.svg)](https://github.com/kolosovpetro/QuickAndSimpleApiAuth/actions/workflows/run-build-and-test-dotnet.yml)

Quick and simple Azure Active Directory authentication and authorization

## Create app registration

- Navigate to Azure portal and create app registration: `QuickAndSimpleApiAuth`
- Under `Expose API` blade: `Set application ID URI`
- Under `Expose API` blade: Create scope `QuickAndSimpleApiAuth.All` for `Admins and users`
- Create specified platform at `Authentication -> Add Platform` with parameters:
    - Type: `Web`
    - Redirect URIs: `https://localhost:7221/signin-oidc`
    - Front-channel logout URL: `https://localhost:7221/signout-oidc`
    - ID Tokens: `Checked`
    - Access Tokens: `Checked`

## Create roles

- Manager for users and groups

## Add Nuget Packages

- Update nuget packages:
    - `Microsoft.AspNetCore.Authentication.JwtBearer`
    - `Microsoft.AspNetCore.Authentication.OpenIdConnect`
    - `Microsoft.Identity.Web`
    - `Microsoft.Identity.Web.UI`

## Add section to appsettings json

```bash
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "kolosovp94gmail.onmicrosoft.com",
    "TenantId": "b40a105f-0643-4922-8e60-10fc1abf9c4b",
    "ClientId": "8753c306-bd03-45be-be09-32a2218eb100",
    "Scopes": "QuickAndSimpleApiAuth.All",
    "CallbackPath": "/signin-oidc",
    "SignedOutCallbackPath": "/signout-callback-oidc"
  },
```

## Add required services

```csharp
var configurationSection = builder.Configuration.GetSection("AzureAd");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(configurationSection);
```

## Add auth middleware

```csharp
app.UseAuthentication();
```

## Add controller attributes

```csharp
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[Authorize]
[ApiController]
[Route("[controller]")]
```

## Configure Postman request to get token

- Request type: `POST`
- Request URL: `https://login.microsoftonline.com/b40a105f-0643-4922-8e60-10fc1abf9c4b/oauth2/v2.0/token`
- Header value: `Content-Type: application/x-www-form-urlencoded`
- Body with form data:
    - `grant_type`: `client_credentials`
    - `client_id`: `8753c306-bd03-45be-be09-32a2218eb100`
    - `client_secret`: `Generate in azure portal and keep`
    - `scope`: `api://8753c306-bd03-45be-be09-32a2218eb100/QuickAndSimpleApiAuth.All`