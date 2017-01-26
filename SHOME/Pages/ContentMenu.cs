﻿using System.Collections.Generic;
using SHOME.Data;
using SHOME.Pages;
using Xamarin.Forms;

namespace SHOME
{
    public class ContentMenu : ContentPage
    {
        public List<Division> Divisions = new List<Division>();


        public ContentMenu(string tab)
        {
            var deviceSettings = new Devices(1009, "Settings", "Settings", "" , 1);
            var deviceEvents = new Devices(1008, "Events", "Events", "", 1);
            var deviceWeather = new Devices(1000, "Weather", "weather", "", 1);
            var deviceEnergyConsumption = new Devices(1001, "EnergyConsumption", "EnergyConsumption", "", 1);
            var deviceEnergyManagement = new Devices(1002, "EnergyManagement", "EnergyManagement", "", 1);
            var deviceCctv = new Devices(1003, "Camaras", "cctv", "", 1);
            var deviceIrrigation = new Devices(1004, "irrigation", "irrigation", "", 1);


            // Division
            var roomm = new Division(1000, "All", "Home", null);
            Divisions.Add(roomm);

            roomm.AddDivice(deviceWeather);
            roomm.AddDivice(deviceEnergyConsumption);
            roomm.AddDivice(deviceEnergyManagement);
            roomm.AddDivice(deviceCctv);
            roomm.AddDivice(deviceIrrigation);
            roomm.AddDivice(deviceEvents);
            roomm.AddDivice(deviceSettings);


            DivisionData(tab);
        }

        public async void DivisionData(string tab)
        {
            var aux = 0;
            var json = await WebServicesData.SyncTask("GET", "division");
            var size = json.Count;

            while (size > aux)
            {
                var result = json[aux];
                var division = new Division(
                    result["idDivision"],
                    result["divisionName"],
                    result["typeDivision"],
                    result["BeaconId"]
                );
                Divisions.Add(division);

                DevicesData(division.Id, division, tab);

                aux++;
            }
        }

		public async void SetClicks(string type, int actuator)
		{
			//http://montalegre.m-iti.org:22941/updateClicks/actuator/16
			var aux = 0;
			var json = await WebServicesData.SyncTask("POST", "updateClicks", type , actuator);


		}

        public async void DevicesData(int id, Division division, string tab)
        {
            var json = await WebServicesData.SyncTask("GET", "ActuatorsDevicesByDivision", id);
            var size = json.Count;
            var aux = 0;

            while (size > aux)
            {
                var result = json[aux];
                var device = new Devices(
                    result["idActuator"],
                    result["deviceName"],
                    result["actuatorName"],
					result["actuatorDescription"],
					result["idActuator"]
                );

                division.AddDivice(device);
                aux++;
                if (size == aux)
                {
                    var deviceAdd = new Devices(1010, "ADD", "ADD" , "" ,1);
                    division.AddDivice(deviceAdd);
                }
            }

            Construtor(tab);
        }

