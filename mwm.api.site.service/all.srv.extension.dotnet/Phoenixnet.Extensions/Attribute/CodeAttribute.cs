using System;

namespace Phoenixnet.Extensions.Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CodeAttribute : System.Attribute
    {
        public static readonly CodeAttribute Default = new CodeAttribute();

        public CodeAttribute() : this(string.Empty)
        {
        }

        public CodeAttribute(string code)
        {
            this.CodeValue = code;
        }

        public virtual string Code => CodeValue;

        protected string CodeValue { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            CodeAttribute other = obj as CodeAttribute;

            return (other != null) && other.Code == Code;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}