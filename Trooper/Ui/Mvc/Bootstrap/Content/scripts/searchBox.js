trooper.ui.control.searchBox = (function (params)
{
    this.id = params.id;
    this.name = params.name;
    this.formId = params.formId;
    this.dataSourceUrl = params.dataSourceUrl;
    this.searchTextField = params.searchTextField;
    this.searchValueField = params.searchValueField;
    this.selectedTextField = params.selectedTextField;
    this.scrollHeight = params.scrollHeight;
    this.popoverPlacement = params.popoverPlacement;
    this.popoutWidth = params.popoutWidth;
	this.currentDataSelection = null;
	this.popoutWidth = null;
	this.currentAjaxRequest = null;    
    this.selectEvents = new Array();
    this.clearEvents = new Array();

    if (params.selectEvent != null) {
        this.selectEvents.push(params.selectEvent);
    }

    if (params.clearEvent != null) {
        this.clearEvents.push(params.clearEvent);
    }
	
	this.init = function () {        
		$('#' + this.inputName + '_bs_search_box').click($.proxy(this.insideClick, this));
		$('html').click($.proxy(this.outsideClick, this));
		this.getInputBox().keyup($.proxy(this.typing, this));
    };
	
	this.getInputBox = function () {
		return $('#' + this.name + '_search');
	};

	this.getHidden = function () {
		return $('#' + this.name);
	};

	this.getContainer = function () {
		return this.getInputBox().parent();
	};

	this.insideClick = function (e) {
		//// This was commented out after the search bar came
		//// into existance in StdTheming. Nothing seems to bothered yet.
		//// May need review.
		//e.stopPropagation();
	};

	this.outsideClick = function () {
		this.getInputBox().popover('destroy');
	};

	this.clear = function() {
		this.getInputBox().val('');
		this.getHidden().val('');
		this.currentDataSelection = null;
	};
	
	/**
	 * If the user clears the text box or hits escape then close the popopup
	 * and clear everything. If they backspace then clear any current selected value.
	 */
	this.typing = function (key) {
		if (this.currentAjaxRequest != null) {
			this.currentAjaxRequest.abort();
		}

		if (this.getInputBox().val() == '' || key.keyCode == 27) {
			this.clear();
			this.getInputBox().popover('destroy');
			this.fireClearEvent();
			return;
		}
		else if (key.keyCode == 8) {
			this.getHidden().val('');
			this.currentDataSelection = null;
			this.fireClearEvent();
		}

		var url = this.dataSourceUrl.indexOf("?") > -1
			? (this.dataSourceUrl + '&text=' + encodeURIComponent(this.getInputBox().val()))
			: (this.dataSourceUrl + '/?text=' + encodeURIComponent(this.getInputBox().val()));

		this.currentAjaxRequest = $.ajax({ url: url })
			.done($.proxy(this.done, this))
			.fail($.proxy(this.fail, this));

		this.wait();
	};

	this.wait = function() {
		this.getInputBox().popover('destroy');

		var html = '<div style="margin: 10px;">' +
			'<p>Just a moment...</p>' +
			'<div class="progress progress-striped">' +
			'<div class="progress-bar progress-bar-info" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%;">' +
			'</div></div>' +
			'<div>';
		
		this.getInputBox().popover({ title: 'Finding...', html: true, content: html, placement: this.popoverPlacement });
		
		this.showPopover();
	};
	
	this.done = function (result) {
		this.getInputBox().popover('destroy');

		var html = '<ul style="height: '+ this.scrollHeight+'px"></ul>';
			
		this.getInputBox().popover({ title: 'Found...', html: true, content: html, placement: this.popoverPlacement });
		
		this.showPopover();
		
		var select = $(this.getContainer()).find('ul');

		if (result != null) {
			for (var i = 0; i < result.length; i++) {
				var option = $('<li></li>');
				option.attr('data-value', JSON.stringify(result[i]));
				option.text(result[i][this.searchTextField]);
				select.append(option);
			}
		}

		$(select).find('li').click($.proxy(this.selected, this));
	};

	this.showPopover = function () {
		this.getInputBox().popover('show');

		var width = null;
		
		if ((this.popoverPlacement == 'bottom' || this.popoverPlacement == 'top') && this.popoutWidth == null) {
			width = this.getInputBox().width();
		} else if (this.popoutWidth != null) {
			width = this.popoutWidth;
		}

		if (width != null) {
			this.getContainer().find('.popover').css('width', width);
			this.getContainer().find('.popover').css('max-width', width);
			this.getInputBox().popover('show');
		}
	};
	
	this.selected = function (e) {
		var raw = $(e.target).attr('data-value');

		if (raw == '') {
			return;
		}

		var value = JSON.parse(raw);
		var text = value[this.selectedTextField];
		
		this.getInputBox().val(text);
		this.getHidden().val(value[this.searchValueField]);
		this.currentDataSelection = value;

		for (var i = 0; i < this.selectEvents.length; i++) {
			this.selectEvents[i](value);
		}
		
		this.getInputBox().popover('destroy');
	};

	this.fireClearEvent = function() {
		for (var i = 0; i < this.clearEvents.length; i++) {
			this.clearEvents[i]();
		}
	};

	this.fail = function (error) {
		////remove comment if debugging
		////alert('fail: ' + error.responseText);
	};

	var publicResult = {
	    id: trooper.utility.control.makeIdAccessor(this),
	    clear: $.proxy(this.clear, this)
	};

	trooper.ui.registry.addControl(this.id, publicResult, 'searchbox');
    $(document).ready($.proxy(this.init, this));

    return publicResult;
});