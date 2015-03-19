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
    	//this.popover().ignoreSelectors(new Array(".jquery-ui-datetimepicker", ".trooper-ui-dtp-component"));
    	this.bsPopover().on('show.bs.popover', $.proxy(this.popoverShow, this));
    	this.bsPopover().on('shown.bs.popover', $.proxy(this.popoverShown, this));        
    };

    this.popoverShow = function () {
    	var html = '';

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

	    html += '<div class="time input-group">';
    	html += '<span class="input-group-addon">Time</span>';
    	html += '<input type="text" maxlength="2" class="hour form-control" />';
    	html += '<span class="input-group-addon dots"> : </span>';
    	html += '<input type="text" maxlength="2" class="minute form-control" />';
    	html += '<span class="input-group-addon dots"> : </span>';
    	html += '<input type="text" maxlength="2" class="second form-control" />';
    	html += '</div>';

	    html += '<hr />';
    	html += '<button class="btn btn-sn btn-block now">Now</button>';

    	this.popover().content(html);
    };
	
    this.popoverShown = function () {
    	this.contentElement().find('.go-prev-month').click($.proxy(this.goPrevMonth, this));
    	this.contentElement().find('.go-next-month').click($.proxy(this.goNextMonth, this));
    	this.contentElement().find('.year, .month, .hour, .minute, .second').change($.proxy(this.controlsChanged, this));
    	this.contentElement().find('.year, .hour, .minute, .second').bind('keypress keydown keyup', $.proxy(this.preventSubmit, this));
    	this.contentElement().find('table').on('click', '.day-this-month', $.proxy(this.dayClicked, this));
    	this.contentElement().find('button.now').click($.proxy(this.goNow, this));

	    this.restorCalendar();
    };

	this.updateCalendar = function(value) {
		this.valAsMoment(value);

		var prevMonthLastWeekDay = moment(value);
		prevMonthLastWeekDay.subtract(1, 'month');
		prevMonthLastWeekDay.endOf('month');

		var lastDayOfthisMonth = moment(value);
		lastDayOfthisMonth.endOf('month');
		var thisMonthDay = 1;

		var daysBefore = prevMonthLastWeekDay.date() - prevMonthLastWeekDay.day();
		var nextMonthDay = 1;

		for (var i = 1; i <= 7; i++) {
			if (i > prevMonthLastWeekDay.day() + 1) {
				this.dayCell(i, thisMonthDay++, true);

			} else {
				this.dayCell(i, daysBefore++, false);
			}
		}

		for (var i = 8; i <= 42; i++) {
			if (thisMonthDay <= lastDayOfthisMonth.date()) {
				this.dayCell(i, thisMonthDay++, true);
			} else {
				this.dayCell(i, nextMonthDay++, false);
			}
		}

		this.year(value.format('YYYY'));
		this.month(value.format('M'));
		this.hour(value.format('HH'));
		this.minute(value.format('mm'));
		this.second(value.format('ss'));
	};

	this.restorCalendar = function() {
		this.updateCalendar(this.valAsMoment());
	};

	this.goPrevMonth = function () {
		var newMoment = this.valAsMoment();

		newMoment.subtract(1, 'month');
		this.updateCalendar(newMoment);
	};

	this.goNextMonth = function() {
		var newMoment = this.valAsMoment();

		newMoment.add(1, 'month');
		this.updateCalendar(newMoment);
	};

	this.controlsChanged = function() {
		var current = this.valAsMoment();

		var day = current.date();

		var newMoment = moment({ year: this.year(), month: this.month() - 1, day: day, hour: this.hour(), minute: this.minute(), second: this.second() });

		if (newMoment.isValid()) {
			debugger;
			this.updateCalendar(newMoment);
			return;
		}

		this.restorCalendar();
		return;
	};
	
	this.dayClicked = function (event) {
		debugger;
		var dayOfMonth = parseInt($(event.target).text());

		if (isNaN(dayOfMonth)) {
			return;
		}

		var newMoment = this.valAsMoment();

		newMoment.date(dayOfMonth);

		if (newMoment.isValid()) {
			this.updateCalendar(newMoment);
		}
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

	this.dayCell = function (cellId, dayOfMonth, isCurrentMonth) {
		var cell = this.contentElement().find('.day-' + cellId);

		cell.text(dayOfMonth);
		
		if (isCurrentMonth) {
			cell.removeClass('day-other-month');
			cell.addClass('day-this-month');
		} else {
			cell.addClass('day-other-month');
			cell.removeClass('day-this-month');
		}

		return cell;
	};

	this.val = function(value) {
		if (arguments.length == 1) {
			var newValue = moment(value);

			if (newValue.isValid()) {
				this.input(newValue.format());
			}
		} else {
			var current = moment(this.input());

			if (current.isValid()) {
				return current;
			}

			return '';
		}
	}

	this.valAsMoment = function (value) {
		if (arguments.length == 1) {
			if (value._isAMomentObject && value.isValid()) {
				this.input(value.format());
				return;
			}
		} else {
			var current = moment(this.input());

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
			return this.contentElement().find('.year').val();
		}
	}

	this.month = function(value) {
		if (arguments.length == 1) {
			this.contentElement().find('.month').val(parseInt(value));
		} else {
			return parseInt(this.contentElement().find('.month').val());
		}
	};

	this.hour = function (value) {
		if (arguments.length == 1) {
			this.contentElement().find('.hour').val(value);
		} else {
			return parseInt(this.contentElement().find('.hour').val());
		}
	};

	this.minute = function (value) {
		if (arguments.length == 1) {
			this.contentElement().find('.minute').val(value);
		} else {
			return parseInt(this.contentElement().find('.minute').val());
		}
	};

	this.second = function (value) {
		if (arguments.length == 1) {
			this.contentElement().find('.second').val(value);
		} else {
			return parseInt(this.contentElement().find('.second').val());
		}
	};

	this.preventSubmit = function (event) {
		if (event.keyCode == 13) {
			event.preventDefault();
			return false;
		}
	};



	/*this.popoverShow = function () {
    	$('body').append('<div id=\"' + this.id + '_tempArea\"></div>');
    	var element = $('#' + this.id + '_tempArea');
    	element.datetimepicker();
    	var width = element.find('.ui-datepicker-inline').width();
    	var height = element.find('.ui-datepicker-inline').height();
    	element.remove();

    	this.popover().content('<div style="width:' + width + 'px; height:' + height + 'px" class=\"jquery-ui-datetimepicker\"></div>');        
    };

    this.popoverShown = function () {
        $('#' + this.id + ' .jquery-ui-datetimepicker').datetimepicker(
            {
                onChangeMonthYear: $.proxy(this.afterRender, this),
                onSelect: $.proxy(this.afterRender, this),
                prevText: '',
                nextText: ''
            });
        
        this.afterRender();
        //this.popover().contentElement().css('padding', 0);
    };

    this.popover = function () {
    	return trooper.ui.registry.getPopover(this.popoverId);
    };

    this.bsPopover = function () {
        return this.popover().bsPopover();
    };

    this.afterRender = function () {
        setTimeout($.proxy(this.fixStyling, this), 10);
    };

    this.fixStyling = function () {
        $('#' + this.id + ' .jquery-ui-datetimepicker *').addClass('trooper-ui-dtp-component');

        $('#' + this.id + ' .ui-icon-circle-triangle-w').addClass('glyphicon glyphicon-chevron-left');
        $('#' + this.id + ' .ui-icon-circle-triangle-w').removeClass('ui-icon ui-icon-circle-triangle-w');
        $('#' + this.id + ' .ui-icon-circle-triangle-e').addClass('glyphicon glyphicon-chevron-right')
        $('#' + this.id + ' .ui-icon-circle-triangle-e').removeClass('ui-icon ui-icon-circle-triangle-e');
    };*/
	
	trooper.ui.registry.addControl(this.id, this, 'datetimepicker');
	$(document).ready($.proxy(this.init, this));

	return {
		date: $.proxy(this.date, this)
	};
});