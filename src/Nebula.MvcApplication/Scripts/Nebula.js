(function ($) {

	// DataTable conventional setup
	$.fn.dataTable.defaults.bFilter = false;
	$.fn.dataTable.defaults.bLengthChange = false;
	$.fn.dataTable.defaults.iDisplayLength = 10;
	$.fn.dataTable.defaults.sDom = "<'row-fluid'<'span6'l><'span6'f>r>t<'row-fluid'<'span6'i><'span6'p>>";
	$.fn.dataTable.defaults.sPaginationType = 'bootstrap';
	$.fn.dataTable.defaults.aoColumnDefs = [
		{
			'aTargets': ['_all'],
			'bSearchable': false,
			'bSortable': false
		},
		{
			'aTargets': ['jq_dt_hidden'],
			'bVisible': false
		},
		{
			'aTargets': ['jq_dt_type_json_datetime'],
			'fnRender': function (o, val) {
				return parseAndFormatJsonDateTime(val);
			}
		},
		{
			'aTargets': ['jq_dt_type_text'],
			'fnRender': function (o, val) {
				return replaceLineBreaks(val);
			}
		}
	];

	// Bootstrap validation setup
	$.validator.setDefaults({
		highlight: function (element) {
			$(element).closest(".control-group").addClass("error");
		},
		unhighlight: function (element) {
			$(element).closest(".control-group").removeClass("error");
		}
	});

	$(document).ready(function () {
		$('form .input-validation-error').closest(".control-group").addClass("error");

		//always select first form element
		$(':input, select, textarea').first().focus();
	});

})(jQuery);

// Functions
function replaceLineBreaks(value) {
	return (value) ? value.replace(/\n/g, '<br />') : '';
}

function parseAndFormatJsonDateTime(value) {
	return (value) ? moment(value).format('DD/MM/YYYY HH:mm:ss') : '';
}