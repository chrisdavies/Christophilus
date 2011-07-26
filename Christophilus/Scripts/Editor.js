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
    },
    
    autoscroll: function (cloner) {
        var caretY = cloner.caretY();
        var sourcePos = cloner.source.position();
        window.scrollTo(sourcePos.left, sourcePos.top + caretY - ($(window).height() / 2));
    },

    autogrow: function (cloner) {
        var extraHeight = 60;
        cloner.source.height(cloner.height() + extraHeight);
    }
};

toph.Editor.init = function () {
    var editor = new toph.Editor($('#editable-entry'));
    
    var savers = {
        local: new toph.LocalSaver(editor),
        remote: new toph.RemoteSaver(editor)
    };

    var synchronizer = new toph.SaverSync(editor, savers);

    var cloner = new toph.TextCloner(editor);
    cloner.addbehavior(editor.autogrow);
    cloner.addbehavior(editor.autoscroll);
    cloner.copySource();
};

$(document).ready(toph.Editor.init);