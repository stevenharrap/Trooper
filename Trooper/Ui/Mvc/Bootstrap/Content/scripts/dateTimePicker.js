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
		    '<a href="#" class="btn btn-default input-group-addon go-prev-month"><i class="glyphicon glyphicon-arrow-left"></i></a>' +
		    '<span class="month input-group-addon"></span>' +
		    '<input class="year form-control" type="text" />' +
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
				html += '<td class="day day-'+ i +'"></td>';
				i++;
			}

			html += '</tr>';
		}
    	
    	html += '</tbody>';
    	html += '</table>';
    	html += '<button class="btn btn-sn btn-block">Now</button>';

    	this.popover().content(html);
    };
	
    this.popoverShown = function () {
    	this.contentElement().find('.go-prev-month').click($.proxy(this.goPrevMonth, this));
    	this.contentElement().find('.go-next-month').click($.proxy(this.goNextMonth, this));
    	this.contentElement().find('.year').change($.proxy(this.yearChanged, this));

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
		
		this.contentElement().find('.month').text(value.format('MMMM'));
		this.year(value.format('YYYY'));
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

	this.yearChanged = function() {
		var newMoment = this.valAsMoment();

		newMoment.year(this.year());

		if (newMoment.isValid()) {
			this.updateCalendar(newMoment);
			return;
		}

		this.restorCalendar();
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

	this.dayCell = function (cellId, text, isCurrentMonth) {
		var cell = this.contentElement().find('.day-' + cellId);

		cell.text(text);
		
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