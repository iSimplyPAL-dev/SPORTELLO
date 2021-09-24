using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OPENgovSPORTELLO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//...
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace OPENgovSPORTELLO.SPID
{
    /// <summary>
    /// 
    /// </summary>
    public partial class POST : GeneralPage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(POST));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string mySAMLResponse= string.Empty;
            try
            {
                if (Request["SAMLResponse"] != null)
                    mySAMLResponse = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Request["SAMLResponse"]));

                if (mySAMLResponse != string.Empty)
                {
                    Log.Debug("OPENgovSPORTELLO.POST.mySAMLResponse="+ mySAMLResponse);
                    MySession.Current.SPIDAuthn = new SPIDAuthn();
                    var reader = XmlReader.Create(new MemoryStream(Convert.FromBase64String(Request["SAMLResponse"])));
                    var serializer = new XmlSerializer(typeof(XmlElement));
                    var samlResponseElement = (XmlElement)serializer.Deserialize(reader);

                    foreach(XmlNode myChildRespose in samlResponseElement.ChildNodes)
                    {
                        if (myChildRespose.Name == "saml:Assertion")
                        {
                           foreach(XmlNode myChildAssertion in myChildRespose.ChildNodes)
                            {
                                if(myChildAssertion.Name== "saml:AttributeStatement")
                                {
                                    foreach (XmlNode myNode in myChildAssertion.ChildNodes)
                                    {
                                        if (myNode.Attributes != null)
                                        {
                                            foreach (XmlAttribute myAttribute in myNode.Attributes)
                                            {
                                                switch (myAttribute.Value)
                                                {
                                                    case "fiscalNumber":
                                                        foreach (XmlNode myChildNode in myNode.ChildNodes)
                                                        {
                                                            MySession.Current.SPIDAuthn.fiscalNumber = myChildNode.InnerText.Replace("\n", "").Trim();
                                                            if (MySession.Current.SPIDAuthn.fiscalNumber.Length > 15)
                                                                MySession.Current.SPIDAuthn.fiscalNumber = MySession.Current.SPIDAuthn.fiscalNumber.Substring(MySession.Current.SPIDAuthn.fiscalNumber.Length - 16, 16);
                                                            if (MySession.Current.SPIDAuthn.fiscalNumber!=string.Empty)
                                                                break;
                                                        }
                                                        break;
                                                    case "ivaCode":
                                                        foreach (XmlNode myChildNode in myNode.ChildNodes)
                                                        {
                                                            MySession.Current.SPIDAuthn.ivaCode = myChildNode.InnerText.Replace("\n", "").Trim();
                                                            if (MySession.Current.SPIDAuthn.ivaCode.Length > 10)
                                                                MySession.Current.SPIDAuthn.ivaCode = MySession.Current.SPIDAuthn.ivaCode.Substring(MySession.Current.SPIDAuthn.ivaCode.Length - 11, 11);
                                                            if (MySession.Current.SPIDAuthn.ivaCode != string.Empty)
                                                                break;
                                                        }
                                                        break;
                                                    case "email":
                                                        foreach (XmlNode myChildNode in myNode.ChildNodes)
                                                        {
                                                            MySession.Current.SPIDAuthn.email = myChildNode.InnerText.Replace("\n", "").Trim();
                                                            if (MySession.Current.SPIDAuthn.email != string.Empty)
                                                                goto Auth;
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    Auth:
                    if (MySession.Current.SPIDAuthn.email != string.Empty)
                    {
                        string myFailureText = string.Empty;
                        var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                        var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
                        var user = manager.FindByEmail(MySession.Current.SPIDAuthn.email);
                        if (user!=null)
                        signinManager.SignIn(user, true, false);
                        // BD 24/09/2021 Problema con le deleghe
                        //string mySignIn = new LoginManager().ManageLogin(MySession.Current.SPIDAuthn.email, "", out myFailureText);
                        string mySignIn = new LoginManager().ManageLogin(MySession.Current.SPIDAuthn.email,
                                                                         user.CodiceFiscale,
                                                                         "", out myFailureText);
                        // BD 24/09/2021

                        switch (mySignIn)
                        {
                            case "GetProfiloFO":
                                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetProfiloFO, Response);
                                break;
                            case "GetDefaultFO":
                                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetDefaultFO, Response);
                                break;
                            case "GetEmailConfirmation":
                                MySession.Current.Ente = null;
                                Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetEmailConfirmation, Response);
                                break;
                            case "Errore":
                                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetRegister, Response);
                                break;
                            case "GetSelDelegante":
                                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetSelDelegante, Response);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    FailureText.Text = "Email obbligatoria per l'accesso.";
                    ErrorMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.POST.Page_Load::errore::", ex);
            }
        }
    }
    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:oasis:names:tc:SAML:2.0:assertion")]
    [XmlRoot(Namespace = "urn:oasis:names:tc:SAML:2.0:assertion", IsNullable = false)]
    public partial class AttributeStatement
    {

        private AttributeStatementAttribute[] attributeField;

        /// <remarks/>
        [XmlElement("Attribute")]
        public AttributeStatementAttribute[] Attribute
        {
            get
            {
                return this.attributeField;
            }
            set
            {
                this.attributeField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:oasis:names:tc:SAML:2.0:assertion")]
    public partial class AttributeStatementAttribute
    {
        private string attributeValueField;

        private string nameField;

        private string nameFormatField;

        /// <remarks/>
        public string AttributeValue
        {
            get
            {
                return this.attributeValueField;
            }
            set
            {
                this.attributeValueField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string NameFormat
        {
            get
            {
                return this.nameFormatField;
            }
            set
            {
                this.nameFormatField = value;
            }
        }
    }
}