using FC.Codeflix.Catalog.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfigureApi(builder.Configuration);

var app = builder.Build();

app.UseConfigureApi(builder.Configuration);

app.Run();

public partial class Program { }
