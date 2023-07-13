using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Crontab.Net.Api.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
         options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
         {
             In = ParameterLocation.Header,
             Description = "Please provide token",
             Name = "Authorization",
             Type = SecuritySchemeType.Http,
             BearerFormat = "Jwt",
             Scheme = "Bearer"
         });
         options.AddSecurityRequirement(new OpenApiSecurityRequirement
         {
             {
                 new OpenApiSecurityScheme
                 {
                     Reference = new OpenApiReference
                     {
                         Id = "Bearer",
                         Type = ReferenceType.SecurityScheme
                     }
                 },
                 new List<string>()
             }
         });
    }
}