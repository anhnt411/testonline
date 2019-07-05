$(document).ready(function () {
    var member = {
        init: function () {
            member.registerEvent();
        },
        registerEvent: function () {

            var currentNameSortAsc = true;
            var currentDateSortAsc = true;
            var currentAddressSortAsc = true;
            var currentPhoneNumberSortAsc = true;
            var totalPage = 0;
            var totalRecord = 0;




            function GetListMember(objectFilter) {
                var id = '';
                $.ajax({
                    url: "/Admin/TestMember/GetListMember?MemberId ="+id,
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
                                GetListMember(dataFilter);
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
            GetListMember(dataFilter);



            $(document).ready(function () {
                $('#searchMember').on('click', function () {

                    var searchValue = $('#searchMemberContent').val();
                    dataFilter.multipeFilter = searchValue;
                    dataFilter.sort[0].field = '';
                    $('#pagination').empty();

                    $('#pagination').removeData("twbs-pagination");

                    $('#pagination').unbind("page");
                    GetListMember(dataFilter);

                });
            })

            $(document).on('click', ".deleteMember", function () {
                if (confirm('Bạn có muốn xóa đơn vị này không')) {
                    var id = $(this).data('idMember');
                    console.log(id);

                    $.ajax({
                        url: "/Admin/TestMember/DeleteMember?Memberid=" + id,
                        type: 'get',
                        success: function (response) {
                            if (response.status == 0) {
                                window.location.href = "/home/error";
                            }
                            if (response.status == 1) {

                                var filter = {};
                                GetListMember(filter);
                                window.location.href = "/Admin/TestMember/Index"
                            }
                        },
                        error: function (err) {
                            console.log(err);
                        }
                    })

                }
            })

            $(document).on("click", ".sortMember", function () {

                var sortName = $(this).data('sortMember');
                dataFilter.sort[0].field = sortName;
                if (sortName == "MemberName") {
                    dataFilter.sort[0].asc = currentNameSortAsc;
                    GetListMember(dataFilter);
                    currentNameSortAsc = !currentNameSortAsc;
                }
                if (sortName == "CreatedDate") {
                    dataFilter.sort[0].asc = currentDateSortAsc;
                    GetListMember(dataFilter);
                    currentDateSortAsc = !currentDateSortAsc;
                }
                if (sortName == "Address") {
                    dataFilter.sort[0].asc = currentAddressSortAsc;
                    GetListMember(dataFilter);
                    currentAddressSortAsc = !currentAddressSortAsc;
                }
                if (sortName == "PhoneNumber") {
                    dataFilter.sort[0].asc = currentPhoneNumberSortAsc;
                    GetListMember(dataFilter);
                    currentPhoneNumberSortAsc = !currentPhoneNumberSortAsc;
                }

            })




        }

    }
    member.init();

})