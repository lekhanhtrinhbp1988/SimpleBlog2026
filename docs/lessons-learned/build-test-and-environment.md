# Lessons learned: Build, test và môi trường

Build tái lập được, chênh lệch môi trường, encoding, analyzer, code dễ test.

Mã `BLD-NN`. Cách viết và luật: xem [mục lục](../lessons-learned.md).

### BLD-01. Build tái lập được

- **Áp dụng:** ghim phiên bản SDK (`global.json`) và công cụ (`dotnet-tools.json`); quy định line ending (`.gitattributes`) và encoding (`.editorconfig`); CI kiểm format. Mục tiêu: máy nào, người nào build cũng ra cùng kết quả.

### BLD-02. Môi trường test giống môi trường thật đến mức cần thiết

- **Chuyện gì xảy ra:** dev dùng SQL Server LocalDB (chỉ có trên Windows), CI chạy Ubuntu. Feature đầu tiên dùng database sẽ làm CI đỏ.
- **Áp dụng:** chọn cách chạy database trong CI (SQL Server trong container) trước feature database đầu tiên, ghi thành ADR. Test dùng database riêng (`SimpleBlog_Test`), không đụng database dev.

### BLD-03. Tiếng Việt có dấu cần được xử lý có chủ đích

- **Chuyện gì xảy ra:** Razor mã hóa ký tự có dấu thành entity (`&#x1EC7;`) khi in qua `@`, làm test so chuỗi trên HTML bị đỏ dù trình duyệt hiển thị đúng. Một số file có BOM, một số không.
- **Áp dụng:** chuỗi tiếng Việt cố định viết thẳng trong `.cshtml`; `.cshtml` lưu UTF-8 có BOM; `.editorconfig` và `dotnet format` trong CI giữ luật này.

### BLD-04. Môi trường của agent không tự cập nhật

- **Chuyện gì xảy ra:** cài GitHub CLI giữa phiên, nhưng terminal của agent không thấy lệnh `gh` cho tới khi mở lại VS Code. App đang chạy khóa file build Debug, agent phải build Release để không tắt app của người dùng.
- **Áp dụng:** cài công cụ xong thì khởi động lại editor; agent không được tắt tiến trình của người dùng.

### BLD-05. Sửa code cho dễ test thay vì lách trong test

- **Áp dụng:** khi test phải dùng reflection hay hack để chạm tới code, đó là tín hiệu code cần sửa (ví dụ `public partial class Program {}` cho `WebApplicationFactory`). Cách lách trong test sẽ bị copy sang test khác và nhân lên.

### BLD-06. Warning bị bỏ qua sẽ tích lại; bật "warning là lỗi" từ đầu thì rẻ

- **Chuyện gì xảy ra:** bật analyzer và warning thành lỗi khi repo còn nhỏ: chỉ một loại lỗi phải xử lý. Ở dự án đã chạy vài năm, cùng việc này thường ra hàng trăm warning, và nhóm sẽ ngại bật.
- **Áp dụng:** bật `TreatWarningsAsErrors` và analyzer ngay từ đầu dự án; sửa warning khi nó mới xuất hiện.

### BLD-07. Bật luật chặt thì đo trước, ngoại lệ thì hẹp nhất có thể

- **Chuyện gì xảy ra:** trước khi bật warning thành lỗi, build thử với analyzer để đếm lỗi. Kết quả chỉ có một loại: CA1707 cấm gạch dưới trong tên method, đụng với quy ước tên test `AC1_...` dùng để truy vết AC.
- **Bài học:** luật chung có thể xung đột với quy ước có chủ đích của dự án. Tắt luật cho cả repo thì mất lợi ích; đổi tên test thì mất truy vết.
- **Áp dụng:** tắt CA1707 chỉ cho `tests/**.cs`, có comment lý do bên cạnh, ghi trong ADR-0006. Mọi ngoại lệ sau này theo cùng cách: phạm vi hẹp nhất, có lý do, ghi ở mục "Cần lưu ý" của PR.

### BLD-08. CI xanh chưa chắc đã chạy test; với bản nâng lớn của công cụ test, kiểm số test đã chạy

- **Chuyện gì xảy ra:** Dependabot mở PR nâng `xunit.runner.visualstudio` từ 3 lên 4, cùng lúc với nâng lớn `Microsoft.NET.Test.Sdk` và `coverlet`. Runner mới có thể không còn nhận test viết bằng xunit v2; khi đó `dotnet test` vẫn có thể báo thành công mà không chạy test nào.
- **Bài học:** CI chỉ chứng minh lệnh chạy xong, không chứng minh nó kiểm được điều cần kiểm. Với thay đổi đụng tới chính công cụ test, phải nhìn vào kết quả, không chỉ vào dấu xanh.
- **Áp dụng:** với PR nâng công cụ test, đọc dòng `Passed! ... Total: N` trong log CI và so với số test trước đó (đã làm cho #10, #11, #12: đều 8/8). Dependabot tách mỗi bản nâng lớn thành một PR riêng, cho cả NuGet lẫn GitHub Actions, để đọc release notes và gỡ ra riêng được.
