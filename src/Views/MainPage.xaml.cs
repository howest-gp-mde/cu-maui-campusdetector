using Mde.CampusDetector.ViewModels;

namespace Mde.CampusDetector.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel mainViewModel;

    public MainPage(MainViewModel mainViewModel)
	{
		InitializeComponent();

        BindingContext = this.mainViewModel = mainViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        mainViewModel.AppearingCommand.Execute(null);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        mainViewModel.DisappearingCommand.Execute(null);
    }
}