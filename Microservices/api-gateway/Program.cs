var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseCors("AllowAll");

app.MapGet("/swagger", () => Results.Content(@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'/>
    <title>API Gateway - All Services</title>
    <link rel='stylesheet' type='text/css' href='https://unpkg.com/swagger-ui-dist@5/swagger-ui.css'>
    <style>
        body { margin: 0; padding: 0; }
        .section-header {
            color: white;
            padding: 16px 32px;
            font-family: Arial, sans-serif;
            font-size: 18px;
            font-weight: bold;
        }
        .top-header { background:#1a1a2e; font-size:22px; text-align:center; }
        .auth-header { background: #27ae60; }
        .qma-header  { background: #2980b9; }
    </style>
</head>
<body>
    <div class='section-header top-header'>API Gateway - All Microservices</div>

    <div class='section-header auth-header'>Auth Service - signup · login · logout · refresh</div>
    <div id='auth-ui'></div>

    <div class='section-header qma-header'>QMA Service - compare · convert · add · subtract · divide · history</div>
    <div id='qma-ui'></div>

    <script src='https://unpkg.com/swagger-ui-dist@5/swagger-ui-bundle.js'></script>
    <script>
        SwaggerUIBundle({
            url: '/auth-swagger/swagger/v1/swagger.json',
            domNode: document.getElementById('auth-ui'),
            presets: [SwaggerUIBundle.presets.apis],
            layout: 'BaseLayout',
            docExpansion: 'list',
            defaultModelsExpandDepth: -1
        });

        SwaggerUIBundle({
            url: '/qma-swagger/swagger/v1/swagger.json',
            domNode: document.getElementById('qma-ui'),
            presets: [SwaggerUIBundle.presets.apis],
            layout: 'BaseLayout',
            docExpansion: 'list',
            defaultModelsExpandDepth: -1
        });
    </script>
</body>
</html>
", "text/html; charset=utf-8"));

app.MapReverseProxy();
app.Run();