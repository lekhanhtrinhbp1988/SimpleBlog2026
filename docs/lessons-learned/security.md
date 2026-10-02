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
