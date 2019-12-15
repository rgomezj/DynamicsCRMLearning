pavliksManagementConsoleApp = {};

$(function () {

    pavliksManagementConsoleApp.eventViewModel = function (libCommon, libTost, $, ko) {

        // Private model
        events = ko.mapping.fromJS([]);
        nameFilter = ko.observable("");

        loading = ko.observable(false);

        var callBackEvent;

        var selectEventLookup = function (eventSelected) {
            callBackEvent(eventSelected);
            $("[data-dismiss=modal]").trigger({ type: "click" });
        }

        var bindData = function (name, _callbackEvent) {
            callBackEvent = _callbackEvent;
            getEventsFiltered(name);
            if (document.getElementById("eventsLookup") != null) {
                ko.applyBindings(pavliksManagementConsoleApp.eventViewModel, document.getElementById("eventsLookup"));
            }
        }

        var searchEvents = function () {
            getEventsFiltered(nameFilter());
        }

        var getEventsFiltered = function (name) {

            loading(true);
            $.getJSON(common.API.EVENTSBYNAME.replace("{0}", nameFilter()), function (data) {
                ko.mapping.fromJS(data, events);
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

        // Private method to get all waitlists

        var getAllEvents = function () {

            libCommon.ShowProgressMessage();

            loading(true);
            $.getJSON(common.API.EVENTSALL, function (data) {

                ko.mapping.fromJS(data, events);
                loading(false);
            })
            .done(function () {
                common.HideProgressMessage();
                loading(false);
            })
            .fail(function (jqxhr, textStatus, error) {
                common.HideProgressMessage();
                toastr.error('There was an error when executing the query');
                loading(false);
            });
        };

        var selectEvent = function (event) {
            localStorage.setItem(libCommon.STORAGEKEYS.EVENTID, event.id());
            window.location.href = libCommon.GetBaseURL() + "/" + libCommon.PAGES.INDEX + "?" + libCommon.QUERYSTRING.EVENTID + "=" + event.id();
        }

        // Public methods
        return {
            getEventsFiltered: getEventsFiltered,
            bindData: bindData,
            selectEventLookup: selectEventLookup,
            searchEvents: searchEvents,
            getAllEvents: getAllEvents,
            selectEvent: selectEvent
        };

    }(common, toastr, $, ko);

    if (document.getElementById("events") != null)
    {
        pavliksManagementConsoleApp.eventViewModel.getEventsFiltered();
        ko.applyBindings(pavliksManagementConsoleApp.eventViewModel, document.getElementById("events"));
    }
});


