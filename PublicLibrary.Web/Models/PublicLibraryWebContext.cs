﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PublicLibrary.Web.Models
{
    public class PublicLibraryWebContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public PublicLibraryWebContext() : base("name=PublicLibraryWebContext")
        {
        }

        public System.Data.Entity.DbSet<PublicLibrary.Domain.Book> Books { get; set; }

        public System.Data.Entity.DbSet<PublicLibrary.Domain.Author> Authors { get; set; }

        public System.Data.Entity.DbSet<PublicLibrary.Domain.User> Users { get; set; }
    }
}
