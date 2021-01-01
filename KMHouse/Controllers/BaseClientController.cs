using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KMHouse.Controllers
{
    public class BaseClientController : Controller
    {
        public BaseClientController()
        {
            ViewBag.Info = new CompanyInfoDao().GetInfo();
        }

        [ChildActionOnly]
        public PartialViewResult Menu()
        {
            //Lấy tất cả category có product or có category con có product
            var dao = new ProductCategoryDao();
            var productCategoryList = dao.ListHasProduct();

            return PartialView(productCategoryList);
        }

        public PartialViewResult Menu2()
        {
            //Lấy tất cả category có product or có category con có product
            var dao = new ProductCategoryDao();
            var productCategoryList = dao.ListHasProduct();

            return PartialView(productCategoryList);
        }
    }
}