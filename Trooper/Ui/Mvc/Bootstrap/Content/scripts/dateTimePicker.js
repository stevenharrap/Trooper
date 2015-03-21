trooper.ui.control.dateTimePicker = (function (params) {
    this.id = params.id;
    this.formId = params.formId;
    this.format = params.format;
    this.dateTimeFormat = params.dateTimeFormat;
    this.warnOnLeave = params.warnOnLeave;
    this.popoverPlacement = params.popoverPlacement;
    this.timezone = params.timezone;
    this.popoverId = params.popoverId;
    this.potentialValue = null;
	
    this.init = function () {
    	this.popover().content('<div></div>');
    	this.bsPopover().on('show.bs.popover', $.proxy(this.popoverShow, this));
    	this.bsPopover().on('shown.bs.popover', $.proxy(this.popoverShown, this));        
    };

    this.popoverShow = function () {
        var html = '';
        this.potentialValue = this.valAsMoment();

        if (this.dateTimeFormat.indexOf('Date') > -1) {
		    html += '<div class="input-group">' +
			    '<a href="#" class="btn btn-default input-group-addon go-prev-month"><i class="glyphicon glyphicon-arrow-left"></i></a>';

		    html += '<select class="month form-control" style="width:50%">';
		    for (var m = 0; m < 12; m++) {
			    html += '<option value="' + (m + 1) + '">' + (moment().month(m).format('MMM')) + '</option>';
		    }
		    html += '</select>';

		    html += '<input class="year form-control" type="text" style="width:50%" />' +
			    '<a href="#" class="btn input-group-addon btn-default go-next-month"><i class="glyphicon glyphicon-arrow-right"></i></a>' +
			    '</div>';

		    html += '<table class="table table-condensed">';
		    html += '<thead>';
		    html += '<tr>';
		    html += '<th>Sun</th>';
		    html += '<th>Mon</th>';
		    html += '<th>Tue</th>';
		    html += '<th>Wed</th>';
		    html += '<th>Thu</th>';
		    html += '<th>Fri</th>';
		    html += '<th>Sat</th>';
		    html += '</tr>';
		    html += '</thead>';
		    html += '<tbody>';

		    var i = 1;

		    for (var w = 0; w < 6; w++) {
			    html += '<tr>';

			    for (var d = 0; d < 7; d++) {
				    html += '<td class="day day-' + i + '"></td>';
				    i++;
			    }

			    html += '</tr>';
		    }

		    html += '</tbody>';
		    html += '</table>';
		    html += '<hr />';
	    }

    	if (this.dateTimeFormat.indexOf('Time') > -1) {
		    html += '<div class="time input-group">';
		    html += '<span class="input-group-addon">Time</span>';
		    html += '<input type="text" maxlength="2" class="hour form-control" />';
		    html += '<span class="input-group-addon dots"> : </span>';
		    html += '<input type="text" maxlength="2" class="minute form-control" />';
		    html += '<span class="input-group-addon dots"> : </span>';
		    html += '<input type="text" maxlength="2" class="second form-control" />';
		    html += '</div>';
		    html += '<hr />';
	    }

    	html += '<div class="btn-group btn-group-justified" role="group">';
    	html += '<div class="btn-group button-now-group" role="group"><button type="button" class="btn btn-sn now"><i class="glyphicon glyphicon-record"></i> Now</button></div>';
    	html += '<div class="btn-group" role="group"><button type="button" class="btn btn-primary ok"><i class="glyphicon glyphicon-ok"></i></button></div>';
    	html += '<div class="btn-group" role="group"><button type="button" class="btn btn-warning cancel"><i class="glyphicon glyphicon-remove"></i></button></div>';
    	html += '</div>';

    	this.popover().content(html);
    };
	
    this.popoverShown = function () {
    	this.contentElement().find('.go-prev-month').click($.proxy(this.goPrevMonth, this));
    	this.contentElement().find('.go-next-month').click($.proxy(this.goNextMonth, this));
    	this.contentElement().find('.year, .month, .hour, .minute, .second').change($.proxy(this.controlsChanged, this));
    	this.contentElement().find('.year, .hour, .minute, .second').bind('keypress keydown keyup', $.proxy(this.preventSubmit, this));
    	this.contentElement().find('table').on('click', '.day-this-month', $.proxy(this.dayClicked, this));
    	this.contentElement().find('button.now').click($.proxy(this.goNow, this));
    	this.contentElement().find('button.ok').click($.proxy(this.okClicked, this));
    	this.contentElement().find('button.cancel').click($.proxy(this.cancelClicked, this));

	    this.restorCalendar();
    };

    this.updateCalendar = function (value) {
        this.potentialValue = value;
		var now = moment();

		var startMoment = moment(value);
		startMoment.startOf('month');

		if (startMoment.day() == 0) {
		    startMoment.subtract(7, 'day');
		}
		else {
		    var daysBefore = startMoment.day();
		    startMoment.subtract(daysBefore, 'day');
		}

		for (var i = 1; i<= 42; i++) {
		    var isMonth = value.month() == startMoment.month();
		    var date = startMoment.date();
		    var today = startMoment.isSame(now, 'day');
		    var selectedDay = startMoment.isSame(value, 'day');

		    this.dayCell(i, date, isMonth, today, selectedDay);

		    startMoment.add(1, 'day');
		}
        
		this.year(value.format('YYYY'));
		this.month(value.format('M'));
		this.hour(value.format('HH'));
		this.minute(value.format('mm'));
		this.second(value.format('ss'));
	};

	this.restorCalendar = function() {
	    this.updateCalendar(this.potentialValue);
	};

	this.goPrevMonth = function () {
	    var newMoment = moment(this.potentialValue);

		newMoment.subtract(1, 'month');
		this.updateCalendar(newMoment);
	};

	this.goNextMonth = function() {
	    var newMoment = moment(this.valAsMoment());

		newMoment.add(1, 'month');
		this.updateCalendar(newMoment);
	};

	this.controlsChanged = function() {
	    var day = this.potentialValue.date();

		var newMoment = moment({ year: this.year(), month: this.month() - 1, day: day, hour: this.hour(), minute: this.minute(), second: this.second() });

        if (newMoment.isValid()) {		    
			this.updateCalendar(newMoment);
			return;
		}

		this.restorCalendar();
		return;
	};
	
	this.dayClicked = function (event) {
		var dayOfMonth = parseInt($(event.target).text());

		if (isNaN(dayOfMonth)) {
			return;
		}

		var newMoment = moment(this.potentialValue);

		newMoment.date(dayOfMonth);

		if (newMoment.isValid()) {
			this.updateCalendar(newMoment);
		}
	};

	this.okClicked = function () {
	    this.valAsMoment(this.potentialValue);
	};

	this.cancelClicked = function () {

	};

	this.goNow = function() {
		var now = moment();

		this.updateCalendar(now);

		return false;
	};

	this.popover = function () {
		return trooper.ui.registry.getPopover(this.popoverId);
	};

	this.bsPopover = function () {
		return this.popover().bsPopover();
	};

	this.contentElement = function() {
		return this.popover().contentElement();
	};

	this.dayCell = function (cellId, dayOfMonth, isCurrentMonth, isToday, isSelectedDay) {
		var cell = this.contentElement().find('.day-' + cellId);

		cell.text(dayOfMonth);
		
		if (isCurrentMonth) {
			cell.removeClass('day-other-month');
			cell.addClass('day-this-month');
		} else {
			cell.addClass('day-other-month');
			cell.removeClass('day-this-month');
		}

		if (isToday) {
		    cell.addClass('day-today');
		}
		else {
		    cell.removeClass('day-today');
		}

		if (isSelectedDay) {
		    cell.addClass('day-selected');
		}
		else {
		    cell.removeClass('day-selected');
		}

		return cell;
	};

	this.val = function(value) {
		if (arguments.length == 1) {
			var newValue = moment(value);

			if (newValue.isValid()) {
			    this.potentialValue = newValue;
				this.input(newValue.format(this.format()));
			}
		} else {
			var current = moment(this.input(), this.format());

			if (current.isValid()) {
				return current;
			}

			return '';
		}
	}

	this.valAsMoment = function (value) {
		if (arguments.length == 1) {
		    if (value._isAMomentObject && value.isValid()) {
		        this.potentialValue = value;
				this.input(value.format(this.format()));
				return;
			}
		} else {
			var current = moment(this.input(), this.format());

			if (current.isValid()) {
				return current;
			}

			return moment();
		}
	}

	this.input = function(value) {
		if (arguments.length == 1) {
			$('#' + this.id + ' input.datetime-input').val(value);
		} else {
			return $('#' + this.id + ' input.datetime-input').val();
		}
	}

	this.year = function(value) {
		if (arguments.length == 1) {
			this.contentElement().find('.year').val(value);
		} else {
			var y = parseInt(this.contentElement().find('.year').val());

			if (isNaN(y)) {
				return moment().year();
			}

			return y;
		}
	}

	this.month = function(value) {
		if (arguments.length == 1) {
			this.contentElement().find('.month').val(parseInt(value));
		} else {
			var m = parseInt(this.contentElement().find('.month').val());

			if (isNaN(m)) {
				return moment().month();
			}

			return m;
		}
	};

	this.hour = function (value) {
		if (arguments.length == 1) {
			this.contentElement().find('.hour').val(value);
		} else {
			var h = parseInt(this.contentElement().find('.hour').val());

			debugger;
			if (isNaN(h)) {
				return moment().hour();
			}

			return h;
		}
	};

	this.minute = function (value) {
		if (arguments.length == 1) {
			this.contentElement().find('.minute').val(value);
		} else {
			var m = this.contentElement().find('.minute').val();

			if (isNaN(m)) {
				return moment().minute();
			}

			return m;
		}
	};

	this.second = function (value) {
		if (arguments.length == 1) {
			this.contentElement().find('.second').val(value);
		} else {
			var s = this.contentElement().find('.second').val();

			if (isNaN(s)) {
				return moment().second();
			}

			return s;
		}
	};

	this.preventSubmit = function (event) {
		if (event.keyCode == 13) {
			event.preventDefault();
			return false;
		}
	};

	this.format = function() {
		switch (this.dateTimeFormat) {
			case 'DateAndTime':
				return 'DD-MM-YYYY HH:mm:ss';
			case 'Date':
				return 'DD-MM-YYYY';
			case 'DateTimeNoSeconds':
				return 'DD-MM-YYYY HH:mm';
			case 'Time':
				return 'HH:mm:ss';
			case 'TimeNoSeconds':
				return 'HH:mm';
		}
	};
	
	trooper.ui.registry.addControl(this.id, this, 'datetimepicker');
	$(document).ready($.proxy(this.init, this));

	return {
		date: $.proxy(this.date, this)
	};
});