namespace Calypso
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainWindow());
        }

        //static Mutex mutex = new Mutex(true, "<some_guid_or_unique_name>");

        //[STAThread]
        //static void Main()
        //{
        //    if (mutex.WaitOne(TimeSpan.Zero, true))
        //    {
        //        ApplicationConfiguration.Initialize();
        //        Application.Run(new MainWindow());

        //        // release mutex after the form is closed.
        //        mutex.ReleaseMutex();
        //        mutex.Dispose();
        //    }
        //}
    }
}