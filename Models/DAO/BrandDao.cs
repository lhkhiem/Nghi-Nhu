using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DAO
{
    public class BrandDao
    {
        private DBContext db = null;

        public BrandDao()
        {
            db = new DBContext();
        }

        public IEnumerable<Brand> ListAll()
        {
            return db.Brands.ToList();
        }

        public byte Insert(Brand entity)
        {
            db.Brands.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public byte Update(Brand entity)
        {
            var model = db.Brands.Find(entity.ID);
            model.Name = entity.Name;
            model.Logo = entity.Logo;
            db.SaveChanges();
            return entity.ID;
        }

        public Brand GetByID(int id)
        {
            return db.Brands.Find(id);
        }

        public bool CheckIsUsed(int id)
        {
            var ck = db.Products.Where(x => x.BrandID == id);
            if (ck.Count() > 0) return true;
            else return false;
        }

        public bool Delete(int id)
        {
            try
            {
                var unit = db.Brands.Find(id);
                db.Brands.Remove(unit);
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