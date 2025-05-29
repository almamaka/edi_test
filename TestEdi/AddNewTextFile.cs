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
using FlaUI.Core.Definitions;

namespace TestEdi
{
    [TestClass]
    public sealed class AddNewTextFile : BaseTest
    {

        [TestInitialize]
        public void TestSetup()
        {
            scenario = feature?.CreateNode<Scenario>("Add new text file");
            scenario?.CreateNode<Given>("The application is launched and main window is opened");
        }

        [TestMethod]
        public void AddNewTextFileTest()
        {
            // When: User clicks the 'New' button
            var addNewFileButton = mainWindow!.FindFirstDescendant(
                cf!.ByAutomationId("New")
            )?.AsButton();

            if (addNewFileButton != null)
            {
                addNewFileButton.Click();
                scenario?.CreateNode<When>("User clicks the 'New' button to create a new file").Pass("Clicked successfully");
            }
            else
            {
                scenario?.CreateNode<When>("User clicks the 'New' button to create a new file").Fail("Button not found");
                Assert.Fail("Add New button not found");
            }

            // And: User types "Hello, Edi!"
            FlaUI.Core.Input.Keyboard.Type("Hello, Edi!");
            scenario?.CreateNode<And>("User types 'Hello, Edi!' into the new file").Pass("Typed text");

            // And: User clicks 'Save' menu
            var saveMenuItem = mainWindow!.FindFirstDescendant(cf!.ByName("Save"))?.AsButton();
            if (saveMenuItem != null)
            {
                saveMenuItem.Click();
                scenario?.CreateNode<And>("User clicks the 'Save' menu item").Pass("Clicked Save menu");
            }
            else
            {
                scenario?.CreateNode<And>("User clicks the 'Save' menu item").Fail("Save menu item not found");
                Assert.Fail("Save menu item not found");
            }

            // Then: Save dialog opens and user inputs file name
            var fileNameTextBox = Retry.WhileNull(() =>
                mainWindow!.FindFirstDescendant(
                 cf.ByControlType(ControlType.Edit)
                    .And(cf.ByName("Fájlnév:"))
                )?.AsTextBox(),
                TimeSpan.FromSeconds(5)
                ).Result;

            string fileName = "NewFile" + RandomGenerator.GenerateRandomString(3) + ".txt";
            if (fileNameTextBox != null)
            {
                fileNameTextBox.Enter(fileName);
                scenario?.CreateNode<And>($"User enters file name '{fileName}'").Pass("Entered file name");
            }
            else
            {
                scenario?.CreateNode<And>("User enters file name").Fail("File name textbox not found");
                Assert.Fail("File name textbox not found");
            }

            // And: User clicks 'Save' button to finalize
            var saveButton = mainWindow!.FindFirstDescendant(cf!.ByAutomationId("1").And(cf.ByControlType(ControlType.Button)))?.AsButton();
            if (saveButton != null)
            {
                saveButton.Click();
                scenario?.CreateNode<And>("User clicks the 'Save' button to save the file").Pass("Clicked Save button");
            }
            else
            {
                scenario?.CreateNode<And>("User clicks the 'Save' button to save the file").Fail("Save button not found");
                Assert.Fail("Save button not found");
            }

            var createdFile = mainWindow!.FindFirstDescendant(cf!.ByName(fileName))?.AsLabel();

            if (createdFile != null)
            {
                bool isVisible = !createdFile.IsOffscreen;
                scenario?.CreateNode<Then>("The file is successfully created").Pass("File created");
                Assert.IsTrue(isVisible, "The new file is created");
            }
            else
            {
                scenario?.CreateNode<Then>("The file is successfully created").Fail("File could not be created");
                Assert.Fail("The new file could not be created");
            }
        }



        //[TestMethod]
        //public void AddNewTextFileTest()
        //{


        //    var addNewFileButton = mainWindow!.FindFirstDescendant(
        //    cf!.ByAutomationId("New")
        //    )?.AsButton();

        //    addNewFileButton?.Click();

        //    FlaUI.Core.Input.Keyboard.Type("Hello, Edi!");

        //    var fileMenu = mainWindow!.FindFirstDescendant(cf!.ByName("File"))?.AsButton();
        //    var saveMenuItem = mainWindow!.FindFirstDescendant(cf!.ByName("Save"))?.AsButton();

        //    saveMenuItem?.Click();

        //    var fileNameTextBox = mainWindow!.FindFirstDescendant(
        //        cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit)
        //        .And(cf.ByName("Fájlnév:"))
        //        )?.AsTextBox();

        //    string fileName = "NewFile" + RandomGenerator.GenerateRandomString(3) + ".txt";
        //    fileNameTextBox?.Enter(fileName);

        //    //FlaUI.Core.Input.Keyboard.Type("newFile.txt");
        //    var saveButton = mainWindow!.FindFirstDescendant(cf!.ByName("Mentés"))?.AsButton();
        //    saveButton?.Click();



        //}

    }
}
