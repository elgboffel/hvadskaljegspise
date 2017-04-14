var Application;
(function (Application) {
    var AsyncContentList = (function () {
        function AsyncContentList($scope, $http) {
            this.$scope = $scope;
            this.$http = $http;
            var _scope = this.$scope;
            var _http = this.$http;
            _scope.page = 0;
            _scope.result = [];
            _scope.pageSize = 0;
            _scope.data = [];
            _scope.selectedTags = [];
            _scope.checkedValue = function (tag, selected) {
                if (selected)
                    _scope.selectedTags.push(tag.id);
                else
                    _scope.selectedTags.splice(_scope.selectedTags.indexOf(tag.id), 1);
                getListByTags(_scope.selectedTags, _scope.currentPageId);
            };
            var getListByTags = function (tagList, currentPageId) {
                _http({
                    method: 'GET',
                    url: '/umbraco/api/asynccontentlist/getlistbytags?tags=' + tagList + '&id=' + currentPageId
                }).then(function (response) {
                    var data = response.data;
                    $('.js-hide-results').hide();
                    $('h1 span').replaceWith('<span>(' + data.length + ')</span>');
                    _scope.result = data;
                });
            };
            _scope.getList = function () {
                _http({
                    method: 'GET',
                    url: '/umbraco/api/asynccontentlist/getlist?data=' +
                        _scope.data +
                        '&page=' + _scope.page++ +
                        '&pageSize=' + _scope.pageSize
                }).then(function (response) {
                    //Convert responed data object to an array of objects
                    var array = $.map(response.data, function (value, index) {
                        return [value];
                    });
                    //Push each object to scope result array, we don't wan't to push an entire array
                    for (var i = 0; i < array.length; i++) {
                        var element = array[i];
                        _scope.result.push(element);
                    }
                });
            };
        }
        return AsyncContentList;
    }());
    Application.app.controller('asyncContentListCtrl', AsyncContentList);
})(Application || (Application = {}));
//# sourceMappingURL=async-content-list.ctrl.js.map