using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.ViewModels;

namespace WpfApp2.Models
{
    public class TestData:BaseViewModel
    {

		private ushort low;

		public ushort MyProperty
		{
			get { return low; }
			set
			{
				low = value;
				this.RaiseProperChanged(nameof(MyProperty));
			}
		}

	}
}
