using PluginManager.Core.Commands;
using PluginManager.Core.Logging;
using PluginManager.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PluginManager.Wpf.ViewModels
{
    public class LogViewModel : ViewModel
    {

        private string logText = String.Empty;
        private string filePath;

        public LogViewModel()
        {
            filePath = FileLog.FilePath;
            if(File.Exists(filePath))
                LogText = File.ReadAllText(filePath);
            IsModified = false;

            SaveLogCommand = new Command(SaveLog);
        }

        private void SaveLog()
        {
            File.WriteAllText(filePath, LogText);
            IsModified = false;
        }


        private bool isModified;

        public bool IsModified
        {
            get { return isModified; }
            set { SetProperty(ref isModified, value); }
        }

        public string LogText
        {
            get { return logText; }
            set
            {
                if (SetProperty(ref logText, value))
                    IsModified = true;
            }
        }

        public ICommand SaveLogCommand { get; }
    }
}
