function BootstrapTextbox(params) {
    this.id = params.id;
    this.formId = params.formId;
    this.maxLength = params.maxLength;
    this.warnOnLeave = params.warnOnLeave;
   
    GetBootstrapHtml().addControl(this.id, this, 'textbox');
    $(document).ready($.proxy(this.init, this));
};

BootstrapTextbox.prototype.id = '';

BootstrapTextbox.prototype.formId = '';

BootstrapTextbox.prototype.maxLength = 0;

BootstrapTextbox.prototype.warnOnLeave = true;

BootstrapTextbox.prototype.getInput = function () {
    return $('#' + this.id);
};

BootstrapTextbox.prototype.init = function () {
    this.getInput().change($.proxy(this.checkInput, this));

    if (this.warnOnLeave) {
        var bsf = GetBootstrapHtml().getForm(this.formId);
        bsf.addVolatileField(this.inputId);
    }
};

BootstrapTextbox.prototype.val = function(value) {
    if (arguments.length == 1) {
        this.getInput().val(value);
    }

    return this.getInput().val();
};