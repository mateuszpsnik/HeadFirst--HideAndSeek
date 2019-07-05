using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadFirst__HideAndSeek
{
    class Opponent
    {
        private Location myLocation;
        Random random;

        public Opponent(Location initialLocation)
        {
            myLocation = initialLocation;
            random = new Random();
        }

        public void Move()
        {
            if (myLocation is IHasExteriorDoor)
            {
                if (random.Next(2) == 1)
                {
                    IHasExteriorDoor hadDoor;
                    hadDoor = myLocation as IHasExteriorDoor;
                    myLocation = hadDoor.DoorLocation;
                }   
                else
                {
                    int exitsLength = myLocation.Exits.Length;
                    myLocation = myLocation.Exits[random.Next(exitsLength)];
                }
            }
            else
            {
                int exitsLength = myLocation.Exits.Length;
                myLocation = myLocation.Exits[random.Next(exitsLength)];
            }
        }

        public bool Check(Location location)
        {
            if (myLocation == location)
                return true;
            else
                return false;
        }
    }
}
