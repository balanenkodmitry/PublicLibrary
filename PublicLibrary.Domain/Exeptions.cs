using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicLibrary.Domain
{
    public class DataBaseAlreadyHasElementException<T> : Exception
    {
        public DataBaseAlreadyHasElementException(T Object) : base("Object already in data base")
        {            
            string mess = Object.ToString();
        }
    }
}
