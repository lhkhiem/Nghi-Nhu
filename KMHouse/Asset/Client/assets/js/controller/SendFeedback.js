var feedback = {
    init: function () {
        feedback.regEvents();
    },
    regEvents: function () {
        $('#feedback').validate({
            rules: {
                name: { required: true, },
                email: { required: true, email: true },
                message: { required: true, }
            },
            messages: {
                name: { required: "Chưa nhập tên?" },
                email: { required: "Chưa có email?", email: "Email không hợp lệ" },
                message: { required: "Chưa có nội dung?" },
            }
        });
        $('#submit').off('click').on('click', function (e) {
            e.preventDefault(e);
            if ($('#feedback').valid()) {
                var name = $('#name').val();
                var email = $('#email').val();
                var phone = $('#phone').val();
                var title = $('#title').val();
                var message = $('#message').val();
                $('#info').html('<span class="text-success">Vui lòng chờ giây lát....</span>');
                $.ajax({
                    url: '/Info/SendFeedback',
                    type: 'POST',
                    data: {
                        name: name,
                        email: email,
                        phone: phone,
                        title: title,
                        message: message
                    },
                    dataType: 'json',
                    success: function (res) {
                        if (res.status == true) {
                            $('#info').html('<span class="text-info">Đã gửi thành công!</span>');
                            feedback.resetForm();
                            //setTimeout(function () {
                            //    $('#info').html("");
                            //}, 3000);
                        }
                        else {
                            $('#info').html('<span class="text-danger">Không gửi được. Vui lòng gọi Hotline</span>');
                            //setTimeout(function () {
                            //    $('#info').html("");
                            //}, 3000);
                        }
                    }
                })
            }
        })
    },
    resetForm: function () {
        $('#name').val('');
        $('#email').val('');
        $('#phone').val('');
        $('#subject').val('');
        $('#message').val('');
    }
}
feedback.init();