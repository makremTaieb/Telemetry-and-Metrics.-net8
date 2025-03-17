using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddOpenTelemetry()
    .ConfigureResource(res => res.AddService("ClientService"))
    .WithMetrics(m =>
    {
        m.AddAspNetCoreInstrumentation()
         .AddHttpClientInstrumentation();

        m.AddOtlpExporter(opt =>
        {
            opt.Endpoint = new Uri("Http://localhost:18889");
        });
    })
    .WithTracing(t =>
    {
        t.AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation();


        t.AddOtlpExporter(opt =>
        {
            opt.Endpoint = new Uri("Http://localhost:18889");
        });
    });

builder.Logging.AddOpenTelemetry(opt =>
{
    opt.AddConsoleExporter()
       .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("ClientService"));


    opt.AddOtlpExporter(x =>
    {
        x.Endpoint = new Uri("Http://localhost:18889");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
