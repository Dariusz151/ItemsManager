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
                console.log('succes". Dodano: ' + login);
                /*
                * DODAC OBSLUGE ZGODNYCH HASEL
                * WYCZYSCIC FORMULARZ PO SUKCESIE (DODANIU UZYTKOWNIKA)
                * */
            },
            error: function (jqXHR, exception) {
                console.log('err"');
            }
        });
    }
    else {
        /*
         * DODAC OBSLUGE NIEZGODNYCH HASEL
         * */
        console.log("Podane hasla roznia sie od siebie!");
    }
}