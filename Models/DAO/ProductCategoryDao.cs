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

        public long Insert(ProductCategory entity)
        {
            if (entity.ParentID == 0) entity.DisplayOrder = GetMaxDislayOrder() + 1;
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
            if (entity.ParentID != 0)
                model.DisplayOrder = 0;
            else
                model.DisplayOrder = GetMaxDislayOrder() + 1;
            db.SaveChanges();
            return entity.ID;
        }

        public ProductCategory GetByID(long id)
        {
            return db.ProductCategories.Find(id);
        }

        public string GetParentName(long id)
        {
            var parentID = this.GetByID(id).ParentID;
            return db.ProductCategories.FirstOrDefault(x => x.ID == parentID).Name;
        }

        public long GetParentID(long id)
        {
            var parentID = this.GetByID(id).ParentID;
            var res = db.ProductCategories.FirstOrDefault(x => x.ID == parentID);
            if (res == null)
                return -1;
            return res.ID;
        }

        public bool CheckIDExist(long id)
        {
            var model = db.ProductCategories.Find(id);
            if (model == null)//không tồn tại
                return true;
            else return false;
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
            var model = db.ProductCategories.Where(x => x.ParentID == 0).OrderByDescending(x => x.DisplayOrder).FirstOrDefault();
            if (model != null)
            {
                return model.DisplayOrder;
            }
            else return 0;
        }

        public bool ChangeOrder(long id, int order)
        {
            try
            {
                var parentId = GetParentID(id);
                var model = db.ProductCategories.Find(id);
                if (parentId == -1)//không có cha= nó chính là gốc
                {
                    var item = db.ProductCategories.FirstOrDefault(x => x.DisplayOrder == order && x.ParentID == 0);
                    if (item != null)
                    {
                        item.DisplayOrder = model.DisplayOrder;
                        model.DisplayOrder = order;
                    }
                    else model.DisplayOrder = order;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    var item = db.ProductCategories.FirstOrDefault(x => x.DisplayOrder == order && x.ParentID == parentId);
                    if (item != null)
                    {
                        item.DisplayOrder = model.DisplayOrder;
                        model.DisplayOrder = order;
                    }
                    else model.DisplayOrder = order;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            //var parentId = GetParentID(id);
            //var productCategory = db.ProductCategories.Find(id);
            //var item = db.ProductCategories.FirstOrDefault(x => x.DisplayOrder == order);
            //if (parentId != -1)
            //{
            //    item = db.ProductCategories.FirstOrDefault(x => x.DisplayOrder == order && x.ParentID == parentId);
            //}
            //else
            //{
            //    item = db.ProductCategories.FirstOrDefault(x => x.DisplayOrder == order);
            //}

            //if (productCategory.DisplayOrder != order)
            //{
            //    if (item == null)
            //    {
            //        productCategory.DisplayOrder = order;
            //        db.SaveChanges();
            //        return true;
            //    }
            //    else
            //    {
            //        item.DisplayOrder = productCategory.DisplayOrder;
            //        productCategory.DisplayOrder = order;
            //        db.SaveChanges();
            //        return true;
            //    }
            //}
            //else return false;
        }

        public bool HasChild(long? id)
        {
            if (db.ProductCategories.FirstOrDefault(x => x.ParentID == id) != null)
                return true;
            else
                return false;
        }

        public List<ProductCategory> GetChild(long id)
        {
            return db.ProductCategories.Where(x => x.ParentID == id).ToList();
        }

        public bool ParentHasProduct(long id)
        {
            var list = db.ProductCategories.Where(x => x.ParentID == 0).ToList();
            if (db.Products.FirstOrDefault(x => x.ProductCategoryID == id) != null)
            {
                return true;
            }
            return false;
        }

        public bool ChildHasProduct(long parentId)
        {
            var list = db.ProductCategories.Where(x => x.ParentID != 0 && x.ParentID == parentId).ToList();
            byte count = 0;
            foreach (var cate in list)
            {
                if (db.Products.FirstOrDefault(x => x.ProductCategoryID == cate.ID && x.Status == true) != null)
                {
                    count++;
                }
            }
            if (count > 0) return true;
            return false;
        }

        public bool HasProduct(long id)
        {
            var products = db.Products.FirstOrDefault(x => x.ProductCategoryID == id && x.Status == true);

            if (products != null) return true;
            return false;
        }

        //public IEnumerable<ProductCategory> ListHasProduct()
        //{
        //    return db.ProductCategories.ToList();
        //}

        public List<ProductCategory> ListHasProduct()
        {
            List<ProductCategory> list = new List<ProductCategory>();
            List<ProductCategory> listOrder = new List<ProductCategory>();
            var listParent = db.ProductCategories.Where(x => x.ParentID == 0);
            var listChild = db.ProductCategories.Where(x => x.ParentID != 0);

            bool f1 = false;
            bool f2 = false;
            bool f3 = false;
            foreach (var item in listParent)
            {
                if (HasChild(item.ID))
                {
                    foreach (var childItem in listChild.Where(x => x.ParentID == item.ID))
                    {
                        if (HasChild(childItem.ID))
                        {
                            foreach (var childLevel3 in listChild.Where(x => x.ParentID == childItem.ID))
                            {
                                if (HasProduct(childLevel3.ID))//neu level 3 co san pham
                                {
                                    list.Add(childLevel3);//add level 3 vao list
                                    f3 = true;//bao hieu danh muc co san pham
                                }
                            }
                        }
                        //ket thuc lap level 3
                        if (f3 == true)//neu level 3 duoc add
                        {
                            list.Add(childItem);//add level 2
                            f2 = true;//bao hieu level 2 duoc add
                        }
                        else//nguoc lai level 3 khong dc add
                        {
                            if (HasProduct(childItem.ID))//kem tra level 2 co san pham khong?
                            {
                                list.Add(childItem);//add level 2
                                f2 = true;//neu co bao hieu level2 dc add
                            }
                        }
                    }
                }
                //ket thuc lap level 2
                if (f2 == true)//neu level 2 duoc add
                {
                    list.Add(item);//add level 1
                    f1 = true;//bao hieu level 1 duoc add
                }
                else//nguoc lai level 2 khong dc add
                {
                    if (HasProduct(item.ID))//kem tra level 1 co san pham khong?
                    {
                        list.Add(item);//add level 1
                        f1 = true;//neu co bao hieu level 1 dc add
                    }
                }
                f1 = false;
                f2 = false;
                f3 = false;
            }

            var list1 = list.Where(x => x.ParentID == 0).OrderBy(x => x.DisplayOrder).ToList();
            foreach (var itemList1 in list1)
            {
                listOrder.Add(itemList1);
                listOrder.AddRange(list.Where(x => x.ParentID == itemList1.ID));
            }
            //foreach (var item in db.ProductCategories)
            //{
            //    byte count = 0;
            //    if (HasChild(item.ID))
            //    {
            //        if (ChildHasProduct(item.ID)) list.Add(item);
            //        foreach (var itemChild in GetChild(item.ID))
            //        {
            //            if (HasProduct(itemChild.ID))
            //            {
            //                list.Add(itemChild);
            //                count++;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (HasProduct(item.ID) && item.ParentID == 0) list.Add(item);
            //    }
            //}
            return listOrder;
        }
    }
}