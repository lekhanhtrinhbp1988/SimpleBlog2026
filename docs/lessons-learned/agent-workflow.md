# Lessons learned: Agent workflow

Dựng và vận hành pipeline agent: vai trò, đầu vào, quyền, điểm duyệt của người, cách agent hiểu sai.

Mã `AGT-NN`. Cách viết và luật: xem [mục lục](../lessons-learned.md).

### AGT-01. Pipeline chỉ có tầng feature thì sẽ code feature trước khi có quyết định nền

- **Chuyện gì xảy ra:** feature đầu tiên (trang Giới thiệu) được làm trước khi có bất kỳ quyết định nào về giao diện, độ rộng trang, kiến trúc hay độc giả. Pipeline `ba → architect → developer → reviewer → tester` chạy trơn tru, nhưng không bước nào hỏi "dự án đã sẵn sàng để làm feature chưa".
- **Bài học:** agent làm đúng phạm vi được giao. Nếu không có tầng dự án (tầm nhìn, yêu cầu phi chức năng, hướng dẫn giao diện, kiến trúc, backlog) thì không agent nào tự tạo ra nó.
- **Áp dụng:** có giai đoạn khởi động dự án (`/init-project`) sinh `docs/project/*`, và cổng chặn: `/feature` không chạy khi các file nền chưa được duyệt.

### AGT-02. "Giả định" trong tài liệu yêu cầu là tín hiệu của quyết định còn thiếu

- **Chuyện gì xảy ra:** BA ghi giả định "cách trình bày trang do bước sau quyết định". Giả định này đi qua trạm duyệt mà không ai dừng lại.
- **Bài học:** khi agent phải tự đoán rồi ghi thành giả định, đó thường là một quyết định của con người bị bỏ sót, không phải chi tiết nhỏ.
- **Áp dụng:** ở trạm duyệt, đọc kỹ mục giả định trước mục AC. BA phải báo `BLOCKED` khi cần một quyết định cấp dự án chưa có, thay vì tự giả định.

### AGT-03. Agent chỉ đọc những gì được liệt kê trong mục đầu vào

- **Chuyện gì xảy ra:** mục đầu vào của mọi agent chỉ có thư mục `docs/features/<feature>/`. Kể cả khi có `ui-guidelines.md`, agent cũng sẽ không đọc.
- **Bài học:** viết tài liệu thôi chưa đủ; phải nối nó vào luồng làm việc của agent.
- **Áp dụng:** mỗi khi thêm một tài liệu chung, sửa mục đầu vào của các agent cần dùng nó, và nhắc trong `CLAUDE.md`.

### AGT-04. Mọi đầu ra đều cần có người kiểm, kể cả test

- **Chuyện gì xảy ra:** reviewer chạy trước tester, nên test do tester viết không ai xem. Tester tạo `WebApplicationFactory` bằng reflection để lách việc `Program` là internal, và cách lách này vào thẳng `main`.
- **Bài học:** bước nào sinh ra code thì phải có bước sau kiểm code đó. Agent sẽ chọn cách nhanh nhất để test xanh nếu không ai ngăn.
- **Áp dụng:** thêm lượt review ngắn cho `tests/` sau tester; bắt buộc ghi mọi cách lách vào mục "cần lưu ý" của PR; sửa code cho dễ test (`public partial class Program`) thay vì lách trong test.

### AGT-05. Chặn phía server chắc hơn luật phía client

- **Chuyện gì xảy ra:** ban đầu chỉ có luật `deny git push` trong `.claude/settings.json`. Khi đã bật bảo vệ nhánh `main` trên GitHub, có thể nới luật phía client cho agent push nhánh feature mà vẫn an toàn.
- **Bài học:** luật trong file cấu hình trên máy chỉ có hiệu lực với ai dùng file đó. Luật trên server (bảo vệ nhánh, CI bắt buộc) áp cho tất cả, kể cả người mới hoặc công cụ khác.
- **Áp dụng:** luật quan trọng (không push `main`, CI phải xanh, phải qua PR) đặt ở GitHub. Luật trong `settings.json` chỉ là lớp phụ cho agent.

### AGT-06. Agent không được tự sửa quyền và quy trình của chính nó

- **Chuyện gì xảy ra:** khi đang ở auto mode, Claude Code chặn agent sửa `.claude/settings.json` và skill, kể cả khi người dùng đã đồng ý trong chat. Phải chuyển sang chế độ hỏi duyệt từng lần sửa.
- **Bài học:** đây là thiết kế đúng. Nếu agent tự nới quyền cho mình được thì mọi luật khác đều vô nghĩa.
- **Áp dụng:** thay đổi trong `.claude/` luôn do người duyệt từng lần và đi qua PR như code.

