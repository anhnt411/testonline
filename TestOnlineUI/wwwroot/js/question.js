$(document).ready(function () {
    var question = {
        init: function () {
            question.registerEvent();
        },
        registerEvent: function () {

            //var displayMessage = function (message, msgType) {
            //    toastr.options = {
            //        "closeButton": true,
            //        "debug": false,
            //        "positionClass": "toast-top-right",
            //        "onClick": null,
            //        "showDuration": "300",
            //        "hideDuration": "1000",
            //        "timeOut": "8000",
            //        "extendedTimeOut": "1000",
            //        "showEasing": "swing",
            //        "hideEasing": "linear",
            //        "showMethod": "fadeIn",
            //        "hideMethod": "fadeOut"
            //    };
            //    toastr[msgType](message);
            //};

            //if ($('#success').val()) {
            //    displayMessage($('#success').val(), 'success');
            //}
            //if ($('#info').val()) {
            //    displayMessage($('#info').val(), 'info');
            //}
            //if ($('#warning').val()) {
            //    displayMessage($('#warning').val(), 'warning');
            //}
            //if ($('#error').val()) {
            //    displayMessage($('#error').val(), 'error');
            //}

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