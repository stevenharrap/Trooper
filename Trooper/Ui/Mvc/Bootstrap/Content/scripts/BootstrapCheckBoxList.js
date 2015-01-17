function BootstrapCheckBoxList(params) {
    this.id = params.id;
    this.formId = params.formId;
    this.name = params.name;
    this.warnOnLeave = params.warnOnLeave;

    GetBootstrapHtml().addControl(this.id, this, 'checkboxlist');
    $(document).ready($.proxy(this.init, this));
}

BootstrapCheckBoxList.prototype.id = '';

BootstrapCheckBoxList.prototype.formId = '';

BootstrapCheckBoxList.prototype.name = '';

BootstrapCheckBoxList.prototype.warnOnLeave = true;

BootstrapCheckBoxList.prototype.init = function () {
    if (this.warnOnLeave) {
        var bsf = GetBootstrapHtml().getForm(this.formId);

        $('[name=' + this.name + ']').each(function (index, value) {
            bsf.addVolatileField($(value));
        });
    }
};