$(document).ready(function () {
    // getnorange1('120', 'u', 'p');
    $("#1").addClass('kt-menu__item--open');
    $("#118").addClass('kt-menu__item--open');

    $('.BtnSelect').click(function () {//Main1 .RoleCard .BtnSelect 
        var val = $(this).attr('id');

        console.log(val);
        GetRoleDtl(val);
    });

    $("#pagename").change(function () {

        

    });


    $('#kt_quick_panel_toggler_btn').click(function () {
        GetRolesByUser("admin");
    });



    $('#BtnInsert').click(function () {
        
      
        //if (validation() == true) {

        $('#drivernoo option:selected').text($('#driverfullname').val());
        sessionStorage.setItem('drivername', $('#driverfullname').val());
        //sessionStorage.setItem('DUserId', $('#username').val());
        sessionStorage.removeItem("driverno");
        sessionStorage.setItem('driverno', $('#driverno').val());

        $('#MstProfile').children().bind('click', function () { return false; });
        $('#MstProfile').bind('click', function () { return false; });

        $("#Address").children().unbind('click');
        $("#Address").unbind('click');
        $("#Cardlog").children().unbind('click');

        getaddlocat();
        $('#Address').click();

        //post();
        //clearFields()
        // }
        // post();
        //clearFields();
    });

    $('#generalSearch').click(function () {
        SearchName();
    });

});


$(document).on('click', '#Main1 .RoleCard .BtnSelect', function (e) {

    var val = $(this).attr('id');
    var rname = $(this).attr('name');
    $('#RoleName').val(rname);
    GetRoleDtl(val);
    //$('#kt_quick_panel').removeClass('kt-quick-panel--on');
    //$('.kt-quick-panel-overlay').removeClass('kt-quick-panel-overlay');
})

$(document).on('click', '#btnDeleteRow', function () {
    $(this).closest('tr').remove();
})

$(document).on('click', '#btnNewRow', function () {
    AddRow();
})

function AddRow() {
    var clone = $('#template tbody').clone();
    var html = clone.removeClass('hide').html();
    $('#tblItemDetails tbody').append(html);
}


function GetRoleDtl(RoleId) {
    $.ajax({
        url: "/_Administrator/GetRoleDtl",//_Administrator/GetRoleDtl
        type: "GET",
        dataType: 'json',
        data: { RoleId: RoleId },
        success: function (result) {
            console.log(result.data);
            $('#tblItemDetails tbody').empty();

            for (var i = 0; i < result.data.length; i++) {
                AddRow();
                $('#tblItemDetails tbody .rowItemDetail').each(function (index, item) {
                    $(this).find("#pagename").val(result.data[index].pageid);
                    $(this).find("#CanCreate").prop("checked", result.data[index].iscreate);
                    $(this).find("#CanDelete").prop("checked", result.data[index].isdelete);
                    $(this).find("#CanEdit").prop("checked", result.data[index].isedited);
                    //$(this).find("#CanCreate").attr('disabled', true);
                    //$(this).find("#CanDelete").attr('disabled', true);
                    //$(this).find("#CanEdit").attr('disabled', true);
                })
            }


        },
        error: function (jqXhr, textStatus, errorMessage) {
        }
    });
}


function GetRolesByUser(UserId) {
    $.ajax({
        url: "/_Administrator/GetRolesByUser",
        type: "GET",
        dataType: 'json',
        data: { UserId: UserId },
        success: function (result) {

            $("#Main1").empty();
            for (var i = 0; i < result.data.length; i++) {

                document.getElementById('Main1').innerHTML += DivHtml(result.data[i].rolename, result.data[i].roleid, i);
            }
        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }
    });
}

function DivHtml(RoleName, RoleID, index) {
    var html = '<div class="col-md-6 RoleCard">';
    html += '<div class="kt-portlet kt-portlet--height-fluid">';
    html += '       <div class="kt-portlet__body">';
    html += '           <div class="kt-widget kt-widget--user-profile-2">';
    html += '              <div class="kt-widget__body">';
    html += '                  <div class="kt-widget__item">';
    html += '                      <div class="kt-widget__contact"> <span class="kt-widget__label">Role Name:</span> <a id="Vehicle" href="#" class="kt-widget__data coName0" id="">' + RoleName + '</a> </div>';
    html += '                      <div class="kt-widget__contact"> <span class="kt-widget__label">Role ID:</span> <a id="Phone" href="#" class="kt-widget__data" coname0="">' + RoleID + ' </a> </div>';
    html += '                   </div>';
    html += '                </div>';
    html += '                <div style="margin-top:-1%" class="kt-widget__footer"> <a href="javascript:;" class="btn btn-label-danger btn-lg btn-upper BtnSelect" name="' + RoleName + '" id="' + RoleID + '">Select ' + RoleID + '</a> </div>';
    html += '             </div>';
    html += '         </div>';
    html += '  </div>';


    return html;
}

