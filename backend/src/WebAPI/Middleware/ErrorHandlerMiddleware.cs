using Application.Auth.Exceptions;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebAPI.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                
                switch (error)
                {
                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case NotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case InvalidTokenException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case ExpiredRefreshTokenException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case InvalidUsernameOrPasswordException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case EmailIsNotConfirmedException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case EmailIsAlreadyConfirmed e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
