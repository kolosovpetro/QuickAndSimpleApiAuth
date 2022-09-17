# Quick And Simple API Auth

[![Run Build and Test](https://github.com/kolosovpetro/QuickAndSimpleApiAuth/actions/workflows/run-build-and-test-dotnet.yml/badge.svg)](https://github.com/kolosovpetro/QuickAndSimpleApiAuth/actions/workflows/run-build-and-test-dotnet.yml)

Quick and simple role-based Azure Active Directory authentication and authorization using JWT tokens

## 1. Create app registration

- Navigate to Azure portal and create app registration: `QuickAndSimpleApiAuthApp` with `Single tenant`
- Record the data of newly `QuickAndSimpleApiAuthApp` created app registration:
    - Client ID: `6f33c1bb-4290-40ed-a026-8fb4bb8b326e`
    - Tenant ID: `b40a105f-0643-4922-8e60-10fc1abf9c4b`
- `Set application ID URI` under `Expose API` blade
- Create scope `QuickAndSimpleApiAuth.All` for `Admins and users` under `Expose API` blade
- Create roles under `App roles` blade:
    - `Manager`
    - `Admin`
    - `Reader`

PS: Allowed member types are `Users/Groups`

## 2. Create ASP NET Core Web API project

Use .NET 6.0 Target platform

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
    "ClientId": "6f33c1bb-4290-40ed-a026-8fb4bb8b326e",
    "Scopes": "QuickAndSimpleApiAuth.All"
  },
```

#### 2.5 Add required attributes

- Controller

```csharp
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
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

#### 3.5 Review the outputs

- Powershell
  ![Powershell_users](https://raw.githubusercontent.com/kolosovpetro/QuickAndSimpleApiAuth/master/img/01_users_created_powershell.PNG)
- Azure Portal
  ![Azure_portal_users](https://raw.githubusercontent.com/kolosovpetro/QuickAndSimpleApiAuth/master/img/02_ad_portal_users.PNG)

## 4. Assign roles to test users

- Go to `Azure Portal -> Enterprise Applications -> QuickAndSimpleApiAuth -> 1. Assign users and groups`
- Assign roles to the test users:
    - Manager: `AlexaWagner@kolosovp94gmail.onmicrosoft.com`
    - Admin: `RichardTrager@kolosovp94gmail.onmicrosoft.com`
    - Reader: `SabrinaBridges@kolosovp94gmail.onmicrosoft.com`
- Review created role assignments
  ![Role_assignments](https://raw.githubusercontent.com/kolosovpetro/QuickAndSimpleApiAuth/master/img/05_enterprise_app_user_roles_assigned.PNG)

## 5. Create app registration for Postman client

- Navigate to Azure portal and create app registration: `QuickAndSimpleApiAuthPostmanApp` with `Single tenant`
- Record the data of newly created `QuickAndSimpleApiAuthPostmanApp` app registration:
    - Client ID: `0efacdad-fe7d-48b3-9531-771e612d3b4e`
    - Tenant ID: `b40a105f-0643-4922-8e60-10fc1abf9c4b`
- In `Authentication` blade `Add a platform` with parameters
    - Type: `Web`
    - Redirect URI: `https://oauth.pstmn.io/v1/callback`
    - Access tokens (used for implicit flows): `Checked`
    - ID tokens (used for implicit and hybrid flows): `Checked`
    - Platform configuration screenshot:
      ![Platform_configuration](https://raw.githubusercontent.com/kolosovpetro/QuickAndSimpleApiAuth/master/img/06_configure_postman_app_platform.PNG)

## 6. Configure Postman request

- Request type: `GET`
- Request URL: `https://localhost:44335/WeatherForecast/GetWeatherManager`
- Headers:
    - Content-Type: `application/x-www-form-urlencoded`
- Authorization:
    - Type: `OAuth 2.0`
    - Add authorization data to: `Request Headers`
    - Token name: `QuickToken`
    - Grant type: `Authorization Code`
    - Callback URL: `https://oauth.pstmn.io/v1/callback`
    - Auth URL: `https://login.microsoftonline.com/b40a105f-0643-4922-8e60-10fc1abf9c4b/oauth2/v2.0/authorize`
    - Access Token URL: `https://login.microsoftonline.com/b40a105f-0643-4922-8e60-10fc1abf9c4b/oauth2/v2.0/token`
    - Client ID: `0efacdad-fe7d-48b3-9531-771e612d3b4e`
    - Client Secret: `Create you own in app regirstation -> Certificates and secrets`
    - Scope: `api://6f33c1bb-4290-40ed-a026-8fb4bb8b326e/QuickAndSimpleApiAuth.All`
    - Client Authentication: `Send as Basic Auth header`
- Postman config screenshot
  ![Postman_config](https://raw.githubusercontent.com/kolosovpetro/QuickAndSimpleApiAuth/master/img/07_postman_config.png)