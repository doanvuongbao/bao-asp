using bao_asp.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// 1. Thêm Services vào container
builder.Services.AddControllers();

// 2. Đăng ký SQL Server (Đã sửa lỗi ngắt dòng và dấu ngoặc)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Tránh lỗi vòng lặp JSON cho Orders/OrderItems
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// 3. Cấu hình Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Cấu hình HTTP request pipeline

// Đưa Swagger ra ngoài if để luôn hiển thị trên Render
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty; // Truy cập link gốc là ra ngay Swagger
});

// Bạn có thể giữ lại đoạn này nếu muốn check môi trường khác
if (app.Environment.IsDevelopment())
{
    // Các cấu hình chỉ dành cho máy cá nhân (nếu có)
}
;

// 5. Map các Controller
app.MapControllers();


app.Run();