trooper.ui.control.dateTimePicker = (function (params) {
    this.id = params.id;
    this.formId = params.formId;
    this.format = params.format;
    this.pickDate = params.pickDate;
    this.pickTime = params.pickTime;
    this.pickSeconds = params.pickSeconds;
    this.warnOnLeave = params.warnOnLeave;
    this.popoverPlacement = params.popoverPlacement;
    this.timezone = params.timezone;
    this.popoverId = params.popoverId;

    this.init = function () {
    	this.popover().content('<div></div>');
    	this.popover().ignoreSelectors(new Array(".jquery-ui-datetimepicker", ".trooper-ui-dtp-component"));
    	this.bsPopover().on('show.bs.popover', $.proxy(this.popoverShow, this));
    	this.bsPopover().on('shown.bs.popover', $.proxy(this.popoverShown, this));        
    };

    this.popoverShow = function () {
    	$('body').append('<div id=\"' + this.id + '_tempArea\"></div>');
    	var element = $('#' + this.id + '_tempArea');
    	element.datetimepicker();
    	var width = element.width();
    	var height = element.height();
    	element.remove();

    	this.popover().content('<div style="width:' + width + 'px; height:' + height + 'px" class=\"jquery-ui-datetimepicker\"></div>');
    };

    this.popoverShown = function () {
        $('#' + this.id + ' .jquery-ui-datetimepicker').datetimepicker(
            {
                onChangeMonthYear: $.proxy(this.addDtpClass, this)
            });

    	this.addDtpClass();
    };

    this.popover = function () {
    	return trooper.ui.registry.getPopover(this.popoverId);
    };

    this.bsPopover = function () {
        return this.popover().bsPopover();
    };

    this.addDtpClass = function () {
        $('#' + this.id + ' .jquery-ui-datetimepicker *').addClass('trooper-ui-dtp-component');
    };
	
	trooper.ui.registry.addControl(this.id, this, 'datetimepicker');
	$(document).ready($.proxy(this.init, this));

	return {
		date: $.proxy(this.date, this)
	};
});