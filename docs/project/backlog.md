---
status: approved
approved_by: TRINH LE
approved_on: 2026-10-05
---

# Backlog

## Tóm tắt cho người duyệt

- Có 18 feature, đánh số F-00 đến F-17, xếp theo thứ tự nên làm. Feature đầu tiên chạy là `walking-skeleton`.
- Đường ngắn nhất tới bài đầu tiên: khung giao diện → hai ngôn ngữ → bạn đăng nhập → soạn bài → trang bài → danh sách bài → sitemap. Sau đó là mốc **"đăng bài đầu tiên công khai"** (deploy, sao lưu, tên miền, giám sát).
- Sau mốc đó: chính sách quyền riêng tư, banner cookie, analytics (để kịp đo số người đọc), rồi bản dịch bài, thẻ, RSS.
- Đăng nhập độc giả và bình luận làm sau, đúng như vision dặn (phạm vi lớn, có dữ liệu cá nhân). Tìm kiếm làm cuối vì khi còn ít bài thì ít cần.
- Mỗi feature ghi rõ câu hỏi nào của bạn phải trả lời trước khi bắt đầu. Các câu đó đã có trong vision và nfr, chưa cần trả lời ngay.
- Các việc kỹ thuật không phải feature (đổi sang PostgreSQL, CI, deploy...) nằm ở bảng riêng, gắn vào trước feature cần chúng.
- Chỉ còn một câu hỏi nhỏ của riêng file này (nội dung tiếng Anh của trang Giới thiệu), hạn chót trước `ui-localization`.

<!--
- Tên feature là đúng tên dùng cho /feature và thư mục docs/features/<tên>/ (tiếng Anh, kebab-case).
- F-00 luôn là walking-skeleton và phải done trước mọi feature nội dung.
- Trạng thái: todo | in-progress | done.
- Mã F-NN không đổi, không dùng lại.
-->

Viết tắt trong cột câu hỏi: "vision CH2" là câu hỏi mở 2 của `vision.md`; "nfr CH5" là câu hỏi mở 5 của `nfr.md`; "Chưa quyết: ..." là mục trong "Chưa quyết" của `architecture.md` (cần ADR trước khi làm).

