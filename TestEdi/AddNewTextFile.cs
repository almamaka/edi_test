using System.Diagnostics;
using AventStack.ExtentReports;
using System.Xml.Linq;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using TestEdi.Helpers;
using AventStack.ExtentReports.Gherkin.Model;

namespace TestEdi
{
    [TestClass]
    public sealed class AddNewTextFile : BaseTest
    {

        [TestMethod]
        public void AddNewTextFileTest()
        {

            var addNewFileButton = mainWindow!.FindFirstDescendant(
            cf!.ByAutomationId("New")
            )?.AsButton();

            addNewFileButton?.Click();

            FlaUI.Core.Input.Keyboard.Type("Hello, Edi!");

            var fileMenu = mainWindow!.FindFirstDescendant(cf!.ByName("File"))?.AsButton();
            var saveMenuItem = mainWindow!.FindFirstDescendant(cf!.ByName("Save"))?.AsButton();

            saveMenuItem?.Click();

            var fileNameTextBox = mainWindow!.FindFirstDescendant(
                cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit)
                .And(cf.ByName("Fájlnév:"))
                )?.AsTextBox();

            string fileName = "NewFile" + RandomGenerator.GenerateRandomString(3) + ".txt";
            fileNameTextBox?.Enter(fileName);

            //FlaUI.Core.Input.Keyboard.Type("newFile.txt");
            var saveButton = mainWindow!.FindFirstDescendant(cf!.ByName("Mentés"))?.AsButton();
            saveButton?.Click();



        }

    }
}
