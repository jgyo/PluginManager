using PluginManager.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PluginManager.Wpf.ViewModels
{
    public class AboutViewModel : ViewModel
    {
        private string programTitle;
        private string companyName;
        private string configuration;
        private string copyright;
        private string description;
        private string infoVersion;
        private string fileVersion;

        public AboutViewModel()
        {
            var assembly = Application.Current.MainWindow.GetType().Assembly;

            foreach (var item in assembly.CustomAttributes)
            {
                switch (item.AttributeType.Name)
                {
                    case "AssemblyProductAttribute":
                        var apa = assembly.GetCustomAttribute<AssemblyProductAttribute>();
                        programTitle = apa.Product;
                        break;
                    case "AssemblyCompanyAttribute":
                        var aca = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
                        companyName = aca.Company;
                        break;
                    case "AssemblyConfigurationAttribute":
                        var aka = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();
                        configuration = aka.Configuration;
                        break;
                    case "AssemblyCopyrightAttribute":
                        var acpa = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
                        copyright = acpa.Copyright;
                        break;
                    case "AssemblyDescriptionAttribute":
                        var ada = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
                        description = ada.Description;
                        break;
                    case "AssemblyInformationalVersionAttribute":
                        var aiva = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                        infoVersion = aiva.InformationalVersion;
                        break;
                    case "AssemblyFileVersionAttribute":
                        var afva = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
                        fileVersion = afva.Version;
                        break;
                    default:
                        break;
                }
            }
            
        }
        public string ProgramTitle
        {
            get
            {
                return programTitle;
            }
        }

        public string InfoVersion
        {
            get
            {
                return infoVersion;
            }
        }

        public string Copyright
        {
            get
            {
                return copyright;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
        }

        public string Configuration
        {
            get
            {
                return configuration;
            }
        }

        public string FileVersion
        {
            get
            {
                return fileVersion;
            }
        }

        public string CompanyName
        {
            get
            {
                return companyName;
            }
        }
    }
}
