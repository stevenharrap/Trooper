trooper.ui.control.popover = (function (params)
{
	this.id = params.id;
	this._content = params.content;
	this.title = params.title;
	this.placement = params.placement;
	this.placementAutoAssist = params.placementAutoAssist;
	this.selector = params.selector;
	this.bsPopover = null;

	this.init = function () {
	    debugger;
		this.bsPopover = $(this.selector).popover(
		{
			content: $.proxy(this.content, this),
			html: true,
			placement: new Array(this.placementAutoAssist ? 'auto ' : '', this.placement).join(''),
			title: this.title,
			trigger: 'manual',
		});

		var show = $.proxy(this.showPopover, this);
		var hide = $.proxy(this.hidePopover, this);

		this.bsPopover.focus(show);
		this.bsPopover.blur(hide);
		this.bsPopover.hover(show, hide);

	};

	this.showPopover = function () {
	    debugger;
	    this.bsPopover('show');
	};
    
	this.hidePopover = function () {
	    debugger;
        this.bsPopover('hide');
    };

	this.content = function(value) {
		if (arguments.length == 1) {
			this._content = value;
		} else {
			return this._content;
		}
	};

	trooper.ui.registry.addControl(this.id, this, 'popover');
	$(document).ready($.proxy(this.init, this));

	return {
		content: $.proxy(this.content, this)
	};
});