using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeRow.Engine
{
    abstract class Component
    {
        public GameObject gameObject {
            private set;
            get;
        }

        public Component(GameObject go)
        {
            gameObject = go;
        }

        public Component() { }
    }
}
