# 02 - Design: walking-skeleton

## Tóm tắt

Thay toàn bộ phần giao diện của khung `dotnet new mvc` bằng layout một cột theo `ui-guidelines.md`, không thêm controller hay route mới:

- **Layout và CSS (ADR-0013).** Viết lại `Views/Shared/_Layout.cshtml` (liên kết bỏ qua, header với tên blog "Trình's Dev Notes" và menu một mục "Giới thiệu", `main`, footer bản quyền). Thêm `wwwroot/css/tokens.css` chứa đúng bảng token, viết lại `wwwroot/css/site.css` chỉ dùng `var(--...)`. Xóa `wwwroot/lib/` (Bootstrap, jQuery, jquery-validation), `_ValidationScriptsPartial.cshtml`, CSS cô lập `_Layout.cshtml.css`, `wwwroot/js/site.js`. Trang không tải JavaScript nào.
- **Trang.** Trang chủ (`HomeController.Index`) hiện `h1` "Bài viết" và câu "Chưa có bài viết nào.". Trang Giới thiệu giữ nguyên nội dung, chỉ thêm mô tả trang. Bỏ action và view `Privacy`; `/Home/Privacy` rơi vào route mặc định không có action nên trả 404.
- **Header bảo mật.** Lớp tĩnh `Services/SecurityHeaders.cs` đặt `Content-Security-Policy`, `X-Content-Type-Options`, `Referrer-Policy`; `Program.cs` gọi nó qua một middleware inline đặt trước định tuyến, nên áp cho cả trang lẫn file tĩnh.
- **`Program` public.** Thêm `public partial class Program;` để test dùng `WebApplicationFactory<Program>` trực tiếp.
- **Công cụ kiểm (ADR-0015).** Thêm package `Microsoft.Playwright` và `Deque.AxeCore.Playwright` vào project test (tester không được sửa `.csproj`). Thêm `package.json` chỉ chứa Stylelint, cấu hình `.stylelintrc.json`, một file CSS vi phạm mẫu để CI chứng minh luật chặn được. CI cài trình duyệt Playwright trước `dotnet test` và có job `stylelint` riêng. Hai acceptance test cần `node_modules/stylelint` mang trait `Category=Stylelint`, chạy trong job `stylelint` (có Node và .NET) và bị loại khỏi `dotnet test` của job `build-and-test` (quyết định của người dùng 2026-10-05, xem mục "CI").

- **Bổ sung 2026-10-05 (AC-45 đến AC-48, AC-14 làm rõ).** Chỉ sửa `site.css`, không đổi HTML: `body` thành cột flex cao tối thiểu bằng khung nhìn, footer có `margin-top: auto` nên bị đẩy xuống đáy khi nội dung ngắn và nằm ngay sau nội dung khi nội dung dài (không `position: fixed`/`sticky`). Vùng `main` (`tabindex="-1"`, đích của liên kết bỏ qua) không vẽ vòng focus qua một luật `.site-main:focus { outline: none }` riêng; luật `:focus-visible` chung của mọi liên kết và nút giữ nguyên.

Feature không có DbContext, entity hay migration. Ràng buộc đăng ký `AddDbContext<T>((sp, options) => ...)` không áp dụng ở đây; nó áp dụng cho feature đầu tiên thêm DbContext (F-04).

## Ánh xạ AC -> thành phần

