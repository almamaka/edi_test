using System.Diagnostics;
using AventStack.ExtentReports;
using System.Xml.Linq;
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
        private static ExtentReports? extent;
        private ExtentTest? test;

        public TestContext? TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            extent = ExtentReportManager.GetExtent();
        }

        [TestInitialize]
        public void Setup()
        {
            test = extent?.CreateTest(TestContext?.TestName);
            test?.Info("Test initializing");

            string solutionDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string exePath = Path.Combine(solutionDir, @"Release\Edi.exe");

            if (!File.Exists(exePath))
            {
                throw new FileNotFoundException($"Could not find Edi.exe at: {exePath}");
            }

            application = Application.Launch(new ProcessStartInfo
            {
                FileName = exePath,
                WorkingDirectory = Path.GetDirectoryName(exePath),
                UseShellExecute = true
            });

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

            if (descriptionLabel != null)
            {
                test?.Log(Status.Pass, "Description label found successfully.");
            }
            else
            {
                test?.Log(Status.Fail, "Description label was not found.");
            }

            Assert.IsNotNull(descriptionLabel, "Description label was not found");
        }

        [TestCleanup]
        public void Cleanup()
        {
            test?.Info("Cleaning up");
            if (application != null && !application.HasExited)
            {
                application.Close();
            }
            automation?.Dispose();
            test?.Pass("Cleanup completed");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            extent?.Flush();
        }
    }
}
