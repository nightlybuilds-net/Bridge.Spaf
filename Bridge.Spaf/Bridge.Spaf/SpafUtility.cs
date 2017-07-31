using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;
using Bridge.jQuery2;

namespace Bridge.Spaf
{
    public static class SpafUtility
    {
        /// <summary>
        /// Get HtmlElement by id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetElementById<T>(string id) where T : HTMLElement
        {
            return (T)jQuery.Select($"#{id}").Get(0);
        }

        /// <summary>
        /// Get Template to a HtmlTemplate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static T GetTemplate<T>(string templateId) where T : HTMLElement
        {
            var template = jQuery.Select($"#{templateId}").Html().Trim();
            var singleElement = jQuery.ParseHTML(template).Single();
            return (T)singleElement;
        }
    }
}
