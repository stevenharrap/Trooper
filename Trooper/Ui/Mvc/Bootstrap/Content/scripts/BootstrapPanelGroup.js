function BootstrapPanelGroup(params) {
    this.id = params.id;
    this.active = params.active;
    this.hasErrors = params.hasErrors;

    GetBootstrapHtml().addControl(this.id, this, 'panelgroup');
    $(document).ready($.proxy(this.init, this));
}

BootstrapPanelGroup.prototype.id = '';

BootstrapPanelGroup.prototype.active = '';

BootstrapPanelGroup.prototype.hasErrors = false;

BootstrapPanelGroup.prototype.init = function () {
    $('#' + this.id + ' .panel .panel-heading a').click($.proxy(this.panelOpened, this));

    if (this.hasErrors) {
        $("html, body").animate({ scrollTop: 0 }, 'slow');
    }

    if (window.location.hash != '') {
        var anchoredPannel = $(window.location.hash);
        
        if (anchoredPannel.length > 0 && !anchoredPannel.hasClass('in')) {
            anchoredPannel.addClass('in');
        }

        this.setFormAnchor(window.location.hash);
    }
    else if (this.active != '') {
        var currentPannel = $('#' + this.active);

        if (currentPannel.length > 0) {
            currentPannel.addClass('in');
        }
    }
};

BootstrapPanelGroup.prototype.panelOpened = function (pannelTitle) {
    var panel = $(pannelTitle.target).parents('.panel');

    if (panel.length > 0 && panel.find('.in').length > 0) {
        this.setFormAnchor('');
    } else {
        var anchor = pannelTitle.target.href.split('#').pop();
        this.setFormAnchor(anchor);
    }
};

BootstrapPanelGroup.prototype.setFormAnchor = function (anchorName) {
    var form = $('#' + this.id).parents('form');
    
    if (form.length == 0) {
        return;
    }

    anchorName = anchorName.replace('#', '');

    var components = GetBootstrapHtml().getUrlAsComponents(form.attr('action'));
    components.hash = anchorName;
    form.attr('action', GetBootstrapHtml().convertComponentsToUrl(components));
};