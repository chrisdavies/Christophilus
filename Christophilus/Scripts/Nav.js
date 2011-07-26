toph.Nav = {
    init: function () {
        toph.Nav.initLogoutBehavior();
        toph.Nav.initFadeBehavior();
    },

    initLogoutBehavior: function () {
        var logoutLink = $('#logout-link');
        var userOptions = $('#user-options');
        logoutLink.css('width', userOptions.width() + 'px');

        userOptions.click(function () {
            logoutLink.toggle();
            return false;
        });
    },

    initFadeBehavior: function () {
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
    }
}

$(document).ready(toph.Nav.init);