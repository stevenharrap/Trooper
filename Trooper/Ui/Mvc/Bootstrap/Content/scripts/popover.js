trooper.ui.control.popover = (function (params)
{
	this.id = params.id;
	this._content = params.content;
	this.title = params.title;
	this._placement = params.placement;
	this.selector = params.selector;

	this.init = function () {
		debugger;
		$(this.selector).popover(
		{
			content: $.proxy(this.content, this),
			html: true,
			placement: $.proxy(this.placement, this),
			selector: this.selector,
			title: this.title
		});
	};

	this.content = function(value) {
		if (arguments.length == 1) {
			this._content = value;
		} else {
			return this._content;
		}
	};

	this.placement = function (value) {
		if (arguments.length == 1) {
			this._placement = value;
		} else {
			return this._placement;
		}
	};

	trooper.ui.registry.addControl(this.id, this, 'popover');
	$(document).ready($.proxy(this.init, this));

	return {
		content: $.proxy(this.content, this),
		placement: $.proxy(this.placement, this)
	};
});