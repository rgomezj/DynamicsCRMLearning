pavliksManagementConsoleApp = {};

$(function () {

    pavliksManagementConsoleApp.registrationOptionsViewModel = function (libCommon, libTost, $, ko) {

        // Private model
        registrationOptions = ko.mapping.fromJS({ registrationLevels: ko.observableArray(), classes: ko.observableArray(), event: ko.observable()});

        registrationLevelSelected = ko.observable();

        var waitlist = libCommon.getValueStorage(libCommon.STORAGEKEYS.WAITLIST);
        if (libCommon.getValueStorage(libCommon.STORAGEKEYS.WAITLIST) == null) {
            libTost.error("Please select a waitlist to use this option");
            return;
        }

        waitlistInfo = ko.mapping.fromJSON(JSON.parse(waitlist))

        // Private method to get all registrationOptions
        var getRegistrationOptions = function () {

            libCommon.ShowProgressMessage();

            var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID);

            $.getJSON(common.API.REGISTRATIONOPTIONSURL.replace("{0}", eventId), function (data) {

                ko.mapping.fromJS(data, registrationOptions);
                // registrationLevels = registrationOptions.registrationLevels;
            })
            .done(function () {
                libCommon.HideProgressMessage();
            })
            .fail(function (jqxhr, textStatus, error) {
                libCommon.HideProgressMessage();
                libTost.error('There was an error when executing the query');
            });
        };

        var processRegistration = function () {

            libCommon.ShowProgressMessage(libCommon.MESSSAGES.WAITLISTGENERATESALESORDER);

            var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID);
            var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);

            var registrationLevel = ko.utils.arrayFirst(registrationOptions.registrationLevels(), function (currentEntry) {
                return currentEntry.id() == registrationLevelSelected();
            });
            
            var registrationOptionsPost = ko.mapping.fromJS({ attendanceType: selectedRegistrationType(), waitlist: ko.observable(), createSalesOrder: ko.observable(false), contact: ko.observable(), salesOrder: ko.observable(), contact: ko.observable(), registrationLevel: registrationLevel, classes: ko.observableArray(), event: ko.observable()});
            
            registrationOptionsPost.salesOrder = waitlistInfo.salesOrder;
            registrationOptionsPost.event = waitlistInfo.eventCRM;
            registrationOptionsPost.contact = waitlistInfo.contact;
            
            registrationOptionsPost.classes = registrationOptions.classes;
            registrationOptionsPost.contact = waitlistInfo.contact;
            registrationOptionsPost.createSalesOrder(true);
            registrationOptionsPost.waitlist = waitlistInfo;
            
            $.ajax({
                url: libCommon.API.REGISTRATIONSAVE,
                type: 'POST',
                data: ko.toJSON(registrationOptionsPost),
                contentType: "application/json;chartset=utf-8",
                statusCode: {
                    200: function (result) {
                        
                        libCommon.HideProgressMessage();
                        libCommon.setProcess(libCommon.PROCESSES.WAITLIST);
                        libCommon.setValueStorage(libCommon.STORAGEKEYS.SALESORDERID, result.salesOrderId);
                        window.location.href = libCommon.GetBaseURL() + "/" + libCommon.PAGES.SALESORDERITEMLIST + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + result.salesOrderId;
                        

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
        };

        var cancelActiveProcess = function () {

            if (!validForm()) {
                return;
            }

            libCommon.ShowProgressMessage(libCommon.MESSSAGES.SALESORDERPROCESSCANCEL);

            var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);

            $.ajax({
                url: libCommon.API.SALESALLORDERMANAGEMENTITEMS.replace("{0}", salesOrderId),
                type: 'DELETE',
                data: JSON.stringify({ salesOrderItem: waitlistInfo.salesOrder.id() }),
                contentType: "application/json;chartset=utf-8",
                statusCode: {
                    200: function (result) {
                        libCommon.HideProgressMessage();
                        processRegistration();
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


        var selectRegistrationLevel = function (registrationLevel) {
            registrationLevelSelected = registrationLevel;
        }
        this.selectedClasses = ko.observableArray([]);
        this.selectedRegistrationType = ko.observable();

        
        this.anyHybrid = ko.computed(function () {
            if (registrationOptions.event() != null && registrationOptions.event().dm_hybrid()) {
                return true;
            }
            else {
                return false;
            }
        });
        this.completeName = ko.computed(function () {

            if (registrationOptions.event() != null && registrationOptions.event().dm_hybrid()) {
                return true;
            }
            else {
                return false;
            }
        });
        this.registrationType = ko.observableArray([libCommon.ENUMS.ATTENDANCETYPEINPERSON, libCommon.ENUMS.ATTENDANCETYPEWEBCAST]);

        this.Process = function (event) {
            if (selectedClasses() != null) {
                alert(selectedClasses());
            }
        }

        this.getRegistrations = function () {
            return _self.registrationOptions;
        }

        var validForm = function () {
            var message = "";

            if (typeof (registrationLevelSelected()) == "undefined") {
                message += "Registration level is required <br/>";
                libTost.error(message);
                return false;
            }
            else {
                return true;
            }
        }

        // Public methods
        return {
            getRegistrationOptions: getRegistrationOptions,
            selectRegistrationLevel: selectRegistrationLevel,
            processRegistration: processRegistration,
            getRegistrations: getRegistrations,
            cancelActiveProcess: cancelActiveProcess
        };

    }(common, toastr, $, ko);

    if (document.getElementById("registrationOptions") != null) {
        pavliksManagementConsoleApp.registrationOptionsViewModel.getRegistrationOptions();
        ko.applyBindings(pavliksManagementConsoleApp.registrationOptionsViewModel, document.getElementById("registrationOptions"));
    }
});



