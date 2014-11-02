using System.Collections;
using System.Text.RegularExpressions;
using System.Web;

namespace TagManagerLib
{
    /*
     * HTML Renderer
     * */
    public class RenderHtml : Renderer
    {
        Hashtable _htmlTags;

        public RenderHtml()
        {
            _htmlTags = new Hashtable();
            _htmlTags.Add(new TagBold(), new HtmlTagBold());
            _htmlTags.Add(new TagColor("green"), new HtmlTagColor("green"));
            _htmlTags.Add(TagNoProcess.Instance, new HtmlTagNoProcess());
        }

        public string Render(SyntaxTree tree)
        {
            string result = "";
            foreach (Node child in tree.Root.Children) {
                result += renderNode(child);
            }
            return result;
        }

        private string renderNode(Node node)
        {
            if (node is InnerTextNode) {
                return HttpUtility.HtmlEncode(((InnerTextNode) node).Text);
            }

            string result = "";
            ITagToHtml tagHtml = null;
            if (node.Tag != null)
            {
                tagHtml = (ITagToHtml) _htmlTags[node.Tag];
            }

            if (tagHtml != null)
            {
                result = tagHtml.OpenHtmlTag;
            }

            if (tagHtml.Equals(HtmlTagNoProcess.Instance))
            {
                result += renderNoPrecessNode(node);
            }
            else {
                foreach (Node child in node.Children) {
                    result += renderNode(child);
                }
            }

            if (tagHtml != null)
            {
                result += tagHtml.CloseHtmlTag;
            }

            return result;

        }

        private string renderNoPrecessNode(Node node)
        {
            string result = "";
            foreach (Node child in node.Children)
            {
                if (child.Tag != null)
                {
                    result += child.Tag.OpenTag + renderNoPrecessNode(child) + child.Tag.CloseTag;
                }
                else
                {
                    result +=  child.ToString() + renderNoPrecessNode(child);
                }
            }
            return HttpUtility.HtmlEncode(result);
        }

    }

    public abstract class ITagToHtml
    {
        public abstract ITag ITag { get;  }
        public abstract string OpenHtmlTag { get; }
        public abstract string CloseHtmlTag { get; }

        public override sealed int GetHashCode()
        {
            return OpenHtmlTag.GetHashCode() * CloseHtmlTag.GetHashCode();
        }

        public override sealed bool Equals(object obj)
        {
            if (obj is ITagToHtml)
            {
                ITagToHtml tag = (ITagToHtml)obj;
                return tag.OpenHtmlTag.Equals(OpenHtmlTag) && tag.CloseHtmlTag.Equals(CloseHtmlTag);
            }
            else
            {
                return base.Equals(obj);
            }
        }
    }

    public sealed class HtmlTagBold : ITagToHtml
    {
        private TagBold _tagBold;

        public HtmlTagBold()
        {
            _tagBold = new TagBold();
        }

        override
        public string OpenHtmlTag { get { return "<b>"; } }

        override
        public string CloseHtmlTag { get { return "</b>"; } }

        override
        public ITag ITag { get { return _tagBold; } }
    }

    public sealed class HtmlTagColor : ITagToHtml
    {
        private TagColor _tagColor;

        public HtmlTagColor(string colorValue)
        {
            _tagColor = new TagColor(colorValue);
        }

        override
        public string OpenHtmlTag { get { return "<font color=\"" + _tagColor.ColorRegex +"\">"; } }

        override
        public string CloseHtmlTag { get { return "</font>"; } }

        override
        public ITag ITag { get { return _tagColor; } }
    }

    public sealed class HtmlTagNoProcess : ITagToHtml
    {
        public static readonly HtmlTagNoProcess Instance = new HtmlTagNoProcess();

        override
        public string OpenHtmlTag { get { return "<p>"; } }

        override
        public string CloseHtmlTag { get { return "</p>"; } }

        override
        public ITag ITag { get { return TagNoProcess.Instance; } }
    }
}
