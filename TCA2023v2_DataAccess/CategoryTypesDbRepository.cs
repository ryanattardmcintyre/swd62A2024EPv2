using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCA2023v2_Domain;

namespace TCA2023v2_DataAccess
{
    /*
    1.	CategoriesDbRepository – accesses/manages categories from a relational database.Hence:
a.Implement a method called AddCategory(…) – add category to the database making sure it has a unique name – if it does not, throw an exception; [1]
    b.Implement a method called GetCategories(…) – gets all the categories from the database order by name in ascending mode; [1]
    c.Implement a method called DeleteCategory(…) – deletes a category only if there are no products attached to the category in question; [1]

2.	The CategoriesDbRepository is one way of managing data related to Categories – managing with a database involved.Another way that can be implemented is to add, load and delete categories from a JSON file.
a.Set up the learnt efficient hierarchical structure in order to implement a CategoriesFileRepository in such a way that CategoriesDbRepository and CategoriesFileRepository follow the same rules; Apply any changes required to the above implementations; [1]
    b.Implement CategoriesFileRepository.AddCategory(…) keeping in mind that the new category details are going to be appended to a JSON file and with the same validation as in CategoriesDbRepository and a unique category Id is generated. [1 + 1]
    c.Implement CategoriesFileRepository.GetCategories() keeping in mind that the categories are going to be loaded from a JSON file. [1]
    d.	Implement CategoriesFileRepository.DeleteCategory() keeping in mind that the selected category is going to be deleted from a JSON file. No need to check for any attached products here.  [1]


3.	In order to answer this task, you need to create a simple (MVC) web application, and make sure that CategoriesFileRepository is used by the Injector class by creating one instance per request[1].   Read Task 4 before start answering this task.

4.	Without the need to create any controllers for Books and Categories, you may hard-code a couple of lines of code (in the program.cs or HomeController.cs) to show that the AddCategory works and writes a newly inserted Category in a JSON file (placed somewhere inside the app – even a hardcoded path is ok here). [1]. 
Note: No Login or Db is needed, therefore when creating the new web app, you may select No Authentication while creating it.
Note: this JSON file (“categories.json”) should be submitted with the rest of the files(if done) on vle.
    */


    public class CategoriesFileRepository: ICategoriesRepository
    {
        public CategoriesFileRepository(string _path) { Path = _path; }
        public string Path { get; set; }
        public void AddCategory(CategoryType b)
        {
            var list = GetCategories().ToList();
            list.Add(b);

            System.IO.File.WriteAllText(Path,
                JsonConvert.SerializeObject(list)
                );
        }

        public void DeleteCategory(CategoryType b)
        {
            var list = GetCategories().ToList();
            list.Remove(b);
            System.IO.File.WriteAllText(Path,
                           JsonConvert.SerializeObject(list)
                           );
        }

        public IQueryable<CategoryType> GetCategories()
        {
            if (System.IO.File.Exists(Path) == false)
            {
                using (var f = System.IO.File.CreateText(Path))
                {
                    f.Close();
                }

                return new List<CategoryType>().AsQueryable();
            }
            else
            {
                string contents = System.IO.File.ReadAllText(Path);
                if (contents == "")
                {
                    return new List<CategoryType>().AsQueryable();

                }
                else
                {
                    var list = JsonConvert.DeserializeObject<List<CategoryType>>(contents);
                    return list.AsQueryable<CategoryType>();    
                }
            }
        }
    }



    public interface ICategoriesRepository
    {
        void AddCategory(CategoryType b);
        IQueryable<CategoryType> GetCategories();
       void DeleteCategory(CategoryType b);
    }


    public class CategoriesDbRepository: ICategoriesRepository
    {

        private LibraryContext _libraryContext;
        public CategoriesDbRepository(LibraryContext context)
        {
            _libraryContext = context;
        }

        public void AddCategory(CategoryType b)
        {
            _libraryContext.CategoryTypes.Add(b);
            _libraryContext.SaveChanges(); //<<<<< important
        }

        public IQueryable<CategoryType> GetCategories()
        {
            return _libraryContext.CategoryTypes;
        }

        public void DeleteCategory(CategoryType b) {
        
            if(_libraryContext.Books.Count(x=>x.CategoryTypeFK == b.Id) == 0)
            {
                _libraryContext.Remove(b); 
                _libraryContext.SaveChanges();

            }

        }
    }
}