function post() {
    
    var roledtl = {};
    var roledtlList = [];
    var userprofile = {};
    var usermst = {};
    var cashierAddrress = {};

    var url = document.getElementById("useridimage").style.backgroundImage;

    usermst["useridimage"] = url.substring(4, url.length - 1);
    usermst["userid"] = document.getElementById('Username').value;
    var e_UserType = document.getElementById("UserType");
    var strUserType = e_UserType.options[e_UserType.selectedIndex].value;
    usermst["fk_retypemst_reftype"] = strUserType;
    usermst["username"] = document.getElementById('Username').value;
    usermst["password"] = document.getElementById('Password').value;
    usermst["usertype"] = strUserType
    usermst["clientno"] = 'RBMRT'
    usermst["cocode"] = 'RABMA'

    usermst["email"] = document.getElementById('Email').value;
    userprofile["dateofbirth"] = document.getElementById('DOB').value;
    var e_religion = document.getElementById("religion");
    var strreligion = e_religion.options[e_religion.selectedIndex].value;
    userprofile["religion"] = strreligion;
    userprofile["contactperson"] = document.getElementById('contactPerson').value;
    userprofile["contactpersoncontactno"] = document.getElementById('contactNoLandline').value;
    userprofile["phoneno"] = document.getElementById('contactNoCell').value;
    userprofile["client"] = 'RBMRT'
    userprofile["cocode"] = 'RABMA'
    userprofile["contactPerson"] = document.getElementById('contactNoCell').value;
    userprofile["contactno"] = document.getElementById('contactPerson').value;
    userprofile["userfullname"] = document.getElementById('Username').value;

    var e_locationType = document.getElementById("addresstype");
    var strlocationType = e_locationType.options[e_locationType.selectedIndex].value;

    cashierAddrress["addresstype"] = strlocationType;
    cashierAddrress["street1"] = document.getElementById('street1').value;
    cashierAddrress["contactPerson"] = document.getElementById('contactPersonLocation').value;
    cashierAddrress["address1"] = document.getElementById('street2').value;
    cashierAddrress["emergencynumber"] = document.getElementById('emergencyContact').value;
    cashierAddrress["address3"] = document.getElementById('street3').value;
    cashierAddrress["contactpersonnumbe"] = document.getElementById('contactNumber').value;
    cashierAddrress["cityname"] = document.getElementById('City').value;
    cashierAddrress["phone1"] = document.getElementById('primaryLandline').value;
    cashierAddrress["countryid"] = document.getElementById('country').value;
    cashierAddrress["phone2"] = document.getElementById('alternateLandline').value;
    cashierAddrress["zone"] = document.getElementById('zone').value;
    cashierAddrress["mobile1"] = document.getElementById('primaryMobile').value;
    //userprofile["state"] = document.getElementById('state').value;
    cashierAddrress["mobile2"] = document.getElementById('alternateMobile').value;
    cashierAddrress["region"] = document.getElementById('region').value;
    cashierAddrress["fax1"] = document.getElementById('fax').value;
    cashierAddrress["latitude"] = document.getElementById('latitude').value;
    cashierAddrress["email1"] = document.getElementById('email').value;
    cashierAddrress["longitude"] = document.getElementById('longitude').value;
    cashierAddrress["pobox"] = document.getElementById('pobox').value;
    cashierAddrress["openingtime"] = document.getElementById('openingtime').value;
    cashierAddrress["closingtime"] = document.getElementById('closingtime').value;






    //usermst["fk_retypemst_reftypeno"] = document.getElementById('UserNo').value;
    //usermst["clientno"] = 'RBMRT';
    //usermst["cocode"] = 'RABMA';







    
    //userprofile["userno"] = document.getElementById('UserNo').value;
    //userprofile["userfullname"] = document.getElementById('FullName').value;
    //userprofile["dateofbirth"] = document.getElementById('DOB').value;
    //var e_religion = document.getElementById("religion");
    //var strreligion = e_religion.options[e_religion.selectedIndex].value;
    //userprofile["religion"] = strreligion;
    //userprofile["contactno"] = document.getElementById('ContactNo').value;
    //userprofile["phoneno"] = document.getElementById('PhoneNo').value;
    //userprofile["contactperson"] = document.getElementById('ContactPerson').value;
    //userprofile["ContactPersonNo"] = document.getElementById('ContactPersonNo').value;
    //var e_lang = document.getElementById("langno");
    //var strLang = e_lang.options[e_lang.selectedIndex].value;
    //userprofile["langno"] = strLang;
    //userprofile["cocode"] = 'RABMA';
    //userprofile["client"] = 'RBMRT';


    for (var i = 0; i < $('#tblItemDetails .rowItemDetail').length; i++) {

        roledtl["roleid"] = document.getElementById('RoleId').value
        //roledtl["pageid"] = $('#tblItemDetails .rowItemDetail').eq(i).find("#pagename option:selected").val();
        roledtl["pageid"] = $('#tblItemDetails .rowItemDetail').eq(i).find("#pagename").children("option:selected").val();

        roledtl["iscreate"] = $('#tblItemDetails .rowItemDetail').eq(i).find("#CanCreate").prop("checked");
        roledtl["isdelete"] = $('#tblItemDetails .rowItemDetail').eq(i).find("#CanDelete").prop("checked");
        roledtl["isedited"] = $('#tblItemDetails .rowItemDetail').eq(i).find("#CanEdit").prop("checked");
        roledtlList.push(roledtl);
    }

    var model = {};
    model["roledtl"] = roledtlList;
    model["userprofile"] = userprofile;
    model["usermst"] = usermst;
    //It is already being already saved in database on that page
    //model["cashieraddress"] = cashierAddrress;
    model["rolename"] = document.getElementById('RoleName').value;
    model["roleid"] = document.getElementById('RoleId').value;

    // console.log(model);
    //$.ajax({
    //    url: "/api/_ApiAdministrator/Add",
    //    type: "post",
    //    data: model
    //});
    $.post('/api/_ApiAdministrator/Add', model, function (response) {
        console.log(response);
        //$("#detailPartialView").html(response); 
    });

    //var rolehdr = {};
    //var rolehdrList = [];
    //for (var i = 0; i < $('#tblItemDetails .rowItemDetail').length; i++) {
    //    rolehdr["roleid"] = document.getElementById('RoleId').value
    //    rolehdr["pageid"] = $('#tblItemDetails .rowItemDetail').eq(i).find("#PageName option:selected").val();
    //    rolehdr["iscreate"] = $('#tblItemDetails .rowItemDetail').eq(i).find("#CanCreate").prop("checked");
    //    rolehdr["isdelete"] = $('#tblItemDetails .rowItemDetail').eq(i).find("#CanDelete").prop("checked");
    //    rolehdr["isedited"] = $('#tblItemDetails .rowItemDetail').eq(i).find("#CanEdit").prop("checked");
    //    rolehdrList.push(rolehdr);
    //}


    //var item = {};
    //item["username"] = document.getElementById('Username').value;
    //item["employeeno"] = document.getElementById('Employeename').value;
    //item["email"] = document.getElementById('Email').value;
    //item["password"] = document.getElementById('Password').value;
    //item["UserNo"] = document.getElementById('UserNo').value;
    //item["FullName"] = document.getElementById('FullName').value;
    //item["DOB"] = document.getElementById('DOB').value;
    //item["Religion"] = document.getElementById('Religion').value;
    //item["ContactNo"] = document.getElementById('ContactNo').value;
    //item["PhoneNo"] = document.getElementById('PhoneNo').value;
    //item["ContactPerson"] = document.getElementById('ContactPerson').value;
    //item["ContactPersonNo"] = document.getElementById('ContactPersonNo').value;
    //item["cocode"] = document.getElementById('ContactPersonNo').value;
    //var e_UserType = document.getElementById("UserType");
    //var strUser = e_UserType.options[e_UserType.selectedIndex].value;
    //item["UserType"] = strUser;
    //item["RoleName"] = document.getElementById('RoleName').value;
    //item["RoleId"] = document.getElementById('RoleId').value;
    //item["UserId"] = document.getElementById('UserId').value;
    //item["ListRoleDtl"] = rolehdrList;
    //var json = JSON.stringify(item);

    //$.post('/api/_ApiAdministrator/Add', item, function (response) {
    //    console.log(response);
    //    //$("#detailPartialView").html(response); 
    //});

}

