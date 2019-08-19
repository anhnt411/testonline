$(document).ready(function () {
    var schedule = {
        init: function () {
            schedule.registerEvent();
        },
        registerEvent: function () {

            var currentNameSortAsc = true;
            var currentTimeSortAsc = true;
            var currentStartDateSortAsc = true;
            var currentEndDateSortAsc = true;
            var currentCategorySortAsc = true;

            var totalPage = 0;
            var totalRecord = 0;




            function GetListSchedule(objectFilter) {

                $.ajax({
                    url: "/Admin/TestSchedule/GetListTestSchedule",
                    type: 'post',
                    data: objectFilter,
                    success: function (response) {

                        $('#listSchedule').html(response);
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
                                GetListSchedule(dataFilter);
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
            GetListSchedule(dataFilter);



            $(document).ready(function () {
                $('#searchQuestionBank').on('click', function () {

                    var categoryId = $("#selectCategoryId option:selected").val();
                    var searchValue = $("#selectCategoryId option:selected").text();
                    if (categoryId != "0") {
                        dataFilter.multipeFilter = searchValue;
                        dataFilter.sort[0].field = '';
                        $('#pagination').empty();

                        $('#pagination').removeData("twbs-pagination");

                        $('#pagination').unbind("page");
                        GetListSchedule(dataFilter);
                    }
                    else {
                        dataFilter.sort[0].field = '';
                        dataFilter.multipeFilter = '';
                        $('#pagination').empty();

                        $('#pagination').removeData("twbs-pagination");

                        $('#pagination').unbind("page");
                        GetListSchedule(dataFilter);
                    }


                });
            })


            $(document).on("click", ".sortSchedule", function () {

                var sortName = $(this).data('sortschedule');


                dataFilter.sort[0].field = sortName;
                if (sortName == "Name") {
                    dataFilter.sort[0].asc = currentNameSortAsc;
                    GetListSchedule(dataFilter);
                    currentNameSortAsc = !currentNameSortAsc;
                }
                if (sortName == "Time") {
                    dataFilter.sort[0].asc = currentTimeSortAsc;
                    GetListSchedule(dataFilter);
                    currentTimeSortAsc = !currentTimeSortAsc;
                }
                if (sortName == "Category") {
                    dataFilter.sort[0].asc = currentCategorySortAsc;
                    GetListSchedule(dataFilter);
                    currentCategorySortAsc = !currentCategorySortAsc;
                }
                if (sortName == "StartDate") {
                    dataFilter.sort[0].asc = currentStartDateSortAsc;
                    GetListSchedule(dataFilter);
                    currentStartDateSortAsc = !currentStartDateSortAsc;
                }

                if (sortName == "EndDate") {
                    dataFilter.sort[0].asc = currentEndDateSortAsc;
                    GetListSchedule(dataFilter);
                    currentEndDateSortAsc = !currentEndDateSortAsc;
                }

            })

            $(document).on('click', ".deletequestiongroup", function () {
                if (confirm('Bạn có muốn xóa nhóm câu hỏi này không ?')) {
                    var id = $(this).data('idquestiongroup');


                    $.ajax({
                        url: "/Admin/QuestionBank/DeleteGroup?questionGroupId=" + id,
                        type: 'get',
                        success: function (response) {
                            if (response.status == 0) {
                                window.location.href = "/home/error";
                            }
                            if (response.status == 1) {
                                $('#pagination').empty();

                                $('#pagination').removeData("twbs-pagination");

                                $('#pagination').unbind("page");
                                var filter = {};
                                GetListSchedule(filter);

                            }
                        },
                        error: function (err) {
                            console.log(err);
                        }
                    })

                }
            })




        }

    }
    schedule.init();

})