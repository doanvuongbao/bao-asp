using bao_asp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Thêm Services vào container
// Gộp chung AddControllers và AddJsonOptions vào một chỗ cho sạch code
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// 2. Đăng ký SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Cấu hình Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Cấu hình HTTP request pipeline
// LUÔN bật Swagger (đưa ra ngoài if) để Render xem được
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // FIX lỗi 404: Sử dụng đường dẫn chuẩn của Swagger UI
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    //c.RoutePrefix = string.Empty; // Vào link gốc là hiện Swagger luôn
});

// Tạm thời comment cái này lại nếu bạn chưa dùng HTTPS trên Render để tránh lỗi vòng lặp chuyển hướng
// app.UseHttpsRedirection();

app.UseAuthorization();

// 5. Map các Controller
app.MapControllers();

app.Run();