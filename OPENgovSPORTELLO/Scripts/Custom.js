$(document).ready(function () {
    CheckCoookiePolicy();
    if ($('#hdDescrEnte').val() != '' && $('#MainContent_hfFrom').val() == 'FO')
    {
        $('#lblDescrEnte').text('Comune di ' + $('#hdDescrEnte').val().replace('&rsquo;', '&rsquo;'));
    }
    else {
        $('#lblDescrEnte').text('Sportello on-line');
    }
    var sErrore = $('#OnlyNumber_error').text();
    if (sErrore == undefined)
        sErrore = '';
    if (sErrore.indexOf('data inizio') > 0) {
        $('#MainContent_lblDataInizioORG').show();
    }
    else {
        $('#MainContent_lblDataInizioORG').hide();
    }
    if ($('#MainContent_hfFrom').val() == 'FO') {
        $('#Home').addClass('HomeFO');
        $('#FAQ').addClass('FAQFO');
        $('.NewLogin').addClass('NewLoginFO');
        $('.Login').addClass('LoginFO');
    }
    else {
        $('#Home').addClass('HomeBO');
        $('#FAQ').addClass('FAQBO');
        $('.NewLogin').addClass('NewLoginBO');
        $('.Login').addClass('LoginBO');
        $('.nascondiregistrati').hide();
    }
    $('#MainContent_Exit').click(function () {
        $('a.Tributi').trigger('click');
    });
    $(".validEmail").verimail({
        denyTempEmailDomains: true,
        messageElement: "p#MessageEmail"
    });
    $(".OnlyNumber").keypress(function (e) {
        var chr = String.fromCharCode(e.which);
        if ("0123456789., 	".indexOf(chr) < 0) {
            console.log('sono su:' + chr + 'FINE');
            console.log('che è su:' + e.which + 'FINE');
            $('label#OnlyNumber_error').text('Inserire solo Numeri!');
            $('label#OnlyNumber_error').show(); //Show error
            $('#lblErrorBO').text('Inserire solo Numeri!');
            $('#lblErrorBO').show(); //Show error
            $('#lblErrorFO').text('Inserire solo Numeri!');
            $('#lblErrorFO').show(); //Show error
            $('#' + $(this).attr('id')).text('');
            $('#' + $(this).attr('id')).focus();
            return false;
        }
    }
    );
    $(".OnlyNumber").blur(function () {
        var chr = $(".OnlyNumber").val();
        chr = chr.replace(',', '').replace('.', '').replace(' ','');
        if (chr !== "" && !$.isNumeric(chr)) {
            $('label#OnlyNumber_error').text('Inserire solo Numeri!');
            $('label#OnlyNumber_error').show(); //Show error
            $('#lblErrorBO').text('Inserire solo Numeri!');
            $('#lblErrorBO').show(); //Show error
            $('#lblErrorFO').text('Inserire solo Numeri!');
            $('#lblErrorFO').show(); //Show error
            console.log(chr);
            $('#' + $(this).attr('id')).text('');
            $('#' + $(this).attr('id')).focus();
            return false;
        }
    });
    $('[id*="_txt"]').blur(function () {
        $('label#OnlyNumber_error').text('');
        $('label#OnlyNumber_error').hide();
        $('#lblErrorBO').text('');
        $('#lblErrorBO').hide();
        if ($('#' + $(this).attr('id')).val().indexOf('../') > 0 || $('#' + $(this).attr('id')).val().indexOf('<') > 0 || $('#' + $(this).attr('id')).val().indexOf('>') > 0) {
            $('label#OnlyNumber_error').text('Presenza di caratteri non validi! Non sono accettati ../ < >');
            $('label#OnlyNumber_error').show(); //Show error
            $('#lblErrorBO').text('Presenza di caratteri non validi! Non sono accettati ../ < >');
            $('#lblErrorBO').show(); //Show error
            $('#lblErrorFO').text('Presenza di caratteri non validi! Non sono accettati ../ < >');
            $('#lblErrorFO').show(); //Show error
            $('#' + $(this).attr('id')).focus();
            return false;
        }
    });
    $("#divLeftMenu a").click(function () {
        if ($('#hfInitDich').val() == '1') {
            alert('Confermare l\'inserimento della dichiarazione mediante il tasto ‘Conferma Dichiarazione’');
            return false;
        }
    });
    $(".nonImplementato").click(function () {
        alert('Non disponibile');
        return false;
    });
    $(".container .BottoneF24").click(function () {
        if ($('#hfInitDich').val() == '1') {
            alert('Confermare l\'inserimento della dichiarazione mediante il tasto ‘Conferma Dichiarazione’');
            return false;
        }
    });
    if ($('#SearchStradario').length > 0) {
        $('.txtVia').attr('readonly', 'true');
        $('.txtVia').hover(function () {
            $(this).prop('title', 'Selezionare la via da stradario');
        });
        $('.txtVia').click(function () {
            alert('Selezionare la via da stradario');
        });
    }
    else {
        $('.txtVia').attr('readonly', '');
    }
    if ($('#SearchEnti').length > 0) {
        var descr = "Selezionare il comune da elenco";
        $('#MainContent_txtLuogoNascita').attr('readonly', 'true');
        $('#MainContent_txtLuogoNascita').hover(function () {
            $(this).prop('title', descr);
        });
        $('#MainContent_txtLuogoNascita').click(function () {
            alert(descr);
        });
        $('#MainContent_txtComuneRes').attr('readonly', 'true');
        $('#MainContent_txtComuneRes').hover(function () {
            $(this).prop('title', descr);
        });
        $('#MainContent_txtComuneRes').click(function () {
            alert(descr);
        });
        $('#MainContent_txtComuneCO').attr('readonly', 'true');
        $('#MainContent_txtComuneCO').hover(function () {
            $(this).prop('title', descr);
        });
        $('#MainContent_txtComuneCO').click(function () {
            alert(descr);
        });
    }
    else {
        $('#MainContent_txtVia').attr('readonly', '');
    }

    $('#MainContent_txtPercPos').blur(function () {
        var chr = $('#MainContent_txtPercPos').val();
        if (chr > 100) {
            $('label#OnlyNumber_error').show(); //Show error
            $('#MainContent_txtPercPos').focus(); //Focus on field
            alert('La percentuale può essere al massimo 100%!');
            return false;
        }
    });
    $("#MainContent_ddlTipologia").change(function () {
        LockPertinenza();        
        ForceCat();
    });   
    $("#MainContent_ddlTipoAtto").change(function () {
        LoadTipoAttoOSAP();
    });
    $('#MainContent_txtPercRid').on('focus', '.insert_adresse', function () {
        // $(this).css('background-color', 'yellow');
        $(this).attr('value', '');
    });

    LoadTooltip();
    LockPertinenza();
    LoadTipoAttoOSAP();
    ManageBottoniera();
    $("#MainContent_optSintetica, #MainContent_optAnalitica, #MainContent_optRaffronto").change(function () {
        ChangeOptAnalisi();
    });

    //tentativo di regolare altezza iframe in base al contenuto
    //$(".iframe").load(function () { console.log("ciao"); $(this).css("height", $(this).contents().height() + "px"); });

    //$(".tooltip").is(":onscreen"); // true / false (partially on screen)
    $('div#TheDialogNews').dialog({ autoOpen: false });
    $('.ui-button-text').text('chiudi');
    $('*[class^="Help"]').attr('target', '_blank');
    $('.VincoloVariazione').keypress(function () {
        if($('#MainContent_lblDataInizioORG').text()!="")
        $('#MainContent_txtDataInizio').val('');
    }
    );
    $('.VincoloVariazione').change(function () {
        if ($('#MainContent_lblDataInizioORG').text() != "")
        $('#MainContent_txtDataInizio').val('');
    }
    );
    $('#MainContent_txtFoglio, #MainContent_txtNumero, #MainContent_txtSub').change(function () {
        if ($('#MainContent_ddlZona').is(':disabled')) {
            $('#MainContent_ddlZona').removeAttr('disabled');
        }
    }
    );
    $(".AnnoAzzera").change(function () {
        $('#MainContent_GrdCalcolo').hide();
        $('#lblTotVersato').hide();
        $('#lblTotDifImposta').hide();
        $('#lblTotDovuto').hide();
    });
    $('#MainContent_Password, #MainContent_ConfirmPassword').keyup(function () {
        var myPwd = $('#MainContent_Password').val();
        console.log('pwd->' + myPwd);
        //a lower case letter must occur at least once
        var lowerCaseLetters = /[a-z]/g;
        if (myPwd.match(lowerCaseLetters)) {
            RuleLowerCase.classList.remove('text-danger');
            RuleLowerCase.classList.add('text-success');
        } else {
            console.log('non ho minuscole');
            RuleLowerCase.classList.remove('text-success');
            RuleLowerCase.classList.add('text-danger');
        }
        // an upper case letter must occur at least once
        var upperCaseLetters = /[A-Z]/g;
        if (myPwd.match(upperCaseLetters)) {
            RuleUpperCase.classList.remove('text-danger');
            RuleUpperCase.classList.add('text-success');
        } else {
            console.log('non ho maiuscole');
            RuleUpperCase.classList.remove('text-success');
            RuleUpperCase.classList.add('text-danger');
        }
        // a special character must occur at least once
        var specialChr = /[@#$%^&+=!._-]/g;
        if (myPwd.match(specialChr)) {
            console.log('ho speciali->'+specialChr);
            RuleSpecialChr.classList.remove('text-danger');
            RuleSpecialChr.classList.add('text-success');
        } else {
            console.log('non ho speciali');
            RuleSpecialChr.classList.remove('text-success');
            RuleSpecialChr.classList.add('text-danger');
        }
        //a digit must occur at least once
        var numbers = /[0-9]/g;
        if (myPwd.match(numbers)) {
            RuleNumber.classList.remove('text-danger');
            RuleNumber.classList.add('text-success');
        } else {
            console.log('non ho numeri');
            RuleNumber.classList.remove('text-success');
            RuleNumber.classList.add('text-danger');
        }
        //no whitespace allowed in the entire string
        var whitespace = / /g;
        if (!myPwd.match(whitespace)) {
            RuleWhiteSpace.classList.remove('text-danger');
            RuleWhiteSpace.classList.add('text-success');
        } else {
            console.log('non ho spazi');
            RuleWhiteSpace.classList.remove('text-success');
            RuleWhiteSpace.classList.add('text-danger');
        }
        //at least 8 characters
        if (myPwd.length >= 8) {
            RuleLength.classList.remove('text-danger');
            RuleLength.classList.add('text-success');
        } else {
            console.log('non 8chr');
            RuleLength.classList.remove('text-success');
            RuleLength.classList.add('text-danger');
        }
        return true;
    });
    /**** 201807 - traduzione pagina ****/
    $('.translation-links a').click(function () {
        console.log('traduco');
        var lang = $(this).data('lang');
        var $frame = $('.goog-te-menu-frame:first');
        if (!$frame.size()) {
            alert("Error: Could not find Google translate frame.");
            return false;
        }
        try {
            $frame.contents().find('.goog-te-menu2-item span.text:contains(' + lang + ')').get(0).click();
        } catch (err) {
            console.log('traduco errore' + err);
        }
        return false;
    });
    /****  ****/
});
$(document).on('focus', '.DefaultPercRid', function () {
    $('#MainContent_txtPercRid').attr('value', '50');
});
$(document).on('focus', '.DefaultPercEse', function () {
    $('#MainContent_txtPercEse').attr('value', '100');
});
function Scopri(id) {
    $('#' + id).show();
}
function Nascondi(id) {
    $('#' + id).hide();
}
function ShowHideDiv(chiamante, oggetto, label) {
    if ($('#' + oggetto).is(':hidden')) {
        $('#' + oggetto).show();
        chiamante.title = 'Nascondi ';// + label;
        //chiamante.innerText = label;
    } else {
        $('#' + oggetto).hide();
        chiamante.title = 'Visualizza ';// + label;
        //chiamante.innerText = label;
    }
}
/*function YouTubeLink(oggetto, link) {
    ShowHideDiv(this, 'ytplayer', 'youtube');
    if ($('#' + oggetto).is(':hidden')) {
        $('#' + oggetto).attr('src', '');
        chiamante.title = 'Nascondi ';
    } else {
        $('#' + oggetto).attr('src', link);
        chiamante.title = 'Visualizza ';
    }
}*/
function uncheckGrdBtn(idControllo) {
    var str = idControllo.substr(0, idControllo.indexOf('_chkSel'));
    console.log('faccio il substring di id str= ' + str);
    var rowCount = $('#' + str + ' > tbody > tr').size();
    console.log('conto i tr all\' interno della tabella e li assegno alla variabile rowCount:  ' + rowCount);
    for (var i = 0; i < rowCount; i++) {
        console.log('sono dentro al FOR');
        console.log('ciclo all interno della tabella');
        console.log('mi chiedo se il check di parametro non è selezionato');
        if ($('#' + idControllo).is(':checked')) {
            console.log('non è selezionato, metto tutti gli altri check deselezionati');
            // $('#' + str + 'tbody > tr').find('[id *= "chkSel"]').attr('checked', 'checked');
            $('tbody tr td input[type="checkbox"]').each(function () {
                if (this.id != idControllo)
                    $(this).prop('checked', false);
            })
        }
    }
    console.log('sono fuori dal FOR');
}
function ShowHideGrdBtn(idControllo) {
    uncheckGrdBtn(idControllo);

    if ($('#' + idControllo).is(':checked')) {
        $('#' + idControllo).closest('td').find('.divGrdBtn').show();
    }
    else {
        $('#' + idControllo).closest('td').find('.divGrdBtn').hide();
    }   

    if ($('#' + idControllo).closest('tr').find('[id*="txtAl"]').val() != '')
    {
        $('#' + idControllo).closest('tr').find('.BottoneClose').hide();
        $('#' + idControllo).closest('tr').find('p#cessazione').hide(); $('#' + idControllo).closest('tr').find('p#close').hide();
        $('#' + idControllo).closest('tr').find('.BottoneAlter').hide();
        $('#' + idControllo).closest('tr').find('p#variazione').hide();
        $('#' + idControllo).closest('tr').find('.BottoneAttention').hide();
        $('#' + idControllo).closest('tr').find('p#inagibile').hide();
        $('#' + idControllo).closest('tr').find('.BottoneUserGroup').hide();
        $('#' + idControllo).closest('tr').find('p#usogratuito').hide();
        $('#' + idControllo).closest('tr').find('.BottoneFolderAdd').hide();
        $('#' + idControllo).closest('tr').find('p#folderadd').hide();
    } else {
        $('#' + idControllo).closest('tr').find('.BottoneClose').show();
        $('#' + idControllo).closest('tr').find('p#cessazione').show(); $('#' + idControllo).closest('tr').find('p#close').show();
        $('#' + idControllo).closest('tr').find('.BottoneAlter').show();
        $('#' + idControllo).closest('tr').find('p#variazione').show();
        $('#' + idControllo).closest('tr').find('.BottoneAttention').show();
        $('#' + idControllo).closest('tr').find('p#inagibile').show();
        $('#' + idControllo).closest('tr').find('.BottoneUserGroup').show();
        $('#' + idControllo).closest('tr').find('p#usogratuito').show();
        $('#' + idControllo).closest('tr').find('.BottoneFolderAdd').show();
        $('#' + idControllo).closest('tr').find('p#folderadd').show();
    }
    if ($('#' + idControllo).closest('td').find('.divGrdBtn').find('input:visible').length <=3)
        $('.panelGrd').height(50);
    else if ($('#' + idControllo).closest('td').find('.divGrdBtn').find('input:visible').length <= 6)
        $('.panelGrd').height(125);
}


function ShowHideGrdConfTariffe(typeGrdConf) {
    if (typeGrdConf == '67' || typeGrdConf=='93') {
        $('[id*="lblHeaderGrdAliquoteCod"]').text('Tipo Utilizzo');
        $('[id*="lblHeaderGrdAliquoteAliq"]').text('% Aliquota');
        $('[id*="lblHeaderGrdAliquoteCodEst"]').text('Codice Tributo F24');
        $('[id*="txtCodice"]').hide();
        if (typeGrdConf == '93') {
            $('[id*="lblHeaderGrdAliquoteProp"]').text('% Proprietario');
            $('[id*="lblHeaderGrdAliquoteInq"]').text('% Inquilino');
        }
    }else {
        if (typeGrdConf == '64') 
            $('[id*="lblHeaderGrdAliquoteAliq"]').text('Valore/MQ');
        else if (typeGrdConf == '66')
            $('[id*="lblHeaderGrdAliquoteAliq"]').text('% Riduzione');
        $('[id*="lblHeaderGrdAliquoteCod"]').text('Codice');
        $('[id*="lblHeaderGrdAliquoteCodEst"]').text('Codice Banca Dati Esterna');
        $('[id*="ddlTipologia"]').hide();
    }
}
function ShowHideGrdBtnDeleghe(idControllo) {
    if ($('#' + idControllo).closest('tr').find('[id*="lblStato"]').text().indexOf('respinta') > 0 || $('#' + idControllo).closest('tr').find('[id*="lblStato"]').text().indexOf('rimossa') > 0) {
        $('#' + idControllo).closest('td').find('.divGrdBtn').hide();
    } else if ($('#' + idControllo).closest('tr').find('[id*="lblStato"]').text().indexOf('Delega dal')>=0) {
        $('#' + idControllo).closest('tr').find('.BottoneAttach').hide(); $('#' + idControllo).closest('tr').find('p#Allega').hide();
        if ($('#' + idControllo).is(':checked')) {
            $('#' + idControllo).closest('td').find('.divGrdBtn').show();
        }
        else {
            $('#' + idControllo).closest('td').find('.divGrdBtn').hide();
        }
    } else {
        if ($('#' + idControllo).closest('tr').find('[id*="hfIdRow"]').val()<= 0) {
            $('#' + idControllo).closest('tr').find('.BottoneAttach').hide(); $('#' + idControllo).closest('tr').find('p#Allega').hide();
            $('#' + idControllo).closest('tr').find('.BottonePrint').hide(); $('#' + idControllo).closest('tr').find('p#Stampa').hide();
        }
        else {
            $('#' + idControllo).closest('tr').find('.BottoneAttach').show(); $('#' + idControllo).closest('tr').find('p#Allega').show();
            $('#' + idControllo).closest('tr').find('.BottonePrint').show(); $('#' + idControllo).closest('tr').find('p#Stampa').show();
        }
        if ($('#' + idControllo).is(':checked')) {
            $('#' + idControllo).closest('td').find('.divGrdBtn').show();
        }
        else {
            $('#' + idControllo).closest('td').find('.divGrdBtn').hide();
        }
    }
    if ($('#' + idControllo).closest('td').find('.divGrdBtn').find('input:visible').length <= 3) {
        $('.panelGrd').height(50);
    }
    else if ($('#' + idControllo).closest('td').find('.divGrdBtn').find('input:visible').length <= 6) {
        $('.panelGrd').height(125);
    }
}
function ShowHideOccupanti() {
    var NC = 0;
    var MQ = 0.00;
    var MQNetti = 0.00;
    $('#MainContent_GrdVani tr').each(function (i, row) {
        // reference all the stuff you need first
        var $row = $(row),
            $nc = $row.find('input[name*="txtNC"]'),
            $mq = $row.find('input[name*="txtMQ"]'),
            $checkedBoxes = $row.find('input:checked');
        NC = $nc.val();
        if (!isNaN($mq.val())) {
            MQ += parseFloat($mq.val().replace(',', '.'));
            MQNetti += parseFloat($mq.val().replace(',', '.'));
        }
        $checkedBoxes.each(function (i, checkbox) {
            if (!isNaN($mq.val())) {
                MQNetti -= parseFloat($mq.val().replace(',', '.'));
            }
        });
    });
    $('#lblTotMQ').text('Tot. MQ:' + MQ);
    $('#lblTotTassabili').text('Tot. MQ Tassabili:' + MQNetti);
    if (NC > 0) {
        $('#divOccupanti').show();
    }
    else {
        $('#divOccupanti').hide();
    }
}
function ShowHideRibalta(id, idControllo) {
    if ($('#' + idControllo).val() == '1') {
        $('#' + id).show();
    }
    else {
        $('#' + id).hide();
    }
}
function ShowHideSettings(id) {
    if ($('#' + id).is(':visible'))
        $('#' + id).hide(250);
    else
        $('#' + id).show(250);
}
function SetIDGrdBtn(id) {
    console.log('#' + id);
}
function CheckInitDich() {
    if ($('#hfInitDich').val() == '1') {
        alert('Confermare l\'inserimento della dichiarazione mediante il tasto ‘Conferma Dichiarazione’');
    }
}
function Setfocus(objField) {
    //vengono intercettati gli errori in modo che se il controllo non è visibile
    //non sia visualizzato nessun errore
    try {
        objField.focus();
        objField.select();
    }
    catch (objErr)
    { }
}
function TrackBlur(objField) {
    objField.select();
}
function txtDateGotfocus(objCtrl) {
    if (isUndefined(objCtrl)) return false;

    objCtrl.select();
}
function txtDateLostfocus(objCtrl) {
    var gg = "";
    var mm = "";
    var aaaa = "";
    var strData;
    var strTmp;
    var len;

    if (isUndefined(objCtrl)) return false;

    strData = objCtrl.value;
    if (strData == "") return false;

    arrayOfStrings = strData.split(".");
    if (arrayOfStrings.length < 3) {
        arrayOfStrings = strData.split("/");
        if (arrayOfStrings.length < 3) {
            arrayOfStrings = strData.split("-");
            if (arrayOfStrings.length < 3) {
                arrayOfStrings = strData.split(",");
            }
        }
    }

    if ((arrayOfStrings.length == 1) &&
 ((arrayOfStrings[0].length == 6) || (arrayOfStrings[0].length == 8))) {
        len = arrayOfStrings[0].length;
        strTmp = arrayOfStrings[0];
        arrayOfStrings[0] = strTmp.substr(0, 2);
        arrayOfStrings[1] = strTmp.substr(2, 2);
        arrayOfStrings[2] = strTmp.substr(4, (len == 8) ? 4 : 2);
    }

    if (arrayOfStrings.length > 0)
        gg = removeChar(arrayOfStrings[0], "");
    if (arrayOfStrings.length > 1)
        mm = removeChar(arrayOfStrings[1], "");
    if (arrayOfStrings.length > 2)
        aaaa = removeChar(arrayOfStrings[2], "");

    //porta il formato sempre a GG/MM/AAAA	
    if (gg.length == 0) gg = "00";
    if (gg.length == 1) gg = "0" + gg;
    if (mm.length == 0) mm = "00";
    if (mm.length == 1) mm = "0" + mm;
    if ((strTmp = CompletaAnno(aaaa)) != "")
        aaaa = strTmp;
    else
        if (aaaa.length < 4) aaaa = "0000";

    objCtrl.value = gg + "/" + mm + "/" + aaaa

    return true;
}
function VerificaData(Data) {
    if (!IsBlank(Data.value)) {
        if (!isDate(Data.value)) {
            $('#OnlyNumber_error').text('Data non valida!');
            $('#OnlyNumber_error').show(); //Show error
            Setfocus(Data);
            return false;
        }
    }
}
function isUndefined(strField) {
    if (typeof (strField) == "undefined")
        return true;
    else
        return false;
}
function IsBlank(sField) {
    var bChar = 0;
    if (sField)
        for (var i = 0; i < sField.length; i++) {
            //altro 160 codice per lo spazio??? 
            if (sField.charAt(i) != " " && sField.charCodeAt(i) != 160) {
                bChar = 1;
                break;
            }
        }
    if (bChar == 0)
        return true;
    else
        return false;
}
function removeChar(strValue, strNewChar) {
    var reTmp = /\D/g;
    var blnNeg = false;

    if (isUndefined(strValue)) strValue = "";
    if (isUndefined(strNewChar)) strNewChar = "";
    if (strValue.charAt(0) == '-') blnNeg = true;

    strValue = strValue.replace(reTmp, strNewChar);
    if (blnNeg) strValue = '-' + strValue;

    return strValue;
}
function CompletaAnno(sAnno) {
    if (isNaN(sAnno)) return "";
    if (sAnno.length > 4 || sAnno.length < 2) return "";

    if (sAnno.length == 2)
        if (parseInt(sAnno) <= 99 && parseInt(sAnno) >= 30)
            sAnno = "19" + sAnno;
        else
            sAnno = "20" + sAnno;

    if (sAnno > 2100 || sAnno < 1900) return "";

    return sAnno;
}
//Verifica che il valore passato sia una data valida
//strData : Data da Verificare , data in formato "GG/MM/AAAA hh:mm"
//sDataDA : Limite inferiore   , data in formato "GG/MM/AAAA hh:mm"
//sDataA  : Limite superiore   , data in formato "GG/MM/AAAA hh:mm"
function isDate(strData, sDataDA, sDataA) {
    var gg, mm, aaaa;
    var hh, mi;
    var dTestDate;
    var ArrDate;
    var dData1;
    var ArrHour;
    var sOra, sOraDa, sOraA;

    //separa data e ora
    ArrDate = strData.split(" ");
    strData = ArrDate[0];
    if (ArrDate.length == 2)
        sOra = ArrDate[1];
    else
        sOra = "";

    ArrDate = strData.split("/");
    if (ArrDate.length != 3) return false;
    gg = ArrDate[0];
    mm = ArrDate[1];
    aaaa = CompletaAnno(ArrDate[2]);

    if (gg == '') return false;
    if (mm == '') return false;
    if (aaaa == '') return false;

    if (sOra != "") {
        ArrHour = sOra.split(":");
        if (ArrHour.length != 2) return false;
        hh = ArrHour[0];
        mi = ArrHour[1];
    }
    else {
        hh = '00';
        mi = '00';
    }

    if (hh == '') return false;
    if (mi == '') return false;

    //verifica che il campo strData contenga una DATA valida
    dTestDate = new Date(aaaa, mm - 1, gg, hh, mi);
    if (dTestDate.getDate() != parseInt(gg * 1)) return false;
    if (dTestDate.getMonth() != parseInt(mm * 1) - 1) return false;
    if (dTestDate.getFullYear() != parseInt(aaaa * 1)) return false;
    if (dTestDate.getHours() != parseInt(hh * 1)) return false;
    if (dTestDate.getMinutes() != parseInt(mi * 1)) return false;

    //verifica range dei valori ammessi
    if (!isUndefined(sDataDA) || !isUndefined(sDataA)) {
        //Verifica limite inferiore
        if (!isUndefined(sDataA) && jTrim(sDataA.toString()) != '') {
            //converte sDataA in un oggetto DATA
            //separa data e ora
            ArrDate = sDataA.split(" ");
            sDataA = ArrDate[0];
            if (ArrDate.length == 2)
                sOraA = ArrDate[1];
            else
                sOraA = "";

            ArrDate = sDataA.split("/");
            if (sOraA != "") {
                ArrHour = sOraA.split(":");
                hh = ArrHour[0];
                mi = ArrHour[1];
            }
            else {
                hh = '00';
                mi = '00';
            }
            dData1 = new Date(ArrDate[2], ArrDate[1] - 1, ArrDate[0], hh, mi);

            if (dTestDate > dData1) return false;
        }

        //Verifica limite superiore
        if (!isUndefined(sDataDA) && jTrim(sDataDA.toString()) != '') {
            //converte sDataDA in un oggetto DATA
            //separa data e ora
            ArrDate = sDataDA.split(" ");
            sDataDA = ArrDate[0];
            if (ArrDate.length == 2)
                sOraDa = ArrDate[1];
            else
                sOraDa = "";

            ArrDate = sDataDA.split("/");
            if (sOraDa != "") {
                ArrHour = sOraDa.split(":");
                hh = ArrHour[0];
                mi = ArrHour[1];
            }
            else {
                hh = '00';
                mi = '00';
            }
            dData1 = new Date(ArrDate[2], ArrDate[1] - 1, ArrDate[0], hh, mi);

            if (dTestDate < dData1) return false;
        }
    }
    return true;
}
function isNumber(strField, intInteri, intDecimali, intMinVal, intMaxVal) {
    console.log('entro isnumber');
    var strInteri = '';
    var strDecimali = '';
    var vetNumber;
    var rePunti = /\./gi;
    var reVirg = /,/gi;

    //alert("strField=" + strField + "\nintInteri=" + intInteri + "\nintDecimali=" + intDecimali + "\nintMinVal=" + intMinVal + "\nintMaxVal=" + intMaxVal);

    // *** Verifica parametri ***
    if (isUndefined(strField)) return false;
    if (IsBlank(strField)) return true;//Numero vuoto
    if (isUndefined(intInteri)) intInteri = -1;
    if (intInteri.toString() == '') intInteri = -1;
    if (isUndefined(intDecimali)) intDecimali = -1;
    if (intDecimali.toString() == '') intDecimali = -1;
    console.log('parametri ok');

    //strField = strField.replace(reVirg,'.');
    //vetNumber = strField.split(".");
    vetNumber = strField.split(",");
    if (vetNumber.length > 2) {
        $('#OnlyNumber_error').text('Numero non valido, troppe virgole inserite.');
        $('#OnlyNumber_error').show(); //Show error
        $(strField).focus(); //Focus on field  
        //alert($('#OnlyNumber_error').text());
        return false;	//verifica che non ci siano 2 o più virgole
    }
    if (vetNumber.length == 2) {
        if (intDecimali == 0) {
            $('#OnlyNumber_error').text('Numero non valido, deve essere un intero.');
            $('#OnlyNumber_error').show(); //Show error
            $(strField).focus(); //Focus on field  
            //alert($('#OnlyNumber_error').text());
            return false;	//se non sono richiesti decimali non deve essere presente la virgola
        }

        strInteri = vetNumber[0];
        strDecimali = vetNumber[1];
        if (strDecimali.length == 0) {
            $('#OnlyNumber_error').text('Numero non valido, devono esserci dei decimali.');
            $('#OnlyNumber_error').show(); //Show error
            $(strField).focus(); //Focus on field  
            //alert($('#OnlyNumber_error').text());
            return false;		//deve esistere almeno una cifra dopo la virgola
        }
    }
    else
        strInteri = strField;

    strInteri = strInteri.replace(rePunti, "");	//elimina dalla parte intera eventuali punti '.'

    if (strInteri == '') return false;
    if (isNaN(strInteri)) return false;
    if (isNaN(strDecimali)) return false;

    //Verifica numero di cifre intere e decimali
    if (intInteri > 0)
        if (strInteri.length > intInteri) {
            $('#OnlyNumber_error').text('Numero non valido, deve essere lungo al massimo '+ intInteri +' caratteri.');
            $('#OnlyNumber_error').show(); //Show error
            $(strField).focus(); //Focus on field  
            //alert($('#OnlyNumber_error').text());
            return false;
        }

    if (intDecimali > 0)
        if (strDecimali.length > intDecimali) {
            $('#OnlyNumber_error').text('Numero non valido, deve avere al massimo '+ intDecimali +' decimali.');
            $('#OnlyNumber_error').show(); //Show error
            $(strField).focus(); //Focus on field  
            //alert($('#OnlyNumber_error').text());
            return false;
        }

    //Verifica il range dei valori ammessi
    if (!isUndefined(intMaxVal) || !isUndefined(intMinVal)) {
        var intNewNumber;
        var strNum;
        strNum = strInteri;
        if (strDecimali.length >= 0) strNum += '.' + strDecimali;
        intNewNumber = new Number(strNum);

        //Verifica limite superiore
        if (!isUndefined(intMaxVal) && intMaxVal.toString().trim() != '') {
            //converte intMaxVal nel formato inglese
            intMaxVal = intMaxVal.toString();
            intMaxVal = intMaxVal.replace(reVirg, '.');
            //alert('intMaxVal=' + intMaxVal + '\nintNewNumber=' +intNewNumber);
            if (intNewNumber > intMaxVal) {
                $('#OnlyNumber_error').text('Numero non valido, deve essere compreso fra '+intMinVal+' e '+intMaxVal+'.');
                $('#OnlyNumber_error').show(); //Show error
                $(strField).focus(); //Focus on field  
                //alert($('#OnlyNumber_error').text());
                return false;
            }
        }
        //Verifica limite inferiore
        if (!isUndefined(intMinVal) && intMinVal.toString().trim() != '') {
            //converte intMinVal nel formato inglese
            intMinVal = intMinVal.toString();
            intMinVal = intMinVal.replace(reVirg, '.');
            //alert('intMinVal=\'' + intMinVal + '\'\nintNewNumber=' +intNewNumber);
            if (intNewNumber < intMinVal) {
                $('#OnlyNumber_error').text('Numero non valido, deve essere compreso fra ' + intMinVal + ' e ' + intMaxVal + '.');
                $('#OnlyNumber_error').show(); //Show error
                $(strField).focus(); //Focus on field  
                //alert($('#OnlyNumber_error').text());
                return false;
            }
        }
    }
    console.log('isnumber');
    $('#OnlyNumber_error').text('');
    $('#OnlyNumber_error').hide(); //Show error
    return true;
}
function CodiceFiscale() {
    if (VerificaPerCodiceFiscale()) {
        //$('#MainContent_CmdInCodiceFiscale').click();
        console.log('ok');
        return true;
    }
    else {
        console.log('ko');
        return false;}
}
function DatiDaCodiceFiscale() {
    sMsg = ""

    if (IsBlank($('#MainContent_txtCodiceFiscale').val())) {
        sMsg = sMsg + "[Codice Fiscale]\n";
    }

    if (!IsBlank(sMsg)) {
        strMessage = "Attenzione...\n\n I campi elencati sono obbligatori per calcolare i dati dal Codice Fiscale!\n\n"
        alert(strMessage + sMsg);
        Setfocus($('#MainContent_txtCodiceFiscale'))
        return false;
    }
    else {
        //$('#MainContent_CmdFromCodiceFiscale').click();
        return true;
    }
}
function VerificaPerCodiceFiscale() {
    if (IsBlank($('#MainContent_txtCognome').val())) {
        $('#OnlyNumber_error').text('Inserisci il cognome');
        $('#OnlyNumber_error').show(); //Show error
        $('#MainContent_txtCognome').focus(); //Focus on field  
        //alert($('#OnlyNumber_error').text());
        return false;
    }
    if (IsBlank($('#MainContent_txtNome').val())) {
        $('#OnlyNumber_error').text('Inserisci il nome');
        $('#OnlyNumber_error').show(); //Show error
        $('#MainContent_txtNome').focus(); //Focus on field  
        //alert($('#OnlyNumber_error').text());
        return false;
    }
    if ($('#MainContent_ddlSesso').val() == '-1') {
        $('#OnlyNumber_error').text('Inserisci il sesso');
        $('#OnlyNumber_error').show(); //Show error
        $('#MainContent_ddlSesso').focus(); //Focus on field  
        //alert($('#OnlyNumber_error').text());
        return false;
    }
    if (IsBlank($('#MainContent_txtLuogoNascita').val())) {
        $('#OnlyNumber_error').text('Inserisci il luogo di nascita');
        $('#OnlyNumber_error').show(); //Show error
        $('#MainContent_txtLuogoNascita').focus(); //Focus on field  
        //alert($('#OnlyNumber_error').text());
        return false;
    }
    if (IsBlank($('#MainContent_txtPVNascita').val())) {
        $('#OnlyNumber_error').text('Inserisci la provincia di nascita');
        $('#OnlyNumber_error').show(); //Show error
        $('#MainContent_txtPVNascita').focus(); //Focus on field  
        //alert($('#OnlyNumber_error').text());
        return false;
    }
    if (IsBlank($('#MainContent_txtDataNascita').val())) {
        $('#OnlyNumber_error').text('Inserisci la data di nascita');
        $('#OnlyNumber_error').show(); //Show error
        $('#MainContent_txtDataNascita').focus(); //Focus on field  
        //alert($('#OnlyNumber_error').text());
        return false;
    }
    else {
        if (!isDate($('#MainContent_txtDataNascita').val())) {
            alert("Inserire la Data di Nascita correttamente in formato: GG/MM/AAAA");
            $('#OnlyNumber_error').text('Inserire la Data di Nascita correttamente in formato: GG/MM/AAAA');
            $('#OnlyNumber_error').show(); //Show error
            $('#MainContent_txtDataNascita').focus(); //Focus on field  
            //alert($('#OnlyNumber_error').text());
            return false;
        }
    }
    return true;
}
function Azzera() {
    $('.Azzera').val('');
    $('.Azzera').selectedIndex = 1;
}
function AzzeraIndInvio() {
    $('.AzzeraIndInvio').val('');
    $('.AzzeraIndInvio').selectedIndex = 1;
}
function ValidaMail(objCtrl) {
    console.log('devo validare mail');
    objCtrl.verimail({
        denyTempEmailDomains: true,
        messageElement: "p#MessageEmail"
    });
}
function VerificaDatiContratto() {
    sMsg = ""

    if ($('#MainContent_ddlTipoContatto').val() == '-1') {
        sMsg = sMsg + "[Titolo Soggetto]\n";
    }
    if (IsBlank($('#MainContent_txtDatiRiferimento').val())) {
        sMsg = sMsg + "[Dati del Riferimento]\n";
    }
    //*** 20140515 - invio mail ***
    if ($('#MainContent_chkInvioInformativeViaMail').checked) {
        if (IsBlank($('#MainContent_txtDataInizioInvio').val())) {
            sMsg = sMsg + "[Data inizio invio]\n";
            alert("Inserire la Data di Inizio invio!");
            Setfocus($('#MainContent_txtDataInizioInvio'));
            return false;
        }
        else {
            if (!isDate($('#MainContent_txtDataInizioInvio').val())) {
                alert("Inserire la Data di Inizio invio correttamente in formato: GG/MM/AAAA!");
                Setfocus($('#MainContent_txtDataInizioInvio'));
                return false;
            }
        }
    }
    else {
        if (!IsBlank($('#MainContent_txtDataInizioInvio').val())) {
            if (!isDate($('#MainContent_txtDataInizioInvio').val())) {
                alert("Inserire la Data di Inizio invio correttamente in formato: GG/MM/AAAA!");
                Setfocus($('#MainContent_txtDataInizioInvio'));
                return false;
            }
            else {
                $('#MainContent_chkInvioInformativeViaMail').checked = "true";
            }
        }
        else {
            //se non commentato la checkbox ritorna selezionata quando in questo caso
            //deve essere deselezionata
            //document.getElementById("chkInvioInformativeViaMail").checked="false";
        }
    }
    //*** ***
    if (!IsBlank(sMsg)) {
        strMessage = "Attenzione...\n\n I campi elencati sono obbligatori!\n\n"
        alert(strMessage + sMsg);
        Setfocus($('#MainContent_txtDatiRiferimento'))
        return false;
    }
    else {
        var valueRif = $('#MainContent_ddlTipoContatto').val();
        switch (valueRif) {
            case "1":
                if (!CheckPhoneNumber($('#MainContent_txtDatiRiferimento').val(),false)) {
                    strMessage = "Attenzione...\n\n il numero di Fax inserito non è valido!\n\n"
                    alert(strMessage);
                    Setfocus($('#MainContent_txtDatiRiferimento'));
                    $('#MainContent_txtDatiRiferimento').addClass('has-error');
                    return false;
                }
                break;
            case "2":
                if (!CheckPhoneNumber($('#MainContent_txtDatiRiferimento').val(),false)) {
                    strMessage = "Attenzione...\n\n il numero di Telefono Ufficio inserito non è valido!\n\n"
                    alert(strMessage);
                    Setfocus($('#MainContent_txtDatiRiferimento'));
                    $('#MainContent_txtDatiRiferimento').addClass('has-error');
                    return false;
                }
                break;
            case "3":

                if (!CheckPhoneNumber($('#MainContent_txtDatiRiferimento').val(),false)) {
                    strMessage = "Attenzione...\n\n il numero di Telefono Abitazione inserito non è valido!\n\n"
                    alert(strMessage);
                    Setfocus($('#MainContent_txtDatiRiferimento'));
                    $('#MainContent_txtDatiRiferimento').addClass('has-error');
                    return false;
                }
                break;
            case "4":
                console.log('controllo');
                console.log('...');
                $("#MainContent_txtDatiRiferimento").verimail({
                            denyTempEmailDomains: true,
                            messageElement: "p#MessageEmail"
                })
                if ($('span.error').html() != null) {
                    console.log($('span.error').html());
                    strMessage = "Attenzione...\n\n E-mail inserita non valida!\n\n"
                    alert(strMessage);
                    Setfocus($('#MainContent_txtDatiRiferimento'));
                    $('#MainContent_txtDatiRiferimento').addClass('has-error');
                    return false;
                } 
                /*if (!ValidaMail($('#MainContent_txtDatiRiferimento').val())) {
                    strMessage = "Attenzione...\n\n E-mail inserita non valida!\n\n"
                    alert(strMessage);
                    Setfocus($('#MainContent_txtDatiRiferimento'))
                    return false;
                }*/
                break;
            case "5":
                if (!CheckPhoneNumber($('#MainContent_txtDatiRiferimento').val(),true)) {
                    strMessage = "Attenzione...\n\n il numero di Cellulare  inserito non è valido!\n\n"
                    alert(strMessage);
                    Setfocus($('#MainContent_txtDatiRiferimento'));
                    $('#MainContent_txtDatiRiferimento').addClass('has-error');
                    return false;
                }
                break;
            default:
        }
    }
    return true;
}
function ModificaContatti(lb, desc, IDRIFERIMENTO, DataValiditaInvioMAIL) {
    $('#MainContent_hdIdContatto').val(unescape(IDRIFERIMENTO));
    $('#MainContent_ddlTipoContatto').val(unescape(lb));
    $('#MainContent_txtDatiRiferimento').val(unescape(desc));
    //*** 20140515 - invio mail ***
    if (unescape(DataValiditaInvioMAIL) != '')
        $('#MainContent_chkInvioInformativeViaMail').checked = true;
    else{
        $('#MainContent_chkInvioInformativeViaMail').checked = false;
        $('#MainContent_txtDataInizioInvio').val(unescape(DataValiditaInvioMAIL));
    }
}
function CheckPhoneNumber(TheNumber, IsCellPhone) {
    var valid = 1
    var GoodChars = "0123456789()-+ "
    var i = 0

    if (TheNumber == "") {
        // Return false if number is empty
        valid = 0;
    }
    for (i = 0; i <= TheNumber.length - 1; i++) {
        if (GoodChars.indexOf(TheNumber.charAt(i)) == -1) {
            // Note: Remove the comments from the following line to see this
            // for loop in action.
            // alert(TheNumber.charAt(i) + " is no good.")
            valid = 0;
        } // End if statement
    } // End for loop
    if (TheNumber.length < 6) {
        valid = 0;
    }
    if (IsCellPhone) {
        if (TheNumber.length < 10) {
            valid = 0;
        }
    }
    return valid;
}
function LockPertinenza() {
	console.log('sono LockPertinenza');
    if ($('#MainContent_ddlTipologia option:selected').text().indexOf('C/2')>0 ||$('#MainContent_ddlTipologia option:selected').text().indexOf('C/6')>0 ||$('#MainContent_ddlTipologia option:selected').text().indexOf('C/7')>0){
        $('#divPertinenza').show();
    }
    else {
        $('#divPertinenza').hide();
		if ($('#MainContent_ddlTipologia option:selected').text().indexOf('abbricabil') > 0 || $('#MainContent_ddlTipologia option:selected').text().indexOf('dificabil') > 0) //|| $('#MainContent_ddlTipologia option:selected').text().indexOf('erren') > 0) 
		{
			$('#divVincoliZone').show();
			$('#divVincoli').show();
			$('#MainContent_ddlZona').removeAttr('disabled');
        }
		else {
		    $('#MainContent_ddlZona').removeAttr('disabled');
            $('#divVincoliZone').hide();
        }
        
        //$('#divVincoli').hide();
        /*if ($('#MainContent_ddlTipologia option:selected').text().indexOf('abbricabil') > 0) {
            $('#lblRendita').text('Valore:');
            $('#lblConsistenza').text('MQ:');
            //$('#divVincoli').show();
        }
        else if ($('#MainContent_ddlTipologia option:selected').text().indexOf('erren') > 0) {
            $('#lblRendita').text('Reddito Dominicale:');
            $('#lblConsistenza').text('MQ:');
        }
        else {
            $('#lblRendita').text('Rendita:');
            $('#lblConsistenza').text('Consistenza:');
        }*/
    }
    if ($('#MainContent_ddlTipologia option:selected').text().indexOf('rincipale') > 0) {
        $('#lblNUtilizzatori').show();
        $('#MainContent_txtNUtilizzatori').show();
    }
    else {
        $('#lblNUtilizzatori').hide();
        $('#MainContent_txtNUtilizzatori').hide();
    }
}
function ForceCat() {
    if ($('#MainContent_ddlTipologia option:selected').text().indexOf('abbricabil') > 0 || $('#MainContent_ddlTipologia option:selected').text().indexOf('dificabil') > 0) //|| $('#MainContent_ddlTipologia option:selected').text().indexOf('erren') > 0) 
    {        
        $("#MainContent_ddlCat").find("option:contains('AF')").each(function () {
            $(this).attr("selected", "selected");           
            $('#MainContent_ddlZona').removeAttr('disabled');
            $("#MainContent_ddlCat").find("option:contains('TA')").removeAttr("selected");
        });        
    }    
    else if ($('#MainContent_ddlTipologia option:selected').text().indexOf('erren') > 0) 
    {        
        $("#MainContent_ddlCat").find("option:contains('TA')").each(function () {
            $(this).attr("selected", "selected");
            $("#MainContent_ddlCat").find("option:contains('AF')").removeAttr("selected");
        });
        
    }
}
function LoadQta() {
    var QTA = 0.00;
    $('#MainContent_GrdUI tr').each(function (i, row) {
        // reference all the stuff you need first
        var $row = $(row),
            $qta = $row.find('input[name*="txtQta"]');
        if (!isNaN($qta.val())) {
            QTA += parseFloat($qta.val().replace(',', '.'));
        }
    });
    $('#lblTotQta').text('Tot. Q.ta:' + QTA);
}
function LoadTipoAttoOSAP() {
    $('#lblDataAtto').text('Data '+$('#MainContent_ddlTipoAtto option:selected').text() + ':');
    $('#lblNumAtto').text('N.'+$('#MainContent_ddlTipoAtto option:selected').text() + ':');
}
function LoadTooltip() {
    $(".imgBaseModuliTributi").hover(
        function () {
            var descr = "Tributi"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".imgBaseModuliProfilo").hover(
        function () {
            var descr = "Profilo"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".imgBaseModuliIstanze").hover(
        function () {
            var descr = "Istanze"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".imgBaseModuliPaga").hover(
        function () {
            var descr = "Paga"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    //*** tooltip generici ***
    $(".BottoneAccept").hover(
        function () {
            var descr = "Valida"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneAlter").hover(
        function () {
            var descr = "Variazione"
            $(this).attr('title', descr);            
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneAttach").hover(
        function () {
            var descr = "Associa"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneAttachGrd").hover(
        function () {
            var descr = "Associa"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneAttention").hover(
        function () {
            var descr = "Inagibile"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneBack").hover(
        function () {
            var descr = "Uscita"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneCalculator").hover(
        function () {
            var descr = "Calcola"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneClose").hover(
        function () {
            var descr = "Cessazione"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneCounter").hover(
        function () {
            var descr = "Protocolla"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneDelete").hover(
        function () {
            var descr = "Elimina"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneDeleteGrd").hover(
        function () {
            var descr = "Elimina"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneDelegate").hover(
        function () {
            var descr = "Deleghe"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneF24").hover(
        function () {
            var descr = "Stampa F24"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneFinder").hover(
        function () {
            var descr = "Cerca da lista"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneFolderAdd").hover(
        function () {
            var descr = "Chiudi ed Inserisci"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneKey").hover(
    function () {
        var descr = "Cambia Password"
        $(this).attr('title', descr);
    }, function () { $(this).attr('title', ''); }
    ); 
    $(".BottoneInbox").hover(
        function () {
            var descr = "Conferma dichiarazione"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    ); 
    $(".BottoneMailBox").hover(
        function () {
            var descr = "Comunicazioni"
            if ($('#MainContent_hfFrom').val() == 'FO')
                descr = "Invia dichiarazione al comune";
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneMap").hover(
        function () {
            var descr = "Vai a Cartografia"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneMapMarker").hover(
        function () {
            var descr = "Cerca in stradario"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneMatchGrd").hover(
        function () {
            var descr = "Coincide con quanto presente tra gli immobili utilizzati per il calcolo del dovuto IMU"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneMissingGrd").hover(
        function () {
            var descr = "Non presente tra gli immobili utilizzati per il calcolo del dovuto IMU"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneNew").hover(
        function () {
            var descr = "Nuovo"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneNewGrd").hover(
        function () {
            var descr = "Nuovo"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneNoMatchGrd").hover(
        function () {
            var descr = "Diverso da quanto presente nei dati utilizzati per il calcolo del dovuto IMU"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneObject").hover(
        function () {
            var descr = "Scegli Tributi"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneObjectGrd").hover(
        function () {
            var descr = "Scegli Tributi"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneOpen").hover(
        function () {
            var descr = "Apri"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottonePrint").hover(
        function () {
            var descr = "Stampa"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneRecycle").hover(
        function () {
            var descr = "Ribalta"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneRecycleCF").hover(
        function () {
            var descr = "Determina luogo e data nascita da codice fiscale";
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneSave").hover(
        function () {
            var descr = "Salva"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneSearch").hover(
        function () {
            var descr = "Ricerca"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneShare").hover(
        function () {
            var descr = "Comproprietari"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneStop").hover(
        function () {
            var descr = "Respingi"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneTesseraSanitaria").hover(
        function () {
            var descr = "Calcolo codice fiscale"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneTown").hover(
        function () {
            var descr = "Scegli Comune"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneTownGrd").hover(
        function () {
            var descr = "Scegli Comune"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneUpdate").hover(
        function () {
            var descr = "Correggi"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneUserGroup").hover(
        function () {
            var descr = "Uso Gratuito"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneWarning").hover(
        function () {
            var descr = "Inutilizzato"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneWork").hover(
        function () {
            var descr = "Presa in carico"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneAddGrd").hover(
        function () {
            var descr = "Aggiungi"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottonDelGrd").hover(
        function () {
            var descr = "Rimuovi"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $(".BottoneExportXLS").hover(
        function () {
            var descr = "Estrai elenco"
            $(this).attr('title', descr);
        }, function () { $(this).attr('title', ''); }
    );
    $("p#cessazione").html("Cessazione");
    $("p#close").html("Chiudi");
    $("p#closeadd").html("Chiudi+Ins.");
    $("p#apri").html("Apri");
    $("p#nuovo").html("Nuovo");
    $("p#NuovaUI").html("Nuovo Imm.");
    $("p#NuovoMsg").html("Nuova Comunicaz.");
    $("p#variazione").html("Variazione");
    $("p#correggi").html("Correggi");
    $("p#inagibile").html("Inagibile");
    $("p#inutilizzato").html("Inutilizzato");
    $("p#usogratuito").html("Comodato Gratuito");
    $("p#F24").html(" Stampa F24");
    if ($('#MainContent_hfFrom').val() == 'FO')
        $("p#Comunicazione").html("Comunicazione");
    else
        $("p#Comunicazione").html("Integrazione");
    $("p#Protocolla").html("Protocolla");
    $("p#InCarico").html("In Carico");
    $("p#Valida").html("Valida");
    $("p#Respingi").html("Respingi");
    $("p#download").html("Scarica");
    $("p#PrintDich").html("Conferma dichiarazione ");
    $("p#SendDich").html("Invia dichiarazione");
    $("p#Elimina").html("Cancella");
    $("p#Salva").html("Salva");
    $("p#Pulisci").html("Pulisci");
    $("p#Ribalta").html("Aggiorna Verticale");
    $("p#folderadd").html("Chiudi ed Inserisci");
    $("p#Copia").html("Copia");
    $("p#Stampa").html("Stampa");
    $("p#Allega").html("Allega");
    $("p#Ravvedi").html("Ravvedimento Operoso");
    $("p#Search").html("Cerca");
    $("p#InsCat").html("Inserisci");
}
function ChangeOptAnalisi() {
    $('#MainContent_GrdSynthetic').hide();
    $('#MainContent_GrdSyntheticNoTrib').hide();
    $('#MainContent_GrdAnalytic').hide();
    $('#MainContent_GrdCompare').hide();
    $('#MainContent_GrdStati').hide();
    $('#MainContent_GrdTempi').hide();
    $('#chart_div').hide();
    if ($('#MainContent_optSintetica').is(':checked') || $('#MainContent_optAnalitica').is(':checked')) {
        $('.ParamRaffronto').hide();

    } else {
        $('.ParamRaffronto').show();
    }
    $('#Synthetic').hide();
    $('#Analytic').hide();
    Azzera();
}
function FieldValidatorRegister() {
    $('#OnlyNumber_error').text('');
    $('#OnlyNumber_error').hide();

    if (!$('#MainContent_chkAccept').is(':checked')) {
        $('#OnlyNumber_error').text('E\' obbligatorio accettare i termini e le condizioni d\'utilizzo.');
        $('#OnlyNumber_error').show(); //Show error
        return false;
    }
    if ($('#MainContent_Email').val().length <=0) {
        $('#OnlyNumber_error').text('Inserisci l\'indirizzo di Posta elettronica');
        $('#OnlyNumber_error').show(); //Show error
        $('#MainContent_Email').focus(); //Focus on field 
    } else {
        var myRet = false;
        if ($('#MainContent_TextBoxCodiceFiscale').val().length > 11) {
            myRet = ControllaCF($('#MainContent_TextBoxCodiceFiscale').val());
            console.log('controllocf'+myRet);
            return myRet;
        } else {
            console.log('controllo piva');
            myRet = ControllaPIVA($('#MainContent_TextBoxCodiceFiscale').val());
            return myRet;
        }
    }
}
function FieldValidatorAnag() {
    $('#OnlyNumber_error').text('');
    $('#OnlyNumber_error').hide();

    if (!$('#divDatiIstanza').is(':visible')) {
        if ($('#MainContent_txtCodiceFiscale').val() == '' && $('#MainContent_txtPartitaIva').val() == '' && !$('#divDatiIstanza').is(':visible')) {
            if ($('#MainContent_txtCodiceFiscale').is(':disabled')) {
                $('#MainContent_txtCodiceFiscale').removeAttr('disabled');
            }
            $('#OnlyNumber_error').text('Inserisci il Codice Fiscale o la Partita IVA');
            $('#OnlyNumber_error').show(); //Show error
            $('#MainContent_txtCodiceFiscale').focus(); //Focus on field 
            //alert($('#OnlyNumber_error').text());
            return false;
        }
        else {
            if (!$('#divDatiIstanza').is(':visible')) {
                if (!ControllaCF($('#MainContent_txtCodiceFiscale').val())) {
                    if ($('#MainContent_txtCodiceFiscale').is(':disabled')) {
                        $('#MainContent_txtCodiceFiscale').removeAttr('disabled');
                    }
                    $('#OnlyNumber_error').text('Codice Fiscale non corretto');
                    $('#OnlyNumber_error').show(); //Show error
                    $('#MainContent_txtCodiceFiscale').focus(); //Focus on field 
                    //alert($('#OnlyNumber_error').text());
                    return false;
                }
            }
        }
        if ($('#MainContent_txtCognome').val() == '' && !$('#divDatiIstanza').is(':visible')) {
            if ($('#MainContent_txtCognome').is(':disabled')) {
                $('#MainContent_txtCognome').removeAttr('disabled');
            }
            $('#OnlyNumber_error').text('Inserisci il Cognome/Ragione Sociale');
            $('#OnlyNumber_error').show(); //Show error
            $('#MainContent_txtCognome').focus(); //Focus on field 
            //alert($('#OnlyNumber_error').text());
            return false;
        }
        if ($('#MainContent_txtCodiceFiscale').val() != '' && $('#MainContent_txtNome').val() == '' && !$('#divDatiIstanza').is(':visible')) {
            if ($('#MainContent_txtNome').is(':disabled')) {
                $('#MainContent_txtNome').removeAttr('disabled');
            }
            $('#OnlyNumber_error').text('Inserisci il Nome');
            $('#OnlyNumber_error').show(); //Show error
            $('#MainContent_txtNome').focus(); //Focus on field 
            //alert($('#OnlyNumber_error').text());
            return false;
        }
    }
}
function FieldValidatorComunicazioni() {
    $('#lblErrorBO').text('');
    $('#lblErrorBO').hide();

    if ($('#MainContent_ddlEnteDest').val() == '') {
        if ($('#MainContent_ddlEnteDest').is(':disabled')) {
            $('#MainContent_ddlEnteDest').removeAttr('disabled');
        }
        $('#lblErrorBO').text('Inserisci l\'Ente');
        $('#lblErrorBO').show(); //Show error
        $('#MainContent_ddlEnteDest').focus(); //Focus on field 
        return false;
    }
    if ($('#MainContent_txtMessage').val() == '') {
        if ($('#MainContent_txtMessage').is(':disabled')) {
            $('#MainContent_txtMessage').removeAttr('disabled');
        }
        $('#OnlyNumber_error').text('Inserisci il Cognome/Ragione Sociale');
        $('#OnlyNumber_error').show(); //Show error
        $('#MainContent_txtMessage').focus(); //Focus on field 
        return false;
    }
    return true;
}
function FieldValidatorUniversali() {
    $('#lblErrorFO').text('');
    $('#lblErrorFO').hide();

    if ($('#MainContent_txtVia').val() == '') {
        if ($('#MainContent_txtVia').is(':disabled')) {
            $('#MainContent_txtVia').removeAttr('disabled');
        }
        $('#lblErrorFO').text('Inserisci la via');
        $('#lblErrorFO').show(); //Show error
        $('.BottoneMapMarker').show(); //Show error
        $('#MainContent_txtVia').focus(); //Focus on field  
        //alert($('#OnlyNumber_error').text());
        return false;
    }
    /*if ($('#MainContent_txtCivico').val() == '') {
        if ($('#MainContent_txtCivico').is(':disabled')) {
            $('#MainContent_txtCivico').removeAttr('disabled');
        }
        $('#OnlyNumber_error').text('Inserisci il civico');
        $('#OnlyNumber_error').show(); //Show error
        $('#MainContent_txtCivico').focus(); //Focus on field  
        //alert($('#OnlyNumber_error').text());
        return false;
    }*/
    if ($('#MainContent_txtDataInizio').val() == '') {
        if ($('#MainContent_txtDataInizio').is(':disabled')) {
            $('#MainContent_txtDataInizio').removeAttr('disabled');
        }
        $('#lblErrorFO').text('Inserisci la data inizio');
        $('#lblErrorFO').show(); //Show error
        $('#MainContent_txtDataInizio').focus(); //Focus on field  
        $('#MainContent_lblDataInizioORG').show();
        //alert($('#OnlyNumber_error').text());
        return false;
    }
    else {
        $('#MainContent_lblDataInizioORG').hide();
        var startDt=$('#MainContent_txtDataInizio').val() ;
        var endDt = $('#MainContent_lblDataInizioORG').text();
        console.log('da->' + new Date(startDt).getTime());
        console.log('a->' + new Date(endDt).getTime());
        /*var month = startDt.getUTCMonth() + 1; //months from 1-12
        var day = startDt.getUTCDate();
        var year = startDt.getUTCFullYear();*/

        /*if (new Date(startDt).getTime() < new Date(endDt).getTime()) {
            if ($('#MainContent_txtDataInizio').is(':disabled')) {
                $('#MainContent_txtDataInizio').removeAttr('disabled');
            }
            $('#OnlyNumber_error').text('La data inizio deve essere successiva a quella iniziale');
            $('#OnlyNumber_error').show(); //Show error
            $('#MainContent_txtDataInizio').focus(); //Focus on field  
            //alert($('#OnlyNumber_error').text());
            return false;
        }
        console.log('oggi->' + new Date().getTime());
        console.log('confronto con->' + new Date(startDt).getTime());
        if (new Date(startDt).getTime() > new Date().getTime()) {
            if ($('#MainContent_txtDataInizio').is(':disabled')) {
                $('#MainContent_txtDataInizio').removeAttr('disabled');
            }
            $('#OnlyNumber_error').text('La data inizio non può essere successiva a quella odierna');
            $('#OnlyNumber_error').show(); //Show error
            $('#MainContent_txtDataInizio').focus(); //Focus on field  
            //alert($('#OnlyNumber_error').text());
            return false;
        }*/
    }
    if ($('#MainContent_txtDataFine').val() != '') {
        var startDt = $('#MainContent_txtDataInizio').val();
        var endDt = $('#MainContent_txtDataFine').val();
        console.log('da->' + new Date(startDt).getTime());
        console.log('a->' + new Date(endDt).getTime());
        /*if (new Date(endDt).getTime() < new Date(startDt).getTime()) {
            if ($('#MainContent_txtDataFine').is(':disabled')) {
                $('#MainContent_txtDataFine').removeAttr('disabled');
            }
            $('#OnlyNumber_error').text('La data fine deve essere successiva a quella di inizio');
            $('#OnlyNumber_error').show(); //Show error
            $('#MainContent_txtDataFine').focus(); //Focus on field  
            //alert($('#OnlyNumber_error').text());
            return false;
        }*/
    }
    if ($('#MainContent_txtPercRid').val() != '' && $('#MainContent_txtPercEse').val() != '') {
        if (parseFloat($('#MainContent_txtPercRid').val().replace(',', '.')) > 0 && parseFloat($('#MainContent_txtPercEse').val().replace(',', '.')) > 0) {
            if ($('#MainContent_txtPercRid').is(':disabled')) {
                $('#MainContent_txtPercRid').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Non possono essere valorizzate sia riduzione sia esenzione');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_txtPercRid').focus(); //Focus on field  
            //alert($('#OnlyNumber_error').text());
            return false;
        }
    }
    return true;
}
function FieldValidatorICI() {
    console.log('FieldValidatorICI()');
    $('#lblErrorFO').text('');
    $('#lblErrorFO').hide();

    if (!FieldValidatorUniversali())
        return false;
    else {
        if ($('#MainContent_txtFoglio').val() == '') {
            if ($('#MainContent_txtFoglio').is(':disabled')) {
                $('#MainContent_txtFoglio').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci il Foglio');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_txtFoglio').focus(); //Focus on field 
            //alert($('#lblErrorFO').text());
            return false;
        }
        if ($('#MainContent_txtNumero').val() == '') {
            if ($('#MainContent_txtNumero').is(':disabled')) {
                $('#MainContent_txtNumero').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci il Numero');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_txtNumero').focus(); //Focus on field 
            //alert($('#lblErrorFO').text());
            return false;
        }
        if ($('#MainContent_ddlTipologia').val() == '-1') {
            if ($('#MainContent_ddlTipologia').is(':disabled')) {
                $('#MainContent_ddlTipologia').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci l\'utilizzo');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_ddlTipologia').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
        if ($('#MainContent_ddlCat').val() == '-1' && $('#MainContent_ddlTipologia option:selected').text().indexOf('erren') <= 0 && ($('#MainContent_ddlTipologia option:selected').text().indexOf('abbricabil') <= 0 && $('#MainContent_ddlTipologia option:selected').text().indexOf('dificabil') <= 0)) {
            if ($('#MainContent_ddlCat').is(':disabled')) {
                $('#MainContent_ddlCat').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci la categoria');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_ddlCat').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
        if ($('#MainContent_ddlTipologia option:selected').text().indexOf('erren') <= 0) {
            if ($('#MainContent_ddlClasse').val() == '' && (!$('#MainContent_ddlCat option:selected').text().match("^F") && (($('#MainContent_ddlTipologia option:selected').text().indexOf('abbricabil') <= 0 && $('#MainContent_ddlTipologia option:selected').text().indexOf('dificabil') <= 0) && $('#MainContent_ddlTipologia option:selected').text().indexOf('erren') <=0))) {
                if ($('#MainContent_ddlClasse').is(':disabled')) {
                    $('#MainContent_ddlClasse').removeAttr('disabled');
                }


                $('#lblErrorFO').text('Inserisci la classe');
                $('#lblErrorFO').show(); //Show error
                $('#MainContent_ddlClasse').focus(); //Focus on field  
                //alert($('#lblErrorFO').text());
                return false;
            }
        }
        if ($('#MainContent_ddlTipologia option:selected').text().indexOf('abbricabil') > 0 || $('#MainContent_ddlTipologia option:selected').text().indexOf('dificabil') > 0) {
            if ($('#MainContent_txtConsistenza').val() == '') {
                if ($('#MainContent_txtConsistenza').is(':disabled')) {
                    $('#MainContent_txtConsistenza').removeAttr('disabled');
                }
                $('#lblErrorFO').text('Inserisci la consistenza');
                $('#lblErrorFO').show(); //Show error
                $('#MainContent_txtConsistenza').focus(); //Focus on field  
                //alert($('#lblErrorFO').text());
                return false;
            }
        }
        if ($('#MainContent_txtRendita').val() == '' && (!$('#MainContent_ddlCat option:selected').text().match("^F"))) {
            if ($('#MainContent_txtRendita').is(':disabled')) {
                $('#MainContent_txtRendita').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci la rendita');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_txtRendita').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
        else if ((parseFloat($('#MainContent_txtRendita').val().replace(',', '.')) <= 0) && (!$('#MainContent_ddlCat option:selected').text().match("^F"))) {
            //console.log('rendita->' + parseFloat($('#MainContent_txtRendita').val().replace(',', '.')));
            if ($('#MainContent_txtRendita').is(':disabled')) {
                $('#MainContent_txtRendita').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci la rendita');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_txtRendita').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
        if ($('#MainContent_ddlPossesso').val() == '-1') {
            if ($('#MainContent_ddlPossesso').is(':disabled')) {
                $('#MainContent_ddlPossesso').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci il possesso');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_ddlPossesso').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
        if ($('#MainContent_txtPercPos').val() == '') {
            if ($('#MainContent_txtPercPos').is(':disabled')) {
                $('#MainContent_txtPercPos').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci la % di possesso');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_txtPercPos').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
        if ($('#MainContent_ddlTipologia option:selected').text().indexOf('rincipale') > 0) {
            if ($('#MainContent_txtNUtilizzatori').val() == '') {
                if ($('#MainContent_txtNUtilizzatori').is(':disabled')) {
                    $('#MainContent_txtNUtilizzatori').removeAttr('disabled');
                }
                $('#lblErrorFO').text('Inserisci il numero utilizzatori');
                $('#lblErrorFO').show(); //Show error
                $('#MainContent_txtNUtilizzatori').focus(); //Focus on field  
                //alert($('#lblErrorFO').text());
                return false;
            }
            else {
                var chr = $('#MainContent_txtNUtilizzatori').val();
                if (chr <= 0) {
                    if ($('#MainContent_txtNUtilizzatori').is(':disabled')) {
                        $('#MainContent_txtNUtilizzatori').removeAttr('disabled');
                    }
                    $('#lblErrorFO').text('Inserisci il numero utilizzatori');
                    $('#lblErrorFO').show(); //Show error
                    $('#MainContent_txtNUtilizzatori').focus(); //Focus on field  
                    //alert($('#lblErrorFO').text());
                    return false;
                }
            }
        }
        if($('*:contains("Istanza di Inagibilità")').length>0)
        {
            console.log('eccolo');
            if ($('#MainContent_txtPercRid').val() == '') {
                if ($('#MainContent_txtPercRid').is(':disabled')) {
                    $('#MainContent_txtPercRid').removeAttr('disabled');
                }
                $('#lblErrorFO').text('Inserisci la riduzione');
                $('#lblErrorFO').show(); //Show error
                $('#MainContent_txtPercRid').focus(); //Focus on field  
                //alert($('#lblErrorFO').text());
                return false;
            }
            else {
                var chr = $('#MainContent_txtPercRid').val();
                if (chr <= 0) {
                    if ($('#MainContent_txtPercRid').is(':disabled')) {
                        $('#MainContent_txtPercRid').removeAttr('disabled');
                    }
                    $('#lblErrorFO').text('Inserisci la riduzione');
                    $('#lblErrorFO').show(); //Show error
                    $('#MainContent_txtPercRid').focus(); //Focus on field  
                    //alert($('#lblErrorFO').text());
                    return false;
                }
            }
        } 
    }
}
function FieldValidatorTARSU() {
    $('#lblErrorFO').text('');
    $('#lblErrorFO').hide();

    if (!FieldValidatorUniversali())
        return false;
    else {
        if ($('#MainContent_txtFoglio').val() == '') {
            if ($('#MainContent_txtFoglio').is(':disabled')) {
                $('#MainContent_txtFoglio').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci il Foglio');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_txtFoglio').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
        if ($('#MainContent_txtNumero').val() == '') {
            if ($('#MainContent_txtNumero').is(':disabled')) {
                $('#MainContent_txtNumero').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci il Numero');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_txtNumero').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
        if ($('#MainContent_ddlRid').val() != '-1' && $('#MainContent_ddlEse').val() != '-1') {
            if ($('#MainContent_ddlRid').is(':disabled')) {
                $('#MainContent_ddlRid').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Impossibile inserire contemporaneamente riduzione ed esenzione');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_ddlRid').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }

    }
}
function FieldValidatorOSAP() {
    $('#lblErrorFO').text('');
    $('#lblErrorFO').hide();

    if (!FieldValidatorUniversali())
        return false;
    else {
        if ($('#MainContent_ddlTipoAtto').val() == '-1') {
            if ($('#MainContent_ddlTipoAtto').is(':disabled')) {
                $('#MainContent_ddlTipoAtto').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci il tipo Atto');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_ddlTipoAtto').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
        /*if ($('#MainContent_txtDataAtto').val() == '') {
            if ($('#MainContent_txtDataAtto').is(':disabled')) {
                $('#MainContent_txtDataAtto').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci la data Atto');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_txtDataAtto').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
        if ($('#MainContent_txtNumAtto').val() == '') {
            if ($('#MainContent_txtNumAtto').is(':disabled')) {
                $('#MainContent_txtNumAtto').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci il numero Atto');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_txtNumAtto').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }*/
        if ($('#MainContent_ddlRichiedente').val() == '-1') {
            if ($('#MainContent_ddlRichiedente').is(':disabled')) {
                $('#MainContent_ddlRichiedente').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci il Richiedente');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_ddlRichiedente').focus(); //Focus on field 
            //alert($('#lblErrorFO').text());
            return false;
        }
        if ($('#MainContent_ddlTributo').val() == '-1') {
            if ($('#MainContent_ddlTributo').is(':disabled')) {
                $('#MainContent_ddlTributo').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci il Tributo');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_ddlTributo').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
        if ($('#MainContent_ddlTipoDurata').val() == '-1') {
            if ($('#MainContent_ddlTipoDurata').is(':disabled')) {
                $('#MainContent_ddlTipoDurata').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci il tipo Durata');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_ddlTipoDurata').focus(); //Focus on field 
            //alert($('#lblErrorFO').text());
            return false;
        }
        if ($('#MainContent_txtDurata').val() == '') {
            if ($('#MainContent_txtDurata').is(':disabled')) {
                $('#MainContent_txtDurata').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci la Durata');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_txtDurata').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
        /*if ($('#MainContent_ddlCat').val() == '-1') {
            if ($('#MainContent_ddlCat').is(':disabled')) {
                $('#MainContent_ddlCat').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci la Categoria');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_ddlCat').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }*/
        if ($('#MainContent_ddlOccupazione').val() == '-1') {
            if ($('#MainContent_ddlOccupazione').is(':disabled')) {
                $('#MainContent_ddlOccupazione').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci l\' occupazione');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_ddlOccupazione').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
        if ($('#MainContent_ddlMisuraCons').val() == '-1') {
            if ($('#MainContent_ddlMisuraCons').is(':disabled')) {
                $('#MainContent_ddlMisuraCons').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci la Misura Consistenza');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_ddlMisuraCons').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
        if ($('#MainContent_txtCons').val() == '') {
            if ($('#MainContent_txtCons').is(':disabled')) {
                $('#MainContent_txtCons').removeAttr('disabled');
            }
            $('#lblErrorFO').text('Inserisci la Consistenza');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_txtCons').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
    }
}
function FieldValidatorICP() {
    $('#lblErrorFO').text('');
    $('#lblErrorFO').hide();

    if ($('#MainContent_txtDataAtto').val() == '') {
        if ($('#MainContent_txtDataAtto').is(':disabled')) {
            $('#MainContent_txtDataAtto').removeAttr('disabled');
        }
        $('#lblErrorFO').text('Inserisci la data Atto');
        $('#lblErrorFO').show(); //Show error
        $('#MainContent_txtDataAtto').focus(); //Focus on field  
        //alert($('#lblErrorFO').text());
        return false;
    }
    if ($('#MainContent_txtNumAtto').val() == '') {
        if ($('#MainContent_txtNumAtto').is(':disabled')) {
            $('#MainContent_txtNumAtto').removeAttr('disabled');
        }
        $('#lblErrorFO').text('Inserisci il numero Atto');
        $('#lblErrorFO').show(); //Show error
        $('#MainContent_txtNumAtto').focus(); //Focus on field  
        //alert($('#lblErrorFO').text());
        return false;
    }
}
function FieldValidatorGestIstanze() {
    $('#OnlyNumber_error').text('');
    $('#OnlyNumber_error').hide();

    if ($('#MainContent_txtNote').val() == '') {
        $('#mypage_error').text('Inserisci le note');
        $('#mypage_error').show(); //Show error
        $('#MainContent_txtNote').focus(); //Focus on field  
        alert($('#mypage_error').text());
        return false;
    }
}
function FieldValidatorRavvedimento(){
    $('#lblErrorFO').text('');
    $('#lblErrorFO').hide();

    if ($('#MainContent_txtAnno').val() == '') {
        $('#lblErrorFO').text('Inserisci l\'anno');
        $('#lblErrorFO').show(); //Show error
        $('#MainContent_txtAnno').focus(); //Focus on field  
        //alert($('#lblErrorFO').text());
        return false;
    }
    else {
        if ($('#MainContent_txtAnno').val()<2013) {
            $('#lblErrorFO').text('Il Ravvedimento può essere fatto dal 2013 in poi!');
            $('#lblErrorFO').show(); //Show error
            $('#MainContent_txtAnno').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
    }
    return true;
}
function FieldValidatorRicBOIstanze() {
    $('#OnlyNumber_error').text('');
    $('#OnlyNumber_error').hide();

    if ($('#MainContent_ddlEnte').val() == '') {
        if ($('#MainContent_ddlEnte').is(':disabled')) {
            $('#MainContent_ddlEnte').removeAttr('disabled');
        }
        $('#OnlyNumber_error').text('Inserisci l\'ente');
        $('#OnlyNumber_error').show(); //Show error
        $('#MainContent_ddlEnte').focus(); //Focus on field  
        //alert($('#OnlyNumber_error').text());
        return false;
    }
    return true;
}
function FieldValidatorRicTempiPagamento() {
    $('#lblErrorBO').text('');
    $('#lblErrorBO').hide();

    if ($('#MainContent_ddlEnte').val() == '') {
        $('#lblErrorBO').text('Inserisci l\'ente');
        $('#lblErrorBO').show(); //Show error
        $('#MainContent_ddlEnte').focus(); //Focus on field  
        //alert($('#OnlyNumber_error').text());
        return false;
    }
    if ($('#MainContent_txtAnno').val() == '') {
        $('#lblErrorBO').text('Inserisci l\'anno');
        $('#lblErrorBO').show(); //Show error
        $('#MainContent_txtAnno').focus(); //Focus on field  
        //alert($('#OnlyNumber_error').text());
        return false;
    }
    if ($('#MainContent_ddlTributo').val() == '') {
        $('#lblErrorBO').text('Inserisci il Tributo');
        $('#lblErrorBO').show(); //Show error
        $('#MainContent_ddlTributo').focus(); //Focus on field  
        //alert($('#OnlyNumber_error').text());
        return false;
    }
    if ($('#MainContent_ddlScadenza').val() == '') {
        $('#lblErrorBO').text('Inserisci la Scadenza');
        $('#lblErrorBO').show(); //Show error
        $('#MainContent_ddlScadenza').focus(); //Focus on field  
        //alert($('#OnlyNumber_error').text());
        return false;
    }
    return true;
}
function capLock(e) {
    kc = e.keyCode ? e.keyCode : e.which;
    sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
    if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk))
        document.getElementById('divMayus').style.visibility = 'visible';
    else
        document.getElementById('divMayus').style.visibility = 'hidden';
}
function ManageBottoniera() {
    if ($('*:contains(" - Istanza di ")').length > 0) {
        console.log('eccolo');
        $('.lead_header').removeClass('col-md-2');
        $('.lead_header').addClass('col-md-5');
        $('.BottoneDiv').hide();
    }
    else {
        $('.lead_header').addClass('col-md-2');
    }
}
function blinker() {
    $('.blink_me').fadeOut(1000, "swing");
    $('.blink_me').fadeIn(1000, "swing");
    $('.blink_slow').fadeOut("slow","swing");
    $('.blink_slow').fadeIn("slow", "swing");    
}
setInterval(blinker, 2000);

function removeBlink() {  
    $('.blink_slow');
}
function LoadNews() {
    $("div#TheDialogNews").dialog();
};
function ConfirmProtocollo(){
    if ($('#MainContent_hfTypeProtocollo').val() == "E")
        return confirm('Ti e\' arrivata la mail dal protocollo?');
    else
        return true;
}
function ConfirmAnagrafica() {
    /*if ($('#MainContent_hfFrom').val() == "FO")
        return confirm('Si desidera annullare l\'operazione di Inserimento o Modifica?\nI dati non salvati andranno persi!');
    else*/
        return true;
}
function PrecompileInquilinoVSProprietario(idControllo) {
    var percInquilino = 100.00 - parseFloat($('#' + idControllo).closest('tr').find('.PercProprietario').val().replace(',', '.'));
    $('#' + idControllo).closest('tr').find('.PercInquilino').val(percInquilino);
}
function PrecompileProprietarioVSInquilino(idControllo) {
    var percProprietario = 100.00 - parseFloat($('#' + idControllo).closest('tr').find('.PercInquilino').val().replace(',', '.'));
    $('#' + idControllo).closest('tr').find('.PercProprietario').val(percProprietario);
}
function FieldValidatorTASI() {
    $('#lblErrorFO').text('');
    $('#lblErrorFO').hide();
    if (($('[id*="_txtValore_"]').length <= 0)) {
        console.log('controllo');
        if (!FieldValidatorUniversali())
            return false;
        else {
            if ($('#MainContent_txtFoglio').val() == '') {
                if ($('#MainContent_txtFoglio').is(':disabled')) {
                    $('#MainContent_txtFoglio').removeAttr('disabled');
                }
                $('#lblErrorFO').text('Inserisci il Foglio');
                $('#lblErrorFO').show(); //Show error
                $('#MainContent_txtFoglio').focus(); //Focus on field 
                //alert($('#lblErrorFO').text());
                return false;
            }
            if ($('#MainContent_txtNumero').val() == '') {
                if ($('#MainContent_txtNumero').is(':disabled')) {
                    $('#MainContent_txtNumero').removeAttr('disabled');
                }
                $('#lblErrorFO').text('Inserisci il Numero');
                $('#lblErrorFO').show(); //Show error
                $('#MainContent_txtNumero').focus(); //Focus on field 
                //alert($('#lblErrorFO').text());
                return false;
            }
            console.log('utilizzo=' + $('#MainContent_ddlTipologia').val());
            if ($('#MainContent_ddlTipologia').val() == '-1') {
                if ($('#MainContent_ddlTipologia').is(':disabled')) {
                    $('#MainContent_ddlTipologia').removeAttr('disabled');
                }
                $('#lblErrorFO').text('Inserisci l\'utilizzo');
                $('#lblErrorFO').show(); //Show error
                $('#MainContent_ddlTipologia').focus(); //Focus on field  
                //alert($('#lblErrorFO').text());
                return false;
            }
            if ($('#MainContent_ddlCat').val() == '-1') {
                if ($('#MainContent_ddlCat').is(':disabled')) {
                    $('#MainContent_ddlCat').removeAttr('disabled');
                }
                $('#lblErrorFO').text('Inserisci la categoria');
                $('#lblErrorFO').show(); //Show error
                $('#MainContent_ddlCat').focus(); //Focus on field  
                //alert($('#lblErrorFO').text());
                return false;
            }
            if ($('#MainContent_txtRendita').val() == '') {
                if ($('#MainContent_txtRendita').is(':disabled')) {
                    $('#MainContent_txtRendita').removeAttr('disabled');
                }
                $('#lblErrorFO').text('Inserisci la rendita');
                $('#lblErrorFO').show(); //Show error
                $('#MainContent_txtRendita').focus(); //Focus on field  
                //alert($('#lblErrorFO').text());
                return false;
            }
            else if (parseFloat($('#MainContent_txtRendita').val().replace(',', '.')) <= 0) {
                console.log('rendita->' + parseFloat($('#MainContent_txtRendita').val().replace(',', '.')));
                if ($('#MainContent_txtRendita').is(':disabled')) {
                    $('#MainContent_txtRendita').removeAttr('disabled');
                }
                $('#lblErrorFO').text('Inserisci la rendita');
                $('#lblErrorFO').show(); //Show error
                $('#MainContent_txtRendita').focus(); //Focus on field  
                //alert($('#lblErrorFO').text());
                return false;
            }
            console.log('perc=' + $('#MainContent_txtPercPos').val());
            if ($('#MainContent_txtPercPos').val() == '') {
                if ($('#MainContent_txtPercPos').is(':disabled')) {
                    $('#MainContent_txtPercPos').removeAttr('disabled');
                }
                $('#lblErrorFO').text('Inserisci la % di possesso');
                $('#lblErrorFO').show(); //Show error
                $('#MainContent_txtPercPos').focus(); //Focus on field  
                //alert($('#lblErrorFO').text());
                return false;
            }
            if ($('#MainContent_ddlNaturaTitolo').val() == '-1') {
                if ($('#MainContent_ddlNaturaTitolo').is(':disabled')) {
                    $('#MainContent_ddlNaturaTitolo').removeAttr('disabled');
                }
                $('#lblErrorFO').text('Inserisci la Natura Titolo');
                $('#lblErrorFO').show(); //Show error
                $('#MainContent_ddlNaturaTitolo').focus(); //Focus on field  
                //alert($('#lblErrorFO').text());
                return false;
            }
        }
    }
    else {
        console.log('entro ed ho ' + parseFloat(($('[id*="_txtValore_"]').val().replace(',', '.'))) + '.');
        if (parseFloat(($('[id*="_txtValore_"]').val().replace(',', '.'))) > 3.3) {
            $('#lblErrorFO').text('Aliquota TASI massima 3,3 per mille');
            $('#lblErrorFO').show(); //Show error
            $('[id*="_txtValore_"]').focus(); //Focus on field  
            //alert($('#lblErrorFO').text());
            return false;
        }
    }
}
function FireServerSideClick() {
    return true;
}
function SelContribuente(id) {
    $('#MainContent_hfIdContribuente').val(id);
    $('#MainContent_CmdSetContribuente').click();
}
function SelNews(id, idGen, tributo) {
    alert(id.value);
    /*$('#MainContent_hfIdNews').val(id);
    $('#MainContent_hfIdGenNews').val(idGen);
    $('#MainContent_hfTributoNews').val(tributo);
    $('#MainContent_CmdReadNews').click();*/
}
function setImageData(imageBase64) {
    console.log('logo');
    $('.divLogo').css("background-image", "url('data:image/png;base64," + imageBase64 + "')");
    //document.getElementById("divLogo").src = "data:image/png;base64," + imageBase64;
}

function CheckCoookiePolicy() {
    if ($.cookie('OPENgovSPORTELLOPolicy') == null) {
        $('#cookie-notice').show();
        //$('#cookie-notice').hide();
    }
    else {
        $('#cookie-notice').hide();
    }
}
function SetCookiePolicy() {
    $.cookie('OPENgovSPORTELLOPolicy', 'OK', { expires: 365 });
}
function ControllaCF(CodiceFiscale) {
    var IsOK = false;
    $('#OnlyNumber_error').hide();
    if (CodiceFiscale.length != 0 && CodiceFiscale.length != 16) {
        $('#OnlyNumber_error').text('La lunghezza del codice fiscale non è corretta: il codice fiscale dovrebbe essere lungo esattamente 16 caratteri.');
        $('#OnlyNumber_error').show();
        //alert($('#OnlyNumber_error').text());
        $('#HeadTitleContent_CmdSave').hide();
        IsOK = false;
    } else {
        var validi, i, s, set1, set2, setpari, setdisp;
        CodiceFiscale = CodiceFiscale.toUpperCase();
        validi = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        for (i = 0; i < 16; i++) {
            if (validi.indexOf(CodiceFiscale.charAt(i)) == -1) {
                $('#OnlyNumber_error').text('Il codice fiscale contiene un carattere non valido `' +
                    CodiceFiscale.charAt(i) +
                    '`.\nI caratteri validi sono le lettere e le cifre.');
                $('#OnlyNumber_error').show();
                //alert($('#OnlyNumber_error').text());
                $('#HeadTitleContent_CmdSave').hide();
                IsOK = false;
            }
        }
        set1 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        set2 = "ABCDEFGHIJABCDEFGHIJKLMNOPQRSTUVWXYZ";
        setpari = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        setdisp = "BAKPLCQDREVOSFTGUHMINJWZYX";
        s = 0;
        for (i = 1; i <= 13; i += 2)
            s += setpari.indexOf(set2.charAt(set1.indexOf(CodiceFiscale.charAt(i))));
        for (i = 0; i <= 14; i += 2)
            s += setdisp.indexOf(set2.charAt(set1.indexOf(CodiceFiscale.charAt(i))));
        if (s % 26 != CodiceFiscale.charCodeAt(15) - 'A'.charCodeAt(0)) {
            $('#OnlyNumber_error').text('Il codice fiscale non è formalmente corretto: il codice di controllo non corrisponde.');
            $('#OnlyNumber_error').show();
            //alert($('#OnlyNumber_error').text());
            $('#HeadTitleContent_CmdSave').hide();
            IsOK = false;
        }
        else {
            $('#HeadTitleContent_CmdSave').show();
            IsOK = true;
        }
    }
    if ($('#OnlyNumber_error').is(':visible')) {
        console.log('nascondi');
        $('#HeadTitleContent_CmdDelegaSave').hide();
    } else {
        console.log('vedi');
        $('#HeadTitleContent_CmdDelegaSave').show();
    }
    return IsOK;
}
function ControllaPIVA(PartivaIva) {
    var IsOK = false;
    $('#OnlyNumber_error').hide();
    if (PartivaIva.length != 0 && PartivaIva.length != 11) {
        $('#OnlyNumber_error').text('La lunghezza della Partita IVA non è corretta: la Partita IVA dovrebbe essere lungo esattamente 11 caratteri.');
        $('#OnlyNumber_error').show();
        //alert($('#OnlyNumber_error').text());
        $('#HeadTitleContent_CmdSave').hide();
        IsOK= false;
    }
    validi = "0123456789";
    for (i = 0; i < 11; i++) {
        if (validi.indexOf(PartivaIva.charAt(i)) == -1) {
            $('#OnlyNumber_error').text('La partita IVA contiene un carattere non valido `'
                + PartivaIva.charAt(i) +
                    '`.\nI caratteri validi sono le cifre.');
            $('#OnlyNumber_error').show();
            //alert($('#OnlyNumber_error').text());
            $('#HeadTitleContent_CmdSave').hide();
            IsOK= false;
        }
    }
    s = 0;
    for (i = 0; i <= 9; i += 2)
        s += PartivaIva.charCodeAt(i) - '0'.charCodeAt(0);
    for (i = 1; i <= 9; i += 2) {
        c = 2 * (PartivaIva.charCodeAt(i) - '0'.charCodeAt(0));
        if (c > 9) c = c - 9;
        s += c;
    }
    if ((10 - s % 10) % 10 != PartivaIva.charCodeAt(10) - '0'.charCodeAt(0)) {
        console.log('cin sbagliato');
        $('#OnlyNumber_error').text('La partita IVA non è valida: il codice di controllo non corrisponde.');
        $('#OnlyNumber_error').show();
        //alert($('#OnlyNumber_error').text());
        $('#HeadTitleContent_CmdSave').hide();
        IsOK= false;
    }
    else {
        IsOK = true;
    }
    if ($('#OnlyNumber_error').is(':visible')) {
        console.log('nascondi');
        $('#HeadTitleContent_CmdDelegaSave').hide();
    } else {
        console.log('vedi');
        $('#HeadTitleContent_CmdSave').show();
        $('#HeadTitleContent_CmdDelegaSave').show();
    }
    return IsOK;
}
function mouseoverPWD(obj) {
    var obj = document.getElementById('MainContent_Password');
    obj.type = "text";
}
function mouseoutPWD(obj) {
    var obj = document.getElementById('MainContent_Password');
    obj.type = "password";
}
function LoadCSS(myCSSfile) {
    console.log('devo caricare css->' + myCSSfile);
    $('<link>')
      .appendTo('head')
      .attr({
          type: 'text/css',
          rel: 'stylesheet',
          href: '/Content/' + myCSSfile + '.css'
      });
}
function ChooseLogin(chiamante,Shown, Hidden) {
    Nascondi(Hidden);
    $('#' + Shown).show();
    $('.login-nav-pills-ul>ul>li.active').removeClass('active');
    $(chiamante).closest('li').addClass('active');
}
/*

$('#MainContent_txtLuogoNascita").autocomplete(
{
    source: '/Handler/Stradario.ashx'
});

$(function () {
$.support.cors = true;
$("[id$=MainContent_txtLuogoNascita]").autocomplete({
    source: function (request, response) {
        console.log('3...2...1...');
        var param = { keyword: $('#MainContent_txtLuogoNascita').val() };
        console.log(param);
        $.ajax({
            url: '<%=ResolveUrl("/Account/ProfiloFO.aspx/GetCountryNames") %>',
            data: JSON.stringify(param),
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                console.log(data);
                response($.map(data.d, function (item) {
                    console.log('quà');
                    return {
                        label: item.split('-')[0],
                        val: item.split('-')[1]
                    }
                }))
            },
            error: function (response) {
                alert(response.responseText);
            },
            failure: function (response) {
                alert(response.responseText);
            }
        });
    },
    select: function (e, i) {
        $("[id$=MainContent_hdCodComuneNascita]").val(i.item.val);
    },
    minLength: 1
});
});   

function GetCountryDetails(country) {
    $.ajax({
        type: "POST",
        url: "/Account/ProfiloFO.aspx/GetCountryNames",
        data: '{country: "' + country + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            response($.map(data.d, function (item) {
                $('#MainContent_hdCodComuneNascita').val(item.val);
            }))
        },
        failure: function (response) {
            alert(response.d);
        }
    });
}
*/