| Mã | Tên feature | Mô tả | Phụ thuộc | Câu hỏi mở cần trả lời trước | Trạng thái | PR |
|---|---|---|---|---|---|---|
| F-00 | `walking-skeleton` | Layout chung theo ui-guidelines.md, CSS nền từ design tokens, `public partial class Program` | — | — | todo | |
| F-01 | `about` | Trang Giới thiệu | — | — | done | #1 |
| F-02 | `ui-localization` | Người đọc chọn được tiếng Việt hoặc tiếng Anh; URL có `/vi`, `/en`; mọi chuỗi giao diện có đủ hai ngôn ngữ; trang Giới thiệu có ở cả hai và `/Home/About` chuyển 301 | F-00, F-01 | vision CH7 (ngôn ngữ khi vào lần đầu); câu hỏi 1 bên dưới (trang Giới thiệu tiếng Anh) | todo | |
| F-03 | `admin-auth` | Tác giả đăng nhập bằng GitHub để vào trang quản trị; người khác vào `/admin` bị chặn | F-02 | vision CH2 (một hay nhiều tác giả) | todo | |
| F-04 | `post-editor` | Tác giả soạn, sửa, đăng bài bằng Markdown trên trang quản trị, đặt slug cho bài | F-03 | vision CH6 (có tải ảnh lên không); Chưa quyết: nơi lưu ảnh (nếu CH6 là có) | todo | |
| F-05 | `post-detail` | Người đọc mở một bài tại `/{lang}/posts/{slug}`, đọc chữ và code có tô sáng, chép code được | F-04 | Chưa quyết: thư viện tô sáng cú pháp | todo | |
| F-06 | `post-list` | Người đọc thấy danh sách bài mới nhất ở trang chủ, có phân trang | F-05 | — | todo | |
| F-07 | `sitemap` | Công cụ tìm kiếm tìm thấy mọi bài qua `sitemap.xml` và `robots.txt` (trang quản trị bị chặn) | F-05, F-06 | — | todo | |
| F-08 | `privacy-policy` | Người đọc đọc được trang chính sách quyền riêng tư song ngữ, có liên kết ở chân mọi trang | F-02 | vision CH5 (`nfr.md` đã chốt: Google Analytics có banner); nfr CH6 (hồ sơ chuyển dữ liệu ra nước ngoài) | todo | |
| F-09 | `cookie-consent` | Người đọc chọn "Đồng ý" hoặc "Từ chối" cookie analytics trên banner, đổi ý được qua liên kết "Cài đặt cookie" ở chân trang | F-08 | — | todo | |
| F-10 | `analytics` | Tác giả đếm được số người đọc duy nhất mỗi tháng; Google Analytics chỉ tải khi người đọc đã đồng ý | F-09 | vision CH1 (mức chỉ số thành công); nfr CH5 (thời gian lưu dữ liệu); nfr CH6; Chưa quyết: ADR Google Analytics | todo | |
| F-11 | `post-translations` | Người đọc chuyển sang bản dịch của bài (nếu có); tác giả thêm và sửa bản dịch trên trang quản trị | F-04, F-05, F-06 | vision CH3 (bài chưa có bản dịch hiện thế nào) | todo | |
| F-12 | `tags` | Người đọc thấy thẻ của bài và xem mọi bài cùng một thẻ; tác giả gắn thẻ khi soạn bài | F-04, F-05, F-06 | vision CH8 (tìm kiếm, RSS, thẻ tách theo ngôn ngữ hay dùng chung) | todo | |
| F-13 | `rss` | Người đọc theo dõi bài mới bằng trình đọc RSS | F-06 | vision CH8 | todo | |
| F-14 | `reader-auth` | Người đọc đăng nhập bằng GitHub và tự xóa được tài khoản của mình | F-03, F-08 | vision CH5 | todo | |
| F-15 | `comments` | Người đọc đã đăng nhập bình luận dưới bài, tự xóa được bình luận của mình; tối đa 5 bình luận mỗi 10 phút | F-05, F-14 | vision CH4 (cách kiểm duyệt bình luận) | todo | |
| F-16 | `comment-moderation` | Tác giả xem, duyệt (nếu có duyệt trước) và xóa bình luận trên trang quản trị | F-03, F-15 | vision CH4 | todo | |
| F-17 | `search` | Người đọc tìm bài theo từ khóa | F-06 | vision CH8; Chưa quyết: cách tìm kiếm | todo | |

Ghi chú về thứ tự và phạm vi:

- `ui-localization` đứng ngay sau `walking-skeleton` để mọi feature sau dùng chung route `{lang}` và localizer (vision, rủi ro đa ngôn ngữ; ADR-0011).
- `admin-auth` chưa cần database (tác giả xác định bằng mã GitHub trong cấu hình, ADR-0010). Feature đầu tiên dùng database là `post-editor`.
- `sitemap`, `privacy-policy`, `cookie-consent`, `comment-moderation` không có tên riêng trong "Phạm vi" của vision nhưng là phần bắt buộc của các mục trong phạm vi: SEO cho danh sách và chi tiết bài (`nfr.md`), dữ liệu cá nhân (vision, Ràng buộc; `nfr.md`), "quản lý bình luận" trong trang quản trị (vision, Phạm vi).
- Thẻ `<title>`, `meta description`, `canonical`, Open Graph, `hreflang` và header bảo mật không phải feature riêng: mỗi feature hiển thị trang tự đáp ứng theo `nfr.md`.
- Giả định: `analytics` làm trước `tags` và bản dịch để có số liệu sớm cho chỉ số 500 người đọc/tháng (đánh giá ở tháng thứ 6 sau bài đầu tiên). Người duyệt muốn đổi thứ tự thì sửa trước khi gõ `duyệt`.

### Mốc "đăng bài đầu tiên công khai"

Sau F-00 đến F-07, trước khi bài đầu tiên lên production. Các việc trong bảng dưới có cột hạn chót "mốc bài đầu tiên" phải xong ở đây.

### Việc chuẩn bị (không phải feature)

