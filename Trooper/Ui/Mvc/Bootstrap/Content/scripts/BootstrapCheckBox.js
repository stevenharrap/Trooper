function BootstrapCheckBox(params) {
    this.id = params.id;
    this.formId = params.formId;
    this.warnOnLeave = params.warnOnLeave;

    GetBootstrapHtml().addControl(this.id, this, 'checkbox');
    $(document).ready($.proxy(this.init, this));
}

BootstrapCheckBox.prototype.id = '';

BootstrapCheckBox.prototype.formId = '';

BootstrapCheckBox.prototype.warnOnLeave = true;

BootstrapCheckBox.prototype.init = function () {
    if (this.warnOnLeave) {
        var bsf = GetBootstrapHtml().getForm(this.formId);
        bsf.addVolatileField(this.inputId);
    }
};