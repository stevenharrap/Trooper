﻿trooper.ui.control.checkBox = (function (params)
{
    this.id = params.id;
    this.formId = params.formId;
    this.warnOnLeave = params.warnOnLeave;

    this.init = function () {
        if (this.warnOnLeave) {
            var form = trooper.ui.registry.getForm(this.formId);
            form.addVolatileField(this.id);
        }
    };

    trooper.ui.registry.addControl(this.id, this, 'checkbox');
    $(document).ready($.proxy(this.init, this));

    return {

    };
});