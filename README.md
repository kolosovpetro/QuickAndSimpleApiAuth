# Quick And Simple API Auth

[![Run Build and Test](https://github.com/kolosovpetro/QuickAndSimpleApiAuth/actions/workflows/run-build-and-test-dotnet.yml/badge.svg)](https://github.com/kolosovpetro/QuickAndSimpleApiAuth/actions/workflows/run-build-and-test-dotnet.yml)

Quick and simple role-based Azure Active Directory authentication and authorization using JWT tokens

## 1. Create app registration

- Navigate to Azure portal and create app registration: `QuickAndSimpleApiAuth`
- Record the data of newly created app registration:
    - Client ID: `8753c306-bd03-45be-be09-32a2218eb100`
    - Tenant ID: `b40a105f-0643-4922-8e60-10fc1abf9c4b`
- `Set application ID URI` under `Expose API` blade
- Create scope `QuickAndSimpleApiAuth.All` for `Admins and users` under `Expose API` blade
- Create app role under `App roles` blade:
    - `Manager`

## 2. Create ASP NET Core Web API project

#### 2.1 Add Nuget Packages

- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Microsoft.AspNetCore.Authentication.OpenIdConnect`
- `Microsoft.Identity.Web`
- `Microsoft.Identity.Web.UI`

#### 2.2 Update service collection

```csharp
var configurationSection = builder.Configuration.GetSection("AzureAd");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(configurationSection);
```

#### 2.3 Use Authentication middleware

```csharp
app.UseAuthentication();
```

#### 2.4 Add section to appsettings json

Where Client ID, Tenant ID and Scopes are from **Step 1**

```bash
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "TenantId": "b40a105f-0643-4922-8e60-10fc1abf9c4b",
    "ClientId": "8753c306-bd03-45be-be09-32a2218eb100",
    "Scopes": "QuickAndSimpleApiAuth.All"
  },
```

#### 2.5 Add required attributes

- Controller

```csharp
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
[Authorize]
[ApiController]
[Route("[controller]")]
```

- HTTP action

```csharp
[Authorize(Roles = "Manager")]
[HttpGet("GetWeatherManager")]
```

## 3. Create test users via Az Powershell

#### 3.1 Prerequisites

- Update Windows Powershell as Administrator using: `Install-Module PSWindowsUpdate`
- Install [Azure PowerShell](https://docs.microsoft.com/en-us/powershell/azure/install-az-ps)
- Install as Powershell Administrator: `Install-Module AzureAD`

#### 3.2 Connect to Azure Active Directory

- Connect to AD: `Connect-AzureAD -TenantId "b40a105f-0643-4922-8e60-10fc1abf9c4b"`
- Define domain variable: `$aadDomainName = ((Get-AzureAdTenantDetail).VerifiedDomains)[0].Name`

#### 3.2 Manager Alexa Wagner

- Create password profile:
    - `$passwordProfile = New-Object -TypeName Microsoft.Open.AzureAD.Model.PasswordProfile`
    - `$passwordProfile.Password = $env:AD_TEST_USER_PASSWORD`
    - `$passwordProfile.ForceChangePasswordNextLogin = $false`
- Define username variable: `$userName="AlexaWagner"`
- Create Alexa Wagner user:
  `New-AzureADUser -AccountEnabled $true -DisplayName $userName -PasswordProfile $passwordProfile -MailNickName $userName -UserPrincipalName "$userName@$aadDomainName"`
- Print new user principal name: `(Get-AzureADUser -Filter "MailNickName eq '$userName'").UserPrincipalName`
- User Principal Name (UPN): `AlexaWagner@kolosovp94gmail.onmicrosoft.com`

#### 3.3 Admin Richard Trager

- Create password profile:
    - `$passwordProfile = New-Object -TypeName Microsoft.Open.AzureAD.Model.PasswordProfile`
    - `$passwordProfile.Password = $env:AD_TEST_USER_PASSWORD`
    - `$passwordProfile.ForceChangePasswordNextLogin = $false`
- Define username variable: `$userName="RichardTrager"`
- Create Richard Trager user:
  `New-AzureADUser -AccountEnabled $true -DisplayName $userName -PasswordProfile $passwordProfile -MailNickName $userName -UserPrincipalName "$userName@$aadDomainName"`
- Print new user principal name: `(Get-AzureADUser -Filter "MailNickName eq '$userName'").UserPrincipalName`
- User Principal Name (UPN): `RichardTrager@kolosovp94gmail.onmicrosoft.com`

#### 3.4 Reader Sabrina Bridges

- Create password profile:
    - `$passwordProfile = New-Object -TypeName Microsoft.Open.AzureAD.Model.PasswordProfile`
    - `$passwordProfile.Password = $env:AD_TEST_USER_PASSWORD`
    - `$passwordProfile.ForceChangePasswordNextLogin = $false`
- Define username variable: `$userName="SabrinaBridges"`
- Create Sabrina Bridges user:
  `New-AzureADUser -AccountEnabled $true -DisplayName $userName -PasswordProfile $passwordProfile -MailNickName $userName -UserPrincipalName "$userName@$aadDomainName"`
- Print new user principal name: `(Get-AzureADUser -Filter "MailNickName eq '$userName'").UserPrincipalName`
- User Principal Name (UPN): `SabrinaBridges@kolosovp94gmail.onmicrosoft.com`

## Configure Postman request to get token

- Request type: `POST`
- Request URL: `https://login.microsoftonline.com/b40a105f-0643-4922-8e60-10fc1abf9c4b/oauth2/v2.0/token`
- Header value: `Content-Type: application/x-www-form-urlencoded`
- Body with form data:
    - `grant_type`: `client_credentials`
    - `client_id`: `8753c306-bd03-45be-be09-32a2218eb100`
    - `client_secret`: `Generate in azure portal and keep`
    - `scope`: `api://8753c306-bd03-45be-be09-32a2218eb100/QuickAndSimpleApiAuth.All`