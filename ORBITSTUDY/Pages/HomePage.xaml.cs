using ORBITSTUDY.Helpers;

namespace ORBITSTUDY.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
			
	}

    private async void btnSettings_Clicked(object sender, EventArgs e)
    {
		var btn = (ImageButton)sender;

		await btn.PlayPressAnimation();
    }
}