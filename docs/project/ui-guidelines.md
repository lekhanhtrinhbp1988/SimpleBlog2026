---
status: approved
approved_by: TRINH LE
approved_on: 2026-10-05
---

# Hướng dẫn giao diện

## Tóm tắt cho người duyệt

- Phong cách giống Substack: mọi trang là một cột chữ hẹp ở giữa, không có cột bên; nền trắng, chữ gần đen, nhiều khoảng trắng, một màu nhấn xanh dương (`#0A58CA`) cho liên kết và nút. Chỉ lấy phần trình bày của Substack; không có nút đăng ký nhận bài, bản tin email hay nội dung trả phí (vision để bản tin email ngoài phạm vi).
- Header tối giản: tên blog bên trái, vài mục menu và nút chuyển ngôn ngữ bên phải. Không có banner hay ảnh bìa.
- Trang bài: tiêu đề lớn và đậm (40 px trên máy tính), dòng phụ đề nếu bài có, rồi dòng tác giả và ngày đăng, rồi thân bài.
- Trang chủ là danh sách gọn: mỗi bài một hàng gồm tiêu đề, mô tả ngắn, ngày; có ảnh nhỏ bên phải **chỉ khi bài có ảnh**. Bài không có ảnh vẫn đẹp, không có ô trống thay ảnh. Việc có tải ảnh lên hay không vẫn chờ câu hỏi mở 6 của `vision.md`.
- Chữ là font có sẵn trên máy, không chân (Segoe UI trên Windows, San Francisco trên Mac và iPhone, Roboto trên Android), không phải tải về. Chữ trong bài 18 px, dòng thưa để dấu tiếng Việt không dính nhau.
- Cột chữ không rộng quá khoảng 70 đến 80 ký tự mỗi dòng (720 px), kể cả trên màn hình rất rộng.
- Chỉ có giao diện sáng; chưa có chế độ tối. Khối code nền xám nhạt, cuộn ngang bên trong khối nếu dòng dài, có nút "Sao chép" khi trình duyệt bật JavaScript; tắt JavaScript vẫn bôi đen và chép được.
- Mọi cặp màu chữ và nền đã tính sẵn độ tương phản, đều vượt mức tối thiểu của chuẩn tiếp cận (4,5 lần). Liên kết trong bài luôn có gạch chân. Dùng được hoàn toàn bằng bàn phím, phần đang chọn luôn có viền xanh rõ.
- Tối ưu cho máy tính; điện thoại vẫn đọc được: menu tự xuống dòng, không có nút ba gạch, không phải kéo ngang cả trang.
- Không dùng font hay thư viện tải từ máy chủ của bên khác.

## Nguyên tắc

Rút từ chân dung độc giả trong `vision.md` (lập trình viên Việt Nam 20 đến 35 tuổi, đọc trên máy tính trong giờ làm, đến từ công cụ tìm kiếm, hay sao chép code), các mức trong `nfr.md`, và lựa chọn phong cách của người dùng (giống Substack).

1. **Một cột, chữ là trung tâm.** Như Substack: mọi trang (trang chủ, bài, Giới thiệu, form) là một cột hẹp ở giữa, không có cột bên, không có khối trang trí. Độc giả đến từ kết quả tìm kiếm thấy ngay tiêu đề, phụ đề, tác giả và ngày ở đầu trang. Cấu trúc heading `h1` đến `h4` đúng thứ bậc để quét nhanh.
2. **Code là nội dung chính, không phải phụ lục.** Khối code dễ đọc (font đơn cách, cỡ đủ lớn, tương phản ≥ 4,5:1 cho mọi màu tô sáng), dễ sao chép (văn bản thật, giữ thụt lề), không làm vỡ cột chữ (cuộn ngang trong khối).
3. **Nhẹ và không phụ thuộc bên ngoài.** Giao diện nhẹ để đạt ngân sách 150 KB CSS và JavaScript, LCP ≤ 2,5 s; font hệ thống, không font hay script từ bên thứ ba; đọc được khi tắt JavaScript.
4. **Bàn phím và tương phản là mặc định.** WCAG 2.2 AA: mọi cặp màu chữ có tỉ lệ ghi trong bảng, focus luôn nhìn thấy, vùng bấm tối thiểu 24 × 24 px.
5. **Hai ngôn ngữ ngang hàng.** Bố cục chịu được chuỗi tiếng Anh và tiếng Việt dài ngắn khác nhau (không đặt độ rộng cố định cho nút và mục menu); ngôn ngữ của bài và trạng thái bản dịch được ghi bằng chữ, không chỉ bằng cờ hay màu.

