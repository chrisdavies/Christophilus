$(function () {
    var container = null;

    function load() {
        var link = $('a.selected', container);
        var loading = $('h2.loading', container).show();
        $('div.tag-cloud', container).remove();
        $.ajax({
            url: '/entries/tagcloud/' + $(link).attr('href'),
            type: 'GET',
            dataType: 'json',
            success: function (result) {
                var cloud = ich.tagCloud(result);
                container.append(cloud);
                var animatedProperties = {
                    paddingLeft: '50px',
                    paddingTop: '75px',
                    paddingBottom: '125px',
                    paddingRight: '150px',
                    opacity: 1
                };

                cloud.animate(animatedProperties, 500);
            },
            error: function (e) {
                alert(toph.error(e));
            },
            complete: function () {
                loading.hide();
            }
        });
    }

    function initContainer() {
        container = $(document.body).append(ich.tagCloudContainer({})).find('div.tag-cloud-container');

        $('a.close', container).click(function () {
            container.fadeOut();
            return false;
        });

        $('div.cloud-types a', container).click(function () {
            $('a', $(this).closest('div')).removeClass('selected');
            $(this).addClass('selected').blur();
            load();
            return false;
        });

        return container;
    }

    $('#show-tagcloud').click(function () {
        container = container || initContainer();
        container.fadeIn();
        load();
        return false;
    });
});