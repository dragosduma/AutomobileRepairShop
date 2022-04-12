using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf.Parsing;
using System.Diagnostics;
using System.Web;
using System.Dynamic;

namespace AutomobileRepairShop.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private AutoRSContext db = new AutoRSContext();
        private List<CarPart> carParts = new List<CarPart>();
        private List<User> users = new List<User>();
        private List<Appointment> appointments = new List<Appointment>();
        private List<Appointment> appointList = new List<Appointment>();
        private List<Car> carsList = new List<Car>();
        private dynamic mymodel = new ExpandoObject();

        [Authorize(Roles = "Employee")]
        public ActionResult Bills()
        {
            ViewBag.IsLogged = IsLogged();
            ViewBag.IsEmployee = IsEmployee();   
            mymodel.CarParts = db.CarParts.ToList();
            mymodel.AddedCarParts = carParts;
            mymodel.AppointList = appointList;
;           return View(mymodel);
        }

        [Authorize(Roles = "Employee")]
        public ActionResult CarParts()
        {
            ViewBag.IsLogged = IsLogged();
            ViewBag.IsEmployee = IsEmployee();
            mymodel.CarParts = db.CarParts.ToList();
            mymodel.AddedCarParts = carParts;
            mymodel.AppointList = appointList;
            return View(mymodel);
        }

        [HttpPost]
        public JsonResult BillsAdd([FromBody]List<CarPart> array)
        {
            foreach (CarPart cp in array)
            {
                CarPart carPart = db.CarParts.Single(model => model.Id == cp.Id);
                carParts.Add(carPart);
                Debug.Write(carPart.Id+" ");
            }
            Debug.WriteLine("");
            return Json(new { success = true });
        }
        [HttpPost]
        public ActionResult CreateBills([FromBody] List<CarPart> array)
        {
            foreach (CarPart cp in array)
            {
                CarPart carPart = db.CarParts.Single(model => model.Id == cp.Id);
               
                Debug.Write(carPart.Id + " ");
            }
            Debug.WriteLine("");
            return RedirectToAction("Bills","Employee") ;
        }

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

        [HttpPost]
        public ActionResult SearchEmail([FromBody] User userMail)
        {
            users = db.Users.ToList();
            appointments = db.Appointments.ToList();

            Debug.WriteLine("SearchEmail entered");
            Debug.WriteLine(userMail.Email);
            foreach(User u in users)
            {
                if (u.Email == userMail.Email)
                {
                    Debug.WriteLine("User gasit");
                    int idUser = u.Id;
                    foreach(Appointment app in appointments)
                    {
                        if(app.IdUser == idUser && app.Finished == false)
                        {
                            appointList.Add(app);
                            Debug.Write("App gasit");
                        }
                    }

                }
            }
            foreach(Appointment app in appointList)
            {
                Debug.Write("App gasit" + app.Id + " " + app.IdCar + " " + app.IdUser);
            }
            mymodel.AppointList = appointList;
            return RedirectToAction("Bills","Employee");
        }     
    }
}