## Design tokens

Toàn bộ token khai báo một lần trong `:root` của một file CSS riêng (ví dụ `wwwroot/css/tokens.css`), mọi CSS khác chỉ dùng `var(--...)`, không viết mã màu hay cỡ chữ trực tiếp. Tên trong bảng là tên biến CSS.

### Màu

Tỉ lệ tương phản tính theo công thức độ chói tương đối của WCAG 2.x. Mức cần đạt (`nfr.md`): chữ thường ≥ 4,5:1, chữ lớn (≥ 24 px, hoặc ≥ 18,66 px đậm) ≥ 3:1, viền thành phần điều khiển và vòng focus ≥ 3:1.

| Token | Giá trị | Dùng cho | Tỉ lệ tương phản với nền |
|---|---|---|---|
| `--color-bg` | `#FFFFFF` | Nền trang | (nền) |
| `--color-surface` | `#F6F8FA` | Nền khối code, code trong dòng, banner cookie, ô ghi chú | (nền phụ) |
| `--color-text` | `#1F2328` | Chữ chính, heading, tiêu đề bài | 15,8:1 trên `--color-bg`; 14,8:1 trên `--color-surface` |
| `--color-text-muted` | `#59636E` | Phụ đề bài, dòng tác giả và ngày, mô tả trong danh sách, nhãn phụ, chú thích, chân trang | 6,1:1 trên `--color-bg`; 5,7:1 trên `--color-surface` |
| `--color-primary` | `#0A58CA` | Liên kết, nền nút chính, gạch chân mục menu đang chọn | 6,4:1 trên `--color-bg`; 6,0:1 trên `--color-surface` |
| `--color-primary-hover` | `#084298` | Liên kết và nút chính khi hover | 9,4:1 trên `--color-bg` |
| `--color-on-primary` | `#FFFFFF` | Chữ trên nền `--color-primary` | 6,4:1 trên `--color-primary`; 9,4:1 trên `--color-primary-hover` |
| `--color-focus` | `#0A58CA` (theo `--color-primary`) | Vòng focus | 6,4:1 với `--color-bg` (mức cần ≥ 3:1) |
| `--color-border` | `#D0D7DE` | Đường kẻ trang trí: dưới header, trên footer, giữa các bài trong danh sách, dưới phần đầu bài | 1,5:1; chỉ dùng cho đường kẻ trang trí, không dùng cho viền ô nhập hay nút |
| `--color-border-strong` | `#6E7781` | Viền ô nhập, nút phụ, thẻ (tag) | 4,5:1 trên `--color-bg` (mức cần ≥ 3:1) |
| `--color-danger` | `#B42318` | Chữ báo lỗi form, viền ô nhập lỗi | 6,6:1 trên `--color-bg` |
| `--color-success` | `#1A7F37` | Chữ báo thành công | 5,1:1 trên `--color-bg` |

Màu tô sáng cú pháp trong khối code (khối code nền sáng, nền `--color-surface`):

| Token | Giá trị | Dùng cho | Tỉ lệ tương phản với `--color-surface` |
|---|---|---|---|
| `--color-code-text` | `#1F2328` | Chữ code mặc định | 14,8:1 |
| `--color-code-comment` | `#59636E` | Chú thích | 5,7:1 |
| `--color-code-keyword` | `#CF222E` | Từ khóa | 5,0:1 |
| `--color-code-string` | `#0A3069` | Chuỗi | 12,0:1 |
| `--color-code-function` | `#8250DF` | Tên hàm, phương thức | 4,7:1 |
| `--color-code-number` | `#0550AE` | Số, hằng | 7,1:1 |
| `--color-code-type` | `#953800` | Kiểu, lớp | 6,9:1 |

Luật dùng màu:

- Không truyền đạt thông tin chỉ bằng màu (WCAG 1.4.1): lỗi form có chữ, liên kết trong bài có gạch chân, mục menu đang chọn có gạch chân và `aria-current="page"`.
- Thêm màu mới phải thêm dòng vào bảng kèm tỉ lệ tương phản với mọi nền nó được đặt lên.
- Chỉ có giao diện sáng; chưa làm chế độ tối, không đọc `prefers-color-scheme`. Tên token đặt theo vai trò (`--color-text`, không phải `--color-black`) để sau này thêm bộ giá trị tối không phải đổi tên.

