### DotNet Core Entityframework / WebApi template
This template provides a complete enterprise stack from an Entity Framework Code First core to a web api project including unit tests with mocking samples. 

#### Prerequisites
Install .NET Core 3.0 and SDK  
https://dotnet.microsoft.com/download/dotnet-core/3.0 

### Installing this template
#### Visual Studio 2019
Download the DotNetCoreEnterpriseProject.zip into:  
C:\Users\\[username]\Documents\Visual Studio 2019\Templates\ProjectTemplates\Visual C#

#### DotNetCore CLI
Download the Enterprise.DotNetCore.Templates.1.0.0.nupkg  
$ dotnet new -i Enterprise.DotNetCore.Templates.1.0.0.nupkg  
 

### Create New Project
#### Visual Studio 2019
* File New Project
* Start typing Enterprise in the search
* Select "ASP.NET Core Enterprise API Solution", click Next
* Choose a location and enter a name for the project
* Add a [User Secret](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows) for "DbConnectionString" point to your instance of SQL Server
* Set WebAPI project as Startup project
* Build the Solution

#### DotNetCore CLI
* $ mkdir [your project path]
* $ cd [your project path]
* $ dotnet new enterprisewebapi --namespace [Namespace] --filePrefix [Namespace]
* $ dotnet build
* $ add a [User Secret](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows) for 'DbConnectionString' point to your instance of SQL Server

### Configure Code First Database Migrations 
Visual Studio:  
* Load Tools > Nuget Package Manager -> Package Manager Console
* Select the Default Projects [ProjectName].DataAccess
* type: <i>Add-Migration initial</i>
* type: <i>Update-Database</i>

DotNetCore CLI:  
* Command/Terminal prompt, navigate to the [ProjectName].DataAccess project
* type: <i>dotnet ef migrations add initial --context [DbContextName]</i>
* type: <i>dotnet ef database update --context [DbContextName]</i>








