using LiquidlabsPostcacheApi.ExternalApi;
using LiquidlabsPostcacheApi.Middleware;
using LiquidlabsPostcacheApi.Repositories;
using LiquidlabsPostcacheApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Application DI Registrations
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();

// Typed HttpClient Registration
builder.Services.AddHttpClient<IJsonPlaceholderClient, JsonPlaceholderClient>(c =>
{
    c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    c.Timeout = TimeSpan.FromSeconds(10);
});

var app = builder.Build();

// Configure HTTP pipeline
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();