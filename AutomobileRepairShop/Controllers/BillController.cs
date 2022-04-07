using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using Syncfusion.Pdf.Parsing;

namespace AutomobileRepairShop.Controllers
{
    public class BillController : Controller
    {
        public ActionResult CreateDocument()
        {
            //Load the pdf template from the project directory
            string root = Directory.GetCurrentDirectory();
            string fileName = root + "/wwwroot/docs/Bill_Example.pdf";
            FileStream docStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(docStream);
            PdfLoadedForm form = loadedDocument.Form;
            
            //Edit each field using the fields' name (inspect the doc in browser)
            (form.Fields[0] as PdfLoadedTextBoxField).Text = "John";
            (form.Fields["name8[last]"] as PdfLoadedTextBoxField).Text = "Doe";
            (form.Fields["description6"] as PdfLoadedTextBoxField).Text = "test description";
            
            //Create the desired pdf file and return it to the user
            MemoryStream stream = new MemoryStream();
            loadedDocument.Save(stream);
            //If the position is not set to '0' then the PDF will be empty.
            stream.Position = 0;
            //Close the document.
            loadedDocument.Close(true);
            //Defining the ContentType for pdf file.
            string contentType = "application/pdf";
            //Define the file name.
            fileName = "output.pdf";
            //Creates a FileContentResult object by using the file contents, content type, and file name.
            return File(stream, contentType, fileName);
        }
    }
}
