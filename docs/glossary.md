# Thuật ngữ

Các từ viết tắt, tên công cụ và thuật ngữ quy trình dùng trong SimpleBlog. Mỗi mục gồm: tên đầy đủ, nghĩa một câu bằng tiếng Việt, và chỗ nó xuất hiện trong repo.

Xếp theo bảng chữ cái của thuật ngữ. Tra nhanh bằng `Ctrl+F`.

## Luật

- Tài liệu, ADR, PR hay agent dùng một thuật ngữ **chưa có** ở đây: thêm vào trong cùng PR.
- Nghĩa viết cho người mới vào dự án: một câu, không dùng thuật ngữ khác chưa giải thích.
- Một từ có hai nghĩa trong dự án (ví dụ "artifact"): ghi cả hai, nói rõ ngữ cảnh.

## A–C

| Thuật ngữ | Tên đầy đủ / tiếng Anh | Nghĩa | Trong repo |
|---|---|---|---|
| AC | Acceptance criteria (tiêu chí nghiệm thu) | Một hành vi kiểm chứng được mà feature phải đạt, đánh mã `AC-1`, `AC-2`… và mỗi AC có ít nhất một test | `docs/features/<feature>/01-requirements.md`, test tên `AC1_…` |
| ADR | Architecture Decision Record (biên bản quyết định kiến trúc) | File ghi một quyết định kỹ thuật lớn: bối cảnh, lựa chọn, phương án bị loại, hệ quả; đã chốt thì không sửa | [docs/adr/](adr/README.md) |
| Agent | Agent (Claude Code) | Một vai do Claude đóng với hướng dẫn, đầu vào và quyền riêng, ví dụ `ba`, `architect`, `developer` | `.claude/agents/*.md` |
| AI-native SDLC playbook | AI-native SDLC playbook | Hướng dẫn của Anthropic chia quy trình phần mềm có AI thành sáu giai đoạn (Plan, Design, Build, Test, Deploy, Maintain), mỗi giai đoạn để lại một file trong git | [docs/process-roadmap.md](process-roadmap.md) |
| AskUserQuestion | AskUserQuestion (Claude Code) | Tool của Claude Code hiện câu hỏi thành form có ô chọn (tối đa 4 câu, mỗi câu 2–4 lựa chọn, luôn có ô "Other" để tự gõ); phải trả lời đủ mọi tab thì nút Submit mới bấm được | AGT-19 |
| App Service | Azure App Service | Dịch vụ của Microsoft Azure chạy web app .NET mà không phải tự quản máy chủ; blog dùng gói miễn phí F1 | [ADR-0009](adr/0009-hosting.md) |
| axe-core | axe-core | Thư viện tự kiểm lỗi tiếp cận (WCAG) trên trang web, chạy trong acceptance test | `docs/project/nfr.md`, [ADR-0015](adr/0015-ui-test-tooling.md) |
| Analyzer | .NET code analyzer | Bộ luật kiểm code lúc build (đặt tên, lỗi tiềm ẩn, hiệu năng); ở repo này mọi cảnh báo của nó làm build thất bại | `Directory.Build.props`, [ADR-0006](adr/0006-code-quality-gates.md) |
| Artifact (pipeline) | Artifact | Các file mỗi bước của `/feature` để lại: `01-requirements` đến `05-test-report` | `docs/features/<feature>/` |
| Artifact (claude.ai) | Artifact | Trang web Claude tạo và đăng trên claude.ai, mở bằng link, riêng tư mặc định | Không nằm trong repo |
| Backlog | Backlog | Danh sách feature sẽ làm, có thứ tự, phụ thuộc và trạng thái `todo`/`in-progress`/`done` | `docs/project/backlog.md` |
| BEHIND | Branch is behind | Trạng thái PR khi `main` có commit mới sau lúc nhánh tách ra; phải bấm Update branch rồi chờ CI chạy lại | Luật bảo vệ nhánh, [ADR-0002](adr/0002-protected-main-squash-only.md) |
| BOM | Byte Order Mark | Vài byte đánh dấu ở đầu file UTF-8; file `.cshtml` của repo có BOM để tiếng Việt không bị đọc sai | `.editorconfig`, [ADR-0005](adr/0005-line-endings-and-encoding.md) |
| Branch | Nhánh | Một dòng phát triển tách riêng khỏi `main` để làm một việc; xong thì merge qua PR và xóa | `feature/…`, `chore/…`, `fix/…` |
| Branch protection | Bảo vệ nhánh | Luật trên GitHub cho `main`: phải qua PR, check bắt buộc phải xanh, cấm force push và xóa | Cài đặt repo trên GitHub, ADR-0002 |
| Breakpoint | Breakpoint | Chiều rộng màn hình mà từ đó bố cục trang đổi cách sắp xếp; blog dùng 768 px và 1280 px | `docs/project/ui-guidelines.md` |
| Breaking change | Thay đổi phá vỡ tương thích | Thay đổi trong thư viện làm code đang dùng nó không còn chạy như cũ; thường đi kèm bản major | Release notes trong PR của Dependabot |
| C4 | C4 model | Cách vẽ kiến trúc theo 4 mức phóng to dần: Context, Container, Component, Code | `docs/project/architecture.md` |
| Canonical URL | `link rel="canonical"` | Thẻ báo cho công cụ tìm kiếm đâu là địa chỉ chính thức của một trang khi có nhiều URL cùng nội dung | `docs/project/nfr.md` |
| CD | Continuous Delivery / Deployment | Tự động đưa bản build đã qua CI lên môi trường chạy thật; repo chưa có | Lộ trình, chưa triển khai |
| Check bắt buộc | Required status check | Check phải xanh thì PR mới merge được; hiện là `build-and-test` và `foundation-gate` | Bảo vệ nhánh `main` |
| Chore | Chore | Loại thay đổi không ảnh hưởng người dùng (cấu hình, công cụ, tài liệu, dọn dẹp); cũng là tiền tố nhánh `chore/` | CONTRIBUTING.md |
| CI | Continuous Integration | Tự động build, kiểm format và chạy test trên mỗi PR và mỗi push lên `main` | `.github/workflows/ci.yml` |
| CodeQL | CodeQL | Công cụ của GitHub quét code tìm lỗ hổng bảo mật và lỗi chất lượng; kết quả ở tab Security → Code scanning | `.github/workflows/codeql.yml` |
| CODEOWNERS | CODEOWNERS | File chỉ định ai bắt buộc phải duyệt PR đụng tới vùng nào; dự định thêm khi có người thứ hai | Chưa có |
| Container | Container (Docker) | Gói phần mềm chạy cô lập kèm mọi thứ nó cần, bật tắt nhanh; CI chạy PostgreSQL trong một container | [ADR-0012](adr/0012-database-on-ci.md) |
| Context engineering | Context engineering | Chọn đưa vào context của model đúng những gì cần, đúng lúc; lý do agent của pipeline bắt đầu "trắng" và đọc file | Mục "Đọc" trong `.claude/agents/*.md` |
| Conventional Commits | Conventional Commits | Quy ước message commit `loại(phạm vi): mô tả`, ví dụ `feat(about): …`, `fix: …`, `chore: …` | CONTRIBUTING.md |
| Core Web Vitals (LCP, CLS, TBT) | Core Web Vitals | Ba chỉ số tốc độ trang của Google: LCP là lúc khối nội dung lớn nhất hiện ra, CLS là mức bố cục bị xô lệch, TBT là thời gian trang bị kẹt không phản hồi | `docs/project/nfr.md` |
| Cổng chặn | Gate | Điểm kiểm tra tự động không cho đi tiếp khi chưa đủ điều kiện; ở đây là chặn feature khi chưa có tài liệu nền | `/feature` bước 0, job `foundation-gate`, [ADR-0007](adr/0007-project-foundation-gate.md) |
| CRLF / LF | Carriage Return Line Feed / Line Feed | Hai cách đánh dấu xuống dòng: Windows dùng CRLF, Linux dùng LF; repo lưu LF, Git tự đổi khi checkout | `.gitattributes`, ADR-0005 |
| CSP | Content-Security-Policy | Header HTTP liệt kê nguồn nào được phép tải script, style, ảnh; chặn script lạ bị chèn vào trang | `docs/project/nfr.md` |
| CSRF | Cross-Site Request Forgery | Kiểu tấn công lừa trình duyệt của người đã đăng nhập gửi yêu cầu họ không định gửi; chặn bằng antiforgery token trên form | `docs/project/nfr.md` |

