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
		/*var keychar = String.fromCharCode(e.which);
		var isNumber = e.which >= 48 && e.which <= 57;
		var isControl = e.which == 0 || e.which == 8;
		var isPeriod = (keychar == "." && this.value.indexOf(".") < 0);

		return isNumber || isControl || isPeriod;*/
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
	var container = jQuery(document); //jQuery('#' + containerId);
	var animation = jQuery('#' + animId);

	animation.css('position', 'absolute');

	if (show) {
		/*animation.css('top', container.offset().top + 'px');
		animation.css('left', container.offset().left + 'px');
		animation.css('width', container.width() + 'px');
		animation.css('height', container.height() + 'px');*/
		animation.css('top', 0);
		animation.css('left', 0);
		animation.css('width', container.width() + 'px');
		animation.css('height', container.height() + 'px');
		animation.css('background-color', 'white');
		animation.show();
		animation.fadeTo(0, 0.7);
	}
	else {
		animation.hide();
		animation.fadeTo(0, 1);
	}
}

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
	tr.find('div.bm-progress-slider').slider('option', 'value', value);
	
	if (data.curr != value) {
		label.addClass('bm-progress-changed');
	}
	else {
		label.removeClass('bm-progress-changed');
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
	jQuery(td).children('div.bm-progress-slider').progressbar({value: data.curr});
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


/**
 * HTML encoder
 */
Encoder = {

	// When encoding do we convert characters into html or numerical entities
	EncodeType: "entity",  // entity OR numerical

	isEmpty: function(val) {
		if (val) {
			return ((val === null) || val.length == 0 || /^\s+$/.test(val));
		} else {
			return true;
		}
	},
	// Convert HTML entities into numerical entities
	HTML2Numerical: function(s) {
		var arr1 = new Array('&nbsp;', '&iexcl;', '&cent;', '&pound;', '&curren;', '&yen;', '&brvbar;', '&sect;', '&uml;', '&copy;', '&ordf;', '&laquo;', '&not;', '&shy;', '&reg;', '&macr;', '&deg;', '&plusmn;', '&sup2;', '&sup3;', '&acute;', '&micro;', '&para;', '&middot;', '&cedil;', '&sup1;', '&ordm;', '&raquo;', '&frac14;', '&frac12;', '&frac34;', '&iquest;', '&agrave;', '&aacute;', '&acirc;', '&atilde;', '&Auml;', '&aring;', '&aelig;', '&ccedil;', '&egrave;', '&eacute;', '&ecirc;', '&euml;', '&igrave;', '&iacute;', '&icirc;', '&iuml;', '&eth;', '&ntilde;', '&ograve;', '&oacute;', '&ocirc;', '&otilde;', '&Ouml;', '&times;', '&oslash;', '&ugrave;', '&uacute;', '&ucirc;', '&Uuml;', '&yacute;', '&thorn;', '&szlig;', '&agrave;', '&aacute;', '&acirc;', '&atilde;', '&auml;', '&aring;', '&aelig;', '&ccedil;', '&egrave;', '&eacute;', '&ecirc;', '&euml;', '&igrave;', '&iacute;', '&icirc;', '&iuml;', '&eth;', '&ntilde;', '&ograve;', '&oacute;', '&ocirc;', '&otilde;', '&ouml;', '&divide;', '&Oslash;', '&ugrave;', '&uacute;', '&ucirc;', '&uuml;', '&yacute;', '&thorn;', '&yuml;', '&quot;', '&amp;', '&lt;', '&gt;', '&oelig;', '&oelig;', '&scaron;', '&scaron;', '&yuml;', '&circ;', '&tilde;', '&ensp;', '&emsp;', '&thinsp;', '&zwnj;', '&zwj;', '&lrm;', '&rlm;', '&ndash;', '&mdash;', '&lsquo;', '&rsquo;', '&sbquo;', '&ldquo;', '&rdquo;', '&bdquo;', '&dagger;', '&dagger;', '&permil;', '&lsaquo;', '&rsaquo;', '&euro;', '&fnof;', '&alpha;', '&beta;', '&gamma;', '&delta;', '&epsilon;', '&zeta;', '&eta;', '&theta;', '&iota;', '&kappa;', '&lambda;', '&mu;', '&nu;', '&xi;', '&omicron;', '&pi;', '&rho;', '&sigma;', '&tau;', '&upsilon;', '&phi;', '&chi;', '&psi;', '&omega;', '&alpha;', '&beta;', '&gamma;', '&delta;', '&epsilon;', '&zeta;', '&eta;', '&theta;', '&iota;', '&kappa;', '&lambda;', '&mu;', '&nu;', '&xi;', '&omicron;', '&pi;', '&rho;', '&sigmaf;', '&sigma;', '&tau;', '&upsilon;', '&phi;', '&chi;', '&psi;', '&omega;', '&thetasym;', '&upsih;', '&piv;', '&bull;', '&hellip;', '&prime;', '&prime;', '&oline;', '&frasl;', '&weierp;', '&image;', '&real;', '&trade;', '&alefsym;', '&larr;', '&uarr;', '&rarr;', '&darr;', '&harr;', '&crarr;', '&larr;', '&uarr;', '&rarr;', '&darr;', '&harr;', '&forall;', '&part;', '&exist;', '&empty;', '&nabla;', '&isin;', '&notin;', '&ni;', '&prod;', '&sum;', '&minus;', '&lowast;', '&radic;', '&prop;', '&infin;', '&ang;', '&and;', '&or;', '&cap;', '&cup;', '&int;', '&there4;', '&sim;', '&cong;', '&asymp;', '&ne;', '&equiv;', '&le;', '&ge;', '&sub;', '&sup;', '&nsub;', '&sube;', '&supe;', '&oplus;', '&otimes;', '&perp;', '&sdot;', '&lceil;', '&rceil;', '&lfloor;', '&rfloor;', '&lang;', '&rang;', '&loz;', '&spades;', '&clubs;', '&hearts;', '&diams;');
		var arr2 = new Array('&#160;', '&#161;', '&#162;', '&#163;', '&#164;', '&#165;', '&#166;', '&#167;', '&#168;', '&#169;', '&#170;', '&#171;', '&#172;', '&#173;', '&#174;', '&#175;', '&#176;', '&#177;', '&#178;', '&#179;', '&#180;', '&#181;', '&#182;', '&#183;', '&#184;', '&#185;', '&#186;', '&#187;', '&#188;', '&#189;', '&#190;', '&#191;', '&#192;', '&#193;', '&#194;', '&#195;', '&#196;', '&#197;', '&#198;', '&#199;', '&#200;', '&#201;', '&#202;', '&#203;', '&#204;', '&#205;', '&#206;', '&#207;', '&#208;', '&#209;', '&#210;', '&#211;', '&#212;', '&#213;', '&#214;', '&#215;', '&#216;', '&#217;', '&#218;', '&#219;', '&#220;', '&#221;', '&#222;', '&#223;', '&#224;', '&#225;', '&#226;', '&#227;', '&#228;', '&#229;', '&#230;', '&#231;', '&#232;', '&#233;', '&#234;', '&#235;', '&#236;', '&#237;', '&#238;', '&#239;', '&#240;', '&#241;', '&#242;', '&#243;', '&#244;', '&#245;', '&#246;', '&#247;', '&#248;', '&#249;', '&#250;', '&#251;', '&#252;', '&#253;', '&#254;', '&#255;', '&#34;', '&#38;', '&#60;', '&#62;', '&#338;', '&#339;', '&#352;', '&#353;', '&#376;', '&#710;', '&#732;', '&#8194;', '&#8195;', '&#8201;', '&#8204;', '&#8205;', '&#8206;', '&#8207;', '&#8211;', '&#8212;', '&#8216;', '&#8217;', '&#8218;', '&#8220;', '&#8221;', '&#8222;', '&#8224;', '&#8225;', '&#8240;', '&#8249;', '&#8250;', '&#8364;', '&#402;', '&#913;', '&#914;', '&#915;', '&#916;', '&#917;', '&#918;', '&#919;', '&#920;', '&#921;', '&#922;', '&#923;', '&#924;', '&#925;', '&#926;', '&#927;', '&#928;', '&#929;', '&#931;', '&#932;', '&#933;', '&#934;', '&#935;', '&#936;', '&#937;', '&#945;', '&#946;', '&#947;', '&#948;', '&#949;', '&#950;', '&#951;', '&#952;', '&#953;', '&#954;', '&#955;', '&#956;', '&#957;', '&#958;', '&#959;', '&#960;', '&#961;', '&#962;', '&#963;', '&#964;', '&#965;', '&#966;', '&#967;', '&#968;', '&#969;', '&#977;', '&#978;', '&#982;', '&#8226;', '&#8230;', '&#8242;', '&#8243;', '&#8254;', '&#8260;', '&#8472;', '&#8465;', '&#8476;', '&#8482;', '&#8501;', '&#8592;', '&#8593;', '&#8594;', '&#8595;', '&#8596;', '&#8629;', '&#8656;', '&#8657;', '&#8658;', '&#8659;', '&#8660;', '&#8704;', '&#8706;', '&#8707;', '&#8709;', '&#8711;', '&#8712;', '&#8713;', '&#8715;', '&#8719;', '&#8721;', '&#8722;', '&#8727;', '&#8730;', '&#8733;', '&#8734;', '&#8736;', '&#8743;', '&#8744;', '&#8745;', '&#8746;', '&#8747;', '&#8756;', '&#8764;', '&#8773;', '&#8776;', '&#8800;', '&#8801;', '&#8804;', '&#8805;', '&#8834;', '&#8835;', '&#8836;', '&#8838;', '&#8839;', '&#8853;', '&#8855;', '&#8869;', '&#8901;', '&#8968;', '&#8969;', '&#8970;', '&#8971;', '&#9001;', '&#9002;', '&#9674;', '&#9824;', '&#9827;', '&#9829;', '&#9830;');
		return this.swapArrayVals(s, arr1, arr2);
	},

	// Convert Numerical entities into HTML entities
	NumericalToHTML: function(s) {
		var arr1 = new Array('&#160;', '&#161;', '&#162;', '&#163;', '&#164;', '&#165;', '&#166;', '&#167;', '&#168;', '&#169;', '&#170;', '&#171;', '&#172;', '&#173;', '&#174;', '&#175;', '&#176;', '&#177;', '&#178;', '&#179;', '&#180;', '&#181;', '&#182;', '&#183;', '&#184;', '&#185;', '&#186;', '&#187;', '&#188;', '&#189;', '&#190;', '&#191;', '&#192;', '&#193;', '&#194;', '&#195;', '&#196;', '&#197;', '&#198;', '&#199;', '&#200;', '&#201;', '&#202;', '&#203;', '&#204;', '&#205;', '&#206;', '&#207;', '&#208;', '&#209;', '&#210;', '&#211;', '&#212;', '&#213;', '&#214;', '&#215;', '&#216;', '&#217;', '&#218;', '&#219;', '&#220;', '&#221;', '&#222;', '&#223;', '&#224;', '&#225;', '&#226;', '&#227;', '&#228;', '&#229;', '&#230;', '&#231;', '&#232;', '&#233;', '&#234;', '&#235;', '&#236;', '&#237;', '&#238;', '&#239;', '&#240;', '&#241;', '&#242;', '&#243;', '&#244;', '&#245;', '&#246;', '&#247;', '&#248;', '&#249;', '&#250;', '&#251;', '&#252;', '&#253;', '&#254;', '&#255;', '&#34;', '&#38;', '&#60;', '&#62;', '&#338;', '&#339;', '&#352;', '&#353;', '&#376;', '&#710;', '&#732;', '&#8194;', '&#8195;', '&#8201;', '&#8204;', '&#8205;', '&#8206;', '&#8207;', '&#8211;', '&#8212;', '&#8216;', '&#8217;', '&#8218;', '&#8220;', '&#8221;', '&#8222;', '&#8224;', '&#8225;', '&#8240;', '&#8249;', '&#8250;', '&#8364;', '&#402;', '&#913;', '&#914;', '&#915;', '&#916;', '&#917;', '&#918;', '&#919;', '&#920;', '&#921;', '&#922;', '&#923;', '&#924;', '&#925;', '&#926;', '&#927;', '&#928;', '&#929;', '&#931;', '&#932;', '&#933;', '&#934;', '&#935;', '&#936;', '&#937;', '&#945;', '&#946;', '&#947;', '&#948;', '&#949;', '&#950;', '&#951;', '&#952;', '&#953;', '&#954;', '&#955;', '&#956;', '&#957;', '&#958;', '&#959;', '&#960;', '&#961;', '&#962;', '&#963;', '&#964;', '&#965;', '&#966;', '&#967;', '&#968;', '&#969;', '&#977;', '&#978;', '&#982;', '&#8226;', '&#8230;', '&#8242;', '&#8243;', '&#8254;', '&#8260;', '&#8472;', '&#8465;', '&#8476;', '&#8482;', '&#8501;', '&#8592;', '&#8593;', '&#8594;', '&#8595;', '&#8596;', '&#8629;', '&#8656;', '&#8657;', '&#8658;', '&#8659;', '&#8660;', '&#8704;', '&#8706;', '&#8707;', '&#8709;', '&#8711;', '&#8712;', '&#8713;', '&#8715;', '&#8719;', '&#8721;', '&#8722;', '&#8727;', '&#8730;', '&#8733;', '&#8734;', '&#8736;', '&#8743;', '&#8744;', '&#8745;', '&#8746;', '&#8747;', '&#8756;', '&#8764;', '&#8773;', '&#8776;', '&#8800;', '&#8801;', '&#8804;', '&#8805;', '&#8834;', '&#8835;', '&#8836;', '&#8838;', '&#8839;', '&#8853;', '&#8855;', '&#8869;', '&#8901;', '&#8968;', '&#8969;', '&#8970;', '&#8971;', '&#9001;', '&#9002;', '&#9674;', '&#9824;', '&#9827;', '&#9829;', '&#9830;');
		var arr2 = new Array('&nbsp;', '&iexcl;', '&cent;', '&pound;', '&curren;', '&yen;', '&brvbar;', '&sect;', '&uml;', '&copy;', '&ordf;', '&laquo;', '&not;', '&shy;', '&reg;', '&macr;', '&deg;', '&plusmn;', '&sup2;', '&sup3;', '&acute;', '&micro;', '&para;', '&middot;', '&cedil;', '&sup1;', '&ordm;', '&raquo;', '&frac14;', '&frac12;', '&frac34;', '&iquest;', '&agrave;', '&aacute;', '&acirc;', '&atilde;', '&Auml;', '&aring;', '&aelig;', '&ccedil;', '&egrave;', '&eacute;', '&ecirc;', '&euml;', '&igrave;', '&iacute;', '&icirc;', '&iuml;', '&eth;', '&ntilde;', '&ograve;', '&oacute;', '&ocirc;', '&otilde;', '&Ouml;', '&times;', '&oslash;', '&ugrave;', '&uacute;', '&ucirc;', '&Uuml;', '&yacute;', '&thorn;', '&szlig;', '&agrave;', '&aacute;', '&acirc;', '&atilde;', '&auml;', '&aring;', '&aelig;', '&ccedil;', '&egrave;', '&eacute;', '&ecirc;', '&euml;', '&igrave;', '&iacute;', '&icirc;', '&iuml;', '&eth;', '&ntilde;', '&ograve;', '&oacute;', '&ocirc;', '&otilde;', '&ouml;', '&divide;', '&Oslash;', '&ugrave;', '&uacute;', '&ucirc;', '&uuml;', '&yacute;', '&thorn;', '&yuml;', '&quot;', '&amp;', '&lt;', '&gt;', '&oelig;', '&oelig;', '&scaron;', '&scaron;', '&yuml;', '&circ;', '&tilde;', '&ensp;', '&emsp;', '&thinsp;', '&zwnj;', '&zwj;', '&lrm;', '&rlm;', '&ndash;', '&mdash;', '&lsquo;', '&rsquo;', '&sbquo;', '&ldquo;', '&rdquo;', '&bdquo;', '&dagger;', '&dagger;', '&permil;', '&lsaquo;', '&rsaquo;', '&euro;', '&fnof;', '&alpha;', '&beta;', '&gamma;', '&delta;', '&epsilon;', '&zeta;', '&eta;', '&theta;', '&iota;', '&kappa;', '&lambda;', '&mu;', '&nu;', '&xi;', '&omicron;', '&pi;', '&rho;', '&sigma;', '&tau;', '&upsilon;', '&phi;', '&chi;', '&psi;', '&omega;', '&alpha;', '&beta;', '&gamma;', '&delta;', '&epsilon;', '&zeta;', '&eta;', '&theta;', '&iota;', '&kappa;', '&lambda;', '&mu;', '&nu;', '&xi;', '&omicron;', '&pi;', '&rho;', '&sigmaf;', '&sigma;', '&tau;', '&upsilon;', '&phi;', '&chi;', '&psi;', '&omega;', '&thetasym;', '&upsih;', '&piv;', '&bull;', '&hellip;', '&prime;', '&prime;', '&oline;', '&frasl;', '&weierp;', '&image;', '&real;', '&trade;', '&alefsym;', '&larr;', '&uarr;', '&rarr;', '&darr;', '&harr;', '&crarr;', '&larr;', '&uarr;', '&rarr;', '&darr;', '&harr;', '&forall;', '&part;', '&exist;', '&empty;', '&nabla;', '&isin;', '&notin;', '&ni;', '&prod;', '&sum;', '&minus;', '&lowast;', '&radic;', '&prop;', '&infin;', '&ang;', '&and;', '&or;', '&cap;', '&cup;', '&int;', '&there4;', '&sim;', '&cong;', '&asymp;', '&ne;', '&equiv;', '&le;', '&ge;', '&sub;', '&sup;', '&nsub;', '&sube;', '&supe;', '&oplus;', '&otimes;', '&perp;', '&sdot;', '&lceil;', '&rceil;', '&lfloor;', '&rfloor;', '&lang;', '&rang;', '&loz;', '&spades;', '&clubs;', '&hearts;', '&diams;');
		return this.swapArrayVals(s, arr1, arr2);
	},


	// Numerically encodes all unicode characters
	numEncode: function(s) {

		if (this.isEmpty(s)) return "";

		var e = "";
		for (var i = 0; i < s.length; i++) {
			var c = s.charAt(i);
			if (c < " " || c > "~") {
				c = "&#" + c.charCodeAt() + ";";
			}
			e += c;
		}
		return e;
	},

	// HTML Decode numerical and HTML entities back to original values
	htmlDecode: function(s) {

		var c, m, d = s;

		if (this.isEmpty(d)) return "";

		// convert HTML entites back to numerical entites first
		d = this.HTML2Numerical(d);

		// look for numerical entities &#34;
		arr = d.match(/&#[0-9]{1,5};/g);

		// if no matches found in string then skip
		if (arr != null) {
			for (var x = 0; x < arr.length; x++) {
				m = arr[x];
				c = m.substring(2, m.length - 1); //get numeric part which is refernce to unicode character
				// if its a valid number we can decode
				if (c >= -32768 && c <= 65535) {
					// decode every single match within string
					d = d.replace(m, String.fromCharCode(c));
				} else {
					d = d.replace(m, ""); //invalid so replace with nada
				}
			}
		}

		return d;
	},

	// encode an input string into either numerical or HTML entities
	htmlEncode: function(s, dbl) {

		if (this.isEmpty(s)) return "";

		// do we allow double encoding? E.g will &amp; be turned into &amp;amp;
		dbl = dbl | false; //default to prevent double encoding

		// if allowing double encoding we do ampersands first
		if (dbl) {
			if (this.EncodeType == "numerical") {
				s = s.replace(/&/g, "&#38;");
			} else {
				s = s.replace(/&/g, "&amp;");
			}
		}

		// convert the xss chars to numerical entities ' " < >
		s = this.XSSEncode(s, false);

		if (this.EncodeType == "numerical" || !dbl) {
			// Now call function that will convert any HTML entities to numerical codes
			s = this.HTML2Numerical(s);
		}

		// Now encode all chars above 127 e.g unicode
		s = this.numEncode(s);

		// now we know anything that needs to be encoded has been converted to numerical entities we
		// can encode any ampersands & that are not part of encoded entities
		// to handle the fact that I need to do a negative check and handle multiple ampersands &&&
		// I am going to use a placeholder

		// if we don't want double encoded entities we ignore the & in existing entities
		if (!dbl) {
			s = s.replace(/&#/g, "##AMPHASH##");

			if (this.EncodeType == "numerical") {
				s = s.replace(/&/g, "&#38;");
			} else {
				s = s.replace(/&/g, "&amp;");
			}

			s = s.replace(/##AMPHASH##/g, "&#");
		}

		// replace any malformed entities
		s = s.replace(/&#\d*([^\d;]|$)/g, "$1");

		if (!dbl) {
			// safety check to correct any double encoded &amp;
			s = this.correctEncoding(s);
		}

		// now do we need to convert our numerical encoded string into entities
		if (this.EncodeType == "entity") {
			s = this.NumericalToHTML(s);
		}

		return s;
	},

	// Encodes the basic 4 characters used to malform HTML in XSS hacks
	XSSEncode: function(s, en) {
		if (!this.isEmpty(s)) {
			en = en || true;
			// do we convert to numerical or html entity?
			if (en) {
				s = s.replace(/\'/g, "&#39;"); //no HTML equivalent as &apos is not cross browser supported
				s = s.replace(/\"/g, "&quot;");
				s = s.replace(/</g, "&lt;");
				s = s.replace(/>/g, "&gt;");
			} else {
				s = s.replace(/\'/g, "&#39;"); //no HTML equivalent as &apos is not cross browser supported
				s = s.replace(/\"/g, "&#34;");
				s = s.replace(/</g, "&#60;");
				s = s.replace(/>/g, "&#62;");
			}
			return s;
		} else {
			return "";
		}
	},

	// returns true if a string contains html or numerical encoded entities
	hasEncoded: function(s) {
		if (/&#[0-9]{1,5};/g.test(s)) {
			return true;
		} else if (/&[A-Z]{2,6};/gi.test(s)) {
			return true;
		} else {
			return false;
		}
	},

	// will remove any unicode characters
	stripUnicode: function(s) {
		return s.replace(/[^\x20-\x7E]/g, "");

	},

	// corrects any double encoded &amp; entities e.g &amp;amp;
	correctEncoding: function(s) {
		return s.replace(/(&amp;)(amp;)+/, "$1");
	},


	// Function to loop through an array swaping each item with the value from another array e.g swap HTML entities with Numericals
	swapArrayVals: function(s, arr1, arr2) {
		if (this.isEmpty(s)) return "";
		var re;
		if (arr1 && arr2) {
			//ShowDebug("in swapArrayVals arr1.length = " + arr1.length + " arr2.length = " + arr2.length)
			// array lengths must match
			if (arr1.length == arr2.length) {
				for (var x = 0, i = arr1.length; x < i; x++) {
					re = new RegExp(arr1[x], 'g');
					s = s.replace(re, arr2[x]); //swap arr1 item with matching item from arr2	
				}
			}
		}
		return s;
	},

	inArray: function(item, arr) {
		for (var i = 0, x = arr.length; i < x; i++) {
			if (arr[i] === item) {
				return i;
			}
		}
		return -1;
	}
}