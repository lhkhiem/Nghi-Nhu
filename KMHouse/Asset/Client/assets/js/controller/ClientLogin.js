var login = {
    init: function () {
        login.regEvents();
    },
    regEvents: function () {
        $('#frmLogin').validate({
            rules: {
                txtEmail: { required: true, email: true },
                txtPassword: { required: true },
            },
            messages: {
                txtEmail: { required: "Chưa nhập email", email: "Email chưa hợp lệ" },
                txtPassword: { required: "Chưa có mật khẩu" },
            }
        });
        $('#login').off('click').on('click', function () {
            if ($('#frmLogin').valid()) {
                $('#error').html("");
                login.login();
            }
        });
    },
    login: function () {
        var email = $('#txtEmail').val();
        var password = $('#txtPassword').val();
        var user = {
            Email: email,
            Password: password
        }
        $.ajax({
            url: '/UserClient/Login',
            data: {
                model: JSON.stringify(user)
            },
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status == 1) {
                    location.reload();
                }
                else {
                    $('#error').html('<p class="alert alert-warning">' + res.msg + '</p>');
                }

            }
        });
    },
}
login.init();