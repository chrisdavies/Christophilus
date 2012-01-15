$(function () {
    function autosize() {
        var area = $(this);
        area.data('span').text(area.val());
    }

    var tas = $('textarea.autosize');
    tas.wrap('<div class="autosize-wrap" />').parent().append('<pre><span></span><br /></pre>');
    tas.bind('input', autosize).each(function () {
        $(this).data('span', $('span', $(this).closest('div.autosize-wrap')));
    }).each(autosize);
});