| AC | Thành phần (controller/action, view, model, dữ liệu) |
|---|---|
| AC-1 | `site.css`: `.container` rộng `min(var(--measure), 100% - 2 × lề)`, `margin-inline: auto`; dùng cho `header .container`, `main.container`, `footer .container` (`_Layout.cshtml`) |
| AC-2 | `_Layout.cshtml`: chỉ một `main`, không `aside`, không lưới; `site.css` không có bố cục nhiều cột |
| AC-3 | `site.css`: reset `box-sizing: border-box`, `.container` không vượt `100%`, `main { overflow-wrap: anywhere }`, liên kết bỏ qua ẩn bằng `transform` lên trên (không tạo cuộn ngang) |
| AC-4 | `_Layout.cshtml`: `<a class="site-title" asp-controller="Home" asp-action="Index">Trình's Dev Notes</a>` |
| AC-5 | `_Layout.cshtml`: mục menu `asp-controller="Home" asp-action="About"`; `HomeController.About()` giữ nguyên |
| AC-6 | `_Layout.cshtml`: `<ul>` của menu chỉ có một `<li>` "Giới thiệu"; bỏ mục Home, Privacy |
| AC-7 | `_Layout.cshtml`: `aria-current="page"` khi route hiện tại là `Home/About`, không có thuộc tính ở trang khác; `site.css`: `.site-nav a[aria-current="page"]` gạch chân `--color-primary` dày 2 px |
| AC-8 | `site.css`: `@media (min-width: 768px)` `.site-header .container` `flex-direction: row; justify-content: space-between` |
| AC-9 | `site.css`: mặc định `.site-header .container` `flex-direction: column; align-items: flex-start`; không có nút menu trong `_Layout.cshtml` |
| AC-10 | `site.css`: header không có `position: sticky` hay `fixed` |
| AC-11 | `_Layout.cshtml`: `<a class="skip-link" href="#main">Bỏ qua tới nội dung chính</a>` là phần tử đầu tiên trong `body` |
| AC-12 | `site.css`: `.skip-link` nằm trọn phía trên khung nhìn (`transform`) khi không có focus; `.skip-link:focus` hiện lại |
| AC-13 | `_Layout.cshtml`: `<main id="main" tabindex="-1">` nhận focus khi theo liên kết `#main` |
| AC-14 | `site.css`: `:focus-visible { outline: var(--focus-ring-width) solid var(--color-focus); outline-offset: var(--focus-ring-offset) }` áp cho mọi liên kết và nút; header không cố định nên không che phần tử có focus. `main#main` không nằm trong thứ tự Tab (`tabindex="-1"`) nên không thuộc phạm vi AC này (xem AC-48) |
| AC-48 | `site.css`: `.site-main:focus { outline: none }` (độ ưu tiên 0,2,0, thắng `:focus-visible` 0,1,0 và vòng focus mặc định của trình duyệt); chỉ nhắm `main`, không đụng liên kết hay nút. `_Layout.cshtml` giữ `tabindex="-1"` nên AC-13 vẫn đạt |
| AC-15 | `site.css`: `.site-nav a`, `.site-title` `display: inline-flex; min-height: var(--space-12)` (48 px); `.skip-link` có đệm `--space-2` × `--space-4` |
| AC-16 | `_Layout.cshtml`: `lang="vi"`, landmark `header`, `nav aria-label`, `main`, `footer`, một `h1` mỗi trang; tương phản theo token (AC-27). Kiểm bằng `Deque.AxeCore.Playwright` (T-8) |
| AC-17 | Menu là liên kết HTML thường, không cần JavaScript; trang không tải script |
| AC-18 | `About.cshtml` là HTML tĩnh render ở server |
| AC-19 | `tokens.css`: `--font-family-base` bắt đầu bằng `system-ui`; `site.css`: `body { font-family: var(--font-family-base) }`; không có `@font-face` |
| AC-20 | `site.css` không đặt `font-size` cho `html` (bỏ `html { font-size: 14px }` cũ) |
| AC-21 | Mọi `font-size` trong `site.css` là token ≥ `--font-size-xs` (14 px) |
| AC-22 | `site.css`: `main p { font-size: var(--font-size-base); line-height: var(--line-height-body) }` |
| AC-23 | `site.css`: `h1 { font-size: var(--font-size-2xl); font-weight: var(--font-weight-bold) }`, `@media (min-width: 768px) { h1 { font-size: var(--font-size-3xl) } }` |
| AC-24 | `_Layout.cshtml` chỉ tham chiếu `~/css/tokens.css`, `~/css/site.css`; `Index.cshtml` bỏ liên kết ngoài của khung mẫu |
| AC-25 | Xóa `wwwroot/lib/`, `_ValidationScriptsPartial.cshtml`; `_Layout.cshtml` không còn thẻ `<script>` |
| AC-26 | `wwwroot/css/tokens.css`: `:root` khai báo đúng từng token của bảng Màu (cả bảng tô sáng code), Chữ, Khoảng cách và bo góc |
| AC-27 | Giá trị màu trong `tokens.css`; unit test `DesignTokenContrastTests` tính tỉ lệ cho các cặp ở mục "Cặp màu kiểm tương phản" |
| AC-28 | `.stylelintrc.json` + `package.json`; `site.css`, `tokens.css` qua Stylelint; acceptance test có trait `Category=Stylelint`, chạy trong job `stylelint` |
| AC-29 | `.stylelintrc.json`: `color-no-hex`, `declaration-property-unit-disallowed-list` (`font-size`, `font`: `px`), tắt cho `tokens.css`; mẫu vi phạm `tests/stylelint/violations.css` và `scripts/check-stylelint-rules.sh`; acceptance test có trait `Category=Stylelint`, chạy trong job `stylelint` |
| AC-30 | `.github/workflows/ci.yml` job `build-and-test`: bước cài Chromium, Firefox, WebKit của Playwright trước `dotnet test`; `dotnet test` chạy mọi test trừ trait `Category=Stylelint`, không lọc trait `Browser` |
| AC-31 | `.github/workflows/ci.yml` job `stylelint`: `npx stylelint` trên CSS của site, đỏ khi có lỗi |
| AC-32 | `_Layout.cshtml` giữ `class="navbar-nav ..."` trên `<ul>` của menu và nhãn `Giới thiệu` viết thẳng trong `<a>`, để bộ chọn trong `AboutAcceptanceTests` vẫn khớp; `About.cshtml` giữ `h1` và đoạn văn |
| AC-33 | `Views/Home/Index.cshtml`: `<h1>Bài viết</h1><p>Chưa có bài viết nào.</p>` |
| AC-34 | `HomeController`: bỏ action `Privacy`; xóa `Views/Home/Privacy.cshtml` |
| AC-35 | `Program.cs`: `public partial class Program;`; unit test `ProgramTests` |
| AC-36 | `_Layout.cshtml`: `<p class="site-footer__copy">© @DateTime.Now.Year Lê Khánh Trình</p>`; `site.css`: `.site-footer { font-size: var(--font-size-xs); color: var(--color-text-muted) }` |
| AC-45 | `site.css`: `body { display: flex; flex-direction: column; min-height: 100vh; min-height: 100dvh }`, `.site-footer { margin-top: auto }`; khi tổng chiều cao header + main + footer nhỏ hơn khung nhìn, phần dư dồn vào lề trên của footer nên cạnh dưới footer trùng cạnh dưới khung nhìn |
| AC-46 | `site.css`: `.site-footer` không có `position: fixed`/`sticky`; khi nội dung cao hơn khung nhìn, `margin-top: auto` bằng 0 và footer nằm sau `main` trong luồng bình thường |
| AC-47 | `site.css`: header, `main`, footer là các mục flex xếp dọc trong luồng, không phần tử nào định vị tuyệt đối hay cố định (chỉ `.skip-link` là `absolute`), nên cạnh trên footer luôn ≥ cạnh dưới `main` (cộng lề dưới `--space-16` của `.site-main`) |
| AC-37 | `_Layout.cshtml`: `<html lang="vi">` |
| AC-38 | `_Layout.cshtml`: `<title>` ghép `ViewData["Title"]` + " - " với tên blog viết thẳng; trang chủ không đặt `Title` |
| AC-39 | `_Layout.cshtml` in `<meta name="description">` từ `ViewData["Description"]`; `Index.cshtml`, `About.cshtml` mỗi view đặt một chuỗi riêng |
| AC-40 | `Services/SecurityHeaders.cs` đặt `X-Content-Type-Options: nosniff`; middleware trong `Program.cs` đứng trước `UseRouting`/`MapStaticAssets` nên áp cả cho `site.css` |
| AC-41 | `Services/SecurityHeaders.cs`: `Referrer-Policy: strict-origin-when-cross-origin` |
| AC-42 | `Services/SecurityHeaders.cs`: hằng `ContentSecurityPolicy` (xem Luồng xử lý), không có `'unsafe-inline'` hay `'unsafe-eval'` |
| AC-43 | `_Layout.cshtml` không có `<script>` inline (bỏ cả `<script type="importmap">`), không có thuộc tính `style=""`; CSS chỉ từ file của site |
| AC-44 | Tên blog viết thẳng trong `_Layout.cshtml` ở cả header và `<title>`, không qua `@`, nên không thể bị mã hóa hai lần |

