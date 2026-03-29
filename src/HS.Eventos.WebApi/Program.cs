using HS.Eventos.Aplicacion;
using HS.Eventos.DataAccess.Dynamo;
using HS.Eventos.Messaging.SQS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAplicacion();
builder.Services.AddDataAccessForDynamo(builder.Configuration);
builder.Services.AddSqsMessageSender();

// Cache registration: use Redis when enabled, otherwise use in-memory distributed cache
var cacheEnabled = builder.Configuration.GetValue<bool>("Cache:Enabled", true);
if (cacheEnabled)
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetValue<string>("Redis:Configuration") ?? "localhost:6379";
        options.InstanceName = "HS.Eventos:";
    });
}
else
{
    builder.Services.AddDistributedMemoryCache();
}

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(cp =>
{
    cp.AllowAnyHeader();
    cp.AllowAnyMethod();
    cp.AllowAnyOrigin();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
