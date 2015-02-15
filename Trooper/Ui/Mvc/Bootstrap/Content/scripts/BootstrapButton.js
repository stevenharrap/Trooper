function BootstrapButton(params) {
    this.id = params.id;
    this.formId = params.formId;
    this.url = params.url;
    this.targetNewWindow = params.targetNewWindow;
    this.launchLoadingOnClick = params.launchLoadingOnClick;
    this.loadingScreenTitle = params.loadingScreenTitle;
    this.confirmOnClick = params.confirmOnClick;
    this.confirmTitle = params.confirmTitle;
    this.submit = params.submit;

    GetBootstrapHtml().addControl(this.id, this, 'button');
    $(document).ready($.proxy(this.init, this));
};

BootstrapButton.prototype.id = '';

BootstrapButton.prototype.formId = '';

BootstrapButton.prototype.url = '';

BootstrapButton.prototype.targetNewWindow = '';

BootstrapButton.prototype.launchLoadingOnClick = false;

BootstrapButton.prototype.loadingScreenTitle = '';

BootstrapButton.prototype.confirmOnClick = false;

BootstrapButton.prototype.confirmTitle = '';

BootstrapButton.prototype.submit = '';

BootstrapButton.prototype.getInput = function () {
    return $('#' + this.id);
};

BootstrapButton.prototype.init = function () {
    $('#' + this.id).click($.proxy(this.clicked, this));
};

BootstrapButton.prototype.clicked = function () {
    if (this.submit && this.url != null) {
        $('#' + this.formId).attr('action', this.url);
        $('#' + this.formId).submit();
        return;
    }

    if (this.url != '') {
        if (this.targetNewWindow) {
            window.open(this.url, '_blank');
        } else {
            window.location.href = this.url;
        }
    }
};