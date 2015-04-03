/** 
* --------------------------------------------------------------------------------------
* <copyright file="upload.js" company="Trooper Inc">
*     Copyright (c) Trooper 2014 - Onwards
* </copyright>
* --------------------------------------------------------------------------------------
*/

//
// Turns a plain old upload form element into a Rabbit element 
//
trooper.ui.control.upload = (function (params)
{
    this.id = params.id;
    this.formId = params.formId;
    this.warnOnLeave = params.warnOnLeave;
    this.selectEvents = new Array();
    this.clearEvents = new Array();
   	
	this.init = function () {		
		this.getFakeBrowseButton().click($.proxy(this.browse, this));
		this.getClearButton().click($.proxy(this.clear, this));
	};
	
	this.addSelectEvent = function(func) {
		this.selectEvents.push(func);
	};
	
	this.addClearEvent = function(func) {
		this.clearEvents.push(func);
	};
	
	this.getIframe = function () {
		return $('#' + this.id + '_container iframe').contents();
	};

	this.getFakeBrowseButton = function () {
		return $('#' + this.id + '_container .browse');
	};

	this.getDisplay = function () {
		return $('#' + this.id + '_container .display');
	};

	this.getClearButton = function () {
		return $('#' + this.id + '_container .clear');
	};

	this.getRealBrowseButton = function () {
		return this.getIframe().find("#UploadedFile");
	};
	
	this.val = function (value) {
		if (arguments.length == 1) {
			$('#' + this.id).text(JSON.stringify(value));
		} else {
			return JSON.parse($('#' + this.id).text());
		}
	};	
	
	//
	// The browse button has been clicked so fire the real browse button
	//
	this.browse = function () {
		this.getRealBrowseButton().click();
		
		if (this.warnOnLeave) {
		    var form = trooper.ui.registry.getForm(this.formId);
			bsf.makeFormDirty();
		}
	};
	
	//
	// The user wants to delete the currently selected docuemnt (from the server)
	// so hide the button and indicate the document deletion should happen on the server
	//
	this.clear = function () {
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
			var form = trooper.ui.getForm(this.formId);
			bsf.makeFormDirty();
		}
		
		this.updateDisplay();
	};
	
	this.iFrameLoad = function () {
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
	
	this.updateDisplay = function () {
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
	
	this.executeSelectEvents = function() {
		for (var i = 0; i < this.selectEvents.length; i++) {
			this.selectEvents[i]();
		}
	};

	this.executeClearEvents = function() {
		for (var i = 0; i < this.clearEvents.length; i++) {
			this.clearEvents[i]();
		}
	};

	var publicResult = {
	    id: trooper.utility.control.makeIdAccessor(this),
	    addSelectEvent: $.proxy(this.addSelectEvent, this),
	    addClearEvent: $.proxy(this.addClearEvent, this),
	    val: $.proxy(this.val, this),
	    clear: $.proxy(this.clear, this),
	    iFrameLoad: $.proxy(this.iFrameLoad, this)
	};
	
	trooper.ui.registry.addControl(this.id, publicResult, 'upload');
	$(document).ready($.proxy(this.init, this));

    return publicResult;
});