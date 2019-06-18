using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bridge.Html5;
using Bridge.jQuery2;

namespace Bridge.Navigation
{
    /// <summary>
    /// INavigator implementation
    /// </summary>
    public class BridgeNavigator : INavigator
    {
        private static IAmLoadable _actualController;

        protected readonly INavigatorConfigurator Configuration;
        public BridgeNavigator(INavigatorConfigurator configuration)
        {
            Configuration = configuration;
        }

        public void EnableSpafAnchors()
        {
            var allAnchors = jQuery.Select("a");
            allAnchors.Off(EventType.Click.ToString());
            allAnchors.Click(ev =>
            {
                var clickedElement = ev.Target;

                if (clickedElement.GetType() != typeof(HTMLAnchorElement))
                    clickedElement = jQuery.Element(ev.Target).Parents("a").Get(0);

                var href = clickedElement.GetAttribute("href");

                if (string.IsNullOrEmpty(href)) return;

                var isMyHref = href.StartsWith("spaf:");

                // if is my href
                if (isMyHref)
                {
                    ev.PreventDefault();
                    var pageId = href.Replace("spaf:", "");
                    this.Navigate(pageId);
                }

                // anchor default behaviour
            });
        }

        /// <summary>
        /// Navigate to a page ID.
        /// The ID must be registered.
        /// </summary>
        /// <param name="pageId"></param>
        public virtual void Navigate(string pageId, Dictionary<string,object> parameters = null)
        {
            var page = this.Configuration.GetPageDescriptorByKey(pageId);
            if (page == null) throw new Exception($"Page not found with ID {pageId}");
            
            // check redirect rule
            var redirectKey = page.RedirectRules?.Invoke();
            if (!string.IsNullOrEmpty(redirectKey))
            {
                this.Navigate(redirectKey,parameters);
                return;
            }

            var body = this.Configuration.Body;
            if(body == null)
                throw new Exception("Cannot find navigation body element.");
            
            // leave actual controlelr
            if (this.LastNavigateController != null)
                this.LastNavigateController.OnLeave();

            this.Configuration.Body.Load(page.HtmlLocation.Invoke(),null, async (o,s,a) =>
            {
                // load dependencies
                if (page.DependenciesScripts != null)
                {
                    var scripts = (page.DependenciesScripts.Invoke()).ToList();
                    if(page.SequentialDependenciesScriptLoad)
                        Utility.SequentialScriptLoad(scripts);
                    {
                        // parallel load
                        var scriptsTask = scripts.Select(url => Task.FromPromise(jQuery.GetScript(url)));
                        await Task.WhenAll(scriptsTask);
                    }
                    
                }
                
                // prepare page
                page.PreparePage?.Invoke();

                // auto enable spaf anchors
                if (!this.Configuration.DisableAutoSpafAnchorsOnNavigate)
                {
                    var enableAnchors = page.AutoEnableSpafAnchors?.Invoke();
                    if(enableAnchors.HasValue && enableAnchors.Value)
                        this.EnableSpafAnchors();
                }

                if (page.PageController != null)
                {
                    // load new controller
                    var controller = page.PageController();
                    controller.OnLoad(parameters);

                    _actualController = controller;
                    
                    this.OnNavigated?.Invoke(this,controller);
                }
                
            }); 
        }

        public event EventHandler<IAmLoadable> OnNavigated;
        public IAmLoadable LastNavigateController => _actualController;

        /// <summary>
        /// Subscribe to anchors click
        /// </summary>
        public virtual void InitNavigation()
        {
            this.EnableSpafAnchors();

            // go home
            this.Navigate(this.Configuration.HomeId);
        }

       
    }
}