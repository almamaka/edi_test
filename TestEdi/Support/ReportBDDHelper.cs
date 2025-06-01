using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports;
using FlaUI.Core.AutomationElements;
using AventStack.ExtentReports.Gherkin;
using System.Xml.Linq;
using System.Windows;

namespace TestEdi.Support
{
    internal class ReportBDDHelper
    {
        public static ExtentTest createThenStep(ExtentTest scenario, string description) 
        {
            return scenario?.CreateNode<Then>(description)!;
        }

        public static ExtentTest createAndStep(ExtentTest scenario, string description)
        {
            return scenario?.CreateNode<And>(description)!;
        }

        public static ExtentTest createWhenStep(ExtentTest scenario, string description)
        {
            return scenario?.CreateNode<When>(description)!;
        }

        public static ExtentTest createGivenStep(ExtentTest scenario, string description)
        {
            return scenario?.CreateNode<Given>(description)!;
        }

        public static ExtentTest chooseStepType(string gherkinStep, ExtentTest scenario, string description) 
        {
            ExtentTest newStep;

            switch (gherkinStep)
            {
                case "Given":
                    newStep = ReportBDDHelper.createGivenStep(scenario, description);
                    break;
                case "When":
                    newStep = ReportBDDHelper.createWhenStep(scenario, description);
                    break;
                case "Then":
                    newStep = ReportBDDHelper.createWhenStep(scenario, description);
                    break;
                default:
                    newStep = ReportBDDHelper.createAndStep(scenario, description);
                    break;
            }
            return newStep;
        }

        public static void AreEqual(ExtentTest scenario, string stepDescription, string expected, string actual)
        {
            var node = scenario.CreateNode<AventStack.ExtentReports.Gherkin.Model.Then>(stepDescription)
                               .Info($"Expected: {expected}")
                               .Info($"Actual: {actual}");

            if (expected == actual)
            {
                node.Pass("Matched successfully.");
            }
            else
            {
                node.Fail("Values did not match.");
            }
        }

        public static void IsTrue(ExtentTest scenario, string stepDescription, bool condition, string successMessage = "Condition is true.", string failureMessage = "Condition is false.")
        {
            var node = scenario.CreateNode<AventStack.ExtentReports.Gherkin.Model.Then>(stepDescription);
            if (condition)
            {
                node.Pass(successMessage);
            }
            else
            {
                node.Fail(failureMessage);
            }
        }
    }
}
