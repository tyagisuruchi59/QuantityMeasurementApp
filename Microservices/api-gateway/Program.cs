var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Use gateway proxy URLs instead of direct service URLs
    c.SwaggerEndpoint("/auth-swagger/swagger/v1/swagger.json", "Auth Service v1");
    c.SwaggerEndpoint("/qma-swagger/swagger/v1/swagger.json", "QMA Service v1");
    c.RoutePrefix = "swagger";
});

app.MapReverseProxy();
app.Run();