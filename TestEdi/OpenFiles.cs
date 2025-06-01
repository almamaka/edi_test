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
using FlaUI.Core.AutomationElements.Infrastructure;
using TestEdi.Support;
using TestEdi.ScreenViews;
using AventStack.ExtentReports.Gherkin;
using System.Windows;

namespace TestEdi
{
    [TestClass]
    public sealed class OpenFiles : BaseTest
    {
        private MainWindowView? mainView;

        [TestInitialize]
        public void TestSetup()
        {
            feature = extent?.CreateTest<Feature>("Edi Application - Open files");
            mainView = new MainWindowView(mainWindow!, cf!);
            
        }

        [TestMethod]
        public void OpenExistingFileTest()
        {
            scenario = feature?.CreateNode<Scenario>("Open existing text file");
            scenario?.CreateNode<Given>("The application is launched and main window is opened");

            string fileName = "NewFile" + RandomGenerator.GenerateRandomString(3) + ".txt";
            string text = "This is an example file text with random strings after this sentence: " + RandomGenerator.GenerateRandomString(40);
            mainView!.AddNewFile(scenario, "And", "A new file is created", "There is an existing file", "New file does not exist", fileName, text);



            mainView!.ClickOn(mainView.OpenFileButton, "And", scenario, "User clicks the 'Open' button", "Open file dialog opened", "Open button not found or cannot be clicked");

            mainView!.TypeText(mainView!.FileNameInput, fileName, "And", scenario, $"User enters {fileName} for opening file", "File name typed", "File name field not found");

            mainView!.ClickOn(mainView.ConfirmOpenButton, "And", scenario, "User clicks 'Open' to finalize", "Open button clicked", "Open button not found or cannot be clicked");

            var reopenedFile = mainView.FindFileLabel(fileName);
            mainView!.AssertElementVisible(reopenedFile, scenario, "Then", "The file is successfully created", "The new file is successfully created", "File could not be created");

            mainView!.ClickOn(mainView!.CloseIcon, "And", scenario, "User closes the reopened file", "Opened file successfully closed", "Close icon not found or cannot be clicked");

        }
    }
}
