﻿trooper.ui.control.checkBoxList = (function (params)
{
    this.id = params.id;
    this.formId = params.formId;
    this.name = params.name;
    this.warnOnLeave = params.warnOnLeave;

    this.init = function () {
        if (this.warnOnLeave) {
            var form = trooper.ui.registry.getForm(this.formId);
            $('[name=' + this.name + ']').each(function (index, value) {
                form.addVolatileField($(value));
            });
        }
    };

    trooper.ui.registry.addControl(this.id, this, 'checkboxlist');
    $(document).ready($.proxy(this.init, this));

    return {

    };

});