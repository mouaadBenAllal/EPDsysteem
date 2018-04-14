/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/
namespace EpdFirefly
{
    using System.Linq;
    using System.Windows;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {
        /// <summary>
        ///     Helper function, Checks if window is open
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>

        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
                ? Current.Windows.OfType<T>().Any()
                : Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }
    }
}