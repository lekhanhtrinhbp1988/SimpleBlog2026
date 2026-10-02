# Lessons learned: Bảo mật

Secret, rà soát trước khi public, các lớp bảo vệ trên GitHub.

Mã `SEC-NN`. Cách viết và luật: xem [mục lục](../lessons-learned.md).

### SEC-01. Secret không bao giờ nằm trong file được commit

- **Thói quen cũ:** để key, mật khẩu, connection string trong `appsettings.json`.
- **Vì sao có hại:** ai đọc được repo là đọc được key. Xóa file ở commit sau **không** xóa được key khỏi lịch sử git; với repo public, bot quét GitHub tìm key chỉ trong vài phút.
- **Áp dụng:**
  - Máy dev: `dotnet user-secrets` (lưu ngoài thư mục repo).
  - Server: biến môi trường, hoặc kho secret như Azure Key Vault.
  - `appsettings*.json` chỉ chứa giá trị không nhạy cảm (connection string LocalDB với `Trusted_Connection` là được).
  - Bật GitHub secret scanning và push protection để chặn push có chứa key.
  - **Nếu key đã từng bị commit ở dự án cũ: coi như đã lộ. Thu hồi và tạo key mới**, không chỉ xóa khỏi file.

### SEC-02. Rà repo trước khi public

- **Áp dụng:** trước khi đưa repo lên public, quét toàn bộ lịch sử (không chỉ commit mới nhất) tìm mật khẩu, key, connection string có thông tin đăng nhập.

### SEC-03. Bảo mật nhiều lớp, và phần lớn miễn phí

- **Áp dụng:** với repo public trên GitHub, các lớp sau không tốn tiền: secret scanning, push protection, CodeQL, Dependabot alerts và security updates, private vulnerability reporting. Chúng là lưới an toàn, không thay cho việc tự cẩn thận và review.

### SEC-04. Check trên PR chỉ báo cảnh báo mới; cảnh báo đã có trên `main` phải xem ở tab Security

- **Chuyện gì xảy ra:** khi bật CodeQL ở PR #6, agent kiểm cảnh báo của riêng PR đó (0 cảnh báo) rồi báo "không có vấn đề". Thực ra lần quét đầu tiên trên `main` đã tìm ra `actions/missing-workflow-permissions` ở `ci.yml` (cảnh báo #4), và cảnh báo này nằm im tới PR #16 mới được phát hiện, khi CodeQL comment vào dòng mới thêm.
- **Bài học:** cảnh báo trên PR chỉ gồm những gì PR đó thêm vào. Khi bật một công cụ quét mới, lần quét đầu trên nhánh chính mới cho thấy toàn bộ hiện trạng.
- **Áp dụng:** sau khi bật hay đổi cấu hình công cụ quét, xem **Security → Code scanning** (hoặc `gh api repos/<owner>/<repo>/code-scanning/alerts`) cho nhánh `main`, không chỉ check trên PR. Mọi workflow khai báo `permissions` tối thiểu ở đầu file (`contents: read`), chỉ nới cho job thật sự cần ghi.
