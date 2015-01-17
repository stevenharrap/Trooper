function StdThemeSearchBar() {
    return window['stdThemeSearchBar'];
}

function StdThemeSearchBar() {
    this.ignoreLoadingScreenElements = new Array();

    $(document).ready($.proxy(this.init, this));
}

StdThemeSearchBar.prototype.searchRowOpen = false;

StdThemeSearchBar.prototype.searchRowChanging = false;

StdThemeSearchBar.prototype.searchRowNormalBg = '#f0f0f0';

StdThemeSearchBar.prototype.searchRowHightlightBg = '#e8e8e8';

StdThemeSearchBar.prototype.searchRowDefaultHeight = 81;

StdThemeSearchBar.prototype.init = function () {
    $(".template-search-row").css('height', this.searchRowDefaultHeight);
    this.vAlignSearchButtons();
    this.screenModeChange(GetBootstrapHtml().getScreenMode());
    GetBootstrapHtml().addScreenModeEvent($.proxy(this.screenModeChange, this));
    
    $(".template-search-row").click($.proxy(this.activateSearchRow, this));
    $(".template-search-row .opacity-box").mouseover($.proxy(this.opacityBoxOver, this, true));
    $(".template-search-row .opacity-box").mouseout($.proxy(this.opacityBoxOver, this, false));
};

StdThemeSearchBar.prototype.screenModeChange = function (size, loading) {
    var searchButton = $('Button[name="Command"][value="Refresh"]');
    var text = size == 'Medium' || size == 'Large' ? ' Search' : '';

    if (searchButton.contents().length == 1) {
        searchButton.append(text);
    } else {
        searchButton.contents().filter(function () { return this.nodeType == 3; }).replaceWith(text);
    }

    if (this.searchRowOpen) {
        $('.template-search-row .close-search-bar').click();
    }
};

StdThemeSearchBar.prototype.vAlignSearchButtons = function () {
    var searchRow = $('.template-search-row');
    var buttonBox = $('.template-search-row .search-bar-buttons');
    var closeBox = $('.template-search-row .close-search-bar');
    var topHeight = searchRow.height() - buttonBox.height() - 15;
    var fontSize = topHeight / 2;;

    if (fontSize > 70) {
        fontSize = 70;
    }

    closeBox.css('font-size', fontSize + 'px');
    closeBox.css('height', topHeight);
    closeBox.css('padding-top', (topHeight / 2) - (fontSize / 2));
};

StdThemeSearchBar.prototype.activateSearchRow = function (e) {
    if (this.searchRowChanging) {
        return;
    }

    var target = $(e.target);

    if (target.hasClass('glyphicon-zoom-out') || (target.contents().length > 0 && $(target.contents()[0]).hasClass('glyphicon-zoom-out'))) {
        return;
    }

    var allowedClick = target.hasClass('opacity-box') || target.hasClass('close-search-bar');

    if (this.searchRowOpen && !allowedClick) {
        return;
    }

    if (this.searchRowOpen) {
        $(".template-search-row").animate(
            { height: this.searchRowDefaultHeight, backgroundColor: this.searchRowNormalBg },
            {
                duration: 250,
                complete: $.proxy(this.searchRowActivated, this, 'contracted'),
                progress: $.proxy(this.vAlignSearchButtons, this)
            });
    }

    if (!this.searchRowOpen) {
        var closePxHeight = $(".template-search-row").css('height');
        var closedHeight = $(".template-search-row").height();
        $(".template-search-row").css('height', 'auto');
        var autoHeight = $(".template-search-row").height();
        
        if (closedHeight == autoHeight) {
            $(".template-search-row").css('height', closePxHeight);
            $(".template-search-row").css('overflow', 'visible');
            return;
        }

        $(".template-search-row").css('height', this.searchRowDefaultHeight);
        $(".close-search-bar").css('visibility', 'visible');

        $(".template-search-row").animate(
            { height: autoHeight + 10, backgroundColor: this.searchRowHightlightBg },
            {
                duration: 250,
                complete: $.proxy(this.searchRowActivated, this, 'expanded'),
                progress: $.proxy(this.vAlignSearchButtons, this)
            });
    }

    this.searchRowChanging = true;
};

StdThemeSearchBar.prototype.searchRowActivated = function (direction) {
    if (direction == 'expanded') {
        $(".template-search-row").css('overflow', 'visible');
        this.searchRowOpen = true;
    }

    if (direction == 'contracted') {
        this.searchRowOpen = false;
        $(".template-search-row").css('overflow', 'hidden');
        $(".close-search-bar").css('visibility', 'hidden');
    }

    this.searchRowChanging = false;
};

StdThemeSearchBar.prototype.opacityBoxOver = function (over) {
    if (this.searchRowOpen) {
        return;
    }

    if (over) {
        $(".template-search-row .opacity-box").css('background-color', this.searchRowHightlightBg);
    } else {
        $(".template-search-row .opacity-box").css('background-color', this.searchRowNormalBg);
    }
};