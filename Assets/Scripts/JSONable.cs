using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class JSONable
    {
        public abstract JSONObject getJSON();
        public abstract void fromJSON(JSONObject json);
    }
}
