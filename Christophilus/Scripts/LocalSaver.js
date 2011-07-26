toph.LocalSaver = function (editor) {
    this.source = editor.source;
    this.day = editor.day;
    this.status = editor.status;
    this.dirtyKey = editor.day + '.dirty';
    this.versionKey = editor.day + '.v';

    this.version = this.loadVersion();
    this.trackChanges();
};

toph.LocalSaver.prototype = {
    save: function () {
        localStorage.setItem(this.day, this.source.val());
    },

    load: function () {
        this.source.val(localStorage.getItem(this.day));
    },

    loadVersion: function () {
        return localStorage.getItem(this.versionKey);
    },

    setVersion: function (v) {
        this.setDirtyFlag(false);
        localStorage.setItem(this.versionKey, v);
    },

    trackChanges: function () {
        var me = this;
        function isIgnorable(e) {
            return e.ctrlKey || e.which == 19 || e.which == 17 || e.which == 115;
        }

        this.source.keyup(function (e) {
            if (!isIgnorable(e)) {
                me.setDirtyFlag(true);
                me.save();
            }
        });
    },

    setDirtyFlag: function (isDirty) {
        if (isDirty) {
            this.status.text('Save');
            localStorage.setItem(this.dirtyKey, true);
        } else {
            this.status.text('Saved');
            localStorage.removeItem(this.dirtyKey);
        }
    },

    isDirty: function () {
        return localStorage.getItem(this.dirtyKey) != null;
    }
};