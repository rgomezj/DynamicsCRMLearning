
ko.bindingHandlers.textualDate = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var valueUnwrapped = ko.utils.unwrapObservable(valueAccessor());
        // var textContent = moment(valueUnwrapped).format('MMMM Do YYYY, h:mm:ss a');
        var textContent = moment(valueUnwrapped).format('MM/DD/YYYY h:mm:ss a');
        ko.bindingHandlers.text.update(element, function () { return textContent; });
    }
};

ko.bindingHandlers.textualDateTime = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var valueUnwrapped = ko.utils.unwrapObservable(valueAccessor());
        var textContent = moment(valueUnwrapped).format('MM/DD/YYYY h:mm a');
        ko.bindingHandlers.text.update(element, function () { return textContent; });
    }
};

ko.bindingHandlers.currencyField = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var valueUnwrapped = ko.utils.unwrapObservable(valueAccessor());
        var myNumeral = numeral(valueUnwrapped);
        var textContent = myNumeral.format('$0.00');
        ko.bindingHandlers.text.update(element, function () { return textContent; });
    }
};