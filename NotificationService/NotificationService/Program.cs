using NotificationService.Services;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services
    .AddHttpClient<IEmailService, MailService>()
    .AddTransientHttpErrorPolicy(
        p =>
            p.WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)))
    );

builder.Services.AddControllers();
builder.Services
    .AddHttpClient<ISmsService, TextService>()
    .AddTransientHttpErrorPolicy(
        p =>
            p.WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)))
    );

builder.Services.Scan(
    scan =>
        scan.FromAssemblyOf<Program>()
            .AddClasses(c => c.Where(t => t.GetMethods().All(m => m.Name != "<Clone>$")))
            .AsImplementedInterfaces()
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
