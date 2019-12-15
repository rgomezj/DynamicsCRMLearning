pavliksManagementConsoleApp = {};

$(function () {

    pavliksManagementConsoleApp.modifySessionsViewModel = function (libCommon, libTost, $, ko) {

        var _self = this;

        // Private model
        _self.registrationOptions = ko.mapping.fromJS({ registrationLevels: ko.observableArray(), sessions: ko.observableArray(), event: ko.observable(), eventOptions: ko.observableArray() });
        registranOptions = ko.mapping.fromJS({ registrationLevels: ko.observableArray(), sessions: ko.observableArray(), event: ko.observable(), eventOptions: ko.observableArray() });

        registrationLevelSelected = ko.observable();


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

        var getRegistranOptions = function () {

            libCommon.ShowProgressMessage();

            var registrationId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.REGISTRATIONID);

            $.getJSON(common.API.REGISTRATIONALLINFO.replace("{0}", registrationId), function (data) {

                ko.mapping.fromJS(data, registranOptions);
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

        var selectRegistrationLevel = function (registrationLevel) {
            registrationLevelSelected = registrationLevel;
        };
        this.sessionToAdd = ko.observableArray();
        this.sessionToRemove = ko.observableArray();
        this.selectedRegistrationType = ko.observable();

        this.eventoOptionsToAdd = ko.observableArray();
        this.eventoOptionsToRemove = ko.observableArray();
        var processAddSessions = function () {
            
            for (var i = 0; i < sessionToAdd().length; i++) {
              

                registranOptions.sessions.push((sessionToAdd()[i]));
                registrationOptions.sessions.remove((sessionToAdd()[i]));
            }

            sessionToAdd.removeAll();

        };
        var processRemoveSessions = function () {
            
            for (var i = 0; i < sessionToRemove().length; i++) {

                registrationOptions.sessions.push((sessionToRemove()[i]));
                registranOptions.sessions.remove(sessionToRemove()[i]);
            }
            sessionToRemove.removeAll();
        };
        var processRemoveEventOptions = function () {
            for (var i = 0; i < eventoOptionsToRemove().length; i++) {
                registrationOptions.eventOptions.push((eventoOptionsToRemove()[i]));
                registranOptions.eventOptions.remove(eventoOptionsToRemove()[i]);
            }
            eventoOptionsToRemove.removeAll();
        };
        var processAddEventOptions = function () {
           
            for (var i = 0; i < eventoOptionsToAdd().length; i++) {
               
                    registranOptions.eventOptions.push((eventoOptionsToAdd()[i]));
                    registrationOptions.eventOptions.remove((eventoOptionsToAdd()[i]));
                
                
            }
            eventoOptionsToAdd.removeAll();
        };

        var process = function () {
            
            
            libCommon.ShowProgressMessage(libCommon.MESSSAGES.RESERVATIONEVENTOPTIONSGENERATE);
                var eventId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.EVENTID);
               
                var registrationId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.REGISTRATIONID);
                var salesOrderId = libCommon.getQueryStringParameterByName(libCommon.QUERYSTRING.SALESORDERID);

                var registrationOptionsPost = ko.mapping.fromJS({ registrationId: ko.observable(registrationId), sessions: ko.observableArray(), event: ko.observable({ id: eventId }), eventOptions: ko.observableArray(), salesOrder: ko.observable({ id: salesOrderId}) });
                registrationOptionsPost.sessions = registranOptions.sessions;
                registrationOptionsPost.eventOptions = registranOptions.eventOptions;
                

                $.ajax({
                    url: libCommon.API.REGISTRATIONUPDATERESERVATIONSOPTION.replace("{0}", registrationId),
                    type: 'PUT',
                    data: ko.toJSON(registrationOptionsPost),
                    contentType: "application/json;chartset=utf-8",
                    statusCode: {
                        200: function (result) {
                            
                            libCommon.HideProgressMessage();
                           // libTost.info('Processed');
                            window.location = libCommon.GetBaseURL() + "/" + libCommon.PAGES.SALESORDERITEMLIST + "?" + libCommon.QUERYSTRING.SALESORDERID + "=" + salesOrderId ;
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
        this.anyHybrid = ko.computed(function () {
            if (registrationOptions.event() != null && registrationOptions.event().oha_hybrid()) {
                return true;
            }
            else {
                return false;
            }
        });
        this.completeName = ko.computed(function () {
            
            if (registrationOptions.event() != null && registrationOptions.event().oha_hybrid()) {
                return true;
            }
            else {
                return false;
            }
        });
        this.registrationType = ko.observableArray(['In Person', 'Web Cast']);

        this.Process = function (event) {
            if (sessionToAdd() != null) {
                alert(sessionToAdd());
            }

        };
        // Public methods
        return {
            getRegistrationOptions: getRegistrationOptions,
            selectRegistrationLevel: selectRegistrationLevel,
            getRegistranOptions:getRegistranOptions,
           // processRegistration: processRegistration,
            sessionToRemove: sessionToRemove,
            eventoOptionsToRemove: eventoOptionsToRemove,
            processAddSessions: processAddSessions,
            processRemoveSessions: processRemoveSessions,
            processRemoveEventOptions: processRemoveEventOptions,
            processAddEventOptions: processAddEventOptions,
            process:process

        };

    }(common, toastr, $, ko);

    if (document.getElementById("modifySessionsPage") != null) {
        pavliksManagementConsoleApp.modifySessionsViewModel.getRegistrationOptions();
        pavliksManagementConsoleApp.modifySessionsViewModel.getRegistranOptions();
        ko.applyBindings(pavliksManagementConsoleApp.modifySessionsViewModel, document.getElementById("modifySessionsPage"));
    }
});



