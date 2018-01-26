# Bridge.Spaf #

## Sharpie Single Page Application Framework 
### The first S is mute (quote of Django)

This README would normally document whatever steps are necessary to get your Bridge .NET Single Page Application up and running.

QUICK STARTUP:

* [youtube video](https://youtu.be/Eid0-XTVaeI) on how setup a solution
* [youtube video](https://youtu.be/RtlkrF2qTn8) on how setup a viewmodels.
* See demo application (Work in progress)

## Table of Contents ##

1. [What is this repository for?](#introduction)
2. [How do I get set up?](#setup)
3. [Configuration](#config)
	1. [IoC Registering](#ioc)
	2. [Navigation Config](#navigationConfig) 	
3. [Navigation](#navigation)
4. [Sending messages](#messenger)
5. [Defining UI](#ui) (WIP)
6. [MVVM Binding](#binding)


### What is this repository for? <a name="introduction"></a>

* Spaf is a framework with which to develop front-end single page applications with the support of C# to Javascript [Bridge .NET](http://bridge.net/) compiler and utilities like [Bridge.Navigation](https://bitbucket.org/markjackmilian/bridge.navigation), [Bridge.Messenger](https://github.com/markjackmilian/Bridge.Messenger) and the IoC container [Bridge.Ioc](https://bitbucket.org/markjackmilian/bridge.ioc).
* Now Spaf is wrapping Knockout libraries to make and easy-peasy wiew definition base on a MVVM like architecture!
* Thanks to [Bridge.EasyTests](https://github.com/markjackmilian/Bridge.EasyTests), now you are able to create your own unit test. 

* Version 0.0.1

* [Learn MarkJackMilian projects](https://bitbucket.org/markjackmilian/)

### How do I get set up? <a name="setup"></a>



* To set up a SPA with Spaf you'll need two projects:
    1. An WEB project;
    2. A Class Library project

* Install Bridge.Spaf Package on Class Library Project

```
Install Package Bridge.Spaf
```
    

* Your Class Library will automatically include the following packages:
    1. Bridge.NET
    2. Bridge.Jquery
    3. Bridge.Ioc
    3. Bridge.Navigation
    4. Bridge.Messenger
    6. Retyped
    7. Retyped.Knockout
    8. Bridge.Spaf

Two classes will be added to your project:

	* SpafApp.cs 
		the entry point of you app that expose IOC container, pages IDs, messages.
        Auto Register NavigationService and all viewmodels
	* CustomRoutesConfig.cs
		a basic custom navigation configuration

### Configuration <a name="config"></a>
* IoC Registering  <a name="ioc"></a>

#### Bridge.IOC API:

```
public interface IIoc
    {
        void RegisterFunc<TType>(Func<TType> func);
        void Register(Type type, IResolver resolver);
        void Register<TType, TImplementation>() where TImplementation : class, TType;
        void Register<TType>() where TType : class;
        void Register(Type type);
        void Register(Type type, Type impl);
        void RegisterSingleInstance<TType, TImplementation>() where TImplementation : class, TType;
        void RegisterSingleInstance<TType>() where TType : class;
        void RegisterSingleInstance(Type type);
        void RegisterSingleInstance(Type type, Type impl);
        void RegisterInstance<TType>(TType instance);
        void RegisterInstance(Type type, object instance);
        void RegisterInstance(object instance);
        TType Resolve<TType>() where TType : class;
        object Resolve(Type type);
    }
```
 
SpafApp will auto register all types that end with "viewmodel". The auto registration will register as transient by default. You can change this behaviour simply adding [SingleInstance] attribute to you viewmodel.

See Bridge.IOC test runner [HERE](http://tests.markjackmilian.net/bridgeioc/) 

HERE a SpafApp.cs file

```
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
```

With this registration:
```
Container.RegisterSingleInstance<INavigator, BridgeNavigatorWithRouting>();
```
spafapp is registering the Navigation service with routing, so browser address will change and back/forward browser button are enabled. If you don't need routing you can register the navigator as follows:

```
Container.RegisterSingleInstance<INavigator, BridgeNavigator>();
```

* Navigation Config  <a name="navigationConfig"></a>

The NavigationConfig class explains to the SPA how to retrieve html pages location and controller through a list of PageDescriptor.

Page Descriptor class:
```
public class PageDescriptor : IPageDescriptor
    {
        public string Key { get; set; }
        public Func<string> HtmlLocation { get; set; }
        public Func<IAmLoadable> PageController { get; set; }
        public IEnumerable<string> JsDependencies { get; set; }

        public Func<bool> CanBeDirectLoad { get; set; }
        public Action PreparePage { get; set; }
    }
```

A page descriptor describes each page of you spaf application. A page is described by a Key (must be univoque id for a page), html location and a PageController.
CanBeDirectLoad => useful only with NavigationWithRouting.. tell to navigation service if a page can be directly navigated wit a full url, if false navigation service will redirect to home page.

It also defines which page is the "Homepage" and which is the SPA main body jquery element through Body and HomeId properties. 


```
class CustomRoutesConfig : BridgeNavigatorConfigBase
    {
        public override IList<IPageDescriptor> CreateRoutes()
        {
            return new List<IPageDescriptor>
            {
                new PageDescriptor
                {
                    CanBeDirectLoad = ()=>true,
                    HtmlLocation = ()=>"pages/home.html", // yout html location
                    Key = SpafApp.HomeId,
                    PageController = () => SpafApp.Container.Resolve<HomeViewModel>()
                },               

            };
        }

        public override jQuery Body { get; } = jQuery.Select("#pageBody");
        public override string HomeId { get; } = SpafApp.HomeId;
    }
```

### Navigation <a name="navigation"></a>


SPAF is able to Navigate from one page to an other using its Navigator. You have to pass to the Navigate() method the target page id, defined by the Key property of its PageDescriptor.

Navigator API:
```
public interface INavigator
    {
        IAmLoadable LastNavigateController { get; }

        /// <summary>
        /// Init the navigation. THis will subscribe to all anchors click too
        /// HRef anchor is spaf:XXX
        /// </summary>
        void InitNavigation();

        /// <summary>
        /// Enable href as spaf:pageID
        /// </summary>
        void EnableSpafAnchors();

        /// <summary>
        /// Navigate to a pageid
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="parameters"></param>
        void Navigate(string pageId, Dictionary<string, object> parameters = null);
    }
```


```
using Bridge.Navigation.Abstraction;
namespace FrontendBridge.Controllers
{
    
    public class HomeController : LoadableViewModels
    {
        private readonly INavigator navigator;

        public HomeController(INavigator navigator)
        {
            this.navigator = navigator;
            
			this.MyObj = new Obj
            {
                OnEventFired = id =>
                {
                    this.navigator.Navigate("test", new Dictionary<string, object>
                    {
                        { "id" , id }
                    });
                }
            };
        }
	}
}
```

Auto Navigation is managed by SpafApp by anchor like:

```
<a href="spaf:pageID">Another Page</a>
```
On startup those anchors are automatically enabled. You can manually enable those anchors using navigator.EnableSpafAnchors();

### Sending messages <a name="messenger"></a>
Spaf is able to manage various messaging queues in order to ensure classes communication (typically between ViewModels). This is possible thanks to the Publisher-Subscriber architecture.
You can send a message to a Receiver calling the Send() method, passing informations like Sender, message and arguments (with can contain data to share).
```
using Bridge.Messenger;
namespace FrontendBridge
{
	[Reflectable]	
	public class MyViewModel : ViewModelBase
	{
    	private string myId = "myId";
		private Button myButton = JQuery.Select($"#{_myId}")
		
        myButton.Click() += (sender, e)
        {
        	Messenger.Send<MyViweModel, bool>(this, "message", true);
        };
        
    }
}

```

Classes that want to receive messages can subscribe to the type of Sender and to the a specific message calling the Subscribe() method, in order to retrieve data from Sender.

```
using Bridge.Messenger;
namespace FrontendBridge
{
	public class MyViewModel2 : LoadableViewModel
	{
    	private bool data;
        
    	public MyViewModel2()
        {
        	Messenger.Subscribe<MyViewModel, bool>(this, "message", (vm, data) =>
            {
            	this.data = bool;	
            });
        }
        
    }
}
```
Eventually classes can Unsubscribe from messaging queues calling the Unsibscribe() method.

```
using Bridge.Messenger;
namespace FrontendBridge
{
	public class MyViewModel2 : LoadableViewModel
	{
    	private bool data;
        
    	public MyViewModel2()
        {
        	Messenger.Subscribe<MyViewModel, bool>(this, "message", (vm, data) =>
            {
            	this.data = bool;	
            });
        }
        
        public Dispose()
        {
        	Messanger.Unsubscribe("message", typeof(MyViewModel), typeof(bool), this);
        }
    }
}
```

### Defining UI <a name="ui"></a>


### MVVM <a name="binding"></a>

You can see this [video](https://youtu.be/RtlkrF2qTn8 on how setup a viewmodels.
SPAF use [Knockooutjs](http://www.knockoutjs.com) as MVVM library.

EXAMPLE:
in you view you can use Knockout notations:
```
<p data-bind="text: Example"></p>
<button data-bind="click: TestClick">Test Button</button>
```

ViewModel:
Example property is defined as KnockoutObservable and is double-way binded to your view.
```
class SecondViewModel : LoadableViewModel
    {
        protected override string ElementId() => SpafApp.SecondId;

        public KnockoutObservable<string> Example { get; set; }

        public SecondViewModel()
        {
            this.Example = ko.observable.Self<string>();
        }
        
        public void TestClick()
        {
        	// called when button is clicked
        }


        public override void OnLoad(Dictionary<string, object> parameters)
        {
            base.OnLoad(parameters);
            var example = parameters.GetParameter<string>("example");
            this.Example.Self($"Hello {example}");
        }

        
    }
```

See demo app for deeper usage and list management.
