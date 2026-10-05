# 04 - Review: walking-skeleton

## Phạm vi đã xem

- File đã sửa: `.editorconfig`, `.github/dependabot.yml`, `.github/workflows/ci.yml`, `CLAUDE.md`, `README.md`, `CONTRIBUTING.md`, `03-tasks.md` (chỉ đổi dấu tick), `HomeController.cs`, `Program.cs`, `Views/Home/Index.cshtml`, `Views/Home/About.cshtml`, `Views/Shared/_Layout.cshtml`, `wwwroot/css/site.css`, `SimpleBlog.Tests.csproj`, `HomeControllerTests.cs`.
- File xóa: `Views/Home/Privacy.cshtml`, `_Layout.cshtml.css`, `_ValidationScriptsPartial.cshtml`, `wwwroot/js/site.js`, toàn bộ `wwwroot/lib/`.
- File mới (untracked), đã đọc: `Services/SecurityHeaders.cs`, `wwwroot/css/tokens.css`, `.stylelintrc.json`, `package.json`, `scripts/check-stylelint-rules.sh`, `tests/stylelint/violations.css`, `ProgramTests.cs`, `SecurityHeadersTests.cs`, `DesignTokenContrastTests.cs`.
- Lệnh thứ ba (`--ignored`) chỉ trả về `bin/`, `obj/`: không có đường dẫn bị `.gitignore` che.
- Vòng bổ sung (T-12, T-13): `git diff HEAD` chỉ đổi `site.css` (bốn khai báo ở `body`, `margin-top: auto` ở footer, khối `.site-main:focus`) và đổi `[ ]` thành `[x]` ở T-12, T-13 trong `03-tasks.md`. Không có file untracked, không có đường dẫn bị `.gitignore` che. `01`, `02` không bị sửa. Đúng `Files:` của T-12, T-13.
- Đã chạy: `dotnet build -c Release` 0 lỗi; `dotnet test -c Release --filter "Category!=Browser"` 63/63 đạt. Chưa chạy Stylelint (máy review không có node_modules) và chưa đo trình duyệt (việc của tester).

## Đối chiếu thiết kế

- Layout, `site.css`, `tokens.css`, `SecurityHeaders`, thứ tự middleware, `public partial class Program;`, Stylelint, CI, Dependabot đều đúng 02. Không thêm project, package hay pattern ngoài thiết kế. Hai package Playwright và axe-core nằm trong ADR-0015.
- `tokens.css` đối chiếu từng dòng với bốn bảng của `ui-guidelines.md`: đúng tên và giá trị.
- `_Layout.cshtml` giữ `class="navbar-nav ..."` và nhãn `Giới thiệu` sát thẻ `<a>` đúng như 02 yêu cầu cho AC-32. Không có `<script>`, không có `style=""`.
- `01-requirements.md`, `02-design.md` không bị sửa. `03-tasks.md` chỉ đổi `[ ]` thành `[x]`.
- Mọi file thay đổi nằm trong `Files:` của task tương ứng.

## Đối chiếu AC

