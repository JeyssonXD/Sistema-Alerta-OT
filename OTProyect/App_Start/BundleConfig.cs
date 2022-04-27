using System.Web;
using System.Web.Optimization;

namespace OTProyect
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //css
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                        "~/assets/vendors/iconfonts/mdi/css/materialdesignicons.min.css",
                        "~/assets/vendors/iconfonts/ionicons/dist/css/ionicons.css",
                        "~/assets/vendors/iconfonts/flag-icon-css/css/flag-icon.min.css",
                        "~/assets/vendors/css/vendor.bundle.base.css",
                        "~/assets/vendors/css/vendor.bundle.addons.css",
                        "~/assets/css/shared/style.css",
                        "~/assets/css/demo_1/style.css",
                        "~/assets/images/favicon.ico",
                        "~/assets/vendors/chosen/css/chosen.min.css",
                        "~/Content/PagedList.css"));
            
            //plugin js
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                      "~/assets/vendors/js/vendor.bundle.base.js",
                      "~/assets/vendors/js/vendor.bundle.addons.js",
                      "~/assets/js/shared/off-canvas.js",
                      "~/assets/js/shared/misc.js",
                      "~/assets/js/demo_1/dashboard.js",
                      "~/assets/js/shared/chart.js",
                      "~/assets/vendors/chosen/js/chosen.jquery.min.js"));
        }
    }
}
