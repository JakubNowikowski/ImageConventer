using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmApp.ViewModels
{
	public interface IOService
	{
		string OpenFileDialog(string defaultPath);
		Stream OpenFile(string path);
	}
}
