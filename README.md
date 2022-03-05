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

## Generating code from OpenAPI definition

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
