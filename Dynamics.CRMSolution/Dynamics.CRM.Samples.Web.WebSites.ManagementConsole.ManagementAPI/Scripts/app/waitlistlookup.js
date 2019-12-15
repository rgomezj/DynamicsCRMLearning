pavliksManagementConsoleApp = {};

$(function () {
    
    pavliksManagementConsoleApp.waitlistlookupViewModel = function (libCommon, libTost, $, ko) {

        // Private model

        fullName = ko.observable("");
        waitlistSince = ko.observable();
        registerLink = ko.observable("");

        loading = ko.observable(false);

        

        var bindData = function (name, _waitlist) {
            debugger;
            if (_waitlist.contact.fullName()) {
                fullName = _waitlist.contact.fullName;
            }
            if (_waitlist.createdOn()) {
                waitlistSince = _waitlist.createdOn;
            }
            if (_waitlist.registrationLink) {
                registerLink = _waitlist.registrationLink;
            }


            if (document.getElementById("waitlistsLookup") != null) {
                ko.applyBindings(pavliksManagementConsoleApp.waitlistlookupViewModel, document.getElementById("waitlistsLookup"));
            }
        }


        // Public methods
        return {
            bindData: bindData
        };

    }(common, toastr, $, ko);
});



