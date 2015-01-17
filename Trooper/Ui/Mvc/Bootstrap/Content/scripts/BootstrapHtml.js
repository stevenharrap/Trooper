function GetBootstrapHtml() {
    return window['bootstrapHtml'];
}

function BootstrapHtml() {
    this.screenModeChangeEvents = new Array();
    this.controls = new Array();

    $(document).ready($.proxy(this.init, this));
}

BootstrapHtml.prototype.init = function () {
    this.currentScreenMode = this.getScreenMode();

    $(window).resize($.proxy(this.screenModeChange, this));
};

/*
 * Provides the last recorded screen mode before the screen.
 */ 
BootstrapHtml.prototype.currentScreenMode = '';

/*
 * The array of events that will be fired when the screen mode changes.
 * To add your own use "addScreenModeEvent".
 */
BootstrapHtml.prototype.screenModeChangeEvents = null;

BootstrapHtml.prototype.controls = null;

BootstrapHtml.prototype.addControl = function (controlId, controlObj, controlType) {
    this.controls.push({ id: controlId, obj: controlObj, type: controlType });
};

BootstrapHtml.prototype.getControl = function (controlId, controlType) {
    for (var i = 0; i < this.controls.length; i++) {
        if (this.controls[i].id == controlId && this.controls[i].type == controlType) {
            return this.controls[i].obj;
        }
    }

    return null;
};

BootstrapHtml.prototype.getForm = function (controlId) {
    return this.getControl(controlId, "form");
};

BootstrapHtml.prototype.getButton = function(controlId) {
    return this.getControl(controlId, "button");
};

BootstrapHtml.prototype.getCheckBox = function (controlId) {
    return this.getControl(controlId, "checkbox");
};

BootstrapHtml.prototype.getCheckBoxList = function (controlId) {
    return this.getControl(controlId, "checkboxlist");
};

BootstrapHtml.prototype.getDateTimePicker = function (controlId) {
    return this.getControl(controlId, "datetimepicker");
};

BootstrapHtml.prototype.getNumericBox = function (controlId) {
    return this.getControl(controlId, "numericbox");
};

BootstrapHtml.prototype.getPanelGroup = function (controlId) {
    return this.getControl(controlId, "pannelgroup");
};

BootstrapHtml.prototype.getRadioList = function (controlId) {
    return this.getControl(controlId, "radiolist");
};

BootstrapHtml.prototype.getSearchBox = function (controlId) {
    return this.getControl(controlId, "searchbox");
};

BootstrapHtml.prototype.getSelectList = function (controlId) {
    return this.getControl(controlId, "selectlist");
};

BootstrapHtml.prototype.getTable = function (controlId) {
    return this.getControl(controlId, "table");
};

BootstrapHtml.prototype.getTextareaBox = function (controlId) {
    return this.getControl(controlId, "textareabox");
};

BootstrapHtml.prototype.getTextbox = function (controlId) {
    return this.getControl(controlId, "textbox");
};

BootstrapHtml.prototype.getUpload = function (controlId) {
    return this.getControl(controlId, "upload");
};

BootstrapHtml.prototype.getVirtualModelWindow = function (controlId) {
    return this.getControl(controlId, "virtualmodalwindow");
};


/**
 * Blackens the main screen and puts up a message.
 * Call this from a button or form submission event to prevent double clicking.
 */
BootstrapHtml.prototype.openLoadingScreen = function (message) {
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

/*
 * Use this to add your own functions that should be fired when the screen mode
 * changes. The new screen mode will be passed to the function.
 * Screen modes are: "ExtraSmall", "Small", "Medium", "Large", "Print"
 */
BootstrapHtml.prototype.addScreenModeEvent = function (func) {
    if (this.screenModeChangeEvents == null) {
        this.screenModeChangeEvents = new Array();
    }

    this.screenModeChangeEvents.push(func);
};

/*
 * Get the screen mode regardless of what "currentScreenMode" is.
 * "ExtraSmall", "Small", "Medium", "Large", "Print"
 */
BootstrapHtml.prototype.getScreenMode = function() {
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

/*
 * This fires when the screen mode changes.
 */
BootstrapHtml.prototype.screenModeChange = function() {
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

/**
 * Closes the loading screen opened by 'openLoadingScreen'
 */
BootstrapHtml.prototype.closeLoadingScreen = function () {
    $('#loading-window').modal('hide');
};

/**
 * Allows you to add a paramater to a url or replace it if the parameter alread exists
 */
BootstrapHtml.prototype.addParamToUrl = function (url, paramName, paramValue) {
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
BootstrapHtml.prototype.removeParamFromUrl = function (url, paramName) {
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
BootstrapHtml.prototype.getUrlAsComponents = function (urlQuery) {
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
BootstrapHtml.prototype.convertComponentsToUrl = function (urlAsComponents) {
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
BootstrapHtml.prototype.combineUrlComponents = function(urlAsComponentsSouce, urlAsComponentsTarget, limitToFields) {
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