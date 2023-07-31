using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel: Conductor<object>, IHandle<LogOnEvent>
    {
        #region Fields
        private IEventAggregator _events;
        private ILoggedInUserModel _user;
        private IAPIHelper _apiHelper;

        #endregion

        #region Constructor
        public ShellViewModel(IEventAggregator events,
                              ILoggedInUserModel user,
                              IAPIHelper apiHelper
        )
        {


            _events = events;
     
            _user = user;
            _apiHelper = apiHelper;

            _events.SubscribeOnPublishedThread(this); // Wires this instance to listening for events

            // Whenver we stop using this, the view model will go away.
            // It will never have information from a previous form
            ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
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
            _user.ResetUserModel();
            _apiHelper.LogOffUser();
            ActivateItemAsync(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
        public void UserManagement()
        {
            ActivateItemAsync(IoC.Get<UserDisplayViewModel>(), new CancellationToken());
        }
        public void ExitApplication()
        {
            TryCloseAsync();
        }
        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);

            NotifyOfPropertyChange(() => IsLoggedIn);
        }
        #endregion
    }
}
