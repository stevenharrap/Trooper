trooper.ui.control.messagesPanel = (function (params)
{
    this.id = params.id;

    this.init = function () {
        $('#' + this.id + '.trooper.messages-panel button.close.close-bar').click(function () {
            $('.trooper.messages-panel').hide(500);
        });

        $('#' + this.id + '.trooper.messages-panel button.close.open-messages').click(function () {
            $('.trooper.messages-panel .messages').show(500);
            $('.trooper.messages-panel button.close.open-messages').hide(500);
            $('.trooper.messages-panel button.close.close-messages').show(500);
        });

        $('#' + this.id + '.trooper.messages-panel button.close.close-messages').click(function () {
            $('.trooper.messages-panel .messages').hide(500);
            $('.trooper.messages-panel button.close.close-messages').hide(500);
            $('.trooper.messages-panel button.close.open-messages').show(500);
        });
    };

    var publicResult = {
        id: trooper.utility.control.makeIdAccessor(this),
    };

    trooper.ui.registry.addControl(this.id, publicResult, 'messagespanel');
    $(document).ready($.proxy(this.init, this));

    return publicResult;
});