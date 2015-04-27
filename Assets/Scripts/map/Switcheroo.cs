using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.map
{
    class Switcheroo
    {

        private static List<GameObject> objects = new List<GameObject>();

        /**
         * Call this only after calling disable
         * */
        public static void reEnable(){
            foreach (GameObject obj in objects)
            {
                obj.SetActive(true);
            }
        }

        /**
         * Call This before calling disable.
         * Disables all gameObjects in the scene
         * */
        public static void disable(){
            objects = new List<GameObject>();
            foreach (GameObject allObjects in GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                objects.Add(allObjects);
                allObjects.SetActive(false);
            }
        }


    }
}
