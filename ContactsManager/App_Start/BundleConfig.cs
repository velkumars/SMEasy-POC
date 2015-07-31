using System.Web.Optimization;

namespace SMEasy
{
    public static class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            if (bundles == null) return;
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            

           

            bundles.Add(new ScriptBundle("~/bundles/jqueryBundle").Include(
                "~/Scripts/jquery/jquery-1.11.3.min.js"
                        //"~/Scripts/jquery/jquery-1.9.1.min.js"

              ));

            bundles.Add(new ScriptBundle("~/bundles/jqureyControls").Include(
                "~/Scripts/jquery/jquery-migrate.min.js",
              "~/Scripts/jquery/jquery-ui-1.10.3.custom.min.js",
              "~/Scripts/jquery/jquery.slimscroll.js",
              "~/Scripts/jquery/jquery.slimscroll.min.js",
              "~/Scripts/jquery/jquery.blockui.min.js",
              "~/Scripts/jquery/jquery.cokie.min.js",
              "~/Scripts/jquery/jquery.uniform.min.js",
              "~/Scripts/jquery/jquery.validate.min.js",
              "~/Scripts/jquery/jquery.unobtrusive-ajax.min.js",
              "~/Scripts/jquery/jquery.validate.unobtrusive.min.js",
              "~/Scripts/bootstrap/bootstrap.min.js",
              "~/Scripts/bootstrap/bootstrapValidator.min.js",
              "~/Scripts/bootstrap/bootstrap-hover-dropdown.min.js",
              "~/Scripts/common/metronic.js",
              "~/Scripts/common/layout.js",
              "~/Scripts/common/demo.js",
             
              "~/Scripts/common/logger.js",
              "~/Scripts/common/jszip.min.js",
               "~/Scripts/common/papaparse.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/angularJs").Include(
                      "~/Scripts/angular/angular.min.js",
                       "~/Scripts/angular/angular-route.min.js",
                       "~/Scripts/angular/angular-resource.min.js",
                       "~/Scripts/angular/angular-ui-router.js",
                      "~/Scripts/angular/angular-sanitize.min.js",
                       "~/Scripts/angular/angular-animate.min.js",
                      "~/Scripts/angular/angular-messages.js",
                      "~/Scripts/angular/angular-cookies.min.js",
                      "~/Scripts/angular/ngStorage.min.js",
                        "~/Scripts/bootstrap/ui-bootstrap-tpls-0.11.2.js",              
                          "~/Content/ngFocus/focusIf.min.js"
                       ));

            //bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
            //     //"~/Scripts/common/kendo.all.min.js",
            //       "~/Scripts/Kendo/kendo.all.min.js",
            //       "~/Scripts/angular/angular-kendo.js"
            //        ));

            bundles.Add(new ScriptBundle("~/bundles/pagescripts").Include(
                   "~/Scripts/pageScripts/Shared/module.js",
                   "~/Scripts/pageScripts/Shared/constant.js",
                   "~/Scripts/pageScripts/Shared/commonService.js",
                  "~/Scripts/pageScripts/Shared/common.js"
                  ));

           

            bundles.Add(new StyleBundle("~/bundles/PageCss").Include(
           "~/Content/plugins/simple-line-icons/simple-line-icons.min.css",
            "~/Content/bootstrap.min.css",
            "~/Content/plugins/uniform/css/uniform.default.css",
             "~/Content/plugins/global/css/tasks.css",
               "~/Content/Site.css",
             
               "~/Content/plugins/global/css/components-rounded.css",
                "~/Content/plugins/global/css/plugins.css",
                "~/Content/plugins/global/css/plugins.css",
                "~/Content/plugins/global/css/themes/default.css",
                "~/Content/plugins/global/css/custom.css"
              ));

            bundles.Add(new StyleBundle("~/bundles/Fontawesome").Include(
                  "~/Content/plugins/font-awesome/css/font-awesome.min.css"
                ));
        }
    }
}