## Mô hình dữ liệu

Không có. Không entity, không migration, không view model mới. Dữ liệu duy nhất của trang là `ViewData["Title"]` và `ViewData["Description"]` do từng view đặt.

| View | `ViewData["Title"]` | `ViewData["Description"]` |
|---|---|---|
| `Home/Index.cshtml` | (không đặt) | `Ghi chép của Lê Khánh Trình về lập trình và phát triển phần mềm.` |
| `Home/About.cshtml` | `Giới thiệu` (giữ như cũ) | `Giới thiệu về tác giả Lê Khánh Trình và mục đích của blog.` |
| `Shared/Error.cshtml` | `Error` (giữ như cũ, không sửa file) | (không đặt) |

## Luồng xử lý

### Pipeline trong `Program.cs`

Thứ tự sau khi sửa (chỉ thêm một bước, phần còn lại giữ nguyên):

1. `UseExceptionHandler`, `UseHsts` (khi không phải Development), như hiện tại.
2. **Mới:** `app.Use(async (context, next) => { SecurityHeaders.Apply(context.Response.Headers); await next(context); });`. Đặt sau `UseExceptionHandler` để khi trang lỗi được render lại, header vẫn được đặt (exception handler xóa header của phản hồi lỗi rồi chạy lại pipeline từ sau nó). Đặt trước `UseRouting`, `MapStaticAssets` để file tĩnh cũng có header (AC-40).
3. `UseHttpsRedirection`, `UseRouting`, `UseAuthorization`, `MapStaticAssets`, `MapControllerRoute`, như hiện tại.
4. Cuối file: `public partial class Program;`.

### `Services/SecurityHeaders.cs`

Lớp `public static class SecurityHeaders` trong namespace `SimpleBlog.Web.Services`:

- `public const string ContentSecurityPolicy = "default-src 'self'; script-src 'self'; style-src 'self'; img-src 'self'; font-src 'self'; connect-src 'self'; object-src 'none'; base-uri 'self'; form-action 'self'; frame-ancestors 'none'";`
- `public static void Apply(IHeaderDictionary headers)`: gán (ghi đè, không cộng dồn) ba header `Content-Security-Policy` = hằng trên, `X-Content-Type-Options` = `nosniff`, `Referrer-Policy` = `strict-origin-when-cross-origin`.

Không thêm `upgrade-insecure-requests`: test trình duyệt chạy app qua Kestrel bằng HTTP, chỉ thị này sẽ đổi yêu cầu CSS sang HTTPS và làm trang mất CSS. Google Analytics được thêm vào CSP ở F-10, không phải bây giờ.

