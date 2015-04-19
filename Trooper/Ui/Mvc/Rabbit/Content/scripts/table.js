trooper.ui.control.table = (function (params)
{
    this.id = params.id;
    this.formId = params.formId;
    this.name = params.name;
    this.rowSelectionMode = params.rowSelectionMode;
    this.columns = params.columns;
    this.postAction = params.postAction;

    this.rowDblclicked = new Array();
    this.rowUnselected = new Array();
    this._selectedRows = new Array();	
	
    this.init = function () {       
		
        $('#' + this.id + ' thead th').click($.proxy(this.columnClick, this));
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

	this.columnClick = function (e) {
	    var th = $(e.currentTarget);
	    var data = this.persistedData();
	    var sortInfo = null;	    

	    for (var i = 0; i < this.columns.length; i++) {
	        var column = this.columns[i];
	        if (th.hasClass('col-' + column.ci)) {
	            if (column.si == null) {
	                return;
	            }

	            sortInfo = data.Sorting[column.si];

	            if (sortInfo.Direction == null) {
	                sortInfo.Direction = 1;
	            } else if (sortInfo.Direction == 1) {
	                sortInfo.Direction = 2;
	            } else {
	                sortInfo.Direction = null;
	                sortInfo.Importance = 0;
	            }

	            break;	            
	        }
	    }

	    if (sortInfo.Direction != null) {
	        var highestInfo = null;

	        for (var i = 0; i < this.columns.length; i++) {
	            var column = this.columns[i];

	            if (highestInfo == null) {
	                highestInfo = data.Sorting[column.si];
	            } else if (column.si != null && data.Sorting[column.si].Importance > highestInfo.Importance) {
	                highestInfo = data.Sorting[column.si];
	            }
	        }

	        if (highestInfo != sortInfo) {
	            sortInfo.Importance = highestInfo.Importance + 1;
	        }
	    }

	    this.persistedData(data);
	    this.post();
	};

	this.rowClick = function (e) {
	    var data = this.persistedData();
	    var tr = $(e.currentTarget).closest('tr');
	    var keys = JSON.parse(tr.attr('data-value'));
        
	    if (data.Selected == null) {
	        data.Selected = new Array();
	    }

	    data.Selected.push(keys);
	    tr.addClass('selected');

	    this.persistedData(data);
	};

	this.clearSelection = function () {	    
	};
    	
	this.screenModeChanged = function (screenMode) {
		for (var c = 0; c < this.columns.length; c++) {
			var data = this.columns[c];
			var element = $('#' + this.id + ' thead th.col-' + data.ci);

			var text = data.h;

			if (screenMode == 'Print' && data.hp != null) {
				text = data.hp;
			} else if (screenMode == 'ExtraSmall') {
				text = this.coalesc(data.hes, data.hs, data.hm, data.h);
			}
			else if (screenMode == 'Small') {
				text = this.coalesc(data.hs, data.hm, data.h);
			}
			else if (screenMode == 'Medium') {
				text = this.coalesc(data.hm, data.h);
			}

			if (data.vim != null) {
				if (jQuery.inArray(screenMode, data.vim) == -1) {
					$('#' + this.id + ' .col-' + data.ci).addClass('hidden');
				} else {
					$('#' + this.id + ' .col-' + data.ci).removeClass('hidden');
				}
			}

			element.find("span.title").text(text);
		}
	};

	this.pageClicked = function (e) {
	    var data = this.persistedData();
	    data.PageNumber = parseInt($(e.currentTarget).attr('data-value')); 
	    this.persistedData(data);

	    this.post();
	};

	this.persistedData = function (jsonData) {
	    if (arguments.length == 1) {
	        $('textarea[name="' + this.name + '"]').val(JSON.stringify(jsonData));
	    } else {
	        return JSON.parse($('textarea[name="' + this.name + '"]').val());
	    }
	};

	this.coalesc = function () {
	    for (var i = 0; i<arguments.length; i++) {
	        if (arguments[i] != null) {
	            return arguments[i];
	        }
	    }

	    return null;
	};

	this.post = function () {
	    trooper.ui.registry.getForm(this.formId).submit(this.url);
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