### Chữ

Font tải từ máy chủ bên thứ ba (Google Fonts, CDN) **không được dùng**: `nfr.md` chỉ cho tải tài nguyên từ chính site và Google Analytics. Blog dùng font hệ thống, không tải file font nào.

| Token | Giá trị | Ghi chú |
|---|---|---|
| `--font-family-base` | `system-ui, -apple-system, "Segoe UI", Roboto, "Helvetica Neue", Arial, "Noto Sans", sans-serif` | Font hệ thống không chân, 0 KB tải về. Segoe UI (Windows), San Francisco (macOS, iOS), Roboto (Android) đều có đủ dấu tiếng Việt. Các tên sau là font dự phòng theo thứ tự; Arial và Noto Sans cũng có đủ dấu tiếng Việt |
| `--font-family-heading` | `var(--font-family-base)` | Tách token để sau này đổi riêng heading mà không sửa CSS khác |
| `--font-family-mono` | `ui-monospace, "Cascadia Mono", "Cascadia Code", Consolas, "SF Mono", Menlo, "Liberation Mono", "Courier New", monospace` | Dùng cho khối code và code trong dòng. Chú thích code có thể là tiếng Việt: kiểm hiển thị dấu (mục Cách kiểm) |
| `--font-size-xs` | `0.875rem` (14 px) | Dòng tác giả và ngày, nhãn phụ, chân trang. Nhỏ nhất được phép |
| `--font-size-sm` | `1rem` (16 px) | Menu, nút, form, mô tả bài trong danh sách |
| `--font-size-base` | `1.125rem` (18 px) | Chữ thân bài viết |
| `--font-size-lg` | `1.25rem` (20 px) | Phụ đề bài, tên blog trong header, `h4` |
| `--font-size-xl` | `1.5rem` (24 px) | `h3`, tiêu đề bài trong danh sách |
| `--font-size-2xl` | `1.875rem` (30 px) | `h2`; mọi `h1` khi màn hình < 768 px |
| `--font-size-3xl` | `2.25rem` (36 px) | `h1` của trang không phải bài (trang chủ, Giới thiệu, chính sách, form) khi màn hình ≥ 768 px |
| `--font-size-4xl` | `2.5rem` (40 px) | `h1` tiêu đề bài khi màn hình ≥ 768 px |
| `--font-size-code` | `0.9375rem` (15 px) | Chữ trong khối code. Code trong dòng dùng `0.9em` theo chữ xung quanh |
| `--line-height-body` | `1.7` | Đoạn văn dài. Tiếng Việt có dấu chồng hai tầng (ví dụ "ệ", "ỗ") nên cần dòng thưa hơn mức 1,5 thông thường |
| `--line-height-heading` | `1.25` | Heading. Không thấp hơn, để dấu của dòng dưới không chạm chữ dòng trên khi tiêu đề bài 40 px xuống dòng |
| `--line-height-code` | `1.6` | Khối code |
| `--font-weight-normal` | `400` | Chữ thường, phụ đề |
| `--font-weight-bold` | `700` | Heading, tiêu đề bài, tên blog, nút |

Luật dùng chữ:

- Gốc `html` giữ 16 px (mặc định trình duyệt), không đặt `font-size` cố định bằng `px` cho `html`; mọi cỡ chữ dùng `rem` để người đọc phóng chữ được. (`site.css` hiện đặt `html { font-size: 14px }` dưới 768 px: phải bỏ.)
- Không dùng chữ nghiêng cho đoạn dài, không dùng chữ in hoa toàn bộ cho câu dài hơn 3 từ (dấu tiếng Việt khó đọc ở chữ hoa).
- Đoạn văn cách nhau `--space-4`; không thụt đầu dòng.

### Khoảng cách, bo góc

Thang khoảng cách bước 4 px. Không dùng giá trị ngoài thang. Bo góc nhỏ, đúng tinh thần gọn của Substack.

