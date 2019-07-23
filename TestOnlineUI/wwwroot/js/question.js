$(document).ready(function () {
    var question = {
        init: function () {
            question.registerEvent();
        },
        registerEvent: function () {
         
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
                    var questionTypeId = $('selectType').val();
                    var listAnswer = [];
                    var listCkeditor = $('.ckeditor');
                    var listisCorrect = $('.isCorrect');
                    var count = listCkeditor.length;
                    for (i = 0; i < count; i++) {
                        var content = CKEDITOR.instances[listCkeditor[0].attr('id')].getData();
                        var iscorrect = $('#' + listisCorrect[0].attr('id')).val();
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
                        beforeSend: function () {
                            $('.loader').show();
                        },
                        complete: function () {
                            $('.loader').hide();
                        },
                        success: function (res) {
                            console.log(res);
                            if (res.status == "-2") {
                                displayMessage('Có lỗi xảy ra', 'error')
                            }

                            if (res.status == "0") {
                                displayMessage('Có lỗi xảy ra', 'warning')
                            }
                            if (res.status == "1") {
                                $('#Password').val('');
                                $('#NewPassword').val('');
                                $('#ConfirmNewPassword').val('');
                                displayMessage('Thay đổi mật khẩu thành công', 'success')

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