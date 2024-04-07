using Microsoft.AspNetCore.Mvc;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;
using System;
using Aram.Infrastructure;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using System.Text.RegularExpressions;
using System.Text;
using BCrypt.Net;
using System.Security.Policy;

namespace ThoBayMau_ASM.Controllers
{

    public class DangNhapDangKiController : Controller
    {
        private readonly ThoBayMau_ASMContext _db;

        public DangNhapDangKiController(ThoBayMau_ASMContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            if (returnUrl != null)
            {
                ViewBag.returnUrl = returnUrl;
            }
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public IActionResult Login(TaiKhoan tk, string? returnUrl)
        {
            var user = _db.TaiKhoan.FirstOrDefault(x => x.TenTK == tk.TenTK);
            
            if (user != null)
            {
                
                

                if (BCrypt.Net.BCrypt.Verify(tk.MatKhau, user.MatKhau))
                {
                    HttpContext.Session.SetString("UserName", user.TenTK.ToString());
                    HttpContext.Session.SetJson("User", user);
                    if (user.TrangThai == false)
                    {
                        TempData["warning"] = "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ admin website để được hỗ trợ";
                        return View(tk);
                    }
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    if (user.LoaiTK)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.ErrPassword = "Tên tài khoản hoặc mật khẩu không chính xác";
                    return View(tk);
                }
            }
            ViewBag.ErrPassword = "Tên tài khoản hoặc mật khẩu không chính xác";

            return View(tk);
        }
        //chuyển tên tài khoản google thành không dấu cho phù hợp với varchar
        static string RemoveDiacritics(string input)
        {
            string regex = @"\p{IsCombiningDiacriticalMarks}+";

            string normalizedString = input.Normalize(NormalizationForm.FormD);
            string withoutDiacritics = Regex.Replace(normalizedString, regex, string.Empty).Normalize(NormalizationForm.FormC);

            withoutDiacritics = withoutDiacritics.Replace("đ", "d").Replace("Đ", "d");

            withoutDiacritics = Regex.Replace(withoutDiacritics, @"\s+", "");

            return withoutDiacritics;
        }
        //tạo mật khẩu tự động tăng bảo mật
        static string RandomPassword(int quantity)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;:,.<>?";
            StringBuilder password = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < quantity; i++)
            {
                int index = random.Next(chars.Length);
                password.Append(chars[index]);
            }

            return password.ToString();
        }

