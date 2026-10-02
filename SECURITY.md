# Bảo mật

## Báo lỗ hổng

Đừng mở issue công khai cho lỗ hổng bảo mật. Hãy dùng **Report a vulnerability** trong tab **Security** của repo trên GitHub (private vulnerability reporting). Chỉ người quản lý repo đọc được báo cáo.

## Secret

Secret gồm: mật khẩu, API key, token, connection string có tên đăng nhập hoặc mật khẩu, khóa ký, chứng chỉ.

### Không bao giờ commit secret

- Không để secret trong `appsettings.json`, `appsettings.*.json`, `launchSettings.json`, code hay test.
- `appsettings*.json` chỉ chứa giá trị không nhạy cảm. Connection string LocalDB dùng `Trusted_Connection=True` (không có mật khẩu) thì được.
- Repo này là **public**: mọi thứ đã push đều có thể bị người khác hoặc bot đọc trong vài phút.

### Để secret ở đâu

| Môi trường | Cách lưu |
|---|---|
| Máy dev | `dotnet user-secrets` (lưu trong hồ sơ người dùng, ngoài thư mục repo). Lần đầu: `dotnet user-secrets init --project src/SimpleBlog.Web`, rồi `dotnet user-secrets set "Key" "value" --project src/SimpleBlog.Web` |
| CI (GitHub Actions) | GitHub repository secrets, đọc qua `${{ secrets.NAME }}` |
| Server | Biến môi trường, hoặc kho secret như Azure Key Vault |

ASP.NET Core tự đọc user secrets (môi trường Development) và biến môi trường, nên code chỉ cần đọc qua `IConfiguration`, không phải biết secret nằm ở đâu.

### Nếu lỡ commit secret

1. **Coi secret đó đã lộ**, kể cả khi chưa push hoặc đã xóa ngay ở commit sau. Lịch sử git vẫn giữ nó.
2. Thu hồi secret ở dịch vụ gốc và tạo secret mới. Đây là bước bắt buộc; xóa khỏi file không đủ.
3. Đưa secret mới vào đúng chỗ theo bảng trên.
4. Báo người quản lý repo.

### Lớp bảo vệ trên GitHub

Repo bật **secret scanning** và **push protection**: GitHub chặn push có chứa secret dạng đã biết (key của các nhà cung cấp lớn) và cảnh báo secret đã lọt vào repo. Đây là lưới an toàn, không thay cho việc tự cẩn thận: nó không nhận ra mọi loại secret.

## Lỗ hổng trong code và thư viện

- **CodeQL** quét code C# và GitHub Actions workflow trên mọi PR và hằng tuần. Cảnh báo nằm ở **Security → Code scanning**.
- **Dependabot alerts** báo khi một thư viện đang dùng có lỗ hổng đã công bố; **Dependabot security updates** tự mở PR nâng lên bản đã vá. Ngoài ra Dependabot mở PR cập nhật thường kỳ hằng tuần.
- Cảnh báo mức High hoặc Critical được xử lý trước các việc khác.
