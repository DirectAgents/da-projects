/*
Set to false if you don't want the error messages
*/
var err = true;

/*
Set to true if you want modal javascript dialogues instead of DOM messages
*/
var modal = false;

/*
The type of container to create for the error message
*/
var errContainer = 'div';
var errorbgcolor = '#FF9999';
var validbgcolor = '#ffffff';
var theForm = getFormId();

/*
Browser detect, need's a little work
*/
var ie;
if (navigator.appVersion.indexOf("MSIE")!=-1) {
        ie = true;
} else {
        ie = false;
}

function getFormId() {
        var totalforms = 0;
        var frms = document.getElementsByTagName('form');
        for (var i=0; i<frms.length; i++) {
                if (frms[i].className.indexOf('validate') != -1) {
                        var totalforms = totalforms + 1;
                        var formtovalidate = frms[i].id;
                }
        }
        if (totalforms > 1) {
                alert('Too many forms to validate!');
        } else {
                return document.getElementById(formtovalidate);
        }
}

function getElementId(type) {
        var frm = getFormId();
        var total = 0;
        var el = frm.elements;
        for (var i=0; i<el.length; i++) {
            if (el[i].className.indexOf(type) != -1) {
                        total = total + 1;
                        theElement = el[i].id;
                }
        }
        if (total == 1) {
                return document.getElementById(theElement);
        } else {
                alert('Either too many or too few of type "'+type+'" on the page!');
        }

}

function getOtherElements(frm) {
        var objInput = frm.elements;
        var others = new Array();
        for (var iCounter=0; iCounter<objInput.length; iCounter++) {
                if (objInput[iCounter].className.indexOf('other') != -1) {
                                        others.push(objInput[iCounter].id);
                }
        }
        return others;
}

function changeClass(el,elclass) {
                                resetbg(el);
                                el.className = elclass;
}

var addBlursandHighlights = function() {

                        var objInput = document.getElementsByTagName('input');
                        for (var iCounter=0; iCounter<objInput.length; iCounter++) {
                                objInput[iCounter].onkeyup = function(){return validateSingle(this);}
                                if ((objInput[iCounter].className.indexOf('req') != -1) || (objInput[iCounter].className.indexOf('code') != -1)) {
                                        changeLabelState(objInput[iCounter],'highlight');
                                }
                        }
                        var objInput = document.getElementsByTagName('textarea');
                        for (var iCounter=0; iCounter<objInput.length; iCounter++) {
                                objInput[iCounter].onkeyup = function(){return validateSingle(this);}
                                if (objInput[iCounter].className.indexOf('req') != -1) {
                                        changeLabelState(objInput[iCounter],'highlight');
                                }
                        }
                        var objInput = document.getElementsByTagName('select');
                        for (var iCounter=0; iCounter<objInput.length; iCounter++) {
                                objInput[iCounter].onchange = function(){return validateSingle(this);}
                                if (objInput[iCounter].className.indexOf('req') != -1) {
                                        changeLabelState(objInput[iCounter],'highlight');
                                }
                        }

}

var resetForm = function() {



                        var objInput = getFormId().elements;
                        for (var iCounter=0; iCounter<objInput.length; iCounter++) {

                                        changeLabelState(objInput[iCounter],'reset');
                                        if ( objInput[iCounter].getAttribute('type') == 'text' ) {
                                                objInput[iCounter].value = '';

                                        }
                                        if ( objInput[iCounter].getAttribute('type') == 'textarea' ) {
                                                objInput[iCounter].value = '';

                                        }
                                        if ( objInput[iCounter].getAttribute('type') == 'select' ) {
                                                objInput[iCounter].value = '';

                                        }


                        }
                        objInput[1].focus();

}







