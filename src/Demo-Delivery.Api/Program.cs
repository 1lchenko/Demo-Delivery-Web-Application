using Demo_Delivery.Api;
using Demo_Delivery.Api.Middleware;
using Demo_Delivery.Application;
using Demo_Delivery.Domain;
using Demo_Delivery.Infrastructure;
using Demo_Delivery.Infrastructure.Common.Extension;

//after second
var builder = WebApplication.CreateBuilder(args);

var conf = builder.Configuration;
builder.Services.AddApplication(conf);
builder.Services.AddDomain();

builder.Services.AddWebServices();

builder.AddInfrastructure(conf);
var app = builder.Build();

app.UseInfrastructure();
app.UseHandlerException();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestLocalization();

app.UseHttpsRedirection();

app.UseCors("PolicyApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();

namespace Demo_Delivery.Api
{
    public partial class Program{}
}
 