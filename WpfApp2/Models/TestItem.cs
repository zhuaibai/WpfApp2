using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.ViewModels;

namespace WpfApp2.Models
{
    public class TestItem:BaseViewModel
    {
		private int _Id;

		public int Id
		{
			get { return _Id; }
			set
			{
				_Id = value;
				this.RaiseProperChanged(nameof(Id));
			}
		}

		private string _name;

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				this.RaiseProperChanged(nameof(Name));
			}
		}
		private bool _IsImportant;

		public bool IsImportant
		{
			get { return _IsImportant; }
			set
			{
				_IsImportant = value;
				this.RaiseProperChanged(nameof(IsImportant));
			}
		}
		private int flag =0;

		public int Flag
		{
			get { return flag; }
			set
			{
				flag = value;
				this.RaiseProperChanged(nameof(Flag));
			}
		}


	}
}
