using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Xml;

namespace QBService
{
    [WebService(Namespace = "http://developer.intuit.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class qb : System.Web.Services.WebService
    {
        public int count = 0;
        public ArrayList req = new ArrayList();

        public qb()
        {
            // used to init log here..
        }

        [WebMethod]
        /// <summary>
        /// WebMethod - getInteractiveURL()
        /// 
        /// Signature: public string getInteractiveURL(string wcTicket, string sessionID)
        ///
        /// IN: 
        /// string wcTicket
        /// string sessionID
        ///
        /// OUT: 
        /// URL string 
        /// Possible values: 
        /// URL to a website
        /// </summary>
        public string getInteractiveURL(string wcTicket, string sessionID)
        {
            return "";
        }

        [WebMethod]
        /// <summary>
        /// WebMethod - interactiveRejected()
        /// 
        /// Signature: public string interactiveRejected(string wcTicket, string reason)
        ///
        /// IN: 
        /// string wcTicket
        /// string reason
        ///
        /// OUT: 
        /// string 
        /// </summary>
        public string interactiveRejected(string wcTicket, string reason)
        {
            return "";
        }

        [WebMethod]
        /// <summary>
        /// WebMethod - interactiveDone()
        /// 
        /// Signature: public string interactiveDone(string wcTicket)
        ///
        /// IN: 
        /// string wcTicket
        ///
        /// OUT: 
        /// string 
        /// </summary>
        public string interactiveDone(string wcTicket)
        {
            return "";
        }

        [WebMethod]
        /// <summary>
        /// WebMethod - serverVersion()
        /// To enable web service with its version number returned back to QBWC
        /// Signature: public string serverVersion()
        ///
        /// OUT: 
        /// string 
        /// Possible values: 
        /// Version string representing server version
        /// </summary>
        public string serverVersion()
        {
            string serverVersion = "2.0.0.1";

            //string evLogTxt = "WebMethod: serverVersion() has been called by QBWebconnector" + "\r\n\r\n";
            //evLogTxt = evLogTxt + "No Parameters required.";
            //evLogTxt = evLogTxt + "Returned: " + serverVersion;

            return serverVersion;
        }

        [WebMethod]
        /// <summary>
        /// WebMethod - clientVersion()
        /// To enable web service with QBWC version control
        /// Signature: public string clientVersion(string strVersion)
        ///
        /// IN: 
        /// string strVersion
        ///
        /// OUT: 
        /// string errorOrWarning
        /// Possible values: 
        /// string retVal
        /// - NULL or <emptyString> = QBWC will let the web service update
        /// - "E:<any text>" = popup ERROR dialog with <any text> 
        ///					- abort update and force download of new QBWC.
        /// - "W:<any text>" = popup WARNING dialog with <any text> 
        ///					- choice to user, continue update or not.
        /// </summary>
        public string clientVersion(string strVersion)
        {
            //string evLogTxt = "WebMethod: clientVersion() has been called by QBWebconnector" + "\r\n\r\n";
            //evLogTxt = evLogTxt + "Parameters received:\r\n";
            //evLogTxt = evLogTxt + "string strVersion = " + strVersion + "\r\n";
            //evLogTxt = evLogTxt + "\r\n";

            string retVal = null;

            double recommendedVersion = 1.5;
            double supportedMinVersion = 1.0;
            double suppliedVersion = Convert.ToDouble(this.parseForVersion(strVersion));

            //evLogTxt = evLogTxt + "QBWebConnector version = " + strVersion + "\r\n";
            //evLogTxt = evLogTxt + "Recommended Version = " + recommendedVersion.ToString() + "\r\n";
            //evLogTxt = evLogTxt + "Supported Minimum Version = " + supportedMinVersion.ToString() + "\r\n";
            //evLogTxt = evLogTxt + "SuppliedVersion = " + suppliedVersion.ToString() + "\r\n";

            if (suppliedVersion < recommendedVersion)
            {
                retVal = "W:We recommend that you upgrade your QBWebConnector";
            }
            else if (suppliedVersion < supportedMinVersion)
            {
                retVal = "E:You need to upgrade your QBWebConnector";
            }

            //evLogTxt = evLogTxt + "\r\n";
            //evLogTxt = evLogTxt + "Return values: " + "\r\n";
            //evLogTxt = evLogTxt + "string retVal = " + retVal;
            //logEvent(evLogTxt);

            return retVal;
        }

        [WebMethod]
        /// <summary>
        /// WebMethod - authenticate()
        /// To verify username and password for the web connector that is trying to connect
        /// Signature: public string[] authenticate(string strUserName, string strPassword)
        /// 
        /// IN: 
        /// string strUserName 
        /// string strPassword
        ///
        /// OUT: 
        /// string[] authReturn
        /// Possible values: 
        /// string[0] = ticket
        /// string[1]
        /// - empty string = use current company file
        /// - "none" = no further request/no further action required
        /// - "nvu" = not valid user
        /// - any other string value = use this company file
        /// </summary>
        public string[] authenticate(string strUserName, string strPassword)
        {
            string[] authReturn = new string[2];

            var log = Logger.Input("authenticate", string.Format("string strUserName={0}, string strPassword={1}", strUserName, strPassword), "");


            // Code below uses a random GUID to use as session ticket
            // An example of a GUID is {85B41BEE-5CD9-427a-A61B-83964F1EB426}
            authReturn[0] = System.Guid.NewGuid().ToString();

            // For simplicity of sample, a hardcoded username/password is used.
            // In real world, you should handle authentication in using a standard way. 
            // For example, you could validate the username/password against an LDAP 
            // or a directory server
            string pwd = "d@gents!";
            string userName = strUserName.Trim();

            //evLogTxt = evLogTxt + "Password locally stored = " + pwd + "\r\n";
            if ((userName.Equals("us") || userName.Equals("intl")) && strPassword.Trim().Equals(pwd))
            {
                //Session["UserName"] = userName;

                // An empty string for authReturn[1] means asking QBWebConnector 
                // to connect to the company file that is currently openned in QB
                //authReturn[1]="c:\\Program Files\\Intuit\\QuickBooks\\sample_product-based business.qbw";
                authReturn[1] = "";
            }
            else
            {
                authReturn[1] = "nvu";
            }

            // You could also return "none" to indicate there is no work to do
            // or a company filename in the format C:\full\path\to\company.qbw
            // based on your program logic and requirements.

            Logger.Output(log, authReturn[1], authReturn[0]);

            return authReturn;
        }

        [WebMethod(Description = "This web method facilitates web service to handle connection error between QuickBooks and QBWebConnector", EnableSession = true)]
        /// <summary>
        /// WebMethod - connectionError()
        /// To facilitate capturing of QuickBooks error and notifying it to web services
        /// Signature: public string connectionError (string ticket, string hresult, string message)
        ///
        /// IN: 
        /// string ticket = A GUID based ticket string to maintain identity of QBWebConnector 
        /// string hresult = An HRESULT value thrown by QuickBooks when trying to make connection
        /// string message = An error message corresponding to the HRESULT
        ///
        /// OUT:
        /// string retVal
        /// Possible values: 
        /// - “done” = no further action required from QBWebConnector
        /// - any other string value = use this name for company file
        /// </summary>
        public string connectionError(string ticket, string hresult, string message)
        {
            if (Session["ce_counter"] == null)
            {
                Session["ce_counter"] = 0;
            }

            //string evLogTxt = "WebMethod: connectionError() has been called by QBWebconnector" + "\r\n\r\n";
            //evLogTxt = evLogTxt + "Parameters received:\r\n";
            //evLogTxt = evLogTxt + "string ticket = " + ticket + "\r\n";
            //evLogTxt = evLogTxt + "string hresult = " + hresult + "\r\n";
            //evLogTxt = evLogTxt + "string message = " + message + "\r\n";
            //evLogTxt = evLogTxt + "\r\n";

            string retVal = null;

            // 0x80040400 - QuickBooks found an error when parsing the provided XML text stream. 
            const string QB_ERROR_WHEN_PARSING = "0x80040400";

            // 0x80040401 - Could not access QuickBooks.  
            const string QB_COULDNT_ACCESS_QB = "0x80040401";

            // 0x80040402 - Unexpected error. Check the qbsdklog.txt file for possible, additional information. 
            const string QB_UNEXPECTED_ERROR = "0x80040402";

            // Add more as you need...

            if (hresult.Trim().Equals(QB_ERROR_WHEN_PARSING))
            {
                //evLogTxt = evLogTxt + "HRESULT = " + hresult + "\r\n";
                //evLogTxt = evLogTxt + "Message = " + message + "\r\n";
                retVal = "DONE";
            }
            else if (hresult.Trim().Equals(QB_COULDNT_ACCESS_QB))
            {
                //evLogTxt = evLogTxt + "HRESULT = " + hresult + "\r\n";
                //evLogTxt = evLogTxt + "Message = " + message + "\r\n";
                retVal = "DONE";
            }
            else if (hresult.Trim().Equals(QB_UNEXPECTED_ERROR))
            {
                //evLogTxt = evLogTxt + "HRESULT = " + hresult + "\r\n";
                //evLogTxt = evLogTxt + "Message = " + message + "\r\n";
                retVal = "DONE";
            }
            else
            {
                // Depending on various hresults return different value 
                if ((int)Session["ce_counter"] == 0)
                {
                    // Try again with this company file
                    //evLogTxt = evLogTxt + "HRESULT = " + hresult + "\r\n";
                    //evLogTxt = evLogTxt + "Message = " + message + "\r\n";
                    //evLogTxt = evLogTxt + "Sending empty company file to try again.";
                    retVal = "";
                }
                else
                {
                    //evLogTxt = evLogTxt + "HRESULT = " + hresult + "\r\n";
                    //evLogTxt = evLogTxt + "Message = " + message + "\r\n";
                    //evLogTxt = evLogTxt + "Sending DONE to stop.";
                    retVal = "DONE";
                }
            }
            //evLogTxt = evLogTxt + "\r\n";
            //evLogTxt = evLogTxt + "Return values: " + "\r\n";
            //evLogTxt = evLogTxt + "string retVal = " + retVal + "\r\n";
            //logEvent(evLogTxt);
            Session["ce_counter"] = ((int)Session["ce_counter"]) + 1;

            return retVal;
        }

        [WebMethod(Description = "This web method facilitates web service to send request XML to QuickBooks via QBWebConnector", EnableSession = true)]
        /// <summary>
        /// WebMethod - sendRequestXML()
        /// Signature: public string sendRequestXML(string ticket, string strHCPResponse, string strCompanyFileName, 
        /// string Country, int qbXMLMajorVers, int qbXMLMinorVers)
        /// 
        /// IN: 
        /// int qbXMLMajorVers
        /// int qbXMLMinorVers
        /// string ticket
        /// string strHCPResponse 
        /// string strCompanyFileName 
        /// string Country
        /// int qbXMLMajorVers
        /// int qbXMLMinorVers
        ///
        /// OUT:
        /// string request
        /// Possible values: 
        /// - “any_string” = Request XML for QBWebConnector to process
        /// - "" = No more request XML 
        /// </summary>
        public string sendRequestXML(
            string ticket,
            string strHCPResponse,
            string strCompanyFileName,
            string qbXMLCountry,
            int qbXMLMajorVers,
            int qbXMLMinorVers)
        {
            var log = Logger.Input("sendRequestXML", strHCPResponse, ticket);

            if (Session["counter"] == null)
            {
                Session["counter"] = 0;
            }

            ArrayList req = buildRequest();
            string request = "";
            int total = req.Count;
            count = Convert.ToInt32(Session["counter"]);

            if (count < total)
            {
                request = req[count].ToString();
                Session["counter"] = ((int)Session["counter"]) + 1;
            }
            else
            {
                count = 0;
                Session["counter"] = 0;
                request = "";
            }

            Logger.Output(log, request, ticket);

            return request;
        }

        [WebMethod(Description = "This web method facilitates web service to receive response XML from QuickBooks via QBWebConnector", EnableSession = true)]
        /// <summary>
        /// WebMethod - receiveResponseXML()
        /// Signature: public int receiveResponseXML(string ticket, string response, string hresult, string message)
        /// 
        /// IN: 
        /// string ticket
        /// string response
        /// string hresult
        /// string message
        ///
        /// OUT: 
        /// int retVal
        /// Greater than zero  = There are more request to send
        /// 100 = Done. no more request to send
        /// Less than zero  = Custom Error codes
        /// </summary>
        public int receiveResponseXML(string ticket, string response, string hresult, string message)
        {
            var log = Logger.Input("receiveResponseXML", response, ticket);

            int retVal = 0;

            if (!hresult.ToString().Equals(""))
            {
                retVal = -101;
            }
            else
            {
                ArrayList req = buildRequest();
                int total = req.Count;
                int count = Convert.ToInt32(Session["counter"]);

                int percentage = (count * 100) / total;
                if (percentage >= 100)
                {
                    count = 0;
                    Session["counter"] = 0;
                }

                retVal = percentage;
            }

            Logger.Output(log, retVal.ToString(), ticket);

            return retVal;
        }

        [WebMethod]
        /// <summary>
        /// WebMethod - getLastError()
        /// Signature: public string getLastError(string ticket)
        /// 
        /// IN:
        /// string ticket
        /// 
        /// OUT:
        /// string retVal
        /// Possible Values:
        /// Error message describing last web service error
        /// </summary>
        public string getLastError(string ticket)
        {
            //string evLogTxt = "WebMethod: getLastError() has been called by QBWebconnector" + "\r\n\r\n";
            //evLogTxt = evLogTxt + "Parameters received:\r\n";
            //evLogTxt = evLogTxt + "string ticket = " + ticket + "\r\n";
            //evLogTxt = evLogTxt + "\r\n";

            int errorCode = 0;
            string retVal = null;
            if (errorCode == -101)
            {
                retVal = "Received empty response."; // This is just an example of custom user errors
            }
            else
            {
                retVal = "Error!";
            }

            //evLogTxt = evLogTxt + "\r\n";
            //evLogTxt = evLogTxt + "Return values: " + "\r\n";
            //evLogTxt = evLogTxt + "string retVal= " + retVal + "\r\n";
            //logEvent(evLogTxt);
            return retVal;
        }

        [WebMethod]
        /// <summary>
        /// WebMethod - closeConnection()
        /// At the end of a successful update session, QBWebConnector will call this web method.
        /// Signature: public string closeConnection(string ticket)
        /// 
        /// IN:
        /// string ticket 
        /// 
        /// OUT:
        /// string closeConnection result 
        /// </summary>
        public string closeConnection(string ticket)
        {
            //string evLogTxt = "WebMethod: closeConnection() has been called by QBWebconnector" + "\r\n\r\n";
            //evLogTxt = evLogTxt + "Parameters received:\r\n";
            //evLogTxt = evLogTxt + "string ticket = " + ticket + "\r\n";
            //evLogTxt = evLogTxt + "\r\n";

            string retVal = null;

            retVal = "OK";

            //evLogTxt = evLogTxt + "\r\n";
            //evLogTxt = evLogTxt + "Return values: " + "\r\n";
            //evLogTxt = evLogTxt + "string retVal= " + retVal + "\r\n";
            //logEvent(evLogTxt);
            return retVal;
        }

        #region UtilityMethods
        public ArrayList buildRequest()
        {
            string strRequestXML = "";
            XmlDocument inputXMLDoc = null;

            // CustomerQuery
            inputXMLDoc = new XmlDocument();
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"4.0\""));

            XmlElement qbXML = inputXMLDoc.CreateElement("QBXML");
            inputXMLDoc.AppendChild(qbXML);
            XmlElement qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            XmlElement customerQueryRq = inputXMLDoc.CreateElement("CustomerQueryRq");
            qbXMLMsgsRq.AppendChild(customerQueryRq);
            customerQueryRq.SetAttribute("requestID", "1");
            XmlElement maxReturned = inputXMLDoc.CreateElement("MaxReturned");
            customerQueryRq.AppendChild(maxReturned).InnerText = "1";

            strRequestXML = inputXMLDoc.OuterXml;
            req.Add(strRequestXML);

            // Clean up
            strRequestXML = "";
            inputXMLDoc = null;
            qbXML = null;
            qbXMLMsgsRq = null;
            maxReturned = null;

            // InvoiceQuery
            inputXMLDoc = new XmlDocument();
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"4.0\""));

