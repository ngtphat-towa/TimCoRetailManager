using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TRMDesktopUI.Helpers;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.ViewModels;

namespace TRMDesktopUI
{
    public class Bootstrapper : BootstrapperBase
    {
        /// <summary>
        /// Read this document
        /// https://caliburnmicro.com/documentation/simple-container
        /// https://caliburnmicro.com/documentation/Tutorials/WPF/SimpleContainer
        /// https://caliburnmicro.com/documentation/configuration
        /// </summary>
        private SimpleContainer _container = new SimpleContainer();
        public Bootstrapper()
        {
            Initialize();
            ConventionManager.AddElementConvention<PasswordBox>(
            PasswordBoxHelper.BoundPasswordProperty,
            "Password",
            "PasswordChanged");
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewForAsync<ShellViewModel>();
        }

        protected override void Configure()
        {
            // Configures the container for the application.

            // Set the instance of the container
            _container.Instance(_container)
              .PerRequest<IProductEndpoint, ProductEndpoint>()
              .PerRequest<ISaleEndpoint,SaleEndpoint>();

            // Register the WindowManager and EventAggregator as singletons
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>()
                .Singleton<IConfigHelper,ConfigHelper>()
                .Singleton<IAPIHelper, APIHelper>();

            // Get all types in the assembly that are classes and end with "ViewModel"
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                // Register each ViewModel type per request
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));
        }
        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

    }
}