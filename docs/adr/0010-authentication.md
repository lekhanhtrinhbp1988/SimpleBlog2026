# 0010. Đăng nhập cho tác giả và độc giả

- Trạng thái: Accepted
- Ngày: 2026-10-05
- Người quyết định: Lê Khánh Trình (chọn phương án A ngày 2026-10-05, chờ duyệt để chuyển Accepted)

## Bối cảnh

- Vision: tác giả đăng nhập trang quản trị; độc giả phải đăng nhập để bình luận, đọc bài không cần đăng nhập. Vision giảm rủi ro bằng cách "ưu tiên phương án không phải tự lưu mật khẩu". Hiện giả định chỉ một tác giả (câu hỏi mở 2 của vision).
- `nfr.md`: trang quản trị chỉ cho vai trò tác giả, độc giả gọi URL quản trị nhận 403; chống CSRF mọi form POST; nếu tự lưu mật khẩu thì dùng hàm băm của ASP.NET Core Identity và khóa 15 phút sau 5 lần sai. Chỉ thu thập dữ liệu cá nhân cần thiết; độc giả tự xóa được tài khoản.
- Độc giả chính là lập trình viên; gần như ai cũng có tài khoản GitHub. Ngân sách 0 đồng (đăng nhập GitHub miễn phí; gửi email xác nhận hay đặt lại mật khẩu thì cần dịch vụ email).

## Quyết định

Chọn **Phương án A: mọi người (tác giả và độc giả) đăng nhập bằng GitHub** (OAuth), phiên đăng nhập bằng cookie của ASP.NET Core. Tác giả là tài khoản GitHub có mã số (user id) ghi trong cấu hình; mọi tài khoản khác là độc giả. Bảng độc giả chỉ lưu mã GitHub, tên hiển thị và thời điểm tạo, không lưu email.

Lý do: không lưu mật khẩu nên không có việc băm, khóa tài khoản, quên mật khẩu, gửi email; ít dữ liệu cá nhân nhất (hợp Nghị định 13/2023); hợp với độc giả là lập trình viên; một cơ chế cho cả hai vai trò.

## Phương án đã cân nhắc

- **A. GitHub cho cả tác giả và độc giả (đã chọn).** Ưu: như trên; dùng handler OAuth có sẵn trong ASP.NET Core, không thêm package (nếu phần đọc thông tin người dùng quá dài thì cân nhắc package `AspNet.Security.OAuth.GitHub`). Nhược: người không có GitHub không bình luận được; GitHub sự cố thì không đăng nhập được, kể cả tác giả.
- **B. Tác giả dùng tài khoản riêng (ASP.NET Core Identity, mật khẩu), độc giả dùng GitHub.** Ưu: tác giả không phụ thuộc GitHub. Nhược: phải lưu mật khẩu, làm khóa tài khoản theo `nfr.md`, có hai cơ chế đăng nhập; thêm bảng Identity.
- **C. Tài khoản riêng cho mọi người (email và mật khẩu).** Ưu: ai cũng đăng ký được. Nhược: lưu email và mật khẩu của độc giả (nhiều dữ liệu cá nhân hơn); cần dịch vụ gửi email miễn phí cho xác nhận và quên mật khẩu; dễ bị tạo tài khoản rác; nhiều việc nhất.

## Đối chiếu hướng dẫn

Nguồn: tài liệu chính thức ASP.NET Core (cần mở lại khi thiết kế feature đăng nhập):

- Đăng nhập qua nhà cung cấp ngoài: https://learn.microsoft.com/aspnet/core/security/authentication/social/
- Cookie authentication không dùng Identity: https://learn.microsoft.com/aspnet/core/security/authentication/cookie
- Phân quyền theo vai trò và policy: https://learn.microsoft.com/aspnet/core/security/authorization/policies
- GitHub OAuth app: https://docs.github.com/apps/oauth-apps/building-oauth-apps/authorizing-oauth-apps

Theo đúng hướng dẫn: dùng cookie authentication và policy có sẵn. Khác: không dùng ASP.NET Core Identity (A) vì không có mật khẩu để quản lý; Identity chỉ cần khi chọn B hoặc C.

## Hệ quả

- Cần tạo hai GitHub OAuth app (dev và production) trên tài khoản của tác giả. Client secret production nằm trong cấu hình nơi deploy, secret dev trong user-secrets (`SECURITY.md`).
- Acceptance test không gọi GitHub thật: test đăng nhập bằng một scheme xác thực giả chỉ đăng ký trong `WebApplicationFactory`.
- Policy `Author` bảo vệ trang quản trị; nếu sau này có nhiều tác giả (câu hỏi mở 2 của vision) thì đổi cấu hình từ một mã thành danh sách.
- Xem lại khi: có độc giả phàn nàn không có GitHub, hoặc vision đổi sang nhiều tác giả có vai trò khác nhau.
