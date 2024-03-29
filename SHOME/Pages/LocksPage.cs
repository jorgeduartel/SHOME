﻿using SHOME.Data;
using Xamarin.Forms;

namespace SHOME
{
    public class LocksPage : ContentPage
    {
        private readonly Image _header = new Image
        {
            Source = "header_lock.png",
            HorizontalOptions = LayoutOptions.Center
        };

        private readonly Switch _powerBtn = new Switch
        {
            HorizontalOptions = LayoutOptions.Center
        };

        public Image Imagelock = new Image
        {
            Source = "unlocked2.png",
            HorizontalOptions = LayoutOptions.Center,
            Scale = 0.5
        };

        public bool State;

        public LocksPage(int device)
        {
            GetState();
        }

        /// <summary>
        /// To display the current state of the lock when the view is loaded.
        /// </summary>
        public async void GetState()
        {
            var json = await WebServicesData.SyncTask("GET", "lockState");
            var size = json.Count;
            var result = json[0];

            State = result["parameterValue"];
            Update();
        }

        /// <summary>
        /// Construtor.
        /// </summary>
        public void Update()
        {
            _powerBtn.IsToggled = State;
            Imagelock.Source = State ? "locked2.png" : "unlocked2.png";
            _powerBtn.Toggled += async (sender, e) =>
            {
                if (e.Value)
                {
                    await WebServicesData.SyncTask("POST", "Lock", 20, 1);
                    Imagelock.Source = "locked2.png";
                }
                else
                {
                    await WebServicesData.SyncTask("POST", "Lock", 20, 0);
                    Imagelock.Source = "unlocked2.png";
                }
            };

            Content = new StackLayout
            {
                Children =
                {
                    _header,
                    _powerBtn,
                    Imagelock
                }
            };
        }
    }
}