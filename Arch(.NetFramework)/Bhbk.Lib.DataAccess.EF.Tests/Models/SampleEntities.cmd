
powershell -command "& { '""Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=' + (Get-Item -Path '.\' -Verbose).FullName + '\Ahfc.Lib.Data.EFCore.Tests\Models\SampleDatabase.mdf;Integrated Security=True"""" Microsoft.EntityFrameworkCore.SqlServer' | out-file -filepath .\connection.tmp -nonewline -encoding ascii }"
set /p CONNECTION=< .\connection.tmp

dotnet ef dbcontext scaffold %CONNECTION% --context SampleEntities --startup-project Ahfc.Lib.Data.EFCore.Tests --project Ahfc.Lib.Data.EFCore.Tests --output-dir Models --use-database-names --verbose --force
set CONNECTION=
