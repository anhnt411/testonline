$(document).ready(function () {
    var questionGroup = {
        init: function () {
            questionGroup.registerEvent();
        },
        registerEvent: function () {

            var currentGroupNameSortAsc = true;
            var currentDateSortAsc = true;
            var currentCategorySortAsc = true;
            var currentDescriptionSortAsc = true;
            var currentNumberSortAsc = true;

            var totalPage = 0;
            var totalRecord = 0;




            function GetListQuestionGroup(objectFilter) {
             
                $.ajax({
                    url: "/Admin/QuestionBank/GetListQuestionGroup",
                    type: 'post',
                    data: objectFilter,
                    success: function (response) {

                        $('#listQuestionBank').html(response);
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
                                GetListQuestionGroup(dataFilter);
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
            GetListQuestionGroup(dataFilter);



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
                        GetListQuestionGroup(dataFilter);
                    }
                    else {
                        dataFilter.sort[0].field = '';
                        dataFilter.multipeFilter = '';
                        $('#pagination').empty();

                        $('#pagination').removeData("twbs-pagination");

                        $('#pagination').unbind("page");
                        GetListQuestionGroup(dataFilter);
                    }
                   

                });
            })

      
            $(document).on("click", ".sortQuestionGroup", function () {

                var sortName = $(this).data('sortquestiongroup');

            
                dataFilter.sort[0].field = sortName;
                if (sortName == "QuestionGroupName") {
                    dataFilter.sort[0].asc = currentGroupNameSortAsc;
                    GetListQuestionGroup(dataFilter);
                    currentGroupNameSortAsc = !currentGroupNameSortAsc;
                }
                if (sortName == "NumberOfQuestion") {
                    dataFilter.sort[0].asc = currentNumberSortAsc;
                    GetListQuestionGroup(dataFilter);
                    currentNumberSortAsc = !currentNumberSortAsc;
                }
                if (sortName == "CategoryName") {
                    dataFilter.sort[0].asc = currentCategorySortAsc;
                    GetListQuestionGroup(dataFilter);
                    currentCategorySortAsc = !currentCategorySortAsc;
                }
                if (sortName == "Description") {
                    dataFilter.sort[0].asc = currentDescriptionSortAsc;
                    GetListQuestionGroup(dataFilter);
                    currentDescriptionSortAsc = !currentDescriptionSortAsc;
                }

                if (sortName == "CreatedDate") {
                    dataFilter.sort[0].asc = currentDateSortAsc;
                    GetListQuestionGroup(dataFilter);
                    currentDateSortAsc = !currentDateSortAsc;
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
                                GetListQuestionGroup(filter);
                                
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
    questionGroup.init();

})