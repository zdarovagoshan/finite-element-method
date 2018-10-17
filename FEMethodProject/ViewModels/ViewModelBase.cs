using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;

namespace FEMethodProject.ViewModels
{
	public class ViewModelBase : ReactiveObject
	{
		public string Title 
		{
			get;
			set;
		}
	}
}
