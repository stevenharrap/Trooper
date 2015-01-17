function BootstrapTextareaBox(params) {
    this.id = params.id;
    this.formId = params.formId;
    this.maxLength = params.maxLength;
    this.warnOnLeave = params.warnOnLeave;
   
    GetBootstrapHtml().addControl(this.id, this, 'textareabox');
    $(document).ready($.proxy(this.init, this));
};

BootstrapTextareaBox.prototype.id = '';

BootstrapTextareaBox.prototype.formId = '';

BootstrapTextareaBox.prototype.maxLength = 0;

BootstrapTextareaBox.prototype.warnOnLeave = true;

BootstrapTextareaBox.prototype.getInput = function () {
    return $('#' + this.id);
};

BootstrapTextareaBox.prototype.init = function () {
    this.getInput().change($.proxy(this.checkInput, this));

    if (this.warnOnLeave) {
        var bsf = GetBootstrapHtml().getForm(this.formId);;
        bsf.addVolatileField(this.inputId);
    }
};

BootstrapTextareaBox.prototype.val = function (value) {
    if (arguments.length == 1) {
        this.getInput().val(value);
    }

    return this.getInput().val();
};