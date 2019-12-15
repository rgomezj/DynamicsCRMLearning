pavliksManagementConsoleApp = {};

$(function () {

    pavliksManagementConsoleApp.registrationTransferViewModel = function (libCommon, libTost, $, ko) {

        // Private model
        registrationOptions = ko.mapping.fromJS({ registrationLevels: ko.observableArray(), classes: ko.observableArray(), event: ko.observable(), eventOptions: ko.observableArray() });

        currentRegistrationInfo = ko.mapping.fromJS({ registration: ko.observable(), classes: ko.observableArray(), event: ko.observable(), eventOptions: ko.observableArray(), salesOrder: ko.observable() });

        registrationLevelSelected = ko.observable();

        eventSelected = ko.mapping.fromJS({ name: ko.observable(""), id: ko.observable(""), email: ko.observable("") });


        var getRegistrationOptions = function (id, targetObject, api) {

            libCommon.ShowProgressMessage();

            $.getJSON(api.replace("{0}", id), function (data) {
                
                ko.mapping.fromJS(data, targetObject);
            })
            .done(function () {
                libCommon.HideProgressMessage();
            })
            .fail(function (jqxhr, textStatus, error) {
                libCommon.HideProgressMessage();
                libTost.error('There was an error when executing the query');
            });
        };

        var transferRegistration = function () {
            
            if (!validForm()) {
                return;
            }

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

                        libCommon.ShowProgressMessage(libCommon.MESSSAGES.WAITLISTGENERATESALESORDER);

                        var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID);


                        var registrationOptionsPost = ko.mapping.fromJS({ attendanceType: selectedRegistrationType(), contact: ko.observable(), registrationLevel: ko.observable({ id: ko.observable(registrationLevelSelected) }), classes: ko.observableArray(), event: ko.observable(), eventOptions: ko.observableArray(), salesOrder: ko.observable() });

                        debugger;
                        registrationOptionsPost.classes = registrationOptions.classes;
                        registrationOptionsPost.eventOptions = selectedEventOptions;
                        registrationOptionsPost.event = eventSelected;
                        registrationOptionsPost.contact(currentRegistrationInfo.registration().contact);
                        registrationOptionsPost.salesOrder(currentRegistrationInfo.registration().salesOrder);
                        registrationOptionsPost.createOrder = false;

                        $.ajax({
                            url: common.API.REGISTRATIONSAVE,
                            type: 'POST',
                            data: ko.toJSON(registrationOptionsPost),
                            contentType: "application/json;chartset=utf-8",
                            statusCode: {
                                200: function (result) {
                                    libCommon.HideProgressMessage();
                                    libTost.info('Saved');
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

        var selectRegistrationLevel = function (registrationLevel) {
            registrationLevelSelected = registrationLevel;
        }

        this.selectedClasses = ko.observableArray([]);
        this.selectedRegistrationType = ko.observable();

        this.selectedEventOptions = ko.observableArray([]);

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

        var openLookupEvents = function () {
            pavliksManagementConsoleApp.eventViewModel.bindData("", assignSelectedEvent);
        }

        var assignSelectedEvent = function (eventFromLookup) {
            if (eventFromLookup.id() == currentRegistrationInfo.event().id()) {
                libTost.error("The target event is the same as the current event for the registration");
            }
            else {
                
                eventSelected.id(eventFromLookup.id())
                eventSelected.name(eventFromLookup.name());
                getRegistrationOptions(eventFromLookup.id(), registrationOptions, libCommon.API.REGISTRATIONOPTIONSURL);
            }
        }

     


        var validForm = function () {
            var message = "";
            if (eventSelected.id() == "") {
                message += "Please select the new event for the registration <br/>";
                libTost.error(message);
                return false;
            }
            else {
                if (typeof (registrationLevelSelected()) == "undefined") {
                    message += "Registration level is required <br/>";
                    libTost.error(message);
                    return false;
                }
                else {
                    return true;
                }
            }
        }

        // Public methods
        return {
            getRegistrationOptions: getRegistrationOptions,
            selectRegistrationLevel: selectRegistrationLevel,
            transferRegistration: transferRegistration,
            currentRegistrationInfo: currentRegistrationInfo,
            openLookupEvents: openLookupEvents
        };

    }(common, toastr, $, ko);

    if (document.getElementById("transferRegistration") != null) {
        var registrationId = common.getQueryStringParameterByName(common.QUERYSTRING.REGISTRATIONID);

        pavliksManagementConsoleApp.registrationTransferViewModel.getRegistrationOptions(registrationId, pavliksManagementConsoleApp.registrationTransferViewModel.currentRegistrationInfo, common.API.REGISTRATIONALLINFO);
        ko.applyBindings(pavliksManagementConsoleApp.registrationTransferViewModel, document.getElementById("transferRegistration"));
    }
});



