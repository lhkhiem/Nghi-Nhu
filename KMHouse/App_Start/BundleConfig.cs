using System.Web;
using System.Web.Optimization;

namespace KMHouse
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery_client").Include(
                

                ));

            bundles.Add(new StyleBundle("~/bundles/css_client").Include(
                
                ));
            BundleTable.EnableOptimizations = true;
        }
    }
}
