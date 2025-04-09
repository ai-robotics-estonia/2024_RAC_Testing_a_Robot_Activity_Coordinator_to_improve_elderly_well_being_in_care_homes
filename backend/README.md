## Useful commands in .net console CLI

Install tooling

~~~bash
dotnet tool update -g dotnet-ef
dotnet tool update -g dotnet-aspnet-codegenerator 
~~~

## EF Core migrations

Run from solution folder

~~~bash
dotnet ef migrations --project App.DAL.EF --startup-project WebApp add FOOBAR
dotnet ef migrations   --project App.DAL.EF --startup-project WebApp remove

dotnet ef database   --project App.DAL.EF --startup-project WebApp update
dotnet ef database   --project App.DAL.EF --startup-project WebApp drop
~~~


## MVC controllers

Install from nuget:
- Microsoft.VisualStudio.Web.CodeGeneration.Design
- Microsoft.EntityFrameworkCore.SqlServer


Run from WebApp folder!

~~~bash
cd WebApp

dotnet aspnet-codegenerator controller -name ContestsController        -actions -m  App.Domain.Contest        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
# use area

dotnet aspnet-codegenerator controller -name RefreshTokensController        -actions -m  App.Domain.Identity.AppRefreshToken        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f

dotnet aspnet-codegenerator controller -name ContestsController        -actions -m  App.Domain.Contest        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f

dotnet aspnet-codegenerator controller -name AppsController        -actions -m  App.Domain.App        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name LogEventsController        -actions -m  App.Domain.LogEvent        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name MapsController        -actions -m  App.Domain.Map        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name MapLocationsController        -actions -m  App.Domain.MapLocation        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name OrganizationsController        -actions -m  App.Domain.Organization        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name OrganizationAppUsersController        -actions -m  App.Domain.OrganizationAppUser        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name RobotsController        -actions -m  App.Domain.Robot        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name RobotMapAppsController        -actions -m  App.Domain.RobotMapApp        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name RobotMapAppOrganizationsController        -actions -m  App.Domain.RobotMapAppOrganization        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f


dotnet aspnet-codegenerator controller -name ArticlesController        -actions -m  App.Domain.Article        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name AppVersionsController        -actions -m  App.Domain.AppVersion        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ArticleCategoriesController        -actions -m  App.Domain.ArticleCategory        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f


dotnet aspnet-codegenerator controller -name GreetingPhrasesController        -actions -m  App.Domain.GreetingPhrase        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name GreetingPhraseCategoriesController        -actions -m  App.Domain.GreetingPhraseCategory        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name WebLinksController        -actions -m  App.Domain.WebLink        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name WebLinkCategoriesController        -actions -m  App.Domain.WebLinkCategory        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f

dotnet aspnet-codegenerator controller -name MapFloorsController        -actions -m  App.Domain.MapFloor        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f


dotnet aspnet-codegenerator controller -name LecturesController        -actions -m  App.Domain.Attendance.Lecture        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name LectureAttendancesController        -actions -m  App.Domain.Attendance.LectureAttendance        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f

cd ..
~~~

Api controllers
~~~bash
dotnet aspnet-codegenerator controller -name ContestsController  -m  App.Domain.Contest        -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
~~~


## Docker

~~~bash
docker buildx build --progress=plain --force-rm --push -t akaver/webapp:latest . 

# multiplatform build on apple silicon
# https://docs.docker.com/build/building/multi-platform/
docker buildx create --name mybuilder --bootstrap --use
docker buildx build --platform linux/amd64 -t akaver/webapp:latest --push .
~~~


## Prompt engineering for Cursor
Context: Article.cs, Organization.cs, AppDbContext.cs
~~~xml
<purpose>
   Article must be categorized
</purpose>

<instructions>
   <instruction>Generate new model class in App.Domain project. named ArticleCategroy</instruction>
   <instruction>Add optional 1:m relationship between Article and ArticleCategroy</instruction>
   <instruction>Category is owned by Organization</instruction>
   <instruction>Use LangStr for CategoryDisplayName</instruction>
   <instruction>Use string for CategoryName, limited to 64 characters</instruction>
   <instruction>ArticleCategory must also have optional collection navigation property to Article</instruction>
   <instruction>Do not modify entity relationship rules in OnModelCreating</instruction>
</instructions>
~~~

Optimize generated controller - use viewmodels

Context: ArticleCategoryController.cs, Organization.cs
~~~xml
<purpose>
   Optimize  controller and associated views - use viewmodels and nameof. Entity is in ArticleCategory.cs
</purpose>

<instructions>
   <instruction>Controllers, Views and Viemodels folders and files are located in Admin area - WebApp/Areas/Admin</instruction>
   <instruction>Generate and use viewmodel for entity</instruction>
   <instruction>Do not copy properties over from entity - use it directly inside viewmodel</instruction>
   <instruction>Avoid using ViewBag and ViewData</instruction>
   <instruction>Use nameof function in selectlist creation</instruction>
   <instruction>Extract selectlist creation to separate method, order data by selectlist value field. Support also to set specific selected value - needed in Edit methods</instruction>
   <instruction>Selectlist creation methods should have default value as null for selected value. Example: (Guid? selectedValue = null)</instruction>
   <instruction>Use async methods with await as needed</instruction>
   <instruction>Update views to use the viewmodel</instruction>
   <instruction>In viewmodels use [ValidateNever] attribute for selectlists</instruction>
</instructions>
~~~