## D–L

| Thuật ngữ | Tên đầy đủ / tiếng Anh | Nghĩa | Trong repo |
|---|---|---|---|
| Definition of Done (DoD) | Định nghĩa "xong" | Danh sách điều kiện để một PR được coi là xong: CI xanh, AC có test, không có secret, có ADR và bài học nếu cần | CONTRIBUTING.md, mẫu PR |
| Definition of Ready (DoR) | Định nghĩa "sẵn sàng" | Điều kiện để một việc được bắt đầu; ví dụ feature phải có trong backlog và đủ tài liệu nền | Cổng chặn trong `ba.md` |
| Dependabot | Dependabot | Bot của GitHub tự kiểm thư viện có lỗ hổng hoặc bản mới và mở PR nâng cấp; tài khoản `dependabot[bot]` | `.github/dependabot.yml`, ADR-0006 |
| Dependency | Thư viện phụ thuộc | Thư viện hay công cụ của bên thứ ba mà dự án dùng (xunit, EF Core, Bootstrap, các GitHub Actions) | `*.csproj`, `wwwroot/lib/`, workflow |
| Design tokens | Design tokens | Giá trị thiết kế có tên theo vai trò (màu chữ, cỡ chữ, khoảng cách), dùng thống nhất và thành biến CSS | `docs/project/ui-guidelines.md` |
| EF Core migration | Entity Framework Core migration | File mô tả một bước thay đổi cấu trúc database, sinh từ model C# và áp lần lượt | `src/SimpleBlog.Web/Migrations/` (chưa có) |
| Eval | Evaluation | Bộ bài kiểm tra cố định chạy lại khi sửa cấu hình agent, để biết thay đổi có làm agent tệ đi không; chưa có | docs/process-roadmap.md, giai đoạn 2 |
| Evaluator-optimizer | Evaluator-optimizer | Mẫu thiết kế agent: một bước làm, một bước chấm và gửi lại để sửa; vòng reviewer/tester trả FAIL cho developer là mẫu này | `.claude/skills/feature/SKILL.md` |
| Feature | Tính năng | Một thay đổi người dùng blog thấy được, đi qua pipeline `/feature` | `docs/features/<feature>/` |
| foundation-gate | Foundation gate | Job CI làm đỏ PR đụng `docs/features/**` khi năm tài liệu nền chưa cùng `approved` | `scripts/check-foundation.sh` |
| Force push | Force push | Push ghi đè lịch sử trên GitHub; bị cấm với `main` và với agent | Bảo vệ nhánh, `.claude/settings.json` |
| Forked subagent | Forked subagent (`/subtask`) | Subagent chạy nền thừa hưởng toàn bộ cuộc trò chuyện hiện tại, tạo bằng lệnh `/subtask <việc>`; xem tiến độ bằng `/tasks` | Không dùng trong pipeline (xem Context engineering) |
| Google Analytics | Google Analytics (GA) | Dịch vụ miễn phí của Google đếm người đọc; ở blog này chỉ tải sau khi độc giả bấm đồng ý trên banner cookie | `docs/project/nfr.md`, feature `analytics` |
| GitHub Actions | GitHub Actions | Dịch vụ chạy tự động của GitHub; mỗi workflow là một file YAML gồm các job, mỗi job gồm các step | `.github/workflows/` |
| GITHUB_TOKEN | GITHUB_TOKEN | Token tạm GitHub cấp cho mỗi lần chạy workflow; repo giới hạn nó ở quyền đọc | `permissions:` trong `ci.yml`, SEC-04 |
| Greenfield | Greenfield project | Dự án làm mới từ đầu, chưa có code hay quyết định cũ ràng buộc | ADR-0007 |
| hreflang | `link rel="alternate" hreflang` | Thẻ báo cho công cụ tìm kiếm rằng một trang có bản ở ngôn ngữ khác và bản đó ở URL nào | `docs/project/nfr.md`, [ADR-0011](adr/0011-url-structure-and-language.md) |
| HSTS | HTTP Strict Transport Security | Header bảo trình duyệt từ nay chỉ vào site bằng HTTPS, không bao giờ bằng HTTP | `docs/project/nfr.md` |
| Hook | Hook (Claude Code) | Script Claude Code tự chạy ở thời điểm cố định, như trước khi dùng tool (`PreToolUse`), và chặn được hành động; khai báo trong settings, skill hoặc phần đầu file subagent | Chưa có; docs/process-roadmap.md mục 1.2 |
| Job / Step / Runner | Job / Step / Runner | Runner là máy chạy; job là một nhóm việc chạy trên một runner; step là từng lệnh trong job | `.github/workflows/ci.yml` |
| Kestrel | Kestrel | Web server có sẵn trong ASP.NET Core; acceptance test bật nó thật để trình duyệt Playwright mở được trang qua địa chỉ HTTP | `tests/SimpleBlog.Tests/Acceptance/WalkingSkeleton/` |
| Last Responsible Moment | Thời điểm muộn nhất có trách nhiệm | Nguyên tắc hoãn quyết định tới lúc muộn nhất mà chưa gây tốn kém, để quyết khi có nhiều thông tin hơn | ADR-0007, mục Câu hỏi mở |
| Lessons learned | Bài học rút ra | Ghi lại điều lần sau nên làm khác, theo chủ đề và mã ổn định (`AGT-NN`, `SEC-NN`…) | [docs/lessons-learned.md](lessons-learned.md) |
| Lighthouse / Lighthouse CI | Lighthouse | Công cụ của Google chấm điểm trang web (tốc độ, tiếp cận, SEO) từ 0 đến 100; bản CI chạy trên mỗi PR và làm đỏ PR khi dưới mức | `docs/project/nfr.md`, [ADR-0015](adr/0015-ui-test-tooling.md) |
| LocalDB | SQL Server Express LocalDB | Bản SQL Server nhẹ cho máy dev, chỉ chạy trên Windows; CI trên Ubuntu không có. Sẽ được thay bằng PostgreSQL trước feature `post-editor` | `appsettings.Development.json`, ADR-0003, [ADR-0008](adr/0008-post-storage-database.md) |
| Localizer | `IStringLocalizer` | Dịch vụ của ASP.NET Core lấy chuỗi giao diện theo ngôn ngữ đang hiển thị, thay cho chuỗi viết cứng | `docs/project/architecture.md`, feature `ui-localization` |

