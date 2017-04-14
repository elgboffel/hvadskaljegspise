namespace Application {
    interface IAsyncContentLList extends ng.IScope {
        page: number;
        pageSize: number;
        currentPageId: string;
        result: any;
        data: string[];
        selectedTags: string[];
        getList(): void;
        checkedValue(tag: ITag, selected: boolean): void;

    }

    interface ITag {
        id: string;
        name: string;
    }

    class AsyncContentList {
        constructor(
            private $scope: IAsyncContentLList,
            private $http: ng.IHttpService
        ) {
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

                getListByTags(_scope.selectedTags, _scope.currentPageId)
            };

            let getListByTags = function (tagList, currentPageId) {
                _http({
                    method: 'GET',
                    url: '/umbraco/api/asynccontentlist/getlistbytags?tags=' + tagList + '&id=' + currentPageId
                }).then(function (response) {
                    var data: any = response.data;

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
                        var element = array[i]
                        _scope.result.push(element);
                    }
                });
            };
        }
    }

    app.controller('asyncContentListCtrl', AsyncContentList);
}