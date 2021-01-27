using Common;
using Models.DAO;
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
            ViewBag.NewsCategoryList = new NewsCategoryDao().ListSortByParent();
            return PartialView(productCategoryList);
        }

        public PartialViewResult Menu2()
        {
            //Lấy tất cả category có product or có category con có product
            var dao = new ProductCategoryDao();
            var productCategoryList = dao.ListHasProduct();
            ViewBag.NewsCategoryList = new NewsCategoryDao().ListSortByParent();
            return PartialView(productCategoryList);
        }

        [ChildActionOnly]
        public PartialViewResult MobileMenu()
        {
            //Lấy tất cả category có product or có category con có product
            var dao = new ProductCategoryDao();
            var productCategoryList = dao.ListHasProduct();

            return PartialView(productCategoryList);
        }

        public PartialViewResult Cart()
        {
            return PartialView();
        }
    }
}