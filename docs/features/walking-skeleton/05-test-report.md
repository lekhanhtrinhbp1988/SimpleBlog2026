# 05 - Test report: walking-skeleton

Acceptance test nằm trong `tests/SimpleBlog.Tests/Acceptance/WalkingSkeleton/` (cộng với `Acceptance/About/` cho AC-32). Ứng dụng chạy bằng `WebApplicationFactory<Program>` với Kestrel thật (`UseKestrel`, `StartServer`); test trình duyệt dùng Playwright (Chromium, Firefox, WebKit) và axe-core. Ứng dụng chưa có DbContext nên không có database test (`SimpleBlog_Test` không cần dùng, `SimpleBlog` không bị chạm tới).

## Kết quả theo AC

| AC | Test | Kết quả |
|---|---|---|
| AC-1 | `LayoutBrowserTests.AC1_MotCotToiDa720px_CanGiua` (3 trình duyệt x 1280, 1920 x 2 trang) | PASS |
| AC-2 | `LayoutBrowserTests.AC2_KhongCoCotThuHai` (3 trình duyệt x 5 độ rộng x 2 trang) | PASS |
| AC-3 | `LayoutBrowserTests.AC3_KhongCuonNgang` (3 x 5 x 2) | PASS |
| AC-4 | `InteractionBrowserTests.AC4_BamTenBlog_VeTrangChu` (3 trình duyệt) | PASS |
| AC-5 | `InteractionBrowserTests.AC5_BamMucGioiThieu_MoTrangGioiThieu` (3 trình duyệt) | PASS |
| AC-6 | `HttpTests.AC6_MenuChiCoMucGioiThieu` (2 trang) | PASS |
| AC-7 | `InteractionBrowserTests.AC7_MucMenuTrangHienTai_DanhDauVaGachChan` (3 trình duyệt) | PASS |
| AC-8 | `LayoutBrowserTests.AC8_HeaderMotHang_TenBlogTraiMenuPhai` (3 x 768, 1280, 1920 x 2) | PASS |
| AC-9 | `LayoutBrowserTests.AC9_HeaderXuongDong_KhongCoNutBaGach` (3 x 320, 360 x 2) | PASS |
| AC-10 | `LayoutBrowserTests.AC10_HeaderKhongDinhKhiCuon` (2 trang, khung nhìn 320 x 150) | PASS |
| AC-11 | `InteractionBrowserTests.AC11_TabDauTien_LienKetBoQuaNhanFocusVaHienRa` (Chromium, Firefox; xem Giới hạn) | PASS |
| AC-12 | `InteractionBrowserTests.AC12_LienKetBoQuaAnKhiChuaCoFocus` (3 trình duyệt) | PASS |
| AC-13 | `InteractionBrowserTests.AC13_EnterTrenLienKetBoQua_FocusVaoMain` (Chromium, Firefox; xem Giới hạn) | PASS |
| AC-14 | `InteractionBrowserTests.AC14_FocusLuonNhinThay` (Chromium, Firefox; xem Giới hạn). Phạm vi mới: chỉ liên kết và nút, đúng 3 phần tử (liên kết bỏ qua, tên blog, mục Giới thiệu); `main` không nằm trong thứ tự Tab | PASS |
| AC-15 | `LayoutBrowserTests.AC15_VungBamDuLon` (3 x 360, 1280 x 2) | PASS |
| AC-16 | `InteractionBrowserTests.AC16_Axe_KhongViPham` (Chromium, 360 và 1280, 2 trang, 4 tag WCAG) | PASS |
| AC-17 | `InteractionBrowserTests.AC17_TatJavaScript_MenuBamDuoc` (3 trình duyệt) | PASS |
| AC-18 | `InteractionBrowserTests.AC18_TatJavaScript_TrangGioiThieuDayDu` (3 trình duyệt) | PASS |
| AC-19 | `InteractionBrowserTests.AC19_FontHeThong_KhongTaiFile` (3 x 2 trang) | PASS |
| AC-20 | `LayoutBrowserTests.AC20_CoChuGocLa16px` (3 x 5 x 2) | PASS |
| AC-21 | `LayoutBrowserTests.AC21_KhongCoChuNhoHon14px` (3 x 360, 1280 x 2) | PASS |
| AC-22 | `LayoutBrowserTests.AC22_Doan18px_DongThua` (3 trình duyệt) | PASS |
| AC-23 | `LayoutBrowserTests.AC23_TieuDeTheoBreakpoint` (3 trình duyệt) | PASS |
| AC-24 | `InteractionBrowserTests.AC24_MoiYeuCauToiChinhSite` (3 x 2 trang) | PASS |
| AC-25 | `InteractionBrowserTests.AC25_KhongConBootstrapJqueryValidation` (3 x 2 trang) | PASS |
| AC-26 | `ProjectFileTests.AC26_TokenDungBangDaDuyet` (so `tokens.css` với bảng trong `ui-guidelines.md`) | PASS |
| AC-27 | `ProjectFileTests.AC27_MoiCapMauDuTuongPhan` | PASS |
| AC-28 | `ProjectFileTests.AC28_CssCuaSiteQuaStylelint` (chạy Stylelint thật) | PASS |
| AC-29 | `ProjectFileTests.AC29_StylelintChanMauVaCoChuVietCung` (chạy Stylelint thật trên file mẫu tạm và `tests/stylelint/violations.css`) | PASS |
| AC-30 | `ProjectFileTests.AC30_CiCaiBaTrinhDuyetVaChayDotnetTestKhongLoc` (kiểm cấu hình `ci.yml`; xem Giới hạn) | PASS |
| AC-31 | `ProjectFileTests.AC31_CiCoJobStylelintDoKhiViPham` (kiểm cấu hình `ci.yml`; xem Giới hạn) | PASS |
| AC-32 | `AboutAcceptanceTests.AC1_...` đến `AC6_...` (6 test của feature `about`, không sửa phần assert) | PASS |
| AC-33 | `HttpTests.AC33_TrangChu_TieuDeVaCauGiaiThich` | PASS |
| AC-34 | `HttpTests.AC34_TrangPrivacyCuaKhungMau_Tra404` (`/Home/Privacy`, `/Privacy`) | PASS |
| AC-35 | `ProjectFileTests.AC35_AcceptanceTestDungTrucTiepLopProgram`; `SkeletonFixture` và `WebFactoryHolder` dùng `WebApplicationFactory<Program>` | PASS |
| AC-36 | `LayoutBrowserTests.AC36_DongBanQuyen_14px_MauMuted` (3 x 2 trang) | PASS |
| AC-37 | `HttpTests.AC37_LangLaVi` (2 trang) | PASS |
| AC-38 | `HttpTests.AC38_TieuDeTrinhDuyetRiengTungTrang` | PASS |
| AC-39 | `HttpTests.AC39_MoTaRiengTungTrang` | PASS |
| AC-40 | `HttpTests.AC40_NoSniff` (2 trang và `/css/site.css`) | PASS |
| AC-41 | `HttpTests.AC41_ReferrerPolicy` (2 trang) | PASS |
| AC-42 | `HttpTests.AC42_CspKhongChoScriptInline` (2 trang) | PASS |
| AC-43 | `InteractionBrowserTests.AC43_KhongViPhamCsp` (3 x 2 trang, không bypass CSP, nghe `securitypolicyviolation` và console) | PASS |
| AC-44 | `InteractionBrowserTests.AC44_TenBlogHienThiDungDauNhay` (3 x 2 trang) | PASS |
| AC-45 | `FooterAndMainFocusBrowserTests.AC45_NoiDungNgan_FooterSatDayKhungNhin` (3 trình duyệt x 360x800, 1280x800, trang chủ, lệch <= 1 px) | PASS |
| AC-46 | `FooterAndMainFocusBrowserTests.AC46_NoiDungDai_FooterNamNgoaiKhungNhin_VaKhongDinh` (3 trình duyệt x 360, 1280; trang Giới thiệu, khung nhìn cao 150 px; kiểm thêm footer cuộn theo trang) | PASS |
| AC-47 | `FooterAndMainFocusBrowserTests.AC47_FooterKhongChongLenNoiDung` (3 trình duyệt x 360, 1280 x 2 trang x khung nhìn cao 1600 và 200 px) | PASS |
| AC-48 | `FooterAndMainFocusBrowserTests.AC48_SauLienKetBoQua_MainKhongVeVongFocus_BanPhim` (Chromium, Firefox; xem Giới hạn) và `..._ChuotVaFocusLapTrinh` (3 trình duyệt; kiểm outline, box-shadow, border của `main`, và liên kết vẫn có outline >= 2 px) | PASS |

