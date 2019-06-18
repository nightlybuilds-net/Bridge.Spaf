using System;
using System.Collections.Generic;
using System.Linq;
using Bridge.jQuery2;

namespace Bridge.Navigation
{
    /// <summary>
    /// INavigatorConfigurator Implementation. Must be extended.
    /// </summary>
    public abstract class BridgeNavigatorConfigBase : INavigatorConfigurator
    {
        private readonly IList<IPageDescriptor> _routes;

        public abstract IList<IPageDescriptor> CreateRoutes();
        public abstract jQuery Body { get; }
        public abstract string HomeId { get; }
        public abstract bool DisableAutoSpafAnchorsOnNavigate { get; }



        protected BridgeNavigatorConfigBase()
        {
            this._routes = this.CreateRoutes();
        }

        public IPageDescriptor GetPageDescriptorByKey(string key)
        {
            return this._routes.SingleOrDefault(s=> string.Equals(s.Key, key, StringComparison.CurrentCultureIgnoreCase));
        }

    }
}