### Layout `Views/Shared/_Layout.cshtml`

Viết lại toàn bộ, UTF-8 có BOM (ADR-0005). Khung (developer giữ đúng cấu trúc, tên class và chữ; khoảng trắng tùy ý):

```cshtml
<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>@(ViewData["Title"] is string pageTitle ? pageTitle + " - " : "")Trình's Dev Notes</title>
    @if (ViewData["Description"] is string description)
    {
        <meta name="description" content="@description" />
    }
    <link rel="stylesheet" href="~/css/tokens.css" asp-append-version="true" />
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
</head>
<body>
    <a class="skip-link" href="#main">Bỏ qua tới nội dung chính</a>
    <header class="site-header">
        <div class="container">
            <a class="site-title" asp-area="" asp-controller="Home" asp-action="Index">Trình's Dev Notes</a>
            <nav aria-label="Menu chính">
                <ul class="navbar-nav site-nav">
                    <li><a asp-area="" asp-controller="Home" asp-action="About" aria-current="@(isAbout ? "page" : null)">Giới thiệu</a></li>
                </ul>
            </nav>
        </div>
    </header>
    <main id="main" class="container site-main" tabindex="-1">
        @RenderBody()
    </main>
    <footer class="site-footer">
        <div class="container">
            <p class="site-footer__copy">© @DateTime.Now.Year Lê Khánh Trình</p>
        </div>
    </footer>
</body>
</html>
```

- `isAbout` tính ở khối `@{ }` đầu file: `ViewContext.RouteData.Values["controller"]` là `Home` và `["action"]` là `About` (so sánh `StringComparison.OrdinalIgnoreCase`). Razor bỏ hẳn thuộc tính khi giá trị là `null`, nên trang chủ không có `aria-current` (AC-7).
- Không có thẻ `<script>` nào, không có `RenderSectionAsync("Scripts")` (không view nào định nghĩa section này; `_ValidationScriptsPartial` bị xóa). Không có thuộc tính `style`.
- Class `navbar-nav` trên `<ul>` không có CSS nào; nó chỉ được giữ để `AboutAcceptanceTests` (tìm chuỗi `class="navbar-nav`) vẫn chạy được (AC-32). Thuộc tính `class` phải bắt đầu đúng bằng `navbar-nav`.
- Nhãn `Giới thiệu` viết thẳng, sát thẻ: `<a ...>Giới thiệu</a>` không có phần tử con, để regex `<a[^>]*>\s*Giới thiệu\s*</a>` của test cũ khớp.

**Mã hóa ký tự.** Tên blog, nhãn menu, nhãn liên kết bỏ qua, dòng bản quyền viết thẳng trong `.cshtml` theo mục "Vietnamese text in Razor" của `CLAUDE.md`. `<title>` và `meta description` cần giá trị riêng mỗi trang nên đi qua `@ViewData`; Razor mã hóa một lần (ví dụ `Giới thiệu` thành `Gi&#x1EDB;i thi&#x1EC7;u`), trình duyệt và `WebUtility.HtmlDecode` giải mã ra đúng chữ. 01 (đầu mục Acceptance criteria, Giả định 8) chấp nhận dạng này. Riêng tên blog trong `<title>` là chữ viết thẳng, không qua `@`, nên không có `&#x27;` (AC-44). Phương án khác đã cân nhắc: dùng `@section` để chữ tiêu đề cũng viết thẳng; không chọn vì nội dung section chèn vào thuộc tính `content` không được mã hóa, dễ vỡ HTML khi mô tả có dấu `"`. F-02 sẽ bật `WebEncoderOptions` (ADR-0011) và chuyển chữ sang `.resx`.

### Trang

- `HomeController`: bỏ action `Privacy()`. `Index()`, `About()`, `Error()` giữ nguyên.
- `Views/Home/Index.cshtml`: khối `@{ ViewData["Description"] = "..."; }` (chuỗi ở bảng Mô hình dữ liệu), rồi `<h1>Bài viết</h1>` và `<p>Chưa có bài viết nào.</p>`, viết thẳng. Không còn `div.text-center`, chữ "Welcome" hay liên kết `learn.microsoft.com`.
- `Views/Home/About.cshtml`: chỉ thêm dòng `ViewData["Description"] = "...";` trong khối `@{ }` đầu file. `h1` và đoạn văn giữ nguyên từng ký tự.
- Xóa `Views/Home/Privacy.cshtml`. `/Home/Privacy` khớp route mặc định nhưng không có action, nên MVC trả 404 (AC-34).
- `Views/Shared/Error.cshtml`: không sửa. Nó dùng layout mới; class `text-danger` không còn CSS, không ảnh hưởng AC nào (01, Giả định 4).

### CSS

Hai file, nạp theo thứ tự `tokens.css` rồi `site.css`. Áp dụng `ui-guidelines.md` mục **Design tokens**, **Layout**, **Thành phần** (trạng thái chung, Liên kết).

