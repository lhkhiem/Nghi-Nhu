using System;
using System.Web.Mvc;
using Models.DAO;
using Models.ViewModels;
using Common;
using Models.EF;
using System.Web.Script.Serialization;

namespace KMHouse.Controllers
{
    public class UserClientController : BaseClientController
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBiulder = new UriBuilder(Request.Url);
                uriBiulder.Query = null;
                uriBiulder.Fragment = null;
                uriBiulder.Path = Url.Action("FacebookCallback");
                return uriBiulder.Uri;
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string model)
        {
            int statusCode = 0;
            string message = string.Empty;
            string account = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            User userStr = serializer.Deserialize<User>(model);
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var res = dao.LoginByEmail(userStr.Email, Encryptor.MD5Hash(Encryptor.EncodeTo64(userStr.Password)));
                if (res == 1)
                {
                    var user = dao.GetDetailByEmail(userStr.Email);
                    var userSession = new UserLoginSession();
                    userSession.UserName = user.UserName;
                    userSession.Email = user.Email;
                    userSession.UserID = user.ID;
                    userSession.UserGroupID = user.UserGroupID;
                    userSession.Name = user.Name;
                    userSession.Address = user.Address;
                    userSession.Phone = user.Phone;
                    Session.Add(ConstantSession.USER_CLIENT_SESSION, userSession);

                    statusCode = res;
                    account = user.Name;
                }
                else if (res == 0)
                {
                    statusCode = res;
                    message = "Tài khoản không tồn tại!";
                }
                else if (res == -1)
                {
                    statusCode = res;
                    message = "Tài khoản tạm khóa!";
                }
                else if (res == -2)
                {
                    statusCode = res;
                    message = "Mật khẩu không đúng!";
                }
            }
            else
            {
                statusCode = -3;
                message = "Không có quyền truy cập!";
            }
            return Json(new
            {
                status = statusCode,
                msg = message,
                account = account
            });
        }

        [HttpPost]
        public JsonResult Register(string model)
        {
            int statusCode = 0;
            string message = string.Empty;
            string account = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            User userStr = serializer.Deserialize<User>(model);
            var dao = new UserDao();
            if (dao.CheckEmailExist(userStr.Email))
            {
                statusCode = 0;
                message = "Tài khoản đã tồn tại!";
            }
            else
            {
                var user = new UserViewModel();
                user.UserName = userStr.Email;
                user.Name = userStr.Name;
                user.Password = Encryptor.MD5Hash(Encryptor.EncodeTo64(userStr.Password));
                user.Phone = userStr.Phone;
                user.Email = userStr.Email;
                user.Address = userStr.Address;
                user.CreateDate = DateTime.Now;
                user.UserGroupID = "GUEST";
                user.CreateBy = "Myself";
                user.Status = true;
                var result = dao.Insert(user);
                if (result > 0)
                {
                    statusCode = 1;
                    message = "Đăng ký thành công!";
                }
                else
                {
                    statusCode = -1;
                    message = "Đăng ký thành công!";
                }
            }
            return Json(new
            {
                status = statusCode,
                msg = message
            });
        }

        public ActionResult Logout()
        {
            Session[ConstantSession.USER_CLIENT_SESSION] = null;
            return Redirect("/");
        }

        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ForgetPassword(string email)
        {
            string message = string.Empty;
            int statusCode = 0;
            var model = new UserDao().CheckEmailExist(email);
            if (model)
            {
                string password = RadomString.GetUniqueKey(6);
                var user = new UserDao().GetDetailByEmail(email);
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Asset/Client/template/newpassword.html"));
                content = content.Replace("{{CustomerName}}", user.Name.ToString());
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Password}}", password);

                new MailHelper().SendMailRecoveryPassword(email, "Khôi phục mật khẩu", content);//gui mail khach hang
                //cập nhật mật khẩu
                var res = new UserDao().UpdatePassword(user.ID, Encryptor.MD5Hash(Encryptor.EncodeTo64(password)));
                if (res)
                {
                    statusCode = 1;
                    message = "Gửi thành công! Vui lòng kiểm tra email của bạn, sau đó đổi lại mật khẩu";
                }
            }
            else
            {
                statusCode = 0;
                message = "Email không tồn tại!";
            }
            return Json(new
            {
                status = statusCode,
                msg = message
            });
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ckemail = new UserDao().CheckEmailExist(model.Email);
                if (ckemail)
                {
                    var user = new UserDao().GetDetailByEmail(model.Email);
                    if (user.Password.Equals(Encryptor.MD5Hash(Encryptor.EncodeTo64(model.Password))))
                    {
                        var res = new UserDao().UpdatePassword(user.ID, Encryptor.MD5Hash(Encryptor.EncodeTo64(model.NewPassword)));
                        if (res)
                        {
                            ViewBag.Error = "<span class='text text-success'>Thông báo: Đổi mật khẩu thành công!</span>";
                        }
                    }
                    else
                    {
                        ViewBag.Error = "<span class='text text-danger'>Lỗi: Mật khẩu cũ không đúng.</span>";
                    }
                }
                else
                {
                    ViewBag.Error = "<span class='text text-danger'>Lỗi: Email không đúng.</span>";
                }
            }
            return View();
        }
    }
}