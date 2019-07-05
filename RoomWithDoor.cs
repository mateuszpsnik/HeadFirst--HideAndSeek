using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadFirst__HideAndSeek
{
    class RoomWithDoor : RoomWithHidingPlace, IHasExteriorDoor
    {
        public RoomWithDoor(string name, string decoration, string doorDescription, string hidingPlace) : base(name, decoration, hidingPlace)
        {
            DoorDescription = doorDescription;
        }
        public string DoorDescription { get; private set; }

        public Location DoorLocation { get; set; }

    }
}
