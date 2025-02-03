namespace SupplierRequestsApp.Presentation.Pages;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnGoToStoragePageClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new StoragesPage());
    }
}