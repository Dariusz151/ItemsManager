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
    
    fetch(url + "/api/users/Login",
        {
            method: 'POST', 
            body: JSON.stringify(login_data), 
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(res => res.json())
        .then(response => {
            if (response.statusCode == 400) {
                toastr.error('Can\'t log in. Invalid login!', 'Invalid login');
            }
            if (response.token != null) {
                sessionStorage.setItem('token', response.token);

                toastr.options = {
                    "positionClass": "toast-bottom-center"
                }
                toastr.warning('Successfully logged in. Redirecting!', 'Success');

                setTimeout(function () {
                    location.replace(url + "/static/");
                }, 1000);
            }
        })
        .catch(error => {
            console.error('Error:', error)
            toastr.error('Can\'t log in!', 'Error');
        });
}