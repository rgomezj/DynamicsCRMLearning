pavliksManagementConsoleApp = {};

$(function () {
    
    pavliksManagementConsoleApp.contactsViewModel = function (libCommon, libTost, $, ko) {

        // Private model
        contacts = ko.mapping.fromJS([]);

        fullName = ko.observable("");
        otherCriteria = ko.observable("");

        loading = ko.observable(false);

        var callBackContact;

        // Private method to get all waitlists
        var getContactsListFiltered = function (name) {
            loading(true);
            $.getJSON(common.API.CONTACTSBYNAME.replace("{0}", name), function (data) {
                ko.mapping.fromJS(data, contacts);
                loading(false);
            })
            .done(function () {
                loading(false);
            })
            .fail(function (jqxhr, textStatus, error) {
                libTost.error('There was an error when executing the query');
                loading(false);
            });
        };

        var selectContact = function (contact) {
            
            callBackContact(contact);
            $("[data-dismiss=modal]").trigger({ type: "click" });
        }

        var searchContacts = function () {
            getContactsListFiltered(fullName());
        }

        var bindData = function (name, _callbackContact) {
            callBackContact = _callbackContact;
            getContactsListFiltered(name);
            if (document.getElementById("contactsLookup") != null) {
                ko.applyBindings(pavliksManagementConsoleApp.contactsViewModel, document.getElementById("contactsLookup"));
            }
        }


        // Public methods
        return {
            selectContact: selectContact,
            bindData: bindData,
            searchContacts: searchContacts
        };

    }(common, toastr, $, ko);
});



