$(function () {
    var container = null;

    function initContainer() {
        container = $(document.body).append(ich.tagCloudContainer({})).find('div.tag-cloud-container');

        $('a.close', container).click(function () {
            container.fadeOut();
            return false;
        });

        return container;
    }

    function load(link) {
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

    $('#show-tagcloud').click(function () {
        container = container || initContainer();
        load(container.fadeIn().find('a.tag-7'));
        return false;
    });
});