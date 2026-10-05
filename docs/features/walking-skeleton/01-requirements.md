# 01 - Requirements: walking-skeleton

## Bối cảnh

`walking-skeleton` là F-00 trong `docs/project/backlog.md`. Mọi feature nội dung khác phụ thuộc vào nó. Mô tả trong backlog: "Layout chung theo ui-guidelines.md, CSS nền từ design tokens, `public partial class Program`". Bảng "Việc chuẩn bị" của backlog gắn thêm hai việc vào F-00:

- Xóa Bootstrap và jQuery, viết lại layout chung và CSS, thêm file token (ADR-0013).
- CI cài trình duyệt Playwright và chạy Stylelint (ADR-0015). Backlog ghi "trước hoặc trong F-00" và đề xuất làm trong F-00, vì layout đã cần kiểm focus, tương phản và CSS. Tài liệu này theo đề xuất đó.

Hiện app vẫn gần với khung mẫu `dotnet new mvc`: giao diện Bootstrap, và một trang thật duy nhất là trang Giới thiệu (feature `about`, F-01, đã xong). Sau feature này, độc giả mở bất kỳ trang nào cũng thấy giao diện một cột kiểu Substack như `ui-guidelines.md` mô tả: header tối giản với tên blog "Trình's Dev Notes", chữ hệ thống, màu theo token, dùng được hoàn toàn bằng bàn phím. Trang không tải thư viện nào từ bên ngoài. Trang Giới thiệu vẫn hoạt động như trước, chỉ đổi giao diện.

Các câu hỏi mở của bản nháp trước (CH-1 đến CH-6) đã được người dùng trả lời ngày 2026-10-05. Câu trả lời ghi ở mục "Giả định", phần "Quyết định của người dùng", và đã đưa vào các AC.

