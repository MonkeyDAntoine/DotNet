using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using TagManagerLib;

namespace WebApp
{
    public partial class _Index : System.Web.UI.Page {

        private RenderHtml htmlRenderer = new RenderHtml();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void RenderTextSource(Object sender, EventArgs e)
        {
            SyntaxTree syntaxTree = new SyntaxTree(SourceCode_TextBox.Text);
            syntaxTree.process();
            RenderingResult_Div.InnerHtml = htmlRenderer.Render(syntaxTree);
        }

        [System.Web.Services.WebMethod]
        public static string BoldText(string text)
        {
            ITag tag = new TagBold();
            return tag.OpenTag + text + tag.CloseTag;
        }
    }
}
