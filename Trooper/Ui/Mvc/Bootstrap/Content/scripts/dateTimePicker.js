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
    	$('#' + this.id + ' .date-delete').click($.proxy(this.deleteClicked, this));
    	$('#' + this.id + ' .datetime-input').inputmask(this.mask());
    	$('#' + this.id + ' .datetime-input').attr('title', 'Entry format is ' + this.format());
    };

    this.popoverShow = function () {
        var html = '';
        this.potentialValue = this.val() == null ? moment() : moment(this.val());
        this.currentMonth = moment(this.potentialValue);

        html += '<div class="format-' + this.dateTimeFormat + '">';

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

		    if (this.dateTimeFormat.indexOf('NoSeconds') == -1) {
		        html += '<span class="input-group-addon dots"> : </span>';
		        html += '<input type="text" maxlength="2" class="second form-control" />';

		    }

		    html += '</div>';
		    html += '<hr />';
	    }

    	html += '<div class="btn-group btn-group-justified" role="group">';
    	html += '<div class="btn-group button-now-group" role="group"><button type="button" class="btn btn-sn now"><i class="glyphicon glyphicon-record"></i> Now</button></div>';
    	html += '<div class="btn-group" role="group"><button type="button" class="btn btn-primary ok"><i class="glyphicon glyphicon-ok"></i></button></div>';
    	html += '<div class="btn-group" role="group"><button type="button" class="btn btn-warning cancel"><i class="glyphicon glyphicon-remove"></i></button></div>';
    	html += '</div>';

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

    	this.disableSelection();

    	var current = this.val() == null ? moment() : moment(this.val(), this.format());
        
    	this.updateCalendar(current);
    	this.updateTime(current);
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

		    this.dayCell(i, date, isMonth, isToday, isSelectedDay);

		    startMoment.add(1, 'day');
		}
        
		this.year(this.currentMonth.year());
		this.month(this.currentMonth.month());
    };

    this.updateTime = function (aMoment) {
        this.hour(aMoment.hour());
        this.minute(aMoment.minute());
        this.second(aMoment.second());
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
	    if (event.keyCode == 13) {
	        return false;
	    }            

	    var test = moment(this.currentMonth);
	    test.year(this.year());

	    if (test.isValid()) {
	        this.currentMonth = test;
	        this.updateCalendar();
	    }
	};

	this.monthChanged = function (event) {
	    this.currentMonth.month(this.month() - 1);
	    this.updateCalendar();
	};

	this.hourChanged = function (event) {	    
	    if (event.keyCode == 13) {
	        return false;
	    }

	    this.okEnabled(this.hour() != null);
	};

	this.minuteChanged = function (event) {
	    if (event.keyCode == 13) {
	        return false;
	    }

	    this.okEnabled(this.minute() != null);
	};

	this.secondChanged = function () {	    
	    if (event.keyCode == 13) {
	        return false;
	    }

	    this.okEnabled(this.second() != null);
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
	    if (this.hour() != null) {
	        this.potentialValue.hour(this.hour());
	    }

	    if (this.minute() != null) {
	        this.potentialValue.minute(this.minute());
	    }

	    if (this.second() != null) {
	        this.potentialValue.second(this.second());
	    }

	    this.val(this.potentialValue.format());
	    this.popover().close();
	};

	this.okEnabled = function (enable) {
	    var button = this.contentElement().find('button.ok');

	    button.prop("disabled", !enable);
	};

	this.deleteClicked = function () {
	    this.val('');
	};	

	this.cancelClicked = function () {
	    this.popover().close();
	};

	this.goNow = function() {
		this.currentMonth = moment();
		this.updateCalendar();
		this.updateTime(this.currentMonth);
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

			if (isNaN(y) || y < 0 || y > 9999)
			{
				return null;
			}

			return y;
		}
    }
    
    this.month = function(value) {
		if (arguments.length == 1) {
			this.contentElement().find('.month').val(parseInt(value));
		} else {
			var m = parseInt(this.contentElement().find('.month').val());

			if (isNaN(m) || m < 1 || m > 12) {
				return null;
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
			    return null;
			}

			if (h < 0) {
			    this.hour(0);
			    return 0;
			}

			if (h > 23) {
			    this.hour(23);
			    return 23;
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
			    return null;
			}

			if (isNaN(m)) {
			    return null;
			}

			if (m < 0) {
			    this.minute(0);
			    return 0;
			}

			if (m > 59) {
			    this.minute(59);
			    return 59;
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
			    return null;
			}

			if (s < 0) {
			    this.second(0);
			    return 0;
			}

			if (s > 59) {
			    this.second(59);
			    return 59;
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
    
	this.mask = function () {
	    switch (this.dateTimeFormat) {
	        case 'DateAndTime':
	            return '99-99-9999 99:99:99';
	        case 'Date':
	            return '99-99-9999';
	        case 'DateTimeNoSeconds':
	            return '99-99-9999 99:99';
	        case 'Time':
	            return '99:99:99';
	        case 'TimeNoSeconds':
	            return '99:99';
	    }
	};

	this.datesEqual = function (moment1, moment2) {
	    return moment1.isSame(moment2, 'day') && moment1.isSame(moment2, 'month') && moment1.isSame(moment2, 'year');
	};

	this.disableSelection = function() {
	    this.contentElement().attr('unselectable', 'on');
	    this.contentElement().css('user-select', 'none');
	    this.contentElement().on('selectstart', false);
	};
	
	var publicResult = {
	    id: trooper.utility.control.makeIdAccessor(this),
	    date: $.proxy(this.date, this)
	};

	trooper.ui.registry.addControl(this.id, publicResult, 'datetimepicker');
	$(document).ready($.proxy(this.init, this));

	return publicResult;
});