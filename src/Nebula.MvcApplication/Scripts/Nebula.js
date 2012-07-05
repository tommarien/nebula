

function replaceLineBreaks(value) {
			 return (value) ? value.replace(/\n/g, '<br />') : '';
}

function parseAndFormatJsonDateTime(value) {
				return (value) ? moment(value).format('DD/MM/YYYY HH:mm:ss') : '';
}