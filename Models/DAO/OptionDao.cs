using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class OptionDao
    {
        private DBContext db = null;

        public OptionDao()
        {
            db = new DBContext();
        }

        public IEnumerable<Option> ListAll()
        {
            return db.Options.ToList();
        }

        public long Insert(Option entity)
        {
            db.Options.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public long Update(Option entity)
        {
            var model = db.Options.Find(entity.ID);
            model.Name = entity.Name;
            db.SaveChanges();
            return entity.ID;
        }

        public Option GetByID(int id)
        {
            return db.Options.Find(id);
        }

        public bool CheckIsUsed(int id)
        {
            var ck = db.ProductOptions.Where(x => x.OptionID == id);
            if (ck.Count() > 0) return true;
            else return false;
        }

        public bool Delete(int id)
        {
            try
            {
                var unit = db.Options.Find(id);
                db.Options.Remove(unit);
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