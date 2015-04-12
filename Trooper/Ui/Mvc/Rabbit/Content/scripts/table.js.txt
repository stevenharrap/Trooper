trooper.ui.control.table = (function (params)
{
    this.id = params.id;
    this.rowSelectionMode = params.rowSelectionMode;
    
    this.rowSelected = new Array();
    this.rowDblclicked = new Array();
    this.rowUnselected = new Array();
    this._selectedRows = new Array();	
	
    this.init = function () {       
		
		$('#' + this.id + ' tbody tr').click($.proxy(this.rowClick, this));
		$('#' + this.id + ' tbody tr').dblclick($.proxy(this.rowDblclick, this));

		this.fixFormAction();

		if ($('#' + this.id + ' tbody tr').parents('form').length > 0) {
			$('#' + this.id + ' thead a').click($.proxy(this.tableActionClick, this));
			$('#' + this.id + ' tfoot a').click($.proxy(this.tableActionClick, this));
		}

		trooper.ui.html.addScreenModeEvent($.proxy(this.screenModeChanged, this));
		this.screenModeChanged(trooper.ui.html.getScreenMode());
    };
	
	this.addRowSelectedEvent = function(func) {
		this.rowSelected.push(func);
	};
	
	this.addRowDblclickedEvent = function(func) {
		this.rowDblclicked.push(func);
	};
	
	this.addRowUnselectedEvent = function(func) {
		this.rowUnselected.push(func);
	};
	
	this.selectedRows = function() {
		return this._selectedRows;
	};
	
	this.screenModeChanged = function (screenMode) {
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
	this.clearSelection = function(callEvents) {
		this._selectedRows = new Array();

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
	this.fixFormAction = function() {
		var forms = $('#' + this.id + ' tbody tr').parents('form');

		if (forms.length == 0) {
			return;
		}

		var form = $(forms[0]);

		var formUrl = trooper.utility.url.getUrlAsComponents(form.attr('action'));
		var pageUrl = trooper.utility.url.getUrlAsComponents(window.location.href);
		
		trooper.utility.url.combineUrlComponents(pageUrl, formUrl, new Array()[this.id + '_sort', this.id + '_sortdir']);
		
		form.attr('action', trooper.utility.url.convertComponentsToUrl(formUrl));
	};
	
	this.tableActionClick = function(e) {
		var forms = $('#' + this.id + ' tbody tr').parents('form');
		
		if (forms.length == 0) {
			return true;
		}

		var form = $(forms[0]);
		var aUrl = trooper.utility.url.getUrlAsComponents($(e.target).attr('href'));
		var formUrl = trooper.utility.url.getUrlAsComponents(form.attr('action'));

		trooper.utility.url.combineUrlComponents(aUrl, formUrl);
		
		form.attr('action', trooper.utility.url.convertComponentsToUrl(formUrl));
		form.submit();

		return false;
	};
	
	this.rowDblclick = function (e) {
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
	
	this.rowClick = function(e) {
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
	
	this.arraysEqual = function(a, b) {
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
	
	var publicResult = {
	    id: trooper.utility.control.makeIdAccessor(this),
	    clearSelection: $.proxy(this.clearSelection, this),
	    addRowSelectedEvent: $.proxy(this.addRowSelectedEvent, this),
	    addRowDblclickedEvent: $.proxy(this.addRowDblclickedEvent, this),
	    addRowUnselectedEvent: $.proxy(this.addRowUnselectedEvent, this),
	    selectedRows: $.proxy(this.selectedRows, this)
	};

	trooper.ui.registry.addControl(this.id, publicResult, 'table');
    $(document).ready($.proxy(this.init, this));

    return publicResult;
});