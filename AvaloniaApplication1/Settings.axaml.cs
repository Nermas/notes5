using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using IniParser;
using IniParser.Model;
using Avalonia.Controls.ApplicationLifetimes;

namespace notes5
{
    public partial class Settings : Window
    {
        private ComboBox FontComboBox;
        private TextBox FontSizeTextBox;

        public Settings()
        {
            InitializeComponent();
            LoadSystemFonts();
            var app = (App)Application.Current;
            var parser = new FileIniDataParser();
            var iniPath = "config.ini";
            IniData data = parser.ReadFile(iniPath);
            FontSizeTextBox.Text = data["TextBox"]["FontSize"];
            FontComboBox.SelectedValue = data["TextBox"]["Font"];
            switch ($"{app.RequestedThemeVariant}")
            {
                case "Default":
                    this.FindControl<RadioButton>("DefaultButton").IsChecked = true;
                    break;
                case "Light":
                    this.FindControl<RadioButton>("LightButton").IsChecked = true;
                    break;
                case "Dark":
                    this.FindControl<RadioButton>("DarkButton").IsChecked = true;
                    break;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            FontComboBox = this.FindControl<ComboBox>("FontComboBox1");
            FontSizeTextBox = this.FindControl<TextBox>("FontSizeTextBox1");
            
        }

        private void LoadSystemFonts()
        {
            var fonts = Avalonia.Media.FontManager.Current.SystemFonts;
            FontComboBox.ItemsSource = fonts.Select(f => f.ToString()).ToList();
        }

        private void ApplyButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string selectedFont = FontComboBox.SelectedItem as string;
            if (int.TryParse(FontSizeTextBox.Text, out int fontSize))
            {
                var parser = new FileIniDataParser();
                var iniPath = "config.ini";
                IniData data = parser.ReadFile(iniPath);
                data["TextBox"]["FontSize"] = $"{fontSize}";
                data["TextBox"]["Font"] = $"{selectedFont}";
                if (this.FindControl<RadioButton>("DefaultButton").IsChecked == true)
                {
                    data["Application"]["Theme"] = $"Default";
                }
                if (this.FindControl<RadioButton>("LightButton").IsChecked == true)
                {
                    data["Application"]["Theme"] = $"Light";
                }
                if (this.FindControl<RadioButton>("DarkButton").IsChecked == true)
                {
                    data["Application"]["Theme"] = $"Dark";
                }
                parser.WriteFile(iniPath, data);
                
                Close();

            }
            else
            {
                Console.WriteLine("Ошибка: неверный формат размера шрифта.");
            }
        }
    }
}