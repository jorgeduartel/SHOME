﻿using System.Collections.Generic;
using SHOME.Data;
using Xamarin.Forms;

namespace SHOME
{
    public class ListEventPage : ContentPage
    {
        public Button buttons;


        public ListEventPage()
        {
            buttons = new Button
            {
                Text = "Add Event",
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            buttons.Clicked += async (sender, e) => { Navigation.PushModalAsync(new CreateEvent()); };

            GetEvents();
        }


        public List<Event> events { get; set; } = new List<Event>();


        public async void GetEvents()
        {
            var aux = 0;
            var json = await WebServicesData.SyncTask("GET", "GetEvents");
            var size = json.Count;

            while (size > aux)
            {
                var result = json[aux];

                var eventS = new Event(
                    result["eventName"]
                );

                events.Add(eventS);
                aux++;
            }


            Constructor();
        }

        public void Constructor()
        {
            var dataTemplate = new DataTemplate(typeof(TextCell));
            dataTemplate.SetBinding(TextCell.TextProperty, "Name");

            var listView = new ListView
            {
                ItemsSource = events,
                ItemTemplate = dataTemplate
            };

            Content = new StackLayout
            {
                Children =
                {
                    buttons,
                    listView
                }
            };
        }

        public class Event
        {
            public Event(string name)
            {
                Name = name;
            }

            public string Name { set; get; }
            public string Descripton { get; set; }
            public string Divison { get; set; }
            public string Device { get; set; }
        }
    }
}