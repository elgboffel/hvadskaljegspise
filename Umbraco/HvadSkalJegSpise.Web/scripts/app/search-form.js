var App;
(function (App) {
    var SearchForm = (function () {
        function SearchForm(element) {
            this._element = element;
            this.openToggle();
            this.doubleClick();
            this.outerClick();
        }
        SearchForm.prototype.openToggle = function () {
            if ($(this._element).hasClass('closed')) {
                $(this._element).find('.submit').on('click', function (e) {
                    $('.search-form--toggle input').focus();
                    $('.search-form--toggle').removeClass('closed').addClass('open');
                });
            }
            $(this._element).find('.close').on('click', function () {
                $('.search-form--toggle').removeClass('open').addClass('closed');
            });
        };
        //if double clicked
        SearchForm.prototype.doubleClick = function () {
            $('.search-form--toggle .submit').on('click', function (e) {
                return false;
                //var clicks = $(this).data('clicks');
                //var url = $(this).attr('data-action');
                //if (clicks) {
                //    window.location.href = url;
                //}
                //$(this).data("clicks", !clicks);
            });
        };
        //if panel is open and click outside of element it will close
        SearchForm.prototype.outerClick = function () {
            $('body').mouseup(function (event) {
                var element = $(".search-form--toggle");
                var isElement = element.has(event.target).length == 1;
                if (!isElement) {
                    element.removeClass('open').addClass('closed');
                }
            });
        };
        return SearchForm;
    }());
    App.SearchForm = SearchForm;
})(App || (App = {}));
$(function () {
    var elements = document.getElementsByClassName('search-form--toggle');
    for (var i = 0; i < elements.length; i++) {
        var carousel = new App.SearchForm(elements[i]);
    }
});
//# sourceMappingURL=search-form.js.map