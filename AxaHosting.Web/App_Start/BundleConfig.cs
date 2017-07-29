using System.Web.Optimization;

namespace AxaHosting.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
           
            bundles.Add(new StyleBundle("~/bootstrap/css").Include("~/css/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/respond.min.js", "~/js/site.js"));
            BundleTable.EnableOptimizations = true;
        }
    }
}