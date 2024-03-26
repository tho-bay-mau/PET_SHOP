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
            if(returnUrl != null)
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
            var user = _db.TaiKhoan.FirstOrDefault(x => x.TenTK == tk.TenTK && x.MatKhau == tk.MatKhau);

            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.TenTK.ToString());
                HttpContext.Session.SetJson("User", user);
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
            return View();
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
                DangNhapByGoogle(email);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                DangNhapByGoogle(email);
                return RedirectToAction("Index", "Home");
            }
            return Json(name + "_" + email);
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
            tk.SDT = "999999999";
            tk.DiaChi = "đ";
            tk.TenTK = name;
            tk.MatKhau = name + "123";
            _db.TaiKhoan.Add(tk);
            _db.SaveChanges();
        }
        public void DangNhapByGoogle(string email)
        {
            var user = _db.TaiKhoan.FirstOrDefault(x => x.Email == email);
            HttpContext.Session.SetString("UserName", user.TenTK.ToString());
            HttpContext.Session.SetJson("User", user);
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
                        MatKhau = tk.MatKhau,
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
            var user = _db.TaiKhoan.FirstOrDefault(s => s.Email == email);

            if (user != null)
            {
                string maXacNhan;
                Random rnd = new Random();
                maXacNhan = rnd.Next().ToString();

              
                HttpContext.Session.SetString("MaXacNhan", maXacNhan);
                HttpContext.Session.SetString("ResetEmail", email);

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential("petshop20002003@gmail.com", "jddd xqev vqyl mgbg");

                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("petshop20002003@gmail.com");
                mail.Subject = "Thông Báo Quan Trọng Từ PEt_shop";

                mail.Body = "Kính gửi,<br>" +
                            "Chúng tôi xác nhận bạn đã sử dụng quên mật khẩu của chúng tôi<br>" +
                            "<strong><h2>Đây là mã xác nhận của bạn: " + maXacNhan + "</h2></strong><br>" +
                            "Xin vui lòng không cung cấp cho người khác<br>" +
                            "Trân trọng.<br>" +
                            "Hỗ trợ Khách Hàng Pet-Shop" + "<br><br>";

                mail.IsBodyHtml = true;
                await smtp.SendMailAsync(mail);

                return RedirectToAction("QuenMatKhau");
            }
            return Json(new { success = false, responseText = "Email không tồn tại trong hệ thống!" });
        }

        public ActionResult QuenMatKhau()
        {
            return View();
        }
        [HttpPost]
        public IActionResult QuenMatKhau(string maXacNhan, string email, string newPassword)
        {
            var sessionMaXacNhan = HttpContext.Session.GetString("MaXacNhan");
            var resetEmail = HttpContext.Session.GetString("ResetEmail");

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

                    ViewData["Result"] = "success";
                    return View();
                }
            }
            // Trường hợp mã xác nhận không khớp hoặc email không tồn tại
            ViewData["Result"] = "error";
            return View();
        }


        [HttpGet]
        public JsonResult CheckEmail(string email)
        {
            bool exists = EmailExistsInNhanVien(email) ;
            return Json(new { exists = exists });
        }

        private bool EmailExistsInNhanVien(string email)
        {
            return _db.TaiKhoan.Any(nv => nv.Email == email);
        }

       
    }
}


