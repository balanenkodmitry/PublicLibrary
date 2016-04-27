﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicLibrary.Domain
{
    public class Book
    {
        public List<Author> Autors { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
        public Availability BookAvailability { get; set; }
        public int? WhosTaken_User { get; set; }

        public enum Availability
        {
            Missing = 0,
            Available = 1,
            Unavailable = 2
        }

        public Book() { }

        public Book(string Name, Availability BookAvailability, List<Author> Autors)
        {
            this.Name = Name;
            this.BookAvailability = BookAvailability;
            this.Autors = Autors;
        }

    }
}
