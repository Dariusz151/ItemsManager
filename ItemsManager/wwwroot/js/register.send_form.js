$(document).keypress(function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        SendRegisterForm();
    }
});

//TODO: fix changes (error codes etc.)

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
    data["Password"] = pswd;
    
    if (pswd_equal) {
        fetch(url + "/api/users/Register",
            {
                method: 'POST', 
                body: JSON.stringify(data),
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(res => res.json())
            .then(response => {
                if (response.statusCode == 400) {
                    var responseStatus = response.statusDescription;
                    switch (responseStatus) {
                        case "UserNull":
                            toastr.error('User is null!', 'Error');
                            break;
                        case "LoginEmpty":
                            toastr.error('Login field is empty!', 'Error');
                            break;
                        case "PasswordEmpty":
                            toastr.error('Password field is empty!', 'Error');
                            break;
                        case "FirstnameEmpty":
                            toastr.error('Firstname field is empty!', 'Error');
                            break;
                        default:
                            toastr.error('Can\'t register!', 'Unknown Error');
                            break;
                    }
                }
                else if (response.statusCode == 200) {
                    toastr.options = {
                        "positionClass": "toast-bottom-center"
                    }
                    toastr.warning('Successfully registered. Please log in!', 'Success');

                    setTimeout(function () {
                        location.replace(url + "/static/Login.html");
                    }, 1000);
                }
                else {
                    toastr.error('Can\'t register!', 'Unknown Error');
                    console.log(response);
                }
            })
            .catch(error => {
                console.error('Error:', error)
                toastr.error('Can\'t register!', 'Error');
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