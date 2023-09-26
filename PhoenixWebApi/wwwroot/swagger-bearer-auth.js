(function () {
    $(function () {
        $('.authorize-wrapper').append("JWT Code : <input type='text' id='input_apiKey' style='width: 100% !important;' /> (input if needed JWT Code)");
        $("#input_apiKey").change(addApiKeyAuthorization);
    });

    function addApiKeyAuthorization() {
        var key = encodeURIComponent($('#input_apiKey')[0].value);
        if (key && key.trim() != "") {
            var apiKeyAuth = new SwaggerClient.ApiKeyAuthorization("Authorization", "pToken " + key, "header");
            window.swaggerUi.api.clientAuthorizations.add("pToken", apiKeyAuth);
        }
    }
})();