Theo bảng "Việc phải làm theo các quyết định" và "Chưa quyết" của `architecture.md`. Mỗi dòng là một PR `chore` (hoặc nằm trong feature được ghi), không chạy qua `/feature`.

| Việc | Theo | Hạn chót |
|---|---|---|
| Sửa `vision.md` (bỏ SQL Server LocalDB), đóng câu hỏi Bootstrap trong `ui-guidelines.md`, thêm thuật ngữ mới vào `docs/glossary.md`, dòng trạng thái ADR-0003 | ADR-0008, 0012, 0013 | Cùng PR tài liệu nền hoặc ngay sau |
| Xóa Bootstrap, jQuery; viết lại `_Layout.cshtml`, `site.css`, thêm `tokens.css` | ADR-0013 | Trong F-00 `walking-skeleton` |
| CI: cài trình duyệt Playwright và Stylelint (`package.json`) | ADR-0015 | Trước hoặc trong F-00 `walking-skeleton` (đề xuất: layout đã cần kiểm focus, tương phản, CSS) |
| Kiểm lại agent `ux` (còn nhắc Bootstrap) | ADR-0013 | Sau F-00 `walking-skeleton` |
| `CLAUDE.md` mục "Vietnamese text in Razor" khi bật `WebEncoderOptions` | ADR-0011 | Trong F-02 `ui-localization` |
| Đổi luật database cho agent (`ba`, `architect`, `developer`, `reviewer`, `tester`) và mục Database, Current state của `CLAUDE.md` sang PostgreSQL, `simpleblog_test` | ADR-0008, 0012 | Trước F-04 `post-editor` |
| Đổi provider sang Npgsql; connection string dev bằng user-secrets; CI có `services: postgres`; cập nhật `README.md`, `SECURITY.md`, `CONTRIBUTING.md`, `docs/process-roadmap.md` mục 4.1 | ADR-0008, 0012 | Trước F-04 `post-editor` |
| ADR nơi lưu ảnh (nếu vision CH6 là có) | architecture, Chưa quyết | Trước F-04 `post-editor` |
| ADR thư viện tô sáng cú pháp | architecture, Chưa quyết | Trước F-05 `post-detail` |
| CI: job Lighthouse CI với dữ liệu mẫu | ADR-0015, `nfr.md` | Trước F-05 `post-detail` (trang đầu tiên có mức hiệu năng phải đạt) |
| Kiểm lại điều khoản gói miễn phí Neon và App Service F1; workflow deploy bằng OIDC; dịch vụ giám sát (ADR) | ADR-0009, architecture | Mốc bài đầu tiên |
| ADR tên miền riêng | architecture, Chưa quyết | Mốc bài đầu tiên |
| ADR sao lưu hằng ngày; thử khôi phục một lần | architecture, Chưa quyết; `nfr.md` | Mốc bài đầu tiên |
| Đo thời gian thức dậy sau deploy (≤ 15 s), ghi vào `architecture.md` | `nfr.md`, architecture | Ngay sau lần deploy đầu tiên |
| ADR cách tìm kiếm | architecture, Chưa quyết | Trước F-17 `search` |
| ADR Google Analytics | architecture, Chưa quyết | Trước F-10 `analytics` |

## Câu hỏi mở

<!-- Mỗi dòng: câu hỏi | hạn chót ("trước feature <tên>" hoặc YYYY-MM-DD) | ai trả lời. Xóa dòng khi đã trả lời và ghi kết quả vào mục tương ứng ở trên. -->

Câu hỏi của từng feature nằm ở cột "Câu hỏi mở cần trả lời trước" (đã có hạn chót và người trả lời trong `vision.md`, `nfr.md`, `architecture.md`). Dưới đây chỉ là câu hỏi riêng của backlog.

| Câu hỏi | Hạn chót | Ai trả lời |
|---|---|---|
| 1. Trang Giới thiệu tiếng Anh lấy nội dung từ đâu? (A, đề xuất: bạn tự viết bản tiếng Anh trước khi làm feature / B: tạm để trang tiếng Anh hiện nội dung tiếng Việt kèm ghi chú, viết sau / C: khác) | Trước feature `ui-localization` | Lê Khánh Trình |
