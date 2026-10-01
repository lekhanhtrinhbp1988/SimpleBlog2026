# 01 - Requirements: gioi-thieu

## Bối cảnh

Người đọc mới vào blog chưa biết blog thuộc về ai và viết để làm gì. Blog cần một trang Giới thiệu cho biết tên blog và nhiệm vụ của nó. Người đọc phải mở được trang này từ menu chính.

Nội dung do chủ blog cung cấp:

- Tên blog: `Lê Khánh Trình`
- Mô tả: `Nhiệm vụ của blog là chia sẻ để giúp cho người nào muốn thay đổi, phát triển, không phân biệt tuổi tác.`

## Phạm vi

### Trong phạm vi

- Một trang Giới thiệu hiển thị tên blog và đoạn mô tả ở trên, đúng từng chữ, kể cả dấu tiếng Việt.
- Một mục "Giới thiệu" trên menu chính để mở trang đó.
- Mục menu xuất hiện trên mọi trang dùng bố cục chung của site.

### Ngoài phạm vi

- Sửa tên blog hay đoạn mô tả qua giao diện (trang quản trị, form chỉnh sửa). Nội dung là cố định.
- Lưu nội dung giới thiệu vào cơ sở dữ liệu.
- Ảnh đại diện, thông tin liên hệ, link mạng xã hội, form liên hệ.
- Đa ngôn ngữ: trang chỉ có tiếng Việt.
- Đổi tên hoặc thay nội dung chỗ khác của site đang hiển thị tên ứng dụng (tiêu đề site, footer, trang chủ).
- Đăng nhập, phân quyền: trang công khai, ai cũng xem được.
- Bỏ hay sửa các mục menu khác đang có.

## Acceptance criteria

### AC-1: Menu có mục Giới thiệu

- Given người dùng mở trang chủ của blog
- When người dùng nhìn vào menu chính
- Then menu có một mục với nhãn "Giới thiệu"

### AC-2: Bấm mục menu mở trang Giới thiệu

- Given người dùng đang ở trang chủ
- When người dùng bấm mục "Giới thiệu" trên menu
- Then trình duyệt chuyển tới trang Giới thiệu và trang tải thành công (không phải trang lỗi hay trang không tìm thấy)

### AC-3: Trang hiển thị tên blog

- Given người dùng đang ở trang Giới thiệu
- When trang tải xong
- Then trang hiển thị đúng chuỗi `Lê Khánh Trình` với đầy đủ dấu tiếng Việt

### AC-4: Trang hiển thị đoạn mô tả

- Given người dùng đang ở trang Giới thiệu
- When trang tải xong
- Then trang hiển thị đúng chuỗi `Nhiệm vụ của blog là chia sẻ để giúp cho người nào muốn thay đổi, phát triển, không phân biệt tuổi tác.` với đầy đủ dấu tiếng Việt

### AC-5: Mục menu có mặt ngay trên trang Giới thiệu

- Given người dùng đang ở trang Giới thiệu
- When người dùng nhìn vào menu chính
- Then menu vẫn có mục "Giới thiệu", giống như trên trang chủ

### AC-6: Mở trực tiếp trang Giới thiệu không cần đi qua menu

- Given người dùng có địa chỉ của trang Giới thiệu (ví dụ được bookmark hoặc gửi link)
- When người dùng mở thẳng địa chỉ đó trong một phiên trình duyệt mới
- Then trang Giới thiệu tải thành công và không yêu cầu đăng nhập

## Giả định

- Nhãn trên menu là đúng chữ "Giới thiệu".
- Tên blog và đoạn mô tả là nội dung cố định, không thay đổi thường xuyên; muốn đổi thì sửa và triển khai lại.
- "Tên blog" ở đây là nội dung hiển thị trên trang Giới thiệu; yêu cầu không đòi thay tên site đang hiển thị ở các chỗ khác.
- Thứ tự vị trí của mục "Giới thiệu" trong menu không quan trọng, miễn là nhìn thấy được trong menu chính.
- Tiêu đề tab trình duyệt của trang nên có chữ "Giới thiệu" để dễ nhận biết, nhưng đây là khuyến nghị, không phải điều kiện nghiệm thu.
- Cách trình bày (cỡ chữ, bố cục, tên blog làm tiêu đề, mô tả làm đoạn văn) do bước sau quyết định, miễn là thỏa AC-3 và AC-4.

## Câu hỏi mở

- Có muốn hiển thị tên blog `Lê Khánh Trình` ở các chỗ khác của site (tiêu đề site trên thanh menu, footer, tiêu đề tab) thay cho tên ứng dụng hiện tại không? Hiện coi là ngoài phạm vi; không ảnh hưởng các AC đã viết.
