using Microsoft.Data.SqlClient;
using Test1retake.Services;


var builder = WebApplication.CreateBuilder(args);

string conStr = builder.Configuration.GetSection("ConnectionStrings")["Defalut"];
builder.Services.AddControllers();
builder.Services.AddScoped<IDbService, DbService>();

var app = builder.Build();

app.MapControllerRoute(
    name: "GetProject",
    pattern: "{controller=Home}/{id?}");

app.UseAuthorization();

app.MapControllers();

app.Run();