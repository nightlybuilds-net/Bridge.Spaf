using System.Collections.Generic;
using Bridge.Html5;
using Bridge.Navigation.Model;

namespace Bridge.Navigation
{
    public class ComplexObjectNavigationHistory : IBrowserHistoryManager
    {
        public void PushState(string pageId, Dictionary<string, object> parameters = null)
        {
            var baseUrl = NavigationUtility.BuildBaseUrl(pageId);

            Window.History.PushState(null, string.Empty,
                parameters != null
                    ? $"{baseUrl}={Global.Btoa(JSON.Stringify(parameters))}" : baseUrl);
        }

        public UrlDescriptor ParseUrl()
        {
            var res = new UrlDescriptor();

            var hash = Window.Location.Hash;
            hash = hash.Replace("#", "");

            if (string.IsNullOrEmpty(hash)) return res;

            var equalIndex = hash.IndexOf('=');
            if (equalIndex == -1)
            {
                res.PageId = hash;
                return res;
            }

            res.PageId = hash.Substring(0, equalIndex);  

            var doublePointsIndx = equalIndex + 1;
            var parameters = hash.Substring(doublePointsIndx, hash.Length - doublePointsIndx);

            if (string.IsNullOrEmpty(parameters)) return res; // no parameters

            var decoded = Global.Atob(parameters);
            var deserialized = JSON.Parse<Dictionary<string, object>>(decoded);

            res.Parameters = deserialized;
            
            return res;
        }
    }
}