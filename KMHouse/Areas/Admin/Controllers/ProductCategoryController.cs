using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Common;

namespace KMHouse.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        [HasCredential(RoleID = "PRODUCTCATEGORY_VIEW")]
        public ActionResult Index()
        {
            SetDropdownList();
            ViewBag.MenuActive = "mIndexProductCategory";
            return View();
        }

        public JsonResult LoadData(int categoryParent, string keyword, int pageIndex, int pageSize)
        {
            string str = NonUnicode.RemoveUnicode(keyword).ToLower();
            var model = new ProductCategoryDao().ListAll().Where(x => x.ParentID == categoryParent && NonUnicode.RemoveUnicode(x.Name.ToLower()).Contains(str));

            int totalRow = model.Count();
            //Sắp xếp theo danh mục cha
            //var list = new List<ProductCategory>();
            model = model.OrderBy(x => x.DisplayOrder)
            //foreach (var item in model)
            //{
            //    if (item.ParentID == 0)
            //    {
            //        list.Add(item);
            //        var child = model.Where(x => x.ParentID == item.ID);
            //        foreach (var subitem in child)
            //        {
            //            list.Add(subitem);
            //        }
            //    }
            //}
            ////Kết thúc sắp xếp.

            //model = list
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
            int totalRowCurent = model.Count();

            return Json(new
            {
                data = model,
                total = totalRow,
                totalCurent = totalRowCurent,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [HasCredential(RoleID = "PRODUCTCATEGORY_CREATE")]
        public JsonResult SaveData(string strProductCategory)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ProductCategory productCategory = serializer.Deserialize<ProductCategory>(strProductCategory);
            bool status = false;
            string action = string.Empty;
            string message = string.Empty;
            bool res = new ProductCategoryDao().CheckIDExist(productCategory.ID);
            if (res)
            {
                var model = new ProductCategoryDao();
                try
                {
                    //if (productCategory.ParentID == null)
                    //{
                    productCategory.CreateBy = ((UserLoginSession)Session[ConstantSession.USER_SESSION]).UserName;
                    model.Insert(productCategory);
                    status = true;
                    action = "insert";
                    //}
                    //else
                    //{
                    //    bool haveParent = new ProductCategoryDao().HasChild(productCategory.ParentID);
                    //    if (!haveParent)
                    //    {
                    //        productCategory.CreateBy = ((UserLoginSession)Session[ConstantSession.USER_SESSION]).UserName;
                    //        model.Insert(productCategory);
                    //        status = true;
                    //        action = "insert";
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    status = false;
                    message = ex.Message;
                }
            }
            else
            {
                //update existing DB
                //save db
                try
                {
                    var model = new ProductCategoryDao().Update(productCategory);
                    status = true;
                    action = "update";
                }
                catch (Exception ex)
                {
                    status = false;
                    message = ex.Message;
                }
            }

            return Json(new
            {
                status = status,
                message = message,
                action = action
            });
        }

        [HasCredential(RoleID = "PRODUCTCATEGORY_EDIT")]
        public JsonResult GetDetail(long id)
        {
            var dao = new ProductCategoryDao();
            var model = dao.GetByID(id);
            var hasChild = dao.HasChild(id);

            return Json(new
            {
                data = model,
                status = true,
                hasChild = hasChild
            }, JsonRequestBehavior.AllowGet);
        }

        [HasCredential(RoleID = "PRODUCTCATEGORY_DELETE")]
        public JsonResult Delete(long id)
        {
            bool status = false;
            string message = "";
            var isUsed = new ProductCategoryDao().CheckProductIsUsed(id);
            var child = new ProductCategoryDao().HasChild(id);
            if (!isUsed)
            {
                if (!child)
                {
                    var res = new ProductCategoryDao().Delete(id);

                    if (res)
                    {
                        status = true;
                    }
                    else
                    {
                        status = false;
                        message = "Có lỗi xảy ra!";
                    }
                }
                else
                {
                    status = false;
                    message = "Vui lòng xóa hết danh mục con trước khi xóa danh mục này!";
                }
            }
            else
            {
                status = false;
                message = "Có Product đang dùng. Vui lòng kiểm tra lại!";
            }
            return Json(new
            {
                status = status,
                message = message
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetParentList(long id)
        {
            var data = new ProductCategoryDao().ListAll().Where(x => x.ParentID == id).ToList();
            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        public void SetDropdownList(long? selectedId = null)
        {
            //var model = new ProductCategoryDao().ListAll();
            //List<ProductCategory> list = new List<ProductCategory>();
            //model = model.OrderBy(x => x.ID);
            //foreach (var item in model)
            //{
            //    if (item.ParentID == null)
            //    {
            //        list.Add(item);
            //        var child = model.Where(x => x.ParentID == item.ID);
            //        foreach (var subitem in child)
            //        {
            //            subitem.Name = "-- " + subitem.Name;
            //            list.Add(subitem);
            //        }
            //    }
            var dao = new ProductCategoryDao();
            List<SelectListItem> categoryList = new List<SelectListItem>();
            categoryList.Add(new SelectListItem()
            {
                Text = "---------",
                Value = "0",
            });
            //if (!dao.HasChild(selectedId))
            //{
            var model = dao.ListAll();
            var listParent = model.Where(x => x.ParentID == 0).OrderBy(x => x.DisplayOrder).ToList();
            var listChild = model.Where(x => x.ParentID != 0).ToList();
            foreach (var item in listParent)
            {
                categoryList.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.ID.ToString(),
                    Selected = selectedId != null && item.ID == selectedId ? true : false
                });
                if (dao.HasChild(item.ID))
                {
                    var child = listChild.Where(x => x.ParentID == item.ID).ToList();
                    foreach (var subitem in child)
                    {
                        subitem.Name = "-- " + subitem.Name;
                        categoryList.Add(new SelectListItem()
                        {
                            Text = subitem.Name,
                            Value = subitem.ID.ToString(),
                            Selected = selectedId != null && subitem.ID == selectedId ? true : false
                        });
                    }
                }
            }
            //}
            ViewBag.ID = categoryList;
            //Kết thúc sắp xếp.
            //ViewBag.ID = private new SelectList(list, "ID", "Name", selectedId);
        }

        public JsonResult ConvertString(string str)
        {
            string strConvert = StringHelper.ToUnsignString(str);
            return Json(new
            {
                str = strConvert
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeOrder(int id, int order)
        {
            var res = new ProductCategoryDao().ChangeOrder(id, order);
            if (res)
            {
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}