function add() {
    var cashierAddrress = {};
    var e_locationType = document.getElementById("addresstype");
    var strlocationType = e_locationType.options[e_locationType.selectedIndex].value;

    cashierAddrress["clinet"] = 'RBMRT';
    cashierAddrress["cocode"] = 'RABMA';
    cashierAddrress["addresstype"] = strlocationType;
    cashierAddrress["street1"] = document.getElementById('street1').value;
    cashierAddrress["contactPerson"] = document.getElementById('contactPersonLocation').value;
    cashierAddrress["address1"] = document.getElementById('street2').value;
    cashierAddrress["emergencynumber"] = document.getElementById('emergencyContact').value;
    cashierAddrress["address3"] = document.getElementById('street3').value;
    cashierAddrress["contactpersonnumbe"] = document.getElementById('contactNumber').value;
    cashierAddrress["cityname"] = document.getElementById('City').value;
    cashierAddrress["phone1"] = document.getElementById('primaryLandline').value;
    cashierAddrress["countryid"] = document.getElementById('country').value;
    cashierAddrress["phone2"] = document.getElementById('alternateLandline').value;
    cashierAddrress["zone"] = document.getElementById('zone').value;
    cashierAddrress["mobile1"] = document.getElementById('primaryMobile').value;
    //userprofile["state"] = document.getElementById('state').value;
    cashierAddrress["mobile2"] = document.getElementById('alternateMobile').value;
    cashierAddrress["region"] = document.getElementById('region').value;
    cashierAddrress["fax1"] = document.getElementById('fax').value;
    cashierAddrress["latitude"] = document.getElementById('latitude').value;
    cashierAddrress["email1"] = document.getElementById('email').value;
    cashierAddrress["longitude"] = document.getElementById('longitude').value;
    cashierAddrress["pobox"] = document.getElementById('pobox').value;
    cashierAddrress["openingtime"] = document.getElementById('openingtime').value;
    cashierAddrress["closingtime"] = document.getElementById('closingtime').value;

    $.post('/api/_ApiAdministrator/AddLocation', cashierAddrress, function (response) {
        
        console.log(response);
        if (response) {

            $("#gridid").append("<tr><td>  " + $("#addresstype option:selected").text() + "   </td><td>    " + document.getElementById('street1').value + "   </td><td>   " + document.getElementById('street2').value + "    </td><td>    " + document.getElementById('region').value + "   </td><td>       </td></tr>");
        }
        //$("#detailPartialView").html(response); 
    });

}

