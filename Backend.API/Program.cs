var builder = WebApplication.CreateBuilder(args);

var allowedOrigin = builder.Configuration["Cors:AllowedOrigin"]
    ?? throw new InvalidOperationException("Cors:AllowedOrigin is not configured.");

var certPath = builder.Configuration["Smp:CertPath"]
    ?? throw new InvalidOperationException("Smp:CertPath is not configured.");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigin)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddSingleton(_ => Session.Create());
builder.Services.AddSingleton<ISessionService>(_ => new SessionService(certPath));
builder.Services.AddSingleton<IMessageRepository, InMemoryMessageRepository>();

builder.Services.AddScoped<ConnectCommandHandler>();
builder.Services.AddScoped<AuthenticateCommandHandler>();
builder.Services.AddScoped<DisconnectCommandHandler>();
builder.Services.AddScoped<UploadCommandHandler>();
builder.Services.AddScoped<DownloadCommandHandler>();
builder.Services.AddScoped<GetCachedMessagesQueryHandler>();
builder.Services.AddScoped<GetSessionStatusQueryHandler>();

builder.Services.AddScoped<RequireSessionFilter>();

builder.Services.AddHealthChecks();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
