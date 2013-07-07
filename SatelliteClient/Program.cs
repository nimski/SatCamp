using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatelliteClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Window());
            }
            catch (Exception e)
            {
                string message = "";
                Exception ex = e;
                while (ex != null)
                {
                    message = message + ex.Message + "  ";
                    ex = ex.InnerException;
                }
                MessageBox.Show("Error: " + message);
            }

        }
    }
}
