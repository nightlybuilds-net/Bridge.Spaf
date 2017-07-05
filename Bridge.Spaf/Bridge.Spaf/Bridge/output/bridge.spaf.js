/**
 * @version 1.0.0.0
 * @copyright Copyright ©  2017
 * @compiler Bridge.NET 15.7.0
 */
Bridge.assembly("Bridge.Spaf", function ($asm, globals) {
    "use strict";

    Bridge.define("Bridge.Spaf.SpafUtility", {
        statics: {
            /**
             * Get HtmlElement by id
             *
             * @static
             * @public
             * @this Bridge.Spaf.SpafUtility
             * @memberof Bridge.Spaf.SpafUtility
             * @param   {Function}    T     
             * @param   {string}      id
             * @return  {T}
             */
            getElementById: function (T, id) {
                return Bridge.cast($(System.String.format("#{0}", id)).get(0), T);
            },
            /**
             * Get Template to a HtmlTemplate
             *
             * @static
             * @public
             * @this Bridge.Spaf.SpafUtility
             * @memberof Bridge.Spaf.SpafUtility
             * @param   {Function}    T             
             * @param   {string}      templateId
             * @return  {T}
             */
            getTemplate: function (T, templateId) {
                var template = $(System.String.format("#{0}", templateId)).html().trim();
                var singleElement = System.Linq.Enumerable.from($.parseHTML(template)).single();
                return Bridge.cast(singleElement, T);
            }
        }
    });
});
