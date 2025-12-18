using Hasamba_Library.Data;
using Hasamba_Library.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Hasamba_LibraryDB")));
builder.Services.AddScoped<ReadersService>();
builder.Services.AddScoped<BooksService>();
builder.Services.AddScoped<LoansService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
