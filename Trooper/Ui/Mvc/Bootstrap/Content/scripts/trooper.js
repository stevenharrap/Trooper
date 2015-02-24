var trooper = {};

trooper.utility = {};

trooper.utility._browser = (function() {
	// returns { browser: 'name', version: number }
	// browser can be: opera | chrome | safari | firefox | msie
	this.getBrowser = function () {
		var result = new Object();
		result.browser = '';
		result.version = '';
		
		var ua = navigator.userAgent;
		var m = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\.?\d+(\.\d+)*)/i);

		if (/trident/i.test(m[1])) {
			var b = /\brv[ :]+(\d+(\.\d+)?)/g.exec(ua);
			result.browser = 'msie';
			result.version = (b[1] || '');

		} else {
			var tem;
			if (m && (tem = ua.match(/version\/([\.\d]+)/i)) != null) {
				m[2] = tem[1];
			}

			m = m ? [m[1], m[2]] : [navigator.appName, navigator.appVersion, '-?'];
			
			result.browser = m[0].toLowerCase();
			result.version = parseFloat(m[1]);
		}
		
		return result;
	};
});

trooper.utility._url = (function ()
{
    /**
     * Allows you to add a paramater to a url or replace it if the parameter alread exists
     */
    this.addParamToUrl = function (url, paramName, paramValue) {
        var urlComponents = this.getUrlAsComponents(url);
        var found = false;

        for (var i = 0; i < urlComponents.params.length; i++) {
            if (urlComponents.params[i].name.toLowerCase() == paramName.toLowerCase()) {
                urlComponents.params[i].value = paramValue;
                found = true;
                break;
            }
        }

        if (!found) {
            urlComponents.params.push({ name: paramName, value: encodeURIComponent(paramValue) });
        }

        return this.convertComponentsToUrl(urlComponents);
    };

    /**
     * Allows you to remove a paramater from a url
     */
    this.removeParamFromUrl = function (url, paramName) {
        var urlComponents = this.getUrlAsComponents(url);

        var newParams = new Array();

        for (var i = 0; i < urlComponents.params.length; i++) {
            if (urlComponents.params[i].name.toLowerCase() != paramName.toLowerCase()) {
                newParams.push(urlComponents.params[i]);
            }
        }

        urlComponents.params = newParams;

        return this.convertComponentsToUrl(urlComponents);
    };

    /**
     * Takes a url in the form location?name=value&name2=value2 etc
     * and returns it as an array of { url: 'http//...', params: Array{ {name: '', value: ''} }, hash: 'placeonpage' }
     */
    this.getUrlAsComponents = function (urlQuery) {
        var result = { url: '', params: new Array(), hash: '' };

        var hash = urlQuery.split('#');
        if (hash.length == 0) {
            return result;
        }

        if (hash.length == 2) {
            result.hash = hash[1];
        }

        var parts = hash[0].split('?');

        if (parts.length == 0) {
            return result;
        }

        result.url = parts[0];

        if (parts.length == 1) {
            return result;
        }

        var keyValues = parts[1].split('&');

        for (var i = 0; i < keyValues.length; i++) {
            var pair = keyValues[i].split('=');

            if (pair.length == 0) {
                continue;
            }

            var name = pair[0];
            var value = pair.length < 2 ? '' : pair[1];

            result.params.push({ name: name, value: value });
        }

        return result;
    };

    /*
     * Converts the output of getUrlAsComponents back to a url
     */
    this.convertComponentsToUrl = function (urlAsComponents) {
        var actionParams = new Array();

        for (var r = 0; r < urlAsComponents.params.length; r++) {
            actionParams.push(urlAsComponents.params[r].name + '=' + urlAsComponents.params[r].value);
        }

        var result = urlAsComponents.url + '?' + actionParams.join("&");

        if (urlAsComponents.hash != '') {
            result += '#' + urlAsComponents.hash;
        }

        return result;
    };

    /**
     * The name/value params from the source are copied into the target.
     * Where the param already exists in the target the existing value is updated.
     * The fields from the source can be limited to the names (strings) in limitToFields.
     */
    this.combineUrlComponents = function (urlAsComponentsSouce, urlAsComponentsTarget, limitToFields) {
        for (var a = 0; a < urlAsComponentsSouce.params.length; a++) {

            if (limitToFields != null) {
                var found = false;
                for (var l = 0; l < limitToFields.length; l++) {
                    if (limitToFields[l] == urlAsComponentsSouce.params[a].name) {
                        found = true;
                    }
                }

                if (!found) {
                    continue;
                }
            }


            var exists = false;
            for (var f = 0; f < urlAsComponentsTarget.params.length && !exists; f++) {
                if (urlAsComponentsTarget.params[f].name == urlAsComponentsSouce.params[a].name) {
                    exists = true;
                    urlAsComponentsTarget.params[f].value = urlAsComponentsSouce.params[a].value;
                }
            }

            if (!exists) {
                urlAsComponentsTarget.params.push({ name: urlAsComponentsSouce.params[a].name, value: urlAsComponentsSouce.params[a].value });
            }
        }

        return;
    };

    return {
        addParamToUrl: $.proxy(this.addParamToUrl, this),
        removeParamFromUrl: $.proxy(this.removeParamFromUrl, this),
        getUrlAsComponents: $.proxy(this.getUrlAsComponents, this),
        convertComponentsToUrl: $.proxy(this.convertComponentsToUrl, this),
        combineUrlComponents: $.proxy(this.combineUrlComponents, this),
    };
});

