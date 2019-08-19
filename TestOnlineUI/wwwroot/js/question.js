$(document).ready(function () {
    var question = {
        init: function () {
            question.registerEvent();
        },
        registerEvent: function () {

            var currentDescriptionSortAsc = true;
            var currentQuestionTypeSortAsc = true;
            var currentCreatedDateSortAsc = true;
           

            var totalPage = 0;
            var totalRecord = 0;




            function GetListQuestion(objectFilter, questionGroupId) {
                var id = '';
                $.ajax({
                    url: "/Admin/Question/GetListQuestion?questionGroupId=" + questionGroupId,
                    type: 'post',
                    data: objectFilter,
                    success: function (response) {

                        $('#listQuestion').html(response);
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
                                GetListQuestion(dataFilter, questionGroupId);
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

            var questionGroupId = $("#selectGroupId option:selected").val();
            console.log(questionGroupId);

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
            GetListQuestion(dataFilter, questionGroupId);



            $(document).ready(function () {
                $('#searchQuestion').on('click', function () {

                    var questionGroupId = $("#selectGroupId option:selected").val();
                    var searchValue = $("#selectGroupId option:selected").text();
                  
                    dataFilter.sort[0].field = '';
                    $('#pagination').empty();

                    $('#pagination').removeData("twbs-pagination");

                    $('#pagination').unbind("page");
                    GetListQuestion(dataFilter, questionGroupId);

                });
            })

            $(document).on('click', ".deleteQuestion", function (e) {
              
                if (confirm('Bạn có muốn xóa câu hỏi này không ?')) {
                    var id = $(this).data('idquestion');
                    var questionGroupId = $("#selectGroupId option:selected").val();
                    console.log(id);

                    $.ajax({
                        url: "/Admin/Question/Delete?id=" + id,
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
                                GetListQuestion(filter, questionGroupId);

                            }
                        },
                        error: function (err) {
                            console.log(err);
                        }
                    })

                }
            })

            $(document).on("click", ".sortQuestion", function () {

                var sortName = $(this).data('sortquestion');

                var questionGroupId = $("#selectGroupId option:selected").val();
                dataFilter.sort[0].field = sortName;
                if (sortName == "Description") {
                    dataFilter.sort[0].asc = currentDescriptionSortAsc;
                    GetListQuestion(dataFilter, questionGroupId);
                    currentDescriptionSortAsc = !currentDescriptionSortAsc;
                }

                if (sortName == "QuestionType") {
                    dataFilter.sort[0].asc = currentQuestionTypeSortAsc;
                    GetListQuestion(dataFilter, questionGroupId);
                    currentQuestionTypeSortAsc = !currentQuestionTypeSortAsc;
                }

                if (sortName == "CreatedDate") {
                    dataFilter.sort[0].asc = currentCreatedDateSortAsc;
                    GetListQuestion(dataFilter, questionGroupId);
                    currentCreatedDateSortAsc = !currentCreatedDateSortAsc;
                }

              

            })

            function toLetters(num) {
                "use strict";
                var mod = num % 26,
                    pow = num / 26 | 0,
                    out = mod ? String.fromCharCode(64 + mod) : (--pow, 'Z');
                return pow ? toLetters(pow) + out : out;
            }

            function fromLetters(str) {
                "use strict";
                var out = 0, len = str.length, pos = len;
                while (--pos > -1) {
                    out += (str.charCodeAt(pos) - 64) * Math.pow(26, len - 1 - pos);
                }
                return out;
            }

            function renderAnswer(answer, answerId) {
                return `<div class="editor">
                                    <div class="editor_content">
                                        <span class = "nameanswer"> ${answer} </span> <span style="color:red;">*</span>
                                    </div>
                                    <textarea id="${answerId}" name="Content" class="ckeditor"></textarea>
                                    <select name="IsCorrect" class="isCorrect">
                                        <option value="false" selected>Sai</option>
                                        <option value="true">Đúng</option>
                                    </select>
                                    <br />
                                    <button class="addanswer">+</button><button class="removeanswer">-</button>
                                    <hr />
                                </div>`;
            }



            CKEDITOR.replace('questionContent', {
                customConfig: '/js/CustomConfig.js',                     
            });

            var desc = CKEDITOR.instances['questionContent'].getData();
            console.log(desc);

            $(document).on('click', '.addanswer', function () {
               
                var countitem = $('.addanswer').length;
                if (countitem >= 2) {
                    var answer = 'Đáp Án ' + toLetters(countitem + 1);
                    var answerid = 'answerContent' + (countitem + 1).toString();
                    $(".answer_content").append(renderAnswer(answer, answerid));
                    CKEDITOR.replace(answerid, {
                        customConfig: '/js/CustomConfig.js',
                    });
                }
              
            });

            $(document).on('click', '.removeanswer', function () {

                var countitem = $('.removeanswer').length;
                if (countitem > 2) {
                    $('.editor').last().remove();
                }

            });

          



            function renderAnswer(answer, answerId) {
                return `<div class="editor">
                                    <div class="editor_content">
                                        <span class="nameanswer"> ${answer} </span> <span style="color:red;">*</span>
                                    </div>
                                    <textarea id="${answerId}" name="Content" class="ckeditor"></textarea>
                                    <select name="IsCorrect" class="isCorrect">
                                        <option value="false" selected>Sai</option>
                                        <option value="true">Đúng</option>
                                    </select>
                                    <br />
                                    <button class="addanswer">+</button><button class="removeanswer">-</button>
                                    <hr />
                                </div>`;
            }

           
         

            function toLetters(num) {
                "use strict";
                var mod = num % 26,
                    pow = num / 26 | 0,
                    out = mod ? String.fromCharCode(64 + mod) : (--pow, 'Z');
                return pow ? toLetters(pow) + out : out;
            }

            function fromLetters(str) {
                "use strict";
                var out = 0, len = str.length, pos = len;
                while (--pos > -1) {
                    out += (str.charCodeAt(pos) - 64) * Math.pow(26, len - 1 - pos);
                }
                return out;
            }
            
        }
    };
    question.init();


})