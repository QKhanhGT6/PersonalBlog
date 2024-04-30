![Blog main interface](https://github.com/QKhanhGT6/PersonalBlog/assets/153789536/ce487961-b69e-4a4a-b154-9b35f9d636bf)# Personal Blog 

## 1) Project Description
Fully functional blog website using **ASP.NET Core MVC 3.1** and **SSMS** (SqlServer) for database. Have a 
+ Home page which shows all the created posts (sorted by Create/Edit date) as well as a search bar (can search for words in post's title / category) and pagination.
+ Post page with a header image and a comment section (require log in to comment).
+ AboutMe page to include author's info.
+ Admin page with the ability to Create/Edit/Delete post and edit AboutMe page. When Create/Edit post, if Publish box is not ticked, that post will be hide from Home page.

## 2) How to install and run project
+ Download ZIP file and extract
+ Open Visual Studio Installer -> Modify. In Individual Components tab, install .NET Core 3.1 Runtime & .Net 6.0 Runtime
+ Open project in Visual Studio, IF NEEDED, modify the ConnectionString (Server=[*insert server name*]) in appsetting.json file
+ Tools -> NuGet Package Manager -> Manage NuGet Packages for solution -> Install these:
  + Microsoft.EntityFrameworkCore
  + Microsoft.EntityFrameworkCore.SqlServer 
  + Microsoft.AspNetCore.Identity.EntityFrameworkCore
  + Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
  + PagedList.Core.Mvc
+ Go to Package Manager Console, Choose OldBLOG.Data in the Default Project dropdown. After that, insert these lines after the PM>:
  '''
  Add-Migration [*insert name of your choice*]
  Update-Database
  '''

## 3) Project Description
### Home page
![Blog main interface](https://github.com/QKhanhGT6/PersonalBlog/assets/153789536/ff5f253e-41ac-4502-9613-6e8fe5300d60)

### Post page
![Blog post interface](https://github.com/QKhanhGT6/PersonalBlog/assets/153789536/1a687412-c241-46aa-a4d8-5164d9885ce5)

### AboutMe page
![Blog AboutMe interface](https://github.com/QKhanhGT6/PersonalBlog/assets/153789536/0cd885fc-a902-404b-8a3d-d68c1e1505b6)

### Admin page
![Blog Admin interface](https://github.com/QKhanhGT6/PersonalBlog/assets/153789536/612fb1e3-218c-4e29-823e-350d23c99f6d)

### Create/Edit page
![Blog Edit interface](https://github.com/QKhanhGT6/PersonalBlog/assets/153789536/fbc30c7a-982e-460c-b9dd-3d4c63c4102c)
