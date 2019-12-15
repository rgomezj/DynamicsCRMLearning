using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-1.11.4.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/toast").Include(
            "~/Scripts/toastr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
            "~/Scripts/moment.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/numeral").Include(
        "~/Scripts/numeral/numeral.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/knockout.validation.js",
                "~/Scripts/knockout.mapping-latest.js",
                "~/Scripts/app/koBindingHandlers.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/sammy-{version}.js",
                "~/Scripts/app/common.js",
                "~/Scripts/app/waitlist.js",
                "~/Scripts/app/events.js",
                "~/Scripts/app/registrationOptions.js",
                "~/Scripts/app/index.js",
                "~/Scripts/app/navigation.js",
                "~/Scripts/app/swapRegistration.js",
                "~/Scripts/app/contacts.js",
                "~/Scripts/app/successfulTransaction.js",
                "~/Scripts/app/salesOrderItemsList.js",
                "~/Scripts/app/registrationTransfer.js",
                "~/Scripts/app/modifiedSession.js"
                ));
              

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.css",
                 "~/Content/Site.css"));
        }
    }
}
