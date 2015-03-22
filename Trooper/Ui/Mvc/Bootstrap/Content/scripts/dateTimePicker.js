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
    this.currentMonth = null;
	
    this.init = function () {
    	this.popover().content('<div></div>');
    	this.bsPopover().on('show.bs.popover', $.proxy(this.popoverShow, this));
    	this.bsPopover().on('shown.bs.popover', $.proxy(this.popoverShown, this));        
    };

    this.popoverShow = function () {
        var html = '';
        this.potentialValue = this.val() == null ? moment() : moment(this.val());
        this.currentMonth = moment(this.potentialValue);

        if (this.dateTimeFormat.indexOf('Date') > -1) {
		    html += '<div class="input-group">' +
			    '<a href="#" class="btn btn-default input-group-addon go-prev-month"><i class="glyphicon glyphicon-arrow-left"></i></a>';

		    html += '<select class="month form-control" style="width:50%">';
		    for (var m = 0; m < 12; m++) {
			    html += '<option value="' + (m + 1) + '">' + (moment().month(m).format('MMM')) + '</option>';
		    }
		    html += '</select>';

		    html += '<input class="year form-control" type="text" style="width:50%" maxlength="4" />' +
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

    	this.contentElement().find('.year').bind('keypress keydown keyup', $.proxy(this.yearChanged, this));
    	this.contentElement().find('.month').change($.proxy(this.monthChanged, this));
    	this.contentElement().find('.hour').bind('keypress keydown keyup',$.proxy(this.hourChanged, this));
    	this.contentElement().find('.minute').bind('keypress keydown keyup', $.proxy(this.minuteChanged, this));
    	this.contentElement().find('.second').bind('keypress keydown keyup', $.proxy(this.secondChanged, this));

    	this.contentElement().find('table').on('click', '.day-this-month', $.proxy(this.dayClicked, this));
    	this.contentElement().find('button.now').click($.proxy(this.goNow, this));
    	this.contentElement().find('button.ok').click($.proxy(this.okClicked, this));
    	this.contentElement().find('button.cancel').click($.proxy(this.cancelClicked, this));

    	this.updateCalendar();
    };

    this.updateCalendar = function () {
		var now = moment();

		var startMoment = moment(this.currentMonth);
		startMoment.startOf('month');

		if (startMoment.day() == 0) {
		    startMoment.subtract(7, 'day');
		}
		else {
		    var daysBefore = startMoment.day();
		    startMoment.subtract(daysBefore, 'day');
		}

		for (var i = 1; i<= 42; i++) {
		    var isMonth = this.currentMonth.month() == startMoment.month();
		    var date = startMoment.date();
		    var isToday = this.datesEqual(startMoment, now);
		    var isSelectedDay = this.datesEqual(this.potentialValue, startMoment);

		    if (isToday) {
		        debugger;
		    }

		    if (isSelectedDay) {
		        debugger;
		    }

		    this.dayCell(i, date, isMonth, isToday, isSelectedDay);

		    startMoment.add(1, 'day');
		}
        
		this.year(this.currentMonth.format('YYYY'));
		this.month(this.currentMonth.format('M'));
		this.hour(this.potentialValue.format('HH'));
		this.minute(this.potentialValue.format('mm'));
		this.second(this.potentialValue.format('ss'));
	};

	this.goPrevMonth = function () {
	    this.currentMonth.subtract(1, 'month');
		this.updateCalendar();
	};

	this.goNextMonth = function () {
	    this.currentMonth.add(1, 'month');
		this.updateCalendar();
	};

	this.yearChanged = function (event) {
	    if (event.keyCode == 8 || event.keyCode == 46) {
	        return;
	    }

	    this.currentMonth.year(this.year());
	    this.updateCalendar();

	    if (event.keyCode == 13) {
	        //event.preventDefault();
	        return false;
	    }
	};

	this.monthChanged = function (event) {
	    this.currentMonth.month(this.month() - 1);
	    this.updateCalendar();
	};

	this.hourChanged = function (event) {
	    if (event.keyCode == 8 || event.keyCode == 46) {
	        return;
	    }

	    this.potentialValue.hour(this.hour());
	    this.updateCalendar();

	    if (event.keyCode == 13) {
	        //event.preventDefault();
	        return false;
	    }
	};

	this.minuteChanged = function (event) {
	    if (event.keyCode == 8 || event.keyCode == 46) {
	        return;
	    }

	    this.potentialValue.minute(this.minute());
	    this.updateCalendar();

	    if (event.keyCode == 13) {
	        //event.preventDefault();
	        return false;
	    }
	};

	this.secondChanged = function () {
	    if (event.keyCode == 8 || event.keyCode == 46) {
	        return;
	    }

	    this.potentialValue.second(this.second());
	    this.updateCalendar();

	    if (event.keyCode == 13) {
	        //event.preventDefault();
	        return false;
	    }
	};	
	
	this.dayClicked = function (event) {
	    var dayOfMonth = parseInt($(event.target).text());
	    debugger;

		if (isNaN(dayOfMonth)) {
			return;
		}

		this.potentialValue = moment(this.currentMonth);
		this.potentialValue.date(dayOfMonth);
		
		this.updateCalendar();
	};

	this.okClicked = function () {
	    this.val(this.potentialValue.format());
	    this.popover().close();
	};

	this.cancelClicked = function () {
	    this.popover().close();
	};

	this.goNow = function() {
		this.currentMonth = moment();
		this.updateCalendar();
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

			var result = newValue.isValid() ? newValue.format(this.format()) : '';
			$('#' + this.id + ' input.datetime-input').val(result);
		} else {
		    var raw = $('#' + this.id + ' input.datetime-input').val();

		    var current = moment(raw, this.format());
		    return current.isValid() ? current : null;
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

			if (isNaN(h)) {
				return moment().hour();
			}

			if (h < 0 || h > 23) {
			    return this.potentialValue.hour();
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

			if (m < 0 || m > 59) {
			    return this.potentialValue.minute();
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

			if (s < 0 || s > 59) {
			    return this.potentialValue.second();
			}

			return s;
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

	this.datesEqual = function (moment1, moment2) {
	    return moment1.isSame(moment2, 'day') && moment1.isSame(moment2, 'month') && moment1.isSame(moment2, 'year');
	};
	
	trooper.ui.registry.addControl(this.id, this, 'datetimepicker');
	$(document).ready($.proxy(this.init, this));

	return {
		date: $.proxy(this.date, this)
	};
});