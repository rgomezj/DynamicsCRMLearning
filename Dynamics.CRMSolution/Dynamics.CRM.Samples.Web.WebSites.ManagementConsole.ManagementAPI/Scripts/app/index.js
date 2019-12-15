pavliksManagementConsoleApp = {};

$(function () {

    pavliksManagementConsoleApp.indexViewModel = function (libCommon, libTost, $, ko) {

        // Private model
        registrations = ko.mapping.fromJS([]);

        // Private method to get all registrations
        var getAllregistrations = function (applyBindings) {

            libCommon.ShowProgressMessage();

            var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID)
            localStorage.setItem(common.STORAGEKEYS.EVENTID, eventId);
            $.getJSON(common.API.REGISTRATIONSBYEVENTURL.replace("{0}", eventId), function (data) {
                ko.mapping.fromJS(data, mapping, registrations);
                applyBindings();
            })
            .done(function () {
                libCommon.HideProgressMessage();
            })
            .fail(function (jqxhr, textStatus, error) {
                libCommon.HideProgressMessage();
                libTost.error('There was an error when executing the query');
            });
        };

        var cancelActiveProcess = function (applyBindings) {

            var process = libCommon.getProcess();
            var salesOrderId = libCommon.getValueStorage(libCommon.STORAGEKEYS.SALESORDERID);
            if (salesOrderId != null) {
                //if (process == libCommon.PROCESSES.CANCELREGISTRATION || process == libCommon.PROCESSES.PROCESSPAYMENT || process == libCommon.PROCESSES.MODIFYPRODUCTS) {
                libCommon.ShowProgressMessage(libCommon.MESSSAGES.SALESORDERPROCESSCANCEL);
                //var salesOrder = ko.mapping.fromJS({ id: ko.observable(), totalTax: ko.observable(0), totalAmount: ko.observable(0), creditAmount: ko.observable(0), paidAmount: ko.observable(0), outStandingBalance: ko.observable(0), statusOrder: ko.observable(libCommon.ENUMS.STATUSORDERCOMPLETE), stateOrder: ko.observable(libCommon.ENUMS.STATEORDERFULFILLED) });
                //var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
                // salesOrder.id = salesOrderId;

                $.ajax({

                    url: common.API.SALESALLORDERMANAGEMENTITEMS.replace("{0}", salesOrderId),
                    type: 'DELETE',
                    data: JSON.stringify({ salesOrderItem: salesOrderId }),
                    contentType: "application/json;chartset=utf-8",
                    statusCode: {

                        200: function (result) {

                            libCommon.HideProgressMessage();
                            pavliksManagementConsoleApp.indexViewModel.getAllregistrations(applyBindings);
                            //changeStatusOrder();
                            //getSalesOrderItemsAssocitedToSalesOrder();
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
            } else {
                if (!ko.dataFor(document.getElementById("indexPage"))) {
                    pavliksManagementConsoleApp.indexViewModel.getAllregistrations(applyBindings);
                }
            }

        }

        var registrationNotificationProcess = function (registration) {
            libCommon.ShowProgressMessage(libCommon.MESSSAGES.NOTIFYREGISTRATION);
            //  var registrationOptionsPost = ko.mapping.fromJS({ registrationId: ko.observable(registration.id()), sessions: ko.observableArray(), event: ko.observable(), eventOptions: ko.observableArray(), salesOrder: ko.observable() });
            $.ajax({
                url: libCommon.API.NOTIFYREGISTRATION.replace("{0}", registration.id()),
                type: 'PUT',
                // data: ko.toJSON(registrationOptionsPost),
                contentType: "application/json;chartset=utf-8",
                statusCode: {
                    200: function (result) {
                        libCommon.HideProgressMessage();
                        libTost.info('The notification was sent.');
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
        var changeStatusOrder = function () {

            var process = libCommon.getProcess();
            var salesOrderId = libCommon.getValueStorage(libCommon.STORAGEKEYS.SALESORDERID);
            if (salesOrderId != null) {
                libCommon.ShowProgressMessage(libCommon.MESSSAGES.SALESORDERPROCESSCANCEL);
                var statusOrder = libCommon.ENUMS.STATUSORDERCOMPLETE;
                var stateOrder = libCommon.ENUMS.STATEORDERFULFILLED;

                if (process == libCommon.PROCESSES.WAITLIST) {
                    statusOrder = libCommon.ENUMS.STATUSORDERNOMONEY;
                    stateOrder = libCommon.ENUMS.STATEORDERCANCELLED;
                }

                if (process == libCommon.PROCESSES.WAITLIST || process == libCommon.PROCESSES.CANCELREGISTRATION || process == libCommon.PROCESSES.TRANSFERREGISTRATION) {

                    var salesOrder = ko.mapping.fromJS({ id: ko.observable(salesOrderId), totalTax: ko.observable(0), totalAmount: ko.observable(0), creditAmount: ko.observable(0), paidAmount: ko.observable(0), outStandingBalance: ko.observable(0), statusOrder: ko.observable(statusOrder), stateOrder: ko.observable(stateOrder) });
                    //var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
                    //salesOrder.id = salesOrderId;

                    $.ajax({

                        url: common.API.SALESORDER,
                        type: 'PUT',
                        data: ko.toJSON(salesOrder),
                        contentType: "application/json;chartset=utf-8",
                        statusCode: {

                            200: function (result) {

                                libCommon.HideProgressMessage();
                                if (!ko.dataFor(document.getElementById("indexPage"))) {
                                    pavliksManagementConsoleApp.indexViewModel.getAllregistrations(applyBindings);
                                }
                                //getSalesOrderItemsAssocitedToSalesOrder();
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
                } else {
                    if (!ko.dataFor(document.getElementById("indexPage"))) {
                        pavliksManagementConsoleApp.indexViewModel.getAllregistrations(applyBindings);
                    }
                }
            }
        }


        var activeStatusOrder = function (urlLocation) {

            // var process = libCommon.getProcess();
            var salesOrderId = libCommon.getValueStorage(libCommon.STORAGEKEYS.SALESORDERID);
            if (salesOrderId != null) {
                //if (process == libCommon.PROCESSES.CANCELREGISTRATION || process == libCommon.PROCESSES.PROCESSPAYMENT || process == libCommon.PROCESSES.MODIFYPRODUCTS) {
                libCommon.ShowProgressMessage(libCommon.MESSSAGES.SALESORDERPROCESSCANCEL);
                var salesOrder = ko.mapping.fromJS({ id: ko.observable(salesOrderId), totalTax: ko.observable(0), totalAmount: ko.observable(0), creditAmount: ko.observable(0), paidAmount: ko.observable(0), outStandingBalance: ko.observable(0), statusOrder: ko.observable(libCommon.ENUMS.STATUSORDERMANAGECONSOLE), stateOrder: ko.observable(libCommon.ENUMS.STATEORDERACTIVE) });
                //var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
                //salesOrder.id = salesOrderId;

                $.ajax({

                    url: common.API.SALESORDER,
                    type: 'PUT',
                    data: ko.toJSON(salesOrder),
                    contentType: "application/json;chartset=utf-8",
                    statusCode: {

                        200: function (result) {

                            window.location.href = urlLocation;

                            //getSalesOrderItemsAssocitedToSalesOrder();
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
            } else {
                if (!ko.dataFor(document.getElementById("indexPage"))) {
                    pavliksManagementConsoleApp.indexViewModel.getAllregistrations(applyBindings);
                }
            }

        }


        var cancelRegistration = function () {
            if (isValidSelection()) {
                var registration = currentlyDisplayed();
                var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID);
                libCommon.setProcess(libCommon.PROCESSES.CANCELREGISTRATION);
                libCommon.setValueStorage(libCommon.STORAGEKEYS.SALESORDERID, registration.salesOrderId());
                var urlLocation = libCommon.GetBaseURL() + "/" + libCommon.PAGES.SALESORDERITEMLIST + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + registration.salesOrderId();
                window.location.href = urlLocation;

                //activeStatusOrder(urlLocation);
                //window.location.href = libCommon.GetBaseURL() + "/" + libCommon.PAGES.SALESORDERITEMLIST + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + registration.salesOrderId();
            }
            else {
                libTost.info('Please select a registration for the process');
            }
        }

        var transferRegistration = function () {
            if (isValidSelection()) {
                var registration = currentlyDisplayed();
                var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID);
                libCommon.setProcess(libCommon.PROCESSES.TRANSFERREGISTRATION);
                libCommon.setValueStorage(libCommon.STORAGEKEYS.SALESORDERID, registration.salesOrderId());
                var urlLocation = libCommon.GetBaseURL() + "/" + libCommon.PAGES.TRANSFERREGISTRATION + "?" + libCommon.QUERYSTRING.REGISTRATIONID + "=" + registration.id() + "&" + libCommon.QUERYSTRING.SALESORDERID + "=" + registration.salesOrderId();
                window.location.href = urlLocation;

                //activeStatusOrder(urlLocation);

                //window.location.href = libCommon.GetBaseURL() + "/" + libCommon.PAGES.TRANSFERREGISTRATION + "?" + libCommon.QUERYSTRING.REGISTRATIONID + "=" + registration.id() + "&"+libCommon.QUERYSTRING.SALESORDERID + "=" + registration.salesOrderId();
            }
            else {
                libTost.info('Please select a registration for the process');
            }
        }

        var processPayment = function () {
            if (isValidSelection()) {
                var registration = currentlyDisplayed();
                var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID);
                libCommon.setProcess(libCommon.PROCESSES.PROCESSPAYMENT);
                libCommon.setValueStorage(libCommon.STORAGEKEYS.SALESORDERID, registration.salesOrderId());
                var urlLocation = libCommon.GetBaseURL() + "/" + libCommon.PAGES.SALESORDERITEMLIST + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + registration.salesOrderId();
                window.location.href = urlLocation;

                //activeStatusOrder(urlLocation);

                //window.location.href = libCommon.GetBaseURL() + "/" + libCommon.PAGES.SALESORDERITEMLIST + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + registration.salesOrderId();
            }
            else {
                libTost.info('Please select a registration for the process');
            }
        }

        var swapRegistration = function () {
            if (isValidSelection()) {
                var registration = currentlyDisplayed();
                var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID);
                libCommon.setProcess(libCommon.PROCESSES.SWAP);
                libCommon.setValueStorage(libCommon.STORAGEKEYS.SALESORDERID, registration.salesOrderId());
                var urlLocation = libCommon.GetBaseURL() + "/" + libCommon.PAGES.SWAPREGISTRATION + "?" + libCommon.QUERYSTRING.REGISTRATIONID + "=" + registration.id();
                window.location.href = urlLocation;

                //activeStatusOrder(urlLocation);

                //window.location.href = libCommon.GetBaseURL() + "/" + libCommon.PAGES.SWAPREGISTRATION + "?" + libCommon.QUERYSTRING.REGISTRATIONID + "=" + registration.id();
            }
            else {
                libTost.info('Please select a registration for the process');
            }
        }

        var modifySessions = function () {
            if (isValidSelection()) {
                var registration = currentlyDisplayed();
                var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID);
                libCommon.setProcess(libCommon.PROCESSES.MODIFYSESSIONOPTIONS);
                libCommon.setValueStorage(libCommon.STORAGEKEYS.SALESORDERID, registration.salesOrderId());
                var urlLocation = libCommon.GetBaseURL() + "/" + libCommon.PAGES.MODIFYSESSIONS + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + registration.salesOrderId() + "&" + libCommon.QUERYSTRING.EVENTID + "=" + eventId + "&" + libCommon.QUERYSTRING.REGISTRATIONID + "=" + registration.id();
                window.location.href = urlLocation;

                //activeStatusOrder(urlLocation);

                //window.location.href = libCommon.GetBaseURL() + "/" + libCommon.PAGES.MODIFYSESSIONS + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + registration.salesOrderId() + "&" + libCommon.QUERYSTRING.EVENTID + "=" + eventId + "&" + libCommon.QUERYSTRING.REGISTRATIONID + "=" + registration.id();
            }
            else {
                libTost.info('Please select a registration for the process');
            }
        }

        var modifyProducts = function () {
            if (isValidSelection()) {
                var registration = currentlyDisplayed();
                var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID);
                libCommon.setProcess(libCommon.PROCESSES.MODIFYPRODUCTS);
                libCommon.setValueStorage(libCommon.STORAGEKEYS.SALESORDERID, registration.salesOrderId());
                var urlLocation = libCommon.GetBaseURL() + "/" + libCommon.PAGES.SALESORDERITEMLIST + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + registration.salesOrderId();
                window.location.href = urlLocation;

                //activeStatusOrder(urlLocation);

                //window.location.href = libCommon.GetBaseURL() + "/" + libCommon.PAGES.SALESORDERITEMLIST + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + registration.salesOrderId();
            }
            else {
                libTost.info('Please select a registration for the process');
            }
        }

        currentlyDisplayed = ko.observable({ id: ko.observable("") });

        var isValidSelection = function () {
            if (currentlyDisplayed().id() != "") {
                return true;
            }
            else {
                return false;
            }
        }

        var selectRow = function (registration) {
            currentlyDisplayed(registration);
        }

        this.anyRegistration = ko.computed(function () {
            if (registrations().length > 0) {
                return true;
            }
            else {
                return false;
            }
        });

        /* Extending the mapping to include a property for all of the elements of the observable array*/
        var mapping = {
            '': {
                create: function (options) {
                    return new createRegistration(options.data, currentlyDisplayed);
                }
            }
        }

        var createRegistration = function (registration, currentlyDisplayed) {
            var self = this;
            var result = ko.mapping.fromJS(registration);

            result.isSelected = ko.computed(function () {

                return currentlyDisplayed().id() === result.id();
            }, result);

            result.NameAndJobTitle = ko.computed(function () {
                var nameReturn = "";
                if (result.contactFullName() != null) {
                    nameReturn = nameReturn + result.contactFullName();
                }
                if (result.contactJobTitle() != null) {
                    nameReturn = nameReturn + "(" + result.contactJobTitle() + ")";
                }
                return nameReturn;
            }, result);

            return result;
        };
        /* END extending the mapping*/


        // Public methods
        return {
            getAllregistrations: getAllregistrations,
            cancelRegistration: cancelRegistration,
            transferRegistration: transferRegistration,
            processPayment: processPayment,
            swapRegistration: swapRegistration,
            modifySessions: modifySessions,
            modifyProducts: modifyProducts,
            selectRow: selectRow,
            registrations: registrations,
            cancelActiveProcess: cancelActiveProcess,
            registrationNotificationProcess: registrationNotificationProcess
        };

    }(common, toastr, $, ko);

    if (document.getElementById("indexPage") != null) {
        pavliksManagementConsoleApp.indexViewModel.cancelActiveProcess(applyBindings);

    }

    function applyBindings() {
        if (document.getElementById("indexPage") != null) {

            if (!ko.dataFor(document.getElementById("indexPage"))) {
                ko.applyBindings(pavliksManagementConsoleApp.indexViewModel, document.getElementById("indexPage"));

            }
        }
    }
});