## M–R

| Thuật ngữ | Tên đầy đủ / tiếng Anh | Nghĩa | Trong repo |
|---|---|---|---|
| Major / Minor / Patch | Phiên bản lớn / nhỏ / bản vá | Ba phần của số phiên bản SemVer; xem SemVer | `.github/dependabot.yml` |
| Markdig | Markdig | Thư viện .NET chuyển văn bản Markdown thành HTML ở server | [ADR-0014](adr/0014-post-content-format.md) |
| Markdown | Markdown | Cách viết văn bản thuần có định dạng bằng ký hiệu đơn giản (`#` tiêu đề, `**đậm**`, khối code); tác giả viết bài bằng Markdown | [ADR-0014](adr/0014-post-content-format.md) |
| Mermaid | Mermaid | Cách vẽ sơ đồ bằng văn bản trong file Markdown; GitHub tự hiển thị thành hình | `docs/project/architecture.md` |
| Merge | Gộp | Đưa thay đổi của một nhánh vào nhánh khác; repo chỉ cho squash merge vào `main` | ADR-0002 |
| Neon | Neon | Dịch vụ PostgreSQL trên mây có gói miễn phí, tự ngủ khi vắng và thức nhanh; nơi chứa database của blog | [ADR-0008](adr/0008-post-storage-database.md) |
| Nghị định 13 | Nghị định 13/2023/NĐ-CP | Quy định của Việt Nam về bảo vệ dữ liệu cá nhân: phải xin đồng ý, cho rút lại, nêu rõ dữ liệu đi đâu | `docs/project/vision.md`, `nfr.md` |
| NFR | Non-functional requirements (yêu cầu phi chức năng) | Hệ thống phải tốt đến mức nào: nhanh, an toàn, dễ tiếp cận…; mỗi dòng có mức đo được và cách kiểm | `docs/project/nfr.md` |
| Npgsql | Npgsql | Thư viện .NET để kết nối PostgreSQL, kể cả provider cho EF Core (`UseNpgsql`) | [ADR-0008](adr/0008-post-storage-database.md) |
| npm | Node Package Manager | Trình quản lý gói của Node.js; ở đây chỉ dùng trên CI để cài Stylelint (`package.json`), app và máy dev không cần Node | `package.json`, [ADR-0015](adr/0015-ui-test-tooling.md) |
| OAuth | OAuth 2.0 | Chuẩn cho phép đăng nhập bằng tài khoản ở dịch vụ khác (ở đây là GitHub) mà blog không giữ mật khẩu | [ADR-0010](adr/0010-authentication.md) |
| OIDC | OpenID Connect | Chuẩn đăng nhập xây trên OAuth; GitHub Actions dùng nó để deploy lên Azure mà không lưu mật khẩu trong repo | [ADR-0009](adr/0009-hosting.md) |
| Open Graph | Open Graph | Các thẻ `og:` trong trang quy định tiêu đề, mô tả, ảnh hiện ra khi chia sẻ link lên mạng xã hội | `docs/project/nfr.md` |
| Orchestrator-workers | Orchestrator-workers | Mẫu thiết kế agent: một agent chính tự chia việc con tùy đầu vào và giao cho các agent khác; `/feature` không phải mẫu này vì các bước cố định | Building Effective AI Agents |
| OWASP Top 10 | Open Worldwide Application Security Project Top 10 | Danh sách 10 nhóm lỗ hổng web phổ biến nhất, dùng làm chuẩn khi review bảo mật | `reviewer.md`, SECURITY.md |
| Output style | Output style (kiểu trả lời) | Bộ chỉ dẫn quy định giọng, độ dài, cách trình bày của mọi câu trả lời trong phiên; có sẵn default, Proactive, Concise, Explanatory, Learning; không áp cho subagent | `/output-style <tên>`; lưu ở `outputStyle` trong `.claude/settings.local.json` |
| Persona | Chân dung người dùng | Mô tả cụ thể một kiểu độc giả (tuổi, nhu cầu, thiết bị, hoàn cảnh đọc) để suy ra quyết định thiết kế | `docs/project/vision.md` |
| Pipeline | Pipeline | Chuỗi bước chạy lần lượt; ở đây là `ba → architect → developer → reviewer → tester` của `/feature` | `.claude/skills/feature/` |
| Playwright | Playwright | Công cụ điều khiển trình duyệt thật (Chromium, Firefox, WebKit) để test giao diện; repo viết bằng C# | [ADR-0015](adr/0015-ui-test-tooling.md) |
| PostgreSQL | PostgreSQL | Hệ quản trị database mã nguồn mở, chạy được trên Windows, Linux và trong container; database của blog | [ADR-0008](adr/0008-post-storage-database.md) |
| Plan mode | Plan mode | Chế độ Claude Code chỉ đọc code và lập kế hoạch, chưa sửa gì, chờ người duyệt kế hoạch | Claude Code |
| PR | Pull request | Đề nghị gộp một nhánh vào `main`; nơi CI chạy, người đọc, và lưu lý do của thay đổi | `.github/pull_request_template.md` |
| Private vulnerability reporting | Báo lỗ hổng riêng tư | Cách người ngoài báo lỗ hổng bảo mật qua tab Security mà không công khai | SECURITY.md |
| Prompt chaining | Prompt chaining | Mẫu thiết kế agent: các bước cố định nối tiếp, đầu ra bước trước là đầu vào bước sau, có thể có cổng chặn ở giữa; `/feature` là mẫu này | `.claude/skills/feature/SKILL.md` |
| Prompt injection | Prompt injection | Nội dung độc hại (trong issue, comment, trang web) lừa agent AI làm việc khác với yêu cầu | Lộ trình quản trị AI, chưa triển khai |
| Push protection | Push protection | GitHub chặn ngay lúc push nếu phát hiện secret dạng đã biết | SECURITY.md |
| Rate limit (429) | Rate limiting | Giới hạn số yêu cầu trong một khoảng thời gian; vượt mức thì server trả mã 429 Too Many Requests, ví dụ bình luận thứ 6 trong 10 phút | `docs/project/nfr.md` |
| Rebase | Rebase | Đặt lại các commit của nhánh lên trên commit mới nhất của nhánh khác; Dependabot nhận lệnh `@dependabot rebase` | PR của Dependabot |
| Release notes / Changelog | Ghi chú phát hành | Danh sách thay đổi của một phiên bản thư viện; phải đọc trước khi nâng bản major | Mô tả PR của Dependabot |
| rem | Root em | Đơn vị cỡ chữ CSS tính theo cỡ chữ gốc của trình duyệt, nên chữ to theo khi người đọc phóng to | `docs/project/ui-guidelines.md` |
| Retro | Retrospective | Buổi hoặc bước nhìn lại sau một chu kỳ để rút bài học và sửa quy trình | `docs/lessons-learned/` |
| RSS | Really Simple Syndication | Nguồn tin dạng XML để độc giả theo dõi bài mới bằng ứng dụng đọc tin | Feature `rss` |