trooper.ui = {};

trooper.ui._registry = (function () {
    this.controls = new Array();

    this.addControl = function (controlId, controlObj, controlType) {
        this.controls.push({ id: controlId, obj: controlObj, type: controlType });
    };

    this.getControl = function (controlId, controlType) {
        for (var i = 0; i < this.controls.length; i++) {
            if (this.controls[i].id == controlId && this.controls[i].type == controlType) {
                return this.controls[i].obj;
            }
        }

        return null;
    };

    this.getForm = function (controlId) {
        return this.getControl(controlId, "form");
    };

    this.getButton = function (controlId) {
        return this.getControl(controlId, "button");
    };

    this.getCheckBox = function (controlId) {
        return this.getControl(controlId, "checkbox");
    };

    this.getCheckBoxList = function (controlId) {
        return this.getControl(controlId, "checkboxlist");
    };

    this.getDateTimePicker = function (controlId) {
        return this.getControl(controlId, "datetimepicker");
    };

    this.getNumericBox = function (controlId) {
        return this.getControl(controlId, "numericbox");
    };

    this.getPanelGroup = function (controlId) {
        return this.getControl(controlId, "pannelgroup");
    };

    this.getRadioList = function (controlId) {
        return this.getControl(controlId, "radiolist");
    };

    this.getSearchBox = function (controlId) {
        return this.getControl(controlId, "searchbox");
    };

    this.getSelectList = function (controlId) {
        return this.getControl(controlId, "selectlist");
    };

    this.getTable = function (controlId) {
        return this.getControl(controlId, "table");
    };

    this.getTextareaBox = function (controlId) {
        return this.getControl(controlId, "textareabox");
    };

    this.getTextbox = function (controlId) {
        return this.getControl(controlId, "textbox");
    };

    this.getUpload = function (controlId) {
        return this.getControl(controlId, "upload");
    };

    this.getVirtualModelWindow = function (controlId) {
        return this.getControl(controlId, "virtualmodalwindow");
    };

    this.getPopover = function (controlId) {
    	return this.getControl(controlId, "popover");
    };

    return {
        addControl: $.proxy(this.addControl, this),
        getControl: $.proxy(this.getControl, this),
        getForm: $.proxy(this.getForm, this),
        getButton: $.proxy(this.getButton, this),
        getCheckBox: $.proxy(this.getCheckBox, this),
        getCheckBoxList: $.proxy(this.getCheckBoxList, this),
        getDateTimePicker: $.proxy(this.getDateTimePicker, this),
        getNumericBox: $.proxy(this.getNumericBox, this),
        getPanelGroup: $.proxy(this.getPanelGroup, this),
        getRadioList: $.proxy(this.getRadioList, this),
        getSearchBox: $.proxy(this.getSearchBox, this),
        getSelectList: $.proxy(this.getSelectList, this),
        getTable: $.proxy(this.getTable, this),
        getTextareaBox: $.proxy(this.getTextareaBox, this),
        getTextbox: $.proxy(this.getTextbox, this),
        getUpload: $.proxy(this.getUpload, this),
        getVirtualModelWindow: $.proxy(this.getVirtualModelWindow, this)
    };
});
 
