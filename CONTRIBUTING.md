# Đóng góp cho SimpleBlog

Luật trong file này áp dụng cho cả người và agent. `CLAUDE.md` import file này, nên agent Claude Code luôn đọc nó.

## Tóm tắt

1. Tách branch từ `main` mới nhất, đặt tên theo loại việc.
2. Commit theo Conventional Commits.
3. Mở pull request vào `main`, điền đủ mẫu PR.
4. CI phải xanh, branch phải cập nhật theo `main`.
5. Merge bằng **Squash and merge**. GitHub tự xóa branch trên server; xóa branch trên máy bằng `/sync` hoặc tay.

## Branch

- `main` được bảo vệ: không ai push thẳng, kể cả chủ repo. Mọi thay đổi đi qua PR.
- Tên branch dùng tiếng Anh, kebab-case, có tiền tố theo loại việc:

  | Tiền tố | Dùng cho |
  |---|---|
  | `feature/<tên>` | Tính năng mới, thường do `/feature` tạo. `<tên>` trùng tên thư mục `docs/features/<tên>/` |
  | `fix/<tên>` | Sửa lỗi |
  | `chore/<tên>` | Cấu hình, công cụ, tài liệu, dọn dẹp, cập nhật thư viện |

- Branch sống ngắn: vài ngày, một mục đích. Xong thì merge và xóa.
- **Không làm tiếp trên branch đã squash merge.** Lịch sử của nó không còn khớp với `main`; việc mới thì tách branch mới.

## Commit và tiêu đề PR

