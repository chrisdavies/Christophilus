var toph = toph || {};
/*
To Do:
when page loads, don't scroll.  keep it at top
when I press enter, it doesn't scroll up.  Only when I type.
 - with big docs, it seems to get off-sync

clean up.
try to merge both pre logics, but no sweat if not
make textarea wider
test in ff, chrome, ie
*/

toph.autogrower = function (element) {
  this.element = element;
  this.pre = this.createPre();
  this.caretTracker = this.createCaretTracker();
  this.extraHeight = 60;

  this.attachResize();
};

toph.autogrower.prototype = {
  createPre: function () {
    var pre = $('<pre class="resizer-pre" style="display:none;border: 1px solid red; position: absolute; top: 58px; left: 800px; width: 700px;" />');
    pre.insertAfter(this.element);
    return pre;
  },

  createCaretTracker: function () {
    var div = $('<div style="position: absolute; top: 0; left: 0; width: 10px; height: 10px; background: red;" />');
    div.insertAfter(this.element);
    
    // TEMP
    var me = this;
    this.element.bind('keydown keyup paste change', function () {
      //document.title = e.keyCode;//38, 40
      var pos = document.getElementById('editable-entry').selectionStart;
      me.pre.text(pos ? me.element.val().substr(0, pos) + '\n' : '');
      var h = me.pre.height();
      var p = me.element.position();
      me.caretTracker.css('top', p.top + h - 10 + 'px');
      window.scrollTo(p.left, p.top + h - ($(window).height() / 2));
    });
    
    return div;
  },

  attachResize: function () {
    var me = this;
    this.element.bind('keydown keyup paste change', function () {
      me.resize();
    });

    this.resize();
  },

  resize: function () {
    this.pre.text(this.element.val());
    this.element.height(this.pre.height() + this.extraHeight);
  }
};

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
    $(document).click(function () { if (!me.element.has(':focus')) me.element.focus(); });
    this.element.focus();
    this.element.keyup(function () { me.saveToLocal(); });
    new toph.autogrower(this.element);
  },

  syncLocalVersion: function () {
    if (this.shouldUploadLocalCopy()) {
      this.element.val(localStorage.getItem(this.day));
      this.element.trigger('change');
      this.save();
    } else {
      this.saveToLocal();
      this.setVersion(this.remoteVersion);
    }
  },

  // Saves all edits to local storage.
  saveToLocal: function () {
    localStorage.setItem(this.day, this.element.val());
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
      Body: this.element.val(),
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
      $('#logout-link').hide();
      nav.fadeTo('slow', .3);
    });
});

$(document).ready(function initLogoutFeature() {
  var logoutLink = $('#logout-link');
  var userOptions = $('#user-options');
  logoutLink.css('width', userOptions.width() + 'px');

  userOptions.click(function () {
    logoutLink.toggle();
    return false;
  });
});