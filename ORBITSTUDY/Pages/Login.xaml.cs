namespace ORBITSTUDY.Pages;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private void btnSignin_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(LoadingPage));
    }

    private void btnRegister_Clicked(object sender, EventArgs e)
    {

    }
}