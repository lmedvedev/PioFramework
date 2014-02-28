﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeaHousePeople.Samples.DataObjects
{
    public class Author
    {
        private int _id;
        private string _name;

        public Author()
        {
        }
        public Author(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
