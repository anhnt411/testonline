﻿$(document).ready(function () {
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

