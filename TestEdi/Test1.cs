using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Tools;
using FlaUI.UIA3;

namespace TestEdi
{
    [TestClass]
    public sealed class Test1
    {
        private Application? application;
        private UIA3Automation? automation;
        private ConditionFactory? cf;
        private Window? mainWindow;

        [TestInitialize]
        public void Setup()
        {
            string path = @"Release\Edi.exe";
            string basedir = AppDomain.CurrentDomain.BaseDirectory;
            string path2 = Directory.GetParent(basedir).Parent.Parent.Parent.Parent.ToString();

            application = FlaUI.Core.Application.Launch(Path.Combine(path2, path));

            automation = new UIA3Automation();

            cf = automation.ConditionFactory;
            mainWindow = Retry.WhileNull(() => application.GetMainWindow(automation), TimeSpan.FromSeconds(10)).Result;
            if (mainWindow == null)
            {
                throw new Exception("Main window was not found after waiting.");
            }
        }

        [TestMethod]
        public void FirstTest()
        {

            Assert.IsNotNull(mainWindow, "Main window was not found");
            Assert.IsNotNull(cf, "ConditionFactory was not initialized");

            //! is used because in setup() it is sure it is not null
            var descriptionLabel = mainWindow!.FindFirstDescendant(
                cf!.ByAutomationId("PART_WindowTitleThumb")
            )?.AsLabel();

            Assert.IsNotNull(descriptionLabel, "Description label was not found");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (application != null && !application.HasExited)
            {
                application.Close();
            }
            automation?.Dispose();
        }
    }
}