| Token | Giá trị |
|---|---|
| `--space-1` | `0.25rem` (4 px) |
| `--space-2` | `0.5rem` (8 px) |
| `--space-3` | `0.75rem` (12 px) |
| `--space-4` | `1rem` (16 px) |
| `--space-6` | `1.5rem` (24 px) |
| `--space-8` | `2rem` (32 px) |
| `--space-12` | `3rem` (48 px) |
| `--space-16` | `4rem` (64 px) |
| `--radius-none` | `0` |
| `--radius-sm` | `4px` — nút, ô nhập, code trong dòng, ảnh nhỏ trong danh sách |
| `--radius-md` | `8px` — khối code, banner cookie, ô ghi chú |
| `--radius-pill` | `9999px` — thẻ (tag) |
| `--border-width` | `1px` |
| `--focus-ring-width` | `2px` |
| `--focus-ring-offset` | `2px` |
| `--measure` | `720px` — độ rộng tối đa của cột duy nhất (xem Layout) |
| `--thumb-width` | `120px` — rộng ảnh nhỏ trong danh sách khi màn hình ≥ 768 px; `90px` dưới 768 px |
| `--thumb-height` | `80px` — cao ảnh nhỏ khi màn hình ≥ 768 px; `60px` dưới 768 px (tỉ lệ 3:2) |

## Layout

**Một cột.** Mọi trang dùng một cột duy nhất, căn giữa, `max-width: var(--measure)` = 720 px: header, trang chủ, thân bài, Giới thiệu, chính sách, form, footer. Không có cột bên (sidebar) ở bất kỳ breakpoint nào. Với chữ 18 px, 720 px tương ứng khoảng 70 đến 80 ký tự mỗi dòng. Đo bằng `px` để kiểm tự động được bằng `getBoundingClientRect().width`.

- Khối code và bảng trong bài: cùng độ rộng cột, `overflow-x: auto` bên trong khối; không tràn ra ngoài cột.
- Lề hai bên: `--space-4` (16 px) khi màn hình < 768 px, `--space-8` (32 px) khi ≥ 768 px.
- Khoảng trắng dọc rộng như Substack: cách header tới nội dung `--space-12`, cách nội dung tới footer `--space-16`.

**Breakpoint.** Biến CSS không dùng được trong `@media`, nên đây là hằng số ghi tay, chỉ dùng các giá trị này, viết theo kiểu mobile-first (`min-width`):

| Tên | Giá trị | Thay đổi |
|---|---|---|
| (mặc định) | 320 đến 767 px | Menu xuống dòng dưới tên blog; mọi `h1` 30 px; ảnh nhỏ 90 × 60 px; lề 16 px |
| `md` | `min-width: 768px` | Tên blog và menu cùng một hàng; `h1` bài 40 px, `h1` trang khác 36 px; ảnh nhỏ 120 × 80 px; lề 32 px |
| `lg` | `min-width: 1280px` | Bố cục tối ưu (`nfr.md`); cột vẫn 720 px, phần thừa hai bên để trắng |

**Header.** Tối giản như Substack, rộng bằng cột: tên blog (chữ `--font-size-lg` đậm, liên kết về trang chủ) bên trái; menu chính và nút chuyển ngôn ngữ bên phải; đường kẻ `--color-border` toàn chiều rộng màn hình ở dưới. Không logo ảnh, không ô tìm kiếm mở sẵn, không nút đăng ký. Không cố định (`position: sticky` không dùng), để không che nội dung đang có focus (WCAG 2.4.11) và không chiếm chỗ khi đọc. Có liên kết "Bỏ qua tới nội dung chính" là phần tử đầu tiên nhận focus, chỉ hiện khi có focus.

**Menu chính.** Tối đa 4 mục, thứ tự dự kiến: Bài viết, Thẻ, Tìm kiếm, Giới thiệu (mục nào có thì hiện khi feature tương ứng xong). Chữ `--font-size-sm` màu `--color-text`. Không dùng nút ba gạch: dưới 768 px các mục tự xuống dòng (`flex-wrap: wrap`), mỗi mục cao ≥ 44 px, nên menu hoạt động không cần JavaScript. Mục của trang hiện tại có `aria-current="page"` và gạch chân dày 2 px màu `--color-primary`.

**Chuyển ngôn ngữ.** Hai liên kết chữ "VI" và "EN" (có `lang` và `hreflang`, nhãn đầy đủ "Tiếng Việt" / "English" trong `aria-label`), không dùng cờ. Ngôn ngữ đang chọn có `aria-current="true"`.

**Footer.** Rộng bằng cột, đường kẻ `--color-border` ở trên. Chữ `--font-size-xs`, màu `--color-text-muted`, gồm: liên kết Chính sách quyền riêng tư, liên kết "Cài đặt cookie" (rút lại đồng ý, bắt buộc theo `nfr.md`), liên kết RSS (khi có), dòng bản quyền. Dưới 768 px các liên kết xếp dọc. Không có ô đăng ký nhận bài.

