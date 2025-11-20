using System;
using System.Windows;

namespace MoniraceWPF
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            // 1. Crea l'applicazione WPF
            Application app = new Application();

            // 2. (Opzionale) Se avevi risorse globali, potresti aggiungerle qui, 
            // ma per ora la tua MainWindow ha già le risorse che servono.

            // 3. Crea la finestra principale
            MainWindow window = new MainWindow();

            // 4. Avvia l'applicazione con quella finestra
            app.Run(window);
        }
    }
}