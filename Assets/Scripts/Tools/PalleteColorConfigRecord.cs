using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.UI {

    [Serializable]
    public class PalleteColorConfigRecord
    {
        public string RecordKey;
        public bool UseMapping;
        public Color Value;
        public string MappingKey;
    }
}