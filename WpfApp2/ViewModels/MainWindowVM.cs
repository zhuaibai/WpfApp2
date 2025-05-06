using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfApp2.UserControls;

namespace WpfApp2.ViewModels
{
    public class MainWindowVM:BaseViewModel
    {
		public MainWindowVM()
		{

		}

		private UserControl _ContentControl;

		public UserControl ContentControl
		{
			get {
				if(_ContentControl == null)
				{
					_ContentControl = new SetProtolViewUC();
				}
				return _ContentControl; }
			set
			{
				_ContentControl = value;
				this.RaiseProperChanged(nameof(ContentControl));
			}
		}

	}
}
