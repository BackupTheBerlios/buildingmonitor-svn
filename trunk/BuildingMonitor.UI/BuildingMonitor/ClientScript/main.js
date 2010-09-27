var bmContractEdit = new function() {
	var _targetID = '';
	var _sourceID = '';
	var _txtRemoveRow = 'Remove';

	this.initialize = function(sourceID, targetID, txtRemoveRow) {
		_sourceID = sourceID;
		_targetID = targetID;
		_txtRemoveRow = txtRemoveRow;
	}

	this.addRow = function() {
		var subItem = jQuery('#' + _sourceID);
		var detail = jQuery('#' + _targetID);
		var rowID = _targetID + '_' + Math.floor(Math.random() * 10001);

		jQuery('#' + _sourceID + ' option:selected').each(function() {
			var data = JSON.parse(jQuery(this).val());

			detail.append('<tr id="' + rowID + '">' +
				'<td><input type="hidden" value="' + escape(JSON.stringify(data.id)) + '" name="subitem-id"/>' + jQuery(this).text() + '</td>' +
				'<td>' + data.qty + '</td>' +
				'<td><input type="text" name="subitem-price"/></td>' +
				'<td><a href="javascript:bmContractEdit.removeRow(\'' + rowID + '\')">' + _txtRemoveRow + '</a></td></tr>');
		});
	}

	this.removeRow = function(rowID) {
		jQuery('#' + rowID).remove();
	}
}

function bmInputInteger(id) {
	jQuery('#' + id).keypress(function(e) {
		var isNumber = e.which >= 48 && e.which <= 57;
		var isControl = e.which == 0 || e.which == 8;

		return isNumber || isControl;
	});
}

function bmInputDecimal(id) {
	bmInputDecimalByObj(document.getElementById(id));
}

function bmInputDecimalByObj(obj) {
	if (!obj)
		return;
	
	obj = jQuery(obj);
	
	obj.keypress(function(e) {
		return bmBlockNonNumbers(this, e, true, false);
	});
	
	obj.blur(function(e) {
		bmExtractNumber(this, 2, false);
	});
	
	obj.keyup(function(e) {
		bmExtractNumber(this, 2, false);
	});
	
	obj.attr('maxlength', '9');
}

function bmLoadingAnimation(animId, containerId, show) {
	var container = jQuery('#' + containerId);
	var animation = jQuery('#' + animId);

	animation.css('position', 'absolute');

	if (show) {
		animation.css('top', container.offset().top + 'px');
		animation.css('left', container.offset().left + 'px');
		animation.css('width', container.width() + 'px');
		animation.css('height', container.height() + 'px');
		animation.show();
		container.fadeTo('fast', 0.5);
	}
	else {
		animation.hide();
		container.fadeTo('fast', 1.0);
	}
}

function bmSelectCheckBoxes(id, value) {
	if (value == 'all')
		jQuery('#' + id + ' input:checkbox').attr('checked', true);
	else if (value == 'none')
		jQuery('#' + id + ' input:checkbox').attr('checked', false);
}

// version: beta
// created: 2005-08-30
// updated: 2005-08-31
// mredkj.com
function bmExtractNumber(obj, decimalPlaces, allowNegative) {
	var temp = obj.value;

	// avoid changing things if already formatted correctly
	var reg0Str = '[0-9]*';
	if (decimalPlaces > 0) {
		reg0Str += '\\.?[0-9]{0,' + decimalPlaces + '}';
	} else if (decimalPlaces < 0) {
		reg0Str += '\\.?[0-9]*';
	}
	reg0Str = allowNegative ? '^-?' + reg0Str : '^' + reg0Str;
	reg0Str = reg0Str + '$';
	
	var reg0 = new RegExp(reg0Str);

	if (reg0.test(temp))
		return true;

	// first replace all non numbers
	var reg1Str = '[^0-9' + (decimalPlaces != 0 ? '.' : '') + (allowNegative ? '-' : '') + ']';
	var reg1 = new RegExp(reg1Str, 'g');
	
	temp = temp.replace(reg1, '');

	if (allowNegative) {
		// replace extra negative
		var hasNegative = temp.length > 0 && temp.charAt(0) == '-';
		var reg2 = /-/g;
		temp = temp.replace(reg2, '');
		if (hasNegative) temp = '-' + temp;
	}

	if (decimalPlaces != 0) {
		var reg3 = /\./g;
		var reg3Array = reg3.exec(temp);
		if (reg3Array != null) {
			// keep only first occurrence of .
			//  and the number of places specified by decimalPlaces or the entire string if decimalPlaces < 0
			var reg3Right = temp.substring(reg3Array.index + reg3Array[0].length);
			reg3Right = reg3Right.replace(reg3, '');
			reg3Right = decimalPlaces > 0 ? reg3Right.substring(0, decimalPlaces) : reg3Right;
			temp = temp.substring(0, reg3Array.index) + '.' + reg3Right;
		}
	}

	obj.value = temp;
}

function bmBlockNonNumbers(obj, e, allowDecimal, allowNegative) {
	var key;
	var isCtrl = false;
	var keychar;
	var reg;

	if (window.event) {
		key = e.keyCode;
		isCtrl = window.event.ctrlKey
	}
	else if (e.which) {
		key = e.which;
		isCtrl = e.ctrlKey;
	}

	if (isNaN(key)) return true;

	keychar = String.fromCharCode(key);

	// check for backspace or delete, or if Ctrl was pressed
	if (key == 8 || isCtrl) {
		return true;
	}

	reg = /\d/;
	var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
	var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 : false;

	return isFirstN || isFirstD || reg.test(keychar);
}
