
window.common = (function () {
    var common = {};

    var PROGRESSMESSAGE = "Please wait until the operation is completed.....";

    common.API = {};
    common.QUERYSTRING = {};
    common.PAGES = {};
    common.STORAGEKEYS = {};
    common.PROCESSES = {};
    common.MESSSAGES = {};
    common.ENUMS = {};
    common.PAGETITLES = [];

    /*API URL Constants*/
    common.API.WAITLISTURL = "../api/event/{0}/Waitlist";
    common.API.WAITLIST = "../api/Waitlist";
    common.API.REGISTRATIONOPTIONSURL = "../api/event/{0}/Registration/RegistrationOptionsEvent";
    common.API.REGISTRATIONSBYEVENTURL = "../api/event/{0}/Registration/RegistrationByEvent";
    common.API.REGISTRATIONSAVE = "../api/registration";
    common.API.CONTACTSBYNAME = "../api/contact?name={0}";
    common.API.EVENTBYID = "../api/event/{0}";
    common.API.EVENTSALL = "../api/event";
    common.API.REGISTRATIONBYID = "../api/registration/{0}";
    common.API.REGISTRATIONUPDATE = "../api/registration";
    common.API.SALESORDER = "../api/salesorder";
    common.API.SALESORDERBYID = "../api/salesorder/{0}";
    common.API.SALESORDERITEMSBYORDEID = "../api/salesorder/{0}/salesorderitem/getSalesOrderItemBySalesOrder";
    common.API.REGISTRATIONALLINFO = "../api/registration/{0}/RegistrationAllInfo";
    common.API.EVENTSBYNAME = "../api/event?name={0}";
    common.API.ORDERMANAGEMENTITEM = "../api/OrderManagementItem";
    common.API.ORDERMANAGEMENTITEMMODIFYAMOUNT = "../api/OrderManagementItem/UpdateOrderManagementItem";
    common.API.ORDERMANAGEMENTITEMMODIFYDEACTIVATED = "../api/OrderManagementItem/UpdateDeactivatedFlag";
    common.API.ORDERMANAGEMENTITEMCONFIRM = "../api/OrderManagementItem/ConfirmedOrderManagementItemByOrderId";
    common.API.ORDERMANAGEMENTITEMDELETE = "../api/OrderManagementItem/?id={0}";
    common.API.SALESALLORDERMANAGEMENTITEMS = "../api/salesorder/{0}/OrderManagementItem";
    common.API.REGISTRATIONUPDATERESERVATIONSOPTION = "../api/registration/{0}/ModifySessionsOptions";
    common.API.NOTIFYREGISTRATION = "../api/registration/{0}/NotifyRegistration";
    common.API.SENDEMAILRECEIPT = "../api/email";
    common.API.GENERATEREPORTRECEIPT = "../api/email/{0}";
    common.API.SALESORDERITEM = "../api/salesorderItem";
    common.API.ORDERTRANSACTIONBYORDER = "../api/salesorder/{0}/OrderTransaction/GetFirsTransaction";
    common.API.ALLORDERTRANSACTIONBYORDER = "../api/salesorder/{0}/OrderTransaction/GetTransactionByOrder";
    common.API.ORDERTRANSACTION = "../api/OrderTransaction";
    common.API.CONFIGURATION = "../api/configuration";

    /*Querystring Constants*/
    common.QUERYSTRING.EVENTID = "eventId";
    common.QUERYSTRING.SALESORDERID = "salesOrderId";
    common.QUERYSTRING.REGISTRATIONID = "registrationId";
    common.QUERYSTRING.TYPENAME = "typename";
    common.QUERYSTRING.ID = "Id";
    common.QUERYSTRING.TOTCREDIT = "totCredit";
    common.QUERYSTRING.OUTSAMOUNT = "outsAmnt";
    common.QUERYSTRING.SLETOTAL = "sleTotal";
    common.QUERYSTRING.CONSOLE = "Console";



    /*Pages Constants*/
    common.PAGES.INDEX = "/Pages/Index";
    common.PAGES.WAITLISTS = "/Pages/Waitlist";
    common.PAGES.SALESORDERITEMLIST = "/Pages/SalesOrderItemsList";
    common.PAGES.MODIFYSESSIONS = "/Pages/ModifySessions";
    common.PAGES.SWAPREGISTRATION = "/Pages/SwapRegistration";
    common.PAGES.TRANSFERREGISTRATION = "/Pages/TransferRegistration";
    common.PAGES.SUCESSFULTRANSACTION = "/Pages/SuccessfulTransaction";
    common.PAGES.CANCELLEDTRANSACTION = "/Pages/CancelledTransaction";
    common.PAGES.FAILEDTRANSACTION = "/Pages/FailedTransaction";
    common.PAGES.REGISTRATIONOPTIONS = "/Pages/RegistrationOptions";

    /*Storage Keys*/
    common.STORAGEKEYS.EVENTID = "eventId";
    common.STORAGEKEYS.PROCESS = "process";
    common.STORAGEKEYS.WAITLIST = "waitlist";
    common.STORAGEKEYS.SALESORDERID = "salesOrderId";
    common.STORAGEKEYS.CHEQUENUMBER = "chequeNumber";
    common.STORAGEKEYS.ISCHEQUE = "isCheque";


    common.PROCESSES.SWAP = "SWAPREGISTRATION";
    common.PROCESSES.TRANSFERREGISTRATION = "TRANSFERREGISTRATION";
    common.PROCESSES.PROCESSPAYMENT = "PROCESSPAYMENT";
    common.PROCESSES.CANCELREGISTRATION = "MANAGEORDER";
    common.PROCESSES.MODIFYSESSIONOPTIONS = "MODIFYSESSIONOPTION";
    common.PROCESSES.MODIFYPRODUCTS = "MODIFYPRODUCTS";
    common.PROCESSES.WAITLIST = "WAITLISTS";

    /*Message Constants*/
    common.MESSSAGES.WAITLISTGENERATESALESORDER = "The registration records is being created, this may take some seconds";
    common.MESSSAGES.SWAPPROCESSING = "Swapping registrant for the registration";
    common.MESSSAGES.SALESORDERPROCESSCANCEL = "Cancelling any pending process for the sales order";
    common.MESSSAGES.PROCESSREFUND = "Refunding Item";
    common.MESSSAGES.RESERVATIONEVENTOPTIONSGENERATE = "The reservations / options are being created, this may take some seconds";
    common.MESSSAGES.GENERATELINK = "The link are being created, this may take some seconds";
    common.MESSSAGES.SENDEMAIL = "Sending Email";
    common.MESSSAGES.NOTIFYREGISTRATION = "The registration is being notified.";


    common.PAGETITLES[common.PROCESSES.SWAP] = "Swap Registration";
    common.PAGETITLES[common.PROCESSES.TRANSFERREGISTRATION] = "Transfer Registration";
    common.PAGETITLES[common.PROCESSES.PROCESSPAYMENT] = "Manage Order";
    common.PAGETITLES[common.PROCESSES.CANCELREGISTRATION] = "Cancel Registration";
    common.PAGETITLES[common.PROCESSES.MODIFYPRODUCTS] = "Modify Products";

    common.ENUMS.ACTIONITEMADD = "Add";
    common.ENUMS.ACTIONITEMUPDATE = "Update";
    common.ENUMS.ACTIONITEMREFUND = "Refund";

    common.ENUMS.STATEORDERACTIVE = "Active";
    common.ENUMS.STATEORDERCANCELLED = "Canceled";
    common.ENUMS.STATEORDERFULFILLED = "Fulfilled";
    common.ENUMS.STATUSORDERPENDING = "Pending";
    common.ENUMS.STATUSORDERMANAGECONSOLE = "ManageConsole";
    common.ENUMS.STATUSORDERCOMPLETE = "Complete";
    common.ENUMS.STATUSORDERNOMONEY = "No Money";

    common.ENUMS.LINETYPECOURSEFEE = "CourseFee";
    common.ENUMS.LINETYPEAMCARE = "AmCare";
    common.ENUMS.LINETYPEPMCARE = "PmCare";
    common.ENUMS.LINETYPESUPERVISEDLUNCH = "SupervisedLunch";

    common.ENUMS.RECORDTYPEREGISTRATION = "Registration";
    common.ENUMS.RECORDTYPERESERVATION = "Reservation";
    common.ENUMS.RECORDTYPEEVENTOPTION = "EventOption";
    common.ENUMS.RECORDTYPEPRODUCT = "Product";

    common.ENUMS.ATTENDANCETYPEINPERSON = "InPerson";
    common.ENUMS.ATTENDANCETYPEWEBCAST = "WebCast";

    common.getFragment = function getFragment() {
        if (window.location.hash.indexOf("#") === 0) {
            return parseQueryString(window.location.hash.substr(1));
        } else {
            return {};
        }
    };

    common.ShowProgressMessage = function (message) {
        $(function () {
            $("#dialog-message").dialog({
                modal: true,
                buttons: {
                }
            });

            if (message) {
                $("#paragraph-dialog-message").html(message);
            }

            $(".ui-dialog-titlebar-close").css("display", "none"); // hide the close button. User can still hit ESC
        });
    }
    common.ShowPopPup = function (title, message) {
        $(function () {
            $("#dialog-message").dialog({

                modal: true,
                buttons: {
                }
            });

            if (message) {
                $("#paragraph-dialog-message").html(message);
            }
            if (title) {
                $("span.ui-dialog-title").text(title);
            }
            $(".ui-dialog-titlebar-close").show(); // hide the close button. User can still hit ESC
        });
    }

    common.HideProgressMessage = function () {
        $("#paragraph-dialog-message").html(PROGRESSMESSAGE);
        $("#dialog-message").dialog("close");
    }

    common.GetBaseURL = function () {
        var applicationPath = "";
        if (window.location.pathname != "" && !window.location.pathname.StartsWithCustom("/Pages")) {
            var entirePathName = window.location.pathname;
            applicationPath = entirePathName.substr(0, entirePathName.indexOf("/", 1));
        }

        return location.protocol + "//" + window.location.host + applicationPath;
    }

    String.prototype.StartsWithCustom = function (str) {
        if (this.indexOf(str) === 0) {
            return true;
        } else {
            return false;
        }
    };

    common.Init = function () {
        if (common.GetBaseURL() == location.href || (common.GetBaseURL() + "/") == location.href) {
            location.href = location.href + "/Pages/Index";
        }
    }

    common.getQueryStringParameterByName = function (name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }

    common.setProcess = function (process) {
        localStorage.setItem(common.STORAGEKEYS.PROCESS, process);
    }

    common.getProcess = function () {
        if (localStorage.getItem(common.STORAGEKEYS.PROCESS) != null && localStorage.getItem(common.STORAGEKEYS.PROCESS) != "") {
            return localStorage.getItem(common.STORAGEKEYS.PROCESS);
        }
        else {
            return null;
        }
    }

    common.setValueStorage = function (key, value) {
        localStorage.setItem(key, value);
    }

    common.getValueStorage = function (key) {
        if (localStorage.getItem(key) != null && localStorage.getItem(key) != "" && localStorage.getItem(key) != "null") {
            return localStorage.getItem(key);
        }
        else {
            return null;
        }
    }

    function parseQueryString(queryString) {
        var data = {},
            pairs, pair, separatorIndex, escapedKey, escapedValue, key, value;

        if (queryString === null) {
            return data;
        }

        pairs = queryString.split("&");

        for (var i = 0; i < pairs.length; i++) {
            pair = pairs[i];
            separatorIndex = pair.indexOf("=");

            if (separatorIndex === -1) {
                escapedKey = pair;
                escapedValue = null;
            } else {
                escapedKey = pair.substr(0, separatorIndex);
                escapedValue = pair.substr(separatorIndex + 1);
            }

            key = decodeURIComponent(escapedKey);
            value = decodeURIComponent(escapedValue);

            data[key] = value;
        }

        return data;
    }

    return common;
})();

common.Init();