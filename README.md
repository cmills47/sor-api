# Site of Refuge API

This repo hosts the core API for Site of Refuge. You can access the [API documentation here](https://siteofrefuge.github.io/sor-api).

API documentation is based on OpenAPI 3.0. Raw swagger yaml can be [seen here](https://github.com/SiteOfRefuge/sor-api/blob/main/docs/swagger.yaml).

## Environment setup
- You should install the [Azure Function Core Tools](https://github.com/Azure/azure-functions-core-tools#installing) if you wish to run and debug these functions locally.

## Generating code from OpenAPI definition
`autorest --input-file:".\docs\swagger.yaml" --version:3.0.6320 --namespace:SiteOfRefuge.API --azure-functions-csharp --generate-metadata:false --output-folder:".\" `
