$(function () {
    function autosize() {
        var area = $(this);
        $('span', area.closest('div.autosize-wrap')).text(area.val());
    }

    var tas = $('textarea.autosize');
    tas.wrap('<div class="autosize-wrap" />').parent().append('<pre><span></span><br /></pre>');
    tas.bind('input', autosize).each(autosize);
});