**Trang chủ.** `h1` "Bài viết" (theo ngôn ngữ giao diện), rồi danh sách bài (xem Thành phần). Không có bài nổi bật, ảnh bìa hay khối giới thiệu lớn.

**Trang chi tiết bài.** Từ trên xuống, kiểu Substack:

1. `h1` tiêu đề bài: `--font-size-4xl` (30 px dưới 768 px), `--font-weight-bold`, `--line-height-heading`, màu `--color-text`.
2. Phụ đề (nếu bài có): thẻ `<p>` ngay dưới `h1`, `--font-size-lg`, `--font-weight-normal`, màu `--color-text-muted`, cách `h1` `--space-2`. Bài không có phụ đề thì bỏ hẳn dòng này, không để khoảng trống. Bài có trường phụ đề hay không do BA của feature chi tiết bài và trình soạn bài chốt.
3. Dòng tác giả: tên tác giả, ngày đăng, ngày sửa (nếu có), ngôn ngữ của bài, liên kết bản dịch (nếu có); ngăn nhau bằng dấu "·"; `--font-size-xs` màu `--color-text-muted`; cách phần trên `--space-4`. Ngày dùng phần tử `<time datetime>`. Không có ảnh đại diện tác giả.
4. Thẻ (tag) của bài.
5. Đường kẻ `--color-border`, cách trên dưới `--space-8`.
6. Thân bài.

Không có ảnh bìa. Không có nút chia sẻ, nút thích hay nút đăng ký.

**Màn hình nhỏ nhất (320 px, `nfr.md`).** Không cuộn ngang cả trang; khối code và bảng cuộn ngang trong khối; URL dài và chuỗi không có dấu cách trong thân bài và tiêu đề được ngắt bằng `overflow-wrap: anywhere`; ảnh `max-width: 100%`; trong danh sách, ảnh nhỏ giữ 90 × 60 px, chữ chiếm phần còn lại.

## Thành phần

Trạng thái chung cho mọi phần tử bấm được (liên kết, nút, ô nhập, thẻ):

- **Focus:** `outline: var(--focus-ring-width) solid var(--color-focus); outline-offset: var(--focus-ring-offset);` dùng `:focus-visible`. Cấm `outline: none` nếu không có thay thế tương đương. Focus không bị phần tử khác che (WCAG 2.4.11).
- **Vùng bấm:** tối thiểu 24 × 24 px (WCAG 2.5.8); nút và mục menu cao ≥ 44 px.
- **Chuyển trạng thái:** không dùng hiệu ứng chuyển động dài hơn 150 ms; tôn trọng `prefers-reduced-motion`.

**Liên kết.**

- Trong thân bài: màu `--color-primary`, gạch chân `text-decoration-thickness: 1px`, `text-underline-offset: 0.2em`. Hover: màu `--color-primary-hover`, gạch chân dày 2 px.
- Trong header, footer, tiêu đề bài trong danh sách: màu chữ thường (`--color-text` hoặc `--color-text-muted`), không gạch chân khi bình thường, gạch chân khi hover và focus.
- Liên kết ra ngoài site: không tự mở tab mới.

**Nút.**

| Loại | Bình thường | Hover | Dùng cho |
|---|---|---|---|
| Chính | Nền `--color-primary`, chữ `--color-on-primary` | Nền `--color-primary-hover` | Hành động chính của form (Đăng, Lưu, Gửi bình luận) |
| Phụ | Nền `--color-bg`, viền `--color-border-strong`, chữ `--color-text` | Nền `--color-surface` | Hủy, Sao chép code, Tải thêm |
| Nguy hiểm | Nền `--color-bg`, viền và chữ `--color-danger` | Nền `--color-surface` | Xóa bài, xóa bình luận, xóa tài khoản |

Chung: chữ `--font-size-sm` đậm, đệm `--space-2` dọc và `--space-4` ngang, bo `--radius-sm`, cao ≥ 44 px, độ rộng theo nội dung (không cố định, vì nhãn tiếng Anh và tiếng Việt dài khác nhau). Trạng thái vô hiệu: `aria-disabled` và chữ giải thích, không chỉ làm mờ.

