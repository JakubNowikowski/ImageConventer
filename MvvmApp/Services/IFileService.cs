using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MvvmApp.Services
{
	public interface IFileService
	{
		bool TryOpenDialog(out string filePath);
		void SaveDialog(ImageSource image, string filePath);
    }
}
