pavliksManagementConsoleApp = {};

$(function () {

    pavliksManagementConsoleApp.successfulTransactionViewModel = function (libCommon, libTost, $, ko) {

        salesOrder = ko.mapping.fromJS({ totalTax: ko.observable(0), totalAmount: ko.observable(0), creditAmount: ko.observable(0), paidAmount: ko.observable(0), outStandingBalance: ko.observable(0) });

        contactsForEmail = ko.observableArray();

        selectedContact = ko.mapping.fromJS({ fullName: ko.observable(""), id: ko.observable(""), email: ko.observable("") });

        selectedContacts = ko.observableArray();

        process = ko.observable("");

        isSwap = ko.computed(function () {
            if (process() != null && process() == libCommon.PROCESSES.SWAP) {
                return true;
            }
            else {
                return false;
            }
        });

        isPayment = ko.computed(function () {
            if (process() != null && process() != libCommon.PROCESSES.SWAP) {
                return true;
            }
            else {
                return false;
            }
        });


        var getSalesInformation = function () {

            libCommon.ShowProgressMessage();

            var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);

            $.getJSON(libCommon.API.SALESORDERBYID.replace("{0}", salesOrderId), function (data) {

                ko.mapping.fromJS(data, salesOrder);
            })
            .done(function () {
                libCommon.HideProgressMessage();
            })
            .fail(function (jqxhr, textStatus, error) {
                libCommon.HideProgressMessage();
                libTost.error('There was an error when executing the query');
            });
        };

        var emailReceipt = function () {
            
            libTost.info("Emailing the receipt");
            var emailViewModel = ko.mapping.fromJS({ contacts: ko.observableArray(), salesOrder: ko.observable(), typeOfTransaction: ko.observableArray() });
            emailViewModel.contacts = contactsForEmail;
            emailViewModel.typeOfTransaction = 1;
            emailViewModel.salesOrder = salesOrder;

            $.ajax({
                url: common.API.SENDEMAILRECEIPT,
                type: 'POST',
                data: ko.toJSON(emailViewModel),
                contentType: "application/json;chartset=utf-8",
                statusCode: {
                    200: function (result) {
                        libCommon.HideProgressMessage();
                    },
                    404: function () {
                        libCommon.HideProgressMessage();
                        libTost.error('Resource Not found');
                    },
                    400: function () {
                        libCommon.HideProgressMessage();
                        libTost.error('Error when processing the request');
                    }
                }
            });
        }

        var generateReceipt = function () {
            
            var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
            var ajaxRequest = $.ajax({
                type: "GET",
                url: common.API.GENERATEREPORTRECEIPT.replace("{0}", salesOrderId),
                contentType: false,
                processData: true
            });

            ajaxRequest.done(function (result) {
                
                window.open(result, "resizeable,scrollbar");

            });

            ajaxRequest.fail(function (jqXHR, textStatus, errorThrown) {
                
                var test;
            });
        };

        var openLookupContacts = function () {

            pavliksManagementConsoleApp.contactsViewModel.bindData("", addSelectedContact);
        }

        var addSelectedContact = function (contact) {
            contactsForEmail.push(contact);
        }

        var removeContacts = function () {
            var id = "";
            for (var i = 0; i < selectedContacts().length; i++) {
                id = selectedContacts()[i];
                contactsForEmail.remove(function (item) {

                    return item.id() == id;
                })
            }
        }
        var goHomePage = function () {
            var eventId = localStorage.getItem(libCommon.STORAGEKEYS.EVENTID);
            if (eventId == null) {
                libTost.error('No event has been selected, please go back to CRM and access management console from an event record');
            }
            else {
                window.location.href = libCommon.GetBaseURL() + "/" + libCommon.PAGES.INDEX + "?" + libCommon.QUERYSTRING.EVENTID + "=" + eventId;
            }
        }


        var confimedActiveProcess = function () {
            var process = libCommon.getProcess();

            if (process == libCommon.PROCESSES.CANCELREGISTRATION || process == libCommon.PROCESSES.WAITLIST || process == libCommon.PROCESSES.TRANSFERREGISTRATION) {
                libCommon.ShowProgressMessage(libCommon.MESSSAGES.SALESORDERPROCESSCANCEL);

                var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
                var isCheque = libCommon.getValueStorage(libCommon.STORAGEKEYS.ISCHEQUE);
                var chequeNumber = libCommon.getValueStorage(libCommon.STORAGEKEYS.CHEQUENUMBER);

                var paymentModel = ko.mapping.fromJS({ id: salesOrderId, chequeNumber: chequeNumber, isCheque: isCheque });


                $.ajax({
                    url: common.API.ORDERMANAGEMENTITEMCONFIRM,
                    type: 'PUT',
                    data: ko.toJSON(paymentModel),
                    contentType: "application/json;chartset=utf-8",
                    statusCode: {
                        200: function (result) {

                            libCommon.setValueStorage(libCommon.STORAGEKEYS.SALESORDERID, null);

                            libCommon.HideProgressMessage();
                            //changeStatusOrder();
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
            }
        }

        var changeStatusOrder = function () {
            var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
            if (salesOrderId != null) {
                libCommon.ShowProgressMessage();
                var salesOrder = ko.mapping.fromJS({ id: ko.observable(salesOrderId), totalTax: ko.observable(0), totalAmount: ko.observable(0), creditAmount: ko.observable(0), paidAmount: ko.observable(0), outStandingBalance: ko.observable(0), statusOrder: ko.observable(libCommon.ENUMS.STATUSORDERCOMPLETE), stateOrder: ko.observable(libCommon.ENUMS.STATEORDERFULFILLED) });
                $.ajax({
                    url: common.API.SALESORDER,
                    type: 'PUT',
                    data: ko.toJSON(salesOrder),
                    contentType: "application/json;chartset=utf-8",
                    statusCode: {
                        200: function (result) {
                            libCommon.HideProgressMessage();
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
            } 
        }


        // Public methods
        return {
            confimedActiveProcess: confimedActiveProcess,
            emailReceipt: emailReceipt,
            generateReceipt: generateReceipt,
            openLookupContacts: openLookupContacts,
            addSelectedContact: addSelectedContact,
            getSalesInformation: getSalesInformation,
            removeContacts: removeContacts,
            goHomePage: goHomePage

        };

    }(common, toastr, $, ko);

    if (document.getElementById("SuccessfulTransaction") != null) {
        pavliksManagementConsoleApp.successfulTransactionViewModel.getSalesInformation();
        pavliksManagementConsoleApp.successfulTransactionViewModel.confimedActiveProcess();

        ko.applyBindings(pavliksManagementConsoleApp.successfulTransactionViewModel, document.getElementById("SuccessfulTransaction"));
    }
});