Banner cookie: nút "Đồng ý" và "Từ chối" **cùng loại nút phụ**, cùng kích cỡ, cạnh nhau (`nfr.md` yêu cầu cùng độ nổi bật). Banner nằm ở cuối khung nhìn, nền `--color-surface`, viền trên `--color-border-strong`, nội dung rộng tối đa `--measure`, không che phần tử đang có focus.

**Thẻ (tag).** Liên kết dạng viên thuốc: viền `--color-border-strong`, bo `--radius-pill`, chữ `--font-size-xs` màu `--color-text`, đệm `--space-1` × `--space-3`. Hover: nền `--color-surface`.

**Khối code.**

- Nền `--color-surface`, bo `--radius-md`, đệm `--space-4`, font `--font-family-mono` cỡ `--font-size-code`, `--line-height-code`, `overflow-x: auto`, `white-space: pre`, `tab-size: 4`.
- Phần tử `<pre><code>`, không bọc dòng; có nhãn ngôn ngữ lập trình (ví dụ "C#") ở góc trên bằng chữ `--font-size-xs` màu `--color-text-muted`.
- Nút "Sao chép" (nút phụ) chỉ được chèn bằng JavaScript; khi chép xong đổi nhãn thành "Đã sao chép" trong 2 giây và báo qua vùng `aria-live="polite"`. Không có JavaScript thì không có nút, vẫn chọn chữ và chép được.
- Khối code cuộn được bằng bàn phím: `tabindex="0"` khi nội dung rộng hơn khối, kèm `aria-label` hoặc nhãn ngôn ngữ.
- Code trong dòng: nền `--color-surface`, bo `--radius-sm`, đệm `0.1em 0.3em`.

**Mục bài trong danh sách.** Kiểu danh sách gọn của Substack, một hàng ngang (`display: flex`, `gap: var(--space-4)`):

- Bên trái, chiếm phần còn lại (`flex: 1; min-width: 0`):
  1. Tiêu đề: `h2`, cỡ `--font-size-xl`, đậm, màu `--color-text`, là liên kết duy nhất tới bài.
  2. Mô tả ngắn: phụ đề của bài, nếu bài không có phụ đề thì đoạn tóm tắt (nguồn tóm tắt do BA của feature danh sách bài chốt); cỡ `--font-size-sm`, màu `--color-text-muted`, tối đa 2 dòng (`line-clamp: 2`, nội dung đầy đủ vẫn có trong HTML). Không có mô tả thì bỏ dòng.
  3. Dòng thông tin: ngày đăng, ngôn ngữ của bài, nhãn "Có bản dịch" nếu có; ngăn bằng "·"; cỡ `--font-size-xs` màu `--color-text-muted`.
- Bên phải, **chỉ khi bài có ảnh nhỏ**: ảnh `--thumb-width` × `--thumb-height` (tỉ lệ 3:2), `object-fit: cover`, bo `--radius-sm`, căn trên. Ảnh không phải liên kết riêng và có `alt=""` (tiêu đề bên cạnh đã nói bài là gì; ảnh chỉ để nhận diện). Bài không có ảnh: **không render phần tử ảnh, không có ô xám hay ảnh mặc định**; cột chữ chiếm toàn bộ chiều rộng.
- Không hiện thẻ (tag) trong danh sách, để giữ danh sách gọn; thẻ nằm ở trang chi tiết bài.
- Không làm cả khối thành liên kết (để trình đọc màn hình không đọc cả khối như một liên kết dài).

**Danh sách bài.** Một cột rộng `--measure`, mới nhất lên đầu; các mục cách nhau `--space-6` trên và dưới, ngăn bằng đường kẻ `--color-border`. Danh sách trộn bài có ảnh và không ảnh vẫn thẳng hàng: chữ của mọi mục bắt đầu cùng mép trái. Phân trang bằng liên kết "Bài mới hơn" / "Bài cũ hơn" (hoạt động không cần JavaScript), có `aria-label` cho vùng phân trang. Danh sách trống: một câu chữ giải thích, không để trang trắng.

**Form.**

- Nhãn hiển thị luôn phía trên ô nhập (`<label for>`), không dùng placeholder thay nhãn. Trường bắt buộc ghi chữ "(bắt buộc)" trong nhãn, không chỉ dấu sao.
- Ô nhập: viền `--color-border-strong`, bo `--radius-sm`, đệm `--space-2` × `--space-3`, chữ `--font-size-sm`, cao ≥ 44 px, rộng 100% cột. Focus: vòng focus chung.
- Lỗi: viền ô `--color-danger` dày 2 px, câu lỗi ngay dưới ô màu `--color-danger` bắt đầu bằng chữ "Lỗi:" (không chỉ dựa vào màu), nối bằng `aria-describedby`, ô có `aria-invalid="true"`. Khi gửi form có lỗi: tóm tắt lỗi ở đầu form, focus chuyển tới tóm tắt.
- Thông báo thành công: chữ `--color-success` trong vùng `role="status"`.

