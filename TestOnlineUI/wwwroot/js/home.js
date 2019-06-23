$(document).ready(function () {
    var home = {
        init: function () {
            home.registerEvent();
        },
        registerEvent: function () {

            var prompt = true;
            $('.loader').hide();

          

            $(document).ready(function () {
                $("#registerAdmin").validate({
                    rules: {
                        'UserName': {
                            required: true,
                            minlength: 5,
                            maxlength: 30
                        },
                        'Email': {
                            required: true,
                            email: true
                        },
                        'FullName': {
                            required: true
                        },
                        'PhoneNumber': {
                            digits: true,
                            minlength: 10,
                            maxlength: 10
                        },
                        'Password': {
                            required: true,
                            minlength: 6
                        },
                        'ConfirmPassword': {
                            required: true,
                            equalTo: "#Password"
                        }


                    },
                    messages: {
                        'UserName': {
                            required: "Nhập vào tên tài khoản",
                            minlength: "Tài khoản phải lớn hơn hoặc bằng 5 ký tự",
                            maxlength: "Tài khoản phải nhỏ hơn hoặc bằng 30 ký tự"
                        },
                        'Email': {
                            required: "Nhập vào email của bạn",
                            email: "Vui lòng nhập đúng định dạng email"
                        },
                        'FullName': {
                            required: "Vui lòng nhập vào họ tên của bạn"
                        },
                        'PhoneNumber': {
                            digits: "Nhập vào số điện thoại hợp lệ có 10 số",
                            minlength: "Nhập vào số điện thoại hợp lệ có 10 số",
                            maxlength: "Nhập vào số điện thoại hợp lệ có 10 số"
                        },
                        'Password': {                      
                            required: "Nhập vào mật khẩu của bạn",
                            minlength: "Mật khẩu cần tối thiểu 6 ký tự"
                        },
                        'ConfirmPassword': {
                            required: "Xác nhận mật khẩu của bạn",
                            equalTo: "Mật khẩu không khớp"
                        
                        }
                    }
                });
                $("#loginAdmin").validate({
                    rules: {
                        'UserName': {
                            required: true
                        },
                        'Password': {
                            required: true
                        }

                    },
                    messages: {
                        'UserName': {
                            required: "Nhập vào tên tài khoản"

                        },
                        'Password': {
                            required: "Nhập vào mật khẩu"
                        }
                    }
                });
                $('#unknowpass').validate({
                    rules: {
                        'Email': {
                            required: true,
                            email: true
                        }
                    },
                    messages: {
                        'Email': {
                            required: "Nhập vào email của bạn",
                            email: "Email không chính xác"
                        }
                    }
                });
                $('#resetPassword').validate({
                    rules: {
                        'Email': {
                            required: true,
                            email: true
                        },
                        'Password': {
                            required: true,
                            minlength: 6
                        },
                        'ConfirmPassword': {
                            required: true,
                            equalTo: "#Password"
                        }
                    },
                    messages: {
                        'Email': {
                            required: "Nhập vào email của bạn",
                            email: "Vui lòng nhập đúng định dạng email"
                        },
                        'Password': {
                            required: "Nhập vào mật khẩu của bạn",
                            minlength: "Mật khẩu cần tối thiểu 6 ký tự"
                        },
                        'ConfirmPassword': {
                            required: "Xác nhận mật khẩu của bạn",
                            equalTo: "Mật khẩu không khớp"

                        }
                    }
                });
            })
          
            
      

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
               
                
                    $('#confirmForgot').off('click').on('click', function (e) {
                        e.preventDefault();

                        prompt = false;
                        if ($('#unknowpass').valid()) {
                            $.ajax({
                                url: '/Home/ForgotPassword',
                                data: { email: $('#emailForgot').val() },
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
                                    if (res.status == "-1") {

                                        displayMessage('Email không tồn tại', 'error')
                                    }
                                    if (res.status == "0") {
                                        displayMessage('Email của bạn chưa được xác thực', 'warning')
                                    }
                                    if (res.status == "1") {
                                        displayMessage('Kiểm tra email của bạn để đặt lại password', 'success')
                                       
                                    }
                                }
                            })
                        }

                       

                    });
                
              
            });



           

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
            });
            
        }
    }
    home.init();
});

