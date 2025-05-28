using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace TestEdi
{
    public static class ExtentReportManager
    {
        private static ExtentReports _extent;
        private static ExtentSparkReporter _htmlReporter;

        public static ExtentReports GetExtent()
        {
            if (_extent == null)
            {
                string solutionDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
                string reportPath = Path.Combine(solutionDir, "TestResults", "ExtentReport.html");
                Directory.CreateDirectory(Path.GetDirectoryName(reportPath));

                _htmlReporter = new ExtentSparkReporter(reportPath);
                _htmlReporter.Config.DocumentTitle = "Test Execution Report";
                _htmlReporter.Config.ReportName = "EDI UI Automation";

                _extent = new ExtentReports();
                _extent.AttachReporter(_htmlReporter);
            }
            return _extent;
        }
    }
}
