toph.SaverSync = function (editor, savers) {
    this.local = savers.local;
    this.remote = savers.remote;

    this.syncLocalVersion();
    this.syncVersionsOnSave();
};

toph.SaverSync.prototype = {
    syncLocalVersion: function () {
        if (this.shouldUploadLocalCopy()) {
            this.local.load();
            this.remote.save();
        } else {
            this.local.save();
            this.local.setVersion(this.remote.version);
        }
    },

    syncVersionsOnSave: function () {
        var me = this;
        $(this.remote).bind('saved', function (e, data) {
            me.local.setVersion(data.version);
        });
    },

    shouldUploadLocalCopy: function () {
        var localVersion = this.local.version;
        return (localVersion && localVersion === this.remote.version && this.local.isDirty());
    }
};