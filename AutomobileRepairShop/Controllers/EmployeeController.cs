using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf.Parsing;
using System.Diagnostics;
using System.Web;

namespace AutomobileRepairShop.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private AutoRSContext db = new AutoRSContext();
        private CarPart carPartsList = new CarPart();

        [Authorize(Roles = "Employee")]
        public ActionResult Bills()
        {
            ViewBag.IsLogged = IsLogged();
            ViewBag.IsEmployee = IsEmployee();
            return View(db.CarParts.ToList());
        }
        [HttpPost]
        public JsonResult Bills(string ItemId)
        {
            CarPart carPart = db.CarParts.Single(model => model.Id.ToString() == ItemId);
            carPartsList.partsList.Add(carPart);
            foreach(CarPart cp in carPartsList.partsList) {
                Debug.WriteLine(cp.Name);
            }
            return Json(new {success=true,Counter=carPartsList.partsList.Count});
        }

        public ActionResult CreateBill(List<CarPart> parts, int userId)
        {
            User user = db.Users.FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                Debug.WriteLine("User does not exist!");
                return Redirect("/");
            }
            string name = user.Name;
            string surname = user.Surname;

            string description = "";
            int price = 0;
            foreach (CarPart cp in parts)
            {
                description = description + cp.Name + '\n';
                price += Convert.ToInt32(cp.Price);
            }

            // Generate Database entry
            CreateBillEntry(description,userId,price);

            // Generate Document
            CreateDocument(name, surname, description, price);

            return Redirect("/");
        }

        public ActionResult CreateBillEntry(string description, int userId, int price)
        {
            Bill bill = new Bill();
            bill.Description = description;
            bill.UserId = userId;
            bill.Price = price;

            //to be added

            //bill.Appointment_ID = appointmentID;
            //bill.Car_ID = carID;
            db.Bills.Add(bill);
            db.SaveChanges();
            return Redirect("/");
        }

        public ActionResult CreateDocument(string name, string surname, string description, int finalPrice)
        {
            //Load the pdf template from the project directory
            string root = Directory.GetCurrentDirectory();
            string fileName = root + "/wwwroot/docs/Bill_Example.pdf";
            FileStream docStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(docStream);
            PdfLoadedForm form = loadedDocument.Form;

            //Edit each field using the fields' name (inspect the doc in browser)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            (form.Fields["name8[first]"] as PdfLoadedTextBoxField).Text = surname;
            (form.Fields["name8[last]"] as PdfLoadedTextBoxField).Text = name;
            (form.Fields["description6"] as PdfLoadedTextBoxField).Text = description;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

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
            string date = DateTime.Now.ToString("yyyyMMdd");
            fileName = date+"_"+name+surname;
            //Creates a FileContentResult object by using the file contents, content type, and file name.
            return File(stream, contentType, fileName);
        }

        public ActionResult Appointments()
        {
            return View();
        }

    }
}
