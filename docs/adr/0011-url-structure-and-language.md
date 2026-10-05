# 0011. Cấu trúc URL và cách chọn ngôn ngữ

- Trạng thái: Accepted
- Ngày: 2026-10-05
- Người quyết định: Lê Khánh Trình (chọn phương án A ngày 2026-10-05, chờ duyệt để chuyển Accepted)

## Bối cảnh

- Blog song ngữ Việt và Anh; mỗi bài một ngôn ngữ, có thể có bản dịch (vision). `nfr.md` yêu cầu: `lang` đúng ngôn ngữ trang, `hreflang` giữa bài và bản dịch, `canonical`, sitemap; URL của bài không đổi khi sửa tiêu đề, nếu buộc đổi thì 301 từ URL cũ.
- Độc giả đến từ công cụ tìm kiếm. Google khuyên mỗi ngôn ngữ một URL riêng, không đổi ngôn ngữ theo cookie trên cùng một URL.
- URL công khai là thứ đắt nhất để đổi về sau (liên kết đã chia sẻ, chỉ mục tìm kiếm).
- Hiện có route mặc định `{controller=Home}/{action=Index}/{id?}` và trang `/Home/About`.
- Ngôn ngữ mặc định khi vào lần đầu là câu hỏi mở 7 của vision (hạn chót trước feature `ui-localization`).

## Quyết định

Chọn **Phương án A: tiền tố ngôn ngữ cho cả hai ngôn ngữ**.

- Trang công khai: `/vi/...` và `/en/...`. Đoạn đường dẫn viết tiếng Anh, chữ thường, dùng chung cho hai ngôn ngữ: `/vi/posts/{slug}`, `/en/posts/{slug}`, `/vi/tags/{tag}`, `/vi/about`.
- Slug: tác giả đặt khi tạo bài (gợi ý tự động từ tiêu đề, bỏ dấu, nối bằng gạch ngang), **không tự đổi khi sửa tiêu đề**. Nếu tác giả đổi slug, slug cũ được giữ lại và chuyển hướng 301 sang slug mới. Bản dịch có slug riêng trong ngôn ngữ của nó.
- `/` chuyển hướng tới `/vi/` hoặc `/en/` theo câu trả lời câu hỏi mở 7 của vision.
- Không có tiền tố ngôn ngữ: trang quản trị `/admin/...` (ngôn ngữ giao diện theo lựa chọn gần nhất lưu trong cookie, `noindex`), đăng nhập `/account/...`, `/sitemap.xml`, `/robots.txt`. Vị trí RSS chốt cùng câu hỏi mở 8 của vision.
- URL cũ `/Home/About` chuyển 301 tới trang Giới thiệu mới.
- Ngôn ngữ giao diện dùng cơ chế localization có sẵn của ASP.NET Core: lấy văn hóa từ đoạn `{lang}` của route, chuỗi giao diện trong file `.resx` cho `vi` và `en`.

Lý do: hai ngôn ngữ đối xứng nên một bảng route, một quy tắc `hreflang`; dễ đọc, dễ kiểm bằng test; đúng khuyến nghị của Google.

## Phương án đã cân nhắc

- **A. Tiền tố cho cả hai: `/vi/posts/{slug}`, `/en/posts/{slug}` (đã chọn).** Ưu: như trên. Nhược: URL tiếng Việt dài thêm 3 ký tự; trang chủ `/` phải chuyển hướng.
- **B. Tiếng Việt không tiền tố, tiếng Anh có `/en`: `/posts/{slug}`, `/en/posts/{slug}`.** Ưu: URL tiếng Việt ngắn, `/` là trang chủ tiếng Việt luôn. Nhược: hai ngôn ngữ không đối xứng, route và test phải xử lý hai trường hợp; nếu sau này đổi ngôn ngữ mặc định thì đổi toàn bộ URL.
- **C. Một URL cho cả hai, ngôn ngữ theo cookie hoặc trình duyệt.** Ưu: ít route nhất. Nhược: công cụ tìm kiếm chỉ thấy một ngôn ngữ, không có `hreflang` đúng nghĩa, trái `nfr.md`. Không khuyên dùng.
- **Slug kèm mã số (`/vi/posts/123/{slug}`)** thay cho slug cố định: không cần lưu slug cũ (mã không đổi, slug sai thì 301), nhưng URL kém đẹp. Giữ làm phương án dự phòng nếu bảng slug cũ thành phức tạp.

## Đối chiếu hướng dẫn

- Google, trang đa ngôn ngữ: https://developers.google.com/search/docs/specialty/international/managing-multi-regional-sites (khuyên URL riêng cho từng ngôn ngữ, không dựa cookie) và `hreflang`: https://developers.google.com/search/docs/specialty/international/localized-versions
- ASP.NET Core localization, `RouteDataRequestCultureProvider`: https://learn.microsoft.com/aspnet/core/fundamentals/localization

## Hệ quả

- Feature `ui-localization` (hoặc walking skeleton, theo backlog) dựng route có `{lang}`, localization và chuyển hướng `/`; mọi feature sau dùng chung.
- Chuỗi giao diện in qua localizer bị Razor mã hóa thành `&#x...;`; cần cấu hình `WebEncoderOptions` cho phép ký tự Unicode để HTML chứa chữ tiếng Việt thật (khi đó cập nhật mục "Vietnamese text in Razor" của `CLAUDE.md`).
- Cần bảng lưu slug cũ để 301 khi tác giả đổi slug.
- Đổi cấu trúc này sau khi đã đăng bài là tốn kém (mọi URL cũ cần 301); xem lại chỉ khi có lý do SEO rõ ràng.
