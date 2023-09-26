
(function ($) {
    'use strict';
    var pageFunction = function () {
        var _detailID = $('#detail-id');
        var _formTeam = $('#form-team');
        var $teamLeaderLookup = $("#team_leader");

        //BEGIN::LOOKUP
        $teamLeaderLookup.select2({
            enable: false,
            width: "auto",
            theme: "bootstrap",
            placeholder: "Assign To",
            ajax: {
                url: $.helper.resolveApi("~/core/User/lookup"),
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var filters = " r.app_fullname LIKE @0 ";
                    var parameters = [];
                    if (params.term == undefined) {
                        parameters.push('%%');
                    }
                    else {
                        parameters.push('%' + params.term + '%');
                    }
                    return {
                        length: 100,
                        filters: "( " + filters + " )",
                        parameters: parameters.toString()  // search term

                    };
                },
                processResults: function (data, page) {
                    console.log(data);
                    var result = { results: [], more: false };
                    if (data && data.items) {
                        result.more = data.totalPages > data.currentPage;
                        $.each(data.items, function () {
                            result.results.push({
                                id: this.id,
                                text: this.app_fullname,
                                username: this.app_username
                            });
                        });
                    };
                    return result;
                },
                cache: true
            },
            escapeMarkup: function (markup) {
                return markup;
            }, // let our custom formatter work
            minimumInputLength: 0,
            templateResult: formatTeamLeaderLookup
            //templateSelection: formatTeamLeaderLookup
        });
        var setLookup_TeamLeader = function (Id, Text) {

            if (Id == '' || Id == null) return;
            var record = [{ id: Id, text: Text }];
            $teamLeaderLookup.append($("<option/>", {
                value: Id,
                text: Text,
                selected: true
            }));
            $teamLeaderLookup.val(Id);

        };
        function formatTeamLeaderLookup(opt) {
            if (!opt.id) {
                return opt.text;
            }

            var template;
            var stateNo = mUtil.getRandomInt(0, 7);
            var states = [
                'success',
                'brand',
                'danger',
                'accent',
                'warning',
                'metal',
                'primary',
                'info'];
            var state = states[stateNo];

            template = `
                <div class="m-card-user m-card-user--sm">
                    <div class="m-card-user__pic">
                        <div class="m-card-user__no-photo m--bg-fill-` + state + `"><span>` + opt.text.substring(0, 1) + `</span></div>
                    </div>
                    <div class="m-card-user__details">
                        <span class="m-card-user__name">` + opt.text + `</span>
                        <a href="javascript:return;" class="m-card-user__email m-link">` + opt.username + `</a>
                    </div>
                </div>`;
            return template;

        };

        //END::LOOKUP

        //BEGIN::Form Validation
        _formTeam.validate({
            rules: {
                team_name: {
                    required: true
                },
                team_leader: {
                    required: true
                }
            },
            invalidHandler: function (e, r) {
                var errors = r.numberOfInvalids();
                if (errors) {
                    var message = errors === 1
                        ? 'Please correct the following error:\n'
                        : 'Please correct the following ' + errors + ' errors.\n';

                    if (r.errorList.length > 0) {
                        for (var x = 0; x < r.errorList.length; x++) {
                            console.warn(r.errorList[x]);
                            errors += "\n\u25CF " + r.errorList[x].message;
                        }
                    }
                    alert(message + errors);
                }
                mUtil.scrollTo(_formTeam, -200);
            },
            submitHandler: function (e) { }
        });
        ////END::Form Validation

        //BEGIN::Load Datatable
        var loadDetailData = function () {

            if (_detailID.val() !== '') {
                mApp.blockPage({
                    overlayColor: "#000000",
                    type: "loader",
                    state: "primary",
                    message: "Processing..."
                }),
                    $.ajax({
                        type: "GET",
                        dataType: 'json',
                        url: $.helper.resolveApi("~/core/Team/" + _detailID.val() + "/details"),
                        success: function (r) {
                            if (r.status.success && r.data) {
                                var record = r.data;
                                console.log(record);
                                $('input[name="team_name"]').val(record.team_name);
                                setLookup_TeamLeader(record.team_leader, record.team_leader_name);
                            } else {
                                swal(
                                    'Information',
                                    r.status.message,
                                    'info'
                                );
                            }
                        },
                        error: function (e, t, s) {
                            swal(
                                'Information',
                                'Ooops, something went wrong !',
                                'info'
                            );
                        }
                    }).then(setTimeout(function () {
                        mApp.unblockPage();
                    }, 2e3)),
                    setTimeout(function () {
                        loadTeamMember(_detailID.val());
                    }, 2e3)



            } else {
                $('.no-record').removeClass("collapse");
            }

        };

        var loadTeamMember = function (teamid) {
            var $loadteammember = $('#loadteammember');
            mApp.block("#m_tabs_member", {
                overlayColor: "#000000",
                type: "loader",
                state: "primary",
                message: "Processing..."
            });
            var url = $loadteammember.attr('data-url') + "?teamid=" + teamid;
            $loadteammember.load(url, function (result) {
                mApp.unblock("#m_tabs_member");
            });
        }
        //END::Load Datatable

        //BEGIN::EVENT BUTTON

        $('#btn-save').click(function (e) {
            var btn = $(this);
            var data = _formTeam.serializeToJSON();
            if (_formTeam.valid()) {
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    contentType: 'application/json',
                    url: $.helper.resolveApi("~/core/Team/save"),
                    data: JSON.stringify(data),
                    success: function (r) {
                        console.log(r);
                        if (r && r.status.success) {
                            history.pushState('', 'ID', location.hash.split('?')[0] + '?id=' + r.data.recordID);
                            _detailID.val(r.data.recordID);
                            $('input[name="id"]').val(r.data.recordID);
                            loadDetailData();
                            toastr.success(r.status.message, "Information");
                        }
                        else {
                            swal(
                                'Saving',
                                r.status.message,
                                'info'
                            );
                        }
                    },
                    error: function (r) {
                        swal(
                            '' + r.status,
                            r.statusText,
                            'error'
                        );
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                    }
                }).then(function () { btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false); });

            }
        });
        //END::EVENT BUTTON

        return {
            init: function () {
                //initialize
                loadDetailData();
            }
        };
    }();



    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));


