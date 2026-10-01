# 04 - Review: gioi-thieu

## Phạm vi đã xem

- Đã sửa: `src/SimpleBlog.Web/Controllers/HomeController.cs`, `src/SimpleBlog.Web/Views/Shared/_Layout.cshtml`, `docs/features/gioi-thieu/03-tasks.md` (chỉ đổi dấu tick).
- File mới (untracked), đã đọc toàn bộ: `src/SimpleBlog.Web/Views/Home/About.cshtml`, `tests/SimpleBlog.Tests/Unit/HomeControllerTests.cs`.
- File bị .gitignore che: chỉ có `bin/`, `obj/` (bỏ qua), không có finding.
- Đã chạy `dotnet test`: 2 test đạt, 0 lỗi.

## Đối chiếu thiết kế

- Đúng 02: thêm action `About()` vào `HomeController` có sẵn, trả về `View()`, không có `[Authorize]` hay route riêng.
- `About.cshtml` có `ViewData["Title"]`, tên blog và mô tả viết thẳng dạng text, không đi qua `@`, không đặt `Layout`.
- `_Layout.cshtml` thêm `<li>` sau mục Privacy, dùng `asp-controller="Home" asp-action="About"`, các mục khác không bị đổi.
- Cả `About.cshtml` và `_Layout.cshtml` đều là UTF-8 có BOM (đã kiểm tra byte đầu).
- Không thêm project, package, pattern ngoài thiết kế. File thay đổi đều nằm trong `Files:` của T-1, T-2, T-3. Các task đã tick đều có code thật.
- `01-requirements.md` và `02-design.md` không bị sửa. Nội dung task trong `03-tasks.md` không đổi, chỉ có dấu tick.

## Đối chiếu AC

| AC | Đáp ứng | Ghi chú |
|---|---|---|
| AC-1 | Đạt | Layout có link "Giới thiệu" trong menu, trang chủ dùng layout chung. |
| AC-2 | Đạt | Link sinh `/Home/About`, action trả `View()` nên HTTP 200. |
| AC-3 | Đạt | `<h1>Lê Khánh Trình</h1>` viết thẳng trong view. |
| AC-4 | Đạt | `<p>` chứa đúng chuỗi mô tả, viết thẳng. |
| AC-5 | Đạt | View dùng layout chung nên menu có mục "Giới thiệu". |
| AC-6 | Đạt | Không có `[Authorize]`, app không cấu hình authentication. |

Phần kiểm tra HTML thật (chuỗi không bị đổi thành entity) thuộc về tester. Qua việc đọc code, không có chỗ nào đưa nội dung qua `@`.

## Findings

| Mã | Mức độ | Vị trí | Mô tả | Hướng sửa |
|---|---|---|---|---|
| (không có) | | | Không phát hiện lỗi đúng sai, bảo mật (không có đầu vào người dùng, không có XSS) hay thiếu unit test cho logic mới. | |

## Kết luận

APPROVE
