var addCart = {
    init: function () {
        addCart.regEvents();
    },
    regEvents: function () {
        $(window).on("load", function () {
            addCart.loadMiniCart();
        });
        $('.addCart').off('click').on('click', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var quantity = $('#quantity').val();
            if (quantity == null) quantity = 1;
            $.ajax({
                url: '/Cart/AddItem',
                type: 'GET',
                data: {
                    productId: id,
                    quantity: quantity
                },
                dataType: 'json',
                success: function (res) {
                    if (res.status == 1) {
                        //var box = bootbox.dialog({
                        //    message: '<p class="text-center">Đã thêm thành công</p>',
                        //    size: "small",
                        //    closeButton: false,
                        //});
                        //setTimeout(function () {
                        //    box.modal('hide');
                        //}, 1500);
                        addCart.loadMiniCart();
                    }
                    else {
                        var box = bootbox.dialog({
                            message: '<p class="text-center mb-0">Sản phẩm chưa có giá,</br>Vui lòng liên hệ shop!</p><p class="text-center mb-0"><i class="fa fa-info" style="font-size:72px;"></i></p>',
                            size: "small",
                            closeButton: false,
                        });
                        setTimeout(function () {
                            box.modal('hide');
                        }, 2000);
                    }
                }
            })
        });
        //Xóa item mini cart
        $('#listItem').on('click', '.btn-delete', function (e) {
            var btn = $(this);
            $.ajax({
                data: { id: $(this).data('id') },
                url: '/Cart/Delete',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        btn.parent().remove();
                        addCart.loadMiniCart();
                    }
                }
            })
        });
        //Xóa item cart index
        $('.btn-delete-item').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: { id: $(this).data('id') },
                url: '/Cart/Delete',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = '/gio-hang'
                    }
                }
            })
        })
    },
    loadMiniCart: function () {
        $.ajax({
            url: '/Cart/LoadMiniCart',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.listItem != null) {
                    var data = res.listItem;
                    var totalPrice = 0;
                    var totalItem = 0;
                    var name = "";
                    var listItem = "";
                    $.each(data, function (i, item) {
                        totalItem += 1;
                        var price = 0;
                        if (item.Product.PromotionPrice != null) price = item.Product.PromotionPrice * item.Quantity;
                        else price = item.Product.Price * item.Quantity;
                        totalPrice += price;
                        name = item.Product.Name;
                        //if (item.Product.Name.length > 10) name = item.Product.Name.substring(0, 10) + "..."; else name = item.Product.Name;

                        //listItem3 += "<li class='" + item.Product.ID + "'>" +
                        //    "<a href='/chi-tiet/" + item.Product.MetaTitle + "-" + item.Product.ID + "' class='minicart-product-image'>" +
                        //    "<img src='" + item.Product.Image + "' alt='cart products'></a>" +
                        //    "<div class='minicart-product-details'>" +
                        //    "<h6><a href='/chi-tiet/" + item.Product.MetaTitle + "-" + item.Product.ID + "'>" + name + "</a></h6>" +
                        //    "<span>" + numeral(price).format('0,0[.]00') + " đ" + " x " + item.Quantity + "</span></div>" +
                        //    "<button class='close btn-delete' data-id='" + item.Product.ID + "'><i class='fa fa-close'></i></button>" +
                        //    "</li> ";
                        listItem += '<div class="product ' + item.Product.ID + '">' +
                            '<div class="product-cart-details">' +
                            '<h4 class="product-title">' +
                            '<a href="/chi-tiet/' + item.Product.MetaTitle + '-' + item.Product.ID + '">' + name + '</a>' +
                            '</h4>' +
                            '<span class="cart-product-info">' +
                            '<span class="cart-product-qty">' + item.Quantity + '</span> x ' + numeral(price).format('0,0[.]00') + '</span>' +
                            '</div >' +

                            '<figure class="product-image-container">' +
                            '<a href="/chi-tiet/' + item.Product.MetaTitle + '-' + item.Product.ID + '" class="product-image">' +
                            '<img src="' + item.Product.Image + '" alt="' + item.Product.Name + '" style="width:60px; heigh:60px">' +
                            '</a>' +
                            '</figure>' +
                            '<a href="#" data-id="' + item.Product.ID + '" class="btn-remove btn-delete" title="Xóa"><i class="icon-close"></i></a>' +
                            '</div>';
                    })
                    $('#quantityMiniCart').html(totalItem);
                    $('#totalMiniCart').html(numeral(totalPrice).format('0,0[.]00') + ' đ');
                    $('#listItem').html(listItem);
                }
            }
        });
    },
}
addCart.init();