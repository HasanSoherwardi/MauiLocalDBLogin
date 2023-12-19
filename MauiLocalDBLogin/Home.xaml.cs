
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Formats.Tar;
using System.Reflection.Metadata;
using System.Text;

namespace MauiLocalDBLogin;

public partial class Home 
{
    User userDetails = new User();
    public Home(User user)
    {
        InitializeComponent();
        userDetails = user;
    }

    protected override void OnAppearing()
    {
        PopulateEmployeeList();

    }
    public void PopulateEmployeeList()
    {
        SQLite_Android Obj = new SQLite_Android();
        EmployeeList.ItemsSource = null;
        EmployeeList.ItemsSource = Obj.GetUsers(userDetails);
    }
   
    private async void BtnEdit_Clicked(object sender, EventArgs e)
    {
        User user = (((Button)sender).CommandParameter) as User;
        if (user != null)
        {
            await Navigation.PushAsync(new RegistrationPage(user));

            MessagingCenter.Unsubscribe<User>(this, "ReciveData");
            MessagingCenter.Subscribe<User>(this, "ReciveData", (value) =>
            {
                // MemoryStream streamRead = new MemoryStream(user.myArray.ToArray());
                // userDetails.myArray = user.myArray.ToArray();
                userDetails = value;
            });
        }
    }

    private async void BtnDelete_Clicked(object sender, EventArgs e)
    {
        User user = (((Button)sender).CommandParameter) as User;
        if (user != null)
        {
            var msgResult = await DisplayActionSheet("Alert", "Cancel", "Ok","Are you sure to delete?");
            switch(msgResult)
            {
                case "Ok":
                SQLite_Android Obj = new SQLite_Android();
                bool response = Obj.DeleteUser(user.id);
                if (response)
                {
                    await DisplayAlert("Deleted", "Deleted Successfully.", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Deletion Failed.", "OK");
                }
                break;
            }
        }
    }

    private async void EmployeeList_Refreshing(object sender, EventArgs e)
    {
        PopulateEmployeeList();
        EmployeeList.IsRefreshing = false;
    }
}