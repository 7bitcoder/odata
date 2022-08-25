
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using odata;
using Swashbuckle.AspNetCore.Community.OData.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var AddModel = () =>
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<WeatherForecast>("WeatherForecast");
    return builder.GetEdmModel();
};
builder.Services.AddControllers()
.AddOData(opts =>
{
    opts.EnableQueryFeatures().Count().AddRouteComponents("odata", AddModel());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
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
