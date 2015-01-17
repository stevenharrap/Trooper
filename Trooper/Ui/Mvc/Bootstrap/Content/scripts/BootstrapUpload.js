/** 
* --------------------------------------------------------------------------------------
* <copyright file="BootstrapUpload.js" company="Trooper Inc">
*     Copyright (c) Trooper 2014 - Onwards
* </copyright>
* --------------------------------------------------------------------------------------
*/

//
// Turns a plain old upload form element into a bootstrap element 
//
function BootstrapUpload(params) {
    this.id = params.id;
    this.formId = params.formId;
    this.warnOnLeave = params.warnOnLeave;
    this.selectEvents = new Array();
    this.clearEvents = new Array();
    
    GetBootstrapHtml().addControl(this.id, this, 'upload');
    $(document).ready($.proxy(this.init, this));
};

//
// The id of the upload element
//
BootstrapUpload.prototype.id = '';

BootstrapUpload.prototype.formId = '';

BootstrapUpload.prototype.warnOnLeave = true;

//
// Array of events
//
BootstrapUpload.prototype.selectEvents = null;

//
// Array of events
//
BootstrapUpload.prototype.clearEvents = null;


BootstrapUpload.prototype.getIframe = function () {
    return $('#' + this.id + '_container iframe').contents();
};

BootstrapUpload.prototype.getFakeBrowseButton = function () {
    return $('#' + this.id + '_container .browse');
};

BootstrapUpload.prototype.getDisplay = function () {
    return $('#' + this.id + '_container .display');
};

BootstrapUpload.prototype.getClearButton = function () {
    return $('#' + this.id + '_container .clear');
};

BootstrapUpload.prototype.getRealBrowseButton = function () {
    return this.getIframe().find("#UploadedFile");
};

BootstrapUpload.prototype.val = function (value) {
    if (arguments.length == 1) {
        debugger;
        $('#' + this.id).text(JSON.stringify(value));
    } else {
        return JSON.parse($('#' + this.id).text());
    }
};

BootstrapUpload.prototype.init = function () {
    this.getFakeBrowseButton().click($.proxy(this.browse, this));
    this.getClearButton().click($.proxy(this.clear, this));
};

//
// The browse button has been clicked so fire the real browse button
//
BootstrapUpload.prototype.browse = function () {
    this.getRealBrowseButton().click();
    
    if (this.warnOnLeave) {
        var bsf = GetBootstrapHtml().getForm(this.formId);
        bsf.makeFormDirty();
    }
};

//
// The user wants to delete the currently selected docuemnt (from the server)
// so hide the button and indicate the document deletion should happen on the server
//
BootstrapUpload.prototype.clear = function () {
    var data = this.val();
    
    if (data.PersistedId != '00000000-0000-0000-0000-000000000000' && data.PersistedId != '') {
        data.PersistedId = '';
        data.PersistedFilename = '';
    }
    else {
        data.CurrentDocumentDeleted = 'True';
    }

    this.val(data);
    
    if (this.warnOnLeave) {
        var bsf = GetBootstrapForm();
        bsf.makeFormDirty();
    }
    
    this.updateDisplay();
};

BootstrapUpload.prototype.iFrameLoad = function () {
    var persistedId = this.getIframe().find("#PersistedId").val();
    var displayFilename = this.getIframe().find("#DisplayFilename").val();

    if (persistedId != '00000000-0000-0000-0000-000000000000' && persistedId != '') {
        var data = this.val();

        data.PersistedId = persistedId;
        data.PersistedFilename = displayFilename;
        data.CurrentDocumentDeleted = 'False';

        this.val(data);
    }

    this.updateDisplay();
};

BootstrapUpload.prototype.updateDisplay = function () {
    //var persistId = this.getPersistId().val();
    //var persistName = this.getPersistName().val();
    //var deleted = this.getCurrentDocumentDeleted().val() == 'True';
    //var currentDocName = this.getCurrentDocumentName().val();
    //var currentDocUrl = this.CurrentDocumentUrl().val();

    var data = this.val();
    this.getDisplay().removeClass('pretend-link');
    this.getDisplay().click(function() { });

    if (data.PersistedId != '00000000-0000-0000-0000-000000000000' && data.PersistedId != '') {
        this.getDisplay().val(data.PersistedFilename);
        this.executeSelectEvents();
    }
    else if (data.CurrentDocumentFilename != '' && data.CurrentDocumentUrl != '' && !data.CurrentDocumentDeleted) {
        this.getDisplay().addClass('pretend-link');
        this.getDisplay().val(data.CurrentDocumentFilename);
        this.getDisplay().click(function () { window.open(data.CurrentDocumentUrl, '_blank'); });
        this.executeSelectEvents();
    }
    else if (data.CurrentDocumentFilename != '' && !data.CurrentDocumentDeleted) {
        this.getDisplay().val(data.CurrentDocumentFilename);
        this.executeSelectEvents();
    }
    else {
        this.getDisplay().val('');
        this.executeClearEvents();
    }
};

BootstrapUpload.prototype.executeSelectEvents = function() {
    for (var i = 0; i < this.selectEvents.length; i++) {
        this.selectEvents[i]();
    }
};

BootstrapUpload.prototype.executeClearEvents = function() {
    for (var i = 0; i < this.clearEvents.length; i++) {
        this.clearEvents[i]();
    }
};