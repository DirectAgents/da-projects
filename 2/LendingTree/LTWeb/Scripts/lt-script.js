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

function Save() {
    var form = $('#questionForm');
    $('#form-fields').load(form.attr('action'), form.serialize());
}
