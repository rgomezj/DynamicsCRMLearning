pavliksManagementConsoleApp = {};

$(function () {

    pavliksManagementConsoleApp.swapRegistrationViewModel = function (libCommon, libTost, $, ko) {

        // Private model
        swapRegistrationCurrent = ko.mapping.fromJS({ contact: ko.observable({}), salesOrder: ko.observable({}) });

        selectedContactSwap = ko.mapping.fromJS({ fullName: ko.observable(""), id: ko.observable(""), email: ko.observable("") });

        // Private method to get all swapRegistrations
        var getCurrentContact = function () {

            libCommon.ShowProgressMessage();
            
            var registrationId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.REGISTRATIONID);
            
            $.getJSON(libCommon.API.REGISTRATIONBYID.replace("{0}", registrationId), function (data) {
                
                ko.mapping.fromJS(data, swapRegistrationCurrent);
            })
            .done(function () {
                libCommon.HideProgressMessage();
            })
            .fail(function (jqxhr, textStatus, error) {
                libCommon.HideProgressMessage();
                libTost.error('There was an error when executing the query');
            });
        };

        var swapRegistration = function () {
            if (selectedContactSwap.fullName() == "")
            {
                libTost.info('Please select a new registrant');
            }
            else
            {
                swapRegistrationProcess();
            }
        }

        var swapRegistrationProcess = function () {

            libCommon.ShowProgressMessage(libCommon.MESSSAGES.SWAPPROCESSING);

            var registrationId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.REGISTRATIONID);
            
            var registrationPost = swapRegistrationCurrent;
            registrationPost.id = ko.observable(registrationId);
            registrationPost.contact = ko.observable({ fullName: ko.observable(""), id: ko.observable(""), email: ko.observable("") });
            registrationPost.contact = selectedContactSwap;
            registrationPost.name = selectedContactSwap.fullName();
            

            $.ajax({
                url: common.API.REGISTRATIONUPDATE,
                type: 'PUT',
                data: ko.toJSON(registrationPost),
                contentType: "application/json;chartset=utf-8",
                statusCode: {
                    200: function (result) {
                        libCommon.HideProgressMessage();
                        libTost.info('Saved');
                        window.location.href = libCommon.GetBaseURL() + "/" + libCommon.PAGES.SUCESSFULTRANSACTION + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + result.salesOrderId;
                    },
                    404: function () {
                        libCommon.HideProgressMessage();
                        libTost.error('Resource Not found');
                    },
                    400: function () {
                        libCommon.HideProgressMessage();
                        libTost.error('Error when processing the request');
                    },
                    500: function () {
                        libCommon.HideProgressMessage();
                        libTost.error('Error when processing the request');
                    }
                }
            });
        };

        var openLookupContacts = function () {
          
            pavliksManagementConsoleApp.contactsViewModel.bindData("", assignSelectedContact);
        }

        var assignSelectedContact = function (contact) {
            
            selectedContactSwap.fullName(contact.fullName());
            selectedContactSwap.email(contact.email());
            selectedContactSwap.id(contact.id());
        }
      
        // Public methods
        return {
            getCurrentContact: getCurrentContact,
            swapRegistration: swapRegistration,
            openLookupContacts: openLookupContacts,
            assignSelectedContact: assignSelectedContact
        };

    }(common, toastr, $, ko);

    if (document.getElementById("swapRegistration") != null) {
        pavliksManagementConsoleApp.swapRegistrationViewModel.getCurrentContact();
        ko.applyBindings(pavliksManagementConsoleApp.swapRegistrationViewModel, document.getElementById("swapRegistration"));
    }
});