| AC | Đáp ứng | Ghi chú |
|---|---|---|
| AC-1 | Đạt | `.container` dùng `min(var(--measure), ...)`, `margin-inline: auto`; áp cho header, main, footer |
| AC-2 | Đạt | Một `main`, không `aside`, không lưới |
| AC-3 | Đạt (cần tester đo) | `box-sizing`, `overflow-wrap: anywhere`, skip-link ẩn bằng `transform` |
| AC-4 | Đạt | `.site-title` dẫn `Home/Index` |
| AC-5 | Đạt | Mục menu dẫn `Home/About` |
| AC-6 | Đạt | Menu chỉ một `<li>`; Privacy đã gỡ |
| AC-7 | Đạt | `aria-current` chỉ khi route `Home/About`; CSS gạch chân `--color-primary` 2 px |
| AC-8 | Đạt | `@media (min-width: 768px)` chuyển `row` + `space-between` |
| AC-9 | Đạt | Mặc định `column`, không có nút menu |
| AC-10 | Đạt | Không `sticky`/`fixed` |
| AC-11 | Đạt | Skip-link là phần tử đầu của `body` |
| AC-12 | Đạt | `transform` đẩy lên trên khung nhìn khi không focus |
| AC-13 | Đạt | `main#main tabindex="-1"` |
| AC-14 | Đạt | `:focus-visible` outline 2 px cho liên kết và nút; header không cố định; `main` ngoài phạm vi theo AC-48 |
| AC-15 | Đạt | `min-height: var(--space-12)` cho title và mục menu |
| AC-16 | Cần tester | `lang`, landmark, một `h1`, tương phản theo token |
| AC-17 | Đạt | Menu là liên kết thường, không script |
| AC-18 | Đạt | HTML tĩnh phía server |
| AC-19 | Đạt | `system-ui` đầu danh sách, không `@font-face` |
| AC-20 | Đạt | Không đặt `font-size` cho `html` |
| AC-21 | Đạt | Cỡ nhỏ nhất là `--font-size-xs` |
| AC-22 | Đạt | `.site-main p` 18 px, line-height 1,7 |
| AC-23 | Đạt | `h1` 30 px, 36 px từ 768 px, đậm 700 |
| AC-24 | Đạt | Chỉ hai CSS của site; link ngoài của khung mẫu đã bỏ |
| AC-25 | Đạt | `lib/`, `site.js`, partial validation đã xóa; `grep` trong `src`, `tests` không còn bootstrap/jquery |
| AC-26 | Đạt | Khớp từng token với bảng |
| AC-27 | Đạt | `DesignTokenContrastTests` có trong bộ test xanh |
| AC-28 | Cần CI | Đọc `site.css`: không hex, không `px` cho font-size, không `!important`. Chưa chạy Stylelint vì máy review không có `node_modules` |
| AC-29 | Đạt (cần CI chạy) | Luật và `overrides` đúng 02; mẫu vi phạm đúng hai dòng; script kiểm cả hai luật |
| AC-30 | Đạt | Bước cài ba trình duyệt trước `dotnet test`, không `--filter` |
| AC-31 | Đạt | Job `stylelint` riêng |
| AC-32 | Đạt | Cấu trúc giữ cho test cũ khớp; test hiện có xanh |
| AC-33 | Đạt | `h1` "Bài viết" và câu "Chưa có bài viết nào." |
| AC-34 | Đạt | Action và view Privacy đã xóa |
| AC-35 | Đạt | `public partial class Program;` + `ProgramTests`. Việc đổi `AboutAcceptanceTests` sang `WebApplicationFactory<Program>` thuộc tester theo 02 |
| AC-36 | Đạt | Dòng bản quyền đúng; footer 14 px, `--color-text-muted` |
| AC-37 | Đạt | `lang="vi"` |
| AC-38 | Đạt | `<title>` đúng định dạng, tên blog viết thẳng |
| AC-39 | Đạt | Hai mô tả khác nhau |
| AC-40 | Đạt | Middleware đứng trước `UseRouting` và `MapStaticAssets` |
| AC-41 | Đạt | Giá trị đúng |
| AC-42 | Đạt | CSP không có `unsafe-inline`/`unsafe-eval` |
| AC-43 | Đạt (cần tester) | Không script inline, không `style=""` |
| AC-44 | Đạt | Tên blog viết thẳng, không qua `@` |
| AC-45 | Đạt (cần tester đo) | `body` flex cột, `min-height: 100vh` rồi `100dvh`; `.site-footer { margin-top: auto }` |
| AC-46 | Đạt | Không `fixed`/`sticky`; footer nằm sau `main` trong luồng |
| AC-47 | Đạt | `main` là flex item, không chồng; lề dưới `main` giữ nguyên |
| AC-48 | Đạt (cần tester) | `.site-main:focus { outline: none }`, đúng một `outline: none` trong `site.css`; `tabindex="-1"` giữ nguyên ở `_Layout.cshtml:31` |

## Tuân thủ tài liệu nền

| Tài liệu | Đạt | Ghi chú |
|---|---|---|
| `ui-guidelines.md` (tokens, layout, thành phần) | Đạt | CSS chỉ dùng `var(--...)`; thêm `100vh`/`100dvh`, `auto`, `none` là giá trị từ khóa, không phải màu/font/khoảng cách tự đặt. `outline: none` chỉ trên `main` không bấm được, đúng 02; `ui-guidelines.md` chưa có quy định này (đã ghi ở 02, không coi là tiền lệ) |
| `nfr.md` (tiếp cận, hiệu năng, SEO, bảo mật) | Đạt | `lang`, `title`, `description`, skip-link, focus, ba header bảo mật. Hiệu năng, `canonical`, Open Graph nằm ngoài phạm vi theo 01 |
| `architecture.md` và ADR | Đạt | Theo ADR-0013 (bỏ Bootstrap/jQuery), ADR-0015 (Playwright, Stylelint). `Services/` có trong bảng tầng. Không có quyết định lớn mới thiếu ADR |

## Findings

| Mã | Mức độ | Vị trí | Mô tả | Hướng sửa |
|---|---|---|---|---|
| F-1 | MINOR | `src/SimpleBlog.Web/Controllers/HomeController.cs:13-14` | Sau khi xóa `Privacy()` còn hai dòng trống liên tiếp. Chỉ là thẩm mỹ, `dotnet format` không báo lỗi | Xóa một dòng trống |

## Kết luận

APPROVE
