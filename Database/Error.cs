using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCRUD.Database
{
    public partial class Error
    {
        private int _number;

        public int number
        {
            get { return _number; }
            set { _number = value; }
        }

        private string _message;

        public string message
        {
            get { return _message; }
            set { _message = value; }
        }

        private string _source;

        public string source
        {
            get { return _source; }
            set { _source = value; }
        }
    }
}