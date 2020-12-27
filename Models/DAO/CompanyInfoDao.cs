using Models.EF;
using System.Linq;
using System.Data;

namespace Models.DAO
{
    public class CompanyInfoDao
    {
        private DBContext db = null;

        public CompanyInfoDao()
        {
            db = new DBContext();
        }

        public CompanyInfo GetInfo()
        {
            return db.CompanyInfoes.FirstOrDefault();
        }
    }
}