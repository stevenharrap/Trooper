trooper.ui.control.selectList = (function (params)
{
    this.id = params.id;
    this.formId = params.formId;
    this.warnOnLeave = params.warnOnLeave;

	this.init = function () {
        if (this.warnOnLeave) {
            var form = trooper.ui.registry.getForm(this.formId);
            form.addVolatileField(this.id);
        }
    };

	var publicResult = {
	    id: trooper.utility.control.makeIdAccessor(this)
	};

	trooper.ui.registry.addControl(this.id, publicResult, 'selectlist');
    $(document).ready($.proxy(this.init, this));

    return publicResult;
});