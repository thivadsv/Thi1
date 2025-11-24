using Microsoft.EntityFrameworkCore;
using VTTAPI.Data; // Đảm bảo đúng namespace

namespace VTTAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // LƯU Ý: TẤT CẢ builder.Services.Add... PHẢI Ở TRÊN builder.Build()

            // 1. Thêm dịch vụ Controllers
            builder.Services.AddControllers();

            // 2. Cấu hình Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Lấy Connection String
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // 3. Đăng ký dịch vụ DbContext (Đã chuyển lên trên)
            // PHẢI dùng UseSqlServer
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // 4. Thêm dịch vụ CORS (CHỈ 1 LẦN)
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                 policy =>
                                 {
                                     // Cho phép truy cập từ các nguồn gốc cụ thể hoặc AllowAnyOrigin() cho môi trường Dev
                                     policy.WithOrigins("http://localhost:3000", "http://127.0.0.1:5500")
                                           .AllowAnyHeader()
                                           .AllowAnyMethod();

                                     // Hoặc dùng policy.AllowAnyOrigin() để cho phép mọi nguồn gốc (chỉ nên dùng trong Dev)
                                     /*
                                     policy.AllowAnyOrigin()
                                           .AllowAnyHeader()
                                           .AllowAnyMethod();
                                     */
                                 });
            });


            // XÂY DỰNG ỨNG DỤNG (Container dịch vụ bị khóa sau dòng này)
            var app = builder.Build();

            // CẤU HÌNH HTTP REQUEST PIPELINE (app.Use...)

            // 1. Cấu hình cho môi trường Development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // 2. Sử dụng HTTPS
            app.UseHttpsRedirection();

            // 3. Sử dụng CORS Middleware (Kích hoạt chính sách)
            // LƯU Ý: Đây là lần duy nhất app.UseCors được gọi
            app.UseCors(MyAllowSpecificOrigins);

            // 4. Sử dụng Phân quyền (Authentication và Authorization)
            app.UseAuthorization();

            // 5. Ánh xạ Controllers
            app.MapControllers();

            app.Run();
        }
    }
}