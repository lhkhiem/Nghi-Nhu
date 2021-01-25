using System.Web;
using System.Web.Optimization;

namespace KMHouse
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css_client").Include(
                "~/Asset/Client/assets/vendor/line-awesome/line-awesome/line-awesome/css/line-awesome.min.css",
                "~/Asset/Client/assets/css/bootstrap.min.css",
                "~/Asset/Client/assets/css/plugins/owl-carousel/owl.carousel.css",
                "~/Asset/Client/assets/css/plugins/magnific-popup/magnific-popup.css",
                "~/Asset/Client/assets/css/style.css",
                "~/Asset/Client/assets/css/skins/skin-demo-13.css",
                "~/Asset/Client/assets/css/demos/demo-13.css"

                 ));
            bundles.Add(new ScriptBundle("~/bundles/jquery_client").Include(
                "~/Asset/Client/assets/js/bootstrap.bundle.min.js",
                "~/Asset/Client/assets/js/jquery.waypoints.min.js",
                "~/Asset/Client/assets/js/owl.carousel.min.js",
                "~/Asset/Client/assets/js/jquery.magnific-popup.min.js",
                "~/Asset/Client/assets/js/main.js",
                "~/Asset/Client/assets/js/demos/demo-13.js",
                "~/Asset/Admin/Js/jquery.validate.min.js",
                "~/Asset/Admin/Js/bootbox.min.js",
                "~/Asset/Client/assets/js/numeral.min.js",
                "~/Asset/Client/assets/js/controller/CartController.js",
                "~/Asset/Client/assets/js/controller/AddToCartController.js",
                "~/Asset/Client/assets/js/controller/ClientLogin.js"
                ));

            BundleTable.EnableOptimizations = true;
        }
    }
}