﻿var toph = toph || {};

toph.Editor = function (element) {
  this.element = element;
  this.saveAction = element.attr('save-action');
  this.day = element.attr('day');
  this.remoteVersion = element.attr('version');

  this.makeEditable();
  this.enableRemoteSaves();
  this.syncLocalVersion();
};

toph.Editor.prototype = {
  // Makes the editable-entry div editable.
  makeEditable: function () {
    var me = this;
    $(document).click(function () { me.element.focus(); });
    this.element.focus();
    this.element.keyup(function () { me.saveToLocal(); });
  },

  syncLocalVersion: function () {
    if (this.shouldUploadLocalCopy()) {
      this.element.html(localStorage.getItem(this.day));
      this.save();
    } else {
      this.saveToLocal();
      this.setVersion(this.remoteVersion);
    }
  },

  // Saves all edits to local storage.
  saveToLocal: function () {
    localStorage.setItem(this.day, this.element.html());
  },

  // Enables the remote save functionalitiyi
  enableRemoteSaves: function () {
    var me = this;
    function isCtrl(e) {
      return e.ctrlKey || e.which == 19 || e.which == 17 || e.which == 115;
    }

    $('#status').click(function saveClicked() {
      me.save();
      return false;
    });

    $(document).keydown(function ctrlS(e) {
      if (e.keyCode == 'S'.charCodeAt(0) && e.ctrlKey) {
        me.save();
        return false;
      }
    });

    this.element.keyup(function (e) {
      if (!isCtrl(e)) {
        me.setDirtyFlag(true);
      }
    });
  },

  // Saves the current journal entry to the server.
  save: function () {
    var me = this;
    $('#status').text('Saving...');
    var journalEntry = {
      Body: this.element.html(),
      Day: this.day
    };

    $.ajax({
      type: 'POST',
      url: this.saveAction,
      dataType: 'json',
      contentType: 'application/json; charset=utf=8',
      timeout: 10000,
      data: JSON.stringify(journalEntry),
      success: function (data) {
        me.setDirtyFlag(false);
        me.setVersion(data.version);
      },
      error: function (request, status, err) {
        $('#status').text(err);
      }
    });
  },

  // Sets the editor to dirty mode, meaning it needs to do a remote save,
  // not that it's been doing naughty searches.
  setDirtyFlag: function (isDirty) {
    var dirtyItem = this.day + '.dirty';
    if (isDirty) {
      $('#status').text('Save');
      localStorage.setItem(dirtyItem, true);
    } else {
      $('#status').text('Saved');
      localStorage.removeItem(dirtyItem);
    }
  },

  isDirty: function () {
    return localStorage.getItem(this.day + '.dirty') != null;
  },

  // Stores the version locally.
  setVersion: function (v) {
    this.remoteVersion = v;
    localStorage.setItem(this.day + '.v', v);
  },

  // Determines whether or not the local version is newer than
  // the server's version.
  shouldUploadLocalCopy: function () {
    var localVersion = localStorage.getItem(this.day + '.v');
    return (localVersion && localVersion === this.remoteVersion && this.isDirty());
  }
};

toph.Editor.init = function () {
  var editor = new toph.Editor($('#editable-entry'));
};

$(document).ready(toph.Editor.init);

$(document).ready(function fadeNavigation() {
  var nav = $('#main-navigation');
  nav.fadeTo('slow', .3);
  nav.hover(
    function hoverIn() {
      nav.stop();
      nav.fadeTo('fast', 1);
    },
    function hoverOut() {
      nav.fadeTo('slow', .3);
    });
});