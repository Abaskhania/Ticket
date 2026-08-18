const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/notifications")
    .withAutomaticReconnect()
    .build();

connection.on("NotificationReceived", function (notification) {
    console.log("New notification:", notification);
    
    //alert(notification.message);
    //location.reload();
    //window.showToast(notification.message);
    //$(".swal2-confirm").click(function () { location.reload() });
    let originalTitle = document.title;
    let interval = setInterval(() => {
        document.title =
            document.title === originalTitle ? "🔔 پیام جدید!" : originalTitle;
    }, 1000);
    Swal.fire({
        title: 'عملیات موفق',
        text: notification.message,
        icon: 'success',
        confirmButtonText: 'باشه',
        confirmButtonColor: '#28a745',
        customClass: {
            confirmButton: 'my-confirm-button'
        },
        didClose: () => {
            connection.invoke("MarkAsRead", notification.id)
                .catch(err => console.error(err));
            location.reload();
            // Your code here
        }
    }).then((result) => {
        if (result.isConfirmed) {
            // کدی که باید بعد از کلیک روی تأیید اجرا شود
            
            connection.invoke("MarkAsRead", notification.id)
                .catch(err => console.error(err));
            location.reload();
            // مثلاً:
            // submitForm();
        }
    });
});

connection.start()
    .then(() => {
        console.log("SignalR Connected");
    })
    .catch(err => {
        console.error("SignalR Error:", err);
    });