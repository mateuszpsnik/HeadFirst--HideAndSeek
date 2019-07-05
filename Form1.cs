using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeadFirst__HideAndSeek
{
    public partial class Form1 : Form
    {
        RoomWithDoor livingRoom;
        Room diningRoom;
        RoomWithDoor kitchen;
        OutsideWithDoor frontYard;
        OutsideWithHidingPlace garden;
        OutsideWithDoor backYard;
        Room staircase;
        RoomWithHidingPlace upperCorridor;
        RoomWithHidingPlace bigBedroom;
        RoomWithHidingPlace smallBedroom;
        RoomWithHidingPlace bathroom;
        OutsideWithHidingPlace route;

        public Form1()
        {
            InitializeComponent();
            CreateObjects();
            MoveToANewLocation(livingRoom);
        }

        private void CreateObjects()
        {
            
            livingRoom = new RoomWithDoor("Salon", "antyczny dywan", "dębowe drzwi z mosiężną klamką", "szafa ścienna");
            diningRoom = new Room("Jadalnia", "kryształowy żyrandol");
            kitchen = new RoomWithDoor("Kuchnia", "nierdzewne stalowe sztućce", "drzwi zasuwane", "szafka");
            frontYard = new OutsideWithDoor("Podwórko przed domem", false, "dębowe drzwi z mosiężną klamką");
            garden = new OutsideWithHidingPlace("Ogród", false, "szopa");
            backYard = new OutsideWithDoor("Podwórko za domem", true, "drzwi zasuwane");
            staircase = new Room("Schody", "drewnianą poręcz");
            upperCorridor = new RoomWithHidingPlace("Korytarz na górze", "obrazek z psem", "szafa ścienna");
            bigBedroom = new RoomWithHidingPlace("Duża sypialnia", "duże łózko", "pod łóżkiem");
            smallBedroom = new RoomWithHidingPlace("Mała sypialnia", "małe łóżko", "pod łóżkiem");
            bathroom = new RoomWithHidingPlace("Łazienka", "umywalkę i toaletę", "pod prysznicem");
            route = new OutsideWithHidingPlace("Droga dojazdowa", false, "garaż");

            livingRoom.DoorLocation = frontYard;
            frontYard.DoorLocation = livingRoom;
            kitchen.DoorLocation = backYard;
            backYard.DoorLocation = kitchen;

            livingRoom.Exits = new Location[] { diningRoom, staircase };
            diningRoom.Exits = new Location[] { livingRoom, kitchen };
            kitchen.Exits = new Location[] { diningRoom };
            frontYard.Exits = new Location[] { backYard, garden, route };
            garden.Exits = new Location[] { frontYard, backYard };
            backYard.Exits = new Location[] { frontYard, garden, route };
            staircase.Exits = new Location[] { livingRoom, upperCorridor };
            upperCorridor.Exits = new Location[] { staircase, bigBedroom, smallBedroom, bathroom };
            bigBedroom.Exits = new Location[] { upperCorridor };
            smallBedroom.Exits = new Location[] { upperCorridor };
            bathroom.Exits = new Location[] { upperCorridor };
            route.Exits = new Location[] { frontYard, backYard };
        }

        private Location currentLocation;

        private void MoveToANewLocation(Location location)
        {
            currentLocation = location;
            exits.Items.Clear();
            for (int i = 0; i < currentLocation.Exits.Length; i++)
                exits.Items.Add(currentLocation.Exits[i].Name);
            exits.SelectedIndex = 0;
            description.Text = currentLocation.Description;

            if (currentLocation is IHasExteriorDoor)
                goThroughTheDoor.Visible = true;
            else
                goThroughTheDoor.Visible = false;
        }

        private void GoHere_Click(object sender, EventArgs e)
        {
            MoveToANewLocation(currentLocation.Exits[exits.SelectedIndex]);
        }

        private void GoThroughTheDoor_Click(object sender, EventArgs e)
        {
            IHasExteriorDoor hasDoor = currentLocation as IHasExteriorDoor;
            MoveToANewLocation(hasDoor.DoorLocation);
        }
    }
}
