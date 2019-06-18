using System;
using System.Collections.Generic;
using Bridge.Html5;
using Bridge.Navigation.Model;

namespace Bridge.Navigation
{
    public class BridgeNavigatorWithRouting : BridgeNavigator
    {
        private readonly IBrowserHistoryManager _browserHistoryManager;

        public BridgeNavigatorWithRouting(INavigatorConfigurator configuration, IBrowserHistoryManager browserHistoryManager) : base(configuration)
        {
            _browserHistoryManager = browserHistoryManager;
            Window.OnPopState += e =>
            {
                var urlInfo = _browserHistoryManager.ParseUrl();
                this.NavigateWithoutPushState(string.IsNullOrEmpty(urlInfo.PageId) ? configuration.HomeId : urlInfo.PageId, urlInfo.Parameters);
            };
        }

        private void NavigateWithoutPushState(string pageId, Dictionary<string, object> parameters = null)
        {
            base.Navigate(pageId, parameters);
        }
        public override void Navigate(string pageId, Dictionary<string, object> parameters = null)
        {
            _browserHistoryManager.PushState(pageId,parameters);
            base.Navigate(pageId, parameters);
        }

        public override void InitNavigation()
        {
            var parsed = _browserHistoryManager.ParseUrl();

            if (string.IsNullOrEmpty(parsed.PageId))
                base.InitNavigation();
            else
            {
                base.EnableSpafAnchors();

                var page = this.Configuration.GetPageDescriptorByKey(parsed.PageId);
                if (page == null) throw new Exception($"Page not found with ID {parsed.PageId}");

                // if not null and evaluation is false fallback to home
                if (page.CanBeDirectLoad != null && !page.CanBeDirectLoad.Invoke())
                {
                    _browserHistoryManager.PushState(this.Configuration.HomeId);
                    this.NavigateWithoutPushState(this.Configuration.HomeId);
                }
                else
                    this.Navigate(parsed.PageId,parsed.Parameters);
            }
        }

        
     
        
    }
}