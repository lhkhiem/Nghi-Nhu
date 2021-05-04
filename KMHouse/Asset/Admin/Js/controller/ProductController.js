var productConfig = {
    pageSize: $('#sltPageSize').val(),
    pageIndex: 1,
    productId: 0
}
var productController = {
    init: function () {
        productController.loadData();
        productController.registerEvent();
    },
    registerEvent: function () {
        //Code cuả trang Index----------------------------------------
        $('#txtKeyword').off('keyup').on('keyup', function (e) {
            productController.loadData(true);
        });
        $('.btnReset').off('click').on('click', function () {
            $('#txtKeyword').val('');
            $('#sltTypeSearch').val(0);
            $('#sltPageSize').val(5);
            productController.loadData(true);
        });
        $('#sltPageSize').off('change').on('change', function () {
            productConfig.pageSize = $(this).val();
            productController.loadData(true);
        })
        $('#btnSearch').off('click').on('click', function () {
            productController.loadData(true);
        });
        $('#ckStatus').on('ifChecked', function (e) {
            productController.loadData(true);
        });
        $('#ckStatus').on('ifUnchecked', function (e) {
            productController.loadData(true);
        });
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                if (result) {
                    productController.deleteUnit(id);
                }
            });
        });

        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Product/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        //btn.text('Kích hoạt');
                        btn.removeClass('lable-danger');
                        btn.addClass('lable-success');
                        productController.loadData();
                    }
                    else {
                        //btn.text('Khóa');
                        btn.removeClass('lable-success');
                        btn.addClass('lable-danger');
                        productController.loadData();
                    }
                }
            });
        });
        $('.btn-images').off('click').on('click', function (e) {
            e.preventDefault();
            $('#imageManage').modal('show');
            $('#hidProductID').val($(this).data('id'));
            productController.loadImages();
        })
        $('#btnChooseImages').off('click').on('click', function (e) {
            e.preventDefault();
            var finder = new CKFinder();
            finder.selectActionFunction = function (url) {
                $('#imageList').append('<div style="float:left"><img src="' + url + '" width="70"/><a href="#" class="btndeleteImage"><i class="fa fa-times"></i></a><div>');
                $('.btndeleteImage').off('click').on('click', function (e) {
                    e.preventDefault();
                    $(this).parent().remove();
                })
            };
            finder.popup();
        });
        $('#btnSaveImages').off('click').on('click', function (e) {
            var images = [];
            var id = $('#hidProductID').val();
            $.each($('#imageList img'), function (i, item) {
                images.push($(item).prop('src'));
            })
            $.ajax({
                url: '/Product/SaveImages',
                type: 'POST',
                data: {
                    id: id,
                    images: JSON.stringify(images)
                },
                dataType: 'json',
                success: function (respone) {
                    if (respone.status) {
                        $('#imageManage').modal('hide');
                        $('#imageList').html('');
                        alert('Cập nhật thành công');
                    }
                }
            });
        });
        $('.btn-option').off('click').on('click', function (e) {
            e.preventDefault();
            productConfig.productId = $(this).data('id');

            productController.setListOption(productConfig.productId);
            productController.loadListOption(productConfig.productId);
            $('#modalOption').modal('show');
        });
        $('#btnOptionAdd').off('click').on('click', function (e) {
            e.preventDefault();
            if ($('#option').val() != null) {
                $('#optionList').append(

                    '<li>' +
                    '<span class="option-item col-md-9">' + $('#option option:selected').text() + '</span>' +
                    '<span>' + $('#optionPrice').val() + 'vnđ</span>' +
                    '<a href="#" data-id="' + $('#option').val() + '" class="optionDelete pull-right"><i class="fa fa-remove"></i></a>' +
                    '</li>'
                );
                productController.insertOption(productConfig.productId, $('#option').val(), $('#optionPrice').val())
                productController.setListOption(productConfig.productId);
            }
            //alert($('#option').val());
        });
        $('#optionList').off('click').on('click', '.optionDelete', function (e) {
            e.preventDefault();
            productController.deleteOption(productConfig.productId, $(this).data('id'));
            productController.setListOption(productConfig.productId);
            productController.loadListOption(productConfig.productId);
        })
        //Code của trang Create và Edit--------------------------------------------------------
        $('#txtMetaTitle').off('click').on('click', function (e) {
            var str = $('#txtName').val();
            productController.convertString(str);
        });
        $('#txtName').off('keyup').on('keyup', function (e) {
            var str = $('#txtName').val();
            productController.convertString(str);
        });
        $('#price').off('keyup').on('keyup', function () {
            $('#convertPrice').text(numeral($(this).val()).format('0,0[.]00') + ' đ');
        });
        $('#price').off('change').on('change', function () {
            $('#convertPrice').text(numeral($(this).val()).format('0,0[.]00') + ' đ');
        });
        $('#promotionPrice').off('keyup').on('keyup', function () {
            $('#convertPromotionPrice').text(numeral($(this).val()).format('0,0[.]00') + ' đ');
        });
        $('#promotionPrice').off('change').on('change', function () {
            $('#convertPromotionPrice').text(numeral($(this).val()).format('0,0[.]00') + ' đ');
        });
    },
    convertString: function (str) {
        $.ajax({
            url: '/Product/ConvertString',
            data: {
                str: str
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                $('#txtMetaTitle').val(response.str);
            }
        });
    },
    setListOption: function (productId) {
        $.ajax({
            url: '/Product/SetListOption',
            data: {
                productId: productId
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var data = response.data;
                var html = '';
                $.each(data, function (i, item) {
                    html += '<option value="' + item.ID + '">' + item.Name + '</>'
                });
                $('#option').html(html);
            }
        });
    },
    loadListOption: function (productId) {
        $.ajax({
            url: '/Product/LoadListOption',
            data: {
                productId: productId
            },
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var data = res.data;
                    var html = '';
                    $.each(data, function (i, item) {
                        html +=
                            '<li>' +
                            '<span class="option-item col-md-9">' + item.OptionName + '</span>' +
                            '<span>' + item.Price + 'vnđ</span>' +
                            '<a href="#" data-id="' + item.OptionID + '" class="optionDelete pull-right"><i class="fa fa-remove"></i></a>' +
                            '</li>';
                    });
                    $('#optionList').html(html);
                }
            }
        });
    },
    insertOption: function (productId, optionId, price) {
        $.ajax({
            url: '/Product/AddProductOption',
            type: 'GET',
            data: {
                productId: productId,
                optionId: optionId,
                price: price
            },
            dataType: 'json',
        });
    },
    deleteOption: function (productId, optionId) {
        $.ajax({
            url: '/Product/DeleteProductOption',
            data: {
                productId: productId,
                optionId: optionId
            },
            type: 'POST',
            dataType: 'json',
        });
    },
    loadImages: function () {
        $.ajax({
            url: '/Product/LoadImages',
            type: 'GET',
            data: {
                id: $('#hidProductID').val()
            },
            dataType: 'json',
            success: function (response) {
                var data = response.data;
                var html = '';
                $.each(data, function (i, item) {
                    html += '<div style="float:left"><img src="' + item + '" width="70" /><a href="#" class="btndeleteImage"><i class="fa fa-times"></i></a></div>'
                });
                $('#imageList').html(html);

                $('.btndeleteImage').off('click').on('click', function (e) {
                    e.preventDefault();
                    $(this).parent().remove();
                });
                //thong bao thanh cong
            }
        });
    },
    convertString: function (str) {
        $.ajax({
            url: '/ProductCategory/ConvertString',
            data: {
                str: str
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                $('#txtMetaTitle').val(response.str);
            }
        });
    },
    dateFormat: function (d) {
        return ((d.getMonth() + 1) + "").padStart(2, "0")
            + "/" + (d.getDate() + "").padStart(2, "0")
            + "/" + d.getFullYear();
    },
    deleteUnit: function (id) {
        $.ajax({
            url: '/Product/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    productController.loadData(true);
                    $('#status').text('Deleted');
                    window.setTimeout(function () { $('#status').text('Normal'); }, 2000);
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    loadData: function (changePageSize) {
        var keyword = $('#txtKeyword').val();
        var type = $('#sltTypeSearch').val();
        var status = $('#ckStatus').prop('checked');
        $.ajax({
            url: '/Product/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                type: type,
                status: status,
                pageIndex: productConfig.pageIndex,
                pageSize: productConfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.ID,
                            Name: item.Name,
                            Description: item.Description,
                            Image: item.Image,
                            CreateDate: productController.dateFormat(new Date(parseInt((item.CreateDate).match(/\d+/)[0]))),
                            CreateBy: item.CreateBy,
                            Price: numeral(item.Price).format('0,0[.]00') + ' đ',
                            ProductCategoryName: item.ProductCategoryName,
                            UnitName: item.UnitName,
                            Status: item.Status == true ? "<a data-id=" + item.ID + " class=\"label label-success btn-active\">Active</a>" : "<a data-id=" + item.ID + " class=\"label label-danger btn-active\">Lock</a>"
                        });
                    });
                    $('#tblData').html(html);
                    $('#lbTotal').text(response.totalCurent + " / " + response.total + ' tổng số bản ghi');
                    productController.paging(response.total, function () {
                        productController.loadData();
                    }, changePageSize);
                    productController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / productConfig.pageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "Đầu",
            next: "Tiếp",
            last: "Cuối",
            prev: "Trước",
            visiblePages: 10,
            onPageClick: function (event, page) {
                productConfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
productController.init();