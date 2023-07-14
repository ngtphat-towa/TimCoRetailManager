using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        #region Private Fields
        // TODO: Remove hard-coded username and password before deploying to production.
        // These values are for manual testing only.
        private string _userName = "ngtphat.towa@gmail.com";
        private string _password = "Pwd12345.";
        #endregion

        #region Properties
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public bool CanLogIn
        {
            get
            {
                bool output = false;
                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    output = true;
                }

                return output;

            }

        }
        #endregion

        #region Methods
        public void LogIn()
        {

        }
        #endregion

        #region Private Methods
       
        #endregion
    }
}
