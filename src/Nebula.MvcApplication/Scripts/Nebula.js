(function ($) {
	// DataTable conventional setup
	$.fn.dataTable.defaults.bFilter = false;
	$.fn.dataTable.defaults.bLengthChange = false;
	$.fn.dataTable.defaults.bJQueryUI = true;
	$.fn.dataTable.defaults.iDisplayLength = 10;
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
			'aTargets': ['jq_dt_type_pre_text'],
			'fnRender': function (o, val) {
				return replaceLineBreaks(val);
			}
		}
	];

	$(document).ready(function () {
		$('input:submit').button();
	});

})(jQuery);

// Functions
function replaceLineBreaks(value) {
	return (value) ? value.replace(/\n/g, '<br />') : '';
}

function parseAndFormatJsonDateTime(value) {
	return (value) ? moment(value).format('DD/MM/YYYY HH:mm:ss') : '';
}