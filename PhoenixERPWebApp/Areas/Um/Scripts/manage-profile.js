$(function () {
    //getImageProfile();
    bindProfile();
});

function gotoprofile() {
    var empId = $('#EmployeeId').val();

    window.location.href = WEB + "/hrisnew/EmployeeBasic/AddEdit?Id=" + empId + "&paramcol=&paramval=&isReadOnly=1"
}

let saveImageProfile = function () {
    var files = $("#fu").get(0).files;
    if (files.length === 0) {
        return swal(
            'Information',
            'Ooops, there is no file for upload, please select file first before upload!',
            'warning'
        );
    }

    var frmData = new FormData();
    frmData.append("File", files[0]);
    frmData.append("Path", "");
    frmData.append("PathFullWithFileName", "IMAGESPROFILE/");
    frmData.append("IsBig", false);

    let urlApi = $.helper.resolveApi("~odrive/uploadProfile");
    $.ajax({
        url: urlApi,
        type: "POST",
        cache: false,
        processData: false,
        contentType: false,
        data: frmData,
        success: function (responseData) {
            window.location.href = "/um/manageuser/profile";

        },
        error: function (error) { console.log(error); }
    });

};
let getImageProfile = function () {
    let urlApi = $.helper.resolveApi("~odrive/GetImageProfile");
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
            console.log(data);
            if (data.webUrl !== null) {
                $("#img-proflie-nava").attr("src",data.webUrl);            
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

let bindProfile = function () {
    let urlApi = $.helper.resolveApi("~ManageUser/GetProfileInfo");
    

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
                console.log(data);
                let urlApiEmployeeBasic = $.helper.resolveApi("~EmployeeBasic/" + data.employeeId);
                $('#EmployeeId').val(data.employeeId);
            if(data.employeeName === null) $('.h-name').text("-");
            else  $('.h-name').text(data.employeeName.toUpperCase());

            if(data.jobTitleName === null)$('.sp-title').text("-");
            else $('.sp-title').text(data.jobTitleName.toUpperCase());

            if(data.email=== null)$('.sp-email').text("-");
            else $('.sp-email').text(data.email.toUpperCase());

            if(data.nickName === null)$('.sp-nick').text("-");
            else $('.sp-nick').text(data.nickName.toUpperCase());

            if(data.gender=== null)$('.sp-gender').text("-");
            else $('.sp-gender').text(data.gender.toUpperCase());

            if(data.divisiName === null)$('.sp-divisi').text("-");
            else $('.sp-divisi').text(data.divisiName.toUpperCase());
    
            if(data.departmentName=== null)$('.sp-dept').text("-");
            else $('.sp-dept').text(data.departmentName.toUpperCase());

            $.ajax({
                type: "GET",
                dataType: 'json',
                url: urlApiEmployeeBasic,
                success: function (result) {
                    console.log(result.data.accountName);
                    $('#Nationality').text(result.data.nationalityLabel);
                    $('#KITASNo').text(result.data.kitasNumber);
                    $('#KITASExpiredDate').text(result.data.kitasExpiry);
                    $('#Religion').text(result.data.religionLabel);
                    $('#MarriedStatusId').text(result.data.marriageLabel);
                    $('#Blood').text(result.data.blood);
                    $('#PostCode').text(result.data.postCode);
                    $('#AddressIdentityCard').text(result.data.addressIdentityCard);
                    $('#HomeAddress').text(result.data.homeAddress);
                    $('#LastEducation').text(result.data.lastEducationLabel);
                    $('#PassportNumber').text(result.data.passportNumber);
                    $('#PassportExpiry').text(result.data.passportExpiry);
                    $('#NpwpNumber').text(result.data.npwpNumber);
                    $('#IdentityCard').text(result.data.identityCard);
                    $('#KkNo').text(result.data.kkNo);


                    $('#JoinDate').text(moment(result.data.joinDate).format('DD-MM-YYYY'));
                    $('#JobTitle').text(result.data.jobTitleLabel);
                    $('#ReportTo').text(result.data.reportToLabel);
                    $('#Grade').text(result.data.gradeLabel);
                    $('#LegalEntity').text(result.data.legalEntityLabel);
                    $('#EmployementStatus').text(result.data.employmentStatusLabel);
                    $('#Location').text(result.data.locationLabel);
                    if (result.data.employementStatusEndDate !== null) {
                        $('#ContractEndDate').text(moment(result.data.employementStatusEndDate).format('DD-MM-YYYY'));
                    } 
                 

                    let urlApi3 = $.helper.resolveApi("~odrive/GetImageEmployeeProfile");
                    var id = result.data.photoFileName;
                    if (id !== null) {
                        $.ajax({
                            type: "GET",
                            dataType: 'json',
                            url: urlApi3,
                            data: { id: id },
                            success: function (data) {
                                console.log(data);
                                if (data.webUrl !== null) {
                                    $("#img-proflie-nava").attr("src", data.webUrl);
                                }
                            },
                            error: function (e, t, s) {
                                console.log(e);
                                //swal(
                                //    'Information',
                                //    'Ooops, something went wrong !',
                                //    'info'
                                //);
                            }
                        }).then(setTimeout(function () {
                            mApp.unblockPage();
                        }, 2e3));
                    }
                }

            })

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



