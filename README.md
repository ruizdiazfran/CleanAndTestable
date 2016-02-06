# CleanAndTestable
A project to show how to put in place a simple and testable C# solution with some useful patterns like CQS, UnitOfWork, Mediator and so on.

#Requirements
SQL Server LocalDB 2012: http://www.microsoft.com/en-us/download/details.aspx?id=29062

<add name="ThingDbContext"
         connectionString="Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\Thing.mdf;Integrated Security=True"
         providerName="System.Data.SqlClient" />

SQL Server LocalDB 2014: https://www.microsoft.com/en-US/download/details.aspx?id=42299

<add name="ThingDbContext"
         connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Thing.mdf;Integrated Security=True"
         providerName="System.Data.SqlClient" />


