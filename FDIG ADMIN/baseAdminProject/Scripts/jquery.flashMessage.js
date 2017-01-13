function jGrowlNotification(message, options) {
    var defaultOptions = { message: null, life: 5000, position: "bottom-right", closerTemplate: "<div>[ Cerrar todas ]</div>", closeTemplate: '<i class="icon-remove"/>' };
    if (message == null) {
        options = defaultOptions;
        setFlashMessageFromCookie(options);
        if (options.message != null)
            $.jGrowl(options.message, defaultOptions);
    }
    else {
        if (!!options.position) defaultOptions.position = options.position;
        if (!!options.themeState) defaultOptions.themeState = options.themeState;
        $.jGrowl(message, defaultOptions);
    }
}

// Get the first alert message read from the cookie
function setFlashMessageFromCookie(defaultOptions) {
    $.each(new Array('Success', 'Error', 'Warning', 'Info'), function (i, aAlert) {

        var options = defaultOptions;
        var cookie = $.cookie("Flash." + aAlert);
        var cookieLife = Number($.cookie("Flash." + aAlert + ".life"));

        if (cookie) {
            if ((aAlert == "error") && (cookieLife == null))
                cookieLife = 60000;
            options.message = cookie;
            if (!isNaN(cookieLife) && (cookieLife > 0)) options.life = cookieLife;
            options.themeState = aAlert.toLowerCase();
            deleteFlashMessageCookie(aAlert);
            return;
        }
    });
}

// Delete the named flash cookie
function deleteFlashMessageCookie(alert) {
    $.cookie("Flash." + alert, null, { path: '/' });
    $.cookie("Flash." + alert + ".life", null, { path: '/' });
}

$(function () {
    jGrowlNotification();
});