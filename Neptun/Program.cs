using Microsoft.EntityFrameworkCore;
using Neptun.Data;
using Neptun.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<CourseService>();


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