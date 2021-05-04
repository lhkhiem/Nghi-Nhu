var optionconfig = {
    pageSize: $('#sltPageSize').val(),
    pageIndex: 1,
}
var optionController = {
    init: function () {
        optionController.loadData();
        optionController.registerEvent();
    },
    registerEvent: function () {
        $('#frmOption').validate({
            rules: {
                txtName: {
                    required: true,
                },
            },
            messages: {
                txtName: {
                    required: "Bạn phải nhập tên",
                },
            }
        });
        $('#txtKeyword').off('keyup').on('keyup', function (e) {
            optionController.loadData(true);
        });
        $('#frmOption').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                if ($('#frmOption').valid()) {
                    optionController.saveData();
                }
            }
        });
        $('#btnAddNew').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            optionController.resetForm();
        });

        $('#btnSave').off('click').on('click', function () {
            if ($('#frmOption').valid()) {
                optionController.saveData();
            }
        });
        $('#sltPageSize').off('change').on('change', function () {
            optionconfig.pageSize = $(this).val();
            optionController.loadData(true);
        })
        $('#btnSearch').off('click').on('click', function () {
            optionController.loadData(true);
        });
        $('#btnReset').off('click').on('click', function () {
            $('#txtKeyword').val('');
            $('#sltTypeSearch').val(0);
            $('#sltPageSize').val(5);
            optionController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            $('#modalAddUpdate').modal('show');
            var id = $(this).data('id');
            optionController.loadDetail(id);
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                if (result) {
                    optionController.deleteOption(id);
                }
            });
        });
    },
    deleteOption: function (id) {
        $.ajax({
            url: '/Option/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == 1) {
                    optionController.loadData(true);
                    $('#status').text('Deleted');
                    window.setTimeout(function () { $('#status').text('Normal'); }, 2000);
                }
                else if (response.status == 0) {
                    bootbox.alert("Có lỗi xảy ra. Vui lòng thử lại");
                }
                else
                    bootbox.alert("Có Sản phẩm đang dùng. Vui lòng thử lại");
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/Option/GetDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    $('#hidID').val(data.ID);
                    $('#txtName').val(data.Name);
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
    saveData: function () {
        var name = $('#txtName').val();
        var id = parseInt($('#hidID').val());
        var option = {
            Name: name,
            ID: id
        }
        $.ajax({
            url: '/Option/SaveData',
            data: {
                strOption: JSON.stringify(option)
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    $('#modalAddUpdate').modal('hide');
                    optionController.loadData(true);
                    if (response.action == 'insert') {
                        $('#status').text('Add new success');
                        window.setTimeout(function () { $('#status').text('Normal'); }, 2000);
                    }
                    else {
                        $('#status').text('Updated');
                        window.setTimeout(function () { $('#status').text('Normal'); }, 2000);
                    }
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
    resetForm: function () {
        $('#hidID').val('0');
        $('#txtName').val('');
    },
    loadData: function (changePageSize) {
        var keyword = $('#txtKeyword').val();
        var type = $('#sltTypeSearch').val();

        $.ajax({
            url: '/Option/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                type: type,
                pageIndex: optionconfig.pageIndex,
                pageSize: optionconfig.pageSize
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
                            Name: item.Name
                        });
                    });
                    $('#tblData').html(html);
                    $('#lbTotal').text(response.totalCurent + " / " + response.total + ' tổng số bản ghi');
                    optionController.paging(response.total, function () {
                        optionController.loadData();
                    }, changePageSize);
                    optionController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / optionconfig.pageSize);

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
                optionconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}
optionController.init();