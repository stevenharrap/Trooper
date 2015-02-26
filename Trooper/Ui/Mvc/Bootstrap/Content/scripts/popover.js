trooper.ui.control.popover = (function (params)
{
	this.id = params.id;
	this._content = params.content;
	this.title = params.title;
	this.placement = params.placement;
	this.placementAutoAssist = params.placementAutoAssist;
	this.selector = params.selector;
	this.behaviour = params.behaviour;

	this.init = function () {
		$(this.selector).popover(
		{
			content: $.proxy(this.content, this),
			html: true,
			placement: new Array(this.placementAutoAssist ? 'auto ' : '', this.placement).join(''),
			title: this.title,
			trigger: this.behaviour == 'Hover' ? 'hover' : 'manual'
		}).click(function (e) {e.preventDefault();});

		//click : click selector to show/hide
		//hover: hover over selector to show/hide
		//focus: ??

		if (this.behaviour == 'ClickThenClickOutside') {
			$(this.selector).click($.proxy(this.selectorClickToggle, this));
			$('body').on('click', $.proxy(this.outsideSelectorClickAndHide, this));
		}
		else if (this.behaviour == 'ClickThenClickAnywhere') {
			$(this.selector).click($.proxy(this.selectorClickToggle, this));
			$(this.selector).on('shown.bs.popover', $.proxy(this.setupAnywhereClickAndHide, this));
		}
	};

	this.selectorClickToggle = function (e) {
		$(this.selector).popover('toggle');
		e.stopPropagation();
	}

	this.outsideSelectorClickAndHide = function(e) {
		$('[data-original-title]').each(function () {
			if (!$(this).is(e.target)
				&& $(this).has(e.target).length === 0
				&& $('.popover').has(e.target).length === 0) {
				$(this).popover('hide');
			}
		});
	};

	this.setupAnywhereClickAndHide = function () {
		$('body').on('click', $.proxy(this.anywhereClickAndHide, this));
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

	this.isOpen = function() {
		var element = $(this.selector);
		return element.parent().find('#' + element.attr('aria-describedby')).hasClass('in');
	};

	trooper.ui.registry.addControl(this.id, this, 'popover');
	$(document).ready($.proxy(this.init, this));

	return {
	    content: $.proxy(this.content, this),
	    isOpen: $.proxy(this.isOpen, this),
	    bsPopover: $.proxy(this.bsPopover, this)
	};
});