var displayErrorMessage = function(frm, el) {



    var labels = document.getElementsByTagName('label');
    var errorMsg = '';

    if (el.type=='radio'||el.type=='checkbox') {
        for (var i = 0; i<labels.length; i++) {
            if (labels[i].htmlFor== el.name) {
                errorMsg = 'You must enter a value for ' + labels[i].innerHTML.replace('*','');
            }
        }
        } else if (el.className.indexOf('email') != -1) {
                for (var i = 0; i<labels.length; i++) {
            if (labels[i].htmlFor==el.id) {
                errorMsg = 'You must enter a <strong>valid</strong> email address.';
            }
        }
    } else if (el.className.indexOf('confirm') != -1) {
                for (var i = 0; i<labels.length; i++) {
            if (labels[i].htmlFor==el.id) {
                errorMsg = 'Both email addresses should be same.';
            }
        }
    } else if (el.className.indexOf('zip') != -1) {
                for (var i = 0; i<labels.length; i++) {
            if (labels[i].htmlFor==el.id) {
                errorMsg = 'You must enter a <strong>valid</strong> zip code.';
            }
        }
    }

    else {
        for (var i = 0; i<labels.length; i++) {
            if (labels[i].htmlFor==el.id) {
                errorMsg = 'You must enter a <strong>valid</strong> value for ' + labels[i].innerHTML.replace('*','');
            }
        }
    }

    if (err) {
        if (modal) {
            alert(errorMsg);
            el.focus();
        }
        else {
            if (document.getElementById(frm.id+'_statusMessage')) {
                                var errorDiv = document.getElementById(frm.id+'_statusMessage');
                                resetbg(errorDiv);
                                errorDiv.className = 'errorMessage';
                                errorDiv.innerHTML = errorMsg;
                                Fat.fade_element(errorDiv.id, 30, 500, '#ffffff', null);
                                el.focus();
            } else {
                                var errorDiv = document.createElement(errContainer);
                                errorDiv.id = frm.id+'_statusMessage';
                                resetbg(errorDiv);
                                errorDiv.className = 'errorMessage';
                                errorDiv.innerHTML = errorMsg;
                                frm.insertBefore(errorDiv, frm.childNodes[0]);
                                Fat.fade_element(errorDiv.id, 30, 500, '#ffffff', null);
                                el.focus();
                        }
        Fat.fade_all();
                }

    }
}

function resetbg(el) {
        el.style.backgroundColor = '';
}

function displayStatusmsg(frm,msg,status) {
        if (err) {
        if (modal) {
            alert(msg);
        }
        else {

                        if (document.getElementById(frm.id+'_statusMessage')) {
                                var statusDiv = document.getElementById(frm.id+'_statusMessage');
                                changeClass(statusDiv,status);
                                Fat.fade_element(statusDiv.id, 30, 500, '#ffffff', null);
                                statusDiv.innerHTML = msg;
            } else {
                                var statusDiv = document.createElement(errContainer);
                                statusDiv.id = frm.id+'_statusMessage';
                                changeClass(statusDiv,status);
                                statusDiv.innerHTML = msg;
                                frm.insertBefore(statusDiv, frm.childNodes[0]);
                                Fat.fade_element(statusDiv.id, 30, 500, '#ffffff', null);
                        }
        }
        }
}