**`wwwroot/css/tokens.css`** (mục Design tokens). Chỉ một khối `:root { ... }`, không selector khác. Tên và giá trị chép đúng từng dòng của bốn bảng (Màu, màu tô sáng code, Chữ, Khoảng cách và bo góc), mã màu viết hoa như trong bảng, với các quy ước:

- `--color-focus: #0A58CA;` viết giá trị, không viết `var(--color-primary)`, để file so được trực tiếp với bảng.
- `--font-family-heading: var(--font-family-base);` đúng như bảng.
- Cột "Giá trị" có chú thích trong ngoặc hoặc sau dấu "—" thì chỉ lấy phần giá trị CSS: `--font-size-xs: 0.875rem;`, `--radius-sm: 4px;`, `--measure: 720px;`.
- `--thumb-width: 120px;`, `--thumb-height: 80px;` (giá trị chính của bảng). Bản 90 × 60 px dưới 768 px làm ở F-06 cùng danh sách bài, khi có ảnh nhỏ.

**`wwwroot/css/site.css`** (viết lại toàn bộ; chỉ dùng `var(--...)` cho màu, cỡ chữ, khoảng cách; breakpoint là hằng `768px` theo mục Layout, không dùng `1280px` vì bố cục `lg` không đổi gì):

| Selector | Khai báo |
|---|---|
| `*, *::before, *::after` | `box-sizing: border-box` |
| `body` | `margin: 0; font-family: var(--font-family-base); font-size: var(--font-size-sm); line-height: var(--line-height-body); color: var(--color-text); background: var(--color-bg)`; **bổ sung (AC-45):** `display: flex; flex-direction: column; min-height: 100vh; min-height: 100dvh` (dòng `100vh` là dự phòng cho trình duyệt chưa hỗ trợ `dvh`; `dvh` tránh footer bị đẩy dưới thanh địa chỉ trên điện thoại) |
| `h1, h2, h3, h4` | `font-family: var(--font-family-heading); font-weight: var(--font-weight-bold); line-height: var(--line-height-heading); margin: 0 0 var(--space-4)` |
| `h1` | `font-size: var(--font-size-2xl)`; ở `md`: `var(--font-size-3xl)` |
| `h2`, `h3`, `h4` | `--font-size-2xl`, `--font-size-xl`, `--font-size-lg` |
| `a` | `color: var(--color-primary); text-decoration-thickness: var(--border-width); text-underline-offset: 0.2em` |
| `a:hover` | `color: var(--color-primary-hover); text-decoration-thickness: calc(2 * var(--border-width))` |
| `:focus-visible` | `outline: var(--focus-ring-width) solid var(--color-focus); outline-offset: var(--focus-ring-offset)` |
| `.container` | `width: min(var(--measure), calc(100% - 2 * var(--space-4))); margin-inline: auto`; ở `md`: lề `var(--space-8)` thay `var(--space-4)` |
| `.skip-link` | `position: absolute; top: var(--space-2); left: var(--space-4); padding: var(--space-2) var(--space-4); background: var(--color-bg); color: var(--color-primary); font-size: var(--font-size-sm); transform: translateY(calc(-100% - var(--space-4)))` |
| `.skip-link:focus` | `transform: none` |
| `.site-header` | `border-bottom: var(--border-width) solid var(--color-border)` |
| `.site-header .container` | `display: flex; flex-direction: column; align-items: flex-start; gap: 0 var(--space-4); padding-block: var(--space-2)`; ở `md`: `flex-direction: row; align-items: center; justify-content: space-between` |
| `.site-title` | `display: inline-flex; align-items: center; min-height: var(--space-12); font-size: var(--font-size-lg); font-weight: var(--font-weight-bold); color: var(--color-text); text-decoration: none` |
| `.site-nav` | `display: flex; flex-wrap: wrap; gap: 0 var(--space-4); list-style: none; margin: 0; padding: 0` |
| `.site-nav a` | `display: inline-flex; align-items: center; min-height: var(--space-12); font-size: var(--font-size-sm); color: var(--color-text); text-decoration: none` |
| `.site-title:hover, .site-title:focus-visible, .site-nav a:hover, .site-nav a:focus-visible` | `text-decoration: underline` |
| `.site-nav a[aria-current="page"]` | `text-decoration: underline; text-decoration-color: var(--color-primary); text-decoration-thickness: calc(2 * var(--border-width)); text-underline-offset: 0.3em` |
| `.site-main` | `margin-block: var(--space-12) var(--space-16); overflow-wrap: anywhere` |
| `.site-main:focus` | **Mới (AC-48):** `outline: none`. Đặt ngay sau `.site-main` |
| `.site-main p` | `font-size: var(--font-size-base); line-height: var(--line-height-body); margin: 0 0 var(--space-4)` |
| `.site-footer` | `border-top: var(--border-width) solid var(--color-border); padding-block: var(--space-6); font-size: var(--font-size-xs); color: var(--color-text-muted)`; **bổ sung (AC-45):** `margin-top: auto` |
| `.site-footer p` | `margin: 0` |