        public async Task LoginByGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(x => new
            {
                x.Issuer,
                x.OriginalIssuer,
                x.Type,
                x.Value
            });
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email);
            var email = emailClaim?.Value;
            var name = User.Identity.Name;
            var timEmail = _db.TaiKhoan.FirstOrDefault(x => x.Email == email);

            if (timEmail == null)
            {

                CreateByGoogle(email, name);

                if (DangNhapByGoogle(email))
                {
                    return RedirectToAction("Index", "Home");
                }
                return NotFound();
            }
            else
            {
                if (DangNhapByGoogle(email))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["warning"] = "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ admin website để được hỗ trợ";
                    return RedirectToAction("Login", "DangNhapDangKi");
                }

            }
        }
        public async Task LogoutByGoogle()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(GoogleDefaults.AuthenticationScheme);
        }
        public void CreateByGoogle(string email, string name)
        {
            TaiKhoan tk = new TaiKhoan();
            tk.Email = email;
            tk.SDT = "00000000000";
            tk.TenTK = RemoveDiacritics(name);
            tk.MatKhau = BCrypt.Net.BCrypt.HashPassword(RandomPassword(15));
            tk.NgayDangKy = DateTime.Now;
            tk.TrangThai = true;
            _db.TaiKhoan.Add(tk);
            _db.SaveChanges();
        }
        public bool DangNhapByGoogle(string email)
        {
            var user = _db.TaiKhoan.FirstOrDefault(x => x.Email == email);
            if (user != null && user.TrangThai == true)
            {
                HttpContext.Session.SetString("UserName", user.TenTK.ToString());
                HttpContext.Session.SetJson("User", user);
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("User");
            return RedirectToAction("Login", "DangNhapDangKi");
        }

        //Dăng ký


        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(TaiKhoan tk)
        {
            if (ModelState.IsValid)
            {

                var existingAccount = _db.TaiKhoan.FirstOrDefault(t => t.TenTK == tk.TenTK);
                var existingEmail = _db.TaiKhoan.FirstOrDefault(t => t.Email == tk.Email);
                var existingPhone = _db.TaiKhoan.FirstOrDefault(t => t.SDT == tk.SDT);


                if (existingAccount != null)
                {
                    TempData["errorMessage"] = "Tài khoản đã tồn tại!";
                    return View();
                }
                else if (existingEmail != null)
                {
                    TempData["errorMessage"] = "Email đã được sử dụng!";
                    return View();
                }
                else if (existingPhone != null)
                {
                    TempData["errorMessage"] = "Số điện thoại đã được sử dụng!";
                    return View();
                }
                else
                {
                    var db = new TaiKhoan()
                    {
                        TenTK = tk.TenTK,
                        Email = tk.Email,
                        MatKhau = BCrypt.Net.BCrypt.HashPassword(tk.MatKhau),
                        SDT = tk.SDT,
                        DiaChi = tk.DiaChi,
                        NgayDangKy = DateTime.Now,
                        TrangThai = true,
                    };
                    _db.TaiKhoan.Add(db);
                    _db.SaveChanges();
                    TempData["Sucess"] = "Đăng kí thành công!";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                TempData["Error"] = "Đăng ký thất bại!";
                return View();
            }
        }
        //QMK
        public ActionResult GuiMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GuiMail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                if (Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    var user = _db.TaiKhoan.FirstOrDefault(s => s.Email == email);

                    if (user != null)
                    {
                        string maXacNhan;
                        int time = 1;
                        Random rnd = new Random();
                        maXacNhan = rnd.Next().ToString();
                        DateTime expireTime = DateTime.Now.AddMinutes(time);

                        HttpContext.Session.SetString("MaXacNhan", maXacNhan);
                        HttpContext.Session.SetString("ResetEmail", email);
                        HttpContext.Session.SetString("ExpireTime", expireTime.ToString("yyyy-MM-dd HH:mm:ss"));

                        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                        smtp.EnableSsl = true;
                        smtp.Port = 587;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Credentials = new NetworkCredential("petshop20002003@gmail.com", "jddd xqev vqyl mgbg");

                        MailMessage mail = new MailMessage();
                        mail.To.Add(email);
                        mail.From = new MailAddress("petshop20002003@gmail.com");
                        mail.Subject = "Thông Báo Quan Trọng Từ Pet_Shop";

                        mail.Body = "Kính gửi,<br>" +
                                    "Chúng tôi xác nhận bạn đã sử dụng quên mật khẩu của chúng tôi<br>" +
                                    "<strong><h2>Đây là mã xác nhận của bạn: " + maXacNhan + "</h2></strong><br>" +
                                    "Mã này sẽ hết hạn sau " + time + " phút <br>" +
                                    "Xin vui lòng không cung cấp cho người khác<br>" +
                                    "Trân trọng.<br>" +
                                    "Hỗ trợ Khách Hàng Pet_Shop" + "<br><br>";

                        mail.IsBodyHtml = true;
                        await smtp.SendMailAsync(mail);
                        TempData["Sucess"] = "Một mã xác nhận đã được gửi đến email của bạn. Vui lòng kiểm tra email và nhập mã xác nhận để đặt lại mật khẩu.";
                        return RedirectToAction("QuenMatKhau");
                    }
                    ViewBag.Email = email;
                    ViewBag.errEmail = "Email chưa được đăng ký trong hệ thống";
                    return View("GuiMail", email);
                }
                ViewBag.errEmail = "Email không hợp lệ";
                return View("GuiMail", email);
            }
            ViewBag.Email = email;
            ViewBag.errEmail = "Email không được bỏ trống";
            return View("GuiMail", email);
        }

        public ActionResult QuenMatKhau()
        {
            return View();
        }
        [HttpPost]
        public IActionResult QuenMatKhau(string maXacNhan, string newPassword)
        {
            var sessionMaXacNhan = HttpContext.Session.GetString("MaXacNhan");
            var resetEmail = HttpContext.Session.GetString("ResetEmail");
            var sessionExpireTime = HttpContext.Session.GetString("ExpireTime");

            DateTime expireTime;
            if (DateTime.TryParse(sessionExpireTime, out expireTime) && DateTime.Now <= expireTime)
            {
                // So sánh mã xác nhận từ session với mã xác nhận từ người dùng nhập vào
                if (maXacNhan == sessionMaXacNhan)
                {
                    var user = _db.TaiKhoan.FirstOrDefault(s => s.Email == resetEmail);

                    if (user != null)
                    {
                        // Đặt lại mật khẩu cho người dùng
                        user.MatKhau = newPassword;
                        _db.Update(user);
                        _db.SaveChanges();

                        TempData["Sucess"] = $"Reset mật khẩu thành công";
                        return View("Login");
                    }
                }
            }
            ViewBag.errCode = "Mã xác nhận không chính xác";
            return View();
        }
    }
}


