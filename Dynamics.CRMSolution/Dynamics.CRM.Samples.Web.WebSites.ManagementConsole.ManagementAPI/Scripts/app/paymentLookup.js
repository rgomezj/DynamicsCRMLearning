pavliksManagementConsoleApp = {};

$(function () {
    
    pavliksManagementConsoleApp.paymentViewModel = function (libCommon, libTost, $, ko) {
        
        chequeNumber = ko.observable("");
        willSpecifyLater = ko.observable(false);

        var callBackPayment;

        var confirmPayment = function () {
            callBackPayment(chequeNumber(), willSpecifyLater());
            $("[data-dismiss=modal]").trigger({ type: "click" });
        }
        var bindingsApplied = false;
        var bindData = function (name, _callbackPayment) {
            callBackPayment = _callbackPayment;
            if (document.getElementById("paymentLookup") != null && !bindingsApplied) {
                ko.applyBindings(pavliksManagementConsoleApp.paymentViewModel, document.getElementById("paymentLookup"));
                bindingsApplied = true;
            }
        }


        // Public methods
        return {
            confirmPayment: confirmPayment,
            bindData: bindData
        };

    }(common, toastr, $, ko);
});



