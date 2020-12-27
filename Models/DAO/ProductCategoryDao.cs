using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DAO
{
    public class ProductCategoryDao
    {
        private DBContext db = null;

        public ProductCategoryDao()
        {
            db = new DBContext();
        }

        public IEnumerable<ProductCategory> ListAll()
        {
            return db.ProductCategories.ToList();
        }

        public bool ParentHasProduct(long id)
        {
            var list = db.ProductCategories.Where(x => x.ParentID == null).ToList();
            if (db.Products.FirstOrDefault(x => x.ProductCategoryID == id) != null)
            {
                return true;
            }
            return false;
        }

        public bool ChildHasProduct(long parentId)
        {
            var list = db.ProductCategories.Where(x => x.ParentID != null && x.ParentID == parentId).ToList();
            byte count = 0;
            foreach (var cate in list)
            {
                if (db.Products.FirstOrDefault(x => x.ProductCategoryID == cate.ID) != null)
                {
                    count++;
                }
            }
            if (count > 0) return true;
            return false;
        }

        public bool HasProduct(long id)
        {
            var products = db.Products.FirstOrDefault(x => x.ProductCategoryID == id);

            if (products != null) return true;
            return false;
        }

        public long Insert(ProductCategory entity)
        {
            if (entity.ParentID == null) entity.DisplayOrder = GetMaxDislayOrder() + 1;
            else entity.DisplayOrder = 0;

            entity.CreateDate = DateTime.Now;
            db.ProductCategories.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public long Update(ProductCategory entity)
        {
            var model = db.ProductCategories.Find(entity.ID);
            model.Name = entity.Name;
            model.ParentID = entity.ParentID;
            model.ModifiedBy = entity.CreateBy;
            model.ModifiedDate = DateTime.Now;
            model.MetaTitle = entity.MetaTitle;
            db.SaveChanges();
            return entity.ID;
        }

        public ProductCategory GetByID(long id)
        {
            return db.ProductCategories.Find(id);
        }

        public bool CheckIDExist(long id)
        {
            var model = db.ProductCategories.Find(id);
            if (model == null)//không tồn tại
                return true;
            else return false;
        }

        public bool HasChild(long? id)
        {
            var model = db.ProductCategories.Find(id);

            if (model.ParentID != null)//không có menu con
                return true;
            else return false;
        }

        public bool GetChild(long id)
        {
            var model = db.ProductCategories.Where(x => x.ParentID == id);

            if (model.Count() > 0)
                return true;//có có menu con
            else return false;//không có menu con
        }

        public bool Delete(long id)
        {
            try
            {
                var productCategory = db.ProductCategories.Find(id);
                var listChild = db.ProductCategories.Where(x => x.ParentID == productCategory.ID);
                db.ProductCategories.RemoveRange(listChild);
                db.ProductCategories.Remove(productCategory);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckProductIsUsed(long id)
        {
            var model = db.Products.FirstOrDefault(x => x.ProductCategoryID == id);
            if (model == null)
                return false;//khong tim thay
            else return true;//tim thay
        }

        public int GetMaxDislayOrder()
        {
            var model = db.ProductCategories.Where(x => x.ParentID == null).OrderByDescending(x => x.DisplayOrder).FirstOrDefault();
            if (model != null)
            {
                return model.DisplayOrder; ;
            }
            else return 0;
        }

        public bool ChangeOrder(long id, int order)
        {
            var productCategory = db.ProductCategories.Find(id);
            var item = db.ProductCategories.FirstOrDefault(x => x.DisplayOrder == order);
            if (productCategory.DisplayOrder != order)
            {
                if (item == null)
                {
                    productCategory.DisplayOrder = order;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    item.DisplayOrder = productCategory.DisplayOrder;
                    productCategory.DisplayOrder = order;
                    db.SaveChanges();
                    return true;
                }
            }
            else return false;
        }
    }
}