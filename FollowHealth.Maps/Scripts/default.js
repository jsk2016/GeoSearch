function sendPartialRequest(url, data, callback) {
    processing = 1;
    $.post(url, data, function (response, status) {
        if (callback != null) {
            var result = new Object();
            result.responseText = response;
            callback.apply(result);

        }

        processing = 0;
    })

     .success(function (response, status, errorMsg) {
         showHideDivMask('IMGDIV', 'none');
         var suc = new Object();
         suc.Code = errorMsg.status;
         suc.MessageType = status;
         //if (toolBarBtn == "Save") {
         //    if (recordSave.toString() == "true") {
         //        suc.Message = "Record saved successfully";
         //        showResponseStatus(suc);
         //    }
         //}
         //else if (toolBarBtn == "Update") {
         //    if (recordSave == "true") {
         //        suc.Message = "Record Updated successfully";
         //        toolBarBtn = "";
         //        showResponseStatus(suc);
         //    }
         //}
         //else {
         //    return false;
         //}
         processing = 0;
     })

    .error(function (response, status, errorMsg) {
        showHideDivMask('IMGDIV', 'none');
        var err = new Object();
        try {
            err = eval("(" + response.responseText + ")");
        }
        catch (e) {
            err = new Object();
            if (response.status == 404) {
                err.Code = response.status;
                err.Message = "URL does not exist.";
                err.MessageType = "Error";
            }
            else if (response.status == 200) {
                // alert(2);
            }
            else {
                err.Code = response.status;
                err.Message = "An error occurred while processing!";
                err.MessageType = "Error";
            }
        }
        showResponseStatus(err);
        processing = 0;
    })

}

var timerToResponseStatus = 0;
function showResponseStatus(msg) {
    if (msg == null) {
        return;
    }
    var message = msg.Message;
    var msgType = msg.MessageType;
    var msgDivID = "#divResponseStatus", close = "$('" + msgDivID + "').hide(123);";
    clearTimeout(timerToResponseStatus);
    with ($(msgDivID)) {
        html("<div class='responseMsgPanelHdr'><div><table width='100%'><tr><td>" + msgType + "</td><td align='right' ><a   href=\"javascript:void(0)\" onclick=\"$('" + msgDivID + "').hide(111);\"> X </a></td></tr></table></div></div><div class='responseMessage' style=\"background:url('/Images/" + msgType + "32.png') no-repeat 3px 2px;\">" + message + "</div>").show().css({ left: Math.abs($(window).width() - width() - 15), top: 5 });
    }
    timerToResponseStatus = setTimeout(close, 5000);
    // if (/Info/i.test(msgType)) 
}

//show / hide div mask
function showHideDivMask(div, dis) {
    showMask(dis);
    divPopUp = $get(div);
    if (dis == "block") {
        isPopupDisplayed = true;
        setMiddleOfWindow($get(div));

        if (divPopUp)
            divPopUp.style.zIndex = 100010;
    }
    else {
        if (divPopUp)
            divPopUp.style.zIndex = 0;
        isPopupDisplayed = false;
        showObj(div, dis);
    }
    return false;
}

function showMask(dis) {
    showObj('divMask', dis);
    if ($get('divMask')) {
        with ($get('divMask').style) {
            if (display == 'block') {
                zIndex = 100005;
                width = document.documentElement.scrollWidth + "px";
                height = document.documentElement.scrollHeight + "px";
            }
            else
                zIndex = 0;
        }
    }
}

function setMiddleOfWindow(obj) {
    if (!obj) return;
    with (obj.style) {
        display = 'block';
        left = (Math.abs($(window).width() - obj.offsetWidth) / 2) + "px";
        top = (Math.abs($(window).height() - obj.offsetHeight) * 0.4) + "px";
    }
}
function showObj(i, dis) {
    if ($get(i)) $get(i).style.display = dis;
}