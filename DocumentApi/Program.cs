using DocumentApi.Data;
using DocumentApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add MVC (Controllers + Views support)
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DocumentsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DocumentsDb")));

builder.Services.AddSingleton<BlobStorageService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable routing for MVC
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
