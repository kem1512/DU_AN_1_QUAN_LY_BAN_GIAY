﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BUS_BussinessLayer.Utilities
{
    public static class ValidateForm
    {
        public static bool CompareObject(object obj1, object obj2)
        {
            if (ReferenceEquals(obj1, obj2)) return true;
            if ((obj1 == null) || (obj2 == null)) return false;
            if (obj1.GetType() != obj2.GetType()) return false;

            var objJson = JsonConvert.SerializeObject(obj1);
            var anotherJson = JsonConvert.SerializeObject(obj2);

            return objJson == anotherJson;
        }
    }
}
