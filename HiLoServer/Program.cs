using GameLogic.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IIterationsList, IterationsList>();
builder.Services.AddSingleton<IValuesList, ValuesList>();

builder.Services.AddScoped<IEngine, Engine>();
builder.Services.AddScoped<IEngineList, EngineList>();

//builder.Services.AddTransient<IValueGeneratorBase<int>, ValueGeneratorInt(1,2)>();

builder.Services.AddTransient<IValueGeneratorBase<int>>(provider => new ValueGeneratorInt(1,100));

//builder.Services.AddTransient<IValueGeneratorBase<int>>(new ValueGeneratorInt(1,100));

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
