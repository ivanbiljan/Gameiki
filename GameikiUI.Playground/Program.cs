using System;

namespace GameikiUI.Playground {
    internal static class Program {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            // Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new Test());
            using (var game = new Game()) {
                game.Run();
            }
        }
    }
}