pavliksManagementConsoleApp = {};

$(function () {

    pavliksManagementConsoleApp.waitListViewModel = function (libCommon, libTost, $, ko) {

        // Private model
        waitlists = ko.mapping.fromJS([]);

        eventInfo = ko.mapping.fromJS({ dm_registrationlimit: ko.observable(0), spacesAvailable: ko.observable(0) });

        // Private method to get all waitlists
        var getAllWaitlist = function () {

            libCommon.ShowProgressMessage();

            var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID)

            $.getJSON(libCommon.API.WAITLISTURL.replace("{0}", eventId), function (data) {
                ko.mapping.fromJS(data, waitlists);
            })
            .done(function () {
                libCommon.HideProgressMessage();
            })
            .fail(function (jqxhr, textStatus, error) {
                libCommon.HideProgressMessage();
                libTost.error('There was an error when executing the query');
            });
        };

        var getEventInfo = function () {

            libCommon.ShowProgressMessage();

            var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID)

            $.getJSON(libCommon.API.EVENTBYID.replace("{0}", eventId), function (data) {
                
                ko.mapping.fromJS(data, eventInfo);
            })
            .done(function () {
                libCommon.HideProgressMessage();
            })
            .fail(function (jqxhr, textStatus, error) {
                libCommon.HideProgressMessage();
                libTost.error('There was an error when executing the query');
            });
        };

        var converToRegistration = function (waitlist) {
            debugger;
            var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID);
            libCommon.setValueStorage(libCommon.STORAGEKEYS.WAITLIST, JSON.stringify(ko.toJSON(waitlist)));
            if (waitlist.salesOrder && typeof (waitlist.salesOrder) == "object") {
                var salesOrderId = waitlist.salesOrder.id();
                window.location.href = libCommon.GetBaseURL() + "/" + libCommon.PAGES.REGISTRATIONOPTIONS + "?" + libCommon.QUERYSTRING.EVENTID + "=" + eventId + "&" + "&" + libCommon.QUERYSTRING.SALESORDERID + "=" + salesOrderId;
            }
            else {
                libTost.error("There is no sales order associated to this Waitlist");
            }
        }
        var openLookupWaitlist = function (waitlist) {
            pavliksManagementConsoleApp.waitlistlookupViewModel.bindData("", waitlist);
        }
        var process = function (waitlist) {
            debugger;

            libCommon.ShowProgressMessage(libCommon.MESSSAGES.GENERATELINK);
            var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID);

            $.ajax({
                url: libCommon.API.WAITLIST.replace("{0}", eventId),
                type: 'POST',
                data: ko.toJSON(waitlist),
                contentType: "application/json;chartset=utf-8",
                statusCode: {
                    200: function (result) {
                        debugger;
                        waitlist.registrationLink = result.registrationLink;
                        libCommon.HideProgressMessage();
                        
                        $("#OpenModalButton").trigger({ type: "click" });

                        openLookupWaitlist(waitlist);
                        //libCommon.ShowPopPup("Copy Link", waitlist.RegistrationLink);

                        // libTost.info('Processed');
                       // window.location = libCommon.GetBaseURL() + "/" + libCommon.PAGES.SALESORDERITEMLIST + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + salesOrderId;
                    },
                    404: function () {

                        libCommon.HideProgressMessage();
                        libTost.error('Resource Not found');
                    },
                    500: function () {

                        libCommon.HideProgressMessage();
                        libTost.error('Error when processing the request');
                    }
                }
            });


        };

        var sendEmail = function (waitlist) {
            debugger;

            var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID);
            if (waitlist.registrationLink != null) {
                libCommon.ShowProgressMessage(libCommon.MESSSAGES.SENDEMAIL);
                $.ajax({
                    url: libCommon.API.WAITLISTURL.replace("{0}", eventId),
                    type: 'PUT',
                    data: ko.toJSON(waitlist),
                    contentType: "application/json;chartset=utf-8",
                    statusCode: {
                        200: function (result) {
                            debugger;
                            libCommon.HideProgressMessage();
                            // libTost.info('Processed');
                            // window.location = libCommon.GetBaseURL() + "/" + libCommon.PAGES.SALESORDERITEMLIST + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + salesOrderId;
                        },
                        404: function () {

                            libCommon.HideProgressMessage();
                            libTost.error('Resource Not found');
                        },
                        500: function () {

                            libCommon.HideProgressMessage();
                            libTost.error('Error when processing the request');
                        }
                    }
                });
            } else {
                libCommon.ShowPopPup("Information","Link has not been generated for the waitlist");

            }


        };



        this.anyWaitlist = ko.computed(function () {
            if (waitlists().length > 0) {
                return true;
            }
            else {
                return false;
            }
        });

        var cancelActiveProcess = function () {
            
            // var process = libCommon.getProcess();
            var salesOrderId = libCommon.getValueStorage(libCommon.STORAGEKEYS.SALESORDERID);
            debugger;
            if (salesOrderId != null) {
                //if (process == libCommon.PROCESSES.CANCELREGISTRATION || process == libCommon.PROCESSES.PROCESSPAYMENT || process == libCommon.PROCESSES.MODIFYPRODUCTS) {
                libCommon.ShowProgressMessage(libCommon.MESSSAGES.SALESORDERPROCESSCANCEL);

                //var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);
                
                $.ajax({

                    url: common.API.SALESALLORDERMANAGEMENTITEMS.replace("{0}", salesOrderId),
                    type: 'DELETE',
                    data: JSON.stringify({ salesOrderItem: salesOrderId }),
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
                if (!ko.dataFor(document.getElementById("waitlist"))) {
                    pavliksManagementConsoleApp.waitListViewModel.getAllWaitlist();
                }
            }

        }

        // Public methods
        return {
            getAllWaitlist: getAllWaitlist,
            openLookupWaitlist: openLookupWaitlist,
            converToRegistration: converToRegistration,
            process: process,
            sendEmail: sendEmail,
            getEventInfo: getEventInfo
        };

    }(common, toastr, $, ko);

    if (document.getElementById("waitlist") != null) {
        //pavliksManagementConsoleApp.waitListViewModel.cancelActiveProcess();
        pavliksManagementConsoleApp.waitListViewModel.getEventInfo();
        pavliksManagementConsoleApp.waitListViewModel.getAllWaitlist();
        ko.applyBindings(pavliksManagementConsoleApp.waitListViewModel, document.getElementById("waitlist"));
    }
});



