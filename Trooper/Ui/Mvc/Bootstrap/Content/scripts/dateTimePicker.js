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
				html += '<td class="day-'+ i +'"></td>';
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

    	var now = moment();

	    this.updateCalendar(now);
    };

	this.updateCalendar = function(value) {
		var lastMonth = moment(value).subtract(1, 'month');
		lastMonth.endOf('month');
		var lastWeekDayLastMonth = lastMonth.day();

		var nextMonth = moment(value).add(1, 'month');
		nextMonth.set('date', 1);
		var firstWeekDayNextMonth = nextMonth.day();

		this.contentElement().find('.month').text(value.format('MMMM'));

		this.valAsMoment(value);
	};

	this.goPrevMonth = function () {
		var current = this.valAsMoment();

		if (current == null) {
			current = moment();
		}

		current.subtract(1, 'month');
		this.updateCalendar(current);
	};

	this.goNextMonth = function() {
		var current = this.valAsMoment();

		if (current == null) {
			current = moment();
		}

		current.add(1, 'month');
		this.updateCalendar(current);
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

	this.val = function(value) {
		if (arguments.length == 1) {
			var newValue = moment(value);

			if (newValue.isValid()) {
				$('#' + this.id).val(newValue.format());
			}
		} else {
			var current = moment($('#' + this.id).val());

			if (current.isValid()) {
				return current;
			}

			return '';
		}
	}

	this.valAsMoment = function (value) {
		if (arguments.length == 1) {
			if (value._isAMomentObject && value.isValid()) {
				$('#' + this.id).val(value.format());
				return;
			}
		} else {
			var current = moment($('#' + this.id).val());

			if (current.isValid()) {
				return current;
			}

			return null;
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