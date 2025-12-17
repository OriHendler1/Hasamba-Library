using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//DB context
builder.Services.AddDbContext<DbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Hasamba_LibraryDB")));

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
