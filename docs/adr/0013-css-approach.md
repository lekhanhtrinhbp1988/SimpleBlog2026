# 0013. Cách viết CSS: Bootstrap, Tailwind hay CSS thuần theo design tokens

- Trạng thái: Accepted
- Ngày: 2026-10-05
- Người quyết định: Lê Khánh Trình (để architect đề xuất, rồi chọn phương án A ngày 2026-10-05; chờ duyệt để chuyển Accepted)

## Bối cảnh

- Câu hỏi mở 1 của `ui-guidelines.md` chuyển sang đây: giữ Bootstrap, dùng Tailwind, hay CSS thuần theo token. Người dùng để architect đề xuất.
- Hiện trạng: `_Layout.cshtml` tải Bootstrap 5 (CSS và JS bundle), jQuery, và có jquery-validation trong `wwwroot/lib` từ template `dotnet new mvc`.
- `nfr.md`: trang bài ≤ 150 KB CSS và JavaScript sau nén (gồm banner cookie); đọc được khi tắt JavaScript; LCP ≤ 2,5 s.
- `ui-guidelines.md`: một cột 720 px, không lưới, không menu ba gạch; mọi màu và cỡ chữ lấy từ biến CSS trong một file token; Stylelint cấm mã màu và cỡ chữ cứng ngoài file token.
- Ước lượng (sau nén gzip): Bootstrap CSS khoảng 30 KB, Bootstrap JS bundle khoảng 23 KB, jQuery khoảng 30 KB. Tổng khoảng 80 KB, hơn nửa ngân sách, mà bố cục một cột gần như không dùng tới.

## Quyết định

Chọn **Phương án A: CSS tự viết theo design tokens**, bỏ Bootstrap, jQuery và jquery-validation.

- `wwwroot/css/tokens.css`: chỉ khai báo biến trong `:root`, đúng bảng của `ui-guidelines.md`.
- `wwwroot/css/site.css`: reset tối thiểu, layout, các thành phần trong `ui-guidelines.md`; chỉ dùng `var(--...)`.
- Kiểm tra form ở server (ASP.NET Core model validation); không có kiểm tra phía trình duyệt bằng jQuery. Form vẫn chạy khi tắt JavaScript.
- JavaScript tự viết, nhỏ, chỉ cho phần cần (nút Sao chép, banner cookie).

Lý do: nhẹ nhất, khớp một-một với token đã duyệt, không có hệ màu và cỡ chữ thứ hai chồng lên, không thêm công cụ build. Thiết kế một cột với số thành phần ít nên viết tay không tốn nhiều.

## Phương án đã cân nhắc

- **A. CSS thuần theo token (đã chọn).** Ưu: nhẹ (ước lượng dưới 10 KB), không công cụ build, Stylelint kiểm thẳng. Nhược: tự viết reset và từng thành phần; không có sẵn thành phần như modal (blog hiện không cần).
- **B. Giữ Bootstrap, ghi đè biến của nó bằng token.** Ưu: có sẵn thành phần, nhiều người biết. Nhược: chiếm khoảng 80 KB ngân sách; ghi đè màu và cỡ chữ của Bootstrap cho khớp token tốn công và dễ sót; menu ba gạch và lưới trái `ui-guidelines.md`; muốn nhẹ phải tự build Sass từ mã nguồn, cần Node.
- **C. Tailwind CSS.** Ưu: CSS đầu ra nhỏ nhờ chỉ giữ class được dùng; token khai báo được trong cấu hình. Nhược: thêm bước build (Tailwind CLI) vào `dotnet build` và CI; class tiện ích dày đặc trong `.cshtml`; Stylelint khó áp luật "chỉ dùng biến"; thêm một công cụ phải học và cập nhật.

## Đối chiếu hướng dẫn

- Bootstrap, tối ưu dung lượng: https://getbootstrap.com/docs/5.3/customize/optimize/
- Tailwind standalone CLI: https://tailwindcss.com/blog/standalone-cli
- ASP.NET Core model validation: https://learn.microsoft.com/aspnet/core/mvc/models/validation
- CSS custom properties: https://developer.mozilla.org/docs/Web/CSS/Using_CSS_custom_properties

Số KB ở trên là ước lượng của agent; walking skeleton đo lại bằng Lighthouse.

## Hệ quả

- Walking skeleton xóa `wwwroot/lib/bootstrap`, `wwwroot/lib/jquery*`, `_ValidationScriptsPartial.cshtml` (hoặc làm rỗng), viết lại `_Layout.cshtml` và `site.css` theo `ui-guidelines.md`.
- Câu hỏi mở 1 của `ui-guidelines.md` được trả lời bởi ADR này; cập nhật bảng câu hỏi mở của file đó (qua PR, cập nhật `approved_on`).
- Mọi feature có form dựa vào kiểm tra phía server và thông báo lỗi theo mục Form của `ui-guidelines.md`.
- Xem lại khi: số thành phần giao diện tăng nhiều (ví dụ trang quản trị phức tạp) đến mức viết tay chậm hơn dùng thư viện.
