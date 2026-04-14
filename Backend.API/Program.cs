var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

var certPath = builder.Configuration["Smp:CertPath"]
    ?? throw new InvalidOperationException("Smp:CertPath is not configured.");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");
app.MapControllers();

app.Run();
