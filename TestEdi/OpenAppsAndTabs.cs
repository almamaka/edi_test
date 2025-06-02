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
    public sealed class OpenAppsAndTabs : BaseTest
    {
        private MainWindowView? mainView;

        [TestInitialize]
        public void TestSetup()
        {
            feature = extent?.CreateTest<Feature>("Edi Application - Opening pages and app");
            mainView = new MainWindowView(mainWindow!, cf!);
            
        }


        [TestMethod]
        public void OpenRecentFilesDialogTest()
        {
            scenario = feature?.CreateNode<Scenario>("Open Recent Files tab");
            scenario?.CreateNode<Given>("The application is launched and main window is opened");

            mainView!.ClickOn(mainView.RecentFilesTabButton, "When", scenario, "User clicks on Recent files tab", "Recent files tab opened successfully", "Recent files tab not found");
            
            var support = new Support.Support();
            string screenShotPath = support.SaveWindowScreenshot(mainWindow!.Properties.NativeWindowHandle);
            scenario?.CreateNode<Then>($"Recent files tab opened and screenshot captured as: {screenShotPath}");


        }

        [TestMethod]
        public void OpenEdi()
        {
            scenario = feature?.CreateNode<Scenario>("Open Edi Application");
            scenario?.CreateNode<When>("Tests started runniing");

            mainView!.AssertElementVisible(mainView!.EdiIcon, scenario, "Then", "Edi launched successfully and main window is opened", "Edi successfully opened", "Edi cannot be opened");

        }

        [TestMethod]
        public void OpenStartPage()
        {
            scenario = feature?.CreateNode<Scenario>("Open Start Page");
            scenario?.CreateNode<Given>("The application is launched and main window is opened");
            
            mainView!.ClickOn(mainView.ViewMenuItem, "When", scenario, "User clicks on View menu", "View menuitem clicked", "View menuitem not found");
            mainView!.ClickOn(mainView.StartPageButton, "And", scenario, "User clicks on start page menuitem", "Start page menuitem clicked", "Start page menuitem not found");

            var startPageLabel = mainView!.FindFileLabel("Start Page");
            mainView!.AssertElementVisible(startPageLabel, scenario, "Then", "Start page opened sucessfully", "The start page successfully opened", "Start page cannot be opened");
            
            mainView!.ClickOn(mainView!.CloseIcon, "And", scenario, "User clicks the Close tab button", "Opened file successfully closed", "Close icon not found or cannot be clicked");

        }

    }
}
