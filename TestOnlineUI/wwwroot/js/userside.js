$(document).ready(function () {
    var userside = {
        init: function () {
            userside.registerEvent();
        },
        registerEvent: function () {

          
            var totalPage = 0;
            var totalRecord = 0;




            function GetListUnit(objectFilter) {

                $.ajax({
                    url: "/User/Home/GetListUserSchedule",
                    type: 'post',
                    data: objectFilter,
                    success: function (response) {

                        $('#listuserschedule').html(response);
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
                                GetListUnit(dataFilter);
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

            

            $('#accessexam').on('click', function () {

                var id = $(this).data('examuser');
                console.log(id);
                window.location.href = "/User/Home/UserViewExamDetail?examId=" + id;
            })
            

            //$(document).on('click', ".deleteunit", function () {
            //    if (confirm('Bạn có muốn xóa đơn vị này không')) {
            //        var id = $(this).data('idunit');
            //        console.log(id);

            //        $.ajax({
            //            url: "/Admin/TestUnit/DeleteUnit?unitid=" + id,
            //            type: 'get',
            //            success: function (response) {
            //                if (response.status == 0) {
            //                    window.location.href = "/home/error";
            //                }
            //                if (response.status == 1) {
            //                    $('#pagination').empty();

            //                    $('#pagination').removeData("twbs-pagination");

            //                    $('#pagination').unbind("page");
            //                    var filter = {};
            //                    GetListUnit(filter);

            //                }
            //            },
            //            error: function (err) {
            //                console.log(err);
            //            }
            //        })

            //    }
            //})

         




        }

    }
    userside.init();

})

