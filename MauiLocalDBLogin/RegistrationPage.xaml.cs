using System;
using System.IO;
using System.Text;
using static SQLite.SQLite3;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace MauiLocalDBLogin;

public partial class RegistrationPage : ContentPage
{
    User UserDetails = new User();
    public RegistrationPage(User user)
	{
        InitializeComponent();

        if (user != null)
        {
            UserDetails = user;
            
            if (user.myArray == null)
            {
                Name.Text = UserDetails.Name;
                dp.Date = UserDetails.DOB;
                Place.Text = UserDetails.POB;
                Email.Text = UserDetails.Email;
                UserId.Text = UserDetails.UserId;
                Password.Text = UserDetails.Password;
                //Password.IsEnabled = false;

                myImage.Source = "man.png";
                SaveBtn.Text = "Update";
                this.Title = "Edit Info";
            }
            else
            {
                PopulateDetails(UserDetails);
            }
        }
        else
        {
            SaveBtn.Text = "Save";
            this.Title = "Registration";
            myImage.Source = "man.png";
        }
    }

    private void PopulateDetails(User user)
    {
        Name.Text = user.Name;
        dp.Date = user.DOB;
        Place.Text = user.POB;
        Email.Text = user.Email;
        UserId.Text = user.UserId;
        Password.Text = user.Password;
        MemoryStream streamRead = new MemoryStream(user.myArray.ToArray());
        myImage.Source = ImageSource.FromStream(() => { return streamRead; });
        SaveBtn.Text = "Update";
        this.Title = "Edit Info";
    }


    private async void SaveBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(Name.Text))
            {
                await DisplayAlert("Input Error", "Name is Required", "OK");
                Name.Focus();
                return;
            }
            if (string.IsNullOrEmpty(Place.Text))
            {
                await DisplayAlert("Input Error", "Place of Birth is Required", "OK");
                Place.Focus();
                return;
            }
            if (string.IsNullOrEmpty(Email.Text))
            {
                await DisplayAlert("Input Error", "Email is Required", "OK");
                Email.Focus();
                return;
            }
            bool bEmail;
            bEmail = Regex.IsMatch(Email.Text, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            if (bEmail == false)
            {
                await DisplayAlert("Input Error", "Invalid Email Address.", "OK");
                Email.Focus();
                return;
            }
            if (string.IsNullOrEmpty(UserId.Text))
            {
                await DisplayAlert("Input Error", "UserId is Required", "OK");
                UserId.Focus();
                return;
            }
            if (string.IsNullOrEmpty(Password.Text))
            {
                await DisplayAlert("Input Error", "Password is Required", "OK");
                Password.Focus();
                return;
            }
            UserDetails.Name = Name.Text;
            UserDetails.DOB = Convert.ToDateTime(dp.Date.ToString());
            UserDetails.POB = Place.Text;
            UserDetails.Email = Email.Text;
            UserDetails.UserId = UserId.Text;
            UserDetails.Password = Password.Text;

            if (SaveBtn.Text == "Save")
            { 
                SQLite_Android Obj = new SQLite_Android();
                bool response = Obj.SaveUser(UserDetails);
                if (response)
                {
                    await DisplayAlert("Saved", "Save Successfully.", "OK");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Not Save. Try with different User Id.", "OK");
                }
            }
            else
            {
                SQLite_Android Obj = new SQLite_Android();
                bool response = Obj.UpdateUser(UserDetails);
                if (response)
                {
                    MessagingCenter.Send<User>(UserDetails,  "ReciveData");
                    await DisplayAlert("Updated", "Update Successfully.", "OK");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Not Save. Try with different User Id.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Input Error", ex.ToString(), "Ok");
        }
    }

    private async void BtnCapture_Clicked(object sender, EventArgs e)
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                // save the file into local storage
                // ---------------------------------
                //string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                //using FileStream localFileStream = File.OpenWrite(localFilePath);
                //await sourceStream.CopyToAsync(localFileStream);

                var sourceStream = await photo.OpenReadAsync();     
                using (var memoryStream = new MemoryStream())
                {
                    sourceStream.CopyTo(memoryStream);
                    UserDetails.myArray = memoryStream.ToArray();
                }
                MemoryStream streamRead = new MemoryStream(UserDetails.myArray.ToArray());
                myImage.Source = ImageSource.FromStream(() => { return streamRead; });
            }
        }
        else
        {
            await DisplayAlert("Error", "Camera is not supported", "OK");
        }
    }

    private async void BtnBrowse_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Pick image please",
            FileTypes = FilePickerFileType.Images                   
        });
        if (result == null)
        {
            return;
        }
        var sourceStream = await result.OpenReadAsync();
        using (var memoryStream = new MemoryStream())
        {
            sourceStream.CopyTo(memoryStream);
            UserDetails.myArray = memoryStream.ToArray();
        }
        MemoryStream streamRead = new MemoryStream(UserDetails.myArray.ToArray());
        myImage.Source = ImageSource.FromStream(() => { return streamRead; });
    }
}