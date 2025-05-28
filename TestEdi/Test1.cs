using FlaUI.UIA3;

namespace TestEdi
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string path = @"Release\Edi.exe";
            string basedir = AppDomain.CurrentDomain.BaseDirectory;
            string path2 = Directory.GetParent(basedir).Parent.Parent.Parent.Parent.ToString();

            var application = FlaUI.Core.Application.Launch(Path.Combine(path2, path));

            var mainWindow = application.GetMainWindow(new UIA3Automation());
            
        }
    }
}
