# Lessons learned

Những bài học rút ra khi dựng SimpleBlog và pipeline agent của nó. Mỗi mục gồm: chuyện gì đã xảy ra, bài học, và cách áp dụng trong repo này.

File này là tài liệu sống: sau mỗi feature hoặc mỗi lần retro, thêm mục mới vào cuối phần tương ứng. Không sửa mục cũ cho "đẹp"; nếu bài học cũ hóa ra sai, thêm mục mới nói rõ vì sao.

Phần A nói về việc dựng workflow cho agent. Phần B nói về vòng đời phát triển phần mềm (SDLC) nói chung, áp dụng được cho cả dự án không dùng agent.

---

## A. Dựng agent workflow

### A1. Pipeline chỉ có tầng feature thì sẽ code feature trước khi có quyết định nền

- **Chuyện gì xảy ra:** feature đầu tiên (trang Giới thiệu) được làm trước khi có bất kỳ quyết định nào về giao diện, độ rộng trang, kiến trúc hay độc giả. Pipeline `ba → architect → developer → reviewer → tester` chạy trơn tru, nhưng không bước nào hỏi "dự án đã sẵn sàng để làm feature chưa".
- **Bài học:** agent làm đúng phạm vi được giao. Nếu không có tầng dự án (tầm nhìn, yêu cầu phi chức năng, hướng dẫn giao diện, kiến trúc, backlog) thì không agent nào tự tạo ra nó.
- **Áp dụng:** có giai đoạn khởi động dự án (`/init-project`) sinh `docs/project/*`, và cổng chặn: `/feature` không chạy khi các file nền chưa được duyệt.

### A2. "Giả định" trong tài liệu yêu cầu là tín hiệu của quyết định còn thiếu

- **Chuyện gì xảy ra:** BA ghi giả định "cách trình bày trang do bước sau quyết định". Giả định này đi qua trạm duyệt mà không ai dừng lại.
- **Bài học:** khi agent phải tự đoán rồi ghi thành giả định, đó thường là một quyết định của con người bị bỏ sót, không phải chi tiết nhỏ.
- **Áp dụng:** ở trạm duyệt, đọc kỹ mục giả định trước mục AC. BA phải báo `BLOCKED` khi cần một quyết định cấp dự án chưa có, thay vì tự giả định.

### A3. Agent chỉ đọc những gì được liệt kê trong mục đầu vào

- **Chuyện gì xảy ra:** mục đầu vào của mọi agent chỉ có thư mục `docs/features/<feature>/`. Kể cả khi có `ui-guidelines.md`, agent cũng sẽ không đọc.
- **Bài học:** viết tài liệu thôi chưa đủ; phải nối nó vào luồng làm việc của agent.
- **Áp dụng:** mỗi khi thêm một tài liệu chung, sửa mục đầu vào của các agent cần dùng nó, và nhắc trong `CLAUDE.md`.

### A4. Mọi đầu ra đều cần có người kiểm, kể cả test

- **Chuyện gì xảy ra:** reviewer chạy trước tester, nên test do tester viết không ai xem. Tester tạo `WebApplicationFactory` bằng reflection để lách việc `Program` là internal, và cách lách này vào thẳng `main`.
- **Bài học:** bước nào sinh ra code thì phải có bước sau kiểm code đó. Agent sẽ chọn cách nhanh nhất để test xanh nếu không ai ngăn.
- **Áp dụng:** thêm lượt review ngắn cho `tests/` sau tester; bắt buộc ghi mọi cách lách vào mục "cần lưu ý" của PR; sửa code cho dễ test (`public partial class Program`) thay vì lách trong test.

### A5. Chặn phía server chắc hơn luật phía client

- **Chuyện gì xảy ra:** ban đầu chỉ có luật `deny git push` trong `.claude/settings.json`. Khi đã bật bảo vệ nhánh `main` trên GitHub, có thể nới luật phía client cho agent push nhánh feature mà vẫn an toàn.
- **Bài học:** luật trong file cấu hình trên máy chỉ có hiệu lực với ai dùng file đó. Luật trên server (bảo vệ nhánh, CI bắt buộc) áp cho tất cả, kể cả người mới hoặc công cụ khác.
- **Áp dụng:** luật quan trọng (không push `main`, CI phải xanh, phải qua PR) đặt ở GitHub. Luật trong `settings.json` chỉ là lớp phụ cho agent.

