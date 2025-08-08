using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.CustomMessageBox.Service
{
    public interface IMessageDialogService
    {
        MessageResult Show(string message, string title = "系统提示",
                          MessageIcon icon = MessageIcon.Information,
                          double fontSize = 18);
    }
}
