using RatingAPI.Helpers;
using Microsoft.OpenApi.Models;
using RatingAPI.Interfaces;
using RatingAPI.Model;
using RatingAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Cors;



var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";//

var builder = WebApplication.CreateBuilder(args);

var key = "JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr";




// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false);

// CORS Configuration
var corsOriginsS = builder.Configuration.GetSection("AngularUrl:MyAngularUrl").Value;


//builder.Services.AddCors(options =>//
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      builder =>
//                      {
//                          builder.WithOrigins(corsOriginsS)
//                          .AllowAnyHeader();
//                      });
//});

builder.Services.AddCors(options =>

{

    options.AddPolicy(

    name: "AllowOrigin",

    builder => {

        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();

    });

});

builder.Services.AddControllers();





builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});


builder.Services.AddSingleton<IUserService>(new UserService(key));




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
//builder.Services.AddScoped<IUserService, UserService>();












var app = builder.Build();




app.UseMiddleware<JwtMiddleware>();

//builder.Services.AddSwaggerGen(swagger =>
//{
//    //This is to generate the Default UI of Swagger Documentation
//    swagger.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Version = "v1",
//        Title = "JWT Token Authentication API",
//        Description = ".NET 8 Web API"
//    });
//    // To Enable authorization using Swagger (JWT)
//    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
//    });
//    //swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
//    //            {
//    //                {
//    //                      new OpenApiSecurityScheme
//    //                        {
//    //                            Reference = new OpenApiReference
//    //                            {
//    //                                Type = ReferenceType.SecurityScheme,
//    //                                Id = "Bearer"
//    //                            }
//    //                        },
//    //                        new string[] {}

//    //                }
//    //            });
//});



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseCors(MyAllowSpecificOrigins);//
app.UseCors("AllowOrigin");


app.UseAuthorization();

app.MapControllers();

app.Run();
