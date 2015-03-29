trooper.ui.control.popover = (function (params)
{
	this.id = params.id;
	this._content = params.content;
	this.title = params.title;
	this.placement = params.placement;
	this.placementAutoAssist = params.placementAutoAssist;
	this.selector = params.selector;
	this.behaviour = params.behaviour;
    this._ignoreSelectors = new Array();

	this.init = function () {
		$(this.selector).popover(
		{
			content: $.proxy(this.content, this),
			html: true,
			placement: new Array(this.placementAutoAssist ? 'auto ' : '', this.placement).join(''),
			title: this.title,
			trigger: this.behaviour == 'Hover' ? 'hover' : 'manual'
		});

		if (this.behaviour == 'ClickThenClickOutside') {
			$(this.selector).click($.proxy(this.selectorClickToggle, this));
			$('body').on('click', $.proxy(this.outsideSelectorClickAndHide, this));
		}
		else if (this.behaviour == 'ClickThenClickAnywhere') {
			$(this.selector).click($.proxy(this.selectorClickToggle, this));
			$(this.selector).on('shown.bs.popover', $.proxy(this.setupAnywhereClickAndHide, this));
		}
	};

	this.selectorClickToggle = function(e) {
	    $(this.selector).popover('toggle');

	    var allPopovers = trooper.ui.registry.getControls('popover');

	    debugger;

	    for (var i = 0; i < allPopovers.length; i++) {
	        if (allPopovers[i].id() != this.id) {
	            allPopovers[i].close();
	        }
	    }

		e.stopPropagation();
	};

	this.outsideSelectorClickAndHide = function (e) {
	    for (var i = 0; i < this._ignoreSelectors.length; i++) {
	        if ($(e.target).is(this._ignoreSelectors[i])) {
	            return;
	        }
	    }

		if (!this.isOpen()) {
			return;
		}

		var popoverContent = $(this.selector).parent().find('.popover');

		if (popoverContent.length === 0) {
			return;
		}

		var element = $(e.target)[0];

		while (element != null && element != popoverContent[0]) {
			element = element.parentElement;
		}

		if (element != null && element == popoverContent[0]) {
			return;
		}

		$(this.selector).popover('hide');
	};

	this.setupAnywhereClickAndHide = function () {
		$('body').on('click', $.proxy(this.anywhereClickAndHide, this));
	};

	this.ignoreSelectors = function (value) {
	    if (arguments.length == 1) {
	        this._ignoreSelectors = value;
	    } else {
	        return this._ignoreSelectors;
	    }
	};

	this.anywhereClickAndHide = function () {
		$(this.selector).popover('hide');
	};

	this.bsPopover = function () {
	    return $(this.selector);
	};

	this.content = function(value) {
		if (arguments.length == 1) {
			this._content = value;
		} else {
			return this._content;
		}
	};

	this.contentElement = function () {
	    return $(this.selector).parent().find('.popover .popover-content');
	};

	this.isOpen = function() {
		var element = $(this.selector);

		return element.parent().find('#' + element.attr('aria-describedby')).hasClass('in');
	};

	this.close = function () {
	    $(this.selector).popover('hide');
	};

	var publicResult = {
	    id: trooper.utility.control.makeIdAccessor(this),
	    content: $.proxy(this.content, this),
	    contentElement: $.proxy(this.contentElement, this),
	    isOpen: $.proxy(this.isOpen, this),
	    close: $.proxy(this.close, this),
	    bsPopover: $.proxy(this.bsPopover, this),
	    ignoreSelectors: $.proxy(this.ignoreSelectors, this)
	};

	trooper.ui.registry.addControl(this.id, publicResult, 'popover');
	$(document).ready($.proxy(this.init, this));

	return publicResult;
});