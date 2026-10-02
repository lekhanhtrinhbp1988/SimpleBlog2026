# 0006. Cổng chất lượng: analyzer, warning thành lỗi, CodeQL, Dependabot

- Trạng thái: Accepted
- Ngày: 2026-10-02
- Người quyết định: Lê Khánh Trình

## Bối cảnh

CI ở [ADR-0003](0003-ci-on-github-actions-ubuntu.md) chỉ kiểm format, build và test. Build chỉ cần không lỗi là qua, warning bị bỏ qua. Không có gì tìm lỗ hổng trong code hay trong thư viện. Code phần lớn do agent viết, nên cần lớp kiểm tự động không phụ thuộc vào việc reviewer (cũng là agent) có để ý hay không.

## Quyết định

- **`Directory.Build.props`** ở thư mục gốc, áp cho mọi project:
  - `AnalysisLevel=latest`, `AnalysisMode=Recommended`: bật bộ luật .NET analyzer mức Recommended.
  - `EnforceCodeStyleInBuild=true`: luật code style trong `.editorconfig` được kiểm khi build.
  - `TreatWarningsAsErrors=true`: mọi warning của compiler và analyzer làm build thất bại, cả trên máy dev lẫn CI.
- **Ngoại lệ duy nhất:** tắt CA1707 (không dùng gạch dưới trong tên) cho `tests/**.cs`, vì tên test cố ý có gạch dưới: acceptance test bắt đầu bằng mã AC (`AC1_...`) để truy vết tới yêu cầu, unit test theo dạng `Method_Scenario`.
- **CodeQL** (`.github/workflows/codeql.yml`): quét C# (build thật bằng SDK trong `global.json`) và GitHub Actions workflow, bộ truy vấn `security-and-quality`, chạy trên mọi PR, mọi push lên `main`, và hằng tuần.
- **Dependabot** (`.github/dependabot.yml`): kiểm NuGet và GitHub Actions hằng tuần. Bản minor và patch gộp một PR; bản major mỗi gói một PR. Tiêu đề PR theo Conventional Commits (`chore(deps)`, `ci(deps)`). Bật thêm Dependabot alerts và security updates trên GitHub.

## Phương án đã cân nhắc

- **`AnalysisMode=All`**: nhiều luật nhất, nhưng sinh nhiều cảnh báo không phù hợp với app MVC nhỏ, dẫn tới tắt hàng loạt luật và mất ý nghĩa.
- **Chỉ bật warning thành lỗi trên CI**: dev chỉ phát hiện khi đã push. Bật ở `Directory.Build.props` để máy dev và CI giống nhau.
- **Đổi tên test bỏ gạch dưới**: mất cách đọc mã AC ngay trong tên test, vốn là nền của việc truy vết yêu cầu tới test.
- **CodeQL default setup (bật bằng nút trên GitHub)**: không cần file, nhưng cấu hình không nằm trong repo, không qua review, và tự chọn SDK có thể lệch `global.json`.

## Hệ quả

- Code mới có warning sẽ không build được. Khi một luật không hợp lý cho một trường hợp, tắt bằng `.editorconfig` cho đúng phạm vi hẹp nhất và ghi lý do bên cạnh; không tắt `TreatWarningsAsErrors`.
- Nâng phiên bản SDK có thể kéo theo luật analyzer mới và làm build đỏ; xử lý trong cùng PR nâng SDK.
- Dependabot sẽ mở PR hằng tuần; vẫn qua CI và người merge như mọi PR khác.
- CodeQL chưa phải check bắt buộc để merge; cân nhắc thêm vào bảo vệ nhánh sau khi đã chạy ổn vài tuần.
- `global.json` ghim SDK chính xác và không được Dependabot theo dõi; nâng SDK làm tay.
