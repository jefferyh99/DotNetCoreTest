﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFactTest
{
    public class Test1 : ITest
    {
        public string GetTestName()
        {
            return "I am Test1";
        }
    }

}
