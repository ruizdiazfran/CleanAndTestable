# CleanAndTestable
A project to show how to put in place a simple and testable C# solution with some useful patterns like CQS, UnitOfWork, Mediator and so on.

#Goals

Below the code for a specification:

```c#
public class ThingTests : BaseTest
{
    private readonly IMediator _mediator;

    public ThingTests(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void Should_create_things()
    {
        const string id = "test-id";
        var request = new ThingCommand.Create {Id = id, Name = "Test", AddressLine = "Test", AddressZip = "20133"};
        Tx(db => _mediator.SendAsync(request).Wait());
        Check(db => db.Things.AnyAsync(_ => _.Id == id).Result.ShouldBeTrue());
    }
}
```

And below the code for a controller. Not so different ?

```c#
[RoutePrefix("api/thing")]
public class ThingController : ApiController
{
    private readonly IMediator _mediator;

    public ThingController(IMediator mediator)
    {
        _mediator = mediator;
    }

   [Route("")]
    public async Task<IHttpActionResult> Post([FromBody] ThingCommand.Create input)
    {
        await _mediator.SendAsync(input);

        return Ok();
    }
}
```

#Requirements
SQL Server LocalDB 2012 [link to download](http://www.microsoft.com/en-us/download/details.aspx?id=29062)

```xml
<add name="ThingDbContext"
         connectionString="Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\Thing.mdf;Integrated Security=True"
         providerName="System.Data.SqlClient" />
```
SQL Server LocalDB 2014  [link to download](https://www.microsoft.com/en-US/download/details.aspx?id=42299)

```xml
<add name="ThingDbContext"
         connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Thing.mdf;Integrated Security=True"
         providerName="System.Data.SqlClient" />
```

