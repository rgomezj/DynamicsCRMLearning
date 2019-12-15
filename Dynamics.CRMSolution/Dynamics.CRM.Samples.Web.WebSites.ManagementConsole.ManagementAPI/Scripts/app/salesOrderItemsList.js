pavliksManagementConsoleApp = {};

$(function () {

    pavliksManagementConsoleApp.salesOrderItemsListViewModel = function (libCommon, libTost, $, ko) {

        // Private model
        orderTransactionItemsByOrder = ko.mapping.fromJS([]);

        salesOrderItems = ko.mapping.fromJS([]);

        salesOrder = ko.mapping.fromJS({ totalTax: ko.observable(0), totalAmount: ko.observable(0), creditAmount: ko.observable(0), paidAmount: ko.observable(0), valueToPay: ko.observable(0), outStandingBalance: ko.observable(0), balanceToPay: ko.observable(0), paymentType: ko.observable(""), chequeNumber: ko.observable(""), willSpecifyLater: ko.observable(false), isCheque: ko.observable(false) });

        pageTitle = ko.observable("");

        messagePayment = ko.observable("");
        totalCreditDisabled = ko.observable(false);
        totalProcess = ko.observable(0);

        refundAvailable = ko.observable(true);

        // Private method to get all registrations
        var getSalesOrderItemsAssocitedToSalesOrder = function (applyBindings) {

            libCommon.ShowProgressMessage();

            var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID)

            $.getJSON(common.API.SALESORDERITEMSBYORDEID.replace("{0}", salesOrderId), function (data) {
                ko.mapping.fromJS(data, mapping, salesOrderItems);
                getOrderTransactionItem(applyBindings);
            })

            .done(function () {
                libCommon.HideProgressMessage();
            })
            .fail(function (jqxhr, textStatus, error) {
                libCommon.HideProgressMessage();
                libTost.error('There was an error when executing the query');
            });
        };

        var getOrderTransaction = function () {

            libCommon.ShowProgressMessage();
            libCommon.setValueStorage(libCommon.STORAGEKEYS.ISCHEQUE, false);

            if (salesOrder.paymentType() != "CreditCard") {
                pavliksManagementConsoleApp.paymentViewModel.bindData("", performTransaction);
                $("#Payment").trigger({ type: "click" });
                //$("[data-toggle=modal]").trigger({ type: "click" });
                //$("#PaymentModal").modal({ show: false })
                //$("#PaymentModal").show();
                libCommon.HideProgressMessage();
            } else {
                performTransaction();
            }
        };

        var performTransaction = function (chequeNumber, willSpecifyLater) {

            if (typeof (chequeNumber) != 'undefined') {
                salesOrder.chequeNumber(chequeNumber);
            }
            if (typeof (willSpecifyLater) != 'undefined') {
                salesOrder.willSpecifyLater(willSpecifyLater);
            }

            var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
            salesOrder.id = salesOrderId;

            $.getJSON(libCommon.API.ORDERTRANSACTIONBYORDER.replace("{0}", salesOrderId), function (data) {
                if (data != null) {
                    $.ajax({
                        url: common.API.ORDERTRANSACTION,
                        type: 'POST',
                        data: ko.toJSON(salesOrder),
                        contentType: "application/json;chartset=utf-8",
                        statusCode: {
                            200: function (result) {
                                if (result.isCheque) {
                                    libCommon.setValueStorage(libCommon.STORAGEKEYS.ISCHEQUE, result.isCheque);
                                    libCommon.setValueStorage(libCommon.STORAGEKEYS.CHEQUENUMBER, result.chequeNumber);
                                }
                                processResponse(result);
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
                            500: function (result) {
                                libCommon.HideProgressMessage();
                                libTost.error(result.responseJSON.message);
                            }
                        }
                    });
                }
                else {
                    libTost.error('There is no an original payment to perform the refund');
                }

            })
               .done(function () {
                   libCommon.HideProgressMessage();
               })
               .fail(function (jqxhr, textStatus, error) {
                   libCommon.HideProgressMessage();
                   libTost.error('There was an error when executing the query');
               });
        }


        var processResponse = function (response) {
            debugger;
            var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
            if (response.ok) {
                var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
                window.location = libCommon.GetBaseURL() + "/" + libCommon.PAGES.SUCESSFULTRANSACTION + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + salesOrderId;
            }
            else {
                var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
                window.location = libCommon.GetBaseURL() + "/" + libCommon.PAGES.CANCELLEDTRANSACTION + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + salesOrderId;

                //libTost.error('the Transaction cannot be completed at this time, please try again');
            }

        }

        function receiveMessage(event) {
            if (event.data == "OK") {
                var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
                window.location = libCommon.GetBaseURL() + "/" + libCommon.PAGES.SUCESSFULTRANSACTION + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + salesOrderId;

            }
            else {
                var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
                window.location = libCommon.GetBaseURL() + "/" + libCommon.PAGES.CANCELLEDTRANSACTION + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + salesOrderId;
            }
        }
        salesOrder.valueToPay = ko.computed(function () {
            var amount = 0;
            for (var i = 0; i < salesOrderItems().length; i++) {
                var soItem = salesOrderItems()[i];
                if (soItem.refundedLocal() && !isNaN(salesOrderItems()[i].amountRefunded())) {
                    amount += parseFloat(salesOrderItems()[i].amountRefunded());
                }
            }
            amount = Math.round(amount * 100) / 100;
            salesOrder.balanceToPay(amount);
            salesOrder.paidAmount(amount);
            return amount;
        });
        // Created on july 21-2016 JAB
        var processSale = function () {

            var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
            var process = libCommon.getProcess();
            var paymentValid = validateTotalPayment(process);
            if (paymentValid) {
                if (process == libCommon.PROCESSES.CANCELREGISTRATION) {
                    getOrderTransaction();
                }
                else {
                    getURLSalesOrderAdministration("salesorder", salesOrderId);
                }
            }
        }


        var validateTotalPayment = function (process) {
            var validPayment = true;
            if (parseInt(salesOrder.valueToPay()) == "") {
                libTost.error('Please specify a value to pay');
                validPayment = false;
            }

            //if (process == libCommon.PROCESSES.CANCELREGISTRATION) {
            //    if (parseInt(salesOrder.valueToPay()) > salesOrder.paidAmount()) {
            //        libTost.error("Value to refund can't be greater than the amount paid.");
            //        validPayment = false;
            //    }
            //}
            return validPayment;
        }

        var callPaymentPage = function (typeName, salesOrderId, salesOrderPaymentURL) {
            var sUrlCalendar = salesOrderPaymentURL + "?"
                                          + libCommon.QUERYSTRING.TYPENAME + "=" + typeName + "&"
                                          + libCommon.QUERYSTRING.ID + "=" + salesOrderId + "&"
                                          + libCommon.QUERYSTRING.TOTCREDIT + "=" + salesOrder.creditAmount() + "&"
                                          + libCommon.QUERYSTRING.OUTSAMOUNT + "=" + salesOrder.valueToPay() + "&"
                                          + libCommon.QUERYSTRING.SLETOTAL + "=" + salesOrder.totalAmount() + "&"
                                          + libCommon.QUERYSTRING.CONSOLE + "=" + "1";


            document.getElementById("paymentPage").src = sUrlCalendar;


            //var iframeWin = $("#paymentPage").src.contentWindow;
            window.addEventListener("message", receiveMessage, false)
            $("#OpenModalPayment").trigger({ type: "click" });
        }

        var getURLSalesOrderAdministration = function (typeName, salesOrderId) {
            libCommon.ShowProgressMessage();

            $.getJSON(libCommon.API.CONFIGURATION, function (data) {
                if (data != null) {

                    var salesOrderPaymentURL = data.eventCalendarURL + "/SalesOrderAdministration.aspx";
                    callPaymentPage(typeName, salesOrderId, salesOrderPaymentURL);
                }
                else {
                    libTost.error('There is no an original payment to perform the refund');
                }

            })
            .done(function () {
                libCommon.HideProgressMessage();
            })
            .fail(function (jqxhr, textStatus, error) {
                libCommon.HideProgressMessage();
                libTost.error('There was an error when executing the query');
            });
        }


        //end  Created on july 21-2016 JAB

        var processRefund = function (salesOrderItem, event) {
            libCommon.ShowProgressMessage(libCommon.MESSSAGES.PROCESSREFUND);

            var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
            if (salesOrderItem.refundedLocal()) {
                debugger;
                if (salesOrderItem.optionSelected() == 'Yes') {
                    $.ajax({
                        url: common.API.ORDERMANAGEMENTITEMDELETE.replace("{0}", salesOrderItem.refundId()),
                        type: 'DELETE',
                        data: ko.toJSON(salesOrderItem),
                        contentType: "application/json;chartset=utf-8",
                        statusCode: {
                            200: function (result) {
                                salesOrderItem.refundedLocal(false);
                                salesOrderItem.amountRefunded(0);
                                //salesOrder.valueToPay(salesOrder.valueToPay() - salesOrderItem.amount());
                                //salesOrder.paidAmount(salesOrder.valueToPay() - salesOrderItem.amount());
                                //salesOrder.balanceToPay(salesOrder.valueToPay());

                                //handleRefund(salesOrderItem);

                                libTost.info(result.message);
                                libCommon.HideProgressMessage();
                            },
                            404: function () {
                                libCommon.HideProgressMessage();
                                salesOrderItem.optionSelected(salesOrderItem.options()[1]);

                                libTost.error('Resource Not found');
                            },
                            400: function () {
                                libCommon.HideProgressMessage();
                                salesOrderItem.optionSelected(salesOrderItem.options()[1]);

                                libTost.error('Error when processing the request');
                            },
                            500: function () {
                                libCommon.HideProgressMessage();
                                salesOrderItem.optionSelected(salesOrderItem.options()[1]);

                                libTost.error('Error when processing the request');
                            }
                        }
                    });
                } else {
                    libCommon.HideProgressMessage();
                }
            }
            else {
                if (salesOrderItem.optionSelected() == 'No') {
                    salesOrderItem.actionItem(libCommon.ENUMS.ACTIONITEMREFUND);

                    $.ajax({
                        url: common.API.ORDERMANAGEMENTITEM,
                        type: 'POST',
                        data: ko.toJSON(salesOrderItem),
                        contentType: "application/json;chartset=utf-8",
                        statusCode: {
                            200: function (result) {
                                salesOrderItem.refundedLocal(true);
                                salesOrderItem.refundId(result.idItem);
                                salesOrderItem.orderManagementItemId(result.idItem);
                                salesOrderItem.amountRefunded(salesOrderItem.amount());

                                //salesOrder.valueToPay(salesOrder.valueToPay() + salesOrderItem.amount());
                                //salesOrder.paidAmount(salesOrder.valueToPay() + salesOrderItem.amount());
                                //salesOrder.balanceToPay(salesOrder.valueToPay());

                                //handleRefund(salesOrderItem);

                                libTost.info(result.message);
                                libCommon.HideProgressMessage();
                            },
                            404: function () {
                                libCommon.HideProgressMessage();
                                salesOrderItem.optionSelected(salesOrderItem.options()[0]);
                                libTost.error('Resource Not found');
                            },
                            400: function () {
                                libCommon.HideProgressMessage();
                                salesOrderItem.optionSelected(salesOrderItem.options()[0]);

                                libTost.error('Error when processing the request');
                            },
                            500: function () {
                                libCommon.HideProgressMessage();
                                salesOrderItem.optionSelected(salesOrderItem.options()[0]);

                                libTost.error('Error when processing the request');
                            }
                        }
                    });
                } else {
                    libCommon.HideProgressMessage();
                }
            }
        }

        var processDeactivate = function (salesOrderItem, event) {
            libCommon.ShowProgressMessage(libCommon.MESSSAGES.PROCESSREFUND);

            var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
            if (salesOrderItem.deactivated()) {
                debugger;
                if (salesOrderItem.registrationDeactivatedSelected() == 'Yes') {
                    salesOrderItem.deactivated(false);
                    $.ajax({
                        url: common.API.ORDERMANAGEMENTITEMMODIFYDEACTIVATED,
                        type: 'PUT',
                        data: ko.toJSON(salesOrderItem),
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
                } else {
                    libCommon.HideProgressMessage();
                }
            }
            else {
                if (salesOrderItem.registrationDeactivatedSelected() == 'No') {
                    salesOrderItem.deactivated(true);
                    $.ajax({
                        url: common.API.ORDERMANAGEMENTITEMMODIFYDEACTIVATED,
                        type: 'PUT',
                        data: ko.toJSON(salesOrderItem),
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

                } else {
                    libCommon.HideProgressMessage();
                }
            }
        }



        var handleRefund = function (salesOrderItem) {
            var lineType = salesOrderItem.lineItemType();
            var refund = salesOrderItem.refundedLocal();
            if (lineType == libCommon.ENUMS.LINETYPECOURSEFEE) {
                if (refund == true) {
                    // TODO: Get the other records associated to the registration, and hide the Refund button
                    for (var i = 0; i < salesOrderItems().length; i++) {
                        var soItem = salesOrderItems()[i];
                        if (soItem.registrationId() == salesOrderItem.registrationId()) {
                            if (soItem.lineItemType() != libCommon.ENUMS.LINETYPECOURSEFEE) {
                                salesOrder.valueToPay(salesOrder.valueToPay() + salesOrderItems()[i].amount());
                                salesOrder.paidAmount(salesOrder.valueToPay() + salesOrderItems()[i].amount());
                                salesOrderItems()[i].refundVisible(false);
                            }
                        }
                    }
                }
                else {
                    // TODO: Get the other records associated to the registration, and show the Refund button
                    for (var i = 0; i < salesOrderItems().length; i++) {
                        var soItem = salesOrderItems()[i];
                        if (soItem.registrationId() == salesOrderItem.registrationId()) {
                            if (soItem.lineItemType() != libCommon.ENUMS.LINETYPECOURSEFEE) {
                                salesOrder.valueToPay(salesOrder.valueToPay() - salesOrderItems()[i].amount());
                                salesOrder.paidAmount(salesOrder.valueToPay() - salesOrderItems()[i].amount());
                                salesOrderItems()[i].refundVisible(true);
                            }
                        }
                    }
                }
            } else {
                if (refund == true) {
                    // TODO: Get the other records associated to the registration, and hide the Refund button
                    for (var i = 0; i < salesOrderItems().length; i++) {
                        var soItem = salesOrderItems()[i];
                        if (soItem.registrationId() == salesOrderItem.registrationId()) {
                            if (soItem.lineItemType() == libCommon.ENUMS.LINETYPECOURSEFEE) {
                                salesOrderItems()[i].refundVisible(false);
                            }
                        }
                    }
                }
                else {
                    // TODO: Get the other records associated to the registration, and show the Refund button
                    for (var i = 0; i < salesOrderItems().length; i++) {
                        var soItem = salesOrderItems()[i];
                        if (soItem.registrationId() == salesOrderItem.registrationId()) {
                            if (soItem.lineItemType() == libCommon.ENUMS.LINETYPECOURSEFEE) {
                                salesOrderItems()[i].refundVisible(true);
                            }
                        }
                    }
                }
            }
        }

        var initForm = function () {
            var process = libCommon.getProcess();
            var processMessage = libCommon.PAGETITLES[process];
            pageTitle(processMessage);

            if (process == libCommon.PROCESSES.CANCELREGISTRATION) {
                messagePayment("Total Refund");
                totalCreditDisabled(true);
            }
            else {
                messagePayment("Balance To Pay");
            }
        }
        var amountFocused = function () {
            var process = libCommon.getProcess();
            var processMessage = libCommon.PAGETITLES[process];

        }
        var modifyQuantity = function (salesOrderItem) {
            if (salesOrderItem.refundedLocal()) {
                libCommon.ShowProgressMessage();

                var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);

                $.ajax({
                    url: common.API.ORDERMANAGEMENTITEMMODIFYAMOUNT,
                    type: 'PUT',
                    data: ko.toJSON(salesOrderItem),
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
        var cancelActiveProcess = function (applyBindings) {

            var process = libCommon.getProcess();

            if (process == libCommon.PROCESSES.CANCELREGISTRATION) {
                libCommon.ShowProgressMessage(libCommon.MESSSAGES.SALESORDERPROCESSCANCEL);

                var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);

                $.ajax({
                    url: common.API.SALESALLORDERMANAGEMENTITEMS.replace("{0}", salesOrderId),
                    type: 'DELETE',
                    data: JSON.stringify({ salesOrderItem: salesOrderId }),
                    contentType: "application/json;chartset=utf-8",
                    statusCode: {
                        200: function (result) {
                            libCommon.HideProgressMessage();
                            getSalesOrder(applyBindings);
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
                getSalesOrder(applyBindings);

                //getSalesOrderItemsAssocitedToSalesOrder(applyBindings);
            }
        }

        var getSalesOrder = function (applyBindings) {

            libCommon.ShowProgressMessage();

            var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);

            $.getJSON(libCommon.API.SALESORDERBYID.replace("{0}", salesOrderId), function (data) {
                ko.mapping.fromJS(data, salesOrder);
                getSalesOrderItemsAssocitedToSalesOrder(applyBindings);
            })
            .done(function () {
                libCommon.HideProgressMessage();
            })
            .fail(function (jqxhr, textStatus, error) {
                libCommon.HideProgressMessage();
                libTost.error('There was an error when executing the query');
            });
        };

        /* Extending the mapping to include a property for all of the elements of the observable array */
        var mapping = {
            '': {
                create: function (options) {
                    return new createSalesOrderItem(options.data);
                }
            }
        }

        var getOrderTransactionItem = function (applyBindings) {

            libCommon.ShowProgressMessage();

            var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID)

            $.getJSON(common.API.ALLORDERTRANSACTIONBYORDER.replace("{0}", salesOrderId), function (data) {
                ko.mapping.fromJS(data, orderTransactionItemsByOrder);
                //startProcess(applyBindings);
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

        var createSalesOrderItem = function (salesOrderItem) {
            var self = this;
            var result = ko.mapping.fromJS(salesOrderItem);

            result.hasChanged = ko.observable(false);

            result.refundedLocal = ko.observable(false);

            result.refundId = ko.observable("");


            result.newAvailable = ko.observable(false);

            result.recordType = ko.computed(function () {

                if (result.reservationId()) {
                    return libCommon.ENUMS.RECORDTYPERESERVATION;
                }
                else if (result.eventOptionId()) {
                    return libCommon.ENUMS.RECORDTYPEEVENTOPTION;
                }
                else if (result.registrationId()) {
                    return libCommon.ENUMS.RECORDTYPEREGISTRATION;

                } else if (result.productId()) {
                    return libCommon.ENUMS.RECORDTYPEPRODUCT;
                }


            });

            result.refundVisible = ko.observable(true);

            //result.amountRefunded = ko.observable(0);

            result.amountRefunded.subscribe(function (newValue) {
                modifyQuantity(result);
            });


            // result.amountRefunded.focused = ko.observable();

            result.options = ko.observableArray(['No', 'Yes']);

            result.optionSelected = ko.observable();

            result.registrationDeactivated = ko.observableArray(['No', 'Yes']);

            result.registrationDeactivatedSelected = ko.observable();

            result.registrationDeactivatedVisible = ko.computed(function () {

                if (result.lineItemType() == libCommon.ENUMS.LINETYPECOURSEFEE) {
                    return true;
                }
                else {
                    return false;
                }
            });

            result.refundText = ko.computed(function () {

                if (result.refundedLocal()) {
                    return "Undo Refund";
                }
                else {
                    return "Refund";
                }
            });
            result.deactivatedFlag = ko.observable(result.deactivated());



            return result;
        };
        /* END extending the mapping*/


        // Public methods
        return {
            cancelActiveProcess: cancelActiveProcess,
            getOrderTransactionItem: getOrderTransactionItem,
            getSalesOrderItemsAssocitedToSalesOrder: getSalesOrderItemsAssocitedToSalesOrder,
            getSalesOrder: getSalesOrder,
            processSale: processSale,
            processRefund: processRefund,
            processDeactivate: processDeactivate,
            processResponse: processResponse,
            amountFocused: amountFocused,
            initForm: initForm
        };

    }(common, toastr, $, ko);

    if (document.getElementById("salesOrderListItemsPage") != null) {
        pavliksManagementConsoleApp.salesOrderItemsListViewModel.initForm();

        // Roger: I'm deferring this processes for when we are sure that any existing process for the order has been removed
        //pavliksManagementConsoleApp.salesOrderItemsListViewModel.getSalesOrder();
        //pavliksManagementConsoleApp.salesOrderItemsListViewModel.getSalesOrderItemsAssocitedToSalesOrder();
        //if (common.getProcess() != common.PROCESSES.WAITLIST) {
        pavliksManagementConsoleApp.salesOrderItemsListViewModel.cancelActiveProcess(applyBindings);
        //}


        // ko.applyBindings(pavliksManagementConsoleApp.salesOrderItemsListViewModel, document.getElementById("salesOrderListItemsPage"));
    }

    function applyBindings() {
        if (document.getElementById("salesOrderListItemsPage") != null) {

            if (!ko.dataFor(document.getElementById("salesOrderListItemsPage"))) {
                ko.applyBindings(pavliksManagementConsoleApp.salesOrderItemsListViewModel, document.getElementById("salesOrderListItemsPage"));
            }
        }
    }

});



