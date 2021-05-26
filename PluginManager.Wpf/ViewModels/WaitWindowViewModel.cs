using PluginManager.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager.Wpf.ViewModels
{
    public class WaitWindowViewModel : ViewModel
    {

        private string windowTitle;

        public string WindowTitle
        {
            get { return windowTitle; }
            set { SetProperty(ref windowTitle, value); }
        }


        private string text;

        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }


        private string description;
        private bool isIndeterminate = true;

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        public bool IsIndeterminate
        {
            get => isIndeterminate;
            set => SetProperty(ref isIndeterminate, value);
        }

    }
}
