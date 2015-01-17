function GetStdThemeTemplate() {
    return window['stdThemeTemplate'];
}

function StdThemeTemplate() {
    $(document).ready($.proxy(this.init, this));
}

StdThemeTemplate.prototype.init = function () {
    $('[data-toggle=offcanvas]').click(function () {
        $('.row-offcanvas').toggleClass('active');
    });

    $('.submit-results button.close.any').click(function () {
        $('.submit-results').hide(500);
    });

    $('.submit-results button.close.open-errors').click(function () {
        $('.submit-results .errors').show(500);
        $('.submit-results button.close.open-errors').hide(500);
        $('.submit-results button.close.close-errors').show(500);
    });

    $('.submit-results button.close.close-errors').click(function () {
        $('.submit-results .errors').hide(500);
        $('.submit-results button.close.close-errors').hide(500);
        $('.submit-results button.close.open-errors').show(500);
    });
    
    setTimeout($.proxy(this.hideLogo, this), 2000);
};

StdThemeTemplate.prototype.linkClick = function (e) {
    if (!GetBootstrapForm().canLeave()) {
        return false;
    }

    this.openLoadingScreen();

    return true;
};

StdThemeTemplate.prototype.hideLogo = function () {
    if ($('body').hasClass('logo-row-open')) {
        $('.top-logo-area').slideUp('slow');
        $('body').animate({ paddingTop: '60px' }, 700);
        $('.navbar-logo-area').show('slow');
    }
};