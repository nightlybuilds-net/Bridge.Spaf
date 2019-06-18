using System.Collections.Generic;

namespace Bridge.Navigation.Model
{
    public class UrlDescriptor
    {
        public string PageId { get; set; }

        public Dictionary<string, object> Parameters { get; set; }
    }
}