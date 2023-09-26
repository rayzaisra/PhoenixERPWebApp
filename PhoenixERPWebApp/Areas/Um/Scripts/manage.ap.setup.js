    let bindData = function (stat) {
        let urlApi = $.helper.resolveApi("~ManageApsetup/Get");
        if (stat !== 'list') urlApi = $.helper.resolveApi("~ManageApsetup/Get/" + $('#hidcode').val());
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        }),
            $.ajax({
                type: "GET",
                dataType: 'json',
                url: urlApi,
                success: function (data) {
                    if (data.length === 0) {
                        generateEmptyTable();
                    }
                    else {
                        if (stat === 'list') generateTable(data);
                        else generateFormData(data);
                    }
                },
                error: function (e, t, s) {
                    //swal(
                    //    'Information',
                    //    'Ooops, something went wrong !',
                    //    'info'
                    //);
                    console.log(e);
                }
            }).then(setTimeout(function () {
                mApp.unblockPage();
            }, 2e3));
    }
    let generateTable= function (data) {
        $('#dt_basic> tbody').empty();
        let table = $('#dt_basic > tbody:last-child');
        $.each(data, function (r, val) {
            var row = "<tr>";
            row = row + "<td>" + val.tenantId + "</td>";
            row = row + "<td>" + val.clientId + "</td>";
            row = row + "<td>" + val.emailAccount + "</td>";
            row = row + "<td>" + val.securityPword + "</td>";
            row = row + "</tr>";
            table.append(row);
            $('#hidcode').val(val.id);
        });
    }
    let generateEmptyTable = function (data) {
        $('#dt_basic> tbody').empty();
        let table = $('#dt_basic > tbody:last-child');
        table.append('<tr><td colspan="4" class="text-center">there is no data</td></tr>');
    }
    let formAdd = function () {
        let cd = $('#hidcode').val();
        window.location.href = '/um/manageazureaccount/add/' + cd;
    }
    let bindEditedData = function () {
        let cd = $('#hidcode').val();
        if (cd !== '0') {
            bindData('form');
        }
    };
let generateFormData = function (data) {
    console.log(data);
    $('.in-tenant').val(data.tenantId);
    $('.in-client').val(data.clientId);
    $('.in-email').val(data.emailAccount);
    $('.in-pwd').val(data.securityPword);
    };
    let save = function () {
        if ($('.in-client').val().length === 0 || $('.in-client').val() === '') {
            swal(
                'Information',
                'Client ID cannot empty!',
                'warning'
            );
            return false;
        }
        else if ($('.in-tenant').val().length === 0 || $('.in-tenant').val() === '') {
            swal(
                'Information',
                'Tenant ID / Directory ID cannot empty!',
                'warning'
            );
            return false;
        }
        else if ($('.in-email').val().length === 0 || $('.in-email').val() === '') {
            swal(
                'Information',
                'Email Account cannot empty!',
                'warning'
            );
            return false;
        }
        else if ($('.in-pwd').val().length === 0 || $('.in-pwd').val() === '') {
            swal(
                'Information',
                'Password cannot empty!',
                'warning'
            );
            return false;
        }

        var model = { id: $('#hidcode').val(), clientId: $('.in-client').val(), tenantId: $('.in-tenant').val(), emailAccount: $('.in-email').val(), securityPword : $('.in-pwd').val() };
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        }),
            $.ajax({
                type: "POST",
                dataType: 'json',
                contentType: 'application/json',
            url: $.helper.resolveApi("~/ManageApsetup/Save"),
                data: JSON.stringify(model),
                success: function (r) {
                    Swal.fire({
                        title: 'Information?',
                        text: "Data has been successful saved",
                        type: 'success',
                        showCancelButton: false,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'OK!'
                    }).then((result) => {
                        if (result.value) {
                            window.location.href = "/um/manageazureaccount";
                        }
                    });
                },
                error: function (r) {
                    console.log('masuk error');
                    console.log(r);
                    swal(
                        '' + r.status,
                        r.statusText,
                        'error'
                    );
                }
            }).then(setTimeout(function () {
                mApp.unblockPage();
            }, 2e3));    }
