$(document).ready(function () {
    var question = {
        init: function () {
            question.registerEvent();
        },
        registerEvent: function () {
          
            CKEDITOR.replace('questionContent', {
                customConfig: '/js/CustomConfig.js',

            
                //filebrowserUploadUrl: '/Home/UploadCKEditor'
            });
            
        }
    };
    question.init();


})