Không dùng `position: sticky`/`fixed`, `transition`, `animation`, `!important`, mã màu, tên màu, hàm màu hay `font-size` bằng `px`. `outline: none` chỉ được xuất hiện đúng một lần, trong `.site-main:focus` (AC-48); `main` không nằm trong thứ tự Tab nên luật "cấm `outline: none`" của `ui-guidelines.md` mục Thành phần (áp cho phần tử bấm được) không bị vi phạm (01, Quyết định của người dùng 8). Không thêm `outline: none` cho bất kỳ selector nào khác, không dùng `:focus { outline: none }` chung.

**Vì sao chọn flex + `margin-top: auto` cho footer (AC-45 đến AC-47).** Phương án đã cân nhắc:

- `position: fixed; bottom: 0` cho footer: bị loại vì AC-46 (không dính khi cuộn) và AC-47 (sẽ chồng lên nội dung).
- `.site-main { flex: 1 0 auto }` thay vì `margin-top: auto` ở footer: cũng đạt, nhưng kéo dài hộp `main` xuống tận footer, làm vùng nhận focus (AC-13) và kết quả đo `main` trong test thay đổi theo chiều cao khung nhìn. Không chọn.
- CSS Grid `grid-template-rows: auto 1fr auto` trên `body`: đạt, nhưng `.skip-link` là con trực tiếp của `body` và dù `absolute` vẫn cần hiểu cách grid đối xử; flex đơn giản hơn và đủ.

`main.container` trong `body` flex vẫn căn giữa vì `margin-inline: auto` hoạt động với mục flex và `width` đã đặt rõ (AC-1 không đổi). Lề `--space-12`/`--space-16` của `.site-main` không còn gộp (margin collapse) với phần tử khác trong flex, nhưng trước đây cũng không gộp vì header và footer có viền; khoảng cách nhìn thấy không đổi.

### Cặp màu kiểm tương phản (AC-27)

Unit test đọc `tokens.css` (tìm thư mục gốc repo bằng cách đi ngược từ `AppContext.BaseDirectory` tới file `SimpleBlog.slnx`), lấy giá trị hex của token, tính tỉ lệ theo công thức độ chói tương đối WCAG 2.x, và kiểm:

| Chữ / thành phần | Nền | Mức |
|---|---|---|
| `--color-text`, `--color-text-muted`, `--color-primary`, `--color-primary-hover`, `--color-danger`, `--color-success` | `--color-bg` | ≥ 4,5 |
| `--color-text`, `--color-text-muted`, `--color-primary` | `--color-surface` | ≥ 4,5 |
| `--color-on-primary` | `--color-primary`, `--color-primary-hover` | ≥ 4,5 |
| `--color-code-text`, `--color-code-comment`, `--color-code-keyword`, `--color-code-string`, `--color-code-function`, `--color-code-number`, `--color-code-type` | `--color-surface` | ≥ 4,5 |
| `--color-focus`, `--color-border-strong` | `--color-bg` | ≥ 3 |

`--color-border` (1,5:1) không kiểm vì bảng chỉ dùng nó cho đường kẻ trang trí.

### Stylelint (ADR-0015)

- `package.json` ở gốc: `"private": true`, chỉ một `devDependencies` là `stylelint` ghim **đúng một phiên bản** (không `^`, không `~`), không script build. Không commit `package-lock.json`, không thêm `stylelint-config-standard` (giữ một dependency trực tiếp).
- `.stylelintrc.json` ở gốc:
  - `rules`: `"color-no-hex": true`, `"color-named": "never"`, `"function-disallowed-list": ["rgb", "rgba", "hsl", "hsla", "hwb", "lab", "lch", "oklab", "oklch", "color"]`, `"declaration-property-unit-disallowed-list": { "font-size": ["px"], "font": ["px"] }`, `"declaration-no-important": true`.
  - `overrides`: với `files: ["**/wwwroot/css/tokens.css"]`, tắt (`null`) bốn luật `color-no-hex`, `color-named`, `function-disallowed-list`, `declaration-property-unit-disallowed-list`.
- `tests/stylelint/violations.css`: mẫu cố ý vi phạm, đúng hai khai báo: một dòng `color: #123456;` và một dòng `font-size: 13px;`. File này không phải acceptance test; nó là dữ liệu cho bước CI kiểm luật (AC-29).
- `scripts/check-stylelint-rules.sh` (bash, `set -euo pipefail`): chạy `npx stylelint tests/stylelint/violations.css --formatter json`, bắt mã thoát; đỏ (exit 1, in lý do) nếu Stylelint thoát 0, hoặc nếu output JSON không có cảnh báo của luật `color-no-hex` và của luật `declaration-property-unit-disallowed-list`. Đạt thì in một dòng xác nhận và exit 0. Kiểm bằng `grep` trên output, không cần `jq`.

### CI (`.github/workflows/ci.yml`)

