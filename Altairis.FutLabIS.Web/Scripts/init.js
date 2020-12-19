/// <reference path="../node_modules/es6-promise/dist/es6-promise.js" />
/// <reference path="../node_modules/aspnet-validation/dist/aspnet-validation.js" />

//HACK: Workaround for https://github.com/ryanelian/aspnet-validation/issues/7
var forms = document.getElementsByTagName('form');
for (var i = 0; i < forms.length; i++) {
    (function (form) {
        var defaultAction = form.action;
        var buttons = form.querySelectorAll("button[type=submit],input[type=submit]");
        var containsFormActionButton = false;

        for (var j = 0; !containsFormActionButton && j < buttons.length; j++) {
            var button = buttons.item(j);
            containsFormActionButton = button.hasAttribute('formaction');
        }

        if (containsFormActionButton) {
            for (var k = 0; k < buttons.length; k++) {
                buttons.item(k).addEventListener('click', (e) => {
                    form.action = e.target.hasAttribute('formaction') ? e.target.getAttribute('formaction') : defaultAction;
                });
            }
        }
    })(forms.item(i));
}

// Regular initialization of validation service
var v = new aspnetValidation.ValidationService();
v.bootstrap();
