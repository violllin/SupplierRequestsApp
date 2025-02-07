using SupplierRequestsApp.Presentation.Pages;

namespace SupplierRequestsApp.Util;

public static class Loading
{
    public static async Task RunWithLoading(INavigation navigation, Func<Task> action)
    {
        await navigation.PushModalAsync(new LoadingComponent());
        try
        {
            await action();
        }
        finally
        {
            await navigation.PopModalAsync();
        }
    }

}