var changeLabelState = function(el,msg) {

    var labels = document.getElementsByTagName('label');

    if (msg == 'error') {
                if (el.className.indexOf('fade') == -1) {
                        el.className = el.className + ' fade';
                }
                for (var i = 0; i<labels.length; i++) {
            if (labels[i].htmlFor==el.id) {
                                labels[i].className = labels[i].className.replace('warning','');
                                labels[i].className = labels[i].className.replace('checkmark','');
                                labels[i].className = labels[i].className.replace('question','');
                                labels[i].className = labels[i].className.replace('highlight','');
                labels[i].className = labels[i].className + ' warning';
            }
        }
        } else if (msg == 'valid') {
                if (el.className.indexOf('fade') != -1) {
                        el.className = el.className.replace('fade','');
                }
                if (el.className.indexOf('error') != -1) {
                        Fat.fade_error_out(el.id, 30, 500, errorbgcolor, validbgcolor);
                }
                for (var i = 0; i<labels.length; i++) {
            if (labels[i].htmlFor==el.id) {
                                labels[i].className = labels[i].className.replace('warning','');
                                labels[i].className = labels[i].className.replace('checkmark','');
                                labels[i].className = labels[i].className.replace('question','');
                                labels[i].className = labels[i].className.replace('highlight','');
                labels[i].className = labels[i].className + ' checkmark';
            }
        }
        } else if (msg == 'question') {
                if (el.className.indexOf('fade') != -1) {
                        el.className = el.className.replace('fade','');
                }
                if (el.className.indexOf('error') != -1) {
                        Fat.fade_error_out(el.id, 30, 500, errorbgcolor, validbgcolor);
                }
                for (var i = 0; i<labels.length; i++) {
            if (labels[i].htmlFor==el.id) {
                                labels[i].className = labels[i].className.replace('warning','');
                                labels[i].className = labels[i].className.replace('checkmark','');
                                labels[i].className = labels[i].className.replace('question','');
                                labels[i].className = labels[i].className.replace('highlight','');
                labels[i].className = labels[i].className + ' question';
            }
        }
        } else {
                for (var i = 0; i<labels.length; i++) {
            if (labels[i].htmlFor==el.id) {
                labels[i].className = labels[i].className.replace('warning','');
                                labels[i].className = labels[i].className.replace('checkmark','');
                                labels[i].className = labels[i].className.replace('question','');
                                labels[i].className = labels[i].className.replace('highlight','');
                                if ((el.className.indexOf('req') != -1) || (el.className.indexOf('code') != -1)) {
                                        labels[i].className = labels[i].className + ' highlight';
                                }
            }
        }
        }
}

