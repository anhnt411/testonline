$(document).ready(function () {
    var question = {
        init: function () {
            question.registerEvent();
        },
        registerEvent: function () {
          
            CKEDITOR.replace('questionContent', {
                customConfig: '/js/CustomConfig.js',                     
            });

                                
            var templateHtml = `<div class="editor">
                                    <div class="editor_content">
                                        <span> Đáp án A </span> <span style="color:red;">*</span>
                                    </div>
                                    <textarea id="answerContent1" name="Content" class="ckeditor"></textarea>
                                    <select name="IsCorrect" class="isCorrect">
                                        <option value="false" selected>Sai</option>
                                        <option value="true">Đúng</option>
                                    </select>
                                    <br />
                                    <button class="addanswer">+</button><button class="removeanswer">-</button>
                                    <hr />
                                </div>`;
                               

            
        }
    };
    question.init();


})