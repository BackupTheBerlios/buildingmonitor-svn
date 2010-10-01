function bmProgressRecInitSlider(td, isAdmin) {
	var data = bmProgressRecGetData(td);

	if (!data)
		return;

	bmProgressRecSetNewProgress(td, data.curr, false)

	jQuery(td).children('div.bm-progress-slider').slider({
		value: data.curr,
		range: 'min',
		slide: function(event, ui) {
			return bmProgressRecSetNewProgress(td, ui.value, false, isAdmin);
		},
		stop: function(event, ui) {
			bmProgressRecSetNewProgress(td, ui.value, true, isAdmin);
		}
	});
}

function bmProgressRecGetData(td) {
	var data = '';
	var input = jQuery(td).parent().find('input[name$="hdnData"]');

	try {
		data = JSON.parse(input.val());
	}
	catch (ex) {
		return '';
	}

	return data;
}

function bmProgressRecSetNewProgress(td, value, update, isAdmin) {
	var data = bmProgressRecGetData(td);
	var tr = jQuery(td).parent();
	var label = tr.find('span.bm-progress-label');

	if (!data)
		return false;

	if (value < data.curr && !isAdmin)
		return false;

	label.text(value + '%');
	tr.find('.bm-progress-slider').slider('option', 'value', value);
	
	if (data.curr != value) {
		label.addClass('bm-progress-changed');
		tr.find('.bm-progress-action-save .ui-icon').removeClass('ui-state-disabled');
	}
	else {
		label.removeClass('bm-progress-changed');
		tr.find('.bm-progress-action-save .ui-icon').addClass('ui-state-disabled');
	}

	if (update) {
		data.newp = value;
		tr.find('input[name$="hdnData"]').val(JSON.stringify(data));
	}

	return true;
}

function bmProgressRecInitProgressBar(td) {
	var data = bmProgressRecGetData(td);

	if (!data)
		return;

	bmProgressRecSetNewProgress(td, data.curr, false);
	jQuery(td).children('div.bm-progress-slider').progressbar({ value: data.curr });
}

function bmProgressRecReset() {
	jQuery('.bm-table input:checked').each(function() {
		var td = jQuery(this).parent();
		var data = bmProgressRecGetData(td);

		if (!data)
			return;

		bmProgressRecSetNewProgress(td, data.curr, true);
	});
}

function bmProgressRecActive(flag) {
	jQuery('.bm-table input:checked').each(function() {
		var tr = jQuery(this).closest('tr');

		tr.find('.bm-progress-slider').slider(flag ? 'enable' : 'disable');

		if (flag)
			tr.find('.bm-progress-status').attr('class', 'bm-progress-status');
	});
}

function bmSaveProgress(index) {
	var tr = jQuery('#tblContractDetail tbody tr')[index];
	var jsonData = jQuery(tr).find('.bm-progress-options input:hidden').val();
	var values = JSON.parse(jsonData);

	if (values.newp <= values.curr)
		return;
	if (jQuery(tr).find('.bm-progress-slider').slider('option', 'disabled'))
		return;

	bmAnimation(true);

	jQuery.ajax({
		type: 'POST',
		data: 'SaveProgress=' + jsonData,
		dataType: 'json',
		success: function(data, status) {
			jQuery(tr).find('.bm-progress-options input:hidden').val(JSON.stringify(data));
			jQuery(tr).find('.bm-progress-slider').slider('destroy');
			bmProgressRecInitSlider(jQuery(tr).children('td.bm-progress'));
			jQuery(tr).find('.bm-progress-slider').slider('disable');
			jQuery(tr).find('.bm-progress-status').attr('class', 'bm-progress-status ui-icon ui-icon-check');
		},
		error: function(request, textStatus, errorThrown) {
			var jDialog = jQuery('#dialog_status');

			jDialog.html('Ha ocurrido un error: ' + textStatus + '<br />Por favor contactar a soporte t&eacute;cnico.');
			jDialog.dialog('option', 'title', 'Error');
			jDialog.dialog('open');
			jQuery(tr).find('.bm-progress-status').attr('class', 'bm-progress-status ui-icon ui-icon-alert');
		},
		complete: function() {
			bmAnimation(false);
		}
	});
}
