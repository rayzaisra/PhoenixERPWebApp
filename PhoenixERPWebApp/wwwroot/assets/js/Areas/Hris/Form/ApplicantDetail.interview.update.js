var ajaxUrl = API + "/Hris/CandidateRecruitmentInterview";
var ajaxUrlList = ajaxUrl + "/List";
var ajaxUrlUpdate = ajaxUrl + "/Update";
var ajaxUrlCreate = ajaxUrl + "/Create";
var ajaxUrlDelete = ajaxUrl + "/Delete";
var ajaxUrlGet = ajaxUrl + "/Get";
var ajaxUrlGetCentral = API + "/Hris/JobVacancy/Get";
var ajaxUrlGetCandidateReq = API + "/Hris/CandidateRecruitment/Get";
var ajaxUrlGetHiringRequest = API + "Hris/HiringRequest/GetListHiringMppWithID";
var ajaxUrlGetEducationInfo = API + "/Hris/CandidateRecruitmentEducationInfo/Get";
var tableList = "EducationInfoList";
var formSearch = "ApplicantProgressFormSearch";
var modalSearch = "ApplicantProgressModalSearch";
var ajaxUrlListJobTitle = API + "Hris/JobTitle/List/";
var pageheader = "";
var paramsSearchIndex = function () {
    var searchdata = $("#" + formSearch).serializeArray();
    var data = { Search_status: 1 };
    $(searchdata).each(function (index, obj) {
        if (obj.name !== "__RequestVerificationToken") {
            data[obj.name] = obj.value;
        }
    });

    return data;
};


$(function () {
    $('#master_location_id').setcombobox({
        data: { 'rows': 100 },
        url: API + "Hris/MasterLocation/List",
        searchparam: '',
        labelField: 'location_name',
        valueField: 'id',
        searchField: 'location_name'
    });

    initlanguage(lang_hr);

    var xid = $('#id').val();
    var candidate_recruitment_id = $('#candidate_recruitment_id').val();
    var job_vacancy_id = $('#job_vacancy_id').val();

    var xform = "InterviewFormUpdate";
    var optionsForm = GetOptionsForm(function () {
        var xret = $("#" + xform).valid();
        if (xret) startprocess();
        return xret;
    }, function (response, statusText, xhr, $form) {
        endprocess();
        response = parseJson(response);
        if (response.success) {
            SuccessNotif(t_createsuccess);
            window.location.href = WEB + "/hris/ApplicantDetail/Interview/" + candidate_recruitment_id + "/" + job_vacancy_id;
        } else {
            DangerNotif(response.message);
        }
    });
    InitForm(xform, optionsForm);
    setFormAction(xform, ajaxUrlUpdate);

    ajaxGet(ajaxUrlGetCentral + "/" + job_vacancy_id, {}, function (response) {
        response = parseJson(response);
        //console.log(response);
        if (response.data) {
            $("#vacancy_name").val(response.data.vacancy_name);
            $("#posting_code").val(response.data.code);

            var dataraw = '';
            if (response.data.posting_status === "0") {
                dataraw = "DRAFT";
            } else if (response.data.posting_status === "1") {
                dataraw = "APPROVED";
            } else if (response.data.posting_status === "2") {
                dataraw = "REJECT";
            } else if (response.data.posting_status === "3") {
                dataraw = "UNPOSTING";
            } else if (response.data.posting_status === "4") {
                dataraw = "POSTED";
            } else {
                dataraw = "CLOSED";
            }
            $("#posting_status").val(dataraw);
            ajaxGet(ajaxUrlGetCandidateReq + "/" + candidate_recruitment_id, {}, function (response) {
                response2 = parseJson(response);
                //console.log(response2);
                if (response2.data) {
                    var dataraw = '';
                    if (response2.data.applicant_progress_id.trim() === "0") {
                        dataraw = "New Applicant";
                    } else if (response2.data.applicant_progress_id.trim() === "1") {
                        dataraw = "Interview";
                    } else if (response2.data.applicant_progress_id.trim() === "2") {
                        dataraw = "Job Offered";
                    } else if (response2.data.applicant_progress_id.trim() === "3") {
                        dataraw = "Approval";
                    } else if (response2.data.applicant_progress_id.trim() === "4") {
                        dataraw = "Accepted";
                    } else if (response2.data.applicant_progress_id.trim() === "5") {
                        dataraw = "KIV / Tallent Pool";
                    } else {
                        dataraw = "Rejected";
                    }

                    $("#recruitment_status").val(dataraw);
                    $("#applicant_code").val(response2.data.code);
                } else {
                    DangerNotif("No data candidate found for selected ID.");
                }
                

            }); } else { DangerNotif("No data job vacancy found for selected ID."); }
       

        ajaxGet(ajaxUrlGet + "/" + xid, {}, function (response) {
            response3 = parseJson(response);
            //console.log(response2);
            if (response3.data) {


                $('#date_interview').val(moment(response3.data.date_interview).format("MM/DD/YYYY"));
                $('#interviewer').val(response3.data.interviewer);
                $('#interview_to').val(response3.data.interview_to);
                var subject = response3.data.subject.replace("Invite ", "");
                $('#subject').val(subject);
                
                $('#action_plan').val(response3.data.subject);
                $('#time_from_hour').val(response3.data.time_from_hour);
                $('#time_from_minutes').val(response3.data.time_from_minutes);
                $('#time_to_hour').val(response3.data.time_to_hour);
                $('#time_to_minutes').val(response3.data.time_to_minutes);
                $('#interview_from').val(response3.data.interview_from);
                $('#content').val(response3.data.content);
                if (response3.data.master_location_id) {
                    ajaxGet(API + "Hris/MasterLocation/Get/" + response3.data.master_location_id, {}, function (response) {
                        response = parseJson(response);
                        //console.log(response);
                        $('#master_location_id').append('<option value="' + response3.data.master_location_id+ '" selected>' + response.data.location_name + '</option>');
                        $("#map_lat").val(response.data.map_lat);
                        $("#map_long").val(response.data.map_long);
                        LoadMapId(response.data.map_lat, response.data.map_long);
                    });
                }

            } else {
                DangerNotif("No data candidate interview found for selected ID.");
            }
            
        });


    });
});



function LoadMapId(lat, long) {
    var container = L.DomUtil.get('mapid');
    if (container !== null) {
        container._leaflet_id = null;
    }
    var mymap = L.map('mapid').setView([lat, long], 10);
    L.marker([lat, long]).addTo(mymap);

    var LeafIcon = L.Icon.extend({

    });

    var greenIcon = new LeafIcon({ iconUrl: WEB + '/assets/img/marker.png' });


    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(mymap);

    //ambil titik (nantinya di bungkus ajax)
    //if (markers !== undefined) {
    //    mymap.removeLayer(markers);
    //}

    markers = new L.markerClusterGroup();
    //var resp = itemmarker;
    //$.each(resp, function (index, item) {

    //    var icondata = greenIcon;
    //    var marker = L.marker([item.lintang, item.bujur], { icon: icondata, title: item.nama }).on('click', markerOnClick);
    //    markers.addLayer(marker);
    //});
    mymap.addLayer(markers);

}

function submitInterviewUpdate() {
    $('#InterviewFormUpdate').submit();
}

 

