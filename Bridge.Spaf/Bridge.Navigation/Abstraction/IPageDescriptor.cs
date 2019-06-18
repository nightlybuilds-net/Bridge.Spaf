using System;
using System.Collections.Generic;

namespace Bridge.Navigation
{
    public interface IPageDescriptor
    {
        /// <summary>
        /// Page Key
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// Html page location
        /// </summary>
        Func<string> HtmlLocation { get; set; }

        /// <summary>
        /// Page Controller
        /// </summary>
        Func<IAmLoadable> PageController { get; set; }

        /// <summary>
        /// If null can be direct loaded
        /// else evaluate func. If false auto redirect to home
        /// </summary>
        /// <returns></returns>
        Func<bool> CanBeDirectLoad { get; set; }

        /// <summary>
        /// Define redirect rule for this descriptor.
        /// If null or string empty => no redirect rules.
        /// Return the KEY of a new page descriptor.
        /// </summary>
        Func<string> RedirectRules { get; set; }

        /// <summary>
        /// Automatic enable spaf anchors on load.
        /// Default is True
        /// </summary>
        Func<bool> AutoEnableSpafAnchors { get; set; }
        
        /// <summary>
        /// Add list of scripts necessary for partial page
        /// </summary>
        Func<IEnumerable<string>> DependenciesScripts { get; set; }
        
        /// <summary>
        /// Action for prepare page.
        /// This is called AFTER dependencies script (if defined) load done
        /// </summary>
        Action PreparePage { get; set; }
        
        /// <summary>
        /// If true dependenciesscript will be loaded sequentially instead then parallel
        /// </summary>
        bool SequentialDependenciesScriptLoad { get; set; }
    }
}