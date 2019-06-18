using System.Collections.Generic;
using Bridge.jQuery2;

namespace Bridge.Navigation
{
    public interface INavigatorConfigurator
    {
        /// <summary>
        /// Element used as Page body
        /// </summary>
        jQuery Body { get; }

        /// <summary>
        /// Home page 
        /// </summary>
        string HomeId { get; }

        /// <summary>
        /// Create page routes
        /// </summary>
        /// <returns></returns>
        IList<IPageDescriptor> CreateRoutes(); 

        /// <summary>
        /// Get a pagedescriptor from pageid
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IPageDescriptor GetPageDescriptorByKey(string key);
        
        
        /// <summary>
        /// Disable auto enable spaf anchors on navigate
        /// </summary>
        bool DisableAutoSpafAnchorsOnNavigate { get; }
    }
}