let saveTemplateApprovalHris = function (data, url) {
    let model = {
        Id:data.Id,
        RefId: data.refId,
        DetailLink: data.detailLink,
        Tname: data.tName,
        MenuId: data.menuId,
        IdTemplate: data.IdTemplate,
        NotificationType: data.NotificationType,
        Module: data.Module
    };
    let urlApi = $.helper.resolveApi("~ManageTransApproval/Save");

   $.ajax({
       type: 'POST',
       url: urlApi,
       dataType: 'json',
       contentType: 'application/json',
        data: JSON.stringify(model),
        success: function (result) {
            mApp.unblock("#m_blockui_list");
            window.location.href = url;
        },
        error: function (e, t, s) {
            //console.log(e)
            console.log(e);
            mApp.unblock("#m_blockui_list");
        }
    });
};


let UpdateStatusApprovalHris = function (data, url) {
    let model = {
        Id : data.Id,
        RefId: data.refId,
        StatusApproved : data.StatusApproved,
        Remarks : data.Remarks
    };
    let urlApi = $.helper.resolveApi("~ManageTransApproval/UpdateStatusApproval");

    $.ajax({
        type: 'POST',
        url: urlApi,
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(model),
        success: function (result) {
            mApp.unblock("#m_blockui_list");
            window.location.href = url;
        },
        error: function (e, t, s) {
           console.log(e)
            mApp.unblock("#m_blockui_list");
        }
    });
};


let saveTemplateApprovalPm = function (data, url) {
    let model = {
        RefId: data.refId,
        DetailLink: data.detailLink,
        Tname: data.tName,
        MenuId: data.menuId
    };
    let urlApi = $.helper.resolveApi("~ManageTransApproval/SavePm");

    $.ajax({
        type: 'POST',
        url: urlApi,
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(model),
        success: function (result) {
            mApp.unblock("#m_blockui_list");
        },
        error: function (e, t, s) {
           console.log(e)
            mApp.unblock("#m_blockui_list");
        }
    });
};
let saveTemplateApprovalFinance = function (data, url) {
    let model = {
        RefId: data.refId,
        DetailLink: data.detailLink,
        Tname: data.tName,
        MenuId: data.menuId,
        Budget:data.budget
    };
    let urlApi = $.helper.resolveApi("~ManageTransApproval/SaveFn");

    $.ajax({
        type: 'POST',
        url: urlApi,
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(model),
        success: function (result) {
            mApp.unblock("#m_blockui_list");
        },
        error: function (e, t, s) {
           console.log(e)
            mApp.unblock("#m_blockui_list");
        }
    });
};