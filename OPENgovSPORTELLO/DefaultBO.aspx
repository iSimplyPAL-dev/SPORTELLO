<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="DefaultBO.aspx.cs" Inherits="OPENgovSPORTELLO._Default" %>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <table>
        <tr>
            <td><a class="Login nav navbar-nav Bottone BottoneLogin"></a></td>
            <td><a class="Login nav navbar-nav">Accedi</a></td>
        </tr>
        <tr>
            <td><a href="Help/BO_FAQ.aspx" class="nav navbar-nav Bottone BottoneFAQ"></a></td>
            <td><a href="Help/BO_FAQ.aspx" class="nav navbar-nav">F.A.Q.</a></td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="body_page">
        <div class="col-md-12 lead">
            <h2 class="col-md-12">
                <label class="col-md-7">Back office dello Sportello Contribuente</label>
            </h2>
        </div>
        <div class="col-md-12 divDescr">
            <div id="BodyHomePage" class="jumbotron text-normal text-justified">
                <p>Il back office dello Sportello Contribuente consente:</p>
                <ul>
                    <li>la gestione delle istanze presentate dai cittadini-contribuenti,</li>
                    <li>l’invio di comunicazioni verso i contribuenti (comunicazioni specifiche o generiche),</li>
                    <li>l’analisi dei dati gestiti on-line,</li>
                    <li>l’aggiornamento del verticale del Comune,</li>
                    <li>l’acquisizione dei pagamenti dei tributi minori.</li>
                </ul>
				<p>Il back office è a disposizione del Patto Territoriale, ma è anche a disposizione degli operatori dei singoli Comuni, consentendo di operare contemporaneamente sugli stessi dati.</p>
                <h3 class="text-right">Il back office  per semplificare le operazioni di sportello.</h3>
            </div>&nbsp;
        </div>
        <div class="col-md-12 divDescr">
            <div class="col-md-4 text-justified">
                <div class="col-md-12 lead ArgPan">
                    <div class="ArgHeadBO">Configurazioni&ensp;<a class="Argbtn btn btn-default Configurazioni">Vai &raquo;</a></div>
                    <input type="image" class="ArgImg imgBaseModuli imgBaseModuliLarge imgBaseModuliConfigurazioni" src="Images/Settings-icon.png" />
                </div>
                <div class="col-md-11" id="HomePageConfigurazioni">Nella sezione Configurazioni si trovano le funzionalità necessarie alla gestione di tutte le tabelle di Sistema e delle tabelle degli specifici tributi. Qui sono configurabili anche gli operatori del Sistema a cura dell’Amministratore.<p/>
				Accedi alla sezione con un clic su <b>&lsquo;Vai &raquo;&rsquo;</b>. Puoi accedere solo dopo aver effettuato il login.</div>
            </div>
            <div class="col-md-4 text-justified">
                <div class="col-md-12 lead ArgPan">
                    <div class="ArgHeadBO">Istanze&ensp;<a class="Argbtn btn btn-default IstanzeBO">Vai &raquo;</a></div>
                    <input type="image" class="ArgImg imgBaseModuli imgBaseModuliLarge imgBaseModuliIstanze" src="Images/Files-2-icon.png" />
                </div>                
                <div class="col-md-11" id="HomePageIstanze">Nella sezione Istanze si trovano le funzionalità necessarie alla gestione di tutte le comunicazioni/dichiarazioni registrate on-line dai contribuenti.
                Accedi alla sezione con un clic su <b>&lsquo;Vai &raquo;&rsquo;</b>. Puoi accedere solo dopo aver effettuato il login.</div>
            </div>
            <div class="col-md-4 text-justified">
                <div class="col-md-12 lead ArgPan">
                    <div class="ArgHeadBO">Cruscotto&ensp;<a class="Argbtn btn btn-default ReportBO">Vai &raquo;</a></div>
                    <input type="image" class="ArgImg imgBaseModuli imgBaseModuliLarge imgBaseModuliCruscotto" src="Images/Analytics-2-icon.png" />
                </div>                
                <div class="col-md-11" id="HomePageCruscotto">Nella sezione Cruscotto si trovano le funzionalità che consentono l’analisi dei dati di sportello, nonché la verifica dello stato di lavorazione delle istanze. Il Cruscotto include le funzioni di estrazioni e reportistica del back office.
                Accedi alla sezione con un clic su <b>&lsquo;Vai &raquo;&rsquo;</b>. Puoi accedere solo dopo aver effettuato il login.</div>
            </div>
        </div>
    </div>
</asp:Content>
