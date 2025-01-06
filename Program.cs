using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SMFG_API_New;
using SMFG_API_New.Models;
using System.Text;
using Microsoft.Extensions.Logging;
using Serilog;


 
//private IExceptionHandling _exceptionHandling;
 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Services.AddScoped<IExceptionHandling, ExceptionHandling>();
builder.Services.AddControllers();

builder.Services.AddDbContext<SmfgApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})

//.AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuerSigningKey = true,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("+4P0HQfbB3gQrtslmN5/PcU1AbqVx8rMjvgHf6/QE3k=")),
//         ValidateIssuer = false,
//         ValidateAudience = false
//     };
// });


//builder.services.Configure<IISOptions>(options =>
//{
//    options.AutomaticAuthentication = false;
//});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
     
   
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String("+4P0HQfbB3gQrtslmN5/PcU1AbqVx8rMjvgHf6/QE3k=")), // Use Base64 decoding
        ValidateIssuer = false, // Set to true if you need to validate Issuer
        ValidateAudience = false // Set to true if you need to validate Audience
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            // .LogError(context.Exception);

            Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

            Console.WriteLine($"Token failed: {context.Exception}");
            return Task.CompletedTask;
        },
         OnTokenValidated = context =>
         {

             Console.WriteLine("Token successfully validated.");
             return Task.CompletedTask;
         },
        OnChallenge = context =>
        {
            Console.WriteLine("Token challenge triggered.");
            return Task.CompletedTask;
        }
    };

});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var configuration = builder.Configuration;
builder.Services.AddScoped<ClsCommon>();



var app = builder.Build();
// Set ports for the application
//app.Urls.Add("http://*:5000");
//app.Urls.Add("https://*:5001");

//public void Configure(IApplicationBuilder app)
//{
//    app.UseMiddleware<GlobalExceptionMiddleware>();
//}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
