# Site of Refuge API

This repo hosts the core API for Site of Refuge. You can access the [API documentation here](https://siteofrefuge.github.io/sor-api).

API documentation is based on OpenAPI 3.0. Raw swagger yaml can be [seen here](https://github.com/SiteOfRefuge/sor-api/blob/main/docs/swagger.yaml).

## Environment setup
- You should install the [Azure Function Core
  Tools](https://github.com/Azure/azure-functions-core-tools#installing) if you
  wish to run and debug these functions locally.

- Install the [dotnet 6.0
  SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

- Run the functions locally with

  ```sh
  func start --csharp

   ```

When running locally you will need to review the `local.settings.sample.json` for the structure of your own `local.settings.json`. For debugging locally on the staging instance, you should use:
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "OpenApi_HideDocument": "true",
    "AuthenticationAuthority": "https://siteofrefugeb2c.b2clogin.com/siteofrefugeb2c.onmicrosoft.com/b2c_1_sms_registry/v2.0",
    "AuthenticationClientId": "30222d8b-d3d1-4f62-9a2c-8161c2252e5b"
  }
}
```

## Setting up Postman
When working with the API there is a good chance you will want to work directly with the endpoints rather than route through the frontend. A great tool for this is [Postman](https://www.postman.com) which you can [download here](https://www.postman.com/downloads/).

There is a bit of tricky configuration you will need to setup to have Postman get new access tokens for you. Below are steps you can follow to make it easier on yourself. 

1. Create a new collection if you haven't already. I called mine ** Site of Refuge API**
2. Click on the collection and go to **Variables**
3. Create the following variables:
   1. API_URL: https://siteofrefuge-api-staging.azurewebsites.net/v1 (or use http://localhost:7071/v1 when working locally)
   2. AUTH_URL: https://siteofrefugeb2c.b2clogin.com/siteofrefugeb2c.onmicrosoft.com/b2c_1_sms_registry/oauth2/v2.0/authorize
   3. TOKEN_URL: https://siteofrefugeb2c.b2clogin.com/siteofrefugeb2c.onmicrosoft.com/b2c_1_sms_registry/oauth2/v2.0/token
   4. CLIENT_ID: 30222d8b-d3d1-4f62-9a2c-8161c2252e5b
4. Go to the **Authorization** tab
5. Input the following settings:
   1. Token Name: Azure AD B2C authenication
   2. Grant Type: Authorization Code (With PKCE)
   3. Callback URL: https://app-staging.siteofrefuge.com
   4. Authorize using browser: <keep unchecked>
   5. Auth URL: {{AUTH_URL}}
   6. Access Token URL: {{TOKEN_URL}}
   7. Client ID: {{CLIENT_ID}}
   8. Client Secret: <leave blank>
   9. Code Challenge Method: SHA-256
   10. Code Verifier: <leave blank>
   11. Scope: openid offline_access
   12. State: {{$randomUUID}}
   13. Client Authentication: Send client credentials in body
6.  Click **Get New Access Token**

At this point you will now have the ability to use that newly minted token with the APIs direclty in Postman.


## Generating code from OpenAPI definition

**WARNING**: Due to the movement to the newer isolation mode for Azure Functions autorest should NOT be used to stub out new API calls. Contact Dana if you have any questions about this.

```sh
autorest --input-file:".\docs\swagger.yaml" \
  --version:3.0.6320 \
  --namespace:SiteOfRefuge.API \
  --azure-functions-csharp \
  --generate-metadata:false \
  --output-folder:".\api"
```

## Generating db from OpenAPI definition
```sh
autorest --input-file:".\docs\swagger.yaml" \
  --use:autorest-sql-testing@latest
  --output-folder=".\db"
```

