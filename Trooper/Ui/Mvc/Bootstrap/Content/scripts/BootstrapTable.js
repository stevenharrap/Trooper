function BootstrapTable(params) {
    this.id = params.id;
    this.rowSelectionMode = params.rowSelectionMode;
    
    this.rowSelected = new Array();
    this.rowDblclicked = new Array();
    this.rowUnselected = new Array();
    this.selectedRows = new Array();

    GetBootstrapHtml().addControl(this.id, this, 'table');
    $(document).ready($.proxy(this.init, this));
}

BootstrapTable.prototype.id = null;

BootstrapTable.prototype.rowSelectionMode = null;

/*
* Array of selected row values
*/
BootstrapTable.prototype.selectedRows = null;

/*
* Array of events
*/
BootstrapTable.prototype.rowSelected = null;

/*
* Array of events
*/
BootstrapTable.prototype.rowDblclicked = null;

/*
* Array of events
*/
BootstrapTable.prototype.rowUnselected = null;

BootstrapTable.prototype.init = function () {
    $('#' + this.id + ' tbody tr').click($.proxy(this.rowClick, this));
    $('#' + this.id + ' tbody tr').dblclick($.proxy(this.rowDblclick, this));

    this.fixFormAction();

    if ($('#' + this.id + ' tbody tr').parents('form').length > 0) {
        $('#' + this.id + ' thead a').click($.proxy(this.tableActionClick, this));
        $('#' + this.id + ' tfoot a').click($.proxy(this.tableActionClick, this));
    }

    GetBootstrapHtml().addScreenModeEvent($.proxy(this.screenModeChanged, this));
    this.screenModeChanged(GetBootstrapHtml().getScreenMode());
};

BootstrapTable.prototype.screenModeChanged = function (screenMode) {
    $('#' + this.id + ' thead th[alt-headers]').each(function (i, e) {
        var element = $(e);
        var data = JSON.parse(element.attr('alt-headers'));
        var text = '';

        if (screenMode == 'Print' && data.pr != null) {
            text = data.pr;
        }

        if (screenMode == 'ExtraSmall' && data.xs != null) {
            text = data.xs;
        }

        if (screenMode == 'Small' && data.sm != null) {
            text = data.sm;
        }

        if (screenMode == 'Medium' && data.md != null) {
            text = data.md;
        }
        
        if (text == '') {
            text = data.lg;
        }

        if (text != '') {
            var a = element.find('a');

            if (a.length > 0) {
                a.text(text);
            } else {
                element.text(text);
            }
        }
    });


//"ExtraSmall", "Small", "Medium", "Large", "Print"
};

/**
 * Clears any selected rows in the table and optionally calls events in
 * rowUnselected if callEvents is true
 */
BootstrapTable.prototype.clearSelection = function(callEvents) {
    this.selectedRows = new Array();

    $('#' + this.id + ' tbody tr.highlight').each(function (index, value) {

        if (callEvents) {
            for (var u = 0; u < this.rowUnselected.length; u++) {
                if (this.rowUnselected[u] != null) {
                    this.rowUnselected[u](data);
                }
            }
        }

        $(value).removeClass('highlight');
    });
};

/**
 * If the table is in a form and we are refreshing from a column or page click then we
 * we need to update the form action so that any other form submition keeps the page and 
 * and column settings.
 */
BootstrapTable.prototype.fixFormAction = function() {
    var forms = $('#' + this.id + ' tbody tr').parents('form');

    if (forms.length == 0) {
        return;
    }

    var form = $(forms[0]);

    var formUrl = GetBootstrapHtml().getUrlAsComponents(form.attr('action'));
    var pageUrl = GetBootstrapHtml().getUrlAsComponents(window.location.href);
    
    GetBootstrapHtml().combineUrlComponents(pageUrl, formUrl, new Array()[this.id + '_sort', this.id + '_sortdir']);
    
    form.attr('action', GetBootstrapHtml().convertComponentsToUrl(formUrl));
};

BootstrapTable.prototype.tableActionClick = function(e) {
    var forms = $('#' + this.id + ' tbody tr').parents('form');
    
    if (forms.length == 0) {
        return true;
    }

    var form = $(forms[0]);
    var aUrl = GetBootstrapHtml().getUrlAsComponents($(e.target).attr('href'));
    var formUrl = GetBootstrapHtml().getUrlAsComponents(form.attr('action'));

    GetBootstrapHtml().combineUrlComponents(aUrl, formUrl);
    
    form.attr('action', GetBootstrapHtml().convertComponentsToUrl(formUrl));
    form.submit();

    return false;
};

BootstrapTable.prototype.rowDblclick = function (e) {
    if (this.rowDblclicked.length == 0) {
        return;
    }

    var tr = $(e.target);

    if (tr.prop("tagName").toLowerCase() == 'td') {
        tr = tr.parent('tr');
    }
    
    var dataitem = tr.attr('data-item');
    var unquoted = dataitem.replace('&quot;', '"');
    var data = jQuery.parseJSON(unquoted);

    this.selectedRows = new Array();
    this.selectedRows.push(data);
    
    for (var s = 0; s < this.rowDblclicked.length; s++) {
        this.rowDblclicked[s](data);
    }
};

BootstrapTable.prototype.rowClick = function(e) {
    var tr = $(e.target);

    if (tr.prop("tagName").toLowerCase() == 'td') {
        tr = tr.parent('tr');
    }

    var willBeSelected = !tr.hasClass('highlight');

    if (this.rowSelectionMode != 'None') {
        if (tr.hasClass('highlight')) {
            tr.removeClass('highlight');
        }
    }

    if (willBeSelected) {
        if (this.rowSelectionMode == 'Single') {
            tr.addClass('highlight').siblings().removeClass('highlight');
        } else if (this.rowSelectionMode == 'Multiple') {
            tr.addClass('highlight');
        }
    }

    var dataitem = tr.attr('data-item');
    var unquoted = dataitem.replace('&quot;', '"');
    var data = jQuery.parseJSON(unquoted);

    if (this.rowSelectionMode == 'Single') {
        this.selectedRows = new Array();

        if (willBeSelected) {
            this.selectedRows.push(data);
        }
    } else if (this.rowSelectionMode == 'Multiple') {
        if (willBeSelected) {
            this.selectedRows.push(data);
        } else {
            var resultRows = new Array();

            for (var i = 0; i < this.selectedRows.length; i++) {
                if (!this.arraysEqual(data, this.selectedRows[i])) {
                    resultRows.push(this.selectedRows[i]);
                }
            }

            this.selectedRows = resultRows;
        }
    }
    
    if (willBeSelected) {
        for (var s = 0; s < this.rowSelected.length; s++) {
            if (this.rowSelected[s] != null) {
                this.rowSelected[s](data);
            }
        }
    } else {
        for (var u = 0; u < this.rowUnselected.length; u++) {
            if (this.rowUnselected[u] != null) {
                this.rowUnselected[u](data);
            }
        }
    }
};

BootstrapTable.prototype.arraysEqual = function(a, b) {
    for (key in a) { //In a and not in b
        if (!b[key]) {
            return false;
        }
    }
    for (key in a) { //in a and b but different values
        if (a[key] && b[key] && a[key] != b[key]) {
            return false;
        }
    }
    for (key in b) { //in b and not in a
        if (!a[key]) {
            return false;
        }
    }

    return true;
};