**Không có.** Các thành phần đặc trưng khác của Substack không thuộc vision nên không làm: nút và ô đăng ký nhận bài qua email, khối nội dung trả phí, nút thích, nút chia sẻ, ảnh đại diện tác giả.

## Ảnh

Việc có tải ảnh lên hay không còn chờ câu hỏi mở 6 của `vision.md`. Khi chưa có ảnh, mọi trang vẫn đầy đủ (danh sách bài hiển thị không có ảnh nhỏ). Nếu có:

- Chỉ ảnh tự làm (ảnh chụp màn hình, sơ đồ); không ảnh trang trí, không ảnh bìa trên trang bài.
- Ảnh trong bài: rộng tối đa 100% cột (720 px); file gốc rộng tối đa 1440 px (đủ nét cho màn hình mật độ điểm ảnh gấp đôi). Sơ đồ ưu tiên SVG; ảnh chụp màn hình dùng WebP hoặc PNG, ≤ 200 KB mỗi ảnh.
- Ảnh nhỏ trong danh sách: tùy chọn, là một trong các ảnh tự làm của bài (cách tác giả chọn do BA của feature trình soạn bài chốt). File phục vụ cho danh sách là bản thu nhỏ 240 × 160 px (gấp đôi kích thước hiển thị), WebP, ≤ 20 KB, không dùng ảnh gốc cỡ lớn.
- Luôn có thuộc tính `width` và `height` để không làm nhảy bố cục (CLS ≤ 0,1); `loading="lazy"` cho mọi ảnh trừ ảnh đầu tiên trong khung nhìn đầu.
- `alt` bắt buộc với ảnh trong bài: ảnh có thông tin thì mô tả thông tin đó (ảnh chụp đoạn code thì chép code thành văn bản, không dùng ảnh thay code); ảnh chỉ để trang trí thì `alt=""`. Trang quản trị không cho đăng ảnh thiếu ô `alt` (phải điền hoặc đánh dấu "trang trí"). Ảnh nhỏ trong danh sách luôn `alt=""` (xem Thành phần).
- Chú thích ảnh dùng `<figure>` và `<figcaption>`, chữ `--font-size-xs` màu `--color-text-muted`.

## Cách kiểm

Công cụ là đề xuất; `architecture.md` chốt công cụ và cách chạy trên CI (theo `nfr.md`).

