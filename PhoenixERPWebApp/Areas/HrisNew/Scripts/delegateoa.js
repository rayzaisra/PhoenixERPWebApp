let openFormApp = function () {
    sessionStorage.setItem('codex','0');
    window.location.href = "/hrisnew/delegateofauthority/RequestDoa";
};

let bindRequest = function () {
    let urlApi = $.helper.resolveApi("~doa/GetListRequest");

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
                    bindEmptyTable();
                }
                else {
                    bindTableRequest(data);
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
};
let bindNeedApproval = function () {
    var key = $('.searchbox').val();
    let urlApi = $.helper.resolveApi("~doa/GetListNeedApproval/" + key);
    if (key === '' || key === null || key === undefined)
        urlApi = $.helper.resolveApi("~doa/GetListNeedApproval");

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
                    bindEmptyTable();
                }
                else {
                    bindTableNeedApproval(data);
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
};

let bindEmptyTable = function () {
    $('#dt_basic> tbody').empty();
    let table = $('#dt_basic > tbody:last-child');
    table.append('<tr><td colspan="7" class="text-center">there is no data</td></tr>');
};

let bindTableRequest = function (data) {
    
    $('#dt_basic> tbody').empty();
    let table = $('#dt_basic > tbody:last-child');
    $.each(data, function (r, val) {
        let reasonRejectAssign = '';
        if (val.reasonRejectAssign === null) reasonRejectAssign = '';
        else reasonRejectAssign = val.reasonRejectAssign;

        let isAssignToApproved = 'Approved';
        if (val.isAssignToApproved === null) isAssignToApproved = '';
        else if (val.isAssignToApproved === false) isAssignToApproved = 'Rejected';

        var row = "<tr>";
        row = row + "<td>" + val.assignTo + "</td>";
        row = row + "<td>" + val.reasonDoa + "</td>";
        row = row + "<td>" + val.fromDateToString + "</td>";
        row = row + "<td>" + val.toDateToString + "</td>";
        row = row + "<td>" + isAssignToApproved + "</td>";
        row = row + "<td>" + reasonRejectAssign + "</td>";
        row = row + "<td>";
        if (val.isAssignToApproved === false) {
            //row = row + "<button class='m-btn btn btn-default btn-sm m-btn--sm pull-right' title='edit' type='button' onclick='editedData(\"" + val.id + "\")'><i class='fa fa-edit'></i></button>";
        }
        else if (val.isAssignToApproved === null) {
            row = row + "<button class='m-btn btn btn-danger btn-sm m-btn--sm pull-right' title='delete' type='button' onclick='removeData(\"" + val.id + "\")'><i class='fa fa-trash-alt'></i></button>";
            //row = row + "<button class='m-btn btn btn-default btn-sm m-btn--sm pull-right' title='edit' type='button' onclick='editedData(\"" + val.id + "\")'><i class='fa fa-edit'></i></button>";
        }
        row = row + "</td>";
        row = row + "</tr>";
        table.append(row);
    });
};

let bindTableNeedApproval = function (data) {
    $('#dt_basic> tbody').empty();
    let table = $('#dt_basic > tbody:last-child');
    $.each(data, function (r, val) {
        var row = "<tr>";
        row = row + "<td>" + val.assignFrom + "</td>";
        row = row + "<td>" + val.reasonDoa + "</td>";
        row = row + "<td>" + val.fromDateToString + "</td>";
        row = row + "<td>" + val.toDateToString + "</td>";

        if (val.isAssignToApproved === false || val.isAssignToApproved === true) {
            var statApproved = 'Rejected';
            if (val.isAssignToApproved) statApproved = 'Approved';
            row = row + "<td>You have to " + statApproved + " this request</td>";
        }
        else {
            row = row + "<td>";
            row = row + "<button class='m-btn btn btn-danger btn-sm m-btn--sm pull-right' title='take rejected delegate' type='button' onclick='expandcollapseDetail(\"trT" + val.id + "\")'><i class='fa fa-ban'></i></button>";
            row = row + "<button class='m-btn btn btn-default btn-sm m-btn--sm pull-right' title='take approved delegate' type='button' onclick='approvedDelegate(\"" + val.id + "\",\"2\")'><i class='fa fa-check'></i></button>";
            row = row + "</td>";
        }
        row = row + "</tr>";

        if (val.isAssignToApproved === null) {
            row = row + "<tr>";
            row = row + "<td colspan='5'><div id='trT" + val.id + "' style='position: relative; display: none;'>";
            row = row + "<label class='label-control'>Reason Rejected</label>";
            row = row + "<textarea class='form-control in-reason-rejected'></textarea><br>";
            row = row + "&nbsp;&nbsp;<button class='btn btn-sm btn-danger pull-right' onclick='approvedDelegate(\"" + val.id + "\",\"1\")'>Post Rejected</button>&nbsp;&nbsp;";
            row = row + "<button class='btn btn-sm btn-default pull-right' onclick='expandcollapseDetail(\"trT" + val.id + "\")'>Hide</button>&nbsp;&nbsp;";
            row = row + "</div></td> ";
            row = row + "</tr>";
        }

        table.append(row);
    });
};