            qbXML = inputXMLDoc.CreateElement("QBXML");
            inputXMLDoc.AppendChild(qbXML);
            qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            XmlElement invoiceQueryRq = inputXMLDoc.CreateElement("InvoiceQueryRq");
            qbXMLMsgsRq.AppendChild(invoiceQueryRq);
            invoiceQueryRq.SetAttribute("requestID", "2");
            maxReturned = inputXMLDoc.CreateElement("MaxReturned");
            invoiceQueryRq.AppendChild(maxReturned).InnerText = "1";

            strRequestXML = inputXMLDoc.OuterXml;
            req.Add(strRequestXML);

            // Clean up
            strRequestXML = "";
            inputXMLDoc = null;
            qbXML = null;
            qbXMLMsgsRq = null;
            maxReturned = null;

            // BillQuery
            inputXMLDoc = new XmlDocument();
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"4.0\""));

            qbXML = inputXMLDoc.CreateElement("QBXML");
            inputXMLDoc.AppendChild(qbXML);
            qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            XmlElement billQueryRq = inputXMLDoc.CreateElement("BillQueryRq");
            qbXMLMsgsRq.AppendChild(billQueryRq);
            billQueryRq.SetAttribute("requestID", "3");
            maxReturned = inputXMLDoc.CreateElement("MaxReturned");
            billQueryRq.AppendChild(maxReturned).InnerText = "1";

            strRequestXML = inputXMLDoc.OuterXml;
            req.Add(strRequestXML);

            return req;
        }

        private string parseForVersion(string input)
        {
            // This method is created just to parse the first two version components
            // out of the standard four component version number:
            // <Major>.<Minor>.<Release>.<Build>
            // 
            // As long as you get the version in right format, you could use
            // any algorithm here. 
            string retVal = "";
            string major = "";
            string minor = "";
            Regex version = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)(\.\w+){0,2}$", RegexOptions.Compiled);
            Match versionMatch = version.Match(input);
            if (versionMatch.Success)
            {
                major = versionMatch.Result("${major}");
                minor = versionMatch.Result("${minor}");
                retVal = major + "." + minor;
            }
            else
            {
                retVal = input;
            }
            return retVal;
        }
        #endregion
    }
}
