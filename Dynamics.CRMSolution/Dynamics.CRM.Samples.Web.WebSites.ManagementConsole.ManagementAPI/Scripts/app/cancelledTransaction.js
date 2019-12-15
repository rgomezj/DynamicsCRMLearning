pavliksManagementConsoleApp = {};

$(function () {

    pavliksManagementConsoleApp.cancelledTransactionViewModel = function (libCommon, libTost, $, ko) {

        salesOrder = ko.mapping.fromJS({ totalTax: ko.observable(0), totalAmount: ko.observable(0), creditAmount: ko.observable(0), paidAmount: ko.observable(0), outStandingBalance: ko.observable(0) });
            
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

        var returnIndex = function () {
            
            var eventId = libCommon.getValueStorage(libCommon.STORAGEKEYS.EVENTID);
            if (eventId == null) {
                libTost.error('No event has been selected, please go back to CRM and access management console from an event record');
            }
            else {
                window.location.href = libCommon.GetBaseURL() + "/" + libCommon.PAGES.INDEX + "?" + libCommon.QUERYSTRING.EVENTID + "=" + eventId;
            }
            //libTost.info("Generating the receipt");
        };

        var cancelActiveProcess = function () {
            
            var process = libCommon.getProcess();

            if (process == libCommon.PROCESSES.CANCELREGISTRATION || process == libCommon.PROCESSES.WAITLIST || process == libCommon.PROCESSES.TRANSFERREGISTRATION) {
                libCommon.ShowProgressMessage(libCommon.MESSSAGES.SALESORDERPROCESSCANCEL);

                var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);

                $.ajax({
                    url: common.API.SALESALLORDERMANAGEMENTITEMS.replace("{0}", salesOrderId),
                    type: 'DELETE',
                    data: JSON.stringify({ salesOrderItem: salesOrderId }),
                    contentType: "application/json;chartset=utf-8",
                    statusCode: {
                        200: function (result) {
                            
                            //changeStatusOrder(libCommon.getValueStorage(libCommon.STORAGEKEYS.SALESORDERID));
                            libCommon.HideProgressMessage();
                            libCommon.setValueStorage(libCommon.STORAGEKEYS.SALESORDERID, null);

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

        var changeStatusOrder = function (salesOrderId) {

            var process = libCommon.getProcess();
            if (salesOrderId != null) {
                libCommon.ShowProgressMessage(libCommon.MESSSAGES.SALESORDERPROCESSCANCEL);
                var statusOrder = "";
                var stateOrder = "";
                debugger;
                if (process == libCommon.PROCESSES.WAITLIST) {
                    statusOrder = libCommon.ENUMS.STATUSORDERNOMONEY;
                    stateOrder = libCommon.ENUMS.STATEORDERCANCELLED;

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

        // Public methods
        return {
            cancelActiveProcess: cancelActiveProcess,
            returnIndex: returnIndex
        };

    }(common, toastr, $, ko);

    if (document.getElementById("CancelledTransaction") != null) {
        pavliksManagementConsoleApp.cancelledTransactionViewModel.cancelActiveProcess();

        ko.applyBindings(pavliksManagementConsoleApp.cancelledTransactionViewModel, document.getElementById("CancelledTransaction"));
    }
});





