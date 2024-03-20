using Microsoft.AspNetCore.Mvc;
using ThoBayMau_ASM.Data;
using ThoBayMau_ASM.Models;
using System;

namespace ThoBayMau_ASM.Controllers
{

    public class DangNhapDangKiController : Controller
    {
        private readonly ThoBayMau_ASMContext _db;

        public DangNhapDangKiController(ThoBayMau_ASMContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
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
        public IActionResult Login(TaiKhoan tk)
        {
            var user = _db.TaiKhoan.FirstOrDefault(x => x.TenTK == tk.TenTK && x.MatKhau == tk.MatKhau);

            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.TenTK.ToString());


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
                TempData["successMessage"] = "Bạn đã đăng ký thành công!";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["errorMessage"] = "Không thể đăng ký, vui lòng kiểm tra lại thông tin!";
                return View();
            }
        }
    }
}
