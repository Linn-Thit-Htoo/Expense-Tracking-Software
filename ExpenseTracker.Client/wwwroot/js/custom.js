//toast
function toastMessage(message) {
    //promis->then
    return new Promise(function (myResolve, myReject) {
        Toastify({
            text: message, // Use the input message here
            offset: {
                x: 50, // horizontal axis - can be a number or a string indicating unity. eg: '2em'
                y: 10 // vertical axis - can be a number or a string indicating unity. eg: '2em'
            },
        }).showToast();
    });
}
