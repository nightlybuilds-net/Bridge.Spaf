using System.Collections.Generic;
using System.Linq;
using Bridge.jQuery2;

namespace Bridge.Navigation
{
    public static class Utility
    {
        /// <summary>
        /// Load script sequentially
        /// </summary>
        /// <param name="scripts"></param>
        public static void SequentialScriptLoad(List<string> scripts)
        {
            if (!scripts.Any()) return;
            var toLoad = scripts.First();
            jQuery.GetScript(toLoad, (o, s, arg3) =>
            {
                scripts.Remove(toLoad);
                SequentialScriptLoad(scripts);
            });
        }
    }
}