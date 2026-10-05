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
| AC-14 | `InteractionBrowserTests.AC14_FocusLuonNhinThay` (Chromium, Firefox; xem Giới hạn) | PASS |
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

44/44 AC PASS.

## Giới hạn của bằng chứng

- **WebKit và phím Tab (AC-11, AC-13, AC-14).** Playwright WebKit trên Windows và macOS không Tab tới liên kết, chỉ tới điều khiển form (giống Safari mặc định). Đã kiểm bằng trang thử chỉ có `<a>`, `<button>`, `<input>`: Tab bỏ qua `<a>`, `Alt+Tab` cũng vậy. Vì vậy trên máy Windows này ba test bàn phím chỉ chạy Chromium và Firefox; WebKit chỉ chạy khi hệ điều hành không phải Windows/macOS (CI Ubuntu). Kết quả WebKit cho ba AC này chưa có bằng chứng cho tới khi CI chạy. Đây là hạn chế của công cụ, không phải lỗi của ứng dụng.
- **AC-30 và AC-31** chỉ kiểm được bằng cách đọc `.github/workflows/ci.yml` (bước cài ba trình duyệt trước `dotnet test`, không `--filter`, không `continue-on-error`, job `stylelint` chạy `npx stylelint`). Việc CI thực sự đỏ khi test đỏ chỉ chứng minh được khi chạy trên GitHub; PR đầu tiên sẽ xác nhận. Stylelint đã được chạy thật ở AC-28, AC-29.
- **AC-16** chạy axe-core trên Chromium với `BypassCSP` (axe cần tiêm script, theo 02). AC-43 không bypass.
- Test trình duyệt cần cài trình duyệt Playwright. Máy này cài bằng `.playwright/node/.../node.exe package/cli.js install chromium firefox webkit` vì không có `pwsh`.
- Trong lần chạy đầu có 11 test đỏ, đều do lỗi trong chính test (assert nhầm vào `href` ở AC-6, khung nhìn AC-10 quá cao để cuộn khỏi header, vòng Tab AC-14 không nhận ra focus quay vòng ở Firefox, hạn chế WebKit ở trên). Đã sửa test; không sửa `src/` và không nới assert của AC.
- `AboutAcceptanceTests.cs` (feature `about`): chỉ thay `WebFactoryHolder` từ reflection sang `WebApplicationFactory<Program>` theo 02 (AC-35); phần assert giữ nguyên.

## Output dotnet test

Lệnh: `dotnet test` từ gốc repo (cấu hình Debug). `dotnet format --verify-no-changes` sạch. Lần chạy `dotnet test -c Release` cũng `Passed: 300`.

```
  Determining projects to restore...
  All projects are up-to-date for restore.
  SimpleBlog.Web -> D:\Projects\SimpleBlog2026\src\SimpleBlog.Web\bin\Debug\net10.0\SimpleBlog.Web.dll
  SimpleBlog.Tests -> D:\Projects\SimpleBlog2026\tests\SimpleBlog.Tests\bin\Debug\net10.0\SimpleBlog.Tests.dll
Test run for D:\Projects\SimpleBlog2026\tests\SimpleBlog.Tests\bin\Debug\net10.0\SimpleBlog.Tests.dll (.NETCoreApp,Version=v10.0)
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:   300, Skipped:     0, Total:   300, Duration: 1 m 52 s - SimpleBlog.Tests.dll (net10.0)
```

## AC chưa đạt

Không.

## Kết luận

44/44 AC đạt, `dotnet test` xanh (300 test). Lưu ý các giới hạn ở trên (WebKit Tab trên Windows, AC-30 và AC-31 xác nhận cuối cùng trên CI).
