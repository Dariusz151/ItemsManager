$(document).keypress(function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        SendRegisterForm();
    }
});


function SendRegisterForm() {
    var fname = $("#register_fname").val();
    var login = $("#register_login").val();
    var email = $("#register_email").val();
    var phone = $("#register_phone").val();

    var pswd_equal = false;
    var pswd = $("#password").val().toString();
    var c_pswd = $("#c_password").val().toString();

    if (pswd === c_pswd)
        pswd_equal = true;

    var data = {};

    data["Login"] = login;
    data["Firstname"] = fname;
    data["Email"] = email;
    data["Phone"] = phone;
    data["Role"] = 3;  //User role
    data["Password"] = pswd;

    if (pswd_equal) {
        $.ajax({
            url: "https://localhost:44378/api/Register",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            success: function (item, textStatus, jqXHR) {
                toastr.success('Added new user!', 'Success');
            },
            error: function (jqXHR, exception) {
                toastr.error('Can\'t add new user!', 'Error');
            }
        });
    }
    else {
        toastr.error('The passwords you entered are different', 'Error');
    }
    ClearForm();
}

function ClearForm() {
    $("#register_fname").val('');
    $("#register_login").val('');
    $("#register_email").val('');
    $("#register_phone").val('');
    $("#password").val('');
    $("#c_password").val('');
}