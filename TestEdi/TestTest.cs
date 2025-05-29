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
    public sealed class TestTest : BaseTest
    {

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

    }
}
