using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bridge.Html5;
using Bridge.Navigation.Model;

namespace Bridge.Navigation
{
    public class QueryParameterNavigationHistory : IBrowserHistoryManager
    {
        public void PushState(string pageId, Dictionary<string, object> parameters = null)
        {
            var baseUrl = NavigationUtility.BuildBaseUrl(pageId);

            Window.History.PushState(null, string.Empty,
                parameters != null
                    ? $"{baseUrl}{BuildQueryParameter(parameters)}" : baseUrl);
        }

        public UrlDescriptor ParseUrl()
        {
            var res = new UrlDescriptor();
            res.Parameters = new Dictionary<string, object>();

            var hash = Window.Location.Hash;
            hash = hash.Replace("#", "");

            if (string.IsNullOrEmpty(hash)) return res;

            var equalIndex = hash.IndexOf('?');
            if (equalIndex == -1)
            {
                res.PageId = hash;
                return res;
            }

            res.PageId = hash.Substring(0, equalIndex);  

            var doublePointsIndx = equalIndex + 1;
            var parameters = hash.Substring(doublePointsIndx, hash.Length - doublePointsIndx);

            if (string.IsNullOrEmpty(parameters)) return res; // no parameters

            
            var splittedByDoubleAnd = parameters.Split("&").ToList();
            splittedByDoubleAnd.ForEach(f =>
            {
                var splitted = f.Split("=");
                res.Parameters.Add(splitted[0],Global.DecodeURIComponent(splitted[1]));
            });

            return res;
        }

        private string BuildQueryParameter(Dictionary<string, object> parameters)
        {
            if (parameters == null || !parameters.Any()) return string.Empty;
            
            var strBuilder = new StringBuilder("?");
            foreach (var keyValuePair in parameters)
            {
                strBuilder.Append(Global.EncodeURIComponent(keyValuePair.Key));
                strBuilder.Append("=");
                strBuilder.Append(Global.EncodeURIComponent(keyValuePair.Value.ToString()));
                strBuilder.Append("&");
            }

            var res = strBuilder.ToString().TrimEnd('&');
            
            return res;

        }

    }
}