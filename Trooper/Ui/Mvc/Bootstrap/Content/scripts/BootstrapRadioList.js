function BootstrapRadioList(params) {
    this.id = params.id;
    this.formId = params.formId;
    this.name = params.name;
    this.warnOnLeave = params.warnOnLeave;

    GetBootstrapHtml().addControl(this.id, this, 'radiolist');
    $(document).ready($.proxy(this.init, this));
}

BootstrapRadioList.prototype.id = '';

BootstrapRadioList.prototype.formId = '';

BootstrapRadioList.prototype.name = '';

BootstrapRadioList.prototype.warnOnLeave = true;

BootstrapRadioList.prototype.init = function () {
    if (this.warnOnLeave) {
        var bsf = GetBootstrapHtml().getForm(this.formId);

        $('[name="' + this.name + '"]').each(function (index, value) {
            bsf.addVolatileField($(value));
        });
    }
};