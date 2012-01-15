var toph = toph || {};

toph.Editor = function (source) {
    this.source = source;
    this.day = source.attr('day');
    this.status = $('#status');

    this.ensureFocus();
};

toph.Editor.prototype = {
    ensureFocus: function () {
        var me = this;
        $(document).click(function smartFocus() { 
            if (!me.source.has(':focus')) me.source.focus(); 
        });

        this.source.focus();
    }
};

toph.Editor.init = function () {
    var editor = new toph.Editor($('#editable-entry'));
    
    var savers = {
        local: new toph.LocalSaver(editor),
        remote: new toph.RemoteSaver(editor)
    };

    var synchronizer = new toph.SaverSync(editor, savers);
};

$(document).ready(toph.Editor.init);