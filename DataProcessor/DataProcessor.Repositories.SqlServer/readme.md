
#Creating model from the database using the Package Manager Console

DataProcessor.Repositories.SqlServer.Console was created to allow the command Scaffold-DbContext to work. Scaffold-DbContext needs a starter project that targets
.Net Framework or .Net Core.

Scaffold-DbContext "Server=localhost;Database=DataProcessor_BUILD;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models