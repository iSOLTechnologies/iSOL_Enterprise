var SCMApp = {

    activateSideBarMenuItemParent: function (Id) {
        $('#' + Id).addClass('start active open');
    },
    activateSideBarMenuItem: function (Id) {
        $('#' + Id).addClass('active open');       
    },

    StartSpinner: function (button)
    {
        var l = Ladda.create(document.querySelector('#' + $(button).attr('id')));
        l.start();
    },

    StopSpinner: function (buttonId) {
        var l = Ladda.create(document.querySelector('#' + buttonId));
        l.stop();
    },

    FormatNumber: function (num, culture = 'en-US') {        
        return Number(num).toLocaleString(culture);
    },
    
    FormatNumberWithDecimal: function (num, culture = 'en-US') {      
        return Number(num).toLocaleString(culture, { style: 'decimal', maximumFractionDigits: 2, minimumFractionDigits: 2 });
    },

    GetSelect2ValuesString: function(id) {
        var self = this;
        if ($('#' + id).val() == undefined)
            return "";
        else
            return $('#' + id).val().toString();
    },
    
    SetSelect2ValuesString: function (id,data) {
        var self = this;
        
        return $('#' + id).val(data).toString();
    }
}