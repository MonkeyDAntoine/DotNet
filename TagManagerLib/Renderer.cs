using System;
using System.Text.RegularExpressions;

namespace TagManagerLib
{
    /*
     * Renderer to tranform text with custom tag
     * 
     * */
    public interface Renderer
    {
        string Render(SyntaxTree tree);
    }

    /*
     * Custom tags
     * 
     * */
    public abstract class ITag
    {
        public abstract string OpenTag { get; }
        public abstract string CloseTag { get; }

        public override sealed int GetHashCode()
        {
            return OpenTag.GetHashCode()*CloseTag.GetHashCode();
        }

        public override sealed bool Equals(object obj)
        {
            if (obj is ITag)
            {
                ITag tag = (ITag)obj;
                return tag.OpenTag.Equals(OpenTag) && tag.CloseTag.Equals(CloseTag);
            }
            else
            {
                return base.Equals(obj);
            }
        }
    }

    public sealed class TagNoProcess : ITag
    {
        public static readonly TagNoProcess Instance = new TagNoProcess();
        override
        public string OpenTag { get { return "#{"; } }
        
        override
        public string CloseTag { get { return "}#"; } }
    }

    public sealed class TagBold : ITag
    {
        override
        public string OpenTag { get { return "B{"; } }
        
        override
        public string CloseTag { get { return "}"; } }
    }

    public sealed class TagColor : ITag
    {
        private string _colorRegex;

        public string ColorRegex { get { return _colorRegex; } }
        
        override
        public string OpenTag { get { return "C[" + _colorRegex + "]{"; } }
        
        override
        public string CloseTag { get { return "}"; } }

        public TagColor(string colorValue)
        {
            _colorRegex = colorValue;
        }
    }
}
