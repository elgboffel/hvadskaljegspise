/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/handlebars/handlebars-1.0.0.d.ts" />
var navigation = {
    menuWrapper: $("#menu-wrapper"),
    containerHeight: $("#menu-wrapper").outerHeight(),
    currentItemOl: $("#menu-wrapper .nav-open").last().find("> ol"),
    depth: 10,
    currentDepth: 0,
    mobileExpanderAnimation: 300,
    upSetup: function () {
        $(document).on("click", "#menu-wrapper .up .bck", function () {
            var parentLi = $(this).closest("li");
            var parentOl = $(this).closest("ol");
            var props = parentOl.data();
            var level = parseInt(props.level);
            var duration = parseFloat($('#menu-wrapper ol ol').css('transition-duration').replace('s', '')) * 1000;
            var setDuration = duration != null ? parseFloat : 1000;
            if (level > 0) {
                navigation.menuWrapper.removeClass("depth-" + level).addClass("depth-" + (level - 1));
            }
            navigation.adjustHeight(parentLi.closest("ol").closest("li").closest("ol"), "init");
            setTimeout(function () {
                parentLi.closest("ol").closest("li").removeClass("nav-open");
            }, setDuration);
            return false;
        });
    },
    expanderSetup: function () {
        $(document).on("click", "#menu-wrapper .exp", function () {
            var expander = $(this);
            var expanderParentLi = expander.closest("li");
            var expanderParentOl = $(this).closest("ol");
            var props = expanderParentOl.data();
            var level = parseInt(props.level);
            //reset Tree Menu
            for (var i = navigation.depth; i >= 0; i--) {
                navigation.menuWrapper.removeClass("depth-" + i);
            }
            if (level > 0) {
                navigation.menuWrapper.removeClass("depth-" + level).addClass("depth-" + (level + 1));
            }
            expanderParentLi.siblings().removeClass("nav-open");
            $("li", expanderParentLi).removeClass("nav-open");
            if (expanderParentLi.hasClass("nav-open")) {
                expanderParentLi.removeClass("nav-open");
            }
            else {
                expanderParentLi.addClass("nav-open");
            }
            if ($("> ol", expanderParentLi).length > 0) {
                navigation.adjustHeight($("> ol", expanderParentLi), "");
            }
            return false;
        });
    },
    adjustHeight: function (x, callee) {
        var hTimeout;
        if ($(".l-2.hc.ancestor").length > 0 && callee == "init") {
            return;
        }
        var newHeight = x.height() + $(".l-0.nav-open > span").height();
        navigation.containerHeight = newHeight > navigation.containerHeight ? newHeight : navigation.containerHeight;
        if ($(".tree-accordion").length < 1) {
            navigation.menuWrapper.css("height", navigation.containerHeight);
        }
        if ($(".tree-accordion").length > 0) {
            var oldHeight = $(".l-0.hc.nav-open").outerHeight() || 0;
            if (newHeight < oldHeight) {
                hTimeout = 300;
            }
            else {
                hTimeout = 100;
            }
            setTimeout(function () {
                $(".l-0.hc").each(function () {
                    $(this).css("height", $(".e-p .exp", $(this)).outerHeight());
                });
                $(".l-0.hc.nav-open").css("height", newHeight);
            }, hTimeout);
        }
    },
    loadLevel: function () {
        $('#tree-menu ol ol .exp').on('click', function (event) {
            var $target = $(event.target);
            var $li = $target.closest("li");
            var $ol = $li.closest("ol");
            if ($li.find('ol').length <= 0) {
                var id = parseInt($target.data().id);
                navigation.getLevelsFor(id).then(function (result) {
                    // DONE
                    var source = $("#tree-menu-list").html();
                    var template = Handlebars.compile(source);
                    var context = JSON.parse(result);
                    var olLevel = parseInt($ol.data().level);
                    context.level = olLevel + 1;
                    var html = template(context);
                    $li.append(html).addClass('nav-open');
                    navigation.loadLevel();
                    navigation.adjustHeight($li.find("ol"), "");
                }, function (response) {
                    // FAILED
                    if (response.status == 400) {
                        var fail = $('<div></div>', {
                            class: "alert alert-danger"
                        }).text('<strong>Woops!</strong> Computer says no.');
                        $li.append(fail);
                    }
                });
            }
        });
    },
    getLevelsFor: function (id) {
        return $.ajax({
            contentType: "application/json",
            accepts: "application/json",
            url: '/umbraco/api/treemenu/gettreelevel?id=' + id,
            method: 'GET'
        });
    },
    mobileTrigger: function () {
        var mobileMenu = $(".mobile-expander");
        //open/close trigger
        $('.mobile-trigger').on('click', function () {
            $(this).toggleClass('is-clicked');
            mobileMenu.slideToggle(navigation.mobileExpanderAnimation);
        });
        //Mobile expander
        //Delay display none for JS to kick in
        setTimeout(function () {
            mobileMenu.css({
                "position": "relative",
                "margin-left": "0",
                "display": "none",
                "visibility": "visible"
            });
        }, 200);
    },
    init: function () {
        navigation.upSetup();
        navigation.expanderSetup();
        navigation.adjustHeight(navigation.currentItemOl, "init");
        navigation.loadLevel();
        navigation.mobileTrigger();
    }
};
$(function () {
    navigation.init();
});
//# sourceMappingURL=tree-menu.js.map