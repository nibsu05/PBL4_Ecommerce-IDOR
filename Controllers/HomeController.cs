using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PBL_3.Models;
using PBL3.Services;
using PBL3.DTO.Buyer;

namespace PBL_3.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ProductService _productService; // Service này sẽ chứa logic Fetch URL

    // Dependency Injection: Đảm bảo ProductService đã được đăng ký trong Program.cs
    public HomeController(ILogger<HomeController> logger, ProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public IActionResult Index()
    {
        try
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách sản phẩm");
            TempData["Error"] = "Có lỗi xảy ra khi tải danh sách sản phẩm";
            return View(new List<Buyer_SanPhamDTO>());
        }
    }

    // =========================================================================
    // PHẦN CÓ LỖ HỔNG SSRF ĐƯỢC THÊM VÀO
    // =========================================================================

    [HttpGet]
    public async Task<IActionResult> CheckImageUrl(string imageUrl) // 👈 Lỗ hổng: Nhận URL từ người dùng
    {
        if (string.IsNullOrEmpty(imageUrl))
        {
            return BadRequest("Vui lòng cung cấp URL hình ảnh.");
        }

        try
        {
            // Controller chuyển URL NGƯỜI DÙNG CUNG CẤP trực tiếp đến Service
            string content = await _productService.FetchUrlContent(imageUrl);

            // Trả về nội dung để attacker dễ dàng xem kết quả khai thác (ví dụ: Metadata)
            return Content(content, "text/plain");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi kiểm tra URL: {Url}", imageUrl);
            // Trả về lỗi 500, nhưng hành vi của server vẫn xảy ra
            return StatusCode(500, $"Không thể lấy nội dung từ URL đã cung cấp. Lỗi: {ex.Message}");
        }
    }

    // =========================================================================
    // CÁC ACTION METHOD CÒN LẠI
    // =========================================================================

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Login()
    {
        // Chuyển hướng đến Login Action trong AccountController
        return RedirectToAction("Login", "Account");
    }

    public IActionResult Register()
    {
        // Chuyển hướng đến Register Action trong AccountController
        return RedirectToAction("Register", "Account");
    }
}