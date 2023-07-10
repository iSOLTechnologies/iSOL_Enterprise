
var UserPageActivityData = "";
String.prototype.GetUserPageActivity = function () {
    // Custom method logic

    return $.ajax({
        url: 'Common/GetUserPageActivity',
        type: 'GET',
        dataType: 'json',
        async: false,
        data: { Guid: this },
        success: function (result) {
            console.log(result);

        },
        error: function (jqXhr, textStatus, errorMessage) {
            console.log(errorMessage);
        }
    }).done(function (result) {

        UpdatePageActivities(result.data);
        UserPageActivityData = result.data;
    });
};
function UpdatePageActivities(data) {
    for (var i = 0; i < data.length; i++) {


        if (data[i].roleActivityTypeCode == 'A' && !data[i].isActive) {
            $("#btnAdd").addClass("d-none");
        }

        if (data[i].roleActivityTypeCode == 'E' && !data[i].isActive) {
            $(".btnEdit").addClass("d-none");
        }
        if (data[i].roleActivityTypeCode == 'D' && !data[i].isActive) {
            $(".btnDelete").addClass("d-none");
        }
        if (data[i].roleActivityTypeCode == 'V' && !data[i].isActive) {
            $(".btnView").addClass("d-none");
        }

    }
}
$(document).on('click', '.paginate_button.page-item', function () {
    console.log('click');
    UpdatePageActivities(UserPageActivityData);
});

