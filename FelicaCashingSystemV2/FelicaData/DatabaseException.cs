﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaData
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string msg)
            : base(msg)
        {
        }
    }
}
