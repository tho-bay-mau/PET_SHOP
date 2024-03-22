using Microsoft.AspNetCore.Mvc;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;
using System;
using Aram.Infrastructure;

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
                HttpContext.Session.SetJson("User", tk);
                if(returnUrl != null)
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

        public IActionResult Logout()
        {

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

    }
}
