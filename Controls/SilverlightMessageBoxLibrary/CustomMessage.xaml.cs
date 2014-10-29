using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace TKS.Controls
{
     partial class CustomMessage : ChildWindow
    {
        /// <summary>
        /// Types of message boxes that can be displayed.
        /// </summary>
        public enum MessageType { Info, Error, Confirm,Warn, TextInput, ComboInput,Action };

        /// <summary>
        /// The string result of any input taking message box (i.e. TextInput and ComboInput).
        /// </summary>
        public string Input { get; set; }

        private Exception _ex;

        /// <summary>
        /// The path to the icons. Note the name of the namespace inside the path.
        /// </summary>
        private const string ICONS_PATH = "/TKS.Controls.MessageBoxLibrary;component/icons/";

        public  CustomMessage(string message, MessageType type = MessageType.Info,Exception ex=null , List<string> inputOptions = null)
        {
            InitializeComponent();

            switch (type)
            {

                case MessageType.Info:
                    //The message is already set up as an info message box.
                    //No need for change.
                    break;

                case MessageType.TextInput:
                case MessageType.ComboInput:

                    //Put the text part of the message on the top so it won't 
                    //interfere with the input box.
                    this.TextBlock.VerticalAlignment = VerticalAlignment.Top;

                    //Modify the margin around the textblock to make it more suitable.
                    Thickness newBorderMargin = this.TextBlock.Margin ;
                    newBorderMargin.Top += 5;
                    this.TextBlock.Margin = newBorderMargin;

                    //Depending on the type of input, make either the textbox or
                    //the combobox visible. 
                    if (type == MessageType.ComboInput)
                    {
                        this.InputComboBox.ItemsSource = inputOptions;

                        //This is optional; Selects the first item in the combo box,
                        //if the combo options are not empty.
                        
                        if (inputOptions != null && inputOptions.Count!= 0)
                            this.InputComboBox.SelectedIndex = 0;
                        

                        this.InputComboBox.Visibility = Visibility.Visible;
                    }
                    else //TextBox input.
                    {
                        this.InputTextBox.Visibility = Visibility.Visible;
                    }
                    this.OKButton.Content = "确定";
                    this.CancelButton.Content = "取消";
                    this.CancelButton.Visibility = Visibility.Visible;
                    break;

                case MessageType.Error:
                    setMessageIcon("Error.png");
                    this.MessageBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    this.OKButton.Content = "确定";
                    this.DetailButton.Content = "详细";
                    this.DetailButton.Visibility = Visibility.Visible;
                    _ex = ex;
                    break;
                case MessageType.Confirm:
                    setMessageIcon("Question.png");
                    this.MessageBorder.BorderBrush = new SolidColorBrush(Colors.Orange);
                    this.OKButton.Content = "是";
                    this.CancelButton.Content = "否";
                    this.CancelButton.Visibility = Visibility.Visible;
                    break;
                case MessageType.Warn :
                    setMessageIcon("Exclamation.png");
                    this.MessageBorder.BorderBrush = new SolidColorBrush(Colors.Yellow);
                    this.OKButton.Content = "是";
                    this.CancelButton.Content = "否";
                    this.CancelButton.Visibility = Visibility.Visible;
                    break;
                case MessageType.Action:
                    setMessageIcon("Exclamation.png");
                    this.MessageBorder.BorderBrush = new SolidColorBrush(Colors.Yellow);
                    this.OKButton.Content = "确认";
                    this.CancelButton.Visibility = Visibility.Collapsed;
                    break;

            }

            //Set the message.
            this.TextBlock.Text = message;
        }

        /// <summary>
        /// Sets the image of the custom message button given the name of the image.
        /// </summary>
        /// <param name="imagePath">The name of the image to set.</param>
        private void setMessageIcon(string imagePath)
        {
            MessageIcon.Source = new BitmapImage(new Uri(ICONS_PATH + imagePath, UriKind.RelativeOrAbsolute));
        }

        #region Button Hanlders

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            //Check to see if the input text box was visiable
            //and that at least some text was entered.
            if (this.InputTextBox.Visibility == Visibility.Visible
                && this.InputTextBox.Text.Length > 0)

                //Store the text in the textbox input into Input property.
                Input = InputTextBox.Text;

            //Else check to see if the input combo box was visible.
            else if (this.InputComboBox.Visibility == Visibility.Visible
                    && this.InputComboBox.SelectedItem != null)

                //Store the selected value.
                Input = (String)InputComboBox.SelectedValue;

            //If no input was received, set the Input property to null
            //so users can check if the input was empty.
            else
                Input = null;

            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #endregion

        #region Keyboad Event Handlers

        private void keyDown(object sender, KeyEventArgs e)
        {
            //Click the OK button if enter was pressed on the textbox.
            if (e.Key == Key.Enter)
            {
                // Create a ButtonAutomationPeer using the ClickButton.
                ButtonAutomationPeer buttonAutoPeer = new ButtonAutomationPeer(OKButton);

                // Create an InvokeProvider.
                IInvokeProvider invokeProvider = buttonAutoPeer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;

                // Invoke the default event, which is click for the button.
                invokeProvider.Invoke();
            }
        }

        #endregion

        private void DetailButton_Click_1(object sender, RoutedEventArgs e)
        {
            DetailMessage detail = null;
            if (_ex == null)
            {
                _ex = new Exception("Exception is null.");
                 
            }

            detail = new DetailMessage(_ex);
            detail.Show();
        }

    }
}

