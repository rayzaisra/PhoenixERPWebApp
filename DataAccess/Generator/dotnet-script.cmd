@echo off
@echo Generating POCO and DTO
@echo Please wait ...

@echo Generate Repo
dotnet script NPocoGenerator.csx -- output:Schema/Database.cs connectionstring:ConnectionStrings:MyDatabase config:appsettings.json namespace:DataAccess.PhoenixERP reponame:PhoenixERPRepo npocomodel:false repogenerate:true

@echo Generate schema dbo
dotnet script NPocoGenerator.csx -- output:Schema/General.cs connectionstring:ConnectionStrings:MyDatabase config:appsettings.json namespace:DataAccess.PhoenixERP.General reponame:PhoenixERPRepo spclass:StoreProcedure schema:dbo
@echo Generate schema finance
dotnet script NPocoGenerator.csx -- output:Schema/Finance.cs connectionstring:ConnectionStrings:MyDatabase config:appsettings.json namespace:DataAccess.PhoenixERP.Finance reponame:PhoenixERPRepo spclass:StoreProcedure schema:fn

@echo Generate Model DTO General
dotnet script DtoGenerator.csx -- output:../../DataAccess.Dto/Generator/Schema/General.cs connectionstring:ConnectionStrings:MyDatabase config:appsettings.json namespace:DataAccess.PhoenixERP.General reponame:PhoenixERPRepo schema:dbo

@echo Generate Model DTO Finance
dotnet script DtoGenerator.csx -- output:../../DataAccess.Dto/Generator/Schema/Finance.cs connectionstring:ConnectionStrings:MyDatabase config:appsettings.json namespace:DataAccess.PhoenixERP.Finance reponame:PhoenixERPRepo schema:fn

REM -----------------------------------------------------
@echo Generate Base Entity
dotnet script DataContextGenerator.GetBaseEntity.csx -- output:../../BusinessLogic/DataContext/DataContext.GetBaseEntity.cs connectionstring:ConnectionStrings:MyDatabase config:appsettings.json namespace:BusinessLogic

@echo Generate App Entities
dotnet script DataContextGenerator.GetAppEntities.csx -- output:../../BusinessLogic/DataContext/DataContext.GetAppEntities.cs connectionstring:ConnectionStrings:MyDatabase config:appsettings.json namespace:BusinessLogic

REM @echo Generate Service Entity
REM dotnet script ServiceGenerator.csx -- output:../../BusinessLogic/Services/Service.Entities.cs connectionstring:ConnectionStrings:MyDatabase config:appsettings.json namespace:BusinessLogic.Services

@echo Generate Service Entity General
dotnet script ServiceGenerator.csx -- output:../../BusinessLogic/Services/Service.Entities.General.cs connectionstring:ConnectionStrings:MyDatabase config:appsettings.json namespace:BusinessLogic.Services.General schema:dbo

@echo Generate Service Entity Finance
dotnet script ServiceGenerator.csx -- output:../../BusinessLogic/Services/Service.Entities.Finance.cs connectionstring:ConnectionStrings:MyDatabase config:appsettings.json namespace:BusinessLogic.Services.Finance schema:fn

set /p temp="Hit enter to continue"