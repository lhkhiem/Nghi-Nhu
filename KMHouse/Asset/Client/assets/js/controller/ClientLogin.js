var login = {
    init: function () {
        login.regEvents();
    },
    regEvents: function () {
        $('#frmForgetPassword').validate({
            rules: {
                txtEmailForget: { required: true, email: true },
            },
            messages: {
                txtEmailForget: { required: "Chưa nhập email", email: "Email chưa hợp lệ" },
            }
        });
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
        $('#frmRegister').validate({
            rules: {
                txtEmail2: { required: true, email: true },
                txtPassword2: { required: true },
                txtName: { required: true },
                txtAddress: { required: true },
                txtPhone: { required: true },
            },
            messages: {
                txtEmail2: { required: "Chưa nhập email", email: "Email chưa hợp lệ" },
                txtPassword2: { required: "Chưa có mật khẩu" },
                txtName: { required: "Chưa có tên" },
                txtAddress: { required: "Chưa có địa chỉ" },
                txtPhone: { required: "Chưa có số điện thoại" },
            }
        });
        $('#login').off('click').on('click', function () {
            if ($('#frmLogin').valid()) {
                $('#error').html("");
                login.login();
            }
        });
        $('#register').off('click').on('click', function () {
            if ($('#frmRegister').valid()) {
                $('#error2').html("");
                login.register();
            }
        });
        $('#forgetPassword').off('click').on('click', function (e) {
            if ($('#frmForgetPassword').valid()) {
                login.renewPassword();
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
    register: function () {
        var email = $('#txtEmail2').val();
        var password = $('#txtPassword2').val();
        var name = $('#txtName').val();
        var address = $('#txtAddress').val();
        var phone = $('#txtPhone').val();
        var user = {
            Email: email,
            Password: password,
            Name: name,
            Address: address,
            Phone: phone
        }
        $.ajax({
            url: '/UserClient/Register',
            data: {
                model: JSON.stringify(user)
            },
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status == 1) {
                    $('#error2').html('<p class="alert alert-success">' + res.msg + '</p>');
                }
                else {
                    $('#error2').html('<p class="alert alert-warning">' + res.msg + '</p>');
                }
            }
        });
    },
    renewPassword: function () {
        var email = $('#txtEmailForget').val();
        $.ajax({
            url: '/UserClient/ForgetPassword',
            data: {
                email: email
            },
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status == 1) {
                    $('#infoForgetPassword').html('<p class="text text-success">' + res.msg + '</p>');
                }
                else {
                    $('#infoForgetPassword').html('<p class="text text-warning">' + res.msg + '</p>');
                }
            }
        });
    }
}
login.init();