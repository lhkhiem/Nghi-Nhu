using Models.DAO;
using Models.EF;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KMHouse.Controllers
{
    public class NewsClientController : BaseClientController
    {
        public ActionResult Index(int pageIndex = 1, int pageSize = 3)
        {
            var model = new NewsDao().ListAll();
            ViewBag.ListNewsCategory = new NewsCategoryDao().ListAll();
            ViewBag.ListRecent = new NewsDao().ListRecentNews(5);
            int totalRecord = model.Count();
            //Lấy 1 khoảng trong list sản phẩm từ pageIndex đến pageSize để phân trang
            model = model.Skip((pageIndex - 1) * pageSize)
              .Take(pageSize);
            //Đếm lấy số record thực tế ở đoạn record hiện tại
            int totalRowCurent = model.Count();
            //Các biến để phân trang
            ViewBag.totalRecord = totalRecord;
            ViewBag.pageIndex = pageIndex;
            int recordStartPosition = ((pageIndex - 1) * pageSize) + 1;
            int recordEndPosition = ((recordStartPosition - 1 + pageSize) < totalRecord) ? (recordStartPosition - 1 + pageSize) : recordStartPosition - 1 + totalRowCurent;
            ViewBag.recordStartPosition = recordStartPosition;
            ViewBag.recordEndPosition = recordEndPosition;
            int maxPage = 5;
            int totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            if (totalRecord % pageSize > 0 && totalRecord > pageSize) totalPage += 1;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = pageIndex + 1;
            ViewBag.Prev = pageIndex - 1;
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            return View(model);
        }

        public ActionResult NewsDetail(long id)
        {
            var news = new NewsDao().GetByIDView(id);
            ViewBag.ListNews = new NewsDao().ListAll();
            ViewBag.NewsCategory = new NewsCategoryDao().GetByID(news.NewsCategoryID);
            ViewBag.ListNewsCategory = new NewsCategoryDao().ListAll();
            ViewBag.ListRelate = new NewsDao().ListRelateNews(id);
            ViewBag.ListRecent = new NewsDao().ListRecentNews(5);
            ViewBag.ListTag = new NewsDao().ListTag(id);
            return View(news);
        }

        public ActionResult NewsTag(string tagId)
        {
            var news = new NewsDao().ListAllByTag(tagId);
            ViewBag.ListTag = new NewsDao().GetTag(tagId);
            return View(news);
        }

        public ActionResult NewsOfCategory(long cateId)
        {
            var newsCategory = new NewsCategoryDao();
            var news = new NewsDao();
            List<NewsViewModel> list = new List<NewsViewModel>();
            if (newsCategory.HasChild(cateId))
            {
                var listCategoryChild = newsCategory.GetChild(cateId);
                foreach (var item in listCategoryChild)
                {
                    list.AddRange(news.ListAllByCategory(item.ID));
                }
            }
            else
            {
                list = news.ListAllByCategory(cateId);
            }
            ViewBag.ListNews = new NewsDao().ListAll();
            ViewBag.ListRecent = new NewsDao().ListRecentNews(5);
            ViewBag.ListNewsCategory = new NewsCategoryDao().ListAll();
            ViewBag.NewsCategory = new NewsCategoryDao().GetByID(cateId);
            return View(list);
        }
    }
}