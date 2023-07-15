using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel: Conductor<object>, IHandle<LogOnEvent>
    {
        #region Fields
        private LoginViewModel _loginVM;
        private SalesViewModel _salesVM;

        private IEventAggregator _events;
        private SimpleContainer _container;
        #endregion

        #region Constructor
        public ShellViewModel(IEventAggregator events, SimpleContainer container, LoginViewModel loginVM,
                              SalesViewModel salesVM)
        {
            _loginVM = loginVM;

            _events = events;
            _salesVM = salesVM;
            _container = container;

            _events.SubscribeOnPublishedThread(this); // Wires this instance to listening for events

            // Whenver we stop using this, the view model will go away.
            // It will never have information from a previous form
            ActivateItemAsync(_container.GetInstance<LoginViewModel>());
        }


        #endregion

        #region Methods
        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesVM);
        }
        #endregion
    }
}
