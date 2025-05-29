using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using AventStack.ExtentReports;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using AventStack.ExtentReports.Gherkin.Model;

namespace TestEdi
{
    public class BaseTest
    {
        protected Application? application;
        protected UIA3Automation? automation;
        protected ConditionFactory? cf;
        protected Window? mainWindow;
        protected static ExtentReports? extent;
        protected ExtentTest? test;
        protected ExtentTest? feature;
        protected ExtentTest? scenario;

        public TestContext? TestContext { get; set; }

        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void ClassInit(TestContext context)
        {
            extent = ExtentReportManager.GetExtent();
        }

        [TestInitialize]
        public void Setup()
        {
            feature = extent?.CreateTest<Feature>("Edi Application - File Management");
            feature?.Info("Feature initializing");

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

            feature?.Pass("Application launched and main window opened");
        }

        [TestCleanup]
        public void Cleanup()
        {
            feature?.Info("Cleaning up");
            if (application != null && !application.HasExited)
            {
                application.Close();
            }
            automation?.Dispose();
            feature?.Pass("Cleanup completed");
        }

        [ClassCleanup(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void ClassCleanup()
        {
            extent?.Flush();
        }
    }
}
