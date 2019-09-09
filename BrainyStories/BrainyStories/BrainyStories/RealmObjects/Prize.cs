using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrainyStories.RealmObjects
{
    public class Prize : RealmObject
    {
        public string Name { get; set; }
        public string Audio { get; set; }

        //set the position in which this prize should be displayed on the prize screen
        public int Row { get; set; }

        public int Column { get; set; }
    }
}