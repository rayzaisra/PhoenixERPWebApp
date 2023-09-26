(function ($) {
    'use strict';
    var pageFunction = function () {

        //Begin :: Country Lookup
        $(".country-lookup").select2({
            enable: false,
            width: "100%",
            theme: "bootstrap",
            placeholder: "Choose Country",
            ajax: {
                url: $.helper.resolveApi("~/core/Country/lookup"),
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var filters = " r.country_name LIKE @0 ";
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
                                text: this.country_name
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
            minimumInputLength: 0
        });

        var setLookup_Country = function (Id, Text) {

            if (Id == '' || Id == null) return;
            var record = [{ id: Id, text: Text }];
            $('#country_id').append($("<option/>", {
                value: Id,
                text: Text,
                selected: true
            }));
            $("#country_id").val(Id);

        };
        //End :: Country Lookup

        return {
            init: function () {

            }
        };
    }();

    $(document).ready(function () {
        pageFunction.init();
    });
}(jQuery));