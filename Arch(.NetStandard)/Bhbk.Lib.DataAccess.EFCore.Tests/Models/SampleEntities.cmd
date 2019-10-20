
powershell -command "& { '""Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=' + (Get-Item -Path '.\' -Verbose).FullName + '\Bhbk.Lib.DataAccess.EFCore.Tests\Models\SampleDatabase.mdf;Integrated Security=True"""" Microsoft.EntityFrameworkCore.SqlServer' | out-file -filepath .\connection.tmp -nonewline -encoding ascii }"
set /p CONNECTION=< .\connection.tmp

dotnet ef dbcontext scaffold %CONNECTION% --context SampleEntities --startup-project Bhbk.Lib.DataAccess.EFCore.Tests --project Bhbk.Lib.DataAccess.EFCore.Tests --output-dir Models --use-database-names --verbose --force
set CONNECTION=
