using System.Collections.Generic;
using Bridge.Navigation.Model;

namespace Bridge.Navigation
{
    public interface IBrowserHistoryManager
    {
        void PushState(string pageId, Dictionary<string, object> parameters = null);
        UrlDescriptor ParseUrl();
    }
}