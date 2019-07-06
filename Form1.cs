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
        Opponent opponent;

        private int moves;

        public Form1()
        {
            InitializeComponent();
            CreateObjects();
            ResetGame();
        }

        private void CreateObjects()
        {
            
            livingRoom = new RoomWithDoor("Salon", "antyczny dywan", "dębowe drzwi z mosiężną klamką", "w szafie ściennej");
            diningRoom = new Room("Jadalnia", "kryształowy żyrandol");
            kitchen = new RoomWithDoor("Kuchnia", "nierdzewne stalowe sztućce", "drzwi zasuwane", "w szafce");
            frontYard = new OutsideWithDoor("Podwórko przed domem", false, "dębowe drzwi z mosiężną klamką");
            garden = new OutsideWithHidingPlace("Ogród", false, "w szopie");
            backYard = new OutsideWithDoor("Podwórko za domem", true, "drzwi zasuwane");
            staircase = new Room("Schody", "drewnianą poręcz");
            upperCorridor = new RoomWithHidingPlace("Korytarz na górze", "obrazek z psem", "w szafie ściennej");
            bigBedroom = new RoomWithHidingPlace("Duża sypialnia", "duże łózko", "pod łóżkiem");
            smallBedroom = new RoomWithHidingPlace("Mała sypialnia", "małe łóżko", "pod łóżkiem");
            bathroom = new RoomWithHidingPlace("Łazienka", "umywalkę i toaletę", "pod prysznicem");
            route = new OutsideWithHidingPlace("Droga dojazdowa", false, "w garażu");

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

            RedrawForm();
        }

        private void GoHere_Click(object sender, EventArgs e)
        {
            moves++;
            MoveToANewLocation(currentLocation.Exits[exits.SelectedIndex]);
        }

        private void GoThroughTheDoor_Click(object sender, EventArgs e)
        {
            moves++;
            IHasExteriorDoor hasDoor = currentLocation as IHasExteriorDoor;
            MoveToANewLocation(hasDoor.DoorLocation);
        }

        private void Check_Click(object sender, EventArgs e)
        {
            moves++;
            RedrawForm();
            if (opponent.Check(currentLocation))
            {
                //znaleziono przeciwnika - gra od początku
                string message = "Odnalazłeś mnie w " + moves + " ruchach!";
                MessageBox.Show(message);
                ResetGame();
            }
            else
            {
                //nie znaleziono przeciwnika - lecimy dalej
            }
        }

        private void StartGame()
        {
            hide.Visible = false;
            for (int i = 1; i <= 10; i++)
            {
                description.Text = i + "... ";
                Application.DoEvents();
                System.Threading.Thread.Sleep(200);
                opponent.Move();
            }
            description.Text = "Gotowy czy nie - nadchodzę!";
            Application.DoEvents();
            System.Threading.Thread.Sleep(500);
            //gra się rozpoczyna RedrawForm() -> MoveToA... (to co jest teraz w inic)
            MoveToANewLocation(livingRoom);
        }

        private void RedrawForm()
        {
            goHere.Visible = true;
            exits.Visible = true;
            if (currentLocation is IHasExteriorDoor)
                goThroughTheDoor.Visible = true;
            else
                goThroughTheDoor.Visible = false;
            check.Visible = true;
            description.Text += " Wykonałeś " + moves + " ruchów.";

            if (currentLocation is IHidingPlace)
            {
                IHidingPlace hidingPlace = currentLocation as IHidingPlace;
                check.Text = "Sprawdź " + hidingPlace.HidingPlace;
            }
            else
                check.Text = "----";
        }

        private void ResetGame()
        {
            opponent = new Opponent(frontYard);
            goHere.Visible = false;
            exits.Visible = false;
            goThroughTheDoor.Visible = false;
            check.Visible = false;
            hide.Visible = true;

            if (moves != 0)
                description.Text = "Odnalazłeś przeciwnika w: " + currentLocation.Name + " w " + moves + " ruchach.";

            moves = 0;
        }

        private void Hide_Click(object sender, EventArgs e)
        {
            StartGame();
        }
    }
}
