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
        this.bsPopover().on('shown.bs.popover', $.proxy(this.showCalendar, this));
    };

    this.showCalendar = function () {
        $("#" + this.id + " .jquery-ui-datepicker").datepicker();
    };

    this.bsPopover = function () {
        return trooper.ui.registry.getPopover(this.popoverId).bsPopover();
    };
	
	trooper.ui.registry.addControl(this.id, this, 'datetimepicker');
	$(document).ready($.proxy(this.init, this));

	return {
		date: $.proxy(this.date, this)
	};
});