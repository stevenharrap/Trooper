trooper.ui.control.radioList = (function (params)
{
    this.id = params.id;
    this.formId = params.formId;
    this.name = params.name;
    this.warnOnLeave = params.warnOnLeave;

    this.init = function () {
        if (this.warnOnLeave) {
            var form = trooper.ui.getForm(this.formId);
			
            $('[name="' + this.name + '"]').each(function (index, value) {
                form.addVolatileField($(value));
            });
        }
    };

    var publicResult = {
        id: trooper.utility.control.makeIdAccessor(this)
    };

    trooper.ui.registry.addControl(this.id, publicResult, 'radiolist');
    $(document).ready($.proxy(this.init, this));

    return publicResult;
});