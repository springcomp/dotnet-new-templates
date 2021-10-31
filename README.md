# Azure Function project templates for `dotnet new`.

This folder contains a simple project template that can be used by `dotnet new` to create an Azure Function
with the following characteristics:

- Exposes an HTTP endpoint and demonstrates how to implement best-practices guidelines for end-to-end correlation.
- Demonstrates how to best use HTTP-based typed clients using the [Refit Library](https://github.com/reactiveui/refit).

This function assumes it is running on Linux and connects to [Azurite]() storage emulator, using the following connection string:

```json
"AzureWebJobsStorage": "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://azurite:10000/devstoreaccount1;QueueEndpoint=http://azurite:10001/devstoreaccount1;TableEndpoint=http://azurite:10002/devstoreaccount1;",
```

If you are running on Windows, use the more traditional connection string instead as follows:

```json
"AzureWebJobsStorage": "UseDevelopmentStorage=true;"
```

## How to use

### Installing the project template

This project template should be available as a package NuGet. First, install the template using the following command:

```pwsh
dotnet new -i <NUGET_PACKAGE_ID>
```

If you want install the latest version, clone this source code and create the NuGet package yourselves:

```pwsh
dotnet pack
dotnet new -i bin\Debug\Springcomp.Templates.1.0.0.nupkg
```

### Using the project template

Create a new Azure Function using the following command:

```pwsh
dotnet new func-http-refit -o <project-name>
```
