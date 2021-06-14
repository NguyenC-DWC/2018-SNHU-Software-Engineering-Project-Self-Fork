using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Financing
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App(){
            /*
            CultureInfo English = new CultureInfo("en-US");
            CultureInfo French = new CultureInfo("fr-FR");
            CultureInfo Italian = new CultureInfo("it-IT");
            CultureInfo Spanish = new CultureInfo("es-ES");

            French.NumberFormat.NumberDecimalSeparator = ",";
            Italian.NumberFormat.NumberDecimalSeparator = ",";


            */

            CultureInfo cultureSet = new CultureInfo(GetLanguage());

           Thread.CurrentThread.CurrentCulture = cultureSet;
           Thread.CurrentThread.CurrentUICulture = cultureSet;
            
        }

        public static void SetLanguage(string locale)
        {
            ConfigurationManager.AppSettings.Set("Locale", locale);

            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(locale);

            var oldWindow = Current.MainWindow;

            Current.MainWindow = new Login();
            Current.MainWindow.Show();

            oldWindow.Close();
        }

        public static string GetLanguage()
        {
            return ConfigurationManager.AppSettings.Get("Locale");
        }
    }
}
