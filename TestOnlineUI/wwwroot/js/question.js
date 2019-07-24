$(document).ready(function () {
    var question = {
        init: function () {
            question.registerEvent();
        },
        registerEvent: function () {

            var displayMessage = function (message, msgType) {
                toastr.options = {
                    "closeButton": true,
                    "debug": false,
                    "positionClass": "toast-top-right",
                    "onClick": null,
                    "showDuration": "300",
                    "hideDuration": "1000",
                    "timeOut": "8000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                };
                toastr[msgType](message);
            };

            if ($('#success').val()) {
                displayMessage($('#success').val(), 'success');
            }
            if ($('#info').val()) {
                displayMessage($('#info').val(), 'info');
            }
            if ($('#warning').val()) {
                displayMessage($('#warning').val(), 'warning');
            }
            if ($('#error').val()) {
                displayMessage($('#error').val(), 'error');
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

            function renderAnswer(answer, answerId) {
                return `<div class="editor">
                                    <div class="editor_content">
                                        <span> ${answer} </span> <span style="color:red;">*</span>
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

            $(document).on('click', '#addQuestionBtn', function () {
              
                if (checkValidQuestion() == false) {
                    alert('Vui lòng điền đầy đủ câu hỏi và đáp án');
                }
                if (checkValidQuestion() == true) {
                    var questionContent = CKEDITOR.instances['questionContent'].getData();
                    var questionGroupId = $('#selectGroupId').val();
                    var questionTypeId = $('#selectType').val();
                    var listAnswer = [];
                   
                    var listisCorrect = $('.isCorrect');
                    var listId = [];
                    $('.ckeditor').each(function () {
                        listId.push($(this).attr('id'));
                    });
                    var count = listisCorrect.length;
                    for (i = 0; i < count; i++) {
                   
                        var content = CKEDITOR.instances[listId[i]].getData();
                        var iscorrect = $(listisCorrect[i]).val();
                        var item = {
                            'Description': content,
                            'IsCorrect': iscorrect
                        };
                        listAnswer.push(item);
                    }
                    var object = {
                        'QuestionTypeKey': questionTypeId,
                        'QuestionGroupId': questionGroupId,
                        'Description': questionContent,
                        'Answers': listAnswer
                    };

                    $.ajax({
                        url: '/Admin/Question/Add',
                        data: { question: object },
                        dataType: 'json',
                        type: 'post',
                        success: function (res) {
                            
                          
                            if (res.status == "0") {
                                console.log(res);
                            }
                            if (res.status == "1") {
                               
                                CKEDITOR.instances['questionContent'].setData('');

                                for ( i = 0; i < count ; i++){
                                 CKEDITOR.instances[listId[i]].setData('');
                                }

                                displayMessage('Thêm mới câu hỏi thành công', 'success')

                            }
                        }
                    })
                }
                
               
            })



            function renderAnswer(answer, answerId) {
                return `<div class="editor">
                                    <div class="editor_content">
                                        <span> ${answer} </span> <span style="color:red;">*</span>
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

            function checkValidQuestion() {
                var k = 1;
                var questionContent = $('.ckeditor');
                questionContent.each(function () {
                    var id = $(this).attr('id');
                  
                    var value = CKEDITOR.instances[id].getData();

                    if (value == "") {
                        k = 0;
                        return false;
                    }
                   
                })
                if (k == 0) {
                    return false;
                }
                else {
                    return true;
                }
              
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