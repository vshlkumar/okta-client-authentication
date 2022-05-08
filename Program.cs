using DemoApplication.Model;
using DemoApplication.Services;
using DemoApplication.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Okta.AspNetCore;
using Exceptionless;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddFluentValidation(x =>
    {
        x.ImplicitlyValidateChildProperties = true;

        //using the automatic register method to register the validators
        x.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        
        x.ValidatorOptions.LanguageManager = new MessageLanguageManager();
    });


//global configuration to log to exceptionless
//get the key from your exceptionless account
ExceptionlessClient.Default.Startup("gQFvkuIZWh9zlFCTRWYKFx0T6VmO4jpVm1zFx6Qs");

var oktaSetting = builder.Configuration.GetSection("Okta").Get<OktaSetting>();

builder.Services.AddSwaggerGen(c =>
{

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Okta Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                         },
                         new string[] {}
                    }
                });
});

//configure the okta setting to services so we can have this direclty under the contructor via dependency injection
builder.Services.Configure<OktaSetting>(builder.Configuration.GetSection("Okta"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
})
.AddOktaWebApi(new OktaWebApiOptions()
{
    OktaDomain = oktaSetting.Domain,
    AuthorizationServerId = oktaSetting.AuthorizationServerId,
    Audience = oktaSetting.Audience
});

builder.Services.AddAuthorization();
builder.Services.AddSingleton<ITokenService, OktaTokenService>();

var app = builder.Build();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = String.Empty;
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
});

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
