﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 11.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace CakeExtracter.Reports
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\GitHub\da-projects\3\CakeExtracter\CakeExtracter\Reports\SearchReportRuntimeTextTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "11.0.0.0")]
    public partial class SearchReportRuntimeTextTemplate : SearchReportRuntimeTextTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write(@"<table width=""620"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0"" style=""border:1px solid #c7c3c7;"">
<tr><td style=""font-size:0px;"">
<table width=""620"" border=""0"" cellspacing=""0"" cellpadding=""0"">
  <tr>
    <td width=""13"" rowspan=""2"" align=""left"" valign=""top"">&nbsp;</td>
    <td width=""195"" rowspan=""2"" align=""left"" valign=""top""><img src=""https://portal.directagents.com/Images/logo1.png"" alt="""" width=""152"" height=""86"" /></td>
    <td height=""24"" colspan=""2"" align=""left"" valign=""top"">&nbsp;</td>
  </tr>
  <tr>
    <td width=""399"" align=""right"" valign=""top""><strong style=""font-family: Arial, Helvetica, sans-serif; text-align: center; font-size: 18px;"">Weekly Summary Report for ");
            
            #line 15 "C:\GitHub\da-projects\3\CakeExtracter\CakeExtracter\Reports\SearchReportRuntimeTextTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.AdvertiserName));
            
            #line default
            #line hidden
            this.Write("</strong></td>\r\n    <td width=\"13\" align=\"right\" valign=\"top\">&nbsp;</td>\r\n  </tr" +
                    ">\r\n  <tr>\r\n    <td align=\"left\" valign=\"top\">&nbsp;</td>\r\n    <td colspan=\"2\" al" +
                    "ign=\"left\" valign=\"top\" style=\"font-family: Arial, Helvetica, sans-serif; font-s" +
                    "ize: 14px;\">Thank you for using the Direct Agents client portal. A summary of yo" +
                    "ur campaign performance is included below. Please <a href=\"http://portal.directa" +
                    "gents.com/\" style=\"color:#37A5F2;\"><strong>log in</strong></a> to access your co" +
                    "mplete reports.<br><br></td>\r\n    <td align=\"right\" valign=\"top\">&nbsp;</td>\r\n  " +
                    "</tr>\r\n  <tr>\r\n    <td colspan=\"4\" height=\"80\" align=\"center\" valign=\"middle\">\r\n" +
                    "   \r\n    <table width=\"620\"  align=\"center\" cellpadding=\"0\" cellspacing=\"0\" styl" +
                    "e=\"border-top:1px solid #c7c3c7; border-bottom:1px solid #c7c3c7; border-left:0p" +
                    "x solid #c7c3c7; border-right:0px solid #c7c3c7;\">\r\n      <tr>\r\n        <td heig" +
                    "ht=\"30\" align=\"left\" valign=\"middle\" bgcolor=\"#e5e5e5\" style=\"color: #000; borde" +
                    "r-right:1px solid #c7c3c7; font-family: Arial, Helvetica, sans-serif; font-size:" +
                    " 13px;\">&nbsp;&nbsp;Week</td>\r\n        <td height=\"30\" align=\"left\" valign=\"midd" +
                    "le\" bgcolor=\"#e5e5e5\" style=\"color: #000; border-right:1px solid #c7c3c7; font-f" +
                    "amily: Arial, Helvetica, sans-serif; font-size: 13px;\">&nbsp;&nbsp;Revenue</td>\r" +
                    "\n        <td height=\"30\" align=\"left\" valign=\"middle\" bgcolor=\"#e5e5e5\" style=\"c" +
                    "olor: #000; border-right:1px solid #c7c3c7; font-family: Arial, Helvetica, sans-" +
                    "serif; font-size: 13px;\">&nbsp;&nbsp;Cost</td>\r\n        <td height=\"30\" align=\"l" +
                    "eft\" valign=\"middle\" bgcolor=\"#e5e5e5\" style=\"color: #000; border-right:1px soli" +
                    "d #c7c3c7; font-family: Arial, Helvetica, sans-serif; font-size: 13px;\">&nbsp;&n" +
                    "bsp;ROAS</td>\r\n        <td height=\"30\" align=\"left\" valign=\"middle\" bgcolor=\"#e5" +
                    "e5e5\" style=\"color: #000; border-right:1px solid #c7c3c7; font-family: Arial, He" +
                    "lvetica, sans-serif; font-size: 13px;\">&nbsp;&nbsp;Margins</td>\r\n        <td hei" +
                    "ght=\"30\" align=\"left\" valign=\"middle\" bgcolor=\"#e5e5e5\" style=\"color: #000; bord" +
                    "er-right:1px solid #c7c3c7; font-family: Arial, Helvetica, sans-serif; font-size" +
                    ": 13px;\">&nbsp;&nbsp;Orders</td>\r\n        <td height=\"30\" align=\"left\" valign=\"m" +
                    "iddle\" bgcolor=\"#e5e5e5\" style=\"color: #000; font-family: Arial, Helvetica, sans" +
                    "-serif; font-size: 13px;\">&nbsp;&nbsp;CPO</td>\r\n      </tr>\r\n      <tr>\r\n       " +
                    " <td height=\"30\" align=\"left\" valign=\"middle\" bgcolor=\"#FFFFFF\" style=\"color: #0" +
                    "00; border-right:1px solid #c7c3c7; font-family: Arial, Helvetica, sans-serif; f" +
                    "ont-size: 13px; text-align: center; font-weight: bold;\">");
            
            #line 37 "C:\GitHub\da-projects\3\CakeExtracter\CakeExtracter\Reports\SearchReportRuntimeTextTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Week));
            
            #line default
            #line hidden
            this.Write("</td>\r\n        <td height=\"30\" align=\"left\" valign=\"middle\" bgcolor=\"#FFFFFF\" sty" +
                    "le=\"color: #000; border-right:1px solid #c7c3c7; font-family: Arial, Helvetica, " +
                    "sans-serif; font-size: 13px; text-align: center; font-weight: bold;\">");
            
            #line 38 "C:\GitHub\da-projects\3\CakeExtracter\CakeExtracter\Reports\SearchReportRuntimeTextTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Revenue));
            
            #line default
            #line hidden
            this.Write("</td>\r\n        <td height=\"30\" align=\"left\" valign=\"middle\" bgcolor=\"#FFFFFF\" sty" +
                    "le=\"color: #000; border-right:1px solid #c7c3c7; font-family: Arial, Helvetica, " +
                    "sans-serif; font-size: 13px; text-align: center; font-weight: bold;\">");
            
            #line 39 "C:\GitHub\da-projects\3\CakeExtracter\CakeExtracter\Reports\SearchReportRuntimeTextTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Cost));
            
            #line default
            #line hidden
            this.Write("</td>\r\n        <td height=\"30\" align=\"left\" valign=\"middle\" bgcolor=\"#FFFFFF\" sty" +
                    "le=\"color: #000; border-right:1px solid #c7c3c7; font-family: Arial, Helvetica, " +
                    "sans-serif; font-size: 13px; text-align: center; font-weight: bold;\">");
            
            #line 40 "C:\GitHub\da-projects\3\CakeExtracter\CakeExtracter\Reports\SearchReportRuntimeTextTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.ROAS));
            
            #line default
            #line hidden
            this.Write("</td>\r\n        <td height=\"30\" align=\"left\" valign=\"middle\" bgcolor=\"#FFFFFF\" sty" +
                    "le=\"color: #000;  border-right:1px solid #c7c3c7;font-family: Arial, Helvetica, " +
                    "sans-serif; font-size: 13px; text-align: center; font-weight: bold;\">");
            
            #line 41 "C:\GitHub\da-projects\3\CakeExtracter\CakeExtracter\Reports\SearchReportRuntimeTextTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Margin));
            
            #line default
            #line hidden
            this.Write("</td>\r\n        <td height=\"30\" align=\"left\" valign=\"middle\" bgcolor=\"#FFFFFF\" sty" +
                    "le=\"color: #000; border-right:1px solid #c7c3c7; font-family: Arial, Helvetica, " +
                    "sans-serif; font-size: 13px; text-align: center; font-weight: bold;\">");
            
            #line 42 "C:\GitHub\da-projects\3\CakeExtracter\CakeExtracter\Reports\SearchReportRuntimeTextTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Orders));
            
            #line default
            #line hidden
            this.Write("</td>\r\n        <td height=\"30\" align=\"left\" valign=\"middle\" bgcolor=\"#FFFFFF\" sty" +
                    "le=\"color: #000; font-family: Arial, Helvetica, sans-serif; font-size: 12px; tex" +
                    "t-align: center; font-weight: bold;\">");
            
            #line 43 "C:\GitHub\da-projects\3\CakeExtracter\CakeExtracter\Reports\SearchReportRuntimeTextTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.CPO));
            
            #line default
            #line hidden
            this.Write("</td>\r\n      </tr>\r\n    </table>\r\n</td>\r\n  </tr>\r\n  <tr>\r\n    <td width=\"13\" alig" +
                    "n=\"left\" valign=\"top\">&nbsp;</td>\r\n    <td colspan=\"2\" align=\"left\" valign=\"top\"" +
                    " ><span style=\"font-family: Arial, Helvetica, sans-serif; font-size: 14px;\" ><br" +
                    ">The Direct Agents Client Portal provides access to real-time reports showing ca" +
                    "mpaign effectiveness and spend across all search channels. You can easily view a" +
                    " breakdown by individual campaign and visualize key metrics your campaign perfor" +
                    "mance with interactive charts. <a href=\"http://portal.directagents.com/\" style=\"" +
                    "color:#37A5F2;\"><strong>View reports now</strong></a></span></td>\r\n    <td width" +
                    "=\"13\" align=\"left\" valign=\"top\">&nbsp;</td>\r\n  </tr>\r\n  <tr>\r\n    <td height=\"13" +
                    "\" colspan=\"4\" align=\"left\" valign=\"top\">&nbsp;</td>\r\n  </tr>\r\n  <tr>\r\n    <td wi" +
                    "dth=\"13\" align=\"left\" valign=\"top\">&nbsp;</td>\r\n    <td height=\"80\" colspan=\"2\" " +
                    "align=\"left\" valign=\"middle\"><table width=\"593\" border=\"0\" align=\"left\" cellpadd" +
                    "ing=\"0\" cellspacing=\"0\">\r\n        <tr>\r\n          <td width=\"68\" align=\"center\" " +
                    "valign=\"top\"><img src=\"https://portal.directagents.com/Images/icon_reports.gif\" " +
                    "alt=\"\" style=\"width:60%; height:auto;\" /></td>\r\n          <td width=\"450\" align=" +
                    "\"left\" valign=\"middle\"><span style=\"font-family: Arial, Helvetica, sans-serif; f" +
                    "ont-size: 14px;\"><strong>Top Tip: You can export your data from your reports in " +
                    "CSV format. </strong><a href=\"http://portal.directagents.com/\" style=\"color:#37A" +
                    "5F2;\"><strong>Log in</strong></a></span></td>\r\n        </tr>\r\n    </table></td>\r" +
                    "\n    <td width=\"13\" align=\"right\" valign=\"top\">&nbsp;</td>\r\n  </tr>\r\n</table>\r\n<" +
                    "/td>\r\n</tr>\r\n</table>\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "11.0.0.0")]
    public class SearchReportRuntimeTextTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
