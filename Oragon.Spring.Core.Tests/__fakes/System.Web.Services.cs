

using System.Security.Permissions;

namespace System.Web.Services
{
    public class _ { }

    public class WebServiceAttribute : Attribute
    {
        public string Namespace { get; set; }
        public string Name { get; set; }
    }

    public class WebPermissionAttribute : Attribute
    {
        private SecurityAction deny;

        public WebPermissionAttribute(SecurityAction deny)
        {
            this.deny = deny;
        }
    }

    public class WebMethodAttribute : Attribute
    {
        public bool? EnableSession { get; set; }
        public double CacheDuration { get; set; }
        public string Description { get; set; }
        public bool TransactionOption { get; set; }
    }

    public class TransactionOption
    {
        public static bool Required { get; set; }
    }
}