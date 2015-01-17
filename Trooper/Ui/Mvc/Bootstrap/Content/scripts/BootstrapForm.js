function BootstrapForm(params) {
    this.id = params.id;

    $(document).ready($.proxy(this.init, this));
}

BootstrapForm.prototype.formIsDirty = false;

BootstrapForm.prototype.init = function() {
    GetBootstrapHtml().addControl(this.id, this, 'form');
};

BootstrapForm.prototype.id = '';

/**
 * Indicates that the field (which can be a field id or jquery object) is volatile
 * and if it changes and the user has not saved the current record then an alert message
 * should be raised if the user attempts to leave the page.
 */
BootstrapForm.prototype.addVolatileField = function (field) {
    var element;

    if (typeof field == 'string') {
        element = $('#' + field);
    } else {
        element = $(field);
    }
    
    if (element != null && element.length > 0) {
        element.change($.proxy(this.makeFormDirty, this));
    }
};

BootstrapForm.prototype.makeFormDirty = function() {
    this.formIsDirty = true;
};

BootstrapForm.prototype.makeFormClean = function () {
    this.formIsDirty = false;
};

BootstrapForm.prototype.canLeave = function () {
    if (this.formIsDirty) {
        var msg = 'You have changed one or more fields but you have not saved your changes.\n';
        msg += '- If this is fine then click Ok.\n';
        msg += '- If not fine, click cancel and save your changes.\n';
        
        if (confirm(msg)) {
            return true;
        }

        return false;
    }

    return true;
};

