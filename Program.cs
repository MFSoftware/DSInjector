using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DSInjector
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            FileStream fileStream = File.Create(Path.GetTempPath() + "injector_path_228.txt");
            byte[] bytes = Encoding.Default.GetBytes(Directory.GetCurrentDirectory());
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run((Form) new Main_Form());
        }
    }
}