trooper.ui._html = (function ()
{
    this.currentScreenMode = '';

    this.screenModeChangeEvents = new Array();
    
    this.init = function () {
        this.currentScreenMode = this.getScreenMode();

        $(window).resize($.proxy(this.screenModeChange, this));
    };

    /*
     * Use this to add your own functions that should be fired when the screen mode
     * changes. The new screen mode will be passed to the function.
     * Screen modes are: "ExtraSmall", "Small", "Medium", "Large", "Print"
     */
    this.addScreenModeChangeEvent = function (func) {
        this.screenModeChangeEvents.push(func);
    };

    /*
     * This fires when the screen mode changes.
     */
    this.screenModeChange = function () {
        if (this.screenModeChangeEvents != null) {
            var size = this.getScreenMode();

            if (size == this.currentScreenMode) {
                return;
            }

            this.currentScreenMode = size;

            for (var i = 0; i < this.screenModeChangeEvents.length; i++) {
                this.screenModeChangeEvents[i](size);
            }
        }
    }

    /*
     * Get the screen mode regardless of what "currentScreenMode" is.
     * "ExtraSmall", "Small", "Medium", "Large", "Print"
     */
    this.getScreenMode = function () {
        var envs = ["ExtraSmall", "Small", "Medium", "Large", "Print"];
        var envValues = ["xs", "sm", "md", "lg", "print"];

        var el = $('<div>');
        el.appendTo($('body'));

        for (var i = envValues.length - 1; i >= 0; i--) {
            var envVal = envValues[i];

            el.addClass('hidden-' + envVal);
            if (el.is(':hidden')) {
                el.remove();
                return envs[i];
            }
        };

        return '';
    };    

    /**
     * Blackens the main screen and puts up a message.
     * Call this from a button or form submission event to prevent double clicking.
     */
    this.openLoadingScreen = function (message) {
        var loadingWindow = null;

        message = arguments.length == 0 || message == null || message == '' ? 'Working...' : message;

        var s = '<div class="modal fade" id="loading-window" tabindex="-1" role="dialog" aria-labelledby="loading-window_label" aria-hidden="true">\n'
            + "<div class='modal-dialog'>\n"
            + "<div class='modal-content'>\n"
            + "<div class='modal-header'>\n"
            + "<h4 class='modal-title' id='loading-window_label'>" + message + "</h4>\n"
            + "</div>\n"
            + "<div class='modal-body'>\n"
            + '<div class="progress progress-striped active">'
            + '<div class="progress-bar"  role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%">'
            + '<span class="sr-only">100% Complete</span>'
            + '</div>'
            + '</div>'
            + "</div>\n</div>\n</div>\n</div>\n";

        if ($('#loading-window').length == 0) {
            loadingWindow = $(s);
        }

        if (loadingWindow != null) {
            loadingWindow.modal({ backdrop: 'static', keyboard: false, show: true });
        }
    };

    /**
     * Closes the loading screen opened by 'openLoadingScreen'
     */
    this.closeLoadingScreen = function () {
        $('#loading-window').modal('hide');
    };

    $(document).ready($.proxy(this.init, this));

    return
    {
        addScreenModeChangeEvent = $.proxy(this.addScreenModeChangeEvent, this),
        screenMode = $.proxy(this.screenMode, this),        
        openLoadingScreen = $.proxy(this.openLoadingScreen, this),
        closeLoadingScreen = $.proxy(this.closeLoadingScreen, this)
    };

    
});

trooper.ui.control = {};

trooper.utility.browser = new trooper.utility._browser();
trooper.utility.url = new trooper.utility._url();
trooper.ui.registry = new trooper.ui._registry();
trooper.ui.html = new trooper.ui._html();