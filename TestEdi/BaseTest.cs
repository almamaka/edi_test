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
            try
            {

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

                application.WaitWhileMainHandleIsMissing(TimeSpan.FromSeconds(10));
                Thread.Sleep(300);

                if (application.HasExited)
                {
                    throw new Exception($"Edi process exited immediately after launch (PID {application.ProcessId})");
                }

                automation = new UIA3Automation();
                cf = automation.ConditionFactory;

               
                mainWindow = Retry.WhileNull(() =>
                {
                    try
                    {
                        return application.GetMainWindow(automation);
                    }
                    catch (ArgumentException)
                    {
                        return null;
                    }
                }, TimeSpan.FromSeconds(10)).Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        [ClassCleanup(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void ClassCleanup()
        {
            extent?.Flush();
        }
    }
}
