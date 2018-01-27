using System;
using System.Linq;
using System.Reflection;
using Bridge;
using Bridge.Ioc;
using Bridge.Messenger;
using Bridge.Navigation;
using Bridge.Spaf.Attributes;

namespace Bridge.Spaf
{
    public class SpafApp
    {
        public static IIoc Container;

        public static void Main()
        {
            Container = new BridgeIoc();
            ContainerConfig(); // config container
            Container.Resolve<INavigator>().InitNavigation(); // init navigation

        }

        private static void ContainerConfig()
        {
            // navigator
            Container.RegisterSingleInstance<INavigator, BridgeNavigatorWithRouting>();
            Container.Register<INavigatorConfigurator, CustomRoutesConfig>(); 

            // messenger
            Container.RegisterSingleInstance<IMessenger, Messenger.Messenger>();

            // viewmodels
            RegisterAllViewModels();

            // register custom resource, services..

        }

        #region PAGES IDS
        // static pages id


        public static string HomeId => "home";
       
        #endregion

        #region MESSAGES
        // messenger helper for global messages and messages ids

        public static class Messages
        {
            public class GlobalSender { };

            public static GlobalSender Sender = new GlobalSender();

            public static string LoginDone => "LoginDone";

        }


        #endregion

        /// <summary>
        /// Register all types that end with "viewmodel".
        /// You can register a viewmode as Singlr Instance adding "SingleInstanceAttribute" to the class
        /// </summary>
        private static void RegisterAllViewModels()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .Where(w => w.Name.ToLower().EndsWith("viewmodel")).ToList();

            types.ForEach(f =>
            {
                var attributes = f.GetCustomAttributes(typeof(SingleInstanceAttribute), true);

                if (attributes.Any())
                    Container.RegisterSingleInstance(f);
                else
                    Container.Register(f);
            });

        }
    }
}
