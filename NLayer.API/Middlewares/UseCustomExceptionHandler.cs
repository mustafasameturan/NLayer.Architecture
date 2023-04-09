using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using NLayer.Core.DTOs;
using NLayer.Service.Exceptions;

namespace NLayer.API.Middlewares;

public static class UseCustomExceptionHandler
{
    public static void UseCustomException(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(config =>
        {
            config.Run(async context =>
            {
                //Custom modelin içerik tipi belirlendi.
                context.Response.ContentType = "application/json";
                //Hata içeriği alındı.
                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                //Client bazlı bir hata ise 400, server bazlı bir hata ise 500 durum kodu döndürüldü.
                var statusCode = exceptionFeature.Error switch
                {
                    ClientSideException => 400,
                    NotFoundException => 404,
                    _ => 500
                };
                context.Response.StatusCode = statusCode;
                
                //Custom Response sınıfımız ile custom exception modelimiz dönüldü.
                var response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);

                //JSON'a çevirildi.
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            });
        });
    }
}