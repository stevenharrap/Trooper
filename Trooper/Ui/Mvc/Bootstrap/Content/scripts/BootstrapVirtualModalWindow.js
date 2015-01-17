function BootstrapVirtualModalWindow(params) {
    this.id = params.id;
    this.title = params.title;
    this.frameUrl = params.frameUrl;
    this.frameHeight = params.frameHeight;
    this.incCloseButton = params.incCloseButton;
    this.buttons = params.buttons;
    this.closeEvents = new Array();
    
    GetBootstrapHtml().addControl(this.id, this, 'virtualmodalwindow');
    $(document).ready($.proxy(this.init, this));
}

BootstrapVirtualModalWindow.prototype.id = null;

BootstrapVirtualModalWindow.prototype.buttons = null;

BootstrapVirtualModalWindow.prototype.frameUrl = null;

BootstrapVirtualModalWindow.prototype.frameHeight = 0;

BootstrapVirtualModalWindow.prototype.title = '';

/* list of events to execute when the window closes */
BootstrapVirtualModalWindow.prototype.closeEvents = null;

BootstrapVirtualModalWindow.prototype.virtualId = 'BootstrapVirtualModalWindow_actual';

BootstrapVirtualModalWindow.prototype.mainFocus = null;
BootstrapVirtualModalWindow.prototype.focusReturned = false;

BootstrapVirtualModalWindow.prototype.init = function() {
};

/**
* Makes a window but does not open it. You can open it with the bootstrap modal methods 
* or call openWindow. Returns a reference to the window.
*/
BootstrapVirtualModalWindow.prototype.makeWindow = function () {
    var result = "<div class=\"modal fade\" id=\"" + this.id
        + "\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"" + this.id + "_label\" aria-hidden=\"true\">\n"
        + "<div class=\"modal-dialog\">\n<div class=\"modal-content\">\n"
        + "<div class=\"modal-header\">\n";

    if (this.incCloseButton) {
        result += "<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>\n";
    }

    result += "<h4 class=\"modal-title\" id=\"" + this.id + "_label\">" + this.title + "</h4>\n</div>\n"
        + "<div class=\"modal-body\" style=\"margin:0; padding:0\">\n"
        + "<iframe style=\"width:100%; height:" + this.frameHeight
        + "px; padding:0\" frameborder=\"0\" src=\"" + this.frameUrl + "\">\n" + "</iframe>\n" + "</div>\n"
        + "<div class=\"modal-footer\">\n";

    if (this.incCloseButton) {
        result += "<button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">" +
            "<span class=\"glyphicon glyphicon-remove\"></span> Close\n</button>\n";
    }

    if (this.buttons != null) {
        for (var i = 0; i < this.buttons.length; i++) {
            result += this.buttons[i];
        }
    }

    result += "</div>\n</div>\n</div>\n</div>\n";

    $(result).appendTo("body");
    $('#' + this.id).on('hide.bs.modal', $.proxy(this.beforeDestroyWindow, this));
    $('#' + this.id).on('hidden.bs.modal', $.proxy(this.destroyWindow, this));

    return $('#' + this.id);
};

/*
* Finds the existing window and opens it or makes a new one and opens it. Returns a referrence to the window.
*/
BootstrapVirtualModalWindow.prototype.openWindow = function() {
    this.focusReturned = false;
    this.mainFocus = document.activeElement;

    var w = $('#' + this.id);

    if (w.length == 0) {
        w = this.makeWindow();
    }

    w.modal('show');

    return w;
};

/*
* Get the model window.
* You can then call .model(...) and other Bootstrap Modal window function
*/
BootstrapVirtualModalWindow.prototype.GetWindow = function() {
    return $('#' + this.id);
};

BootstrapVirtualModalWindow.prototype.GetFrame = function() {
    var iframe = $('#' + this.id + ' .modal-body iframe')[0];

    return iframe.contentWindow ? iframe.contentWindow : iframe.contentDocument.defaultView;
};

BootstrapVirtualModalWindow.prototype.closeWindow = function() {
    $('#' + this.id).modal('hide');
};

BootstrapVirtualModalWindow.prototype.beforeDestroyWindow = function () {
    //// This was necessary to ensure the main window gets focus back. Othewise 
    //// the iframe gobbles it and never gives back... eary. An IE thing in some versions.

    if (!this.focusReturned) {
        this.focusReturned = true;
        setTimeout($.proxy(this.forceFocus, this), 1000);

        this.GetFrame().src = 'about:blank';
    }
};

/*
 * Some instances of IE wont give the cursor back to the screen. We need to force it.
 */
BootstrapVirtualModalWindow.prototype.forceFocus = function () {
    var location = $(this.mainFocus);

    var tempTextArea = $('<textarea></textarea>');

    if (location != null && location.length > 0) {
        tempTextArea.insertAfter(location);
    } else {
        $(tempTextArea).appendTo("body");
    }
    
    tempTextArea.focus();
    tempTextArea.remove();
};

BootstrapVirtualModalWindow.prototype.destroyWindow = function () {
    $('#' + this.id).remove();

    for (var i = 0; i < this.closeEvents.length; i++) {
        this.closeEvents[i]();
    }
};