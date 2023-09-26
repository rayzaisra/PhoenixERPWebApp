(function ($) {
	'use strict';
	var pageFunction = (function (pageFunction) {
		pageFunction.init = function () {
			//initialize
		};
		pageFunction.reject = function () {
			//reject function
            callAction({ formId: "form", title: "reject", type: "POST", url: param.rejectUrl });
		};
		pageFunction.approve = function () {
			//approve function
            callAction({ formId: "form", title: "approve", type: "POST", url: param.approveUrl });
		};
		return pageFunction;
	})(pageFunction || {});

	$(document).ready(function () {
		pageFunction.init();
		$('#btn-reject').click(pageFunction.reject);
		$('#btn-approve').click(pageFunction.approve);
	});
}(jQuery));