let editedData = function (id) {
    sessionStorage.setItem('codex', id);
    window.location.href = "/hrisnew/delegateofauthority/RequestDoa";
};
let backtolist = function (id) {
    window.location.href = "/hrisnew/delegateofauthority";
};

let bindEditRequest = function () {
    var codex = sessionStorage.getItem('codex');
    $('#hidId').val(codex);
    if (codex !== '0') {
        let urlApi = $.helper.resolveApi("~doa/GetById/" + codex);
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
                $('#ddlEmpApp').val(data.assignTo);
                $('.tgl-mulai').val(data.fromDateToString);
                $('.tgl-selesai').val(data.toDateToString);
                $('#Tbreason').val(data.reasonDoa);
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
};

let saveRequest = function () {
    var code = $('#hidId').val();
    var ddl = $('#ddlEmpApp').val();
    var tglmulai = $('.tgl-mulai').val();
    var tglselesai = $('.tgl-selesai').val();
    var reason = $('#Tbreason').val();

    if (ddl === '0') {
        swal(
            'Information',
            'Please Select Delegate User!',
            'error'
        );
        return false;
    }
    if (reason.length === 0) {
        swal(
            'Information',
            'Reason cannot empty!',
            'error'
        );
        return false;
    }
    if (tglmulai.length === 0) {
        swal(
            'Information',
            'Start From Date cannot empty!',
            'error'
        );
        return false;
    }

    if (tglselesai.length === 0) {
        swal(
            'Information',
            'End To Date cannot empty!',
            'error'
        );
        return false;
    }

    var model = {
        Id: code,
        AssignFrom: null,
        AssignTo: ddl,
        FromDate: tglmulai,
        ToDate: tglselesai,
        IsAssignToApproved: null,
        ReasonDoa: reason,
        ReasonRejectAssign: null
    };

    $.ajax({
        type: "POST",
        dataType: 'json',
        contentType: 'application/json',
        url: $.helper.resolveApi("~/doa/Save"),
        data: JSON.stringify(model),
        success: function (r) {
            window.location.href = "/hrisnew/delegateofauthority";
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
    });
};

let bindEmpAppSelect = function () {
    let urlApi = $.helper.resolveApi("~ManageUser/GetUserForDoa");

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
            var ddl = $('#ddlEmpApp');
            ddl.empty();
            $(ddl).append($('<option>', {
                value: '0',
                text: 'SELECT DELEGATE USER'
            }));
            var ds = sessionStorage.getItem('data');
            ds = JSON.parse(ds);
            $.each(data, function (i, item) {
                if (ds.AppUserId !== item.id) {
                    $(ddl).append($('<option>', {
                        value: item.id,
                        text: item.aliasName
                    }));
                }
                });

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
        }, 355));
};

let approvedDelegate = function (idx, stat) {
    //status note :
    //1 = Rejected
    //2 = Approved
    let statusApproved = false;
    if (stat === "2") statusApproved = true;

    var model = {
        Id: idx,
        IsAssignToApproved: statusApproved,
        ReasonRejectAssign :''
    };

    let msgApproved = "You won't take approved this delegation!";
    let tipeApproved = 'info';
    let confirmButtonTxt = 'Yes, approved it!';
    if (stat === "1") {
        var reason = $('.in-reason-rejected').val();
        if (reason.length === 0 || reason === '') {
            swal(
                'Information',
                'Reason rejected cannot empty!',
                'error'
            );
            return false;
        }
        model.ReasonRejectAssign = reason;
        msgApproved = "You won't rejected this delegation!";
        tipeApproved = 'warning';
        confirmButtonTxt = 'Yes, rejected it!';
    }

    Swal.fire({
        title: 'Are you sure?',
        text: msgApproved,
        type: tipeApproved,
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: confirmButtonTxt
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                dataType: 'json',
                contentType: 'application/json',
                url: $.helper.resolveApi("~/doa/Approved"),
                data: JSON.stringify(model),
                success: function (r) {
                    bindNeedApproval();
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
            });
        }
    });



};

let expandcollapseDetail = function (divname) {
    var div = document.getElementById(divname);
    if (div.style.display === "none") {
        $(div).show(1300);
        div.style.display = "block";
    } else {
        $(div).hide(1300);
        div.style.display = "none";
    }
};
