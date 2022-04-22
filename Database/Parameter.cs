using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace webCRUD.Database
{
    public partial class Parameter
    {
        private string _name;

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _value;

        public string value
        {
            get { return _value; }
            set { _value = value; }
        }

        private SqlDbType _dataType;

        public SqlDbType dataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        private ParameterDirection _direction;

        public ParameterDirection direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        private int? _size;

        public int? size
        {
            get { return _size; }
            set { _size = value; }
        }
    }
}