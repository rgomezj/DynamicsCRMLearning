pavliksManagementConsoleApp = {};

$(function () {

    pavliksManagementConsoleApp.navigationViewModel = function (libCommon, libTost, $, ko) {


        var goHomePage = function () {
            var eventId = localStorage.getItem(libCommon.STORAGEKEYS.EVENTID);
            if (eventId == null) {
                libTost.error('No event has been selected, please go back to CRM and access management console from an event record');
            }
            else {
                window.location.href = libCommon.GetBaseURL() + "/" + libCommon.PAGES.INDEX + "?" + libCommon.QUERYSTRING.EVENTID + "=" + eventId;
            }
        }

        var goWaitlists = function () {
            var eventId = localStorage.getItem(libCommon.STORAGEKEYS.EVENTID);
            if (eventId == null) {
                libTost.error('No event has been selected, please go back to CRM and access management console from an event record');
            }
            else {
                window.location.href = libCommon.GetBaseURL() + "/" + libCommon.PAGES.WAITLISTS + "?" + libCommon.QUERYSTRING.EVENTID + "=" + eventId;
            }

        }

        // Public methods
        return {
            goHomePage: goHomePage,
            goWaitlists: goWaitlists
        };

    }(common, toastr, $, ko);

    if (document.getElementById("Navigation") != null) {
        if (!ko.dataFor(document.getElementById("Navigation"))) {
            ko.applyBindings(pavliksManagementConsoleApp.navigationViewModel, document.getElementById("Navigation"));
        }
    }
});



