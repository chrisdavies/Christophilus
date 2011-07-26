toph.TextCloner = function (editor) {
    this.source = editor.source;

    this.createTarget();
    this.bindTargetToSource();
};

toph.TextCloner.prototype = {
    createTarget: function () {
        var pre = $(['<pre class="resizer-pre">',
          '<span class="precaret" /><span class="postcaret" />',
          '</pre>'].join(''));
        pre.insertAfter(this.source);

        this.target = pre;
        this.precaret = $('span.precaret', pre);
        this.postcaret = $('span.postcaret', pre);
    },

    bindTargetToSource: function () {
        var me = this;

        this.source.bind('keydown keyup paste change', function () {
            me.copySource();
        });
    },

    copySource: function () {
        var caretIndex = this.source.get(0).selectionStart;
        var sourceText = this.source.val();
        this.copyTo(this.precaret, sourceText, 0, caretIndex);
        this.copyTo(this.postcaret, sourceText, caretIndex);
        $(this).trigger('cloned');
    },

    copyTo: function (target, text, start, end) {
        if (end === undefined) {
            end = text.length;
        }

        if (start >= end) {
            target.text('');
        } else {
            target.text(text.substr(start, end - start));
        }
    },

    addbehavior: function (callback) {
        var me = this;
        $(this).bind('cloned', function () {
            callback(me);
        });
    },

    height: function () {
        return this.target.height();
    },

    caretY: function () {
        return this.precaret.height();
    }
};