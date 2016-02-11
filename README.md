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
    
    // ... other methods omitted

    [Route("")]
    public async Task<IHttpActionResult> Post([FromBody] ThingCommand.Create input)
    {
        await _mediator.SendAsync(input);

        return CreatedAtRoute("ThingDetail",new {id= input.Id},input);
    }

    [Route("{id}")]
    public async Task<IHttpActionResult> Delete([FromUri] ThingCommand.Delete input)
    {
        try
        {
            await _mediator.SendAsync(input);
        }
        catch (EntityNotFound)
        {
            return NotFound();
        }

        return Ok();
    }
}
```

Code for a db integration test.

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

Code for an api integration test.

```c#
public class ThingApiSpecs : IDisposable
{
    private readonly HttpClient _httpClient;

    public ThingApiSpecs(HttpClient httpClient)
    {
        _httpClient = httpClient;
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

    public void Should_delete(ThingCommand.Delete request)
    {
        //  Act
        request.Id = "my-thirdy";

        //  Arrange
        var response = _httpClient.DeleteAsync($"/api/thing/{request.Id}").Result;

        //  Assert
        response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }
}
```
Not so different ?

#Technology stack
* Project
    * .NET Framework 4.5.2  
    * OWIN & WebApi
    * Entity Framework 6.x
    * [Fluent Validation] (https://github.com/JeremySkinner/FluentValidation)
    * [MediatR] (https://github.com/jbogard/MediatR)
    * [Autofac] (http://autofac.org/)
    * [Automapper] (http://automapper.org/)
* Test
    * [Autofixture] (https://github.com/AutoFixture/AutoFixture)
    * [Fixie](https://fixie.github.io/)
    * [Should Assertions](https://github.com/erichexter/Should)

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

