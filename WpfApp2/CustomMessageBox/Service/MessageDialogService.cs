using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.CustomMessageBox.Models;
using WpfApp2.CustomMessageBox.ViewModels;

namespace WpfApp2.CustomMessageBox.Service
{
    public class MessageDialogService : IMessageDialogService
    {
        public MessageResult Show(string message, string title = "系统提示",
                                MessageIcon icon = MessageIcon.Information,
                                double fontSize = 18)
        {
            var model = new MessageDialogModel
            {
                Message = message,
                Title = title,
                Icon = icon,
                FontSize = fontSize
            };

            var view = new MessageDialogView();
            var viewModel = new MessageDialogViewModel(model, view);
            view.DataContext = viewModel;

            view.ShowDialog();

            return viewModel.Result;
        }
    }
}
