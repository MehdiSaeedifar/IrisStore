using System.Web.Optimization;

namespace Iris.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/bootstrap.js"));


            bundles.Add(new ScriptBundle("~/bundles/layout").Include(
                       "~/Scripts/layout.js",
                       "~/Scripts/toastr.js"));


            bundles.Add(new ScriptBundle("~/bundles/adminlayout").Include(
                      "~/Scripts/AdminLTE/app.js",
                      "~/Scripts/bootstrap-confirmation.js",
                      "~/Scripts/toastr.js",
                      "~/Scripts/admin.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqGrid").Include(
                "~/Scripts/jqGrid/js/i18n/grid.locale-fa.js",
                       "~/Scripts/jqGrid/js/jquery.jqGrid.js"));

            bundles.Add(new ScriptBundle("~/bundles/fileUpload").Include(
              "~/Scripts/jQuery-File-Upload/js/jquery.iframe-transport.js",
                     "~/Scripts/jQuery-File-Upload/js/jquery.fileupload.js",
                     "~/Scripts/jQuery-File-Upload/js/jquery.fileupload-process.js",
                     "~/Scripts/jQuery-File-Upload/js/jquery.fileupload-image.js",
                     "~/Scripts/jQuery-File-Upload/js/jquery.fileupload-validate.js",
                     "~/Scripts/jQuery-File-Upload/js/jquery.fileupload-ui.js"));



            bundles.Add(new ScriptBundle("~/bundles/jquerynumber").Include(
                "~/Scripts/jquery.number.js"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
              "~/Scripts/select2.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-editable").Include(
              "~/Scripts/bootstrap-editable.js"));

            bundles.Add(new ScriptBundle("~/bundles/PersianDatePicker").Include(
              "~/Scripts/PersianDatePicker.js"));


            bundles.Add(new ScriptBundle("~/bundles/manage-product").Include(
                "~/Scripts/Admin/Product/info-tab.js",
                        "~/Scripts/Admin/Product/price-tab.js",
                        "~/Scripts/Admin/Product/discount-tab.js",
                        "~/Scripts/Admin/Product/image-tab.js"));



            bundles.Add(new ScriptBundle("~/bundles/search-product").Include(
            "~/Scripts/path.js",
                   "~/Scripts/jquery.ui.slider-rtl.js",
                   "~/Scripts/search-product.js"));


            bundles.Add(new ScriptBundle("~/bundles/product-page").Include(
            "~/Scripts/highcharts.src.js",
                   "~/Scripts/jquery.magnific-popup.js",
                   "~/Scripts/star-rating.js",
                   "~/Scripts/starRating-plugin.js",
                   "~/Scripts/product-page.js"));



            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));



            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-rtl/bootstrap-rtl.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/font-awesome.css",
                      "~/Content/animate.css",
                      "~/Content/awesome-bootstrap-checkbox.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                  "~/Content/toastr.css",
                      "~/Content/site.css"));


            bundles.Add(new StyleBundle("~/Content/AdminLTE/admintheme").Include(
                     "~/Content/AdminLTE/AdminLTE.css",
                     "~/Content/AdminLTE/skins/skin-blue.css"
                     ));

            bundles.Add(new StyleBundle("~/Content/admin").Include(
                    "~/Content/toastr.css",
                    "~/Scripts/jqGrid/css/ui.jqgrid.css",
                    "~/Content/admin.css"
                    ));


            bundles.Add(new StyleBundle("~/Content/themes/base/jqueryUi").Include(
                      "~/Content/themes/base/core.css",
          "~/Content/themes/base/resizable.css",
          "~/Content/themes/base/selectable.css",
          "~/Content/themes/base/accordion.css",
          "~/Content/themes/base/autocomplete.css",
          //"~/Content/themes/base/button.css",
          "~/Content/themes/base/dialog.css",
          "~/Content/themes/base/slider.css",
          //"~/Content/themes/base/tabs.css",
          //"~/Content/themes/base/datepicker.css",
          //"~/Content/themes/base/progressbar.css",
          "~/Content/themes/base/theme.css"
          ));


            bundles.Add(new StyleBundle("~/Content/product-page").Include(
                   
                     "~/Content/star-rating.css",
                     "~/Content/magnific-popup.css"));
        }
    }
}
