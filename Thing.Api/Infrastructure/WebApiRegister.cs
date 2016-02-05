using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace Thing.Api.Infrastructure
{
    public static class WebApiRegister
    {
        public static void Register(HttpConfiguration configuration)
        {
            configuration.MapHttpAttributeRoutes();

            configuration.Filters.Add(new FluentValidationActionFilter());
            configuration.Filters.Add(new UnitOfWorkActionFilter());

            configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            configuration.Formatters.Remove(configuration.Formatters.XmlFormatter);
        }
    }
}