/**
* Function called to validate form elements.
* @param frm [Object HTMLFormElement] The form to validate
* @return [Boolean] Did the form validate or not
**/
var validate = function(frm) {

        var el = frm.elements;
        for (var i=0; i<el.length; i++) {
                el[i].className = el[i].className.replace('error','');
                validateSingle(el[i]);
        }
        for (var i=0; i<el.length; i++) {
            if (el[i].className.indexOf('req') != -1 && el[i].className.indexOf('email') == -1 && el[i].className.indexOf('zip') == -1 && el[i].className.indexOf('confirm') == -1) {

                        // Text, Textarea, File
                        if (el[i].type=='text'||el[i].type=='textarea'||el[i].type=='file') {
                                if (el[i].value=="") {
                                        resetbg(el[i]);
                                        el[i].className = el[i].className + ' error';
                                         err == true ? displayErrorMessage(frm, el[i]) : null;
                                        return false;
                                 } else {
                                        resetbg(el[i]);
                                        el[i].className = el[i].className.replace('error','');
                                }

                        }


                        // Radio
                        else if (el[i].type=='radio') {
                                var radiogroup = el[el[i].name];
                        var itemchecked = false;
                        for(var j = 0 ; j < radiogroup.length ; ++j) {
                                        if(radiogroup[j].checked) {
                                                itemchecked = true;
                                                break;
                                        }
                        }
                        if(!itemchecked) {
                                        resetbg(el[i]);
                                        el[i].className = el[i].className + ' error';
                                         err == true ? displayErrorMessage(frm, el[i]) : null;
                                        return false;
                        }
                        }

                        // Checkbox
                        else if (el[i].type=='checkbox') {
                                var itemchecked = false;
                        var elems = document.getElementsByTagName("input");
                        for(var j=0; j<elems.length; j++) {
                                        if(elems[j].type=='checkbox'&&elems[j].name==el[i].name) {
                                        if(elems[j].checked) {
                                                itemchecked = true;
                                                break;
                                        }
                                }
                        }
                        if(!itemchecked) {
                                        resetbg(el[i]);
                                        el[i].className = el[i].className + ' error';
                                         err == true ? displayErrorMessage(frm, el[i]) : null;
                                        return false;
                                }
                        }

                        // Select-one
                        else if (el[i].type=='select-one') {
                                if (el[i].selectedIndex==0) {
                                        resetbg(el[i]);
                                        el[i].className = el[i].className + ' error';
                                         err == true ? displayErrorMessage(frm, el[i]) : null;
                                        return false;
                        } else {
                                        resetbg(el[i]);
                                el[i].className = el[i].className.replace('error','');
                        }
                        }

                        // Select-multiple
                        else if (el[i].type=='select-multiple') {
                                var optionselected = false;
                              for(var j=0;j<el[i].options.length; ++j) {
                                      if (el[i].options[j].selected) {
                                              optionselected = true;
                                              break;
                                      }
                              }
                              if (!optionselected) {
                                        resetbg(el[i]);
                                        el[i].className = el[i].className + ' error';
                                         err == true ? displayErrorMessage(frm, el[i]) : null;
                                        return false;
                        } else {
                                        resetbg(el[i]);
                                el[i].className = el[i].className.replace('error','');
                        }
                        }
                }

                else if (el[i].className.indexOf('zip') != -1) {
                        if (el[i].className.indexOf('req') != -1) {
                                if (el[i].type=='text') {
                                        //var email=/^[A-Za-z0-9]+([_\.-][A-Za-z0-9]+)*@[A-Za-z0-9]+([_\.-][A-Za-z0-9]+)*\.([A-Za-z]){2,4}$/i;
                                        var email = /^([0-9]{5})(?:[-\s]*([0-9]{4}))?$/i;
                                        if (!email.test(el[i].value) || el[i].value == '') {
                                                resetbg(el[i]);
                                                el[i].className = el[i].className + ' error';
                                                err == true ? displayErrorMessage(frm, el[i]) : null;
                                                return false;
                                        } else {
                                                resetbg(el[i]);
                                                el[i].className = el[i].className.replace('error','');
                                        }

                                }
                        }
                }

                else if (el[i].className.indexOf('confirm') != -1) {
                        if (el[i].className.indexOf('req') != -1) {
                                if (el[i].type=='text') {
                                        var email=/^[A-Za-z0-9]+([_\.-][A-Za-z0-9]+)*@[A-Za-z0-9]+([_\.-][A-Za-z0-9]+)*\.([A-Za-z]){2,4}$/i;

                                        var myTextField = getElementId('email').value;


                                        if (!email.test(el[i].value) || el[i].value == '' || el[i].value != myTextField) {
                                                resetbg(el[i]);
                                                el[i].className = el[i].className + ' error';
                                                err == true ? displayErrorMessage(frm, el[i]) : null;
                                                return false;
                                        } else {
                                                resetbg(el[i]);
                                                el[i].className = el[i].className.replace('error','');
                                        }

                                }
                        }
                }
                //email
                else if (el[i].className.indexOf('email') != -1) {
                        if (el[i].className.indexOf('req') != -1) {
                                if (el[i].type=='text') {
                                        var email=/^[A-Za-z0-9]+([_\.-][A-Za-z0-9]+)*@[A-Za-z0-9]+([_\.-][A-Za-z0-9]+)*\.([A-Za-z]){2,4}$/i;
                                        if (!email.test(el[i].value) || el[i].value == '') {
                                                resetbg(el[i]);
                                                el[i].className = el[i].className + ' error';
                                                err == true ? displayErrorMessage(frm, el[i]) : null;
                                                return false;
                                        } else {
                                                resetbg(el[i]);
                                                el[i].className = el[i].className.replace('error','');
                                        }

                                }
                        } else {
                                if (el[i].type=='text') {
                                        var email=/^[A-Za-z0-9]+([_\.-][A-Za-z0-9]+)*@[A-Za-z0-9]+([_\.-][A-Za-z0-9]+)*\.([A-Za-z]){2,4}$/i;
                                        if (!email.test(el[i].value) && el[i].value != '') {
                                                resetbg(el[i]);
                                                el[i].className = el[i].className + ' error';
                                                err == true ? displayErrorMessage(frm, el[i]) : null;
                                                return false;
                                        } else {
                                                resetbg(el[i]);
                                                el[i].className = el[i].className.replace('error','');
                                        }

                                }
                        }
                }
        }
        return true;
}

