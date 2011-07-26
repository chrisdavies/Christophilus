toph.RemoteSaver = function (editor) {
    this.source = editor.source;
    this.day = editor.day;
    this.status = editor.status;
    this.saveAction = this.source.attr('save-action');
    this.isSaving = false;
    this.version = editor.source.attr('version');

    this.waitForSave();
};

toph.RemoteSaver.prototype = {
    waitForSave: function () {
        var me = this;
        $(document).keydown(function (e) {
            return me.processKeyCommand(e);
        });

        me.status.click(function saveClicked() {
            me.save();
            return false;
        });
    },

    processKeyCommand: function (e) {
        var isSaveCommand = this.isCtrlS(e);
        if (isSaveCommand) {
            this.save();
        }

        return !isSaveCommand;
    },

    isCtrlS: function (e) {
        return e.ctrlKey && e.keyCode == 'S'.charCodeAt(0);
    },

    save: function () {
        if (this.isSaving) {
            return;
        }

        this.status.text('Saving...');

        var journalEntry = {
            Body: this.source.val(),
            Day: this.day
        };

        this.saveJournalEntry(journalEntry);
    },

    saveJournalEntry: function (journalEntry) {
        var me = this;
        $.ajax({
            type: 'POST',
            url: this.saveAction,
            dataType: 'json',
            contentType: 'application/json; charset=utf=8',
            timeout: 10000,
            data: JSON.stringify(journalEntry),
            success: function (data) {
                me.isSaving = false;
                me.status.text('Saved');
                $(me).trigger('saved', [data]);
            },
            error: function (request, status, err) {
                me.isSaving = false;
                me.status.text(err);
            }
        });
    }
};