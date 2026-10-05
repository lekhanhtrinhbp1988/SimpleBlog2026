# 0015. Công cụ kiểm giao diện, tiếp cận và hiệu năng

- Trạng thái: Accepted
- Ngày: 2026-10-05
- Người quyết định: Lê Khánh Trình (chọn phương án A ngày 2026-10-05, chờ duyệt để chuyển Accepted)

## Bối cảnh

- `nfr.md` và `ui-guidelines.md` đề xuất Playwright, axe-core, Lighthouse CI, Stylelint và để `architecture.md` chốt công cụ và cách chạy trên CI, không được hạ mức cần đạt.
- Cần kiểm trong trình duyệt thật: không cuộn ngang ở 320 đến 1920 px trên Chromium, Firefox, WebKit; focus; banner cookie; yêu cầu mạng tới tên miền ngoài; 0 vi phạm axe-core; điểm Lighthouse.
- Hiện có: xUnit v2, `Microsoft.AspNetCore.Mvc.Testing`; acceptance test trong `tests/SimpleBlog.Tests/Acceptance/<Feature>/`, do agent tester viết bằng C#.
- `WebApplicationFactory` mặc định chạy server trong bộ nhớ, trình duyệt không gọi được; .NET 10 cho phép nó chạy Kestrel thật trên một cổng.

## Quyết định

Chọn **Phương án A: Playwright viết bằng C# trong project test hiện có, Node chỉ dùng trong CI cho Lighthouse CI và Stylelint.**

- Package `Microsoft.Playwright` và `Deque.AxeCore.Playwright` thêm vào `tests/SimpleBlog.Tests`. Test trình duyệt nằm cùng chỗ acceptance test, viết bằng C# và xUnit, app chạy qua `WebApplicationFactory` với Kestrel thật.
- Lighthouse CI (`@lhci/cli`) và Stylelint chạy bằng `npx` trong job CI riêng, có `package.json` nhỏ ở thư mục gốc chỉ cho công cụ; app không cần Node để build hay chạy.
- Test trình duyệt được đánh dấu (trait) để chạy riêng được; CI cài trình duyệt Playwright trước khi chạy.

Lý do: một ngôn ngữ và một bộ chạy test cho mọi acceptance test, tester agent làm như hiện nay; Lighthouse và Stylelint chỉ có bản Node nên dùng Node đúng chỗ đó.

## Phương án đã cân nhắc

- **A. Playwright .NET trong xUnit, Node chỉ cho Lighthouse CI và Stylelint (đã chọn).** Ưu: như trên. Nhược: thêm hai package test và Node trong CI; test trình duyệt chậm hơn test HTTP; cài trình duyệt trên CI tốn thêm khoảng 1 phút.
- **B. Project test riêng bằng Playwright TypeScript (`@playwright/test`, `@axe-core/playwright`).** Ưu: hệ sinh thái Playwright đầy đủ nhất, có trình xem kết quả. Nhược: thêm một ngôn ngữ và một project test; agent tester phải viết hai loại test; truy vết AC trải ra hai nơi.
- **C. Chỉ test HTML bằng HttpClient, kiểm trình duyệt bằng tay.** Ưu: không thêm gì. Nhược: không kiểm được bố cục, focus, axe-core, Lighthouse theo `nfr.md`; trái yêu cầu đã duyệt. Loại trừ khi `nfr.md` được sửa.

## Đối chiếu hướng dẫn

- Playwright cho .NET: https://playwright.dev/dotnet/docs/intro
- axe-core cho Playwright .NET: https://github.com/dequelabs/axe-core-nuget
- Lighthouse CI: https://github.com/GoogleChrome/lighthouse-ci
- Integration test ASP.NET Core, `WebApplicationFactory`: https://learn.microsoft.com/aspnet/core/test/integration-tests (cần mở lại để xác nhận cách chạy Kestrel thật trong .NET 10)
- Stylelint: https://stylelint.io/

## Hệ quả

- Walking skeleton thêm package, fixture trình duyệt dùng chung, job CI cho Lighthouse và Stylelint, và sửa `Program` thành `public partial class Program`.
- Thời gian CI tăng (ước lượng từ khoảng 40 giây lên vài phút); nếu vượt 10 phút thì tách job hoặc chỉ chạy đủ trình duyệt và độ rộng trên `main`.
- Lighthouse CI cần dữ liệu mẫu (seed) và database; dùng database CI theo ADR-0012.
- Xem lại khi: thời gian CI quá lâu, hoặc Playwright .NET thiếu tính năng cần cho `nfr.md`.
