$(document).ready(function () {
    var adminjs = {
        init: function () {
            adminjs.registerFunction();
        },
        registerFunction() {


            console.log('ok');

            var prompt = true;
            $('.loader').hide();

            $(document).ready(function () {
                  
                $("#changeadminpassword").validate({
                    rules: {                      
                        'Password': {
                            required: true,
                            minlength: 6
                        }
                        ,
                        'NewPassword': {
                            required: true,
                            minlength:6
                        },
                        'ConfirmNewPassword': {
                            required: true,
                            equalTo: "#NewPassword"
                        }

                    },
                    messages: {
                        'Password': {
                            required: "Nhập vào mật khẩu của bạn",
                            minlength: "Mật khẩu cần tối thiểu 6 ký tự"

                        }
                        ,
                        'NewPassword': {
                            required: "Nhập vào mật khẩu mới của bạn",
                            minlength: "Mật khẩu cần tối thiểu 6 ký tự"
                        },
                        'ConfirmNewPassword': {
                            required: "Xác nhận mật khẩu mới",
                            equalTo: "Mật khẩu không khớp"
                        }
                    }
                });

                $("#updateAdmin").validate({
                    rules: {
                        'FullName': {
                            required: true
                        },
                        'PhoneNumber': {
                            digits: true,
                            minlength: 10,
                            maxlength: 10
                        },
                        'file': {
                         
                            extension: "jpg|jpeg|gif|png|bmp"
                        }
                    },
                    messages: {
                        'FullName': {
                            required: "Vui lòng nhập vào họ tên của bạn"
                        },
                        'PhoneNumber': {
                            digits: "Nhập vào số điện thoại hợp lệ có 10 số",
                            minlength: "Nhập vào số điện thoại hợp lệ có 10 số",
                            maxlength: "Nhập vào số điện thoại hợp lệ có 10 số"
                        },
                        'file': {
                         
                            extension : 'File không hợp lệ'
                        }

                    }
                })

                $('#addCategoryFrm').validate({
                    rules: {
                        'Name': {
                            required: true
                        },
                        'file': {
                           extension: "jpg|jpeg|gif|png|bmp"
                        }
                    },
                    messages: {
                        'Name': {
                            required: "Vui lòng nhập vào tên chuyên mục"
                        },
                        'file': {

                            extension: 'File không hợp lệ'
                        }
                    }
                })

                $('#addlistquestionfrm').validate({
                    rules: {

                        'file': {
                            extension: "xls|xlsx"
                        }
                    },
                    messages: {

                        'file': {

                            extension: 'File không hợp lệ'
                        }
                    }
                })

                $('#updateCategoryFrm').validate({
                    rules: {
                        'Name': {
                            required: true
                        },
                        'file': {
                            extension: "jpg|jpeg|gif|png|bmp"
                        }
                    },
                    messages: {
                        'Name': {
                            required: "Vui lòng nhập vào tên chuyên mục"
                        },
                        'file': {

                            extension: 'File không hợp lệ'
                        }
                    }
                })

                $('#addunitfrm').validate({
                    rules: {
                        'Name': {
                            required: true
                        },
                        'PhoneNumber': {
                            digits: true,
                            minlength: 10,
                            maxlength: 10
                        }
                    },
                    messages: {
                        'Name': {
                            required: "Vui lòng nhập vào tên chuyên mục"
                        },
                        'PhoneNumber': {
                            digits: "Nhập vào số điện thoại hợp lệ có 10 số",
                            minlength: "Nhập vào số điện thoại hợp lệ có 10 số",
                            maxlength: "Nhập vào số điện thoại hợp lệ có 10 số"
                        }
                    }
                })

                $('#addunitfrm').validate({
                    rules: {
                        'Name': {
                            required: true
                        },
                        'PhoneNumber': {
                            digits: true,
                            minlength: 10,
                            maxlength: 10
                        }
                    },
                    messages: {
                        'Name': {
                            required: "Vui lòng nhập vào tên chuyên mục"
                        },
                        'PhoneNumber': {
                            digits: "Nhập vào số điện thoại hợp lệ có 10 số",
                            minlength: "Nhập vào số điện thoại hợp lệ có 10 số",
                            maxlength: "Nhập vào số điện thoại hợp lệ có 10 số"
                        }
                    }
                })

                $('#updateunitfrm').validate({
                    rules: {
                        'Name': {
                            required: true
                        },
                        'PhoneNumber': {
                            digits: true,
                            minlength: 10,
                            maxlength: 10
                        }
                    },
                    messages: {
                        'Name': {
                            required: "Vui lòng nhập vào tên chuyên mục"
                        },
                        'PhoneNumber': {
                            digits: "Nhập vào số điện thoại hợp lệ có 10 số",
                            minlength: "Nhập vào số điện thoại hợp lệ có 10 số",
                            maxlength: "Nhập vào số điện thoại hợp lệ có 10 số"
                        }
                    }
                })

                $('#addmemberfrm').validate({
                    rules: {
                        'Name': {
                            required: true
                        },
                        'PhoneNumber': {
                            digits: true,
                            minlength: 10,
                            maxlength: 10
                        },
                        'Email': {
                            required: true,
                            email: true
                        }
                    },
                    messages: {
                        'Name': {
                            required: "Vui lòng nhập vào họ tên"
                        },
                        'PhoneNumber': {
                            digits: "Nhập vào số điện thoại hợp lệ có 10 số",
                            minlength: "Nhập vào số điện thoại hợp lệ có 10 số",
                            maxlength: "Nhập vào số điện thoại hợp lệ có 10 số"
                        },
                        'Email': {
                            required: "Nhập vào email đúng định dạng",
                            email: "Vui lòng nhập đúng định dạng email"
                        }
                    }
                })

                $('#updatememberfrm').validate({
                    rules: {
                        'Name': {
                            required: true
                        },
                        'PhoneNumber': {
                            digits: true,
                            minlength: 10,
                            maxlength: 10
                        },
                        'Email': {
                            required: true,
                            email: true
                        }
                    },
                    messages: {
                        'Name': {
                            required: "Vui lòng nhập vào họ tên"
                        },
                        'PhoneNumber': {
                            digits: "Nhập vào số điện thoại hợp lệ có 10 số",
                            minlength: "Nhập vào số điện thoại hợp lệ có 10 số",
                            maxlength: "Nhập vào số điện thoại hợp lệ có 10 số"
                        },
                        'Email': {
                            required: "Nhập vào email đúng định dạng",
                            email: "Vui lòng nhập đúng định dạng email"
                        }
                    }
                })

                $('#addquestionbank').validate({
                    rules: {
                        'Name': {
                            required: true
                        }
                    },
                    messages: {
                        'Name': {
                            required: "Vui lòng nhập vào tên nhóm câu hỏi"
                        }
                    }
                })

                $('#addschedule').validate({
                    rules: {
                        'Name': {
                            required: true
                        },
                        'Time': {
                            required: true,
                            digits: true,
                            min: 10,
                            max: 300
                        },
                        'TotalQuestion': {
                            required: true,
                            digits: true,
                            min: 1
                        },
                        'Percentage': {
                            required: true,
                            digits: true,
                            min: 1,
                            max: 100,
                        },
                        'Description': {
                            required: true
                        }

                        
                    },
                    messages: {
                        'Name': {
                            required: "Vui lòng nhập vào tên kì thi"
                        },
                        'Time': {
                            required: 'Nhập vào thời gian thi',
                            digits: 'Sai định dạng',
                            min: 'Thời gian thi phải lớn hơn 10 phút',
                            max: 'Thời gian thi phải nhỏ hơn 300 phút'
                        },
                        'TotalQuestion': {
                            required: 'Nhập vào tổng số câu hỏi ',
                            digits: 'Số câu hỏi phải là số nguyên',
                            min: 'Số câu hỏi phải lớn hơn 1'
                        },
                        'Percentage': {
                            required: 'Nhập vào phần trăm để đạt kì thi',
                            digits: 'Sai định dạng',
                            min: 'Phần trăm phải lớn hơn 1 và nhỏ hơn 100',
                            max: 'Phần trăm phải lớn hơn 1 và nhỏ hơn 100',
                        },
                        'Description': {
                            required: 'Vui lòng nhập vào mô tả về cuộc thi'
                        }
                    }
                })

                

               
            });

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
           
            $(document).ready(function () {
                $('#confirmChangePassword').off('click').on('click', function (e) {
                    e.preventDefault();
                 
                    
                    prompt = false;
                   
                    if ($('#changeadminpassword').valid()) {
                        var data = {
                            'password': $('#Password').val(),
                            'newpassword': $('#NewPassword').val(),
                            'confirmnewpassword': $('#ConfirmNewPassword').val()
                        };
                      
                        $.ajax({
                            url: '/Home/ChangePassword',
                            data: { viewmodel: data },
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
                  



                });
            })

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
            $(document).on('click', '.deletememberschedule', function () {
                if (confirm('Bạn có muốn xóa thí sinh này khỏi đợt thi không ?')) {
                    var id = $(this).data('idmember');
                    var schedule = $(this).data('schedule');
                    var viewModel = {
                        'ScheduleId': schedule,
                        'MemberId': id
                    };
                   

                    $.ajax({
                        url: "/Admin/TestSchedule/DeleteMember",
                        type: 'post',
                        data: { viewmodel: viewModel },
                        success: function (response) {
                            if (response.status == 0) {
                                alert('Xảy ra lỗi, thử lại sau');
                            }
                            if (response.status == 1) {
                                window.location.href = "/Admin/TestSchedule/ViewListMemberDetail?scheduleId=" + schedule;

                            }
                        },
                        error: function (err) {
                            console.log(err);
                        }
                    })

                }
               

            })

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
                    var listNameAnswer= $('.nameanswer');
                    var listId = [];
                    $('.ckeditor').each(function () {
                        listId.push($(this).attr('id'));
                    });
                    var count = listisCorrect.length;
                    for (i = 0; i < count; i++) {

                        var content = CKEDITOR.instances[listId[i]].getData();
                        var iscorrect = $(listisCorrect[i]).val();
                        var nameanswer = $(listNameAnswer[i]).text();
                        var sequence = i;
                        var item = {
                            'AnswerName': nameanswer,
                            'Description': content,
                            'Sequence': sequence,
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
                                displayMessage('Xảy ra lỗi , chắc chắn rằng câu hỏi của bạn là hợp lệ', 'error')
                            }
                            if (res.status == "1") {

                                CKEDITOR.instances['questionContent'].setData('');

                                for (i = 0; i < count; i++) {
                                    CKEDITOR.instances[listId[i]].setData('');
                                }

                                displayMessage('Thêm mới câu hỏi thành công', 'success')

                            }
                        }
                    })
                }


            })

            $(document).on('click', '#submitexam', function () {
                var result = [];
                var examid = $(this).data('examid');
               
                $('.useranswer').each(function () {
                    var a = $(this).data('questionid');
                    var b = $(this).data('answerid');
                    var object = {
                        'questionid': a,
                        'answerid': b
                    };
                    result.push(object);

                })
                var viewModel = {
                    'ExamId': examid,
                    'ListAnswer' : result
                };

                $.ajax({
                    url: '/User/Home/UserViewExamDetail',
                    data: { viewModel: viewModel },
                    dataType: 'json',
                    type: 'post',
                    success: function (res) {
                        if (res.status == "0") {
                            displayMessage('Xảy ra lỗi, thử lại sau', 'error')
                        }
                        if (res.status == "1") {

                            displayMessage('Cảm ơn bạn đã hoàn thành bài thi', 'success')

                        }
                    }
                })
            })
            $(document).on('click', '#createExam', function () {
             
                var totalquestion = $(this).data('totalquestion');
                var tong = 0;
                var realnumber = $('.realnumber');
                for (var i = 0; i < realnumber.length; i++) {
                    tong = tong + parseInt($(realnumber[i]).val());
                }
              
                if (totalquestion != tong) {
                    alert('Số câu hỏi trong đề thi là '+ totalquestion);
                }
                else
                {
                    var scheduleId = $('#scheduleid').text();
                    var numberexam = $('#numberExam').val();
                    var questionGroupInfo = [];

                    var list = $('.realnumber');
                    var hidegroup = $('.hidegroupid');
                    for (var i = 0; i < list.length; i++) {
                        var numberquestion = $(list[i]).val();
                        var idgroup = $(hidegroup[i]).text();
                        var object = {
                            QuestionGroupId: idgroup,
                            TotalQuestion: numberquestion
                        };
                        questionGroupInfo.push(object);
                    }
                    var viewModel = {
                        'ScheduleId': scheduleId,
                        'TotalExam': numberexam,
                        'QuestionGroupList': questionGroupInfo
                    };

                    $.ajax({
                        url: '/Admin/TestSchedule/CreateExam',
                        data: { viewModel: viewModel },
                        dataType: 'json',
                        type: 'post',
                        success: function (res) {


                            if (res.status == "0") {
                                displayMessage('Xảy ra lỗi , vui lòng thử lại sau', 'error');
                            }
                            if (res.status == "1") {
                                displayMessage('Tạo đề thi thành công', 'success');
                               

                            }
                        }
                    })
                }
            })

            $(document).on('click', '#sendEmail', function () {
                var listmember = [];
                $('.tdmemberid').each(function () {
                    var id = $(this).text();
                    listmember.push(id);
                })
                var scheduleId = $(this).data('schedule');
                var viewModel = {
                    'ScheduleId': scheduleId,
                    'ListMember': listmember
                };

                $.ajax({
                    url: '/Admin/TestSchedule/SendEmail',
                    data: { viewModel: viewModel },
                    dataType: 'json',
                    type: 'post',
                    beforeSend: function () {
                        $('.loader').show();
                    },
                    complete: function () {
                        $('.loader').hide();
                    },
                    success: function (res) {


                        if (res.status == "0") {
                            displayMessage('Xảy ra lỗi, thử lại sau', 'error')
                        }
                        if (res.status == "1") {

                           

                            displayMessage('Gửi email đến các thí sinh thành công', 'success')

                        }
                    }
                })


            })

            $(document).on('click', '#updateQuestionBtn', function () {

                if (checkValidQuestion() == false) {
                    alert('Vui lòng điền đầy đủ câu hỏi và đáp án');
                }
                if (checkValidQuestion() == true) {
                    var questionContent = CKEDITOR.instances['questionContent'].getData();
                    var questionGroupId = $('#selectGroupId').val();
                    var questionTypeId = $('#selectType').val();
                    var questionId = $('#questionId').val();
                   
                    var listAnswer = [];

                    var listisCorrect = $('.isCorrect');
                    var listNameAnswer = $('.nameanswer');
                    var listId = [];
                    $('.ckeditor').each(function () {
                        listId.push($(this).attr('id'));
                    });
                    var count = listisCorrect.length;
                    for (i = 0; i < count; i++) {

                        var content = CKEDITOR.instances[listId[i]].getData();
                        var iscorrect = $(listisCorrect[i]).val();
                        var nameanswer = $(listNameAnswer[i]).text();
                        var sequence = i;
                        var item = {
                            'AnswerName': nameanswer,
                            'Sequence': sequence,
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
                        url: '/Admin/Question/UpdateQuestion?questionId=' + questionId,
                        data: { question: object },
                        dataType: 'json',
                        type: 'post',
                        success: function (res) {


                            if (res.status == "0") {
                                displayMessage('Xảy ra lỗi , chắc chắn rằng câu hỏi của bạn là hợp lệ', 'error')
                            }
                            if (res.status == "1") {

                               

                                displayMessage('Update câu hỏi thành công', 'success')

                            }
                        }
                    })
                }


            })

            $(document).on('click', '#viewExam', function () {
                var id = $(this).data('scheduleid');
                window.location.href = '/Admin/TestSchedule/ViewExam?scheduleId='+id;
            })

            $(document).on('click', '#adduser', function () {
                var id = $(this).data('scheduleid');
                window.location.href = '/Admin/TestSchedule/CreateListMember?scheduleId='+id;
            })

            $(document).on('click', '#viewUser', function () {
                var id = $(this).data('scheduleid');
                window.location.href = '/Admin/TestSchedule/ViewListMemberDetail?scheduleId=' + id;
            })
            $(document).on('click', '#accessexam', function () {

                var id = $(this).data('examuser');
                console.log(id);
                window.location.href = "/User/Home/UserViewExamDetail?userexamId=" + id;
            })

            $(document).ready(function () {
                $('#backListCategory').on('click', function () {
                    window.location.href = '/Admin/TestCategory/Index';
                });
            })

            $(document).ready(function () {
                $('#backListUnit').on('click', function () {
                    window.location.href = '/Admin/TestUnit/Index';
                });
            })

            $(document).ready(function () {
                
                $('#backListMember').on('click', function () {
                    var id = $(this).data('memberunitid');
                    window.location.href = '/Admin/TestMember/Index?unitId='+id;
                });
            })

            $(document).ready(function () {
                $('#backListQuestionGroup').on('click', function () {
                    window.location.href = '/Admin/QuestionBank/Index';
                })
            })

            $(document).ready(function () {
               
                $('#backListGroup').on('click', function () {
                    var id = $(this).data('questiongroupid');
                   
                    window.location.href = '/Admin/Question/Index?questionGroupId='+id;
                })
            })

         

            $(document).ready(function () {

                $('input:not(:button,:submit),textarea,select').change(function () {
                    window.onbeforeunload = function () {
                        if (prompt == true)
                            return "You have made changes to the page";
                    }
                });


                $('button:submit').click(function (e) {
                    prompt = false;
                });

                $('input:submit').click(function (e) {
                    prompt = false;
                });
            });
        },
        GetListCategory: function (objectFilter) {
            $.ajax({
                url: "/Admin/TestCategory/GetListCategory",
                type: 'post',
                data: objectFilter,
                success: function (response) {
                    $('#listCategory').html(response);
                },
                error: function (ex) {
                    console.log(ex);
                }
            })
        }

         
    }
    adminjs.init();
});

