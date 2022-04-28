using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf.Parsing;
using System.Diagnostics;
using System.Web;
using System.Dynamic;
using System.Windows;

namespace AutomobileRepairShop.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private AutoRSContext db = new AutoRSContext();
        private static List<CarPart> carParts = new List<CarPart>();
        private List<User> users = new List<User>();
        private List<Appointment> appointments = new List<Appointment>();
        private static List<Appointment> appointList = new List<Appointment>();
        private static List<AppointClasses> appointClasses = new List<AppointClasses>();
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
            return View(mymodel);
        }

        [Authorize(Roles = "Employee")]
        public ActionResult CarParts()
        {
            carParts.Clear();
            ViewBag.IsLogged = IsLogged();
            ViewBag.IsEmployee = IsEmployee();
            mymodel.CarParts = db.CarParts.ToList();
            mymodel.AddedCarParts = carParts;
            mymodel.AppointList = appointClasses;
            Debug.WriteLine("CarParts Method");
            return View(mymodel);
        }

        [HttpPost]
        public JsonResult BillsAdd([FromBody] List<CarPart> array)
        {
            carParts.Clear();
            foreach (CarPart cp in array)
            {
                CarPart carPart = db.CarParts.Single(model => model.Id == cp.Id);
                carParts.Add(carPart);
                Debug.Write(carPart.Id + " ");
            }
            Debug.WriteLine("");
            return Json(new { success = true });
        }
        [HttpPost]
        public ActionResult CreateBills([FromBody] List<CarPart> array)
        {
            carParts.Clear();
            foreach (CarPart cp in array)
            {
                CarPart carPart = db.CarParts.Single(model => model.Id == cp.Id);
                carParts.Add(carPart);
                Debug.Write(carPart.Id + " ");
            }
            Debug.WriteLine("");
            return RedirectToAction("Bills", "Employee");
        }

        public JsonResult CreateBill([FromBody] List<CarPart> partsIds)//, int appointmentId)
        {
            Debug.WriteLine("Creating bill");

            int appointmentId = partsIds.LastOrDefault().Id;
            partsIds.RemoveAt(partsIds.Count - 1);

            Appointment appointment = db.Appointments.FirstOrDefault(x => x.Id == appointmentId);
            User user = db.Users.FirstOrDefault(x => x.Id == appointment.IdUser);
            if (user == null)
            {
                Debug.WriteLine("User does not exist!");
                return Json(false);
            }
            string name = user.Name;
            string surname = user.Surname;

            string description = "";
            int price = 0;
            foreach (CarPart cp in partsIds)
            {
                CarPart carPart = db.CarParts.Single(model => model.Id == cp.Id);
                description = description + carPart.Name + '@';
                price += Convert.ToInt32(carPart.Price);
            }
            // Generate Database entry
            Bill bill = CreateBillEntry(appointment,description,price);

            return Json(new { data = CreateDocument(bill) });
        }

        public Bill CreateBillEntry(Appointment appointment, string description, int price)
        {
            Debug.WriteLine("Creating Entry");
            Bill bill = new Bill();
            bill.Description = description;
            bill.UserId = appointment.IdUser;
            bill.Price = price;

            bill.AppointmentId = appointment.Id;
            bill.CarId = appointment.IdCar;
            db.Bills.Add(bill);
            db.SaveChanges();
            return bill;
        }

        

        public string CreateDocument(Bill bill)
        {
            User user = db.Users.FirstOrDefault(x => x.Id == bill.UserId);
            string name = user.Name;
            string surname = user.Surname;
            string description = DescriptionDecode(bill.Description); 
            double finalPrice = bill.Price;

            Debug.WriteLine("Creating Document");
            //Load the pdf template from the project directory
            string root = Directory.GetCurrentDirectory();
            string fileName = root + "/wwwroot/docs/Bill_Example2.pdf";
            FileStream docStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(docStream);
            PdfLoadedForm form = loadedDocument.Form;

            //Edit each field using the fields' name (inspect the doc in browser)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            (form.Fields["name8[first]"] as PdfLoadedTextBoxField).Text = surname;
            (form.Fields["name8[last]"] as PdfLoadedTextBoxField).Text = name;
            (form.Fields["description6"] as PdfLoadedTextBoxField).Text = description;
            (form.Fields["finalPrice11"] as PdfLoadedTextBoxField).Text = finalPrice.ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.


            //Define the file name.
            string date = DateTime.Now.ToString("yyyyMMdd");
            fileName = name + surname + "_" + date + ".pdf";
            //Create the desired pdf file and return it to the user
            FileStream stream = new FileStream(root + "\\wwwroot\\docs\\" + fileName, FileMode.Create, FileAccess.ReadWrite);
            loadedDocument.Save(stream);
            //If the position is not set to '0' then the PDF will be empty.
            stream.Position = 0;
            //Close the document.
            loadedDocument.Close(true);
            
            //Defining the ContentType for pdf file.
            string contentType = "application/pdf";
            


            //Creates a FileContentResult object by using the file contents, content type, and file name.
            stream.Position = 0;
            FileStreamResult fileResult = File(stream, contentType, fileName);
            stream.Close();
            
            return fileName;
        }

        public IActionResult DownloadFile(string fileName)
        {
            string root = Directory.GetCurrentDirectory() + "\\wwwroot\\docs\\";
            string filePath = root + fileName;
            string contentType = "application/pdf";
            FileStream docStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(docStream,contentType, filePath);
        }

        public string DescriptionDecode(string description)
        {
            return description.Replace('@', '\n');
        }

        public ActionResult Appointments()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SearchEmail([FromBody] User userMail)
        {
            users = db.Users.ToList();
            appointments = db.Appointments.ToList();

            Debug.WriteLine("SearchEmail entered");
            Debug.WriteLine(userMail.Email);
            User u = db.Users.FirstOrDefault(model => model.Email.ToLower() == userMail.Email.ToLower());
            if (u == null)
            {
                ViewBag.Message = "User doesn't exist";
                return Json(new { status = false });
            }
            else
            {
                Debug.WriteLine("User gasit");
                foreach (Appointment app in appointments)
                {
                    if (app.IdUser == u.Id && app.Finished == false)
                    {
                        Car car = db.Cars.Single(model => model.Id == app.IdCar);
                        AppointClasses thisapp = new AppointClasses(app, car, u);
                        appointClasses.Add(thisapp);
                        Debug.Write("App gasit:" + thisapp.User.Name + " " + thisapp.Car.Brand + " " + thisapp.Appointment.Date);
                    }
                }

                return Json(new { status = true });
            }   
        }
    }
}

