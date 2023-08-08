var ApprMaxID = 0;
function GetTotalApprovals() {

    $.ajax({
        url: 'Common/GetTotalApprovals',
        type: 'GET',
        dataType: 'json',
        async: false,
        success: function (data)
        {
            

        },
    }).done(function (data)
    {
        ApprMaxID = data.maxId;
        $(".ApprNotNum").text(data.totalNotifications);
    });

}
function GetNewApprovals() {
    
    $.ajax({
        url: 'Common/GetNewApprovals',
        type: 'GET',
        dataType: 'json',
        data: { ApprMaxID: ApprMaxID },
        async: false,
        success: function (data)
        {
            

        },

    }).done(function (data) {
        ApprMaxID = data.maxId;
        $(".ApprNotNum").text(data.totalNotifications);

        
        if (data.isNew) {
            $("#btnApprNot").trigger('click');
            PlayNotificationBeep();

        }
    });
    setTimeout(GetNewApprovals, 2000);

}


function PlayNotificationBeep() {
    var beep = new Audio("assets/media/sounds/notification.wav");
    beep.play();
}

// Call the function to play the beep sound

