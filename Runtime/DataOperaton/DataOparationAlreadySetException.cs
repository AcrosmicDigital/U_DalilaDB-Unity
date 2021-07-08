using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U.DalilaDB
{
    public class DataOparationAlreadySetException : Exception
    {

        const string error = " An altern operation can only be set once";

        public DataOparationAlreadySetException() : base(error) { }

        public DataOparationAlreadySetException(string message) : base(message + error) { }

        public DataOparationAlreadySetException(string message, Exception inner) : base(message + error, inner) { }

    }
}