        public async void Construtor(string tab)
        {
            switch (tab)
            {
                case "Bedroom":
                    BackgroundImage = "bedroom_background.png";
                    break;
                case "Living Room":
                    BackgroundImage = "lounge_background.png";
                    break;
                case "Home":
                    BackgroundImage = "menu_background.png";
                    break;
                case "Garden":
                    BackgroundImage = "garden_background.png";
                    break;
                case "Kitchen":
                    BackgroundImage = "kitchen_background.png";
                    break;
            }

            var buttonLights = new List<Image>();
            var buttonCctvs = new List<Image>();
            var buttonACs = new List<Image>();
            var buttonListAudio = new List<Image>();
            var buttonListBlinds = new List<Image>();
            var buttonListLock = new List<Image>();
            var buttonListIrrigation = new List<Image>();
            var buttonListWeather = new List<Image>();
            var buttonListEnergyConsumption = new List<Image>();
            var buttonListEnergyManagement = new List<Image>();

            var stack = new StackLayout();
            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

            foreach (var s in Divisions)
                if ((s.Type == tab) || (tab == "Home"))
                {
                    var grid = new Grid
                    {
                        BackgroundColor = new Color(0, 0, 0, 0),
                        RowDefinitions = new RowDefinitionCollection
                        {
                            new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                        }
                    };

                    stack.Children.Add(new Label
                    {
                        Text = s.Name.ToUpper(),
                        HorizontalTextAlignment = TextAlignment.Center,
                        BackgroundColor = Color.FromRgba(211, 211, 211, 100),
                        FontSize = 18,
                        FontFamily = "Roboto"
                    });


                    var rowGrid = 0;
                    var columnGrid = 0;
                    foreach (var dev in s.devices)
                        switch (dev.Type)
                        {
                            case "Light":
                                var buttonLight = new Image
                                {
                                    Source = "lights.png",
                                    WidthRequest = 70,
                                    HeightRequest = 70,
                                    Opacity = 2
                                };
                                dev.buttons = buttonLight;
								dev.buttons.GestureRecognizers.Add( new TapGestureRecognizer
								{
							Command = new Command(() => { SetClicks("actuator", dev.Id); Navigation.PushAsync(new LightsPage(dev.Id));  })
									
                                });
                                grid.Children.Add(buttonLight, columnGrid, rowGrid);

								var lab = (new Label
								{
									Text =  dev.Description.Substring(0, 10),
									TextColor = Color.White,
									HorizontalTextAlignment = TextAlignment.Center
								});
								grid.Children.Add(lab, columnGrid, (rowGrid+1));

                                columnGrid++;
                                break;

                            case "CCTV":
                                var buttonCctv = new Image {Source = "cctv.png", WidthRequest = 7, HeightRequest = 70};
                                dev.buttons = buttonCctv;
                                dev.buttons.GestureRecognizers.Add(new TapGestureRecognizer
                                {
                                    Command = new Command(() => { SetClicks("actuator", dev.Id); Navigation.PushAsync(new CameraPage()); })
                                });

								
                                grid.Children.Add(buttonCctv, columnGrid, rowGrid);


								var labCCTV = (new Label
								{
									Text = dev.Description.Substring(0, 10),
									TextColor = Color.White,
									HorizontalTextAlignment = TextAlignment.Center
								});
										grid.Children.Add(labCCTV, columnGrid, (rowGrid + 1));

                                columnGrid++;
                                break;

                            case "Ac":
                                var buttonAC = new Image {Source = "ac.png", WidthRequest = 7, HeightRequest = 70};
                                buttonACs.Add(buttonAC);
                                grid.Children.Add(buttonAC, columnGrid, rowGrid);
                                columnGrid++;
                                break;

                            case "Audio":
                                var buttonAudio = new Image {Source = "audio.png", WidthRequest = 7, HeightRequest = 70};
                                buttonListAudio.Add(buttonAudio);
                                grid.Children.Add(buttonAudio, columnGrid, rowGrid);


                                columnGrid++;
                                break;
                            case "Blinds":
                                var buttonBlinds = new Image
                                {
                                    Source = "blinds.png",
                                    WidthRequest = 7,
                                    HeightRequest = 70
                                };
                                buttonListBlinds.Add(buttonBlinds);
                                grid.Children.Add(buttonBlinds, columnGrid, rowGrid);

                                columnGrid++;
                                break;
                            case "Lock":
                                var buttonLock = new Image {Source = "lock.png", WidthRequest = 7, HeightRequest = 70};
                                dev.buttons = buttonLock;
                                dev.buttons.GestureRecognizers.Add(new TapGestureRecognizer
                                {
                                    Command = new Command(() => { Navigation.PushAsync(new LocksPage(dev.Id)); })
                                });
                                grid.Children.Add(buttonLock, columnGrid, rowGrid);

								var labLock = (new Label
								{
									Text = dev.Description.Substring(0, 10),
									TextColor = Color.White,
									HorizontalTextAlignment = TextAlignment.Center
								});
								grid.Children.Add(labLock, columnGrid, (rowGrid + 1));

                                columnGrid++;
                                break;
                            case "Irrigations":
                                var buttonIrrigation = new Image
                                {
                                    Source = "irrigation.png",
                                    WidthRequest = 7,
                                    HeightRequest = 70
                                };
                                buttonListIrrigation.Add(buttonIrrigation);
                                grid.Children.Add(buttonIrrigation, columnGrid, rowGrid);

								var labIrrigations = (new Label
								{
									Text = dev.Description.Substring(0, 10),
									TextColor = Color.White,
									HorizontalTextAlignment = TextAlignment.Center
								});
								grid.Children.Add(labIrrigations, columnGrid, (rowGrid + 1));
                                columnGrid++;
                                break;
                            case "weather":
                                var buttonWeather = new Image
                                {
                                    Source = "weather.png",
                                    WidthRequest = 7,
                                    HeightRequest = 70
                                };
                                dev.buttons = buttonWeather;
                                dev.buttons.GestureRecognizers.Add(new TapGestureRecognizer
                                {
                                    Command = new Command(() => { Navigation.PushAsync(new Weather()); })
                                });
                                grid.Children.Add(buttonWeather, columnGrid, rowGrid);
                                columnGrid++;
                                break;
                            case "EnergyConsumption":
                                var buttonEnergyConsumption = new Image
                                {
                                    Source = "energy_consumption.png",
                                    WidthRequest = 7,
                                    HeightRequest = 70
                                };
                                dev.buttons = buttonEnergyConsumption;
                                dev.buttons.GestureRecognizers.Add(new TapGestureRecognizer
                                {
                                    Command = new Command(() => { Navigation.PushAsync(new ConsumptionPage()); })
                                });
                                grid.Children.Add(buttonEnergyConsumption, columnGrid, rowGrid);
                                columnGrid++;
                                break;
                            case "EnergyManagement":
                                var buttonEnergyManagement = new Image
                                {
                                    Source = "energy_management.png",
                                    WidthRequest = 7,
                                    HeightRequest = 70
                                };
                                buttonListEnergyManagement.Add(buttonEnergyManagement);
                                grid.Children.Add(buttonEnergyManagement, columnGrid, rowGrid);
                                columnGrid++;
                                break;
                            case "Events":
                                var buttonEvents = new Image
                                {
                                    Source = "icon_events.png",
                                    WidthRequest = 7,
                                    HeightRequest = 70
                                };
                                dev.buttons = buttonEvents;
                                dev.buttons.GestureRecognizers.Add(new TapGestureRecognizer
                                {
                                    Command = new Command(() => { Navigation.PushAsync(new ListEventPage()); }),

                                });
                                grid.Children.Add(buttonEvents, columnGrid, rowGrid);
                                columnGrid++;
                                break;

                            case "Settings":
                                var buttonSettings = new Image
                                {
                                    Source = "icon_settings.png",
                                    WidthRequest = 7,
                                    HeightRequest = 70
                                };
                                dev.buttons = buttonSettings;
                                dev.buttons.GestureRecognizers.Add(new TapGestureRecognizer
                                {
                                    Command = new Command(() => { Navigation.PushAsync(new SettingsPage()); })
                                });
                                grid.Children.Add(buttonSettings, columnGrid, rowGrid);
                                columnGrid++;
                                break;
                            case "ADD":
                                var buttonADD = new Image
                                {
                                    Source = "icon_addDevice.png",
                                    WidthRequest = 7,
                                    HeightRequest = 70
                                };
                                dev.buttons = buttonADD;
                                dev.buttons.GestureRecognizers.Add(new TapGestureRecognizer
                                {
                                    Command = new Command(() => { Navigation.PushAsync(new AddDevice(s.Id)); })
                                });
                                grid.Children.Add(buttonADD, columnGrid, rowGrid);
                                columnGrid++;
                                break;
                        }


                    stack.Children.Add(new ScrollView
                    {
                        Content = grid,
                        Orientation = ScrollOrientation.Horizontal
                    });
                }


            var scollVertical = new ScrollView
            {
                Content = stack,
                Orientation = ScrollOrientation.Vertical
            };

            Content = scollVertical;

        }


        public class Devices
        {
            public Devices(int id, string name, string type, string description , int idActuator)
            {
                Id = id;
                Name = name;
                Type = type;
				Description = description;
				IDActuator = idActuator;
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
			public string Description { get; set; }
			public int IDActuator { get; set; }

            public Image buttons { get; set; } = new Image();
        }

        public class Division
        {
            public Division(int id, string name, string type, string beaconId)
            {
                Id = id;
                Name = name;
                Type = type;
                BeaconId = beaconId;
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string BeaconId { get; set; }

            public List<Devices> devices { get; } = new List<Devices>();

            public void AddDivice(Devices dev)
            {
                devices.Add(dev);
            }
        }
    }
}