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
using TestEdi.Support;
using TestEdi.ScreenViews;

namespace TestEdi
{
    [TestClass]
    public sealed class AddNewTextFile : BaseTest
    {
        private MainWindowView? mainView;

        [TestInitialize]
        public void TestSetup()
        {
            feature = extent?.CreateTest<Feature>("Edi Application - Add files");
            mainView = new MainWindowView(mainWindow!, cf!);
        }

        [TestMethod]
        public void AddNewTextFileTest()
        {
            scenario = feature?.CreateNode<Scenario>("Add new text file");
            scenario?.CreateNode<Given>("The application is launched and main window is opened");

            mainView!.ClickOn(mainView!.NewFileButton, "Given", scenario, "User clicks on 'New' button", "New button clicked.", "Button not found or cannot be clicked.");

            string text = "This is an example file text with random strings after this sentence: " + RandomGenerator.GenerateRandomString(40);
            FlaUI.Core.Input.Keyboard.Type(text);
            scenario?.CreateNode<And>("User types into the empty file").Pass("Typed text");

            mainView!.ClickOn(mainView!.SaveMenuButton, "And", scenario, "User clicks on 'Save'menu item", "Save menu item clicked. Save dialog opened.", "Menu item not found or cannot be clicked.");

            string fileName = "NewFile" + RandomGenerator.GenerateRandomString(3) + ".txt";
            mainView!.TypeText(mainView!.FileNameInput, fileName, "And", scenario, $"User types in {fileName} into 'File name' field", "File name added to input field", "File name field not found");

            mainView!.ClickOn(mainView.ConfirmSaveButton, "And", scenario, "User clicks 'Save' to finalize", "Save button clicked", "Save button not found or cannot be clicked");

            var createdFile = mainView.FindFileLabel(fileName);
            mainView!.AssertElementVisible(createdFile, scenario, "Then", "The file is successfully created", "The new file is successfully created", "File could not be created");


            mainView!.ClickOn(mainView!.CloseIcon, "And", scenario, "User clicks the Close tab button", "Opened file successfully closed", "Close icon not found or cannot be clicked");

        }

        [TestMethod]
        public void SetupDefaultFileNameTest()
        {
            scenario = feature?.CreateNode<Scenario>("Add new text file");
            scenario?.CreateNode<Given>("The application is launched and main window is opened");

            mainView!.ClickOn(mainView.ToolsMenuItem, "When", scenario, "User clicks on Tools menu", "Tools menuitem clicked", "Tools menuitem not found");
            mainView!.ClickOn(mainView.SettingsButton, "And", scenario, "User clicks on Settings menuitem", "Settings menuitem clicked", "Settings menuitem not found");

            var defaultFileNameTextField = mainWindow!.FindFirstDescendant(cf!.ByControlType(ControlType.Edit)).AsTextBox();
            var defaultFileName = "ExampleFile";
            mainView.TypeText(defaultFileNameTextField, defaultFileName, "And", scenario, "User modifies default file name", "Filename modified", "Default file name field not found");

            mainView.ClickOn(mainView!.OKButton, "And", scenario, "User clicks on OK button", "OK button clicked", "OK button not found");
            
            mainView!.ClickOn(mainView!.NewFileButton, "Then", scenario, "User clicks on 'New' button", "New button clicked.", "Button not found or cannot be clicked.");
            var newFile = mainView.FindFileLabel(defaultFileName + ".txt");
            mainView!.AssertElementVisible(newFile, scenario, "Then", "The file is successfully created", "The new file is successfully created", "File could not be created");
            mainView!.ClickOn(mainView!.CloseIcon, "And", scenario, "User clicks the Close tab button", "New file successfully closed", "Close icon not found or cannot be clicked");
            
            mainView!.SetDefaultFileNameBackToUntitled();

        }

    }
}