| Luật | Kiểm tự động bằng | Cách |
|---|---|---|
| Tương phản của mọi token màu (gồm màu tô sáng code) | Unit test đọc file token CSS | Với mỗi cặp chữ và nền ghi trong bảng Màu, tính tỉ lệ theo công thức WCAG, đỏ nếu dưới 4,5:1 (chữ) hoặc 3:1 (viền điều khiển, focus). Bắt được lỗi ngay cả khi trang chưa có phần tử dùng màu đó |
| Tương phản trên trang thật, nhãn form, `lang`, landmark, heading | axe-core trong acceptance test (Playwright) | Luật gắn tag `wcag2a`, `wcag2aa`, `wcag21aa`, `wcag22aa`; 0 vi phạm; chạy cả khi banner cookie hiện và đã ẩn |
| Focus nhìn thấy | Playwright | Bấm Tab qua từng phần tử bấm được; `getComputedStyle` của phần tử có focus có `outline-style` khác `none` và `outline-width` ≥ 2 px; phần tử có focus nằm trong khung nhìn và không bị banner che (so sánh hình chữ nhật bao) |
| Liên kết "Bỏ qua tới nội dung chính" | Playwright | Tab lần đầu tiên focus vào liên kết này; Enter chuyển focus tới `main` |
| Không cuộn ngang cả trang | Playwright ở 320, 360, 768, 1280, 1920 px | `document.documentElement.scrollWidth` ≤ chiều rộng khung nhìn, trên trang bài có khối code dài, URL dài, tiêu đề dài, và trang chủ có bài có ảnh nhỏ |
| Một cột, độ rộng cột | Playwright ở 1280 và 1920 px | Chiều rộng nội dung header, `main`, footer đều ≤ 720 px; khối code cùng chiều rộng thân bài; không có phần tử `aside` hay cột thứ hai nằm cạnh `main` |
| Cỡ chữ và line-height thân bài | Playwright | `getComputedStyle` của đoạn văn trong thân bài: `font-size` = 18 px, `line-height` ≥ 1,7 × cỡ chữ; không phần tử chữ nào < 14 px |
| Phần đầu trang bài | Playwright ở 1280 px và 360 px | Thứ tự trong DOM: `h1`, phụ đề (nếu có), dòng tác giả có `<time datetime>`; `h1` cỡ 40 px ở 1280 px và 30 px ở 360 px, đậm 700; bài không có phụ đề thì không có phần tử phụ đề rỗng |
| Ảnh nhỏ tùy chọn trong danh sách | Playwright với dữ liệu mẫu trộn bài có ảnh và không ảnh | Mục không ảnh không có `img`; mục có ảnh có `img` với `alt=""`, `width`, `height`, kích thước hiển thị 120 × 80 px ở 1280 px và 90 × 60 px ở 360 px; mép trái tiêu đề của mọi mục bằng nhau; mỗi mục chỉ có một liên kết tới bài |
| Font hệ thống | Playwright | Không có yêu cầu mạng loại `font`; `font-family` tính được của `body` bắt đầu bằng `system-ui` |
| Vùng bấm | Playwright | Mọi `a`, `button`, `input` hiển thị có kích thước ≥ 24 × 24 px (trừ liên kết nằm trong câu văn, WCAG cho phép); nút và mục menu cao ≥ 44 px |
| Menu hoạt động không cần JavaScript | Playwright với JavaScript tắt ở 360 px | Mọi mục menu hiển thị và bấm được |
| Khối code giữ thụt lề, chép được | Acceptance test (theo `nfr.md`) | So khớp văn bản của `<pre><code>` với mã gốc |
| Nút "Đồng ý" và "Từ chối" cùng kích cỡ | Playwright (theo `nfr.md`) | So `getBoundingClientRect` của hai nút; cả hai trong khung nhìn đầu |
| Không dùng mã màu hay cỡ chữ trực tiếp ngoài file token | Kiểm tĩnh trong CI (ví dụ Stylelint với luật cấm mã màu hex và `px` cho `font-size` ngoài file token) | Đỏ nếu CSS ngoài file token chứa mã màu hoặc cỡ chữ cứng |
| Không tải font hay CSS từ bên thứ ba | Playwright (dùng chung kiểm danh sách tên miền của `nfr.md`) | Mọi yêu cầu mạng loại `font` và `stylesheet` đều tới chính site |

Kiểm thủ công (reviewer ghi vào `04-review.md`):

- Đi hết trang bằng bàn phím, thứ tự focus hợp lý, không bẫy focus.
- Dấu tiếng Việt hiển thị đúng ở chữ thân bài, tiêu đề bài 40 px khi xuống hai dòng, phụ đề và chú thích trong khối code (ví dụ chuỗi "Mỗi ngày học thêm một điều, ổn định và đều đặn") trên Windows (Chrome hoặc Edge) và một trình duyệt WebKit: dấu không bị cắt, không bị chồng vào dòng trên.
- Trang hiển thị cả tiếng Việt và tiếng Anh không bị vỡ bố cục khi nhãn dài hơn.
- So trang chủ và trang bài với phong cách đã chọn (một cột hẹp, header tối giản, tiêu đề lớn đậm, danh sách gọn), không có thành phần Substack nằm ngoài vision.

## Câu hỏi mở

Các lựa chọn thẩm mỹ đã được người dùng trả lời (phong cách giống Substack, màu nhấn xanh dương `#0A58CA`, font hệ thống không chân, chỉ giao diện sáng, khối code nền sáng) và ghi vào các mục trên. Còn một câu chuyển cho `architecture.md`.

| Câu hỏi | Hạn chót | Ai trả lời |
|---|---|---|
| 1. Giữ Bootstrap hay bỏ, viết CSS riêng theo token? Bootstrap CSS, JavaScript và jQuery hiện có chiếm phần đáng kể của ngân sách 150 KB và có hệ màu, cỡ chữ riêng chồng lên token ở đây; bố cục một cột ở đây không cần hệ lưới của Bootstrap | Trước feature giao diện đầu tiên (walking skeleton) | `architecture.md` (agent architect), người dùng duyệt |
