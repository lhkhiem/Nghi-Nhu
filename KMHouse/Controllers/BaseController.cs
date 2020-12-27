using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KMHouse.Controllers
{
    public class BaseController : Controller
    {
        [ChildActionOnly]
        public PartialViewResult MainMenu()
        {
            ViewBag.CategoryList = new ProductCategoryDao().ListAll();
            ViewBag.NewsCategoryList = new NewsCategoryDao().ListAll();
            return PartialView();
        }
    }
}