### A6. Agent không được tự sửa quyền và quy trình của chính nó

- **Chuyện gì xảy ra:** khi đang ở auto mode, Claude Code chặn agent sửa `.claude/settings.json` và skill, kể cả khi người dùng đã đồng ý trong chat. Phải chuyển sang chế độ hỏi duyệt từng lần sửa.
- **Bài học:** đây là thiết kế đúng. Nếu agent tự nới quyền cho mình được thì mọi luật khác đều vô nghĩa.
- **Áp dụng:** thay đổi trong `.claude/` luôn do người duyệt từng lần và đi qua PR như code.

### A7. Quyền của agent đọc từ file trên nhánh đang đứng

- **Chuyện gì xảy ra:** nhánh `chore/editor-config` tách từ `main` cũ, nơi `settings.json` còn chặn push. Agent không push được dù luật mới đã nằm ở một PR khác.
- **Bài học:** đổi quy trình hay quyền thì nên merge trước, rồi mới tạo các nhánh dựa vào quy trình đó.
- **Áp dụng:** PR sửa `.claude/` được ưu tiên merge sớm; nhánh mới luôn tách từ `main` mới nhất.

### A8. Giữ con người ở điểm quyết định, tự động hóa việc hoàn tác được

- **Bài học:** con người chỉ cần giữ hai điểm: duyệt yêu cầu (`01`) và bấm merge. Các bước còn lại (push nhánh feature, mở PR, đồng bộ máy sau merge) có thể hoàn tác hoặc đã có lớp bảo vệ khác, nên giao cho agent.
- **Áp dụng:** `/feature` tự push và mở PR; `gh pr merge` bị chặn với agent; `/sync` dọn máy sau khi người đã merge.

### A9. Câu trả lời ngắn của người dùng có thể mơ hồ

- **Chuyện gì xảy ra:** người dùng trả lời "đồng ý" trong lúc đang bôi đen câu hỏi mở. Có thể hiểu là duyệt tài liệu, cũng có thể hiểu là đồng ý với câu hỏi mở.
- **Bài học:** khi hai cách hiểu dẫn tới hai việc khác nhau, hỏi lại một câu trước khi làm.

### A10. Việc không phải feature cũng cần quy trình

- **Chuyện gì xảy ra:** CI, quy định nhánh, quyền agent, định dạng file đều được làm tay ngoài pipeline, không có review hay test theo tiêu chí.
- **Áp dụng:** thêm `/fix` cho sửa lỗi và việc nhỏ: mô tả, developer, reviewer, test tái hiện, PR.

### A11. Đặt quy ước đặt tên từ đầu

- **Chuyện gì xảy ra:** feature đầu tiên tên `gioi-thieu`, sau phải đổi thành `about` ở branch, thư mục tài liệu, class test và namespace.
- **Áp dụng:** tên branch, thư mục và code dùng tiếng Anh, kebab-case cho branch và thư mục. Nội dung hiển thị cho người đọc vẫn là tiếng Việt.

### A12. Tiếng Việt có dấu cần được xử lý có chủ đích

- **Chuyện gì xảy ra:** Razor mã hóa ký tự có dấu thành entity (`&#x1EC7;`) khi in qua `@`, làm test so chuỗi trên HTML bị đỏ dù trình duyệt hiển thị đúng. Một số file có BOM, một số không.
- **Áp dụng:** chuỗi tiếng Việt cố định viết thẳng trong `.cshtml`; `.cshtml` lưu UTF-8 có BOM; `.editorconfig` và `dotnet format` trong CI giữ luật này.

### A13. Môi trường của agent không tự cập nhật

