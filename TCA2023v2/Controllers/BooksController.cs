using Microsoft.AspNetCore.Mvc;
using TCA2023v2_DataAccess;
using TCA2023v2_Domain;

namespace TCA2023v2.Controllers
{

    /*
     * •	File uploaded (that represents the book) is being saved successfully in a folder of your choice with a unique filename; [1]
•	A validation should be implemented to check whether another book with the same name has already been uploaded in which case a message should be displayed on screen and saving not allowed to continue [1.5]
•	A relative path together with other data that describes the book (see class Book in A2.2) is saved in the database  using the proper structure introduced in A2.2 and an injected repository instance in the controller/method [1]
•	A check that does not allow files greater than 10MB should be setup.  If large files are uploaded, then throw an Exception with a relevant message. A try-catch should be applied to the whole method so that if the upload fails (case in point file size exceeded), saving to the database must not be allowed and a proper message should be displayed [1.5]

     */

    public class BooksController : Controller
    {
        public IActionResult Create(Book b, IFormFile myFile, [FromServices] IWebHostEnvironment host, [FromServices] BooksDbRepository repo)
        {
            string absolutePath = host.WebRootPath + "/Books/" + b.Isbn + System.IO.Path.GetExtension(myFile.FileName);
            if (repo.GetBook(b.Isbn) == null)
            {
                if (myFile.Length < 1024*1024*1024)
                {
                    string relativePath = "/Books/" + b.Isbn + System.IO.Path.GetExtension(myFile.FileName);

                    using (var fs = new FileStream(absolutePath, FileMode.CreateNew))
                    {
                        myFile.CopyTo(fs);
                        fs.Close();
                    }
                    b.Path = relativePath;
                    repo.AddBook(b);
                }
            }
            else
            {
                //do not allow book upload
            }
            return View();
        }
    }
}
