function bmContractPaidWorkAddRow(tableId, tableTotalId, cssNameTotal, inputAdvanceId) {
	var table = jQuery('#' + tableId);
	var tbody = table.children('tbody');
	var rowId = tableId + '_' + Math.floor(Math.random() * 10001);
	var rows = table.find('tbody tr');
	var lastRow = '';
	var minVal = 0;
	var slider = '';

	if (tbody.length == 0) {
		table.append('<tbody></tbody>')
		tbody = table.children('tbody');
	}
		
	if (rows.length > 0) {
		lastRow = jQuery(rows[rows.length - 1]);
		minVal = lastRow.find('input[name="payment-condition"]').val();
	}

	tbody.append('<tr>' +
		'<td><input type="checkbox" class="bm-row-check"/></td>' +
		'<td class="bm-progress"><span class="bm-progress-label"></span><div class="bm-progress-slider"></div><input type="hidden" name="payment-condition" /></td>' +
		'<td class="bm-number"><input type="text" name="payment-date" /></td>' +
		'<td class="bm-number"><input type="text" name="payment-amount" /></td>' +
		'</tr>');
		
	tbody.find('tr:last input[name="payment-date"]').datepicker();
	bmInputDecimalByObj(tbody.find('tr:last input[name="payment-amount"]'));

	tbody.find('tr:last input[name="payment-amount"]').change(function() {
		bmContractPaidWorkSetBalance(tableId, tableTotalId, cssNameTotal, inputAdvanceId);
	});
	
	if (lastRow) {
		slider = lastRow.next().find('.bm-progress-slider');
		lastRow.next().find('input[name="payment-condition"]').val(minVal);
	}
	else {
		slider = table.find('.bm-progress-slider');
		tbody.find('input[name="payment-condition"]').val(minVal);
	}

	bmContractApplySlider(slider, minVal);
	bmDisplayAlternate(tableId);
}

function bmContractPaidWorkSetBalance(tableId, tableTotalId, cssNameTotal, inputAdvanceId) {
	var table = jQuery('#' + tableId);
	var rows = table.find('tbody input[name="payment-amount"]');
	var tfoot = table.children('tfoot');
	var total = Number(jQuery('#' + tableTotalId + ' .' + cssNameTotal).text());
	var advance = Number(jQuery('#' + inputAdvanceId).val());
	var sum = 0;
	
	if (tfoot.length == 0){
		table.append('<tfoot></tfoot>')
		tfoot = table.children('tfoot');
		tfoot.append('<tr>' +
			'<td>&nbsp</td>' +
			'<td>&nbsp</td>' +
			'<td class="bm-label-total">Saldo:</td>'+
			'<td class="bm-number bm-total"></td>'+
			'</tr>');
	}

	
	rows.each(function() {
		sum += Number(this.value);
	});

	tfoot.find('.bm-total').text((total - advance - sum).toFixed(2));
}

function bmContractRemoveRow(tableId) {
	var checked = jQuery('#' + tableId + ' td input:checked');

	checked.each(function() {
		jQuery(this).parent().parent().remove();
	});
	
	bmDisplayAlternate(tableId);
}

function bmContractRestorePaidWork(tableId, tableTotalId, cssNameTotal, inputAdvanceId) {
	var table = jQuery('#' + tableId);
	var input = jQuery('#' + inputAdvanceId);

	table.find('input[name="payment-date"]').each(function() {
		jQuery(this).datepicker();
	});
	table.find('input[name="payment-condition"]').each(function() {
		var slider = jQuery(this).parent().children('.bm-progress-slider');

		bmContractApplySlider(slider, this.value);
	});
	table.find('input[name="payment-amount"]').each(function() {
		bmInputDecimalByObj(this);
	});
	table.find('input[name="payment-amount"]').change(function() {
		bmContractPaidWorkSetBalance(tableId, tableTotalId, cssNameTotal, inputAdvanceId);
	});
	input.change(function() {
		bmContractPaidWorkSetBalance(tableId, tableTotalId, cssNameTotal, inputAdvanceId);
	});
}