- **Chuyện gì xảy ra:** cài GitHub CLI giữa phiên, nhưng terminal của agent không thấy lệnh `gh` cho tới khi mở lại VS Code. App đang chạy khóa file build Debug, agent phải build Release để không tắt app của người dùng.
- **Áp dụng:** cài công cụ xong thì khởi động lại editor; agent không được tắt tiến trình của người dùng.

### A14. Luật chỉ nằm trong tài liệu thì sẽ bị bỏ qua, kể cả bởi agent

- **Chuyện gì xảy ra:** file này ghi rõ "thêm mục mới sau mỗi lần retro", nhưng qua thêm hai PR có bài học mới (#5, #6), agent không cập nhật. Người dùng phải hỏi mới phát hiện.
- **Bài học:** đây là A3 lặp lại ở cấp quy trình. Một việc không có ai, hoặc bước nào, bắt buộc phải làm thì sẽ không được làm.
- **Áp dụng:** Definition of Done và mẫu PR có mục "đã cập nhật lessons learned nếu có bài học mới". Về lâu dài, thêm bước retro vào cuối `/feature` để agent tự đề xuất mục mới.

### A15. `CLAUDE.md` link tới file khác thì agent không tự đọc; phải import

- **Chuyện gì xảy ra:** đề xuất ban đầu là chuyển luật Git sang `CONTRIBUTING.md` và để `CLAUDE.md` "chỉ trỏ tới". Người dùng hỏi lại có vi phạm best practice không, và lúc đó mới thấy: link thường không được nạp vào context, agent sẽ bỏ sót luật.
- **Áp dụng:** dùng import `@CONTRIBUTING.md`, `@README.md` trong `CLAUDE.md`: một nguồn duy nhất mà agent vẫn luôn thấy. Tài liệu dài và chỉ cần cho một số việc (ADR, vision) thì gắn vào mục đầu vào của agent cần nó, không import cho mọi phiên.

### A16. Đề xuất của agent cần được chất vấn

- **Chuyện gì xảy ra:** ở A15, câu hỏi "có đi ngược best practice không?" của người dùng làm lộ chỗ thiếu trong đề xuất của agent.
- **Bài học:** agent trình bày đề xuất rất tự tin, kể cả khi chưa xét hết hệ quả. Hỏi "có vi phạm tiêu chuẩn nào không", "nhược điểm là gì", "phương án khác là gì" là cách rẻ để bắt lỗi trước khi làm.
- **Áp dụng:** với đề xuất thay đổi quy trình hay kiến trúc, yêu cầu agent nêu phương án đã cân nhắc; mẫu PR và mẫu ADR đều có mục này.

### A17. Bật luật chặt thì đo trước, ngoại lệ thì hẹp nhất có thể

- **Chuyện gì xảy ra:** trước khi bật warning thành lỗi, build thử với analyzer để đếm lỗi. Kết quả chỉ có một loại: CA1707 cấm gạch dưới trong tên method, đụng với quy ước tên test `AC1_...` dùng để truy vết AC.
- **Bài học:** luật chung có thể xung đột với quy ước có chủ đích của dự án. Tắt luật cho cả repo thì mất lợi ích; đổi tên test thì mất truy vết.
- **Áp dụng:** tắt CA1707 chỉ cho `tests/**.cs`, có comment lý do bên cạnh, ghi trong ADR-0006. Mọi ngoại lệ sau này theo cùng cách: phạm vi hẹp nhất, có lý do, ghi ở mục "Cần lưu ý" của PR.

### A18. Skill nên tự kiểm tham số

- **Chuyện gì xảy ra:** `/sync chore/contributor-docs'` có dư dấu `'`. Agent tự hiểu là gõ nhầm và làm tiếp đúng ý, nhưng đó là đoán.
- **Áp dụng:** skill kiểm tham số trước khi làm (branch có tồn tại không); sai thì dừng và gợi ý tên gần đúng, thay vì để agent tự đoán.

---

## B. Vòng đời phát triển phần mềm (SDLC)

### B1. Branch sống ngắn và bị xóa sau khi merge

- **Thói quen cũ:** branch để lại tùm lum sau khi xong việc.
- **Vì sao có hại:** không ai biết branch nào còn dùng, branch nào đã merge, branch nào bỏ dở; dễ làm tiếp trên branch cũ rồi xung đột.
- **Áp dụng:** branch đặt tên theo loại (`feature/`, `chore/`, `fix/`), sống vài ngày, merge bằng squash, rồi xóa. GitHub tự xóa branch trên server sau merge; `/sync` xóa branch trên máy. Không làm tiếp trên branch đã squash.

### B2. Secret không bao giờ nằm trong file được commit

- **Thói quen cũ:** để key, mật khẩu, connection string trong `appsettings.json`.
- **Vì sao có hại:** ai đọc được repo là đọc được key. Xóa file ở commit sau **không** xóa được key khỏi lịch sử git; với repo public, bot quét GitHub tìm key chỉ trong vài phút.
- **Áp dụng:**
  - Máy dev: `dotnet user-secrets` (lưu ngoài thư mục repo).
  - Server: biến môi trường, hoặc kho secret như Azure Key Vault.
  - `appsettings*.json` chỉ chứa giá trị không nhạy cảm (connection string LocalDB với `Trusted_Connection` là được).
  - Bật GitHub secret scanning và push protection để chặn push có chứa key.
  - **Nếu key đã từng bị commit ở dự án cũ: coi như đã lộ. Thu hồi và tạo key mới**, không chỉ xóa khỏi file.

### B3. `main` được bảo vệ, mọi thay đổi đi qua PR và CI

- **Áp dụng:** không ai push thẳng `main`, kể cả chủ repo. CI phải xanh, branch phải cập nhật theo `main` trước khi merge. "Chạy được trên máy tôi" không phải bằng chứng; CI mới là bằng chứng.

### B4. Message commit có cấu trúc

- **Áp dụng:** Conventional Commits (`feat`, `fix`, `docs`, `chore`, `ci`, `refactor`, `test`). Với squash merge, tiêu đề PR trở thành commit trên `main`, nên tiêu đề PR phải theo quy ước. Lợi ích: đọc `git log` là biết loại thay đổi, sinh được changelog tự động.

### B5. Yêu cầu phải kiểm chứng được

- **Áp dụng:** mỗi yêu cầu có acceptance criteria đánh số (`AC-1`, `AC-2`...). Mỗi AC có ít nhất một test tự động mang đúng mã AC. "Xong" nghĩa là mọi AC có test xanh.

### B6. Quyết định nền trước, nhưng không quyết định mọi thứ ngay

- **Áp dụng:**
  - Chốt trước feature đầu tiên những thứ đổi về sau rất tốn: độc giả và mục tiêu, layout và thang chữ, cấu trúc URL, đa ngôn ngữ, cách lưu dữ liệu, đăng nhập, nơi deploy.
  - Hoãn những thứ rẻ để đổi, nhưng ghi lại thành câu hỏi mở có hạn chót (nguyên tắc Last Responsible Moment).
  - Feature đầu tiên là walking skeleton: một lát cắt mỏng chạy xuyên mọi tầng.

### B7. Ghi lại "tại sao", không chỉ "cái gì"

- **Áp dụng:** quyết định kỹ thuật lớn ghi thành ADR (Architecture Decision Record) trong `docs/adr/`: bối cảnh, các phương án, lựa chọn, hệ quả. Code cho biết hệ thống làm gì; ADR cho biết vì sao nó được làm như vậy. Người mới đọc ADR sẽ không đề xuất lại phương án đã bị loại mà không biết lý do.

### B8. Build tái lập được

- **Áp dụng:** ghim phiên bản SDK (`global.json`) và công cụ (`dotnet-tools.json`); quy định line ending (`.gitattributes`) và encoding (`.editorconfig`); CI kiểm format. Mục tiêu: máy nào, người nào build cũng ra cùng kết quả.

### B9. Môi trường test giống môi trường thật đến mức cần thiết

- **Chuyện gì xảy ra:** dev dùng SQL Server LocalDB (chỉ có trên Windows), CI chạy Ubuntu. Feature đầu tiên dùng database sẽ làm CI đỏ.
- **Áp dụng:** chọn cách chạy database trong CI (SQL Server trong container) trước feature database đầu tiên, ghi thành ADR. Test dùng database riêng (`SimpleBlog_Test`), không đụng database dev.

### B10. Rà repo trước khi public

- **Áp dụng:** trước khi đưa repo lên public, quét toàn bộ lịch sử (không chỉ commit mới nhất) tìm mật khẩu, key, connection string có thông tin đăng nhập.

### B11. Sửa code cho dễ test thay vì lách trong test

- **Áp dụng:** khi test phải dùng reflection hay hack để chạm tới code, đó là tín hiệu code cần sửa (ví dụ `public partial class Program {}` cho `WebApplicationFactory`). Cách lách trong test sẽ bị copy sang test khác và nhân lên.

### B12. Nhìn lại sau mỗi chu kỳ

- **Áp dụng:** sau mỗi feature, ghi những gì gặp vấn đề và đề xuất sửa quy trình (retrospective). File này là nơi gom các bài học đó.

### B13. Tài liệu cho người và tài liệu cho agent là hai thứ khác nhau

- **Chuyện gì xảy ra:** repo có `CLAUDE.md` chi tiết cho agent nhưng không có cả `README.md`. Người mới clone về không biết bắt đầu từ đâu.
- **Áp dụng:** `README.md` (chạy dự án trong 10 phút), `CONTRIBUTING.md` (quy trình), `SECURITY.md` (secret và báo lỗ hổng), mẫu PR. `CLAUDE.md` import hai file đầu và chỉ thêm phần riêng cho agent.

### B14. Ghi ADR lúc quyết định, không phải sau

- **Chuyện gì xảy ra:** ADR-0002 đến 0005 được viết bù cho các quyết định đã làm từ vài ngày trước. Lý do còn nhớ được vì mới xảy ra; sau vài tháng sẽ mất.
- **Áp dụng:** quyết định kỹ thuật lớn có ADR trong cùng PR với thay đổi đó (đã có trong Definition of Done). ADR đã chốt không sửa; đổi ý thì viết ADR mới thay thế.

### B15. Warning bị bỏ qua sẽ tích lại; bật "warning là lỗi" từ đầu thì rẻ

- **Chuyện gì xảy ra:** bật analyzer và warning thành lỗi khi repo còn nhỏ: chỉ một loại lỗi phải xử lý. Ở dự án đã chạy vài năm, cùng việc này thường ra hàng trăm warning, và nhóm sẽ ngại bật.
- **Áp dụng:** bật `TreatWarningsAsErrors` và analyzer ngay từ đầu dự án; sửa warning khi nó mới xuất hiện.

### B16. Bảo mật nhiều lớp, và phần lớn miễn phí

- **Áp dụng:** với repo public trên GitHub, các lớp sau không tốn tiền: secret scanning, push protection, CodeQL, Dependabot alerts và security updates, private vulnerability reporting. Chúng là lưới an toàn, không thay cho việc tự cẩn thận và review.

### B17. "Enterprise-ready" là nhiều lớp; xây theo giai đoạn thật của dự án

- **Bài học:** ngoài code và test, enterprise cần thêm: review của người bắt buộc, bảo mật code và thư viện, cổng chất lượng, phát hành (phiên bản, môi trường, rollback), vận hành (log, giám sát, runbook), quản lý truy cập, kiểm toán, và quản trị việc dùng AI (eval cho agent, chính sách dữ liệu gửi model, prompt injection, trách nhiệm với code do AI viết).
- **Áp dụng:** không xây hết ngay. Làm cái rẻ trước (CodeQL, Dependabot, warning là lỗi), cái cần khi có người thứ hai (người duyệt bắt buộc, `CODEOWNERS`, organization), cái cần khi đưa lên mạng (CD, staging, giám sát), và chỉ làm phần enterprise thật (SSO, SBOM, chứng nhận tuân thủ) khi khách hàng hoặc luật yêu cầu.
