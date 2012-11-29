function STMRCWindow(page, sHeight, swidth) {
    if (swidth && sHeight) {
        window.open(page, "CtrlWindow", ",toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,dependent=no,directories=no,width=" + swidth + ",height=" + sHeight + ",x=50,y=50");
    }
    else {
        window.open(page, "CtrlWindow", "toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,dependent=no,directories=no,width=500,height=540,x=50,y=50");
    }
}

function InitializeProgressbar(barvalue) {
    $('#progressbar').progressbar({ value: barvalue });
}

function SetState(replace) {
    var url = "";
    var index = $('input[name="QuestionIndex"]').val();
    if (index > 0)
        url = "?q=" + index;
    if (replace)
        History.replaceState({ index: index }, null, url);
    else
        History.pushState({ index: index }, null, url);
}

function Save() {
    if (processing) {
        alert('already submitted');
        return;
    }
    var form = $('#questionForm');
    if (form.validationEngine('validate')) {
        if ($('input[name="QuestionKey"]').val() == 'SSN') {
            processing = true;
            $('#SaveButton').css('background-image', "url('../../Content/images/btn-processing.jpg')");
            // change button to "processing" & disable clicking again
            // todo? server side check for already submitted LTModel...
        }
        $('#form-fields').load(form.attr('action'), form.serialize(), function () {
            SetState();
        });
    }
}

function CheckLoad(index, baseurl) {
    var pageindex = $('input[name="QuestionIndex"]').val();
    if (pageindex != index)
        $('#form-fields').load(baseurl + '?q=' + index);
        //$('#form-fields').load(baseurl, { q: index }); //does a POST
}