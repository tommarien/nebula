// DataTable conventional setup
jQuery.fn.dataTable.defaults.bFilter = false;
jQuery.fn.dataTable.defaults.bLengthChange = false;
jQuery.fn.dataTable.defaults.iDisplayLength = 10;
jQuery.fn.dataTable.defaults.aoColumnDefs = [
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

// Functions
function replaceLineBreaks(value) {
    return (value) ? value.replace(/\n/g, '<br />') : '';
}

function parseAndFormatJsonDateTime(value) {
    return (value) ? moment(value).format('DD/MM/YYYY HH:mm:ss') : '';
}