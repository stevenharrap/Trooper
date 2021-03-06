﻿trooper.ui.control.form = (function (params) {
    this.id = params.id;
    this.formIsDirty = false;

    this.init = function ()
    {        
        $('#' + this.id).click($.proxy(this.clicked, this));
    };

    /**
     * Indicates that the field (which can be a field id or jquery object) is volatile
     * and if it changes and the user has not saved the current record then an alert message
     * should be raised if the user attempts to leave the page.
     */
    this.addVolatileField = function (field) {
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

    this.makeFormDirty = function () {
        this.formIsDirty = true;
    };

    this.makeFormClean = function () {
        this.formIsDirty = false;
    };

    this.canLeave = function () {
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

    this.onSubmit = function (func) {
        return $('#' + this.id).submit(func);
    };

    this.submit = function (action) {
        if (action != null) {
            $('#' + this.id).attr('action', this.url);
        }

        $('#' + this.id).submit();
    };

    var publicResult = {
        id: trooper.utility.control.makeIdAccessor(this),
        addVolatileField: $.proxy(this.addVolatileField, this),
        makeFormDirty: $.proxy(this.makeFormDirty, this),
        makeFormClean: $.proxy(this.makeFormClean, this),
        canLeave: $.proxy(this.canLeave, this),
        onSubmit: $.proxy(this.onSubmit, this),
        submit: $.proxy(this.submit, this)
    };

    trooper.ui.registry.addControl(this.id, publicResult, 'form');
    $(document).ready($.proxy(this.init, this));
});