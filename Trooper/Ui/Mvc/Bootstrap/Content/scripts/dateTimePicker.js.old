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

    this.init = function () {
        this.getSelectButton().click($.proxy(this.selectButtonClick, this));
        this.getDeleteButton().click($.proxy(this.deleteButtonClick, this));
        this.getDateTextbox().change($.proxy(this.dateTextboxChange, this));

        var value = this.getDateTextbox().val();
        this.lastGoodValue = value == '' ? null : value;

        this.getContainer().click($.proxy(this.insideClick, this));
        $('html').click($.proxy(this.outsideClick, this));
    };

    this.selectButtonClick = function () {
        if (this.popoverIsOpen) {
            this.hidePopover();
            return;
        }

        var value = this.getDateTextbox().val();
        var date = null;

        if (value != null) {
            date = this.formatStringToDate(this.format, value);

            if (date == null) {
                date = new Date();
            }
        }

        this.makeCalander(date);
    };

    this.deleteButtonClick = function () {
        this.getDateTextbox().val('');
    };

    this.insideClick = function (e) {
        e.stopPropagation();
    };

    this.outsideClick = function () {
        if (!this.popoverIsOpen) {
            return;
        }

        this.hidePopover();
    };

    this.dateTextboxChange = function () {
        if (this.getDateTextbox().val() == '') {
            return;
        }

        var date = this.date();

        if (date == null) {
            this.getDateTextbox().val(this.lastGoodValue);
            date = this.date();
        }

        this.date(date);
    };

    this.makeCalander = function (date) {
        var html = '';

        if (this.pickDate) {
            var startDayNumber = new Date(date.getFullYear(), date.getMonth(), 1).getDay();
            var lastMonth = new Date(
                date.getMonth() == 1 ? date.getFullYear() - 1 : date.getFullYear(),
                date.getMonth() == 1 ? 12 : date.getMonth() - 1,
                1);
            var nextMonth = new Date(
                date.getMonth() == 12 ? date.getFullYear() + 1 : date.getFullYear(),
                date.getMonth() == 12 ? 1 : date.getMonth() + 1,
                1);
            var numDays = this.daysInMonth(date);

            html += '<div class="input-group">' +
                '<a href="#" data-value="' + this.dateToString(lastMonth) + '" class="btn btn-default input-group-addon go-month"><i class="glyphicon glyphicon-arrow-left"></i></a>' +

                '<span class="month input-group-addon">' + this.getMonthName(date.getMonth()) + '</span>' +
                '<input class="year form-control" type="text" data-value="' + date + '" value="' + this.formatDateToString('yyyy', date) + '" />' +

                '<a href="#" data-value="' + this.dateToString(nextMonth) + '" class="btn input-group-addon btn-default go-month"><i class="glyphicon glyphicon-arrow-right"></i></a>' +

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

            html += '</tbody>';
            html += '<tr>';

            for (var i = 0; i < startDayNumber; i++) {
                html += '<td></td>';
            }

            // Border is a counter, initialize it with the number of "blank" days at the
            // start of the calendar.  Now each time we add a new date we'll do a modulus
            // 7 and check for 0 (remainder of border/7 = 0), if it's zero it's time to
            // make a new row.
            var border = startDayNumber;

            // For each day in the month, insert it into the calendar.
            for (i = 1; i <= numDays; i++) {
                var day = new Date(date);
                day.setDate(i);

                html += '<td class="day" data-value="' + this.dateToString(day) + '">' + i + '</td>';
                border++;
                if (((border % 7) == 0) && (i < numDays)) {
                    // Time to start a new row, if there are any days left.
                    html += '</tr><tr>';
                }
            }

            // All the days have been used up, so just pad empty days until the
            // end of the calendar.
            while ((border++ % 7) != 0) {
                html += '<td></td>';
            }

            // Finish the table.
            html += '</tr>';
            html += '</tbody>';
            html += '</table>';
        }

        if (this.pickTime) {
            var currentDate = this.date();
            var hour = currentDate == null ? 0 : currentDate.getHours();
            var minute = currentDate == null ? 0 : currentDate.getMinutes();
            var second = currentDate == null ? 0 : currentDate.getSeconds();

            html += '<div class="time input-prepend"><span class="add-on">Time</span>';

            html += '<select class="hours">';
            for (var h = 0; h < 24; h++) {
                html += '<option value="' + h + '"' + (h == hour ? ' selected="selected"' : '') + '>' + h + '</option>';
            }
            html += '</select>';

            html += '<span class="add-on dots"> : </span><select class="minutes">';
            for (var m = 0; m < 60; m++) {
                html += '<option value="' + m + '"' + (m == minute ? ' selected="selected"' : '') + '>' + m + '</option>';
            }
            html += '</select>';

            html += '<span class="add-on dots"> : </span><select class="seconds">';
            for (var s = 0; s < 60; s++) {
                html += '<option value="' + s + '"' + (s == second ? ' selected="selected"' : '') + '>' + s + '</option>';
            }
            html += '</select>';

            html += '</div>';
        }

        this.makePopover(html);

        this.getContainer().find('.go-month').click($.proxy(this.goMonth, this));
        this.getYearTextbox().change($.proxy(this.goYear, this));
        this.getContainer().find('.day').click($.proxy(this.selectDay, this));

        this.getContainer().find('.hours').change($.proxy(this.goHour, this));
        this.getContainer().find('.minutes').change($.proxy(this.goMinute, this));
        this.getContainer().find('.seconds').change($.proxy(this.goSecond, this));
    };

    this.goMonth = function (e) {
        var date = new Date($(e.currentTarget).attr('data-value'));

        this.makeCalander(date);

        return false;
    };

    this.date = function (value) {
        if (arguments.length == 1) {
            var result = this.formatDateToString(this.format, value);

            if (result != null) {
                this.lastGoodValue = this.getDateTextbox().val();
            }

            this.getDateTextbox().val(result);
        }
        else {
            var result = this.formatStringToDate(this.format, this.getDateTextbox().val());

            if (result == null) {
                result = this.formatStringToDate(this.format, this.lastGoodValue);
            }

            return result;
        }
    };

    this.goYear = function (e) {
        var year = $(e.currentTarget).val();
        year = parseInt(year);

        if (isNaN(year) || year < 1900 || year > 2500) {
            year = (new Date()).getFullYear();
        }

        var date = new Date($(e.currentTarget).attr('data-value'));

        this.makeCalander(new Date(year, date.getMonth(), date.getDay()));

        return false;
    };

    this.selectDay = function (e) {
        var date = new Date($(e.currentTarget).attr('data-value'));

        var currentDate = this.date();

        date.setHours(currentDate == null ? 0 : currentDate.getHours());
        date.setMinutes(currentDate == null ? 0 : currentDate.getMinutes());
        date.setSeconds(currentDate == null ? 0 : currentDate.getSeconds());

        this.getYearTextbox().val(this.formatDateToString('yyyy', date));
        this.date(date);

        this.hidePopover();
    };

    this.goHour = function () {
        var date = this.date();

        if (date == null) {
            return;
        }

        date.setHours(parseInt(this.getContainer().find('.hours').val()));

        this.date(date);
    };

    this.goMinute = function () {
        var date = this.date();

        if (date == null) {
            return;
        }

        date.setMinutes(parseInt(this.getContainer().find('.minutes').val()));

        this.date(date);
    };

    this.goSecond = function () {
        var date = this.date();

        if (date == null) {
            return;
        }

        date.setSeconds(parseInt(this.getContainer().find('.seconds').val()));

        this.date(date);
    };

    this.getMonthName = function (monthNumber) {
        var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

        return months[monthNumber];
    };

    this.daysInMonth = function (date) {
        return new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
    };

    this.getContainer = function () {
        return $('#' + this.id + '_helper');
    };

    this.makePopover = function (html) {
        if (!this.popoverIsOpen) {
            var title = this.pickDate && this.pickTime ? "Select a date and time.." : this.pickDate ? "Select a date..." : "select a time...";
            var popoverElement = this.popoverPlacement == 'left' ? this.getDateTextbox() : this.getSelectButton();

            popoverElement.popover('destroy');

            if (this.popoverPlacement == 'left') {
                popoverElement.popover({ title: title, html: true, content: html, placement: 'left', trigger: 'manual' });
            } else {
                popoverElement.popover({ title: title, html: true, content: html, placement: this.popoverPlacement, trigger: 'manual' });
            }

            popoverElement.popover('show');
        } else {
            this.getContainer().find('.popover-content').html(html);
        }

        this.popoverIsOpen = true;
    };

    this.hidePopover = function () {
        var popoverElement = this.popoverPlacement == 'left' ? this.getDateTextbox() : this.getSelectButton();

        if (this.popoverIsOpen) {
            popoverElement.popover('hide');
        }

        this.popoverIsOpen = false;
    };

    this.getSelectButton = function () {
        return this.getContainer().find("button.date-select");
    };

    this.getDeleteButton = function () {
        return this.getContainer().find("button.date-delete");
    };

    this.getDateTextbox = function () {
        return this.getContainer().find('input#' + this.id);
    };

    this.getYearTextbox = function () {
        return this.getContainer().find('input.year');
    };

    this.dataChanged = function () {
        if (this.warnOnLeave) {
            var form = trooper.ui.registry.getForm(this.formId);
            form.makeFormDirty();
        }
    };

    this.dateToString = function (date) {
        var ver = trooper.utility.browser.getBrowser();

        if (ver.browser == 'msie' && ver.version <= 8.0) {
            ////ie 8 or less
            var dir = this.timezone[0] == '-' ? 1 : -1;
            var time = this.timezone.replace('-', '').replace('+', '').split(':');
            var hours = time[0] * dir;
            var minutes = time[1] * dir;
            var result = new Date(date);

            result.setHours(date.getHours() + hours);
            result.setMinutes(date.getMinutes() + minutes);

            return this.formatDateToString('yyyy/MM/ddThh:mm:ss', result);
        }
        else if (ver.browser == 'msie' && ver.version > 8.0) {
            ////ie 9 or better
            return this.formatDateToString('yyyy-MM-ddThh:mm:ss' + this.timezone, date);
        }
        else if (ver.browser == 'safari') {
            return this.formatDateToString('yyyy/MM/dd hh:mm:ss GMT' + this.timezone.replace(':', ''), date);
        } else {
            ////other browser
            return this.formatDateToString('yyyy-MM-dd hh:mm:ss GMT' + this.timezone, date);
        }
    };

	this.formatDateToString = function (format, date) {	
		var keys = [
			'dd', 'MM', 'yy', 'yyyy', 'hh', 'mm', 'ss'
		];

		var result = format;

		for (var k = 0; k < keys.length; k++) {
			var key = keys[k];
			
			switch (key) {
				case 'dd':
					result = result.replace(key, this.padLeft(date.getDate(), 2, '0'));
					continue;
					
				case 'MM':
					result = result.replace(key, this.padLeft(date.getMonth() + 1, 2, '0'));
					continue;
					
				case 'yyyy':
					result = result.replace(key, date.getFullYear() < 1900 ? date.getFullYear() + 1900 : date.getFullYear());
					continue;
					
				case 'hh':
					result = result.replace(key, this.padLeft(date.getHours(), 2, '0'));
					continue;
					
				case 'mm':
					result = result.replace(key, this.padLeft(date.getMinutes(), 2, '0'));
					continue;
					
				case 'ss':
					result = result.replace(key, this.padLeft(date.getSeconds(), 2, '0'));
					continue;
			}
		}

		return result;
	};
	
	this.formatStringToDate = function (format, dateString) {
		if (dateString == null || dateString == '') {
			return null;
		}

		var defaultDate = new Date();

		var year = this.getDatePartValue(format, 'yyyy', dateString);
		var month = this.getDatePartValue(format, 'MM', dateString);
		var day = this.getDatePartValue(format, 'dd', dateString);
		var hour = this.getDatePartValue(format, 'hh', dateString);
		var minute = this.getDatePartValue(format, 'mm', dateString);
		var second = this.getDatePartValue(format, 'ss', dateString);

		if (isNaN(year) || isNaN(month) || isNaN(day) || isNaN(hour) || isNaN(minute) || isNaN(second)) {
			return null;
		}

		var result = new Date(
			year == -1 ? defaultDate.getFullYear() : year,
			month == -1 ? defaultDate.getMonth() : month - 1,
			day == -1 ? defaultDate.getDate() : day,
			hour == -1 ? defaultDate.getHours() : hour,
			minute == -1 ? defaultDate.getMinutes() : minute,
			second == -1 ? defaultDate.getSeconds() : second);

		return result;
	};
	
	this.getDatePartValue = function (format, part, dateString) {
		var pos = format.indexOf(part);

		if (pos == -1) {
			return -1;
		}

		var raw = dateString.substr(pos, part.length);

		if (raw.length != part.length) {
			return NaN;
		}

		var value = parseInt(raw);

		return value;
	};
	
	this.padLeft = function(s, l, c) {
		if (l < s.toString().length) {
			return s;
		}

		return Array(l - s.toString().length + 1).join(c || ' ') + s.toString();
	};
	
	trooper.ui.registry.addControl(this.id, this, 'datetimepicker');
	$(document).ready($.proxy(this.init, this));

	return {
		date: $.proxy(this.date, this)
	};
});