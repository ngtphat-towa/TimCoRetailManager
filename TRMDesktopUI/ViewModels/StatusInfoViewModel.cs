using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.ViewModels
{
    public class StatusInfoViewModel: Screen
    {

        #region Properties
        public string Header { get; private set; }
        public string Message { get; private set; }
        #endregion


        #region Methods

        public void UpdateMessage(string header, string message)
        {
            Header = header;
            Message = message;

            NotifyOfPropertyChange(() => Header);
            NotifyOfPropertyChange(() => Message);
        }

        public void Close()
        {
            TryCloseAsync();
        } 
        #endregion
    }
}
