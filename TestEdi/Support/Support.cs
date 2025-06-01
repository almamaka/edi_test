using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaUI.Core.Definitions;
using System.Xml.Linq;

namespace TestEdi.Support
{
    internal class Support : BaseTest
    {
        public void WriteOutAllElementsOnWindow() {
        var allElements = mainWindow!.FindAllDescendants();
            foreach (var element in allElements)
            {
                var controlType = element.ControlType;
                var name = element.Name;

                string automationId;
                try
                {
                    automationId = element.AutomationId;
                }
                catch
                {
                    automationId = "(not supported)";
                }

                Console.WriteLine($"Element: {controlType} | Name: {name} | AutomationId: {automationId}");
            }
        }

        public void WriteOutAllWindows()
        {
            var allWindows = automation!.GetDesktop().FindAllChildren();
            foreach (var win in allWindows)
            {
                Console.WriteLine(win.Name);
            }
        }

        public string SaveWindowScreenshot(IntPtr windowHandle)
        {
            try
            {
                using var screenshot = FlaUI.Core.Capturing.Capture.Screen();
                var fileName = $"screenshot_{DateTime.Now:yyyyMMdd_HHmmssfff}.png";
                string solutionDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string screenshotPath = Path.Combine(solutionDir, "ScreenShots", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath));


                screenshot.ToFile(screenshotPath);
                return screenshotPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Screenshot failed: " + ex.Message);
                return string.Empty;
            }
        }
    }
}
