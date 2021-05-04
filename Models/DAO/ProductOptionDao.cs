using Models.EF;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DAO
{
    public class ProductOptionDao
    {
        private DBContext db = null;

        public ProductOptionDao()
        {
            db = new DBContext();
        }

        public IEnumerable<ProductOptionViewModel> ListByProduct(long productId)
        {
            var model = from a in db.ProductOptions
                        join b in db.Options
                        on a.OptionID equals b.ID
                        where a.ProductID.Equals(productId)
                        select new ProductOptionViewModel()
                        {
                            ProductID = a.ProductID,
                            OptionID = a.OptionID,
                            Price = a.Price,
                            OptionName = b.Name,
                        };
            return model;
        }

        public bool Insert(ProductOption entity)
        {
            db.ProductOptions.Add(entity);
            db.SaveChanges();
            return true;
        }

        public bool Delete(long productId, long optionId)
        {
            try
            {
                var item = db.ProductOptions.Where(x => x.ProductID.Equals(productId) && x.OptionID.Equals(optionId)).FirstOrDefault();
                db.ProductOptions.Remove(item);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}