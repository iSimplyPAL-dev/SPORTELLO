$(document).ready(function () {
    $('a.HomeBO').attr('href', '/DefaultBO');
    $('a.HomeFO').attr('href', '/DefaultFO');

    $('a.LoginFO').attr('href', '/Account/LoginFO');
    $('a.NewLoginFO').attr('href', '/Account/RegisterFO');
    $('a.LoginBO').attr('href', '/Account/LoginBO');
    $('a.NewLoginBO').attr('href', '/Account/RegisterBO');
    $('a.Profilo').attr('href', '/Account/ProfiloFO');
    $('a.Tributi').attr('href', '/Dichiarazioni/FO_Base.aspx');
    $('a.ReportFO').attr('href', '/Cruscotto/FO_ReportGen.aspx');
    $('a.ReportBO').attr('href', '/Cruscotto/BO_ReportGen.aspx');
    $('a.Paga').attr('href', '/Paga/FO_PayGen.aspx');
    $('a.TARSU').attr('href', '/Dichiarazioni/TARSU/Riepilogo.aspx');
    $('a.ICI').attr('href', '/Dichiarazioni/ICI/Riepilogo.aspx');
    $('a.TASI').attr('href', '/Dichiarazioni/TASI/Riepilogo.aspx');
    $('a.OSAP').attr('href', '/Dichiarazioni/OSAP/Riepilogo.aspx');
    $('a.ICP').attr('href', '/Dichiarazioni/ICP/Riepilogo.aspx');
    $('a.PROVVEDIMENTI').attr('href', '/Dichiarazioni/PROVVEDIMENTI/Riepilogo.aspx');
    $('a.IstanzeFO').attr('href', '/Istanze/FO_IstanzeGen.aspx');
    $('a.IstanzeBO').attr('href', '/Istanze/BO_IstanzeGen.aspx');
    $('a.Configurazioni').attr('href', '/Settings/BO_SettingsBase.aspx');
    $('a.AnalisiIstanze').attr('href', '/Cruscotto/Analisi/BO_AnalisiIstanzeFO.aspx')
    $('a.AnalisiEventi').attr('href', '/Cruscotto/Analisi/BO_AnalisiIstanzeBO.aspx')
    $('a.ComunicazioniBOvsFO').attr('href', '/Cruscotto/BO_ComunicazioniBOvsFO.aspx')
    $('a.BODovutoVersato').attr('href', '/Cruscotto/Analisi/BO_DovutoVersato.aspx')
    $('a.BOTempiPagamento').attr('href', '/Cruscotto/Analisi/BO_TempiPagamento.aspx')

    $('.FOCartellaUnica').attr('href', '/Cruscotto/FO_CartellaUnica.aspx');
    $('.BOCartellaUnica').attr('href', '/Cruscotto/BO_CartellaUnica.aspx');

    $('a.FAQBO').attr('href', '/Help/BO_FAQ.aspx');
    $('a.FAQFO').attr('href', '/Help/FO_FAQ.aspx');

    $('iframe.LoginBySPID').attr('src', 'https://spid.grandcombin.vda.it');
    
    $('a.ForgotPWD').attr('href', '/Account/Forgot.aspx');
})

