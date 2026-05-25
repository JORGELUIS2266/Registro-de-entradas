using System;
using System.Windows.Forms;

namespace proyecto
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool loggedOut = false;
            do
            {
                loggedOut = false;
                using (Login loginForm = new Login())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        using (Form1 mainForm = new Form1())
                        {
                            Application.Run(mainForm);
                            if (mainForm.DialogResult == DialogResult.Retry)
                            {
                                loggedOut = true;
                            }
                        }
                    }
                }
            } while (loggedOut);
        }
    }
}
