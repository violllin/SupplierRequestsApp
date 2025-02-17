namespace SupplierRequestsApp.Presentation.Pages
{
    public class LoadingComponent : ContentPage
    {
        public LoadingComponent()
        {
            BackgroundColor = Color.FromArgb("#80000000");
            Content = new ActivityIndicator
            {
                IsRunning = true,
                Color = Colors.White,
                Scale = 3,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
        }
    }
}