var validateSingle = function(el) {

            if (el.className.indexOf('req') != -1 && el.className.indexOf('email') == -1 && el.className.indexOf('zip') == -1  && el.className.indexOf('confirm') == -1) {

                        // Text, Textarea, File
                        if (el.type=='text'||el.type=='textarea'||el.type=='file') {
                                if (el.value=="") {
                                         changeLabelState(el,'error');
                                        return false;
                                 } else {
                                        changeLabelState(el,'valid');
                                }


                        }


                        // Radio
                        else if (el.type=='radio') {
                                var radiogroup = el[el.name];
                        var itemchecked = false;
                        for(var j = 0 ; j < radiogroup.length ; ++j) {
                                        if(radiogroup[j].checked) {
                                                itemchecked = true;
                                                break;
                                        }
                        }
                        if(!itemchecked) {
                                        changeLabelState(el,'error');
                                        return false;
                                 } else {
                                        changeLabelState(el,'valid');
                                }
                        }

                        // Checkbox
                        else if (el.type=='checkbox') {
                                var itemchecked = false;
                        var elems = document.getElementsByTagName("input");
                        for(var j=0; j<elems.length; j++) {
                                        if(elems[j].type=='checkbox'&&elems[j].name==el.name) {
                                        if(elems[j].checked) {
                                                itemchecked = true;
                                                break;
                                        }
                                }
                        }
                        if(!itemchecked) {
                                        changeLabelState(el,'error');
                                        return false;
                                 } else {
                                        changeLabelState(el,'valid');
                                }
                        }

                        // Select-one
                        else if (el.type=='select-one') {
                                if (el.selectedIndex==0) {
                                        changeLabelState(el,'error');
                                        return false;
                                 } else {
                                        changeLabelState(el,'valid');
                                }
                        }

                        // Select-multiple
                        else if (el[i].type=='select-multiple') {
                                var optionselected = false;
                              for(var j=0;j<el.options.length; ++j) {
                                      if (el.options[j].selected) {
                                              optionselected = true;
                                              break;
                                      }
                              }
                              if (!optionselected) {
                                        changeLabelState(el,'error');
                                        return false;
                                 } else {
                                        changeLabelState(el,'valid');
                                }
                        }
                }
                else if (el.className.indexOf('zip') != -1) {
                        if (el.type=='text') {
                                //var email=/^[A-Za-z0-9]+([_\.-][A-Za-z0-9]+)*@[A-Za-z0-9]+([_\.-][A-Za-z0-9]+)*\.([A-Za-z]){2,4}$/i;
                                var email = /^([0-9]{5})(?:[-\s]*([0-9]{4}))?$/i;
                                if (email.test(el.value)) {

                                                changeLabelState(el,'valid');

                                 } else if (el.className.indexOf('req') != -1) {
                                                changeLabelState(el,'error');
                                                return false;
                                } else if (!el.value=='') {
                                                changeLabelState(el,'error');
                                                return false;
                                }

                        }
                }

                else if (el.className.indexOf('confirm') != -1) {
                        if (el.type=='text') {
                                var email=/^[A-Za-z0-9]+([_\.-][A-Za-z0-9]+)*@[A-Za-z0-9]+([_\.-][A-Za-z0-9]+)*\.([A-Za-z]){2,4}$/i;

                                var myTextField = getElementId('email').value;

                                if (email.test(el.value) && el.value == myTextField) {

                                                changeLabelState(el,'valid');

                                 } else if (el.className.indexOf('req') != -1) {
                                                changeLabelState(el,'error');
                                                return false;
                                } else if (!el.value=='') {
                                                changeLabelState(el,'error');
                                                return false;
                                }

                        }
                }

                //email
                else if (el.className.indexOf('email') != -1) {
                        if (el.type=='text') {
                                var email=/^[A-Za-z0-9]+([_\.-][A-Za-z0-9]+)*@[A-Za-z0-9]+([_\.-][A-Za-z0-9]+)*\.([A-Za-z]){2,4}$/i;
                                if (email.test(el.value)) {


                                                changeLabelState(el,'valid');

                                 } else if (el.className.indexOf('req') != -1) {
                                                changeLabelState(el,'error');
                                                return false;
                                } else if (!el.value=='') {
                                                changeLabelState(el,'error');
                                                return false;
                                }

                        }
                } else {
                        if (el.type=='text'||el.type=='textarea'||el.type=='file') {
                                if (el.value=="") {
                                         changeLabelState(el,'question');
                                        return false;
                                 } else {
                                        changeLabelState(el,'valid');
                                }

                        }
                }

        return true;
}

