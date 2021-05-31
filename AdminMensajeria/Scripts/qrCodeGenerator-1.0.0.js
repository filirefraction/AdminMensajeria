function generateQrCode(size, value, name){
    // Clear Previous QR Code
    $('#' + name ).empty();

    // Set Size to Match User Input
    $('#' + name).css({
        'width': size,
        'height': size
    })

    // Generate and Output QR Code
    $('#' + name).qrcode({ width: size, height: size, text: value });

}