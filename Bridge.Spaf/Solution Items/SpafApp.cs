using Bridge.Ioc;
using Bridge.Ioc.Abstract;
using Bridge.Messenger;
using Bridge.Navigation.Abstraction;
using Bridge.Navigation.Impl;

namespace Bridge.Spaf
{
    public class SpafApp
    {
        public static IIoc Container;

        public static void Main()
        {
            Container = new BridgeIoc();

            ContainerConfig();

            Container.Resolve<INavigator>().InitNavigation();

        }

        private static void ContainerConfig()
        {
            // navigator
            Container.RegisterSingleInstance<INavigator, BridgeNavigatorWithRouting>();
            //Container.Register<INavigatorConfigurator, YourExtendOf_BridgeNavigatorConfigBase>(); // todo

            // messenger
            Container.RegisterSingleInstance<IMessenger, Messenger.Messenger>();

            // todo register your controllers, services, resources....

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
    }
}
