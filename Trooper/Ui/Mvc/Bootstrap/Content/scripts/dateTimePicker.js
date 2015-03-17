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
    	//this.popover().content('<div></div>');
    	//this.popover().ignoreSelectors(new Array(".jquery-ui-datetimepicker", ".trooper-ui-dtp-component"));
    	this.bsPopover().on('show.bs.popover', $.proxy(this.popoverShow, this));
    	this.bsPopover().on('shown.bs.popover', $.proxy(this.popoverShown, this));        
    };

    this.popoverShow = function () {
	    var html = '';
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
    	
    	for (var w = 0; w < 6; w++) {
    		html += '<tr>';

			for (var d = 0; d < 7; d++) {
				html += '<td></td>';
			}

			html += '</tr>';
		}
    	
    	html += '</tbody>';
    	html += '</table>';

	    this.popover().content(html);
    };

	this.popoverShown = function() {
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