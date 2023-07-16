using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel: Conductor<object>, IHandle<LogOnEvent>
    {
        #region Fields
        private LoginViewModel _loginVM;
        private SalesViewModel _salesVM;

        private IEventAggregator _events;

        private ILoggedInUserModel _user;

        #endregion

        #region Constructor
        public ShellViewModel(IEventAggregator events, 
                              LoginViewModel loginVM,
                              SalesViewModel salesVM,
                              ILoggedInUserModel user
        )
        {
            _loginVM = loginVM;

            _events = events;
            _salesVM = salesVM;
            _user = user;
           

            _events.SubscribeOnPublishedThread(this); // Wires this instance to listening for events

            // Whenver we stop using this, the view model will go away.
            // It will never have information from a previous form
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }


        #endregion

        #region Properties
        public bool IsLoggedIn
        {
            get
            {
                bool output = false;

                if (string.IsNullOrWhiteSpace(_user.Token) == false)
                {
                    output = true;
                }

                return output;
            }
        }
        #endregion

            #region Methods
        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItemAsync(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
        public void ExitApplication()
        {
            TryCloseAsync();
        }
        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesVM);

            NotifyOfPropertyChange(() => IsLoggedIn);
        }
        #endregion
    }
}