48/48 AC PASS.

## Giới hạn của bằng chứng

- **WebKit và phím Tab (AC-11, AC-13, AC-14, AC-48 bản phím).** Playwright WebKit trên Windows và macOS không Tab tới liên kết, chỉ tới điều khiển form (giống Safari mặc định). Đã kiểm bằng trang thử chỉ có `<a>`, `<button>`, `<input>`: Tab bỏ qua `<a>`, `Alt+Tab` cũng vậy. Vì vậy trên máy Windows này các test bàn phím chỉ chạy Chromium và Firefox; WebKit chỉ chạy khi hệ điều hành không phải Windows/macOS (CI Ubuntu). Kết quả WebKit cho ba AC này chưa có bằng chứng cho tới khi CI chạy. Đây là hạn chế của công cụ, không phải lỗi của ứng dụng.
- **AC-30 và AC-31** chỉ kiểm được bằng cách đọc `.github/workflows/ci.yml` (bước cài ba trình duyệt trước `dotnet test`, không `--filter`, không `continue-on-error`, job `stylelint` chạy `npx stylelint`). Việc CI thực sự đỏ khi test đỏ chỉ chứng minh được khi chạy trên GitHub; PR đầu tiên sẽ xác nhận. Stylelint đã được chạy thật ở AC-28, AC-29.
- **AC-16** chạy axe-core trên Chromium với `BypassCSP` (axe cần tiêm script, theo 02). AC-43 không bypass.
- Test trình duyệt cần cài trình duyệt Playwright. Máy này cài bằng `.playwright/node/.../node.exe package/cli.js install chromium firefox webkit` vì không có `pwsh`.
- Trong lần chạy đầu có 11 test đỏ, đều do lỗi trong chính test (assert nhầm vào `href` ở AC-6, khung nhìn AC-10 quá cao để cuộn khỏi header, vòng Tab AC-14 không nhận ra focus quay vòng ở Firefox, hạn chế WebKit ở trên). Đã sửa test; không sửa `src/` và không nới assert của AC.
- `AboutAcceptanceTests.cs` (feature `about`): chỉ thay `WebFactoryHolder` từ reflection sang `WebApplicationFactory<Program>` theo 02 (AC-35); phần assert giữ nguyên.

