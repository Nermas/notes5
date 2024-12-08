using System;
using System.Globalization;
using Avalonia;
using Avalonia.Styling;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Data.Converters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Avalonia.Interactivity;
using IniParser;
using IniParser.Model;

namespace notes5;

public partial class MainWindow : Window
{
    public ObservableCollection<Note> MyItems { get; } = new ObservableCollection<Note>();
    private TextBox _textBox;
    private int Selected_id; 
    private bool selection_change = false ; 

    private Database _database;
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        _database = new Database("items.db");
        
        LoadItemsAsync();
        var listBox = this.FindControl<ListBox>("MyListBox");
        listBox.SelectionChanged += MyListBox_SelectionChanged;
        _textBox = this.FindControl<TextBox>("textbox");
        _textBox.TextChanged += TextBox_TextChanged;
        ChangeTextBoxSettings();

    }
    private void MyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var listBox = sender as ListBox;
        var selectedNote = listBox.SelectedItem as Note;
        if (selectedNote != null)
        {
            _textBox.Text = $"{selectedNote.Text}";
            Selected_id = selectedNote.Id;
            selection_change = true;
            _textBox.IsReadOnly = false;
        }
    }
    private async void TextBox_TextChanged(object sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        if (selection_change == false)
        {
            var newText = _textBox.Text;
            _database.UpdateItem(Selected_id, newText);

            LoadItemsAsync();
        }
        else
        {
            selection_change = false;
        }
    }

    private async void ChangeTextBoxSettings()
    {
        var parser = new FileIniDataParser();
        var iniPath = "config.ini";
        IniData data = parser.ReadFile(iniPath);
        _textBox.FontSize = int.Parse(data["TextBox"]["FontSize"]);
        _textBox.FontFamily = data["TextBox"]["Font"];
        var app = (App)Application.Current;
        switch (data["Application"]["Theme"])
        {
            case "Light":
                app.RequestedThemeVariant = ThemeVariant.Light;
                break;
            case "Dark":
                app.RequestedThemeVariant = ThemeVariant.Dark;
                break;
            default:
                app.RequestedThemeVariant = ThemeVariant.Default;
                break;
        }
    }
    private async void LoadItemsAsync()
    {
        var listBox = this.FindControl<ListBox>("MyListBox");
        var selectedId = Selected_id; 

        listBox.SelectionChanged -= MyListBox_SelectionChanged;

        var items = await _database.GetItemsAsync();
        MyItems.Clear();
        foreach (var item in items)
        {
            MyItems.Add(item);
        }

        var selectedNote = MyItems.FirstOrDefault(note => note.Id == selectedId);
        if (selectedNote != null)
        {
            listBox.SelectedItem = selectedNote;
        }

        listBox.SelectionChanged += MyListBox_SelectionChanged;

    }
    public void CreateNote(object source, RoutedEventArgs args)
    {
        var newNote = new Note
        {
            Id = 0, 
            Text = "",
            EditedAt = DateTime.Now,
            CreatedAt = DateTime.Now
        };

        _database.AddItem(newNote);

        LoadItemsAsync();

        _textBox.Text = newNote.Text;
        Selected_id = newNote.Id; 
    }
    private async void DeleteNote(object? sender, RoutedEventArgs e)
    {
        var listBox = this.FindControl<ListBox>("MyListBox");
        var selectedNote = listBox.SelectedItem as Note;

        if (selectedNote != null)
        {
            var confirmationDialog = new ConfirmDialog();
            var result = await confirmationDialog.ShowDialog<bool>(this);

            if (result)
            {
                _database.DeleteItem(selectedNote.Id);
                _textBox.Text = string.Empty;
                Selected_id = 0;
                LoadItemsAsync();
            }
            else
            {
                Debug.WriteLine("Удаление отменено пользователем.");
            }
        }
        else
        {
            Debug.WriteLine("Не выбрана заметка для удаления.");
        }
    }

    private async void SettingsClick(object? sender, RoutedEventArgs e)
    {
        var settings = new Settings();
        await settings.ShowDialog<bool>(this);
        ChangeTextBoxSettings();
    }

    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

}
