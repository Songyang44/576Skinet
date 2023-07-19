
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreContext>(opt=>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
}
);

builder.Services.AddScoped<IProductRepository, ProductsRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
//AppDomain.CurrentDomain.GetAssemblies()：这是一个方法调用，获取当前应用程序域中加载的所有程序集。
//GetAssemblies() 方法返回一个 Assembly 数组，其中包含了当前应用程序域中加载的所有程序集。
//AppDomain. CurrentDomain. GetAssemblys(): This is a method call that retrieves all assemblies loaded in the current application domain. 
//The GetAssemblys() method returns an Assembly array that contains all the assemblies loaded in the current application domain.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

using var scope =app.Services.CreateScope();
var services=scope.ServiceProvider;
var context=services.GetRequiredService<StoreContext>();
var logger=services.GetRequiredService<ILogger<Program>>();
try{
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch(Exception ex){
    logger.LogError(ex,"An error occured during migration");
}

app.Run();
