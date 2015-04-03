trooper.ui.control.textBox = (function (params)
{
    this.id = params.id;
    this.formId = params.formId;
    this.maxLength = params.maxLength;
    this.warnOnLeave = params.warnOnLeave;
   
    this.init = function () {
		this.getInput().change($.proxy(this.checkInput, this));
		
        if (this.warnOnLeave) {
            var form = trooper.ui.registry.getForm(this.formId);
            form.addVolatileField(this.id);
        }
    };
	
	this.getInput = function () {
		return $('#' + this.id);
	};

	this.val = function(value) {
		if (arguments.length == 1) {
			this.getInput().val(value);
		}

		return this.getInput().val();
	};
	
	var publicResult = {
	    id: trooper.utility.control.makeIdAccessor(this)
	};

	trooper.ui.registry.addControl(this.id, publicResult, 'textbox');
	$(document).ready($.proxy(this.init, this));

    return publicResult;
});