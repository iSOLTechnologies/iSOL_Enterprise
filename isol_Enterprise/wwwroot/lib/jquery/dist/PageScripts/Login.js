function Login() {
    var abc = 0;
}

Login.prototype.Events = function () {
    var self = this;

    $('#LoginForm').show();

    $(document).on('click', '#closeLogin', function () {
        $('#LoginModal').modal('hide');
    })

    $(document).on('click', '#ConfirmLogin', function () {
        self.ForceLoginUser();
    })


    $(document).on('click', '#btnLogin', function () {
        //debugger
        //SCMApp.StartSpinner($(this));  
        $(this).addClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
        $(this).attr('disabled', true);
        self.LoginUser();
        //SCMApp.StopSpinner($(this));
    })

    $(document).on('click', '#BtnforgetPassword', function () {
        //debugger
        //SCMApp.StartSpinner($(this));  
        $('#LoginForm').hide();
        $('#ForgetForm').show();
    })

    $(document).on('click', '#back-btn', function () {
        //debugger
        //SCMApp.StartSpinner($(this));  
        $('#ForgetForm').hide();
        $('#LoginForm').show();
    })

    $(document).on('click', '#btnSendMail', function () {
        //debugger
        //SCMApp.StartSpinner($(this));  
        self.SendEmail($('#forgetPassword').val());
    })

    $(document).on('click', '#BtnUpdate', function () {
        //debugger
        self.resetpassword();

    })

    //$(document).on('keyup', '#password', function () {
    //    //debugger
    //    self.LoginUser();

    //})

    $(document).on('keyup', '#password', function () {
        //debugger
        if (event.keyCode === 13) {
            self.LoginUser();
        }

    })

    $(document).on('keyup', '#Username', function () {
        //debugger

        if (event.keyCode === 13) {
            self.LoginUser();
        }

    })
    // 
},


    Login.prototype.RecordLogs = function (controllerName, msg) {
        var self = this;
        //debugger

    var jqxhr =  $.getJSON('https://json.geoiplookup.io/?callback=?', function () {
           
        }).done(function (data) {
            //debugger
            console.log(JSON.stringify(data, null, 2));
            var formData = {

                ControllerName: controllerName,
                MsgLog: msg,
                IpAddress: data.ip,
                CountryCode: data.country_code,
                CountryName: data.country_name,
                Location: data.city

            };
            //if (data != null && data != '' && data != undefined) {

                $.ajax({
                    url: '/SystemLogs/RecordLogs',
                    dataType: 'json',
                    type: 'Post',
                    data: formData,
                    async: true,
                    success: function (result) {

                    },
                    error: function (jqXhr, textStatus, errorMessage) {
                        console.log(errorMessage);
                    }

                }).done(function (result) {
                    setTimeout(
                        function () {

                            $('#btnLogin').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
                            $('#btnLogin').attr('disabled', false);
                           
                        }, 300);
                  
                    window.location.href = "/Dashboard";
                });
           // }

        });


        //$.ajax({
        //    url: "https://json.geoiplookup.io/?callback=?",
        //    dataType: 'json',
        //    async: false,
        //    //data: myData,
        //    success: function (data) {

        //    }

        //}).done(function (data) {
        //    //debugger
        //    var formData = {

        //        ControllerName: controllerName,
        //        MsgLog: msg,
        //        IpAddress: data.ip,
        //        CountryCode: data.country_code,
        //        CountryName: data.country_name,
        //        Location: data.city

        //    };
        //    if (data != null && data != '' && data != undefined) {

        //        $.ajax({
        //            url: '/SystemLogs/RecordLogs',
        //            dataType: 'json',
        //            type: 'Post',
        //            data: formData,
        //            async: true,
        //            success: function (result) {

        //            },
        //            error: function (jqXhr, textStatus, errorMessage) {
        //                console.log(errorMessage);
        //            }

        //        }).done(function (result) {


        //        });
        //    }

        //});



    }

Login.prototype.SendEmail = function (email) {
    var self = this;

    var req = $.ajax({
        url: "/Home/SendEmail",
        type: "POST",
        dataType: 'json',
        data: { email: email },
        success: function (resp) {
            if (!resp.isError == true) {
                //  SCMApp.StopSpinner('btnSendMail');
                toastr.success('Email has been Send');
                //window.location.href = "/Dashboard";
            }
            else {
                // SCMApp.StopSpinner('btnSendMail');
                //toastr.error(resp.message);
            }
        },
        error: function (jqXhr, textStatus, errorMessage) { // error callback 
            // $('p').append('Error: ' + errorMessage);
        }
    });
},

    Login.prototype.FieldsData = function () {
        var self = this;
        var FormData = {

            password: $('#Password').val()
            //   Guid: $('#cPassword').val()
        }
        return FormData;
    },


    Login.prototype.resetpassword = function () {
        var self = this;
        //debugger
        var req = $.ajax({
            url: "/Home/UpdatePassword",
            type: "POST",
            dataType: 'json',
            //data: { password: FieldsData },
            data: self.FieldsData(),
            success: function (resp) {
                if (!resp.isError == true) {
                    // SCMApp.StopSpinner('BtnUpdate');
                    toastr.success('Update SuccessFull..');
                    //window.location.href = "/Dashboard";
                }
                else {
                    //    SCMApp.StopSpinner('BtnUpdate');
                    //toastr.error(resp.message);
                }
            },
            error: function (jqXhr, textStatus, errorMessage) { // error callback 
                // $('p').append('Error: ' + errorMessage);
            }
        });
    },

    Login.prototype.LoginUser = function () {
        var self = this;
        if (($('#Username').val() == "") || ($('#password').val() == "")) {
            toastr.error("Username and password are mandatory");
            // SCMApp.StopSpinner('btnLogin');
            return;
        }

        var formData = {
            Username: $('#Username').val(),
            Password: $('#password').val()
        };

        var req = $.ajax({
            url: "/Home/Login",
            type: "POST",
            dataType: 'json',
            data: formData,
            success: function (resp) {
                //debugger
                if (resp.data == true) {
                    //  SCMApp.StopSpinner('btnLogin');
                   // self.RecordLogs("Users", "User Login");
                    window.location.href = "/Dashboard";
                }

                else if (resp.isError) {
                    //    SCMApp.StopSpinner('btnLogin');
                    toastr.error(resp.message);
                    setTimeout(
                        function () {

                            $('#btnLogin').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
                            $('#btnLogin').attr('disabled', false);

                        }, 300);

                }
            },
            error: function (jqXhr, textStatus, errorMessage) {
                $('#btnLogin').removeClass('kt-spinner kt-spinner--right kt-spinner--md kt-spinner--light');
                $('#btnLogin').attr('disabled', false);// error callback
               
            }
        });
    }



var login = new Login();
login.Events();