using System.Windows.Media;

namespace MvvmApp.Services
{
    public interface IFileService
	{
        #region Methods

        bool TryOpenDialog(out string filePath);
		void SaveDialog(ImageSource image, string filePath);
        
        #endregion
    }
}
