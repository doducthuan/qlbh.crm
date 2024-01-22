"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("ReceiveNotification", function (noti) {
    if (localStorage.currentLoggedInUser) {
        if (JSON.parse(localStorage.currentLoggedInUser).id == noti.accountId) {
            toastr.info("Bạn vừa nhận được thông báo mới");
            loadNotification(pageIndexNotification, pageSizeNotification);
        }
    }
});

connection.start().then(function () {
    //console.log(item)
}).catch(function (err) {
    return console.error(err.toString());
});

