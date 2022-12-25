﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SqlDbApplication.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string errorMessage, Exception inner) : base(errorMessage, inner)
        {
        }
    }
}
