using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Gherkin;
using AventStack.ExtentReports;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using FlaUI.Core.AutomationElements.Infrastructure;
using FlaUI.Core.Tools;
using TestEdi.Support;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using System.Security.RightsManagement;


namespace TestEdi.ScreenViews
{
    public class MainWindowView
    {
        private readonly Window _window;
        private readonly ConditionFactory _cf;

        public MainWindowView(Window window, ConditionFactory cf)
        {
            _window = window;
            _cf = cf;
        }

        public AutomationElement EdiIcon => _window.FindFirstDescendant(_cf!.ByAutomationId("PART_Icon"));
        public Button NewFileButton => _window.FindFirstDescendant(_cf.ByAutomationId("New"))?.AsButton();

        public Button SaveMenuButton => _window.FindFirstDescendant(_cf.ByName("Save"))?.AsButton();

        public Button OpenFileButton => _window.FindFirstDescendant(_cf.ByAutomationId("PART_ActionButton"))?.AsButton();

        public Button ViewMenuItem => _window.FindFirstDescendant(_cf!.ByName("View").And(_cf.ByControlType(ControlType.MenuItem)))?.AsButton();

        public Button ToolsMenuItem => _window.FindFirstDescendant(_cf!.ByName("Tools").And(_cf.ByControlType(ControlType.MenuItem)))?.AsButton();
        public Button RecentFilesTabButton => _window.FindFirstDescendant(_cf!.ByName("Recent Files").And(_cf.ByControlType(ControlType.Text))).AsButton();

        public Button OKButton => _window.FindFirstDescendant(_cf!.ByName("OK")).AsButton();
        public AutomationElement RecentFilesPanel => _window.FindFirstDescendant(_cf!.ByAutomationId("Not Supported").And(_cf.ByControlType(ControlType.Pane)));
        public TextBox FileNameInput => Retry.WhileNull(() =>
                _window!.FindFirstDescendant(
                 _cf.ByControlType(ControlType.Edit)
                    .And(_cf.ByName("Fájlnév:"))
                )?.AsTextBox(),
                TimeSpan.FromSeconds(5)
                ).Result;

        public Button ConfirmSaveButton => _window.FindFirstDescendant(_cf.ByAutomationId("1")
            .And(_cf.ByControlType(ControlType.Button)))?.AsButton();

        public Button ConfirmOpenSplitButton => _window.FindFirstDescendant(_cf.ByAutomationId("1")
            .And(_cf.ByControlType(ControlType.SplitButton)))?.AsButton();

        public Button ConfirmOpenButton => _window.FindFirstDescendant(_cf.ByAutomationId("1")
            .And(_cf.ByControlType(ControlType.Button)))?.AsButton();

        public Button CloseIcon => _window.FindFirstDescendant(_cf.ByAutomationId("DocumentCloseButton"))?.AsButton();

        public Button StartPageButton => _window.FindFirstDescendant(_cf!.ByName("Start Page").And(_cf.ByControlType(ControlType.MenuItem)))?.AsButton();

        public Button SettingsButton => _window.FindFirstDescendant(_cf!.ByName("Settings...").And(_cf.ByControlType(ControlType.MenuItem)))?.AsButton();

        public AutomationElement StartPageEdiText => _window.FindFirstChild(_cf.ByName("Edi").And(_cf.ByControlType(ControlType.Text)));
        public AutomationElement TodayGroup => _window.FindFirstDescendant(_cf!.ByName("Today").And(_cf.ByControlType(ControlType.Group)));

        public AutomationElement FindFileLabel(string fileName) =>
            _window.FindFirstDescendant(_cf.ByName(fileName));

        public void ClickOn(AutomationElement element, string testStep, ExtentTest scenario, string description, string passText, string failText) 
        {
            if (element != null)
            {
                element.Click();
                ReportBDDHelper.chooseStepType(testStep, scenario, description).Pass(passText);
            }
            else
            {
                ReportBDDHelper.chooseStepType(testStep, scenario, description).Fail(passText);
                Assert.Fail($"Element not found or not a button: {description}");
            }
        }

        public void TypeText(TextBox? textBox, string textToType, string testStep, ExtentTest scenario, string description, string passText, string failText)
        {
            if (textBox != null)
            {
                textBox.Enter(textToType);
                ReportBDDHelper.chooseStepType(testStep, scenario, description).Pass(passText);
            }
            else
            {
                ReportBDDHelper.chooseStepType(testStep, scenario, description).Fail(passText);
                Assert.Fail("TextBox not found");
            }
        }

        public void AssertElementVisible(AutomationElement? element, ExtentTest scenario, string testStep, string description, string passText, string failText)
        {
            if (element != null && !element.IsOffscreen)
            {
                ReportBDDHelper.chooseStepType(testStep, scenario, description).Pass(passText);
                Assert.IsTrue(true);
            }
            else
            {
                ReportBDDHelper.chooseStepType(testStep, scenario, description).Fail(passText);
                Assert.Fail("Element not visible");
            }
        }

        public void AddNewFile(ExtentTest scenario, string testStep, string description, string passText, string failText, string fileName, string textToType) 
        {
            NewFileButton.Click();
            FlaUI.Core.Input.Keyboard.Type(textToType);
            SaveMenuButton.Click();
            FileNameInput.Enter(fileName);
            ConfirmSaveButton.Click(); ;
            AssertElementVisible(FindFileLabel(fileName), scenario, testStep, description, passText, failText);
            CloseIcon.Click();

        }

        public void SetDefaultFileNameBackToUntitled()
        {
            ToolsMenuItem.Click();
            SettingsButton.Click();
            var defaultFileNameTextField = _window.FindFirstDescendant(_cf!.ByControlType(ControlType.Edit)).AsTextBox();
            defaultFileNameTextField.Enter("Untitled");
            OKButton.Click();
        }
    }
}
