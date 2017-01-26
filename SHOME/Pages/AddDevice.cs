﻿using System;
using System.Json;
using System.Linq;
using SHOME.Data;
using Xamarin.Forms;

namespace SHOME.Pages
{
    internal class AddDevice : ContentPage
    {
        public AddDevice()
        {
            DeviceConstrutor();
        }

        private void DeviceConstrutor()
        {
            var header = new Image
            {
                Source = new FileImageSource
                {
                    File = Device.OnPlatform(
                        "Images/header_addDevice.png",
                        "header_addDevice.png",
                        "Images/header_addDevice.png")
                },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start
            };

            var forms = new Grid
            {
                Padding = new Thickness(20, 0, 20, 10),
                BackgroundColor = new Color(0, 0, 0, 0),
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition
                    {
                        Height = new GridLength(1, GridUnitType.Star)
                    }
                }
            };

            var devLabel = new Label{ Text = " Device Name", FontFamily = "Roboto", FontSize = 14, VerticalTextAlignment = TextAlignment.End};
            var devIf = new Entry {FontSize = 12};
            var deviceCode = new Label { Text = " Device Code", FontFamily = "Roboto", FontSize = 14, VerticalTextAlignment = TextAlignment.End };
            var devCodeIf = new Entry { FontSize = 12 };
            var devModel = new Label { Text = " Device Model", FontFamily = "Roboto", FontSize = 14, VerticalTextAlignment = TextAlignment.End };
            var devModelIf = new Entry { FontSize = 12 }; ;
            var devOrder = new Label { Text = " Device Order", FontFamily = "Roboto", FontSize = 14, VerticalTextAlignment = TextAlignment.End };
            var devOrderIf = new Entry { FontSize = 12 };

            var addButton = new Button
            {
                Text = "ADD ACTUATOR",
                FontFamily = "Roboto",
                FontSize = 18,
                VerticalOptions = LayoutOptions.End
            };
            addButton.Clicked += async (sender, e) =>
            {
                if (!IsValid(devIf, devCodeIf, devOrderIf, devModelIf)) return;
                /*var json = await WebServicesData.SyncTask("POST", "insertdevice", 
                    devIf.Text, devCodeIf.Text, devModelIf.Text, devOrder.Text,
                    0, 1, 0, 0, DateTime.Now.Date, 0, 1);*/
                //TODO
                AtuatorConstrutor(0);
            };

            forms.Children.Add(devLabel, 0 , 0);
            forms.Children.Add(devIf, 0, 1);
            forms.Children.Add(deviceCode, 0, 2);
            forms.Children.Add(devCodeIf, 0, 3);
            forms.Children.Add(devModel, 0, 4);
            forms.Children.Add(devModelIf, 0, 5);
            forms.Children.Add(devOrder, 0, 6);
            forms.Children.Add(devOrderIf, 0, 7);
            forms.Children.Add(addButton, 0, 8);

            var scroll = new ScrollView
            {
                Content = forms
            };
            
            Content = new StackLayout
            {
                Children =
                {
                    header,
                    scroll
                }
            };
        }
        
        private void AtuatorConstrutor(int id)
        {
            var header = new Image
            {
                Source = new FileImageSource
                {
                    File = Device.OnPlatform(
                        "Images/header_addDevice.png",
                        "header_addDevice.png",
                        "Images/header_addDevice.png")
                },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start
            };

            var forms = new Grid
            {
                Padding = new Thickness(20, 0, 20, 10),
                BackgroundColor = new Color(0, 0, 0, 0),
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition
                    {
                        Height = new GridLength(1, GridUnitType.Star)
                    }
                }
            };

            var actName = new Label { Text = " Actuator Name", FontFamily = "Roboto", FontSize = 14, VerticalTextAlignment = TextAlignment.End };
            var actIf = new Entry { FontSize = 12 };
            var actDescr = new Label { Text = " Actuator Description", FontFamily = "Roboto", FontSize = 14, VerticalTextAlignment = TextAlignment.End };
            var actDescrIf = new Entry { FontSize = 12 };
            
            var saveButton = new Button
            {
                Text = "SAVE",
                FontFamily = "Roboto",
                FontSize = 18,
                VerticalOptions = LayoutOptions.End
            };

            saveButton.Clicked += async (sender, e) =>
            {
                if (!IsValid(actIf, actDescrIf)) return;
                /*var json = await WebServicesData.SyncTask("POST", "insertactuator",
                    actIf.Text, actDescrIf.Text, DateTime.Now.Date, DateTime.Now.Date,
                    0, 0, id, 0);*/
                await DisplayAlert("SUCCESS", "Actuator " + actIf.Text + " added!", "Ok");
                //await Navigation.PushModalAsync(new ContentPage());
            };

            forms.Children.Add(actName, 0, 0);
            forms.Children.Add(actIf, 0, 1);
            forms.Children.Add(actDescr, 0, 2);
            forms.Children.Add(actDescrIf, 0, 3);
            forms.Children.Add(saveButton, 0, 4);

            var scroll = new ScrollView
            {
                Content = forms
            };

            Content = new StackLayout
            {
                Children =
                {
                    header,
                    scroll
                }
            };
        }


        private static bool IsValid(params Entry[] parameters)
        {
            return parameters.All(par => !string.IsNullOrEmpty(par.Text));
        }

    }
}