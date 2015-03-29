trooper.ui.control.button = (function (params)
{
    this.id = params.id;
    this.formId = params.formId;
    this.url = params.url;
    this.targetNewWindow = params.targetNewWindow;
    this.launchLoadingOnClick = params.launchLoadingOnClick;
    this.loadingScreenTitle = params.loadingScreenTitle;
    this.confirmOnClick = params.confirmOnClick;
    this.confirmTitle = params.confirmTitle;
    this.submit = params.submit;

    this.init = function () {        
        $('#' + this.id).click($.proxy(this.clicked, this));
    };

    this.clicked = function () {
        if (this.submit && this.url != '') {
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
    
    var publicResult = {
        id: trooper.utility.control.makeIdAccessor(this),
    };

    trooper.ui.registry.addControl(this.id, publicResult, 'button');
    $(document).ready($.proxy(this.init, this));

    return publicResult;
});