Bổ sung 2026-10-05, sau khi người dùng mở app xem bằng mắt (PR #23 đang mở): thêm hai quy định về vị trí footer và vòng focus của vùng nội dung chính (AC-45 đến AC-48), và làm rõ phạm vi của AC-14. Các AC khác giữ nguyên. `ui-guidelines.md` chưa có hai quy định này (xem "Quyết định của người dùng", mục 7 và 8).

## Phạm vi

### Trong phạm vi

- Layout chung cho mọi trang công khai, theo mục Layout và Thành phần của `ui-guidelines.md`:
  - một cột rộng tối đa 720 px, căn giữa;
  - header gồm tên blog "Trình's Dev Notes" và menu chính, với liên kết "Bỏ qua tới nội dung chính";
  - footer gồm dòng bản quyền "© <năm hiện tại> Lê Khánh Trình";
  - khi nội dung trang ngắn hơn khung nhìn, footer nằm sát đáy khung nhìn; khi nội dung dài hơn, footer nằm ngay sau nội dung và cuộn theo trang (bổ sung 2026-10-05).
- Vùng nội dung chính không hiện vòng focus khi nhận focus từ liên kết "Bỏ qua tới nội dung chính"; mọi liên kết và nút vẫn có vòng focus (bổ sung 2026-10-05).
- Menu chính chỉ có mục "Giới thiệu", vì đây là trang duy nhất đã có. `ui-guidelines.md` quy định mục nào có thì hiện khi feature tương ứng xong.
- CSS nền:
  - file token chứa đúng bảng token của `ui-guidelines.md`;
  - CSS của site chỉ dùng biến token;
  - font hệ thống, cỡ chữ theo `rem`, trạng thái focus, vùng bấm, breakpoint `md` và `lg`.
- Gỡ Bootstrap, jQuery và jquery-validation khỏi site (ADR-0013).
- Trang Giới thiệu hiện có hiển thị trong layout mới, giữ nguyên nội dung.
- Trang chủ hiển thị trong layout mới, với tiêu đề "Bài viết" và câu "Chưa có bài viết nào." cho tới khi F-06 thêm danh sách bài.
- Gỡ trang Privacy của khung mẫu; địa chỉ cũ trả 404.
- Acceptance test khởi động app trực tiếp, không cần cách lách bằng reflection (`public partial class Program`, ghi trong backlog).
- CI chạy test trình duyệt (Playwright, axe-core) và Stylelint cho mọi PR (ADR-0015).
- Thuộc tính `lang`, `<title>` và `meta description` cho các trang hiện có.
- Header bảo mật cho mọi phản hồi: `Content-Security-Policy` không cho script inline, `X-Content-Type-Options`, `Referrer-Policy` (theo `nfr.md`).

### Ngoài phạm vi

- Chuyển ngôn ngữ VI/EN, URL có `/vi` và `/en`, chuỗi giao diện trong file tài nguyên, chuyển hướng 301 từ `/Home/About`. Các việc này thuộc F-02 `ui-localization`, feature phụ thuộc F-00. Vì vậy trong feature này chữ giao diện vẫn viết thẳng bằng tiếng Việt, theo mục "Vietnamese text in Razor" của `CLAUDE.md`. Mức "0 chuỗi viết cứng" của `nfr.md` được đáp ứng ở F-02.
- Liên kết "Chính sách quyền riêng tư" và "Cài đặt cookie" ở footer, và banner cookie. Các việc này thuộc F-08 `privacy-policy` và F-09 `cookie-consent`. Liên kết RSS thuộc F-13.
- Mục menu "Bài viết", "Thẻ", "Tìm kiếm". Mỗi mục được thêm cùng feature tương ứng (F-06, F-12, F-17).
- Danh sách bài, trang chi tiết bài, khối code, thẻ, form, nút, ảnh. Các thành phần này làm cùng feature đầu tiên cần tới chúng.
- `link rel="canonical"` và thẻ Open Graph. Hai thứ này cần URL đầy đủ, mà URL đổi ở F-02; chuyển sang F-02 `ui-localization` (người dùng trả lời 2026-10-05).
- Lighthouse CI và mức hiệu năng, dung lượng trang của `nfr.md`. Backlog đặt Lighthouse CI trước F-05 `post-detail`.
- Database, đăng nhập, deploy, HTTPS và HSTS trên production (mốc "đăng bài đầu tiên công khai").
- Chế độ tối.
- Sửa agent `ux`. Đây là việc chuẩn bị riêng, làm sau F-00.
- Sửa `vision.md`, `ui-guidelines.md`, `docs/glossary.md` theo ADR-0008, 0012, 0013. Đây là việc chuẩn bị riêng của PR tài liệu nền.
- Đổi tên project, solution, repo hay namespace `SimpleBlog`. Tên "Trình's Dev Notes" chỉ là tên hiển thị cho độc giả.
- Bổ sung vào `ui-guidelines.md` hai quy định mới của AC-45 đến AC-48 (footer sát đáy khi nội dung ngắn; vùng nội dung chính không vẽ vòng focus). Đề xuất làm bằng PR riêng sửa tài liệu nền, cập nhật `approved_on`.
- Footer dính cố định ở đáy khung nhìn khi cuộn. Người dùng đã chọn không làm (AC-46).

## Acceptance criteria

Trong các AC dưới đây:

- "Các trang hiện có" là trang chủ và trang Giới thiệu.
- "Các độ rộng" là 320, 360, 768, 1280 và 1920 CSS px.
- "Ba trình duyệt" là Chromium, Firefox và WebKit.
- "Tên blog" là chuỗi `Trình's Dev Notes`, với dấu nháy đơn thẳng (U+0027), không phải dấu nháy cong (U+2019).
- Mọi AC so sánh chữ (tên blog, `<title>`, tiêu đề, câu, dòng bản quyền) so trên **chữ người đọc thấy**, tức là sau khi trình duyệt hoặc trình phân tích HTML đã giải mã HTML entity. Trong mã HTML thô, dấu nháy đơn có thể xuất hiện dưới dạng `'`, `&#x27;` hay `&#39;`, và chữ tiếng Việt có thể ở dạng entity; cả hai đều đạt. Không đạt nếu người đọc thấy chính chuỗi entity (ví dụ `Trình&#x27;s Dev Notes` hiện nguyên văn, do bị mã hóa hai lần).

### Bố cục

### AC-1: Một cột rộng tối đa 720 px

- Given mở một trong các trang hiện có ở độ rộng 1280 px hoặc 1920 px
- When đo vùng nội dung của header, nội dung chính và footer
- Then mỗi vùng rộng không quá 720 px và căn giữa màn hình

### AC-2: Không có cột thứ hai

- Given mở một trong các trang hiện có ở bất kỳ độ rộng nào trong các độ rộng
- When xem bố cục trang
- Then không có cột bên hay khối nào nằm cạnh nội dung chính

### AC-3: Không cuộn ngang cả trang

- Given mở một trong các trang hiện có trên một trong ba trình duyệt, ở một trong các độ rộng
- When trang tải xong
- Then chiều rộng cuộn của trang không vượt quá chiều rộng khung nhìn

### Header và menu

### AC-4: Tên blog dẫn về trang chủ

- Given đang ở trang Giới thiệu
- When bấm tên blog "Trình's Dev Notes" ở header
- Then trang chủ mở ra

### AC-5: Menu có mục Giới thiệu

- Given đang ở trang chủ
- When bấm mục "Giới thiệu" trong menu chính
- Then trang Giới thiệu mở ra

### AC-6: Menu không có mục của feature chưa làm

- Given mở một trong các trang hiện có
- When xem menu chính
- Then menu chỉ có mục "Giới thiệu", không có "Bài viết", "Thẻ", "Tìm kiếm" hay liên kết nào khác của khung mẫu (ví dụ "Home", "Privacy")

### AC-7: Đánh dấu mục menu của trang hiện tại

- Given đang ở trang Giới thiệu
- When xem mục "Giới thiệu" trong menu
- Then mục đó được đánh dấu là trang hiện tại cho trình đọc màn hình (`aria-current="page"`) và có gạch chân màu nhấn, còn ở trang chủ thì mục đó không được đánh dấu

### AC-8: Header một hàng trên màn hình rộng

- Given mở một trong các trang hiện có ở độ rộng từ 768 px trở lên
- When xem header
- Then tên blog nằm bên trái và menu nằm bên phải trên cùng một hàng

### AC-9: Header xuống dòng trên màn hình hẹp

- Given mở một trong các trang hiện có ở độ rộng 320 px hoặc 360 px
- When xem header
- Then menu nằm dưới tên blog, và không có nút ba gạch

### AC-10: Header không dính khi cuộn

- Given mở một trang có nội dung dài hơn màn hình
- When cuộn xuống cuối trang
- Then header cuộn đi cùng trang, không còn nằm trong khung nhìn

### Bàn phím và tiếp cận

### AC-11: Liên kết bỏ qua là phần tử đầu tiên nhận focus

- Given vừa mở một trong các trang hiện có
- When bấm Tab lần đầu
- Then liên kết "Bỏ qua tới nội dung chính" nhận focus và hiện ra

### AC-12: Liên kết bỏ qua ẩn khi không có focus

- Given mở một trong các trang hiện có và chưa bấm phím nào
- When xem trang
- Then liên kết "Bỏ qua tới nội dung chính" không nhìn thấy được

### AC-13: Liên kết bỏ qua chuyển tới nội dung chính

- Given liên kết "Bỏ qua tới nội dung chính" đang có focus
- When bấm Enter
- Then focus chuyển tới vùng nội dung chính

### AC-14: Focus luôn nhìn thấy

- Given mở một trong các trang hiện có
- When bấm Tab lần lượt qua mọi phần tử bấm được (liên kết và nút; vùng nội dung chính không phải phần tử bấm được, xem AC-48)
- Then mỗi phần tử khi có focus đều có viền focus dày ít nhất 2 px, và phần tử đó nằm trong khung nhìn

### AC-48: Vùng nội dung chính không vẽ vòng focus

- Given vừa dùng liên kết "Bỏ qua tới nội dung chính", focus đang ở vùng nội dung chính
- When xem vùng nội dung chính
- Then không có vòng focus hay viền nào bao quanh vùng nội dung chính

### AC-15: Vùng bấm đủ lớn

- Given mở một trong các trang hiện có ở độ rộng 360 px và 1280 px
- When đo các phần tử bấm được trong header và footer
- Then mục menu cao ít nhất 44 px, và mọi liên kết khác rộng và cao ít nhất 24 px

### AC-16: Không vi phạm tiếp cận

- Given mở một trong các trang hiện có ở độ rộng 360 px và 1280 px
- When chạy axe-core với các luật gắn tag `wcag2a`, `wcag2aa`, `wcag21aa`, `wcag22aa`
- Then số vi phạm là 0

### AC-17: Menu dùng được khi tắt JavaScript

- Given trình duyệt tắt JavaScript, độ rộng 360 px
- When mở trang chủ và bấm mục "Giới thiệu"
- Then mục menu hiển thị, bấm được, và trang Giới thiệu mở ra

### AC-18: Nội dung đọc được khi tắt JavaScript

- Given trình duyệt tắt JavaScript
- When mở trang Giới thiệu
- Then toàn bộ nội dung của trang hiển thị

### Chữ

### AC-19: Font hệ thống

- Given mở một trong các trang hiện có
- When đọc font tính được của phần thân trang
- Then danh sách font bắt đầu bằng `system-ui`, và trang không tải file font nào

### AC-20: Cỡ chữ gốc không bị thu nhỏ

- Given mở một trong các trang hiện có ở một trong các độ rộng
- When đọc cỡ chữ tính được của phần tử gốc `html`
- Then cỡ chữ là 16 px

### AC-21: Không có chữ nhỏ hơn 14 px

- Given mở một trong các trang hiện có ở độ rộng 360 px và 1280 px
- When đo cỡ chữ của mọi phần tử chứa chữ đang hiển thị
- Then không phần tử nào có cỡ chữ dưới 14 px

### AC-22: Đoạn văn 18 px, dòng thưa

- Given mở trang Giới thiệu
- When đo một đoạn văn trong nội dung chính
- Then cỡ chữ là 18 px và chiều cao dòng ít nhất 1,7 lần cỡ chữ

### AC-23: Tiêu đề trang theo breakpoint

- Given mở trang Giới thiệu
- When đo tiêu đề chính của trang ở độ rộng 1280 px rồi ở 360 px
- Then tiêu đề cỡ 36 px ở 1280 px và 30 px ở 360 px, độ đậm 700 ở cả hai

### Tài nguyên và token

### AC-24: Không tải tài nguyên từ bên ngoài

- Given mở một trong các trang hiện có
- When ghi lại mọi yêu cầu mạng của trang
- Then mọi yêu cầu đều tới chính site

### AC-25: Không còn Bootstrap và jQuery

- Given mở một trong các trang hiện có
- When ghi lại mọi file CSS và JavaScript trang tải
- Then không có file nào của Bootstrap, jQuery hay jquery-validation

### AC-26: Token đúng bảng đã duyệt

- Given file token của site
- When so từng token với bảng Màu, Chữ, Khoảng cách và bo góc trong `ui-guidelines.md`
- Then file có đủ mọi token của các bảng, mỗi token đúng giá trị trong bảng

### AC-27: Mọi cặp màu đủ tương phản

- Given các giá trị màu trong file token
- When tính tỉ lệ tương phản của từng cặp chữ và nền ghi trong bảng Màu của `ui-guidelines.md`
- Then mỗi cặp đạt mức của bảng: ít nhất 4,5:1 với chữ, ít nhất 3:1 với viền điều khiển và vòng focus

### AC-28: CSS của site qua kiểm tra Stylelint

- Given CSS hiện có của site
- When chạy Stylelint với cấu hình của dự án
- Then không có lỗi

### AC-29: Stylelint chặn màu và cỡ chữ viết cứng

- Given một file CSS không phải file token chứa mã màu hex, hoặc chứa `font-size` tính bằng `px`
- When chạy Stylelint với cấu hình của dự án
- Then Stylelint báo lỗi cho dòng đó

### CI

### AC-30: CI chạy test trình duyệt

- Given một PR vào `main`
- When CI chạy
- Then các test trình duyệt (Playwright, gồm axe-core) chạy trên ba trình duyệt, và CI đỏ nếu một test trình duyệt đỏ

### AC-31: CI chạy Stylelint

- Given một PR vào `main` có CSS vi phạm luật Stylelint của dự án
- When CI chạy
- Then CI đỏ

### Trang hiện có

### AC-32: Trang Giới thiệu giữ nguyên nội dung

- Given layout mới đã thay layout cũ
- When chạy các acceptance test hiện có của feature `about`
- Then mọi test đều đạt

### AC-33: Trang chủ có tiêu đề và câu giải thích

- Given chưa có bài viết nào
- When mở trang chủ
- Then trang có tiêu đề chính "Bài viết" và câu "Chưa có bài viết nào.", không còn nội dung chào mừng của khung mẫu

### AC-34: Trang Privacy của khung mẫu không còn

- Given site đã có layout mới
- When mở địa chỉ trang Privacy của khung mẫu
- Then nhận mã 404

### AC-35: Acceptance test khởi động app trực tiếp

- Given project test
- When một acceptance test khởi động app để gửi yêu cầu
- Then test dùng trực tiếp lớp `Program` của app, không dùng cách lách bằng reflection đang ghi trong `CLAUDE.md`

### Footer

### AC-36: Dòng bản quyền ở footer

- Given mở một trong các trang hiện có
- When xem footer
- Then footer có dòng "© <năm hiện tại> Lê Khánh Trình", chữ 14 px màu `--color-text-muted`

### AC-45: Footer sát đáy khung nhìn khi nội dung ngắn

- Given mở trang chủ ở khung nhìn 360 × 800 px hoặc 1280 × 800 px, trên một trong ba trình duyệt
- When trang tải xong, chưa cuộn
- Then cạnh dưới của footer trùng cạnh dưới khung nhìn (lệch không quá 1 px)

### AC-46: Footer không dính khi nội dung dài

- Given mở một trang có nội dung cao hơn khung nhìn (ví dụ trang Giới thiệu với khung nhìn đủ thấp)
- When trang tải xong, chưa cuộn
- Then footer không nằm trong khung nhìn

### AC-47: Footer không chồng lên nội dung chính

- Given mở một trong các trang hiện có ở độ rộng 360 px và 1280 px, với khung nhìn cao hơn nội dung và với khung nhìn thấp hơn nội dung
- When đo vị trí vùng nội dung chính và footer
- Then cạnh trên của footer nằm ở dưới hoặc trùng cạnh dưới của vùng nội dung chính

### SEO

### AC-37: Ngôn ngữ trang là tiếng Việt

- Given mở một trong các trang hiện có
- When đọc thuộc tính `lang` của phần tử `html`
- Then giá trị là `vi`

### AC-38: Tiêu đề trình duyệt riêng cho từng trang

- Given mở trang chủ rồi trang Giới thiệu
- When đọc `<title>` của từng trang, sau khi giải mã HTML entity
- Then trang chủ có `<title>` "Trình's Dev Notes", trang Giới thiệu có "Giới thiệu - Trình's Dev Notes"

### AC-39: Mô tả trang riêng cho từng trang

- Given mở trang chủ rồi trang Giới thiệu
- When đọc `meta description` của từng trang
- Then mỗi trang có mô tả không rỗng, và hai mô tả khác nhau

### Header bảo mật

### AC-40: Chặn đoán kiểu nội dung

- Given gửi yêu cầu tới một trong các trang hiện có, hoặc tới file CSS của site
- When đọc header của phản hồi
- Then có `X-Content-Type-Options: nosniff`

### AC-41: Chính sách referrer

- Given gửi yêu cầu tới một trong các trang hiện có
- When đọc header của phản hồi
- Then có `Referrer-Policy: strict-origin-when-cross-origin`

### AC-42: Chính sách nội dung không cho script inline

- Given gửi yêu cầu tới một trong các trang hiện có
- When đọc header `Content-Security-Policy` của phản hồi
- Then header có mặt, chỉ cho tải script, style, font và kết nối từ chính site, và không cho chạy script inline

### AC-43: Trang vẫn hiển thị đủ dưới chính sách nội dung

- Given mở một trong các trang hiện có trên trình duyệt
- When trang tải xong
- Then trình duyệt không báo vi phạm `Content-Security-Policy` nào

### AC-44: Tên blog hiển thị đúng dấu nháy

- Given mở một trong các trang hiện có trên trình duyệt
- When đọc chữ hiển thị của tên blog ở header và của `<title>`
- Then cả hai chứa đúng "Trình's Dev Notes", không chứa chuỗi `&#x27;`, `&#39;` hay `&amp;` nào hiện thành chữ

## Giả định

### Quyết định của người dùng

Người dùng trả lời 2026-10-05 các câu hỏi mở CH-1 đến CH-6 của bản nháp trước:

1. CH-1, tên blog: tên hiển thị là "Trình's Dev Notes", không phải "SimpleBlog". Áp dụng ở header (AC-4, AC-44) và trong `<title>` (AC-38). "SimpleBlog" vẫn là tên project và repo.
2. CH-2, bản quyền: footer ghi "© <năm hiện tại> Lê Khánh Trình" (AC-36).
3. CH-3, trang chủ: tiêu đề "Bài viết" và câu "Chưa có bài viết nào." cho tới F-06 (AC-33).
4. CH-4, trang Privacy của khung mẫu: gỡ bỏ, địa chỉ cũ trả 404 (AC-6, AC-34). Trang chính sách thật làm ở F-08.
5. CH-5, SEO: F-00 làm `lang`, `<title>`, `meta description` (AC-37 đến AC-39); `canonical` và Open Graph sang F-02.
6. CH-6, header bảo mật: làm trong F-00 (AC-40 đến AC-43).

Người dùng góp ý 2026-10-05, sau khi mở app xem bằng mắt (PR #23 đang mở):

7. Footer: trên trang chủ (nội dung chỉ hai dòng), footer nằm lửng giữa màn hình, phía dưới trống. Người dùng chọn: khi nội dung ngắn hơn khung nhìn thì footer nằm sát đáy khung nhìn (AC-45); khi nội dung dài thì footer nằm sau nội dung như bình thường, không dính cố định khi cuộn (AC-46), và không chồng lên nội dung (AC-47). Mục Footer của `ui-guidelines.md` chưa quy định điều này.
8. Vòng focus của vùng nội dung chính: sau khi dùng liên kết "Bỏ qua tới nội dung chính", vùng nội dung chính hiện vòng focus 2 px bao cả nội dung, trông như lỗi. Người dùng chọn: không vẽ vòng focus trên vùng nội dung chính khi nó nhận focus (AC-48); mọi liên kết và nút vẫn giữ vòng focus (AC-14, đã sửa để nói rõ vùng nội dung chính không thuộc "phần tử bấm được"). AC-13 giữ nguyên: focus vẫn phải chuyển tới vùng nội dung chính. Quyết định này không trái `ui-guidelines.md`: luật "cấm `outline: none`" ở mục Thành phần áp dụng cho phần tử bấm được (liên kết, nút, ô nhập, thẻ), và vùng nội dung chính không nằm trong thứ tự Tab. Nhưng `ui-guidelines.md` cũng chưa nói rõ trường hợp này.

Hai quy định ở mục 7 và 8 chưa có trong `ui-guidelines.md`. Đề xuất bổ sung vào `ui-guidelines.md` bằng PR riêng (xem Ngoài phạm vi), để các feature sau theo cùng quy định.

### Suy đoán cấp feature

Người duyệt muốn đổi thì sửa trước khi duyệt.

1. Định dạng `<title>` là "<Tên trang> - <Tên blog>". Riêng trang chủ chỉ là "<Tên blog>" (AC-38).
2. Năm trong dòng bản quyền là năm hiện tại, tự đổi theo năm (AC-36).
3. Nhãn mục menu giữ là "Giới thiệu" như feature `about` đã làm. Nhãn liên kết bỏ qua lấy đúng chữ trong `ui-guidelines.md`: "Bỏ qua tới nội dung chính".
4. Chỉ kiểm hai trang hiện có (trang chủ và Giới thiệu). Trang lỗi của khung mẫu cũng dùng layout mới, nhưng không có AC riêng.
5. Ba trình duyệt và các độ rộng lấy theo `nfr.md` và mục Cách kiểm của `ui-guidelines.md`. Nếu CI vượt 10 phút, ADR-0015 cho phép chạy đủ trình duyệt và độ rộng chỉ trên `main`. Việc đó là quyết định của architect, không đổi AC.
6. Chữ giao diện tiếng Việt viết thẳng trong view cho tới F-02 (xem Ngoài phạm vi).
7. Dấu nháy trong tên blog là dấu nháy thẳng `'` (U+0027), đúng như người dùng gõ. Nếu muốn dấu nháy cong `’` (U+2019) thì sửa trước khi duyệt; AC-4, AC-38, AC-44 đổi theo.
8. Tên blog có dấu nháy đơn và chữ có dấu, nên khi in qua `@` trong Razor sẽ bị mã hóa thành entity (`&#x27;`, `&#x1EC7;`...). Vì vậy các AC so chữ trên nội dung đã giải mã (xem đầu mục Acceptance criteria), không so chuỗi trong HTML thô. Theo `CLAUDE.md`, chữ cố định nên viết thẳng trong `.cshtml`; cách đặt tên blog vào `<title>` là việc của architect, miễn đạt AC-38 và AC-44. Test nào so HTML thô (ví dụ test hiện có của `about` trong AC-32) cần lưu ý điều này nếu có so tên blog.
9. Khung nhìn 360 × 800 px và 1280 × 800 px ở AC-45 là hai cỡ thường gặp của điện thoại và máy tính; ở cả hai, trang chủ hiện tại ngắn hơn khung nhìn. Độ lệch 1 px cho phép làm tròn số lẻ của trình duyệt.
10. "Khung nhìn đủ thấp" ở AC-46 và AC-47 do tester chọn, miễn nội dung trang cao hơn khung nhìn.
11. "Không có vòng focus" ở AC-48 nghĩa là trên vùng nội dung chính không nhìn thấy viền hay vòng nào do focus gây ra, ở cả ba trình duyệt.
12. AC mới được đặt trong nhóm theo chủ đề (AC-45 đến AC-47 ở nhóm Footer, AC-48 ở nhóm Bàn phím và tiếp cận), nên thứ tự mã trong file không liên tục; mã AC cũ không đổi.

## Câu hỏi mở

Không có. Các câu CH-1 đến CH-6 và hai góp ý bổ sung đã được trả lời ngày 2026-10-05 (xem "Quyết định của người dùng" ở mục Giả định).
