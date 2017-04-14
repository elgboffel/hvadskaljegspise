/// <reference path="../typings/jquery/jquery.d.ts" />
$(function () {
    var clipboard = new Clipboard('#copy-link');
    $('.share-item a').on('click', function (e) {
        e.preventDefault();
    });
});
