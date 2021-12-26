# ShareLoanApp-API


## [](https://github.com/dotnet-web-api-template/tree/develop/#table-of-contents)Table of Contents

-   [System Requirements](https://github.com/dotnet-web-api-template/tree/develop/#system-requirements)
-   [Supported OS](https://github.com/dotnet-web-api-template/tree/develop/#supported-operating-systems)
-   [Prerequisites](https://github.com/dotnet-web-api-template/tree/develop/#prerequisites)
-   [Install Development tools](https://github.com/dotnet-web-api-template/tree/develop/#install-development-tools)
-   [Install SQL Server with Docker](https://github.com/dotnet-web-api-template/tree/develop/#running-sql-server-with-docker)
-   [Setup Project](https://github.com/dotnet-web-api-template/tree/develop/#setup-project)
-   [Setup Environment Variables](https://github.com/dotnet-web-api-template/tree/develop/#setup-environment-variables)
-   [Database Migration](https://github.com/dotnet-web-api-template/tree/develop/#database-migration)
-   [Run Project Locally](https://github.com/dotnet-web-api-template/tree/develop/#running-the-application-locally-on-your-machine)
-   [API Endpoints](https://github.com/dotnet-web-api-template/tree/develop/#api-endpoints)

## Preparing Development Environment

## System Requirements
* 1.8 GHz or faster processor. Quad-core or better recommended
* 2 GB of RAM; 8 GB of RAM recommended (2.5 GB minimum if running on a virtual machine)
* Hard disk space: Minimum of 800MB up to 210 GB of available space, depending on features installed; typical installations require 20-50 GB of free space.
* Hard disk speed: to improve performance, install Windows and Visual Studio on a solid state drive (SSD).
* Video card that supports a minimum display resolution of 720p (1280 by 720); Visual Studio will work best at a resolution of WXGA (1366 by 768) or higher.

## Supported operating systems
* Windows 10 Client Version 1607+ x64, x86
* macOS High Sierra (10.13+) x64
* Ubuntu 16.04+ x64, ARM32, ARM4
* You can use any Linux distribution which inheritance from Debian based

## Prerequisites
Before you begin, ensure you have installed the following
* Install the .NET Core `SDK 5.0.12`. Download link can be found [here](https://dotnet.microsoft.com/download/dotnet/5.0) - Choose an operating system of your choice to download SDK.
* Install the `ASP.NET Core Runtime 5.0.12`. Download link can be found [here](https://dotnet.microsoft.com/download/dotnet/5.0) - Choose runtime for your OS


## Install development tools. 
Visual Studio 2019 community edition or Rider or Visual Code is the preferred IDE
* Windows: Download `Viual Studio 2019 or 2022` community or `Visual Studio Code` [here](https://visualstudio.microsoft.com/downloads/)
* Mac or Linux: Download `Visual Studio Code` [here](https://code.visualstudio.com/download)
* The installation guide for Visual Studio can be followed [here](https://visualstudio.microsoft.com/downloads/)
* The setting up of Visual Studio Code can be followed [here](https://visualstudio.microsoft.com/downloads/)

## Install SQL Server

### Windows users
Please see SQL Server Hardware and software requirements [here](https://docs.microsoft.com/en-us/sql/sql-server/install/hardware-and-software-requirements-for-installing-sql-server?view=sql-server-ver15) before you start installing.
1. Download and install the SQL Server 2019 Developer free specialized edition [here](https://go.microsoft.com/fwlink/?linkid=866658);
2. Download and install the SQL Server Management Studio [here](https://aka.ms/ssmsfullsetup)
3. You can follow the MS SQL Management Studio installation guide [here](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)

## Running SQL Server with Docker
### Windows, Linux or macOS users 
Run SQL Server container images with Docker. Note: Use `Bash` command shell
1. Download and install and launch `Docker Engine(Docker Desktop)`. Choose your download base on your OS. Check [here](https://docs.docker.com/engine/install/) for what to download
2. Increase the Memory of Docker
	* Select Preferences from the little Docker icon in the top menu
	* Slide the memory slider up to at least 4GB
	* Click Apply & Restart
2. Pull the SQL Server 2019 Linux container image from Microsoft Container Registry.
		`sudo docker pull mcr.microsoft.com/mssql/server:2019-CU5-ubuntu-18.04`
3. To run the container image with Docker, you can use the following command from a bash shell (Linux/macOS) or elevated PowerShell command prompt.
	    `docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=<YourStrong@Passw0rd>" -p 1433:1433 --name sql1 -d mcr.microsoft.com/mssql/server:2019-CU5-ubuntu-18.04`
    * `-e` specifies an environment variable, here we specify password and acceptance of EULA (end user license agreement),
    * `-p` specifies the port to forward so that we can connect from the host (our local machine),
    * `--name` specifies the name used to identify the container - this is useful to start/stop/delete the container,
    * `-d` specifies that we want to start a detached container (runs in background).
4. To view your Docker containers, use the docker ps command.
		`sudo docker ps -a`
5. If the STATUS column shows a status of Up, then SQL Server is running in the container and listening on the port specified in the PORTS column
6. For more info, following the steps [here](https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver15&pivots=cs1-bash)
7. After a successful connection to the SQL Server, download and install [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver15) for a SQL Server GUI for your OS.
  
## Setup project

1. Clone the repository
	`git clone https://github.com/dotnet-web-api-template.git`
2. Checkout branch to `develop` and change directory `mtn-loan-api`
	`git checkout develop`
	`cd mtn-loan-api`
3. Run `dotnet restore` to install package dependencies
4. Run `dotnet build` to build the application


## Setup Environment Variables
Microsoft `Secret Manager` is used to store the apps environment variables on development. The environment variables are stored in a separate location from the project tree.
1. Open the application with your preferred IDE

### Using Visual Studio 2019 Community edition (Windows & macOS)

1. Right click on the `ShareLoanApp.API` project
2. Select `Manager User Secrets` from the context-menu
3. A `secret.json` will be open in the text editor of the IDE, then fill the following with your `env` variables

	```{
{
  "JwtSettings":{
    "Secret": "jwtsecretkey.iejoijojd"
  },
  "EmailSettings": {
    "SENDER_EMAIL": "email_account_on_sendgrid",
    "SENDGRID_KEY": "send_grid_key...;ldpkfkpe"
  },
  "AzureSettings": {
    "AZURE_STORAGE_CONNECTION": "DefaultEndpointsProtocol=https;AccountName={storagenameonazureblob}};AccountKey=secretkeyofaccount_eg_tk;kr;tsfkkljlkjfljsljf==;EndpointSuffix=core.windows.net",
    "AZURE_CONTAINER_NAME": "nameofazureblobcontainer"
  },
  "ConnectionString": {
    "DefaultConnection": "Server=sqlserver.ip.address,port;Database=<databasename>;User Id=sa;Password=password32;MultipleActiveResultSets=true"
  }
}
4. Note: Provide your SQL Server connection string for `DefaultConnection` in the above `secret.json` file
   * The database name should be named `ShareLoanAppDB`

### Using Visual Studio Code

1. Change directory into the `ShareLoanApp.API` project directory
    `cd src/ShareLoanApp.API`
2. On the bash terminal run `dotnet user-secrets init`
3. To set a `Secret` for example to set an `env` variable for the database connection string, simply do the following
    `dotnet user-secrets set "ConnectionString:DefaultConnection1":"Server=localhost;Database=EShareLoanAppDB;Trusted_Connection=True;MultipleActiveResultSets=true"`
4. You can now go ahead to set `env` variables for each secrets needed for the project.

Note: You should note that the `secret.json` file is generated and can be located in the following location
    * Windows: `%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json`
    * Linux/macOS: `~/.microsoft/usersecrets/<user_secrets_id>/secrets.json`
If you can locate the `secret.json` and then open it, add the following `env` variables
    
    ```{
    {
      "JwtSettings":{
        "Secret": "jwtsecretkey.iejoijojd"
      },
      "EmailSettings": {
        "SENDER_EMAIL": "email_account_on_sendgrid",
        "SENDGRID_KEY": "send_grid_key...;ldpkfkpe"
      },
      "AzureSettings": {
        "AZURE_STORAGE_CONNECTION": "DefaultEndpointsProtocol=https;AccountName={storagenameonazureblob}};AccountKey=secretkeyofaccount_eg_tk;kr;tsfkkljlkjfljsljf==;EndpointSuffix=core.windows.net",
        "AZURE_CONTAINER_NAME": "nameofazureblobcontainer"
      },
      "ConnectionString": {
        "DefaultConnection": "Server=sqlserver.ip.address,port;Database=<databasename>;User Id=sa;Password=password32;MultipleActiveResultSets=true"
      }
    }
Provide your SQL Server connection string for `DefaultConnection` in the above `secret.json` file

You can go to this link [here](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=linux) for more information on how to setup a `Secret Manager` on your environment

## Database Migration
### Entity Framework Core tools
1.  `dotnet ef` must be installed as a global or local tool. Install `dotnet ef` as a global tool with the following command:
    `dotnet tool install --global dotnet-ef`
2. In the terminal change directory into the `src/ShareLoanApp.API` project directory
    `cd src/ShareLoanApp.API`
3. Add Migration: From the directory above run command: `dotnet ef Migrations add <migration-name> --project ../ShareLoanApp.Infrastructure.Data`
3. Apply the migration to the database to create the schema.
    `dotnet ef database update`

Visit this link [here](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli) for more information on using `ef core tool`

## Running the application locally on your machine
Make sure all the above instructions are done completely before running the application
1. If you are using `Visual Studio 2019 community`
    * Right click on the `ShareLoanApp.API` project and select `Set as Startup Project`
    * Then press the `F5` to run the application or click on the `IIS Express` run button on the standard toolbar to run the application

2. If you are using `Visual Studio Code`
    * In your terminal, change directory into `mtn-loan-api` directory where the solution file is located and then run the following: 
        `dotnet restore`
        `dotnet run`
    * The above commands will restore missing application packages, build and then run the application
3. Either ways will make the project web APIs accessible on Swagger over `http://localhost:5000/index.html` Or  `https://localhost:5001/index.html`

## API Endpoints
* Check out the applications endpoints on Swagger [here](https://webapi_domain_name.com)
