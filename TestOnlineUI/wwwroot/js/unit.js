$(document).ready(function () {
    var unit = {
        init: function () {
            unit.registerEvent();
        },
        registerEvent: function () {

            var currentNameSortAsc = true;
            var currentDateSortAsc = true;
            var totalPage = 0;
            var totalRecord = 0;




            function GetListUnit(objectFilter) {

                $.ajax({
                    url: "/Admin/TestUnit/GetListUnit",
                    type: 'post',
                    data: objectFilter,
                    success: function (response) {

                        $('#listUnit').html(response);
                        totalRecord = $('#rows').data('totalrow');
                        totalPage = Math.ceil(totalRecord / 5);
                        console.log(totalPage);

                        $('#pagination').twbsPagination({
                            totalPages: totalPage,
                            visiblePages: 5,
                            first: '<<',
                            prev: '<',
                            next: '>',
                            last: '>>',
                            onPageClick: function (event, page) {

                                dataFilter.skip = (page - 1) * 5;
                                dataFilter.take = 5;
                                GetListCategory(dataFilter);
                            }
                        })


                    },
                    error: function (ex) {
                        console.log(ex);
                    }
                })


            }

            //function paging (totalRow, callback) {
            //    var totalPage = Math.ceil(totalRow / 5);

            var dataFilter = {
                "filter": [
                    {
                        "field": "",
                        "valueString": "",
                        "valueDateTimeFrom": "",
                        "valueDateTimeTo": "",
                        "valueDecimalFrom": 0,
                        "valueDecimalTo": 0,
                        "valueBit": false,
                        "isActive": true
                    }
                ],
                "sort": [
                    {
                        "field": "",
                        "asc": '',
                        "isActive": true
                    }
                ],
                "multipeFilter": "",
                "skip": 0,
                "take": 5,
                "isExport": false
            };
            GetListUnit(dataFilter);



            $(document).ready(function () {
                $('#searchUnit').on('click', function () {

                    var searchValue = $('#searchUnitContent').val();
                    dataFilter.multipeFilter = searchValue;
                    dataFilter.sort[0].field = '';
                    $('#pagination').empty();

                    $('#pagination').removeData("twbs-pagination");

                    $('#pagination').unbind("page");
                    GetListUnit(dataFilter);

                });
            })

            $(document).on('click', ".deleteunit", function () {
                if (confirm('Bạn có muốn xóa đơn vị này không')) {
                    var id = $(this).data('idunit');

                    $.ajax({
                        url: "/Admin/TestUnit/DeleteUnit?unitid=" + id,
                        type: 'get',
                        success: function (response) {
                            if (response.status == 0) {
                                window.location.href = "/home/error";
                            }
                            if (response.status == 1) {

                                var filter = {};
                                GetListUnit(filter);
                            }
                        },
                        error: function (err) {
                            console.log(err);
                        }
                    })

                }
            })

            $(document).on("click", ".sortUnit", function () {

                var sortName = $(this).data('sortunit');
                dataFilter.sort[0].field = sortName;
                if (sortName == "UnitName") {
                    dataFilter.sort[0].asc = currentNameSortAsc;
                    GetListCategory(dataFilter);
                    currentNameSortAsc = !currentNameSortAsc;
                }
                if (sortName == "CreatedDate")
                    dataFilter.sort[0].asc = currentDateSortAsc;
                GetListCategory(dataFilter);
                currentDateSortAsc = !currentDateSortAsc;

            })




        }

    }
    unit.init();

})