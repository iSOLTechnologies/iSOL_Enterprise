function Forgetpassword() {
   
}

   Forgetpassword.prototype.Events = function () {
    var self = this;
    $('#LoginForm').show();

    $(document).on('click', '#BtnUpdate', function () {
        debugger
     self.resetpassword();
    })
    // 
},


    

    Forgetpassword.prototype.fielddata = function () {
        var self = this;
        var FormData = {
            password: $('#Password').val(),
            guid: $('#cPassword').val()

        }
        return FormData;
       },

     


    Forgetpassword.prototype.resetpassword = function () {
    var self = this;
    debugger
    var req = $.ajax({
        url: "/Home/UpdatePassword",
        type: "POST",
        dataType: 'json',
        data: { password: fielddata },
        success: function (resp) {
            if (!resp.isError == true) {
                SCMApp.StopSpinner('BtnUpdate');
                toastr.success('Password Updated SuccessFull..');
                window.location.href = "/Home/";
            }
            else {
                SCMApp.StopSpinner('BtnUpdate');
                //toastr.error(resp.message);
            }
        },
        error: function (jqXhr, textStatus, errorMessage) { // error callback 
            // $('p').append('Error: ' + errorMessage);
        }
    });
},

    Forgetpassword.prototype.LoginUser = function () {
        var self = this;
        if (($('#username').val() == "") || ($('#password').val() == "")) {
            toastr.error("Username and password are mandatory");
            SCMApp.StopSpinner('btnLogin');
            return;
        }

        var formData = {
            UserName: $('#username').val(),
            Password: $('#password').val()
        };

        var req = $.ajax({
            url: "/Home/Forgetpassword",
            type: "POST",
            dataType: 'json',
            data: formData, 
            success: function (resp) {
                if (resp.data == true) {
                    SCMApp.StopSpinner('btnLogin');
                    window.location.href = "/Dashboard";
                }
                else if (resp.isError) {
                    SCMApp.StopSpinner('btnLogin');
                    toastr.error(resp.message);
                }
            },
            error: function (jqXhr, textStatus, errorMessage) { // error callback 
                // $('p').append('Error: ' + errorMessage);
            }
        });
    }

var Forgetpassword = new Forgetpassword();
Forgetpassword.Events();