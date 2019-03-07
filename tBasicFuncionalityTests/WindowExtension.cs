using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.Factory;
using TestStack.White.UIItems.WindowItems;

namespace tBasicFuncionalityTests
{
    public static class WindowExtension
    {
        public static Window GetAppWindow(out Application application, string applicationPath, string windowName)
        {
            application = Application.Launch(applicationPath); //launch application
            return application.GetWindow(windowName, InitializeOption.NoCache); //"PlusCalculator"
        }

        public static void SetTextBox(this Window window, string textBoxName, string valutToSet)
        {
            var textBox = window.Get<TextBox>(textBoxName); //get text box in window
            textBox.Text = valutToSet; //set value to getted textbox
        }

        public static string GetTextBoxValue(this Window window, string textBoxName)
        {
            var textBox = window.Get<TextBox>(textBoxName); //get element with results
            return textBox.Text; //get result text
        }

        public static void ClickButton(this Window window, string buttonName)
        {
            var button = window.Get<Button>(buttonName); //get button to perform calculation
            button.Click(); //perform calculation
        }
    }
}
