# 0005. Line ending LF trong repo, UTF-8, `.cshtml` có BOM

- Trạng thái: Accepted
- Ngày: 2026-10-02
- Người quyết định: Lê Khánh Trình

## Bối cảnh

- Dev dùng Windows (CRLF), CI chạy Ubuntu (LF). Mỗi commit đều có cảnh báo `LF will be replaced by CRLF`. Khi có thêm người, line ending lẫn lộn sẽ sinh diff rác.
- Nội dung blog là tiếng Việt có dấu. Encoding của file đang lẫn lộn: `.cshtml` có BOM, `.cs` và `.json` lúc có lúc không.
- Razor mã hóa ký tự có dấu thành entity (`&#x1EC7;`) khi in qua `@`. Trình duyệt hiển thị đúng, nhưng test so chuỗi trên HTML bị đỏ.

## Quyết định

- `.gitattributes` với `* text=auto`: trong repo luôn lưu LF; Git đổi sang line ending của hệ điều hành khi checkout. Script `.sh` cố định LF; `.cmd`, `.bat`, `.ps1` cố định CRLF; ảnh và font là binary.
- `.editorconfig`: UTF-8 không BOM cho mọi file, **trừ `.cshtml` dùng UTF-8 có BOM**. Không đặt `end_of_line` (để `.gitattributes` lo). Bỏ qua `wwwroot/lib`.
- CI chạy `dotnet format --verify-no-changes`.
- Chuỗi tiếng Việt cố định trong view viết thẳng trong `.cshtml`, không qua `@ViewData`, `@Model` hay biến C#.

## Phương án đã cân nhắc

- **`eol=lf` cho mọi file, kể cả working copy trên Windows**: đồng nhất tuyệt đối, nhưng buộc phải checkout lại toàn bộ file trên máy dev, và một số công cụ Windows vẫn ghi CRLF.
- **Cấu hình bộ mã hóa HTML trong `Program.cs` để giữ nguyên ký tự có dấu**: test so chuỗi dễ hơn, nhưng đổi cách mã hóa của toàn site. Chưa cần khi chuỗi cố định viết thẳng trong view.
- **UTF-8 có BOM cho mọi file**: đồng nhất, nhưng đi ngược mặc định của phần lớn công cụ và gây lỗi với một số script.

## Hệ quả

- Không còn diff do line ending. `git add --renormalize .` không đổi file nào vì repo vốn đã lưu LF.
- Ai quên chạy `dotnet format` sẽ thấy CI đỏ.
- Khi nội dung tiếng Việt đến từ database (qua `@Model`), test phải so trên văn bản đã giải mã HTML, hoặc phải xem lại phương án cấu hình bộ mã hóa.
