[![Build Status](https://dev.azure.com/nanotome/streak/_apis/build/status%2FRanque.Build?branchName=master)](https://dev.azure.com/nanotome/streak/_build/latest?definitionId=6&branchName=master)

This project contains the APIs, database and other core infrastructure items needed for the 
'backend' of all Ranque client applications.

The server project is written in C# using .NET Core with ASP.NET Core. The database is written 
in SQL Server. The codebase can be developed, built, run, and deployed on Windows.

# Developer Documentation
## Clone repository
```bash
git clone git@github.com:gnerkus/ranque.git
cd ranque
```

## SQL Server
Create a database called `streak` in a local SQL Server instance.

## Configure Secrets
- Add a random secret string to the `RANQUE_SECRET` environment variable
- Copy your Sentry DSN into the `SENTRY_DSN` environment variable

## Build and Run the Server
1. Open a new terminal window in the root of the repository
2. Restore nuget packages
    ```bash
      dotnet restore
    ```
3. Start the server
    ```bash
      dotnet run
    ```
4. Test that the API is alive by navigating to https://localhost:5001/health

### Rider
Launch the project by clicking the "Play" button for the project.