//function validation() {
//     post();
//}
//return true;
    //toastr.remove();
    //var letters = /^[0-9a-zA-Z]+$/;

    //if ($('#Username').val() == "" || $('#Username').val() == null) {

    //    var flag = false;
    //    setInterval(function () {
    //        if (!flag) {
    //            flag = true;//store this to compare later
    //            toastr.warning("Username is mendatory");
    //        }
    //    }, 1);
    //}
    //else {
    //    if ($('#Username').val().match(letters)) {
    //    }
    //    else {

    //        var flag1 = false;
    //        setInterval(function () {
    //            if (!flag1) {
    //                flag1 = true;//store this to compare later
    //                toastr.warning('Please type alphanumeric characters only in Username');
    //            }
    //        }, 1);
    //    }
    //}
    //if ($('#Username').val().length > 50) {


    //    var flag50 = false;
    //    setInterval(function () {
    //        if (!flag50) {
    //            flag50 = true;//store this to compare later
    //            toastr.warning("User name must be less than 51 digits");
    //        }
    //    }, 1);
    //}
    //if ($('#Email').val() == "" || $('#Email').val() == null) {

    //    var flag2 = false;
    //    setInterval(function () {
    //        if (!flag2) {
    //            flag2 = true;//store this to compare later
    //            toastr.warning("Email is mendatory");
    //        }
    //    }, 1);
    //}
    //else {
    //    if ($('#Email').val().match(letters)) {
    //    }
    //    else {

    //        var flag3 = false;
    //        setInterval(function () {
    //            if (!flag3) {
    //                flag3 = true;//store this to compare later
    //                toastr.warning('Please type alphanumeric characters only in Email');
    //            }
    //        }, 1);
    //    }
    //}
    //if ($('#Email').val().length > 50) {


    //    var flag51 = false;
    //    setInterval(function () {
    //        if (!flag51) {
    //            flag51 = true;//store this to compare later
    //            toastr.warning("Email must be less than 51 digits");
    //        }
    //    }, 1);
    //}
    //if ($('#Password').val() == "" || $('#Password').val() == null) {

    //    var flag4 = false;
    //    setInterval(function () {
    //        if (!flag4) {
    //            flag4 = true;//store this to compare later
    //            toastr.warning("Password is mendatory");
    //        }
    //    }, 1);
    //}
    //else {
    //    if ($('#Password').val().match(letters)) {
    //    }
    //    //else {

    //    //    var flag5 = false;
    //    //    setInterval(function () {
    //    //        if (!flag5) {
    //    //            flag5 = true;//store this to compare later
    //    //            toastr.warning('Please type alphanumeric characters only in Password');
    //    //        }
    //    //    }, 1);
    //    //}
    //}
    //if ($('#Password').val().length > 30) {


    //    var flag52 = false;
    //    setInterval(function () {
    //        if (!flag52) {
    //            flag52 = true;//store this to compare later
    //            toastr.warning("Password must be less than 31 digits");
    //        }
    //    }, 1);
    //}
    //if ($('#UserNo').val() == "" || $('#UserNo').val() == null) {

    //    var flag6 = false;
    //    setInterval(function () {
    //        if (!flag6) {
    //            flag6 = true;//store this to compare later
    //            toastr.warning("UserNo is mendatory");
    //        }
    //    }, 1);
    //}
    //else {
    //    if ($('#UserNo').val().match(letters)) {
    //    }
    //    else {

    //        var flag7 = false;
    //        setInterval(function () {
    //            if (!flag7) {
    //                flag7 = true;//store this to compare later
    //                toastr.warning('Please type alphanumeric characters only in UserNo');
    //            }
    //        }, 1);
    //    }
    //}
    //if ($('#UserNo').val().length > 20) {


    //    var flag53 = false;
    //    setInterval(function () {
    //        if (!flag53) {
    //            flag53 = true;//store this to compare later
    //            toastr.warning("User no must be less than 21 digits");
    //        }
    //    }, 1);
    //}
    //if ($('#FullName').val() == "" || $('#FullName').val() == null) {

    //    var flag8 = false;
    //    setInterval(function () {
    //        if (!flag8) {
    //            flag8 = true;//store this to compare later
    //            toastr.warning("FullName is mendatory");
    //        }
    //    }, 1);
    //}
    //else {
    //    if ($('#FullName').val().match(letters)) {
    //    }
    //    else {

    //        var flag9 = false;
    //        setInterval(function () {
    //            if (!flag9) {
    //                flag9 = true;//store this to compare later
    //                toastr.warning('Please type alphanumeric characters only in FullName');
    //            }
    //        }, 1);
    //    }
    //}
    //if ($('#FullName').val().length > 50) {


    //    var flag54 = false;
    //    setInterval(function () {
    //        if (!flag54) {
    //            flag54 = true;//store this to compare later
    //            toastr.warning("Fullname must be less than 51 digits");
    //        }
    //    }, 1);
    //}
    //if ($('#DOB').val() == "" || $('#DOB').val() == null) {

    //    var flag10 = false;
    //    setInterval(function () {
    //        if (!flag10) {
    //            flag10 = true;//store this to compare later
    //            toastr.warning("DOB is mendatory");
    //        }
    //    }, 1);
    //}
    ////else {
    ////    if ($('#DOB').val().match(letters)) {
    ////    }
    //    //else {

    //    //    var flag11 = false;
    //    //    setInterval(function () {
    //    //        if (!flag11) {
    //    //            flag11 = true;//store this to compare later
    //    //            toastr.warning('Please type alphanumeric characters only in DOB');
    //    //        }
    //    //    }, 1);
    //    //}
    ////}
    //if ($('#religion').val() == "" || $('#religion').val() == null) {

    //    var flag12 = false;
    //    setInterval(function () {
    //        if (!flag12) {
    //            flag12 = true;//store this to compare later
    //            toastr.warning("religion is mendatory");
    //        }
    //    }, 1);
    //}
    //else {
    //    if ($('#religion').val().match(letters)) {
    //    }
    //    //else {

    //    //    var flag13 = false;
    //    //    setInterval(function () {
    //    //        if (!flag13) {
    //    //            flag13 = true;//store this to compare later
    //    //            toastr.warning('Please type alphanumeric characters only in religion');
    //    //        }
    //    //    }, 1);
    //    //}
    //}
    //if ($('#ContactNo').val() == "" || $('#ContactNo').val() == null) {

    //    var flag14 = false;
    //    setInterval(function () {
    //        if (!flag14) {
    //            flag14 = true;//store this to compare later
    //            toastr.warning("ContactNo is mendatory");
    //        }
    //    }, 1);
    //}
    //else {
    //    if ($('#ContactNo').val().match(letters)) {
    //    }
    //    //else {

    //    //    var flag15 = false;
    //    //    setInterval(function () {
    //    //        if (!flag15) {
    //    //            flag15 = true;//store this to compare later
    //    //            toastr.warning('Please type alphanumeric characters only in ContactNo');
    //    //        }
    //    //    }, 1);
    //    //}
    //}
    //if ($('#ContactNo').val().length > 25) {


    //    var flag55 = false;
    //    setInterval(function () {
    //        if (!flag55) {
    //            flag55 = true;//store this to compare later
    //            toastr.warning("Contact no must be less than 26 digits");
    //        }
    //    }, 1);
    //}
    //if ($('#PhoneNo').val() == "" || $('#PhoneNo').val() == null) {

    //    var flag16 = false;
    //    setInterval(function () {
    //        if (!flag16) {
    //            flag16 = true;//store this to compare later
    //            toastr.warning("PhoneNo is mendatory");
    //        }
    //    }, 1);
    //}
    //else {
    //    if ($('#PhoneNo').val().match(letters)) {
    //    }
    //    //else {

    //    //    var flag17 = false;
    //    //    setInterval(function () {
    //    //        if (!flag17) {
    //    //            flag17 = true;//store this to compare later
    //    //            toastr.warning('Please type alphanumeric characters only in PhoneNo');
    //    //        }
    //    //    }, 1);
    //    //}
    //}
    //if ($('#PhoneNo').val().length > 25) {


    //    var flag56 = false;
    //    setInterval(function () {
    //        if (!flag56) {
    //            flag56 = true;//store this to compare later
    //            toastr.warning("Phone no must be less than 26 digits");
    //        }
    //    }, 1);
    //}
    //if ($('#ContactPerson').val() == "" || $('#ContactPerson').val() == null) {

    //    var flag18 = false;
    //    setInterval(function () {
    //        if (!flag18) {
    //            flag18 = true;//store this to compare later
    //            toastr.warning("ContactPerson is mendatory");
    //        }
    //    }, 1);
    //}
    //else {
    //    if ($('#ContactPerson').val().match(letters)) {
    //    }
    //    else {

    //        var flag19 = false;
    //        setInterval(function () {
    //            if (!flag19) {
    //                flag19 = true;//store this to compare later
    //                toastr.warning('Please type alphanumeric characters only in ContactPerson');
    //            }
    //        }, 1);
    //    }
    //}
    //if ($('#ContactPerson').val().length > 50) {


    //    var flag57 = false;
    //    setInterval(function () {
    //        if (!flag57) {
    //            flag57 = true;//store this to compare later
    //            toastr.warning("Contact person must be less than 51 digits");
    //        }
    //    }, 1);
    //}
    //if ($('#langno').val() == "" || $('#langno').val() == null) {

    //    var flag20 = false;
    //    setInterval(function () {
    //        if (!flag20) {
    //            flag20 = true;//store this to compare later
    //            toastr.warning("langno is mendatory");
    //        }
    //    }, 1);
    //}
    //else {
    //    if ($('#langno').val().match(letters)) {
    //    }
    //    else {

    //        var flag21 = false;
    //        setInterval(function () {
    //            if (!flag21) {
    //                flag21 = true;//store this to compare later
    //                toastr.warning('Please type alphanumeric characters only in langno');
    //            }
    //        }, 1);
    //    }
    //}
    //if ($('#ContactPersonNo').val() == "" || $('#ContactPersonNo').val() == null) {

    //    var flag22 = false;
    //    setInterval(function () {
    //        if (!flag22) {
    //            flag22 = true;//store this to compare later
    //            toastr.warning("ContactPersonNo is mendatory");
    //        }
    //    }, 1);
    //}
    //else {
    //    if ($('#ContactPersonNo').val().match(letters)) {
    //    }
    //    //else {

    //    //    var flag23 = false;
    //    //    setInterval(function () {
    //    //        if (!flag23) {
    //    //            flag23 = true;//store this to compare later
    //    //            toastr.warning('Please type alphanumeric characters only in ContactPersonNo');
    //    //        }
    //    //    }, 1);
    //    //}
    //}
    //if ($('#ContactPersonNo').val().length > 25) {


    //    var flag58 = false;
    //    setInterval(function () {
    //        if (!flag58) {
    //            flag58 = true;//store this to compare later
    //            toastr.warning("Contact person no must be less than 26 digits");
    //        }
    //    }, 1);
    //}
    //if ($('#UserType').val() == "" || $('#UserType').val() == null) {

    //    var flag24 = false;
    //    setInterval(function () {
    //        if (!flag24) {
    //            flag24 = true;//store this to compare later
    //            toastr.warning("UserType is mendatory");
    //        }
    //    }, 1);
    //}
    ////else {
    ////    if ($('#UserType').val().match(letters)) {
    ////    }
    ////    //else {

    ////    //    var flag25 = false;
    ////    //    setInterval(function () {
    ////    //        if (!flag25) {
    ////    //            flag25 = true;//store this to compare later
    ////    //            toastr.warning('Please type alphanumeric characters only in UserType');
    ////    //        }
    ////    //    }, 1);
    ////    //}
    ////}
    ////if ($('#UserType').val().length > 5) {


    ////    var flag59 = false;
    ////    setInterval(function () {
    ////        if (!flag59) {
    ////            flag59 = true;//store this to compare later
    ////            toastr.warning("User type must be less than 6 digits");
    ////        }
    ////    }, 1);
    ////}
    //if ($('#UserId').val() == "" || $('#UserId').val() == null) {

    //    var flag26 = false;
    //    setInterval(function () {
    //        if (!flag26) {
    //            flag26 = true;//store this to compare later
    //            toastr.warning("UserId is mendatory");
    //        }
    //    }, 1);
    //}
    //else {
    //    if ($('#UserId').val().match(letters)) {
    //    }
    //    //else {

    //    //    var flag27 = false;
    //    //    setInterval(function () {
    //    //        if (!flag27) {
    //    //            flag27 = true;//store this to compare later
    //    //            toastr.warning('Please type alphanumeric characters only in UserId');
    //    //        }
    //    //    }, 1);
    //    //}
    //}
    //if ($('#UserId').val().length > 15) {


    //    var flag60 = false;
    //    setInterval(function () {
    //        if (!flag60) {
    //            flag60 = true;//store this to compare later
    //            toastr.warning("User id must be less than 16 digits");
    //        }
    //    }, 1);
    //}
    //if ($('#RoleName').val() == "" || $('#RoleName').val() == null) {

    //    var flag28 = false;
    //    setInterval(function () {
    //        if (!flag28) {
    //            flag28 = true;//store this to compare later
    //            toastr.warning("RoleName is mendatory");
    //        }
    //    }, 1);
    //}
    //else {
    //    if ($('#RoleName').val().match(letters)) {
    //    }
    //    else {

    //        var flag29 = false;
    //        setInterval(function () {
    //            if (!flag29) {
    //                flag29 = true;//store this to compare later
    //                toastr.warning('Please type alphanumeric characters only in RoleName');
    //            }
    //        }, 1);
    //    }
    //}
    //if ($('#RoleName').val().length > 50) {


    //    var flag61 = false;
    //    setInterval(function () {
    //        if (!flag61) {
    //            flag61 = true;//store this to compare later
    //            toastr.warning("Role name must be less than 51 digits");
    //        }
    //    }, 1);
    //}
    //if ($('#RoleId').val() == "" || $('#RoleId').val() == null) {

    //    var flag30 = false;
    //    setInterval(function () {
    //        if (!flag30) {
    //            flag30 = true;//store this to compare later
    //            toastr.warning("RoleId is mendatory");
    //        }
    //    }, 1);
    //}
    //else {
    //    if ($('#RoleId').val().match(letters)) {
    //    }
    //    else {

    //        var flag31 = false;
    //        setInterval(function () {
    //            if (!flag31) {
    //                flag31 = true;//store this to compare later
    //                toastr.warning('Please type alphanumeric characters only in RoleId');
    //            }
    //        }, 1);
    //    }
    //}
    //if ($('#RoleId').val().length > 5) {


    //    var flag62 = false;
    //    setInterval(function () {
    //        if (!flag62) {
    //            flag62 = true;//store this to compare later
    //            toastr.warning("Role id must be less than 6 digits");
    //        }
    //    }, 1);
    //}
    //if ($('#pagename').val() == "" || $('#pagename').val() == null) {

    //    var flag32 = false;
    //    setInterval(function () {
    //        if (!flag32) {
    //            flag32 = true;//store this to compare later
    //            toastr.warning("pagename is mendatory");
    //        }
    //    }, 1);
    //}
    //else {
    //    if ($('#pagename').val().match(letters)) {
    //    }
    //    else {

    //        var flag33 = false;
    //        setInterval(function () {
    //            if (!flag33) {
    //                flag33 = true;//store this to compare later
    //                toastr.warning('Please type alphanumeric characters only in pagename');
    //            }
    //        }, 1);
    //    }
    //}
    //if ($('#pagename').val().length > 50) {


    //    var flag63 = false;
    //    setInterval(function () {
    //        if (!flag63) {
    //            flag63 = true;//store this to compare later
    //            toastr.warning("Page name must be less than 51 digits");
    //        }
    //    }, 1);
    //}
    //if ($('#Username').val() != "" && $('#Username').val().match(letters) && $('#Username').val().length < 51 && $('#Email').val() != "" && $('#Email').val().length < 51 && $('#Password').val() != "" && $('#Password').val().length < 31 && $('#UserNo').val() != "" && $('#UserNo').val().match(letters) && $('#UserNo').val().length < 21 && $('#FullName').val() != "" && $('#FullName').val().match(letters) && $('#FullName').val().length < 51 && $('#DOB').val() != "" && $('#religion').val() != "" && $('#ContactNo').val() != "" && $('#ContactNo').val().match(letters) && $('#ContactNo').val().length < 26 && $('#PhoneNo').val() != "" && $('#PhoneNo').val().match(letters) && $('#PhoneNo').val().length < 26 && $('#ContactPerson').val() != "" && $('#ContactPerson').val().match(letters) && $('#ContactPerson').val().length < 51 && $('#langno').val() != "" && $('#ContactPersonNo').val() != "" && $('#ContactPersonNo').val().match(letters) && $('#ContactPersonNo').val().length < 26 && $('#UserType').val() != "" && $('#UserType').val().match(letters)  && $('#UserId').val() != "" && $('#UserId').val().match(letters) && $('#UserId').val().length < 16 && $('#RoleName').val() != "" && $('#RoleName').val().match(letters) && $('#RoleName').val().length < 51 && $('#RoleId').val() != "" && $('#RoleId').val().match(letters) && $('#RoleId').val().length < 6 && $('#pagename').val() != "" && $('#pagename').val().match(letters) && $('#pagename').val().length < 51 ) {
    //    post();

    //}
//}


function clearFields() {

    var formData = {
        username: $('#Username').val(null),
        userid: $('#Username').val(null),
        email: $('#Email').val(null),
        Password: $('#Password').val(null),
        UserNo: $('UserNo').val(null),
        FullName: $('#FullName').val(null),
        DOB: $('#DOB').val(null),
        religion: $('#religion').val(null),
        ContactNo: $('#ContactNo').val(null),
        PhoneNo: $('#PhoneNo').val(null),
        ContactPerson: $('#ContactPerson').val(null),
        langno: $('#langno').val(null),
        ContactPersonNo: $('#ContactPersonNo').val(null),
        UserType: $('#UserType').val(null),
        UserType: $('#UserType').val(null),
        UserId: $('#UserId').val(null),
        RoleName: $('#RoleName').val(null),
        RoleId: $('#RoleId').val(null),
        pagename: $('#pagename').val(null),
        pagename: $('#pagename').val(null)
    };
    return formData;
}
