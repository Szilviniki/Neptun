using Microsoft.EntityFrameworkCore;
using Neptun.Data;
using Neptun.Services;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddSwaggerGen(c => {
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<ScheduleService>();
builder.Services.AddScoped<RegistrationService>();
builder.Services.AddHostedService<NotificationBackgroundService>();



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