Theo [Conventional Commits](https://www.conventionalcommits.org/): `<loại>(<phạm vi>): <mô tả ngắn>`.

| Loại | Khi nào |
|---|---|
| `feat` | Tính năng người dùng thấy được |
| `fix` | Sửa lỗi người dùng gặp |
| `docs` | Chỉ tài liệu |
| `test` | Chỉ test |
| `refactor` | Đổi cấu trúc code, hành vi không đổi |
| `ci` | Cấu hình CI |
| `chore` | Việc khác không ảnh hưởng người dùng |

Khi squash merge, **tiêu đề PR trở thành commit trên `main`**, nên tiêu đề PR phải theo quy ước này. Commit bên trong branch có thể tự do hơn, nhưng nên giữ cùng quy ước.

## Pull request

- Điền đủ mẫu PR, đặc biệt mục **Vì sao** và **Phương án đã cân nhắc**. Đây là nơi lưu lý do của từng thay đổi; người sau đọc `git log` sẽ bấm sang PR để hiểu.
- Mọi cách lách (workaround) phải ghi ở mục **Cần lưu ý**.
- Nếu GitHub báo branch `BEHIND`, bấm **Update branch** rồi chờ CI chạy lại.
- Chỉ có **Squash and merge**; hai cách merge còn lại đã bị tắt.
- Người bấm merge là người. Agent được push branch và mở PR, không được merge.

## CI

`.github/workflows/ci.yml` chạy trên Ubuntu cho mọi PR vào `main` và mọi push lên `main`:

1. `dotnet format --verify-no-changes`
2. `dotnet build -c Release`
3. cài trình duyệt Playwright (Chromium, Firefox, WebKit)
4. `dotnet test -c Release` (loại các test có trait `Category=Stylelint`)

Job `stylelint` cài cả Node và .NET: chạy Stylelint trên CSS của site và `scripts/check-stylelint-rules.sh` để chứng minh luật còn chặn được vi phạm và hai acceptance test `Category=Stylelint` (xem [ADR-0015](docs/adr/0015-ui-test-tooling.md)). Trên máy dev cần `npm install` ở gốc repo để hai test này xanh.

Cùng workflow có job `foundation-gate` (`scripts/check-foundation.sh`): PR nào thêm hoặc sửa `docs/features/**` (trừ `_template/`) sẽ đỏ nếu năm tài liệu nền trong `docs/project/` chưa cùng `status: approved` (xem [ADR-0007](docs/adr/0007-project-foundation-gate.md)). PR `chore`, `fix`, tài liệu khác không bị ảnh hưởng.

CI không có SQL Server LocalDB. Test cần database sẽ cần cách khác trên CI (xem [ADR-0003](docs/adr/0003-ci-on-github-actions-ubuntu.md)).

`.github/workflows/codeql.yml` quét lỗ hổng và chất lượng code C# và workflow trên mọi PR, mọi push lên `main`, và hằng tuần. Kết quả xem ở tab **Security → Code scanning**.

Dependabot mở PR cập nhật NuGet và GitHub Actions hằng tuần, tiêu đề `chore(deps): ...` hoặc `ci(deps): ...`. Xử lý như mọi PR: CI xanh thì đọc changelog của bản major trước khi merge.

## Warning là lỗi

`Directory.Build.props` bật .NET analyzer mức Recommended, kiểm code style khi build, và **coi mọi warning là lỗi**, cả trên máy dev lẫn CI (xem [ADR-0006](docs/adr/0006-code-quality-gates.md)).

- Sửa warning, đừng tắt nó.
- Nếu một luật thật sự không hợp với một trường hợp, tắt bằng `dotnet_diagnostic.<ID>.severity = none` trong `.editorconfig`, cho phạm vi hẹp nhất có thể, kèm một dòng comment giải thích. Ghi việc này ở mục **Cần lưu ý** của PR.
- Không tắt `TreatWarningsAsErrors`, không dùng `#pragma warning disable` rải rác trong code.

## Định dạng file

- `.gitattributes`: trong repo luôn lưu LF; Git đổi sang line ending của hệ điều hành khi checkout.
- `.editorconfig`: UTF-8 không BOM, thụt lề 4 dấu cách (2 cho JSON, YAML, file project). `.cshtml` dùng **UTF-8 có BOM** để tiếng Việt không bị vỡ.
- Chạy `dotnet format` trước khi commit. CI sẽ đỏ nếu còn lệch định dạng.
- Xem [ADR-0005](docs/adr/0005-line-endings-and-encoding.md).

## Khởi động dự án: `/init-project`

Trước feature nội dung đầu tiên, dự án phải có năm tài liệu nền đã được duyệt trong `docs/project/`: `vision.md`, `nfr.md`, `ui-guidelines.md`, `architecture.md`, `backlog.md` (xem [ADR-0007](docs/adr/0007-project-foundation-gate.md)).

Chạy skill `/init-project [yêu cầu ban đầu]`. Agent `product`, `ux`, `architect` soạn lần lượt từng file và hỏi lại khi thiếu thông tin. Sau mỗi file, **người đọc và gõ `duyệt`**; chỉ khi đó file mới được ghi `status: approved`. Agent không bao giờ tự duyệt. Cuối cùng skill mở PR `docs(project): project foundation`.

Sửa một tài liệu nền đã duyệt: qua PR như mọi thay đổi, cập nhật `approved_on`. Đổi quyết định kiến trúc: viết ADR mới thay thế.

**Cổng chặn:** chưa đủ năm file `approved` thì `/feature` dừng ngay ở bước 0, và CI `foundation-gate` đỏ với mọi PR đụng `docs/features/**`. Sau đó, `ba` chỉ nhận feature có trong `backlog.md` ở trạng thái `todo` với phụ thuộc đã `done`, và dừng khi cần một quyết định cấp dự án chưa có, thay vì tự giả định. Feature mới phải được thêm vào backlog (qua PR) trước khi chạy `/feature`.

## Feature: quy trình `/feature`

Feature mới chạy bằng skill `/feature <tên> <mô tả>` của Claude Code:

1. **BA** viết `01-requirements.md` với các AC đánh số. **Người duyệt** ở bước này: đọc kỹ mục giả định và câu hỏi mở.
2. **Architect** viết `02-design.md` và `03-tasks.md`.
3. **Developer** viết code và unit test, tự chạy build và test.
4. **Reviewer** đối chiếu thay đổi với thiết kế, ghi `04-review.md`.
5. **Tester** viết acceptance test cho từng AC, ghi `05-test-report.md`.
6. Agent push branch và mở PR. **Người đọc PR và bấm merge**, rồi chạy `/sync`.

Xem [ADR-0004](docs/adr/0004-agent-pipeline-with-human-gates.md) về vì sao người giữ hai điểm duyệt này.

## Việc không phải feature

Sửa lỗi, cấu hình, tài liệu: hiện làm tay trên branch `fix/` hoặc `chore/`, vẫn qua PR và CI. Quy trình riêng (`/fix`) sẽ được bổ sung.

## Definition of Done

Một PR được coi là xong khi:

- [ ] CI xanh.
- [ ] Với feature: mọi AC trong `01` có test tự động và đạt (bảng trong `05`).
- [ ] Không có secret trong thay đổi (xem [SECURITY.md](SECURITY.md)).
- [ ] Cách lách, nếu có, đã ghi ở mục **Cần lưu ý** của PR.
- [ ] Quyết định kỹ thuật mới có ADR trong `docs/adr/`.
- [ ] Tài liệu liên quan (`README.md`, file này, `CLAUDE.md`) đã cập nhật nếu cách làm thay đổi.
- [ ] Thuật ngữ hay từ viết tắt mới xuất hiện trong thay đổi đã có trong [docs/glossary.md](docs/glossary.md).
- [ ] Có bài học mới (lỗi bất ngờ, cách lách, đề xuất bị sửa lại, quy trình thiếu) thì đã thêm mục vào file chủ đề phù hợp trong `docs/lessons-learned/` (cách viết: [docs/lessons-learned.md](docs/lessons-learned.md)).

## Khi nào viết ADR

Viết ADR khi một quyết định:

- khó hoặc tốn công để đổi về sau (database, cấu trúc URL, cách đăng nhập, nơi deploy), hoặc
- có nhiều phương án hợp lý và người sau có thể thắc mắc vì sao không chọn phương án khác.

Cách viết: xem [docs/adr/README.md](docs/adr/README.md).
