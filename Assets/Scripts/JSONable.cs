using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    interface JSONable
    {
        JSONObject getJSON();
        void fromJSON(JSONObject json);

    }
}
