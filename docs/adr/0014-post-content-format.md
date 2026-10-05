# 0014. Định dạng nội dung bài

- Trạng thái: Accepted
- Ngày: 2026-10-05
- Người quyết định: Lê Khánh Trình (chọn phương án A ngày 2026-10-05, chờ duyệt để chuyển Accepted)

## Bối cảnh

- Vision để architect đề xuất định dạng nội dung bài (Markdown, trình soạn thảo trực quan...). Bài có ba loại: hướng dẫn kỹ thuật nhiều code, ghi chép ngắn, bài dài chủ yếu chữ. Tác giả là lập trình viên.
- `nfr.md`: khối code là văn bản, giữ thụt lề khi chép; màu tô sáng có tương phản ≥ 4,5:1; nội dung bài đọc được khi tắt JavaScript; trang ≤ 150 KB CSS và JS; nội dung người nhập không được chạy script (`<script>`, `onerror`, `javascript:`).
- `ui-guidelines.md`: khối code `<pre><code>` có nhãn ngôn ngữ, màu tô sáng theo token `--color-code-*`.

## Quyết định

Chọn **Phương án A: bài lưu dạng Markdown trong database, chuyển thành HTML ở server** bằng thư viện Markdig.

- Tắt HTML thô trong Markdown (Markdig `DisableHtml`), và chặn liên kết, ảnh có scheme khác `http`, `https`, `mailto` hoặc đường dẫn tương đối.
- Tô sáng cú pháp làm ở server, xuất class CSS ánh xạ sang token `--color-code-*`; không tải thư viện tô sáng bằng JavaScript. Thư viện cụ thể chốt sau (xem "Chưa quyết" của `architecture.md`).
- HTML kết quả có thể lưu kèm để không phải chuyển lại mỗi lần đọc (chốt trong feature hiển thị bài).
- Trang quản trị dùng ô `<textarea>` có xem trước, không dùng trình soạn thảo JavaScript nặng.

Lý do: Markdown là cách lập trình viên viết code trong bài tự nhiên nhất; chuyển ở server nên trang đọc không cần JavaScript, nhẹ, khối code là văn bản thật; tắt HTML thô làm giảm hẳn bề mặt XSS.

## Phương án đã cân nhắc

- **A. Markdown, chuyển ở server bằng Markdig (đã chọn).** Ưu: như trên; Markdig là thư viện Markdown phổ biến nhất của .NET, theo chuẩn CommonMark. Nhược: thêm một package; tác giả không thấy định dạng trực tiếp khi gõ (có xem trước).
- **B. Trình soạn thảo trực quan (kiểu Word), lưu HTML.** Ưu: thấy ngay định dạng. Nhược: phải tự host một thư viện soạn thảo JavaScript lớn cho trang quản trị; HTML do người nhập phải qua bộ lọc (thêm package lọc HTML); khối code kém tiện, dễ mất thụt lề khi dán.
- **C. Markdown, chuyển ở trình duyệt bằng JavaScript.** Ưu: không thêm package .NET. Nhược: tắt JavaScript thì không đọc được bài, trái `nfr.md`; tốn ngân sách 150 KB; công cụ tìm kiếm thấy chậm hơn.

## Đối chiếu hướng dẫn

- Markdig: https://github.com/xoofx/markdig (tùy chọn `DisableHtml`, các extension)
- CommonMark: https://commonmark.org/
- OWASP XSS Prevention Cheat Sheet: https://cheatsheetseries.owasp.org/cheatsheets/Cross_Site_Scripting_Prevention_Cheat_Sheet.html (theo đúng: không cho HTML tùy ý, kiểm scheme URL)

## Hệ quả

- Thêm package `Markdig` khi làm feature hiển thị hoặc soạn bài đầu tiên.
- Acceptance test XSS của `nfr.md` áp lên đầu ra của bộ chuyển Markdown.
- Ảnh trong bài (nếu câu hỏi mở 6 của vision trả lời "có") viết bằng cú pháp ảnh Markdown, bắt buộc có `alt` theo `ui-guidelines.md`.
- Xem lại khi: tác giả thấy soạn Markdown chậm, hoặc cần định dạng Markdown không hỗ trợ.
