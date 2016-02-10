using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Thing.Core.Contracts;

namespace Thing.Api.Infrastructure
{
    public class UnitOfWorkActionFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext,
            CancellationToken cancellationToken)
        {
            await GetUnitOfWork(actionExecutedContext.Request).CommitAsync(actionExecutedContext.Exception);
        }

        public override async Task OnActionExecutingAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            //if (!await Initer.HasCompleted())
            //{
            //    throw new InvalidOperationException("UOW is missing");
            //}

            await GetUnitOfWork(actionContext.Request).StartAsync();
        }

        private static IUnitOfWork GetUnitOfWork(HttpRequestMessage request)
        {
            var unitOfWork = request.GetDependencyScope().GetService(typeof (IUnitOfWork)) as IUnitOfWork;

            if (unitOfWork == null)
            {
                throw new InvalidOperationException("UOW is missing");
            }

            return unitOfWork;
        }
    }
}