- Job `build-and-test`: thêm bước giữa `dotnet build` và `dotnet test`:
  `pwsh tests/SimpleBlog.Tests/bin/Release/net10.0/playwright.ps1 install --with-deps chromium firefox webkit` (script này do package `Microsoft.Playwright` sinh khi build; `pwsh` có sẵn trên runner Ubuntu). `dotnet test` không lọc trait `Browser`, nên test trình duyệt đỏ làm job đỏ (AC-30). Bản sửa 2026-10-05: lệnh thành `dotnet test --no-build --configuration Release --filter "Category!=Stylelint"` (xem đoạn "Test cần Stylelint" bên dưới).
- Job mới `stylelint` (runner `ubuntu-latest`, cùng `permissions: contents: read` ở cấp workflow): `actions/checkout` (cùng major với job khác), `actions/setup-node` major mới nhất với `node-version: lts/*`, `npm install --no-audit --no-fund`, `npx stylelint "src/SimpleBlog.Web/wwwroot/css/**/*.css"` (AC-28, AC-31), rồi `bash scripts/check-stylelint-rules.sh` (AC-29).
- **Test cần Stylelint (quyết định của người dùng 2026-10-05, sau khi CI PR #23 đỏ).** Hai acceptance test `ProjectFileTests.AC28_CssCuaSiteQuaStylelint` và `AC29_StylelintChanMauVaCoChuVietCung` gọi `node_modules/stylelint`, mà job `build-and-test` không có Node nên hai test đỏ ở đó. Cách làm:
  - Hai test mang `[Trait("Category", "Stylelint")]`; không đổi assert, không skip. Trên máy dev chưa `npm install`, `dotnet test` vẫn đỏ ở hai test này với thông báo hiện có ("chua co node_modules/stylelint: chay `npm install` o goc repo").
  - Job `build-and-test`: `dotnet test --no-build --configuration Release --filter "Category!=Stylelint"`. Test trình duyệt vẫn chạy (AC-30).
  - Job `stylelint`: sau `actions/setup-node`, thêm `actions/setup-dotnet` (cùng major với job `build-and-test`) với `global-json-file: global.json`; sau bước `bash scripts/check-stylelint-rules.sh`, thêm bước `dotnet test --configuration Release --filter Category=Stylelint` (tự restore và build). Job không cài trình duyệt Playwright vì không chạy test `Browser`.
  - Không bị bỏ qua lặng lẽ: nếu trait bị xóa hoặc đổi tên, hai test rơi vào `build-and-test` và đỏ ở đó vì thiếu `node_modules`; nếu job `stylelint` mất bước `dotnet test`, reviewer thấy trong diff `ci.yml`.
  - Phương án đã loại: **cài Node và `npm install` vào job `build-and-test`**. Chạy được nhưng job vốn dài nhất (build, ba trình duyệt) phải thêm Node và npm, Node xuất hiện ở hai job thay vì một, trái tinh thần "Node chỉ ở job `stylelint`" của thiết kế ban đầu. **Skip hai test khi thiếu `node_modules`**: bị loại vì CI thiếu Node sẽ báo xanh mà không kiểm gì.
- `.github/dependabot.yml`: thêm mục `package-ecosystem: npm`, `directory: "/"`, lịch như NuGet, `commit-message.prefix: "chore(deps)"`, nhóm minor và patch như NuGet.
- Không thêm Lighthouse CI (backlog: trước F-05). Không cache trình duyệt Playwright trong feature này; thêm nếu thời gian CI là vấn đề.

### Test trình duyệt (để tester dùng, không phải task của developer)

- Package có sẵn sau T-8: `Microsoft.Playwright`, `Deque.AxeCore.Playwright`.
- App chạy bằng Kestrel thật: `WebApplicationFactory<Program>` của .NET 10 có `UseKestrel(...)` và `StartServer()`; lấy địa chỉ từ `ClientOptions.BaseAddress` (cần xác nhận API trong tài liệu `integration-tests` như ADR-0015 ghi). Địa chỉ là HTTP; app không có cổng HTTPS nên `UseHttpsRedirection` không chuyển hướng.
- Đánh dấu test trình duyệt `[Trait("Category", "Browser")]` để chạy riêng được (`--filter Category=Browser`). Trên máy dev, cài trình duyệt một lần: `pwsh tests/SimpleBlog.Tests/bin/Debug/net10.0/playwright.ps1 install`.
- Hook ổn định cho test: `header .container`, `main#main`, `footer .container`, `.site-title`, `nav[aria-label="Menu chính"] a`, `.skip-link`, `.site-footer__copy`.
- axe-core tiêm script vào trang; nếu CSP chặn việc này, dùng `BypassCSP = true` cho context của test axe (AC-16) nhưng **không** cho test AC-43.
- AC-10 cần trang dài hơn khung nhìn: dùng khung nhìn thấp (ví dụ 320 × 400) trên trang Giới thiệu thay vì thêm trang giả.
- AC-45: so `footer.site-footer` `getBoundingClientRect().bottom` với `window.innerHeight` (lệch ≤ 1 px). AC-46, AC-47: cùng cách khung nhìn thấp như AC-10; đo `footer.site-footer` (`top` > `innerHeight` khi chưa cuộn) và so `top` của nó với `bottom` của `main#main`. Hook đo footer là phần tử `footer.site-footer`, không phải `footer .container`.
- AC-48: theo liên kết bỏ qua (Tab rồi Enter), xác nhận `document.activeElement` là `main#main` (AC-13), rồi đọc `getComputedStyle(main).outlineStyle` là `none` (hoặc `outlineWidth` là `0px`) và `boxShadow` là `none`, trên cả ba trình duyệt.

## Thay đổi so với khung hiện có

- **Package test `Microsoft.Playwright`, `Deque.AxeCore.Playwright`** trong `tests/SimpleBlog.Tests`. Lý do và phương án đã cân nhắc: ADR-0015 (Accepted). Thêm trong task của developer vì tester không được sửa `.csproj`.
- **Node chỉ trong CI: `package.json`, `.stylelintrc.json`, job `stylelint`, mục `npm` trong Dependabot.** Theo ADR-0015. App không cần Node để build hay chạy; máy dev không bắt buộc có Node để chạy app, nhưng muốn `dotnet test` xanh toàn bộ thì cần `npm install` (hai test trait `Category=Stylelint`). Job `stylelint` cài thêm .NET SDK để chạy hai test đó (bản sửa 2026-10-05, mục "CI").
- **`Services/SecurityHeaders.cs`**: thư mục `Services/` đã có trong bảng tầng của `architecture.md` ("logic không thuộc một controller"). Không phải pattern mới. Phương án đơn giản hơn đã cân nhắc: viết ba header thẳng trong lambda ở `Program.cs`; không chọn vì không unit test được giá trị CSP. Không dùng package header bảo mật bên thứ ba (ví dụ NetEscapades): ba header không đáng thêm dependency.
- **Bỏ** Bootstrap, jQuery, jquery-validation, `site.js`, CSS cô lập của layout, trang Privacy (ADR-0013, 01).

Không thêm project, không thêm pattern (repository, mediator...).

## Rủi ro

- **`public partial class Program` và analyzer.** Với `TreatWarningsAsErrors`, khai báo này có thể kích hoạt CA1050 (kiểu ngoài namespace) hoặc CA1052 (lớp chỉ có thành viên static). Nếu SDK 10 đã tự sinh `Program` public bằng source generator, analyzer ASP0027 sẽ báo khai báo thừa. Cách xử lý ghi trong T-1: bỏ đúng luật đó trong `.editorconfig` với phạm vi `[src/SimpleBlog.Web/Program.cs]` kèm comment, hoặc bỏ khai báo nếu ASP0027 xác nhận generator đã làm việc này. Không dùng `#pragma`.
- **Test cũ của `about` dựa vào chuỗi `class="navbar-nav`** và URL `/Home/About`. Thiết kế giữ cả hai. Tester vẫn phải sửa `WebFactoryHolder` của test cũ sang `WebApplicationFactory<Program>` cho AC-35; việc đó không đổi phần assert.
- **CSP và công cụ dev.** `dotnet watch` hay Browser Link tiêm script vào trang khi chạy dev; CSP có thể chặn script tiêm inline. Chấp nhận: chỉ ảnh hưởng hot reload trên máy dev, không ảnh hưởng test hay production.
- **CSP và axe-core**: xem mục Test trình duyệt.
- **Thời gian CI.** Ba trình duyệt × năm độ rộng × hai trang. ADR-0015 cho phép chỉ chạy đủ ma trận trên `main` nếu CI vượt 10 phút; feature này chạy đủ trên mọi PR vì AC-30 yêu cầu ba trình duyệt trên PR. Đo lại sau PR đầu tiên.
- **Stylelint không có lock file.** Chỉ ghim phiên bản `stylelint`, phụ thuộc bắc cầu có thể đổi giữa hai lần chạy CI. Chấp nhận vì máy dev không bắt buộc có Node để sinh `package-lock.json`; Dependabot theo dõi phiên bản trực tiếp. Xem lại nếu CI Stylelint đỏ không do CSS.
- **Năm trong bản quyền** dùng `DateTime.Now` của server; quanh giao thừa có thể lệch múi giờ với người đọc vài giờ. Chấp nhận.
- **`dvh` và thanh địa chỉ di động.** Trên điện thoại thật, `100vh` lớn hơn phần nhìn thấy khi thanh địa chỉ hiện, nên footer có thể nằm dưới mép màn hình vài chục px; `100dvh` sửa việc này trên trình duyệt hỗ trợ. Trong Playwright, `vh` và `dvh` bằng nhau nên AC-45 không phụ thuộc vào điểm này.
- **`ui-guidelines.md` chưa có quy định của AC-45 đến AC-48.** Thiết kế dựa trên quyết định của người dùng trong 01. Bổ sung vào `ui-guidelines.md` là PR tài liệu nền riêng (01, Ngoài phạm vi); cho tới đó, feature sau không được coi `outline: none` trên `.site-main:focus` là tiền lệ cho phần tử bấm được.
- **WebKit trên Windows** của Playwright có thể khác Safari thật; nfr chấp nhận Playwright WebKit làm đại diện.