## S–Z

| Thuật ngữ | Tên đầy đủ / tiếng Anh | Nghĩa | Trong repo |
|---|---|---|---|
| SAST | Static Application Security Testing | Quét mã nguồn tìm lỗ hổng mà không chạy chương trình; CodeQL là một công cụ SAST | `codeql.yml` |
| SBOM | Software Bill of Materials | Danh sách mọi thành phần và thư viện trong một bản build, phục vụ kiểm toán chuỗi cung ứng; chưa có | Lộ trình enterprise |
| Service container | GitHub Actions service container | Container chạy cạnh một job CI (ví dụ PostgreSQL) để test dùng, tắt khi job xong | [ADR-0012](adr/0012-database-on-ci.md) |
| SEO | Search Engine Optimization | Làm trang dễ được công cụ tìm kiếm hiểu và xếp hạng: tiêu đề, mô tả, sitemap, URL ổn định | `docs/project/nfr.md` |
| Secret | Secret | Mật khẩu, API key, token, connection string có thông tin đăng nhập; không bao giờ commit | SECURITY.md |
| Secret scanning | Secret scanning | GitHub quét repo tìm secret đã lọt vào và cảnh báo | SECURITY.md |
| SemVer | Semantic Versioning | Số phiên bản `MAJOR.MINOR.PATCH`: patch sửa lỗi, minor thêm tính năng không phá cái cũ, major có thay đổi phá vỡ tương thích | semver.org; `.github/dependabot.yml` |
| Sitemap | `sitemap.xml` | File liệt kê mọi trang công khai để công cụ tìm kiếm tìm thấy | Feature `sitemap` |
| Skill | Skill (Claude Code) | Một lệnh gõ dạng `/tên` chạy quy trình đã định sẵn, ví dụ `/feature`, `/init-project`, `/sync` | `.claude/skills/` |
| Slug | Slug | Phần cuối URL đọc được, tạo từ tiêu đề bài, ví dụ `/vi/posts/gioi-thieu-ef-core`; không đổi khi sửa tiêu đề | [ADR-0011](adr/0011-url-structure-and-language.md) |
| Squash merge | Squash and merge | Gộp mọi commit của một PR thành một commit duy nhất trên `main`; cách merge duy nhất repo cho phép | ADR-0002 |
| Stylelint | Stylelint | Công cụ kiểm file CSS; ở đây cấm mã màu và cỡ chữ viết cứng ngoài file design tokens | `docs/project/ui-guidelines.md` |
| Superseded | Bị thay thế | PR hoặc ADR không còn hiệu lực vì đã có cái mới thay; PR của Dependabot ghi "Superseded by #N" | PR #7, #12; luật ADR |
| Supply chain | Chuỗi cung ứng phần mềm | Mọi thứ bên thứ ba tham gia tạo ra sản phẩm (thư viện, công cụ build, action CI); một mắt xích bị chèn mã độc là cả sản phẩm bị ảnh hưởng | SEC-03, SEC-04 |
| Testcontainers | Testcontainers | Thư viện để test tự bật và tắt container (ví dụ database); phương án đã cân nhắc nhưng không chọn cho CI | [ADR-0012](adr/0012-database-on-ci.md) |
| TreatWarningsAsErrors | Treat warnings as errors | Cài đặt build coi mọi cảnh báo là lỗi | `Directory.Build.props` |
| User secrets | .NET user-secrets | Nơi lưu secret cho máy dev nằm ngoài thư mục repo, nên không bị commit; dùng cho connection string PostgreSQL có mật khẩu | [ADR-0008](adr/0008-post-storage-database.md), SECURITY.md |
| Walking skeleton | Walking skeleton | Phiên bản mỏng nhất chạy xuyên mọi tầng của hệ thống, làm trước các feature nội dung | F-00 trong `backlog.md` |
| WCAG | Web Content Accessibility Guidelines | Tiêu chuẩn quốc tế để người khuyết tật dùng được web; mức AA là mức thường đặt cho trang công khai | `docs/project/nfr.md` |
| WebEncoderOptions | WebEncoderOptions | Cài đặt của ASP.NET Core cho Razor in nguyên ký tự tiếng Việt thay vì mã hóa thành `&#x...;` | [ADR-0011](adr/0011-url-structure-and-language.md) |
| WebP | WebP | Định dạng ảnh nén nhỏ hơn JPEG/PNG; ảnh thu nhỏ của bài dùng WebP | `docs/project/ui-guidelines.md` |
| WebApplicationFactory | WebApplicationFactory | Lớp của ASP.NET Core chạy cả ứng dụng trong bộ nhớ để acceptance test gửi request thật | `tests/SimpleBlog.Tests/Acceptance/` |
| Workaround | Cách lách | Cách tạm để vượt một trở ngại thay vì sửa gốc; phải ghi ở mục "Cần lưu ý" của PR | CONTRIBUTING.md |
| Workflow | Workflow | Một file YAML trong `.github/workflows/` mô tả việc GitHub Actions chạy và khi nào chạy | `ci.yml`, `codeql.yml` |
| Worktree | Git worktree | Một thư mục làm việc thứ hai của cùng repo, đứng ở nhánh khác, dùng để thử mà không làm bẩn nhánh đang làm | BLD-09 |
| xUnit | xUnit.net | Framework viết test cho .NET mà repo dùng (bản v2, 2.9.3) | `tests/SimpleBlog.Tests/` |
| YAML | YAML Ain't Markup Language | Định dạng file cấu hình dựa trên thụt lề, dùng cho workflow và Dependabot | `.github/` |
