// ***********************************************************************
// Assembly         : EcubeWebPortalCMS
// Author           : anish.parikh
// Created          : 07-19-2016
//
// Last Modified By : anish.parikh
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="BundleConfig.cs" company="string.Empty">
//     Copyright ©  2016
// </copyright>
// <summary>KCSPL</summary>
// ***********************************************************************

/// <summary>
/// The EcubeWebPortalCMS namespace.
/// </summary>
namespace EcubeWebPortalCMS
{
    using System.Web.Optimization;

    /// <summary>
    /// Class BundleConfig.
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Register Bundles..
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/assets/css/site.css"));
            bundles.Add(new StyleBundle("~/Content/Style/login").Include(
                                    "~/Content/assets/css/site.css",
                                    //"~/Content/Style/style.css",
                                    //"~/Scripts/Library/bootstrap/bootstrap-responsive.min.css",
                                    //"~/Scripts/Library/bootstrap/bootstrap.css",
                                    "~/Scripts/Library/jAlert/jquery.alerts.css",
                                    "~/Scripts/Library/chosen/chosen.css",
                                    "~/Scripts/Library/tipsy/tipsy.css",
                                    "~/Scripts/Library/uniform/uniform.default.css"));
            bundles.Add(new StyleBundle("~/Content/Style/css").Include(
                        "~/Content/Style/jquery-ui-1.10.4.custom.css",
                        "~/Content/assets/css/site.css",
                        //"~/Content/Style/style.css",
                        "~/Content/Style/font-awesome.css",
                        "~/Content/Style/ui.jqgrid.css",
                        //"~/Scripts/Library/bootstrap/bootstrap-responsive.min.css",
                        //"~/Scripts/Library/bootstrap/bootstrap.css",
                        "~/Scripts/Library/jAlert/jquery.alerts.css",
                        "~/Scripts/Library/chosen/chosen.css",
                        "~/Scripts/Library/tipsy/tipsy.css",
                        "~/Scripts/Library/uniform/uniform.default.css"));

            bundles.Add(new StyleBundle("~/Content/Style/FullCalender").Include(
                   "~/Scripts/Library/fullcalendar/fullcalendar.css",
                   "~/Scripts/Library/fullcalendar/fullcalendar.print.css"));
          
            /*All Required jQuery List*/
            bundles.Add(new ScriptBundle("~/Scripts/login").Include(
                           "~/Scripts/JQuery/jquery-1.11.0.js",
                            "~/Scripts/JQuery/jquery-migrate-1.2.1.js",
                            "~/Scripts/Library/tipsy/jquery.tipsy.js",
                            "~/Scripts/Library/uniform/jquery.uniform.js",
                            "~/Scripts/Library/jAlert/jquery.alerts.js",
                            "~/Scripts/Utility/Login.js"));

            bundles.Add(new ScriptBundle("~/Scripts/ConnectionSetting").Include(
                           "~/Scripts/JQuery/jquery-1.11.0.js",
                            "~/Scripts/JQuery/jquery-migrate-1.2.1.js",
                            "~/Scripts/Library/tipsy/jquery.tipsy.js",
                            "~/Scripts/Library/uniform/jquery.uniform.js",
                            "~/Scripts/Library/jAlert/jquery.alerts.js",
                            "~/Scripts/Utility/ConnectionSetting.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Js").Include(
                           "~/Scripts/JQuery/jquery-1.11.0.js",
                            "~/Scripts/JQuery/jquery-migrate-1.2.1.js",
                            "~/Scripts/JQuery/jquery.debouncedresize.js",
                            "~/Scripts/JQuery/jquery-ui-1.10.4.custom.js",
                            "~/Scripts/JQuery/jquery.jqGrid.src.js",
                            "~/Scripts/JQuery/grid.locale-en.js",
                            "~/Scripts/JQuery/grid.locale-en.js",
                            "~/Content/assets/js/bootstrap.min.js",
                            //"~/Scripts/Library/bootstrap/bootstrap.js",
                            "~/Scripts/Library/tipsy/jquery.tipsy.js",
                            "~/Scripts/Library/uniform/jquery.uniform.js",
                            "~/Scripts/Library/inputmask/jquery.maskedinput-1.3.js",
                            "~/Scripts/Library/timepicker/jquery-ui-timepicker.js",
                            "~/Scripts/Library/chosen/chosen.jquery.js",
                            "~/Scripts/Library/jAlert/jquery.alerts.js",
                            "~/Scripts/Utility/Common.js",
                            "~/Scripts/Utility/shortcut.js",
                            "~/Scripts/Utility/UserActive.js"));

            bundles.Add(new ScriptBundle("~/Content/Script/Widget").Include(
                       "~/Scripts/Library/colorpicker/bootstrap-colorpicker.js",
                       "~/Scripts/Library/sticky/sticky.js",
                       "~/Scripts/Library/multiselect/js/jquery.multi-select.js",
                       "~/Scripts/Library/multiselect/js/jquery.quicksearch.js",
                       "~/Scripts/Library/tag_handler/jquery.taghandler.js",
                       "~/Scripts/Library/DateTimePicker/bootstrap-datepicker.js"));

            bundles.Add(new ScriptBundle("~/Content/Script/HighCharts").Include(
                      "~/Scripts/Demo/Chart.js",
                      "~/Scripts/Library/chart/jquery.highcharts.js"));

            bundles.Add(new ScriptBundle("~/Content/Script/FullCalender").Include(
                     "~/Scripts/Library/fullcalendar/fullcalendar.min.js",
                     "~/Scripts/Demo/FCalendar.js"));
          BundleTable.EnableOptimizations = true; // Convert to minify file
        }
    }
}
