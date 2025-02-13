namespace SupplierRequestsApp.Presentation.Pages
{
    public class LoadingComponent : ContentPage
    {
        public LoadingComponent()
        {
            BackgroundColor = Color.FromArgb("#80000000");
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 20,
                Children =
                {
                    new ActivityIndicator
                    {
                        IsRunning = true,
                        Color = Colors.White,
                        Scale = 3
                    },
                    new Label
                    {
                        Text = "Загрузка",
                        TextColor = Colors.White,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontSize = 20
                    }
                }
            };
        }
    }
}