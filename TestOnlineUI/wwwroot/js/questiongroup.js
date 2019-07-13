$(document).ready(function () {
    var questionGroup = {
        init: function () {
            questionGroup.registerEvent();
        },
        registerEvent: function () {

            var currentNameSortAsc = true;
            var currentDateSortAsc = true;
            var currentAddressSortAsc = true;
            var currentPhoneNumberSortAsc = true;
            var currentEmailSortAsc = true;

            var totalPage = 0;
            var totalRecord = 0;




            function GetListQuestionGroup(objectFilter, unitId) {
                var categoryId = '';
                $.ajax({
                    url: "/Admin/QuestionBank/GetListQuestionGroup?categoryId=" + categoryId,
                    type: 'post',
                    data: objectFilter,
                    success: function (response) {

                        $('#listMember').html(response);
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
                                GetListQuestion(dataFilter, unitId);
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

            var unitId = $("#selectCategoryId option:selected").val();
            console.log(unitId);

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
            GetListQuestionGroup(dataFilter, unitId);



            $(document).ready(function () {
                $('#searchQuestionGroup').on('click', function () {

                    var unitId = $("#selectCategoryId option:selected").val();
                    var searchValue = $("#selectCategoryId option:selected").text();
                    dataFilter.multipeFilter = searchValue;
                    dataFilter.sort[0].field = '';
                    $('#pagination').empty();

                    $('#pagination').removeData("twbs-pagination");

                    $('#pagination').unbind("page");
                    GetListQuestionGroup(dataFilter, unitId);

                });
            })

            $(document).on('click', ".deletequestiongroup", function () {
                if (confirm('Bạn có muốn xóa thành viên này không')) {
                    var id = $(this).data('idquestiongroup');
                    var unitId = $(this).data('categoryid');
                    console.log(id);

                    $.ajax({
                        url: "/Admin/QuestionBank/DeleteQuestionGroup?Groupid=" + id,
                        type: 'get',
                        success: function (response) {
                            if (response.status == 0) {
                                window.location.href = "/home/error";
                            }
                            if (response.status == 1) {

                                var filter = {};
                                GetListMember(filter);
                                window.location.href = "/Admin/QuestionBank/Index";
                            }
                        },
                        error: function (err) {
                            console.log(err);
                        }
                    })

                }
            })

            $(document).on("click", ".sortQuestionGroup", function () {

                var sortName = $(this).data('sortquestiongroup');

                var unitId = $("#selectCategoryId option:selected").val();
                dataFilter.sort[0].field = sortName;
                if (sortName == "FullName") {
                    dataFilter.sort[0].asc = currentNameSortAsc;
                    GetListMember(dataFilter, unitId);
                    currentNameSortAsc = !currentNameSortAsc;
                }
                if (sortName == "DateOfBirth") {
                    dataFilter.sort[0].asc = currentDateSortAsc;
                    GetListMember(dataFilter, unitId);
                    currentDateSortAsc = !currentDateSortAsc;
                }
                if (sortName == "Address") {
                    dataFilter.sort[0].asc = currentAddressSortAsc;
                    GetListMember(dataFilter, unitId);
                    currentAddressSortAsc = !currentAddressSortAsc;
                }
                if (sortName == "PhoneNumber") {
                    dataFilter.sort[0].asc = currentPhoneNumberSortAsc;
                    GetListMember(dataFilter, unitId);
                    currentPhoneNumberSortAsc = !currentPhoneNumberSortAsc;
                }

                if (sortName == "Email") {
                    dataFilter.sort[0].asc = currentEmailSortAsc;
                    GetListMember(dataFilter, unitId);
                    currentEmailSortAsc = !currentEmailSortAsc;
                }

            })




        }

    }
    questionGroup.init();

})