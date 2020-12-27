using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class SliderDao
    {
        private DBContext db = null;

        public SliderDao()
        {
            db = new DBContext();
        }

        public IEnumerable<Slide> ListAll()
        {
            return db.Slides.OrderBy(x => x.DisplayOrder).ToList();
        }

        public IEnumerable<Slide> ListActive()
        {
            return db.Slides.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public int Insert(Slide entity)
        {
            entity.DisplayOrder = GetMaxDislayOrder() + 1;
            db.Slides.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public int Update(Slide entity)
        {
            var model = db.Slides.Find(entity.ID);
            model.Name = entity.Name;
            model.Caption1 = entity.Caption1;
            model.Caption2 = entity.Caption2;
            model.Caption3 = entity.Caption3;
            model.Caption4 = entity.Caption4;
            model.Caption5 = entity.Caption5;
            model.Caption6 = entity.Caption6;
            model.Image = entity.Image;
            model.Link = entity.Link;
            model.ModifiedBy = entity.ModifiedBy;
            model.ModifiedDate = entity.ModifiedDate;

            db.SaveChanges();
            return entity.ID;
        }

        public Slide GetByID(int id)
        {
            return db.Slides.Find(id);
        }

        public bool CheckIDExist(int id)
        {
            var model = db.Slides.Find(id);
            if (model == null)//không tồn tại
                return true;
            else return false;
        }

        public bool Delete(int id)
        {
            try
            {
                var menutype = db.Slides.Find(id);
                db.Slides.Remove(menutype);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangeStatus(int id)
        {
            var slide = db.Slides.Find(id);
            slide.Status = !slide.Status;
            db.SaveChanges();
            return slide.Status;
        }

        public bool ChangeOrder(int id, int order)
        {
            var slide = db.Slides.Find(id);
            var item = db.Slides.FirstOrDefault(x => x.DisplayOrder == order);
            if (slide.DisplayOrder != order)
            {
                if (item == null)
                {
                    slide.DisplayOrder = order;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    item.DisplayOrder = slide.DisplayOrder;
                    slide.DisplayOrder = order;
                    db.SaveChanges();
                    return true;
                }
            }
            else return false;
        }

        public int GetMaxDislayOrder()
        {
            var model = db.Slides.OrderByDescending(x => x.DisplayOrder).FirstOrDefault();
            if (model != null)
            {
                return model.DisplayOrder;
            }
            else return 0;
        }
    }
}