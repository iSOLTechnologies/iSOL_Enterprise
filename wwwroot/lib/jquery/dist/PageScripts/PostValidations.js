function CheckGlobalRoles(activity) {
    
    var a = null;
    $.ajax({
        url: "/Common/CheckGlobalRoles",//_Administrator/GetRoleDtl
        type: "GET",
        dataType: 'json',
        async: false,
        data: { activity: activity },
        success: function (resp) {
            
            if (resp.isError) {
                toastr.error(resp.message);
                a = false;

            } else if (resp.isAuthorized) {

                a = true;

            } else {
                toastr.error(resp.message)
                a = false;
            }
        },
        error: function (jqXhr, textStatus, errorMessage) {

        }
    });
    
    return a;
}