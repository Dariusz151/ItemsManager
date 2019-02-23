var articlesTable = [];

$(document).keypress(function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        SendLoginForm();
    }
});

function SendLoginForm() {
    var login = $("#login_login").val();
    var pswd = $("#login_password").val();
    var login_data = {};
    login_data.Login = login;
    login_data.Password = pswd;
    
    fetch(url + "/api/Login",
        {
            method: 'POST', 
            body: JSON.stringify(login_data), 
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(res => res.json())
        .then(response => {
            if (response.statusCode == 400) {
                var responseStatus = response.statusDescription;
                switch (responseStatus) {
                    case "ObjectNull":
                        toastr.error('Object is null!', 'Error');
                        break;
                    case "LoginEmpty":
                        toastr.error('Login field is empty!', 'Error');
                        break;
                    case "PasswordEmpty":
                        toastr.error('Password field is empty!', 'Error');
                        break;
                    default:
                        toastr.error('Can\'t logg in!', 'Unknown Error');
                        break;
                }
            }
            else if (response.statusCode == 200) {
                sessionStorage.setItem("userID", response.userID);
                sessionStorage.setItem("userName", login);
                toastr.options = {
                    "positionClass": "toast-bottom-center"
                }
                toastr.warning('Successfully logged in. Redirecting!', 'Success');
                
                setTimeout(function () {
                    location.replace(url + "/static/");
                }, 1000);
            }
            else {
                toastr.error('Can\'t log in!', 'Unknown Error');
                console.log(response);
            }
        })
        .catch(error => {
            console.error('Error:', error)
            toastr.error('Can\'t log in!', 'Error');
        });
}