## Output dotnet test

Lệnh: `dotnet test -c Release` từ gốc repo. `dotnet format --verify-no-changes` sạch. Vòng này dùng `-c Release` vì app đang chạy khóa file Debug (`SimpleBlog.Web.exe`, tiến trình của người dùng, không kill).

```
  Determining projects to restore...
  All projects are up-to-date for restore.
  SimpleBlog.Web -> D:\Projects\SimpleBlog2026\src\SimpleBlog.Web\bin\Debug\net10.0\SimpleBlog.Web.dll
  SimpleBlog.Tests -> D:\Projects\SimpleBlog2026\tests\SimpleBlog.Tests\bin\Debug\net10.0\SimpleBlog.Tests.dll
Test run for D:\Projects\SimpleBlog2026\tests\SimpleBlog.Tests\bin\Debug\net10.0\SimpleBlog.Tests.dll (.NETCoreApp,Version=v10.0)
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:   346, Skipped:     0, Total:   346, Duration: 1 m 44 s - SimpleBlog.Tests.dll (net10.0)
```

## AC chưa đạt

Không.

## Kết luận

48/48 AC đạt, `dotnet test` xanh (346 test). Lưu ý các giới hạn ở trên (WebKit Tab trên Windows, AC-30 và AC-31 xác nhận cuối cùng trên CI).

## Vòng bổ sung (AC-14 sửa phạm vi, AC-45 đến AC-48)

- Test mới nằm ở `Acceptance/WalkingSkeleton/FooterAndMainFocusBrowserTests.cs`. Test AC-14 cũ không phải sửa (đã chỉ kiểm liên kết, nút, đúng 3 phần tử).
- Lần chạy đầu của vòng này có 3 test AC-46 đỏ (Chromium, Firefox, WebKit, 1280 px) với thông báo `dieu kien thu: noi dung phai cao hon khung nhin`: lỗi của test, khung nhìn 300 px cao hơn nội dung trang Giới thiệu ở 1280 px. Đã hạ khung nhìn xuống 150 px; không đổi assert của AC. Ngoài ra không có test nào đỏ (343 test khác đạt).
- Lỗi timeout chập chờn Developer báo: trong lần chạy cuối (346 test) không có test trình duyệt nào timeout, nên chưa có tên test để ghi. `dotnet format --verify-no-changes` sạch.
- `dotnet test` (Debug) không chạy được vì app của người dùng đang khóa `SimpleBlog.Web.exe` (lỗi MSB3021/MSB3027); đã dùng Release theo `CLAUDE.md`. Test lệnh xanh ở Release; output ở mục trên là của lần chạy cuối.
