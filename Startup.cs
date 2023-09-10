using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Bank;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql("server=localhost;port=3306;database=bank_system;uid=root;password=1234", new MySqlServerVersion(new Version(8, 0, 31))));
builder.Services.AddScoped<ILoginService, Login>();
builder.Services.AddScoped<IBalance, Balance>();
builder.Services.AddScoped<IName, Name>();
builder.Services.AddScoped<ITransfer, Transfer>();
builder.Services.AddScoped<ISignIn, SignIn>();
builder.Services.AddScoped<IInsertStatement, InsertStatement>();
builder.Services.AddScoped<IRetriveFK, RetriveFK>();
builder.Services.AddScoped<IRetriveStatement, RetriveStatement>();
builder.Services.AddScoped<IFetchStatements, FetchStatements>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.EnvironmentName == "Development")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();