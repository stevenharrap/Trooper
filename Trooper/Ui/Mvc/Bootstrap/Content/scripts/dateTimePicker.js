trooper.ui.control.dateTimePicker = (function (params) {
    this.id = params.id;
    this.formId = params.formId;
    this.dateTimeFormat = params.dateTimeFormat;
    this.warnOnLeave = params.warnOnLeave;
    this.popoverPlacement = params.popoverPlacement;
    this.utcOffset = params.utcOffset;
    this.popoverId = params.popoverId;
    this.minimum = params.minimum;
	this.maximum = params.maximum;
    this.potentialValue = null;
    this.currentMonth = null;
	
    this.init = function () {
    	this.popover().content('<div></div>');
    	this.bsPopover().on('show.bs.popover', $.proxy(this.popoverShow, this));
    	this.bsPopover().on('shown.bs.popover', $.proxy(this.popoverShown, this));
    	$('#' + this.id + ' .date-delete').click($.proxy(this.deleteClicked, this));
    	$('#' + this.id + ' .datetime-input').attr('title', 'Entry format is ' + this.clientFormat());

	    $('#' + this.id + ' .datetime-input').inputmask(
		    this.mask(),
		    {
		    	'oncomplete': $.proxy(this.userInputChanged, this),
		    	'onincomplete': $.proxy(this.userInputChanged, this),
		    	'oncleared': $.proxy(this.userInputCleared, this)
		    });

	    var val = this.minimum == null ? null : moment(this.minimum, this.serverFormat());
	    this.minimum = val == null ? null : this.minimum;

	    val = this.maximum == null ? null : moment(this.maximum, this.serverFormat());
	    this.maximum = val == null ? null : this.maximum;

	    this.resetUserInput();
    };

    this.popoverShow = function () {
        var html = '';
        this.potentialValue = this.valAsMomentOrNow();
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

    	this.contentElement().find('.year').bind('change keydown keyup paste', $.proxy(this.yearChanged, this));
    	this.contentElement().find('.month').change($.proxy(this.monthChanged, this));
    	this.contentElement().find('.hour').bind('keypress keydown keyup',$.proxy(this.hourChanged, this));
    	this.contentElement().find('.minute').bind('keypress keydown keyup', $.proxy(this.minuteChanged, this));
    	this.contentElement().find('.second').bind('keypress keydown keyup', $.proxy(this.secondChanged, this));

    	this.contentElement().find('table').on('click', '.day-is-selectable', $.proxy(this.dayClicked, this));
    	this.contentElement().find('button.now').click($.proxy(this.goNow, this));
    	this.contentElement().find('button.ok').click($.proxy(this.okClicked, this));
    	this.contentElement().find('button.cancel').click($.proxy(this.cancelClicked, this));

    	this.disableSelection();

    	var current = this.valAsMomentOrNow();
        
    	this.updateCalendar();
    	this.updateTime(current);
    };

    this.updateCalendar = function () {
		var now = this.newMoment();

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
		    var isSelectable = isMonth;

		    this.dayCell(i, date, isMonth, isToday, isSelectedDay, isSelectable);

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
		    event.preventDefault();
	        return false;
	    }

	    var val = this.year();

	    if (val == null) {
	        return true;
	    }

	    var test = moment(this.currentMonth);
	    test.year(val);

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

	    var form = trooper.ui.registry.getForm(this.formId);
	    form.makeFormDirty();

	    this.valAsMoment(this.potentialValue);
		this.resetUserInput();
	    this.popover().close();
	};

	this.okEnabled = function (enable) {
	    var button = this.contentElement().find('button.ok');

	    button.prop("disabled", !enable);
	};

	this.deleteClicked = function () {
		var form = trooper.ui.registry.getForm(this.formId);
		form.makeFormDirty();

		this.valAsMoment(null);
		this.resetUserInput();
	};	

	this.cancelClicked = function () {
	    this.popover().close();
	};

	this.goNow = function() {
		this.currentMonth = this.newMoment();
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

	this.dayCell = function (cellId, dayOfMonth, isCurrentMonth, isToday, isSelectedDay, isSelectable) {
		var cell = this.contentElement().find('.day-' + cellId);

		cell.text(dayOfMonth);
		
		if (isCurrentMonth) {
			cell.removeClass('day-other-month');
			cell.addClass('day-this-month');
		} else {
			cell.addClass('day-other-month');
			cell.removeClass('day-this-month');
		}

		if (isSelectable) {
			cell.addClass('day-is-selectable');
		}
		else {
			cell.removeClass('day-is-selectable');
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
    
	this.rawVal = function (value) {
		if (arguments.length == 1) {
			$('#' + this.id + ' input[type="hidden"]').val(value);
		} else {
			var raw = $('#' + this.id + ' input[type="hidden"]').val();

			return raw;
		}
	};

	this.userInput = function (value) {
		if (arguments.length == 1) {
			$('#' + this.id + ' .datetime-input').val(value);
		} else {
			return $('#' + this.id + ' .datetime-input').text();
		}
	}

	this.resetUserInput = function()
	{
		var val = this.valAsMoment();

		if (val == null) {
			this.userInput('');
			return;
		}

		this.userInput(val.format(this.clientFormat()));
	};

	this.userInputChanged = function () {
		var val = moment(this.userInput(), this.clientFormat());

		if (!val.isValid()) {
			this.resetUserInput();
			return;
		}

		var format = this.clientFormat();
		var result = this.valAsMomentOrNow();

		if (format.indexOf('YYYY') > -1) {
			result.year(val.year());
		}

		if (format.indexOf('MM') > -1) {
			result.month(val.month());
		}

		if (format.indexOf('DD') > -1) {
			result.date(val.date());
		}

		if (format.indexOf('HH') > -1) {
			result.hour(val.hour());
		}

		if (format.indexOf('mm') > -1) {
			result.minute(val.minute());
		}

		if (format.indexOf('ss') > -1) {
			result.second(val.second());
		}

		this.valAsMoment(result);
	};

	this.userInputCleared = function () {
		this.valAsMoment(null);
	};

	this.valAsServerFormat = function (value) {
		if (arguments.length == 1) {
			if (value == null || value == '') {
				this.rawVal('');
			}

	        var newValue = moment(value, this.serverFormat());

	        if (newValue.isValid()) {
	        	this.valAsMoment(newValue);
	        }
	    } else {
	        var result = this.valAsMoment();

	        if (result != null) {
	            return result.format(this.serverFormat());
	        }

	        return '';
	    }
	}

	this.valAsMoment = function (value) {
		if (arguments.length == 1) {
			if (moment.isMoment(value) && value.isValid()) {
				var result = value.format(this.serverFormat());
				this.rawVal(result);
			} else {
				this.rawVal('');
			}
		} else {
	        var raw = this.rawVal();

	        var current = moment(raw, this.serverFormat());
	        return current.isValid() ? current : null;
	    }
	}

	this.valAsMomentOrNow = function () {
	    var m = this.valAsMoment();

	    if (m != null) {
	        return m;
	    }

	    return this.newMoment();
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

	this.newMoment = function () {
	    var result = moment();

	    result.utcOffset(this.utcOffset);

	    return result;
	};

	this.clientFormat = function() {
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

	this.serverFormat = function () {
	    return 'YYYY-MM-DD HH:mm:ss';
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
	    val: $.proxy(this.val, this),
	    valAsServerFormat: $.proxy(this.valAsServerFormat, this),
	    valAsMoment: $.proxy(this.valAsMoment, this),
	};

	trooper.ui.registry.addControl(this.id, publicResult, 'datetimepicker');
	$(document).ready($.proxy(this.init, this));

	return publicResult;
});