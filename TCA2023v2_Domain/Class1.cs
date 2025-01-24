﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCA2023v2_Domain
{
    /*
     * 
     * a.	Classes implementation (above) – 
i.	Ids should be set as primary key and identity. It should be autogenerated meaning that it should be filled automatically when a new book is inserted in the database – this has to be done from the code not from the database [1];
ii.	CategoryType should be referenced in Book by means of a foreign key and a navigational field. Therefore make the required changes [1];
b.	LibraryDbContext  [1] – this should be an abstraction of the database accommodating all the requests/references in this task;
c.	BooksDbRepository – accesses/manages books entities from a relational database. Methods you should implement:
i.	AddBook – should be efficiently built; [1]
ii.	GetBook – gets only ONE book or nothing – the requested ONE; [1]
iii.	GetBooks – gets all books; method should be built in such a way that query is not executed immediately but the call is postponed to when the ToList() method is called [1]


     * */
   public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public string Path { get; set; }

        [ForeignKey("CategoryType")]
        public int CategoryTypeFK { get; set; } //foreign key
        public virtual CategoryType CategoryType { get; set; } //navigational property

        public Book()
        {
            Id = Guid.NewGuid();
        }
    }

    public class CategoryType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; } 
        public string Title { get; set; }
    }
}