### AGT-07. Quyền của agent đọc từ file trên nhánh đang đứng

- **Chuyện gì xảy ra:** nhánh `chore/editor-config` tách từ `main` cũ, nơi `settings.json` còn chặn push. Agent không push được dù luật mới đã nằm ở một PR khác.
- **Bài học:** đổi quy trình hay quyền thì nên merge trước, rồi mới tạo các nhánh dựa vào quy trình đó.
- **Áp dụng:** PR sửa `.claude/` được ưu tiên merge sớm; nhánh mới luôn tách từ `main` mới nhất.

### AGT-08. Giữ con người ở điểm quyết định, tự động hóa việc hoàn tác được

- **Bài học:** con người chỉ cần giữ hai điểm: duyệt yêu cầu (`01`) và bấm merge. Các bước còn lại (push nhánh feature, mở PR, đồng bộ máy sau merge) có thể hoàn tác hoặc đã có lớp bảo vệ khác, nên giao cho agent.
- **Áp dụng:** `/feature` tự push và mở PR; `gh pr merge` bị chặn với agent; `/sync` dọn máy sau khi người đã merge.

### AGT-09. Câu trả lời ngắn của người dùng có thể mơ hồ

- **Chuyện gì xảy ra:** người dùng trả lời "đồng ý" trong lúc đang bôi đen câu hỏi mở. Có thể hiểu là duyệt tài liệu, cũng có thể hiểu là đồng ý với câu hỏi mở.
- **Bài học:** khi hai cách hiểu dẫn tới hai việc khác nhau, hỏi lại một câu trước khi làm.

### AGT-10. Việc không phải feature cũng cần quy trình

- **Chuyện gì xảy ra:** CI, quy định nhánh, quyền agent, định dạng file đều được làm tay ngoài pipeline, không có review hay test theo tiêu chí.
- **Áp dụng:** thêm `/fix` cho sửa lỗi và việc nhỏ: mô tả, developer, reviewer, test tái hiện, PR.

### AGT-11. Luật chỉ nằm trong tài liệu thì sẽ bị bỏ qua, kể cả bởi agent

