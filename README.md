# CleanAndTestable
A project to show how to put in place a simple and testable C# solution with some useful patterns like CQS, UnitOfWork, Mediator,  Dependency Injection Pattern, Composition Root and so on.

#Goals
Our motto for testing is "low ceremony" and "adhere to application".

Heavily inspired from [https://vimeo.com/68390508](https://vimeo.com/68390508) but with some difference because Moq is not used during test phase. Insted of Moq we use the real IOC container with specific overrides for some infrastructure (like Db or WebApi endpoint).
Low ceremony.

Below the code for a simple controller:

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

    [Route("")]
    public async Task<IHttpActionResult> Delete([FromUri] ThingCommand.Delete input)
    {
        await _mediator.SendAsync(input);

        return Ok();
    }
}
```

Code for an integration test (for db).

```c#
public class ThingSpecs : SpecsForDb
{
    private readonly IMediator _mediator;

    public ThingTests(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void Should_create(ThingCommand.Create request)
    {
        //  Act

        //  Arrange
        Persist(() => _mediator.SendAsync(request).Wait());

        //  Assert
        Do(db => db.Things.AnyAsync(_ => _.Id == request.Id).Result.ShouldBeTrue());
    }

    public void Should_delete(ThingCommand.Delete request)
    {
        //  Act
        request.Id = "my-thirdy";

        //  Arrange
        Persist(()=>_mediator.SendAsync(request).Wait());

        //  Assert
        Do(db => db.Things.AnyAsync(_ => _.Id == request.Id).Result.ShouldBeFalse());
    }
}
```

Code for an integration test (for api).

```c#
    public void Should_get_one()
    {
        //  Arrange
        const string id = "my-first";

        //  Act
        var response = _httpClient.GetAsync($"/api/thing/{id}").Result;

        //  Assert
        response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }

    public void Should_create(ThingCommand.Create request)
    {
        //  Act

        //  Arrange
        var response = _httpClient.PostAsJsonAsync($"/api/thing",request).Result;

        //  Assert
        response.StatusCode.ShouldEqual(HttpStatusCode.Created);
        response.Headers.Location.AbsoluteUri.ShouldEqual($"http://localhost/api/thing/{request.Id}");
    }
```

Not so different ?

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

