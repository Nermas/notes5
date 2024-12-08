using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace notes5;

public partial class ConfirmDialog : Window
{
    public ConfirmDialog()
    {
        InitializeComponent();
    }
    public bool IsConfirmed { get; private set; }

    private void Yes_Click(object? sender, RoutedEventArgs e)
    {
        Close(true); // Возвращаем true, если нажата кнопка "Да"
    }

    private void No_Click(object? sender, RoutedEventArgs e)
    {
        Close(false); // Возвращаем false, если нажата кнопка "Нет"
    }
}