# Lessons learned: Quy trình và tài liệu

Retro, tài liệu cho người và cho agent, mức trưởng thành của quy trình.

Mã `PRC-NN`. Cách viết và luật: xem [mục lục](../lessons-learned.md).

### PRC-01. Nhìn lại sau mỗi chu kỳ

- **Áp dụng:** sau mỗi feature, ghi những gì gặp vấn đề và đề xuất sửa quy trình (retrospective). File này là nơi gom các bài học đó.

### PRC-02. Tài liệu cho người và tài liệu cho agent là hai thứ khác nhau

- **Chuyện gì xảy ra:** repo có `CLAUDE.md` chi tiết cho agent nhưng không có cả `README.md`. Người mới clone về không biết bắt đầu từ đâu.
- **Áp dụng:** `README.md` (chạy dự án trong 10 phút), `CONTRIBUTING.md` (quy trình), `SECURITY.md` (secret và báo lỗ hổng), mẫu PR. `CLAUDE.md` import hai file đầu và chỉ thêm phần riêng cho agent.

### PRC-03. "Enterprise-ready" là nhiều lớp; xây theo giai đoạn thật của dự án

- **Bài học:** ngoài code và test, enterprise cần thêm: review của người bắt buộc, bảo mật code và thư viện, cổng chất lượng, phát hành (phiên bản, môi trường, rollback), vận hành (log, giám sát, runbook), quản lý truy cập, kiểm toán, và quản trị việc dùng AI (eval cho agent, chính sách dữ liệu gửi model, prompt injection, trách nhiệm với code do AI viết).
- **Áp dụng:** không xây hết ngay. Làm cái rẻ trước (CodeQL, Dependabot, warning là lỗi), cái cần khi có người thứ hai (người duyệt bắt buộc, `CODEOWNERS`, organization), cái cần khi đưa lên mạng (CD, staging, giám sát), và chỉ làm phần enterprise thật (SSO, SBOM, chứng nhận tuân thủ) khi khách hàng hoặc luật yêu cầu.

### PRC-04. Tài liệu sống cần cấu trúc mở rộng được và mã ổn định ngay từ đầu

- **Chuyện gì xảy ra:** lessons learned bắt đầu là một file, mã A1–A18 và B1–B17. Sau hai ngày đã 35 mục và người dùng thấy sẽ sớm quá dài. Khi tách theo chủ đề, ADR-0004 đang trích dẫn "A1, A10" bằng đường dẫn cũ, mà ADR đã chốt thì không được sửa.
- **Bài học:** tài liệu được thêm vào liên tục sẽ phải tách. Nếu mã gắn với vị trí (phần A, phần B) thì tách là phải đổi mã, làm gãy các trích dẫn.
- **Áp dụng:** giữ `docs/lessons-learned.md` làm mục lục ở đường dẫn cũ, kèm bảng đổi mã cũ sang mới; mã mới theo chủ đề (`AGT-NN`, `SEC-NN`...), không bao giờ đổi, và giữ nguyên khi mục chuyển sang file khác. Áp dụng cùng cách cho mọi tài liệu sống sau này: đặt mã ổn định, có mục lục, có luật tách file.

### PRC-05. Thiết kế lớn: đưa ADR `Proposed` và bản thiết kế có sẵn đề xuất cho từng câu hỏi

- **Chuyện gì xảy ra:** thiết kế `/init-project` ban đầu chỉ nằm trong chat; người dùng phải hỏi lại "ghi lại cho tao để tao duyệt". Khi ghi thành ADR-0007 (`Proposed`) và `docs/design/project-foundation.md`, với 9 câu hỏi mỗi câu kèm đề xuất, người dùng duyệt bằng một câu "làm như đề xuất".
- **Bài học:** người duyệt cần một tài liệu đọc được, nằm ở chỗ chung và comment được, với các điểm cần quyết đã được tách riêng và có sẵn lựa chọn mặc định. Thiết kế chỉ nằm trong chat thì khó duyệt và mất khi phiên kết thúc.
- **Áp dụng:** thay đổi lớn về quy trình hay kiến trúc: mở PR gồm ADR `Proposed` và bản thiết kế; mục "Câu hỏi cần quyết" có đề xuất cho từng câu; khi người chốt, ghi quyết định và ngày vào bản thiết kế, đổi ADR sang `Accepted`, rồi triển khai trong cùng PR hoặc PR tiếp theo.

### PRC-06. Giải thích thuật ngữ ngay lần đầu dùng, và giữ một bảng thuật ngữ chung

- **Chuyện gì xảy ra:** agent dùng ADR, Dependabot, CodeQL, SemVer suốt nhiều ngày mà không giải thích. Người dùng phải tự tra Google "ADR là gì", và không hiểu vì sao `dependabot[bot]` mở PR #10–#12, rồi #12 tự đóng và #14 xuất hiện.
- **Bài học:** người làm lâu trong nghề, hay agent, quên rằng thuật ngữ quen với mình là rào cản với người khác. Hiểu sai một từ dẫn tới duyệt sai một quyết định. Người mới vào nhóm sẽ gặp đúng những chỗ khó này.
- **Áp dụng:** `docs/glossary.md` định nghĩa mọi từ viết tắt và thuật ngữ quy trình, kèm chỗ dùng trong repo. Definition of Done và mẫu PR có mục "thuật ngữ mới đã có trong glossary". `CLAUDE.md` yêu cầu agent nói tên đầy đủ và nghĩa một dòng ngay lần đầu một thuật ngữ xuất hiện trong cuộc trao đổi, kể cả khi nó đã có trong glossary.
