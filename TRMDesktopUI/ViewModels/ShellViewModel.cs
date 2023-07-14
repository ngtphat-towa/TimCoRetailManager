using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel: Conductor<object>
    {
        #region Fields
        private LoginViewModel _loginVM;
        #endregion

        #region Constructor
        public ShellViewModel(LoginViewModel loginVM)
        {
            _loginVM = loginVM;
            ActivateItemAsync(_loginVM);
        }
        #endregion

        #region Methods
        #endregion
    }
}
