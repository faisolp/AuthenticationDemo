using System.Security.Cryptography;
using System.Text;
using YourApp.Models;

namespace YourApp.Services
{
    public interface IUserService
    {
        Task<AuthResponse> RegisterAsync(RegisterModel model);
        Task<AuthResponse> LoginAsync(LoginModel model);
    }

    public class UserService : IUserService
    {
        private readonly IJwtService _jwtService;
        // สมมติว่าเรามี DbContext ให้เพิ่มตรงนี้
        // private readonly ApplicationDbContext _context;

        public UserService(IJwtService jwtService)
        {
            _jwtService = jwtService;
            // _context = context;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterModel model)
        {
            // ตรวจสอบว่ามี username หรือ email ในระบบแล้วหรือไม่
            // if (await _context.Users.AnyAsync(u => u.Username == model.Username || u.Email == model.Email))
            //    throw new Exception("Username or Email already exists");

            // สร้าง hash password
            var passwordHash = HashPassword(model.Password);

            // สร้าง user object
            var user = new UserModel
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = passwordHash
            };

            // บันทึกลงฐานข้อมูล
            // _context.Users.Add(user);
            // await _context.SaveChangesAsync();

            // สร้าง token
            return _jwtService.GenerateJwtToken(user);
        }

        public async Task<AuthResponse> LoginAsync(LoginModel model)
        {
            // ค้นหา user จาก username
            // var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == model.Username);

            // สมมติว่าเราพบ user (ในตัวอย่างนี้เราสร้าง dummy user)
            var user = new UserModel
            {
                Id = 1,
                Username = model.Username,
                Email = $"{model.Username}@example.com",
                PasswordHash = HashPassword(model.Password) // ในโค้ดจริงควรเทียบกับค่าในฐานข้อมูล
            };

            if (user == null)
                throw new Exception("Username or password is incorrect");

            // ตรวจสอบ password
            if (user.PasswordHash != HashPassword(model.Password))
                throw new Exception("Username or password is incorrect");

            // สร้าง token
            return _jwtService.GenerateJwtToken(user);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}