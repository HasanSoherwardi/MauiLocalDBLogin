namespace MauiLocalDBLogin
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UName.Text))
            {
                await DisplayAlert("Input Error", "Please enter user name!!!", "OK");
                UName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(Password.Text))
            {
                await DisplayAlert("Input Error", "Please enter password!!!", "OK");
                Password.Focus();
                return;
            }
            User UserDetails = new User();
            UserDetails.UserId = UName.Text;
            UserDetails.Password = Password.Text;

            SQLite_Android Obj = new SQLite_Android();
            var res = Obj.GetUser(UserDetails);
            if (res != null)
            {
                UserDetails = res;
                await DisplayAlert("Welcome", "Login Successfully.", "OK");
               await Navigation.PushModalAsync(new Home(UserDetails));

            }
            else
            {
               await DisplayAlert("Error", "Invalid Credentials", "OK");
            }

        }


        private async void BtnRegistration_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegistrationPage(null));
        }
    }

}