/**
* Function called when a element changes it's value.
**/
var evaluate = function() {
        for (i=0; i<conds.length; i++) {
                var element = document.getElementsByName(conds[i][0]);
                var elemVal = conds[i][1];
                var elements = document.getElementsByName(conds[i][2]);
                var bol = conds[i][3];

                if (ie) {
                /* For Internet Explorer */
                        for (var k=0; k<element.length; k++) {
                                if (element[k].name==event.srcElement.name) {
                                        if (event.srcElement.type=='select-one') {
                                                if(event.srcElement[event.srcElement.selectedIndex].value==elemVal) {
                                                        setState(elements, bol);
                                                } else {
                                                        setState(elements, !bol);
                                                }
                                        } else if (event.srcElement.type=='select-multiple') {
                                                var optionselected = false;
                                                for(var j=0; j<event.srcElement.options.length; j++) {
                                                      if (event.srcElement.options[j].value==elemVal&&event.srcElement.options[j].selected) {
                                                              optionselected = true;
                                                              break;
                                                      }
                                              }
                                                if (optionselected) {
                                                        setState(elements, bol);
                                                } else {
                                                        setState(elements, !bol);
                                                }
                                        } else {
                                                if(event.srcElement.value==elemVal) {
                                                        setState(elements, bol);
                                                } else {
                                                        setState(elements, !bol);
                                                }
                                        }
                                }
                        }
                } else {
                /* For other browsers */
                        for (var k=0; k<element.length; k++) {
                                if (element[k].name==this.name) {
                                        if (this.type=='select-one') {
                                                if(this[this.selectedIndex].value==elemVal) {
                                                        setState(elements, bol);
                                                } else {
                                                        setState(elements, !bol);
                                                }
                                        } else if (this.type=='select-multiple') {
                                                var optionselected = false;
                                                for(var j=0; j<this.options.length; j++) {
                                                      if (this.options[j].value==elemVal&&this.options[j].selected) {
                                                              optionselected = true;
                                                              break;
                                                      }
                                              }
                                                if (optionselected) {
                                                        setState(elements, bol);
                                                } else {
                                                        setState(elements, !bol);
                                                }
                                        } else {
                                                if(this.value==elemVal) {
                                                        setState(elements, bol);
                                                } else {
                                                        setState(elements, !bol);
                                                }
                                        }
                                }
                        }
                }
        }
}

/**
* Set a single or a group of elements to required or not required.
* @param elements [Object HTMLCollection] The elements to set required states on
* @param bol [Boolean] Set elements required or not required
**/
var setState = function(elements, bol) {
        if (bol) {
                for (var j=0; j<elements.length; j++) {
                        if (elements[j].className.indexOf('req')==-1) {
                                elements[j].className += ' req';
                        }
                }
        } else {
                for (var j=0; j<elements.length; j++) {
                        newClassName = elements[j].className.replace('req','');
                        elements[j].className = newClassName;
                }
        }
}

