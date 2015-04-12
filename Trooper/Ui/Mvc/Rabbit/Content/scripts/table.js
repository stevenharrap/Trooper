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
		$('#' + this.id + ' tfoot li').click($.proxy(this.pageClicked, this));

        trooper.ui.html.addScreenModeChangeEvent($.proxy(this.screenModeChanged, this));
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

	this.clearSelection = function () {

	};
	
	this.screenModeChanged = function (screenMode) {
	    var headers = $('#' + this.id + ' thead th[data-value]');

	    for (var i = 0; i < headers.length; i++) {	        
	        var element = $(headers[i]);
	        var data = JSON.parse(element.attr('data-value'));
	        var text = data.h;

	        if (screenMode == 'Print' && data.hp != null) {
	            element.text(data.hp);
	            return;
	        }

	        if (screenMode == 'ExtraSmall') {
	            text = this.coalesc(data.hes, data.hs, data.hm, data.h);
	        }
	        else if (screenMode == 'Small') {
	            text = this.coalesc(data.hs, data.hm, data.h);
	        }
	        else if (screenMode == 'Medium') {
	            text = this.coalesc(data.hm, data.h);
	        }

	        element.text(text);
	    }
	};

	this.pageClicked = function (e) {

	};

	this.coalesc = function () {
	    debugger;
	    for (var i = 0; i<arguments.length; i++) {
	        if (arguments[i] != null) {
	            return arguments[i];
	        }
	    }

	    return null;
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