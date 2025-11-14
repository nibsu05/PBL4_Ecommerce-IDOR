using Microsoft.EntityFrameworkCore;
using PBL3.Dbcontext;
using PBL3.Entity;
using PBL3.Enums;
using PBL3.Services;
using PBL3.Repositories;
using PBL3.Middleware; // added to enable UseAdminAuth extension
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();
// Đăng ký DbContext - sử dụng Pomelo MySQL provider vì connection string trỏ tới MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
 options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
 ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Đăng ký các Repository
builder.Services.AddScoped<IUserRepositories, UserRepositories>();
builder.Services.AddScoped<IPlatformWalletRepositories, PlatformWalletRepositories>();
builder.Services.AddScoped<IProductRepositories, ProductRepositories>();
builder.Services.AddScoped<IReviewRepositories, ReviewRepositories>();
builder.Services.AddScoped<ISellerRepositories, SellerRepositories>();
builder.Services.AddScoped<IBuyerRepositories, BuyerRepositories>();
builder.Services.AddScoped<IOrderRepositories, OrderRepositories>();
builder.Services.AddScoped<IOrderDetailRepositories, OrderDetailRepositories>();
builder.Services.AddScoped<ICartItemRepositories, CartItemRepositories>();
builder.Services.AddScoped<IAddressBuyerRepositories, AddressBuyerRepositories>();
builder.Services.AddScoped<IBankRepositories, BankRepositories>();
builder.Services.AddScoped<IVoucherRepositories, VoucherRepositories>();
builder.Services.AddScoped<IVoucher_BuyerRepositories, Voucher_BuyerRepositories>();
builder.Services.AddScoped<IReturnExchangeRepositories, ReturnExchangeRepositories>();

// Đăng ký các Service
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<BuyerService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<WalletService>();
builder.Services.AddScoped<ReturnExchangeService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
 app.UseExceptionHandler("/Home/Error");
 app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // ⚠️ Đảm bảo dòng này có để phục vụ hình ảnh, CSS, JS

app.UseRouting();

app.UseSession(); // ✅ Đặt sau UseRouting nhưng trước UseAuthorization
app.UseAdminAuth(); // ✅ Middleware kiểm tra session và redirect tới login nếu không phải admin
app.UseAuthorization();

// Map route
app.MapControllerRoute(
 name: "default",
 pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