- **Chuyện gì xảy ra:** file này ghi rõ "thêm mục mới sau mỗi lần retro", nhưng qua thêm hai PR có bài học mới (#5, #6), agent không cập nhật. Người dùng phải hỏi mới phát hiện.
- **Bài học:** đây là AGT-03 lặp lại ở cấp quy trình. Một việc không có ai, hoặc bước nào, bắt buộc phải làm thì sẽ không được làm.
- **Áp dụng:** Definition of Done và mẫu PR có mục "đã cập nhật lessons learned nếu có bài học mới". Về lâu dài, thêm bước retro vào cuối `/feature` để agent tự đề xuất mục mới.

### AGT-12. `CLAUDE.md` link tới file khác thì agent không tự đọc; phải import

- **Chuyện gì xảy ra:** đề xuất ban đầu là chuyển luật Git sang `CONTRIBUTING.md` và để `CLAUDE.md` "chỉ trỏ tới". Người dùng hỏi lại có vi phạm best practice không, và lúc đó mới thấy: link thường không được nạp vào context, agent sẽ bỏ sót luật.
- **Áp dụng:** dùng import `@CONTRIBUTING.md`, `@README.md` trong `CLAUDE.md`: một nguồn duy nhất mà agent vẫn luôn thấy. Tài liệu dài và chỉ cần cho một số việc (ADR, vision) thì gắn vào mục đầu vào của agent cần nó, không import cho mọi phiên.

### AGT-13. Đề xuất của agent cần được chất vấn

- **Chuyện gì xảy ra:** ở AGT-12, câu hỏi "có đi ngược best practice không?" của người dùng làm lộ chỗ thiếu trong đề xuất của agent.
- **Bài học:** agent trình bày đề xuất rất tự tin, kể cả khi chưa xét hết hệ quả. Hỏi "có vi phạm tiêu chuẩn nào không", "nhược điểm là gì", "phương án khác là gì" là cách rẻ để bắt lỗi trước khi làm.
- **Áp dụng:** với đề xuất thay đổi quy trình hay kiến trúc, yêu cầu agent nêu phương án đã cân nhắc; mẫu PR và mẫu ADR đều có mục này.

### AGT-14. Skill nên tự kiểm tham số

- **Chuyện gì xảy ra:** `/sync chore/contributor-docs'` có dư dấu `'`. Agent tự hiểu là gõ nhầm và làm tiếp đúng ý, nhưng đó là đoán.
- **Áp dụng:** skill kiểm tham số trước khi làm (branch có tồn tại không); sai thì dừng và gợi ý tên gần đúng, thay vì để agent tự đoán.

### AGT-15. Agent tạo PR bằng lệnh thì mẫu PR bị bỏ qua; phải bắt agent dùng mẫu

- **Chuyện gì xảy ra:** mẫu PR có checkbox "có bài học mới thì đã thêm vào lessons learned", và `CLAUDE.md` yêu cầu agent tự thêm bài học. Vậy mà PR #15 và #16 vẫn thiếu bài học, người dùng phải hỏi lần thứ hai. Nguyên nhân: agent tạo PR bằng `gh pr create --body-file` với mô tả tự viết; GitHub chỉ điền mẫu khi tạo PR trên giao diện web, nên checkbox đó không bao giờ xuất hiện để nhắc.
- **Bài học:** một luật được nhắc ở nhiều chỗ vẫn bị bỏ qua nếu chỗ thật sự dùng (lúc viết mô tả PR) không chứa nó. Phải đặt điểm kiểm ngay tại bước thực hiện.
- **Áp dụng:** `CLAUDE.md`, bước 9 của `/feature` và bước cuối của `/init-project` yêu cầu mô tả PR đi theo đúng các mục của `.github/pull_request_template.md`, đánh dấu từng checkbox của Definition of Done (hoặc ghi rõ vì sao không áp dụng).

### AGT-16. Tra tài liệu chính thức trước khi kết luận về một tính năng của công cụ

- **Chuyện gì xảy ra:** người dùng hỏi agent có dùng `/subtask` và `/tasks` chưa. Agent trả lời "không biết lệnh `/subtask`", dựa trên trí nhớ. Người dùng tự tìm thấy nó trong Claude Code Docs. Cũng trong ngày, agent chỉ giới thiệu AI-native SDLC playbook của Anthropic khi người dùng hỏi thẳng, dù nó sát với dự án nhất.
- **Bài học:** công cụ AI ra tính năng mới liên tục; trí nhớ của agent luôn có độ trễ. "Không thấy trong trí nhớ" khác với "không tồn tại".
- **Áp dụng:** câu hỏi về tính năng Claude Code thì tra code.claude.com (hoặc dùng agent `claude-code-guide`); thiết kế agent hay quy trình thì đọc hướng dẫn của Anthropic trước, dẫn nguồn, nêu rõ chỗ đề xuất khác hướng dẫn. Không tìm thấy thì nói "không thấy trong tài liệu", không nói "không tồn tại". Lộ trình quy trình nằm trong `docs/process-roadmap.md`, có link tới các hướng dẫn đó.

### AGT-17. Đối chiếu hướng dẫn ngay cả với đề xuất cũ của chính mình

- **Chuyện gì xảy ra:** AGT-12 đề xuất import `README.md` và `CONTRIBUTING.md` vào `CLAUDE.md` để có một nguồn duy nhất. Khi tra hướng dẫn chính thức lúc làm skill `/design`, thấy khuyến nghị "Keep CLAUDE.md under 200 lines" và "chỉ đưa vào những gì áp dụng rộng". Đo lại: `CLAUDE.md` cộng hai file import là 254 dòng.
- **Bài học:** một đề xuất hợp lý về một mặt (không chép lại nội dung) có thể đi ngược khuyến nghị ở mặt khác (độ dài context luôn nạp). Chỉ nhìn thấy khi đối chiếu với nguồn chính thức, không phải khi tự suy luận.
- **Áp dụng:** skill `/design` bắt buộc mục "Đối chiếu hướng dẫn" có nguồn mở trực tiếp. Việc đưa `CLAUDE.md` về dưới 200 dòng là mục 1.8 của `docs/process-roadmap.md`. Mọi thứ thêm vào `CLAUDE.md` từ nay phải ngắn và trỏ sang skill hay tài liệu.

### AGT-18. Frontmatter YAML sai cú pháp làm agent biến mất mà không báo lỗi; phải kiểm tự động

- **Chuyện gì xảy ra:** khi `/init-project` tới bước `architecture`, gọi agent `architect` nhận lỗi "Agent type 'architect' not found", dù `.claude/agents/architect.md` vẫn nằm đúng chỗ. Nguyên nhân: PR #16 thêm vào `description:` (không có nháy) đoạn `"Chế độ: dự án."`. Trong YAML, `: ` bên trong một giá trị không có nháy là lỗi cú pháp, nên Claude Code bỏ qua cả agent mà không báo gì. `/feature` cũng hỏng ở bước 2 từ PR #16, chỉ là chưa ai chạy. Sửa xong vẫn phải nạp lại phiên thì agent mới xuất hiện.
- **Bài học:** cấu hình agent cũng là code. Lỗi của nó không làm đỏ build, chỉ lộ ra khi đúng agent đó được gọi, có khi nhiều ngày sau. Giá trị YAML có chứa `: ` hay `#` thì phải đặt trong nháy.
- **Áp dụng:** sửa `architect.md` (bọc description trong nháy đơn) trong PR `docs(project): project foundation`. Việc tiếp theo: thêm vào CI một bước parse frontmatter của `.claude/agents/*.md` và `.claude/skills/*/SKILL.md`, đỏ khi lỗi hoặc thiếu `name`/`description`; đưa vào `docs/process-roadmap.md` qua `/design`.

### AGT-19. Hỏi người dùng bằng form có lựa chọn và tóm tắt bằng lời thường, không bằng danh sách dài

- **Chuyện gì xảy ra:** vòng đầu của `/init-project`, agent `product` trả 7 câu hỏi, mỗi câu 3–4 lựa chọn, dạng văn bản liền. Người dùng nói "file vision.md đọc rất mỏi mắt" và hỏi cách hiện câu hỏi thành ô chọn. Chuyển sang tool `AskUserQuestion` (tối đa 4 câu mỗi form, 2–4 lựa chọn mỗi câu, luôn có ô "Other" để tự gõ) thì người dùng trả lời cả 5 file mà không phải đọc lại. Bảng NFR 4 cột cũng khó đọc ("nhiều khi tao đọc mà không hiểu gì"), cho tới khi được chỉ chỉ cần đọc cột "Mức cần đạt" và hỏi 3 câu cho mỗi dòng. Một lần form có nút Submit mờ: phải trả lời đủ mọi tab thì nút mới sáng.
- **Bài học:** tài liệu nền phục vụ hai người đọc: agent cần chi tiết kiểm chứng được, người duyệt cần biết mình đang cam kết điều gì. Đưa nguyên văn câu hỏi của agent cho người dùng là chuyển gánh nặng đọc sang người dùng. Câu hỏi có đề xuất mặc định, dạng ô chọn, giúp người duyệt quyết nhanh mà vẫn đúng (PRC-05).
- **Áp dụng:** trong lần chạy này, người điều phối chuyển câu hỏi của agent thành form và yêu cầu `ux`, `architect`, `product` (backlog) viết mục "Tóm tắt cho người duyệt" ở đầu file. Việc tiếp theo, qua `/design` và PR riêng: ghi hai điều này vào `.claude/skills/init-project/SKILL.md` (trạm duyệt và vòng hỏi đáp dùng `AskUserQuestion`) và vào các agent `product`, `ux`, `architect` (câu hỏi kèm 2–4 lựa chọn có đề xuất; mục tóm tắt bắt buộc).

### AGT-20. Luật ngôn ngữ trả lời chỉ nằm ở prompt hệ thống thì vẫn bị trôi

- **Chuyện gì xảy ra:** cài đặt `language` của Claude Code yêu cầu trả lời bằng tiếng Việt, vậy mà người điều phối `/init-project` trả lời bằng tiếng Anh nhiều lượt liền. Người dùng phải hỏi "tao muốn tiếng Việt thì phải sửa cái gì?" dù luật đã có.
- **Bài học:** luật nói một lần, nằm xa lượt hiện tại, giữa một context phần lớn bằng tiếng Anh (`CLAUDE.md`, mô tả tool), sẽ yếu dần (cùng gốc với AGT-11). Luật phải đúng ở mọi lượt thì nên được nhắc ngay cạnh tin nhắn mới nhất.
- **Áp dụng:** đề xuất hook `UserPromptSubmit` chèn câu nhắc "trả lời bằng tiếng Việt" vào mỗi lượt; làm qua `/design` và PR riêng, vì đụng `.claude/`.

### AGT-21. Bài học chỉ áp dụng tạm trong một lần chạy sẽ mất ở skill khác; phải ghi vào mọi skill liên quan

- **Chuyện gì xảy ra:** AGT-19 đã chốt "hỏi người dùng bằng form `AskUserQuestion`" trong lần chạy `/init-project`, nhưng phần "Áp dụng" để việc ghi vào skill sang một PR sau. Ngay lần chạy `/feature walking-skeleton` tiếp theo, người điều phối làm đúng câu chữ của skill ("chuyển nguyên câu hỏi của BA cho người dùng") và đưa 6 câu hỏi của BA ra dạng bảng. Người dùng phải nhắc lại: "lúc nãy tao đã yêu cầu mày đưa ra câu hỏi tuần tự, dạng lựa chọn".
- **Bài học:** agent làm theo chữ của skill đang chạy, không theo bài học nằm trong file khác. Một yêu cầu về cách giao tiếp với người dùng áp dụng cho mọi skill có hỏi người dùng (`/feature`, `/init-project`, và agent nào trả câu hỏi), nên chỉ sửa một chỗ hoặc hẹn sửa sau thì sẽ lặp lại lỗi.
- **Áp dụng:** trong lần chạy này, người điều phối chuyển sang form cho câu hỏi mở và trạm duyệt, và lưu luật vào bộ nhớ của Claude Code. Việc tiếp theo, qua `/design` và PR `chore` riêng: ghi vào `.claude/skills/feature/SKILL.md` (bước 1 BLOCKED và trạm duyệt dùng `AskUserQuestion`), `.claude/skills/init-project/SKILL.md`, và các agent `ba`, `product`, `ux`, `architect` (mỗi câu hỏi kèm 2–4 phương án, phương án đề xuất đứng đầu).

### AGT-22. 44 AC đạt hết vẫn lọt lỗi nhìn thấy ngay; pipeline giao diện cần một bước nhìn trang thật

- **Chuyện gì xảy ra:** `walking-skeleton` qua đủ năm bước với 44/44 AC đạt và 300 test xanh. Người dùng mở app lần đầu thì thấy ngay hai lỗi: footer nằm lửng giữa màn hình ở trang ngắn, và khung focus xanh bao cả vùng nội dung sau khi dùng liên kết bỏ qua. Cả hai không phải code sai so với thiết kế: `ui-guidelines.md` không quy định vị trí footer, còn AC-14 viết "mọi phần tử có focus đều có viền" mà không nghĩ tới `main`. Developer và tester đều ghi "chưa mở app xem bằng mắt". Phải chạy lại cả pipeline từ BA (thêm AC-45 đến AC-48).
- **Bài học:** test tự động chỉ kiểm điều đã được viết thành AC. Lỗi thẩm mỹ và bố cục hay nằm ở chỗ tài liệu im lặng, nên không AC nào bắt được. Với feature có giao diện, phải có ai đó nhìn trang thật trước khi PR được coi là xong.
- **Áp dụng:** vòng này thêm AC-45 đến AC-48 và ghi chú bổ sung hai quy định vào `ui-guidelines.md` qua PR riêng. Việc tiếp theo, qua `/design`: cho tester chụp ảnh màn hình (Playwright `screenshot`) các trang ở 360 px và 1280 px, đính vào `05-test-report.md`; và thêm vào trạm cuối của `/feature` một dòng nhắc người dùng chạy app xem trước khi merge.

### AGT-23. CI đỏ sau khi mở PR chưa có đường đi trong `/feature`; test kiểm cấu hình CI khóa luôn cách sửa

- **Chuyện gì xảy ra:** PR #23 đỏ trên CI vì hai test Stylelint cần `node_modules` mà job `build-and-test` không cài Node; trên máy dev chúng xanh vì đã `npm install`. `/feature` không có bước "CI đỏ sau khi push", nên người điều phối phải tự ghép một vòng sửa (architect thêm T-14, developer làm). Developer sửa `ci.yml` đúng thiết kế mới thì test AC-30 của tester đỏ, vì nó cấm mọi `--filter`; developer không được sửa acceptance test nên trả `FAIL`, pipeline dừng. Người dùng chọn chạy tiếp `--from tester`, nên T-14 (`ci.yml`, `README.md`, `CONTRIBUTING.md`) không qua reviewer.
- **Bài học:** test đọc cấu hình (CI, project file) là một bản sao của thiết kế; đổi thiết kế thì test đó phải đổi cùng lúc, mà luật quyền tách developer và tester làm việc này cần hai bước. Và một pipeline chỉ coi "test trên máy xanh" là xong thì CI đỏ sẽ rơi ra ngoài quy trình.
- **Áp dụng:** qua `/design` và PR riêng: thêm vào `/feature` bước chờ `gh pr checks` sau khi mở PR và một nhánh "CI đỏ" (đọc log, gọi agent theo nơi lỗi, vẫn đi qua reviewer); khi task của architect đổi cấu hình mà acceptance test có kiểm, ghi rõ trong task để tester cập nhật trong cùng vòng.
