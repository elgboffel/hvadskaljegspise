/// <reference path="../typings/owlcarousel/owlcarousel.d.ts" />
var App;
(function (App) {
    var Carousel = (function () {
        function Carousel(element) {
            this._element = element;
            this._defaultOptions = JSON.parse(this._element.getAttribute("data-options"));
            this.activate();
            //this.placeNavigation();
        }
        Carousel.prototype.activate = function () {
            $(this._element).owlCarousel(this._defaultOptions);
        };
        return Carousel;
    }());
    App.Carousel = Carousel;
})(App || (App = {}));
$(function () {
    var elements = document.getElementsByClassName('owl-carousel');
    for (var i = 0; i < elements.length; i++) {
        var carousel = new App.Carousel(elements[i]);
    }
});
//# sourceMappingURL=carousel.jquery.js.map