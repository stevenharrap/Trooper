function BootstrapSelectList(params) {
    this.id = params.id;
    this.formId = params.formId;
    this.warnOnLeave = params.warnOnLeave;

    GetBootstrapHtml().addControl(this.id, this, 'selectlist');
    $(document).ready($.proxy(this.init, this));
}

BootstrapSelectList.prototype.id = '';

BootstrapSelectList.prototype.formId = '';

BootstrapSelectList.prototype.warnOnLeave = true;

BootstrapSelectList.prototype.init = function () {
    if (this.warnOnLeave) {
        var bsf = GetBootstrapHtml().getForm(this.formId);
        bsf.addVolatileField(this.id);
    }
};