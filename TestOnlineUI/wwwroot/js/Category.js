$(document).ready(function () {
    var category = {
        init: function () {
            category.registerEvent();
        },
        registerEvent: function () {
           
            var currentNameSortAsc = true;
            var currentDateSortAsc = true;
            var totalPage = 0;
            var totalRecord = 0;
           
        
           

            function GetListCategory(objectFilter) {
               
                $.ajax({
                    url: "/Admin/TestCategory/GetListCategory",
                    type: 'post',
                    data: objectFilter,
                    success: function (response) {
                     
                        $('#listCategory').html(response);
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
            GetListCategory(dataFilter);

           

            $(document).ready(function () {
                $('#searchCategory').on('click', function () {
                
                    var searchValue = $('#searchCategoryContent').val();
                    dataFilter.multipeFilter = searchValue;
                    dataFilter.sort[0].field = '';
                    $('#pagination').empty();

                    $('#pagination').removeData("twbs-pagination");

                    $('#pagination').unbind("page");
                    GetListCategory(dataFilter);

                });
            })

            $(document).on('click', ".deletecategory", function () {
                if (confirm('Bạn có muốn xóa chuyên mục này không')) {
                    var id = $(this).data('idcategory');
                   
                    $.ajax({
                        url: "/Admin/TestCategory/DeleteCategory?categoryid="+id,
                        type: 'get',
                        success: function (response) {
                            if (response.status == 0) {
                                window.location.href = "/home/error";
                            }
                            if (response.status == 1) {
                               
                                var filter = {};
                                GetListCategory(filter);
                            }
                        },
                        error: function (err) {
                            console.log(err);
                        }
                    })

                }
            })

            $(document).on("click", ".sortCategory", function () {
               
                var sortName = $(this).data('sortcategory');
                dataFilter.sort[0].field = sortName;
                if (sortName == "CategoryName") {
                    dataFilter.sort[0].asc = currentNameSortAsc;
                    GetListCategory(dataFilter);
                    currentNameSortAsc = !currentNameSortAsc;
                }
                if (sortName == "UpdatedDate")
                    dataFilter.sort[0].asc = currentDateSortAsc;
                    GetListCategory(dataFilter);
                    currentDateSortAsc = !currentDateSortAsc;
                
                })
             
            

            
        }
     
    }
    category.init();

})