function bmContractApplySlider(slider, currentValue) {	
	slider.slider({
		value: currentValue,
		range: 'min',
		slide: function(event, ui) {
			var td = jQuery(this).parent();
			var tr = td.parent();
			var min = 0;

			if (tr.prev().length > 0)
				min = tr.prev().find('input[name="payment-condition"]').val();

			if (ui.value < min)
				return false;

			td.children('input[name="payment-condition"]').val(ui.value);
			td.children('.bm-progress-label').text(ui.value + '%');

			tr.nextAll().each(function() {
				var tr = jQuery(this);
				var value = tr.find('input[name="payment-condition"]').val();

				if (value < ui.value) {
					tr.find('.bm-progress-slider').slider('option', 'value', ui.value);
					tr.find('input[name="payment-condition"]').val(ui.value);
					tr.find('.bm-progress-label').text(ui.value + '%');
				}
			});
		}
	});

	slider.parent().children('.bm-progress-label').text(currentValue + '%');
}

function bmContractSumTotal(tableId, inputName, totalClass, useOnChange) {
	var inputs = jQuery('#' + tableId + ' input[name$="' + inputName + '"]');
	var sum = 0;

	inputs.each(function() {
		sum += Number(this.value);
	});

	jQuery('#' + tableId + ' .' + totalClass).text(sum.toFixed(2));

	if (!useOnChange)
		return;
		
	inputs.change(function() {
		var	sum1 = 0;
		var inputs = jQuery('#' + tableId + ' input[name$="' + inputName + '"]');

		inputs.each(function() {
			sum1 += Number(this.value);
		});

		jQuery('#' + tableId + ' .' + totalClass).text(sum1.toFixed(2));
	});
}

function bmContractFillSubItemsDetail(inputId, tableId, totalClass) {
	var data = '';
	var html = '';
	var arrow = ' &rarr; ';
	var table = jQuery('#' + tableId);
	var tbody = table.children('tbody');
	var counter = 0;

	try {
		data = JSON.parse(jQuery('#' + inputId).val());
	}
	catch (ex) {
		return;
	}

	if (tbody.length == 0) {
		table.append('<tbody></tbody>')
		tbody = table.children('tbody');
	}
	else 
		tbody.empty();

	for (var p in data.projects) {
		var project = data.projects[p];

		for (var b in project.blocks) {
			var block = project.blocks[b];

			for (var w in block.works) {
				var work = block.works[w];

				for (var g in work.groups) {
					var group = work.groups[g];
					
					html = '<tr class="bm-group-hd"><td colspan="6">' + project.name + arrow + block.name + arrow + work.name + arrow + group.name + '</td></tr>';
					tbody.append(html);
					
					for (var i in group.items) {
						var item = group.items[i];					
			
						for (var s in item.subitems) {
							var subitem = item.subitems[s];
							var rowData = new Object();

							rowData.pId = project.pId;
							rowData.bId = block.bId;
							rowData.wId = work.wId;
							rowData.gId = group.gId;
							rowData.iId = item.iId;
							rowData.sId = subitem.sId;
							rowData.progress = subitem.progress;

							html = '<tr>';
							html += '<td><input type="checkbox" class="bm-row-check"/><input type="hidden" name="subitem-data" value="' + Encoder.htmlEncode(JSON.stringify(rowData)) + '"/></td>';
							html += '<td>' + (++counter) + '. ' + subitem.name + '</td>';
							html += '<td class="bm-number">' + subitem.price.toFixed(2) + '</td>';
							html += '<td class="bm-number">' + subitem.qty + ' ' + subitem.unit + '</td>';
							html += '<td class="bm-number">' + subitem.progress  + '%</td>';
							html += '<td class="bm-number"><input type="hidden" value="' + subitem.total.toFixed(2) + '"/><input type="text" size="8" name="subitem-price-total" value="' + subitem.total.toFixed(2) + '"/></td>';
							html += '</tr>'; 
							tbody.append(html);
						}
					}
				}
			}
		}		
	}
	
	bmDisplayAlternate(tableId);
	bmInputDecimalByObj(tbody.find('input[name="subitem-price-total"]'));
}

function bmDisplayAlternate(tableId) {
	var i = 0;

	jQuery('#' + tableId + ' tbody tr').each(function() {
		var tr = jQuery(this);

		if (tr.hasClass('bm-group-hd'))
			i = 0;
		else {
			if (i % 2 != 0)
				tr.addClass('alternate');
			else
				tr.removeClass('alternate');
			i++;
		}
	});
}

function bmSetRefPriceTotal(tableId) {
	jQuery('#' + tableId + ' input:checked').each(function() {
		var tds = jQuery(this).parent().nextAll();
		var tdTo = tds[tds.length - 1];

		jQuery(tdTo).children('input[name="subitem-price-total"]').val(jQuery(tdTo).children('input:hidden').val());
	});
}