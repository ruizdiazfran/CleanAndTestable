using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using FluentValidation;
using FluentValidation.Results;

namespace Thing.Api.Infrastructure
{
    public class FluentValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var actionArguments = actionContext.ActionArguments.Select(argument => argument);

            foreach (var actionArgument in actionArguments)
            {
                if (actionArgument.Value == null)
                {
                    var actionArgumentDescriptor = GetActionArgumentDescriptor(actionContext, actionArgument.Key);

                    if (actionArgumentDescriptor.IsOptional)
                    {
                        continue;
                    }

                    var validator = GetValidatorForActionArgumentType(actionContext,
                        actionArgumentDescriptor.ParameterType);

                    if (validator == null)
                    {
                        continue;
                    }

                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);

                    return;
                }
                else
                {
                    var validator = GetValidatorForActionArgument(actionContext, actionArgument.Value);

                    if (validator == null)
                    {
                        continue;
                    }

                    var validationResult = validator.Validate(actionArgument.Value);

                    if (validationResult.IsValid)
                    {
                        continue;
                    }

                    WriteErrorsToModelState(validationResult, actionContext);
                }
            }

            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.BadRequest, new ApiResourceValidationErrorWrapper(actionContext.ModelState));
            }
        }

        private static HttpParameterDescriptor GetActionArgumentDescriptor(HttpActionContext actionContext,
            string actionArgumentName)
        {
            return actionContext.ActionDescriptor
                .GetParameters()
                .SingleOrDefault(prm => prm.ParameterName == actionArgumentName);
        }

        private static IValidator GetValidatorForActionArgument(HttpActionContext actionContext, object actionArgument)
        {
            var abstractValidatorType = typeof (IValidator<>);
            var validatorForType = abstractValidatorType.MakeGenericType(actionArgument.GetType());
            var resolver = actionContext.Request.GetDependencyScope();

            return resolver.GetService(validatorForType) as IValidator;
        }

        private static IValidator GetValidatorForActionArgumentType(HttpActionContext actionContext, Type actionArgument)
        {
            var abstractValidatorType = typeof (IValidator<>);
            var validatorForType = abstractValidatorType.MakeGenericType(actionArgument);
            var resolver = actionContext.Request.GetDependencyScope();

            return resolver.GetService(validatorForType) as IValidator;
        }

        private static void WriteErrorsToModelState(ValidationResult validationResults, HttpActionContext actionContext)
        {
            foreach (var error in validationResults.Errors)
            {
                actionContext.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }

    internal class ApiResourceValidationErrorWrapper
    {
        private const string ErrorMessage = "The request is invalid.";

        private const string MissingPropertyError = "Undefined error.";

        public ApiResourceValidationErrorWrapper(ModelStateDictionary modelState)
        {
            Message = ErrorMessage;
            SerializeModelState(modelState);
        }

        public ApiResourceValidationErrorWrapper(string message, ModelStateDictionary modelState)
        {
            Message = message;
            SerializeModelState(modelState);
        }

        public string Message { get; private set; }

        public IDictionary<string, IEnumerable<string>> Errors { get; private set; }

        private void SerializeModelState(ModelStateDictionary modelState)
        {
            Errors = new Dictionary<string, IEnumerable<string>>();

            foreach (var keyModelStatePair in modelState)
            {
                var key = keyModelStatePair.Key;

                var errors = keyModelStatePair.Value.Errors;

                if (errors == null || errors.Count <= 0)
                {
                    continue;
                }

                var errorMessages = errors.Select(error => string.IsNullOrEmpty(error.ErrorMessage)
                    ? MissingPropertyError
                    : error.ErrorMessage).ToArray();

                Errors.Add(key, errorMessages);
            }
        }
    }
}