/**
* Attach conditions to form elements.
**/
var attachConditions = function() {
/*        attachBlurs(document.getElementsByTagName('input'));
        attachBlurs(document.getElementsByTagName('select'));*/
        var frmEl = getFormId();
        if (ie) {
                frmEl.attachEvent('onsubmit', validateFields, false);
//		document.getElementById('fm_zip').attachEvent('onblur', validateFields, false);
		document.getElementById('Submit').attachEvent('onclick', validateFields, false);
        } else {
                frmEl.addEventListener('submit', validateFields, false);
        }
        if (ie) {
                frmEl.attachEvent('onreset', resetForm, false);
        } else {
                frmEl.addEventListener('reset', resetForm, false);
        }
        frmEl.onsubmit = function() { return false; }
}

/**
* Attach onBlur or onClick to elements depending on element type.
* @param elements [Object HTLMCollection] The elements to attach onBlur events to
**/
var attachBlurs = function(elements) {
        for (var i=0; i<elements.length; i++) {
                if (elements[i].type=='checkbox'||elements[i].type=='radio') {
                        if (ie) {
                                elements[i].attachEvent('onclick', evaluate, false);
                        } else {
                                elements[i].addEventListener('click', evaluate, false);
                        }
                } else {
                        if (ie) {
                                elements[i].attachEvent('onblur', evaluate, false);
                        } else {
                                elements[i].addEventListener('blur', evaluate, false);
                        }
                }
        }
}

/*
Conditions
Syntax: conds.push(Array('element name','element value','element name', false));
You should put your conditions in a seperate file and include in the page.
*/
var conds = Array();



/*
AJAXify
*/
function validateFields() {
        var frmEl = getFormId();
        if (validate(frmEl)) {
                //sendPosEmail();
                frmEl.onsubmit = function() { return true; }   
        }
}


function cleanString(str1) {
        str1 = str1.replace(/&/g,"**am**");
        str1 = str1.replace(/=/g,"**eq**");
        str1 = str1.replace(/\+/g,"**pl**");
        return str1;
}

function unCleanString(str1) {
        str1 = str1.replace(/\*\*am\*\*/g,"&");
        str1 = str1.replace(/\*\*eq\*\*/g,"=");
        str1 = str1.replace(/\*\*pl\*\*/g,"+");
        str1 = str1.replace(/\*\*nl\*\*/g,"<br />");
        return str1;
}

function hidemessage() {
        msg = '<strong>Thank you!</strong> Your <a href="javascript:showmessage();" title="Click here to see the message you sent">message</a> has been sent.'
        displayStatusmsg(getFormId(),msg,'OK');
}

function showmessage() {
        var fieldArea = getFormId();
        var name = unCleanString(grabPosXML("name"));
        var email = unCleanString(grabPosXML("email"));
        var subject = unCleanString(grabPosXML("regarding"));
        var message = unCleanString(grabPosXML("message"));
        var msg = '<h2>From: <span>'+name+' ('+email+')</span></h2><br /><h2>Subject: <span>'+subject+'</span></h2><br /><h2>Message:</h2> <p class="message">'+message+'</p><a href="javascript:hidemessage();" title=Click here to hide this message">Hide</a>';
        displayStatusmsg(fieldArea,msg,'none');

}



/*
Add onLoad events to start the whole thing
*/
function ajaxContact() {
        var frmEl = getFormId();
        addEvent(frmEl, 'submit', validateFields, false);
        addEvent(frmEl, 'reset', resetForm, false);
        frmEl.onsubmit = function() { return false; }

}

if (ie) {
        window.attachEvent("onload", addBlursandHighlights, false);
        window.attachEvent("onload", attachConditions, false);

} else {
        window.addEventListener("load", addBlursandHighlights, false);
        window.addEventListener("load", attachConditions, false);
}
/*addEvent(window, 'load',ajaxContact, false);
addEvent(window,'unload',EventCache.flush, false);*/
