# 02 - Design: about

## Tóm tắt

Thêm action `About` vào `HomeController` có sẵn. Action này trả về view tĩnh `Views/Home/About.cshtml`, view chứa tên blog và đoạn mô tả viết thẳng dưới dạng text. Thêm một mục menu "Giới thiệu" vào `Views/Shared/_Layout.cshtml`, trỏ tới action đó bằng tag helper `asp-controller="Home" asp-action="About"`. URL của trang là `/Home/About`, đi theo route mặc định có sẵn, nên không phải sửa `Program.cs`.

Không có dữ liệu, không có model, không có DbContext. Nội dung cố định (01, mục Ngoài phạm vi: không lưu vào cơ sở dữ liệu, không sửa qua giao diện).

## Ánh xạ AC -> thành phần

| AC | Thành phần (controller/action, view, model, dữ liệu) |
|---|---|
| AC-1 | `Views/Shared/_Layout.cshtml`: thêm `<li class="nav-item">` với link nhãn `Giới thiệu` trong `ul.navbar-nav`. Trang chủ (`Home/Index`) dùng layout chung qua `_ViewStart.cshtml` |
| AC-2 | Link trong layout dùng `asp-controller="Home" asp-action="About"`, sinh `href="/Home/About"`; `HomeController.About()` trả về `View()` -> HTTP 200 |
| AC-3 | `Views/Home/About.cshtml`: `<h1>Lê Khánh Trình</h1>` viết thẳng dạng text trong file |
| AC-4 | `Views/Home/About.cshtml`: `<p>` chứa đúng chuỗi mô tả trong AC-4, viết thẳng dạng text trong file |
| AC-5 | `About.cshtml` dùng layout chung (mặc định từ `_ViewStart.cshtml`, không đặt `Layout = null`), nên menu có mục "Giới thiệu" như mọi trang khác |
| AC-6 | `HomeController.About()` không có `[Authorize]`; app không cấu hình authentication; route mặc định `{controller}/{action}` phục vụ `/Home/About` trực tiếp mà không cần cookie hay session |

## Mô hình dữ liệu

Không có. Không entity, không migration, không view model. Nội dung nằm thẳng trong file Razor.

## Luồng xử lý

1. Người dùng mở bất kỳ trang nào dùng `_Layout.cshtml` (vd. `/`). Menu render link `<a class="nav-link text-dark" href="/Home/About">Giới thiệu</a>`.
2. Người dùng bấm link hoặc mở thẳng `/Home/About`. Route mặc định chuyển request tới `HomeController.About()`.
3. Action trả về `View()` -> render `Views/Home/About.cshtml` bên trong `_Layout.cshtml` -> HTTP 200, `text/html; charset=utf-8`.

### Lưu ý mã hóa ký tự (quan trọng cho AC-1, AC-3, AC-4, AC-5)

Mặc định, ASP.NET Core Razor đưa giá trị của biểu thức `@...` qua `HtmlEncoder`. Encoder này chỉ để nguyên ký tự Basic Latin, còn ký tự có dấu tiếng Việt thì đổi thành entity số (vd. `ệ` -> `&#x1EC7;`). Trình duyệt vẫn hiển thị đúng, nhưng HTML trả về sẽ không chứa nguyên văn chuỗi `Lê Khánh Trình`, và test so khớp chuỗi trên HTML sẽ đỏ. Text viết thẳng trong file `.cshtml` (không qua `@`) thì được giữ nguyên byte. Vì vậy:

- Nhãn menu `Giới thiệu`, tên blog, đoạn mô tả phải viết thẳng dạng text trong markup. Không đưa qua `@ViewData[...]`, `@Model`, biến C#, hằng số hay resource rồi in ra bằng `@`.
- Không dùng mẫu `<h1>@ViewData["Title"]</h1>` như `Privacy.cshtml` để in tên blog.
- Vẫn đặt `ViewData["Title"] = "Giới thiệu";` cho tiêu đề tab (khuyến nghị trong 01, không phải AC). Ở `<title>` giá trị này sẽ bị encode thành entity, nhưng trình duyệt vẫn hiển thị đúng, và không AC nào kiểm tra phần này.
- File `About.cshtml` và `_Layout.cshtml` phải lưu dạng UTF-8 (nên có BOM để chắc chắn trình biên dịch Razor đọc đúng trên Windows).

Phương án khác đã cân nhắc: cấu hình `WebEncoderOptions` với `UnicodeRanges.All` trong `Program.cs`. Không chọn, vì nó đổi hành vi encode của toàn site trong khi feature chỉ cần text tĩnh.

## Thay đổi so với khung hiện có

Không. Không thêm project, package, pattern, controller mới hay route mới. Chỉ thêm một action vào `HomeController` có sẵn, một view và một mục menu.

Feature không có DbContext, nên không có task đăng ký DbContext trong `Program.cs`. Ràng buộc `AddDbContext<T>((sp, options) => ...)` sẽ áp dụng cho feature đầu tiên thêm DbContext.

## Rủi ro

- **Encode ký tự có dấu**: nếu developer in nội dung qua `@`, AC-1/3/4/5 có thể đỏ khi test so khớp chuỗi trên HTML thô. Cách giảm: quy định rõ trong T-2, T-3, và trong unit test không kiểm tra nội dung view (unit test chỉ kiểm tra action).
- **Unicode normalization**: chuỗi tiếng Việt có thể ở dạng NFC (ký tự dựng sẵn) hoặc NFD (ký tự gốc + dấu kết hợp). Chép từ 01 có thể ra dạng khác với chuỗi tester dùng. Cách giảm: dùng dạng NFC (dạng thường gặp khi gõ bằng bộ gõ tiếng Việt và trong file markdown của repo).
- **HTTPS redirect trong test**: `app.UseHttpsRedirection()` có thể trả 307 cho request HTTP từ `WebApplicationFactory`. Đây là hành vi sẵn có của khung, tester xử lý ở phía client test (vd. `BaseAddress` https). Feature này không sửa `Program.cs`.
- **Footer/brand vẫn ghi `SimpleBlog.Web`**: đúng theo 01 (ngoài phạm vi), không phải lỗi.
