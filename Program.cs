using bao_asp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Thêm Services vào container
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// --- THÊM CẤU HÌNH CORS TẠI ĐÂY ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.AllowAnyOrigin()   // Cho phép tất cả nguồn (hoặc .WithOrigins("http://localhost:5173"))
              .AllowAnyMethod()   // Cho phép GET, POST, PUT, DELETE
              .AllowAnyHeader();  // Cho phép mọi Header
    });
});
// --------------------------------

// 2. Đăng ký SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Cấu hình Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Cấu hình HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

// --- KÍCH HOẠT CORS TẠI ĐÂY (Phải đặt TRƯỚC MapControllers) ---
app.UseCors("AllowReactApp");
// ------------------------------------------------------------

app.UseAuthorization();

// 5. Map các Controller
app.MapControllers();

app.Run();