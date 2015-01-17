function BootstrapNumericBox(params) {
    this.id = params.id;
    this.formId = params.formId;
    this.numericType = params.numericType;
    this.warnOnLeave = params.warnOnLeave;

    this.minimum = parseFloat(params.minimum);

    if (isNaN(this.minimum)) {
        this.minimum = null;
    }

    this.maximum = parseFloat(params.maximum);

    if (isNaN(this.maximum)) {
        this.maximum = null;
    }

    this.decimalDigits = parseFloat(params.decimalDigits);

    if (isNaN(this.decimalDigits) || this.decimalDigits < 0) {
        this.decimalDigits = null;
    }

    GetBootstrapHtml().addControl(this.id, this, 'numericbox');
    $(document).ready($.proxy(this.init, this));
};

BootstrapTextbox.prototype.id = '';

BootstrapTextbox.prototype.formId = '';

BootstrapTextbox.prototype.numericType = '';

BootstrapTextbox.prototype.minimum = null;

BootstrapTextbox.prototype.maximum = null;

BootstrapTextbox.prototype.decimalDigits = null,

BootstrapTextbox.prototype.warnOnLeave = true;

BootstrapTextbox.prototype.getInput = function () {
    return $('#' + this.id);
};

BootstrapTextbox.prototype.init = function () {
    this.getInput().change($.proxy(this.checkInput, this));

    if (this.warnOnLeave) {
        var bsf = GetBootstrapHtml().getForm(this.formId);
        bsf.addVolatileField(this.inputId);
    }
};

BootstrapTextbox.prototype.val = function (value) {
    if (arguments.length == 1) {
        this.getInput().val(value);
        this.checkInput();
    }

    return this.getInput().val();
};

BootstrapTextbox.prototype.checkInput = function () {
    var value = this.getInput().val().toString();

    switch (this.numericType) {
        case 'Integer':
            value = this.numericParse(value, false, this.decimalDigits, this.minimum, this.maximum);

            break;
        case 'Decimal':
        case 'Currency':
        case 'Percentage':
            value = this.numericParse(value, true, this.decimalDigits, this.minimum, this.maximum);
            break;
    }

    this.getInput().val(value);
};

BootstrapTextbox.prototype.numericParse = function (value, allowDecimal, decimalDigits, minimum, maximum) {
    value = value.toString();
    var result = '';
    var foundDecimal = false;

    for (var p = 0; p < value.length; p++) {
        var c = value[p];

        switch (c) {
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                result += c;
                break;

            case '.':
                if (!foundDecimal) {
                    result += '.';
                    foundDecimal = true;
                }
                break;

            case '-':
            case '+':
                if (p == 0) {
                    result += c;
                }
        }
    }

    if (result == '') {
        result = 0;
    }

    result = parseFloat(result);

    if (isNaN(result)) {
        result = 0;
    }

    if (minimum != null && result < minimum) {
        result = minimum;
    }

    if (maximum != null && result > maximum) {
        result = maximum;
    }

    if (result == null) {
        result = 0;
    }

    if (allowDecimal) {
        if (decimalDigits != null) {
            result = result.toFixed(decimalDigits);
        }

        return result;
    }

    return parseInt(Math.round(parseFloat(result)));
};