# 03 - Tasks: about

<!-- Developer chỉ được đổi [ ] thành [x] và chỉ sửa file trong dòng Files: của từng task. -->

## Tasks

- [x] T-1: Thêm action `public IActionResult About()` vào `HomeController`, trả về `View()`, không gắn `[Authorize]` hay attribute route riêng (dùng route mặc định, URL `/Home/About`). Viết unit test `HomeControllerTests.About_ReturnsViewResult`: gọi `new HomeController().About()`, assert kết quả là `ViewResult` và `ViewName` là `null` (tức view mặc định `About`). Kết quả mong đợi: `dotnet build` xanh, `dotnet test --filter "FullyQualifiedName~HomeControllerTests"` xanh.
  - Files: `src/SimpleBlog.Web/Controllers/HomeController.cs`, `tests/SimpleBlog.Tests/Unit/HomeControllerTests.cs`
  - AC: AC-2, AC-6

- [x] T-2: Tạo view `Views/Home/About.cshtml`, lưu UTF-8 (có BOM). Không đặt `Layout` (dùng layout chung từ `_ViewStart.cshtml`). Nội dung theo thứ tự: khối `@{ ViewData["Title"] = "Giới thiệu"; }` ở đầu file; `<h1>Lê Khánh Trình</h1>`; `<p>Nhiệm vụ của blog là chia sẻ để giúp cho người nào muốn thay đổi, phát triển, không phân biệt tuổi tác.</p>`. Tên blog và mô tả viết thẳng dạng text, chép đúng từng ký tự từ AC-3 và AC-4 trong 01 (dạng NFC), không đưa qua `@ViewData`, `@Model` hay biến C# (xem "Lưu ý mã hóa ký tự" trong 02). Kết quả mong đợi: chạy app, mở `/Home/About` được HTTP 200; view source của HTML trả về chứa nguyên văn hai chuỗi trên (không phải entity `&#x...;`) và có menu của layout.
  - Files: `src/SimpleBlog.Web/Views/Home/About.cshtml`
  - AC: AC-3, AC-4, AC-5, AC-6

- [x] T-3: Trong `Views/Shared/_Layout.cshtml`, thêm một `<li class="nav-item">` vào `ul.navbar-nav`, đặt sau mục Privacy, chứa `<a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="About">Giới thiệu</a>`. Nhãn `Giới thiệu` viết thẳng dạng text. Không sửa hay bỏ mục menu, brand, footer hay `<title>` đang có. Giữ file ở UTF-8 (có BOM nếu file chưa có). Kết quả mong đợi: HTML của `/` và `/Home/About` đều chứa `<a class="nav-link text-dark" href="/Home/About">Giới thiệu</a>`; bấm link mở được trang Giới thiệu.
  - Files: `src/SimpleBlog.Web/Views/Shared/_Layout.cshtml`
  - AC: AC-1, AC-2, AC-5
