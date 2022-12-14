using AutomobileRepairShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf.Parsing;
using System.Collections;
using System.Diagnostics;
using System.Dynamic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace AutomobileRepairShop.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private AutoRSContext db = new AutoRSContext();
        private List<CarPart> carParts = new List<CarPart>();
        private List<User> users = new List<User>();
        private List<Appointment> appointments = new List<Appointment>();
        private static List<Appointment> appointList = new List<Appointment>();
        private static List<AppointClasses> appointClasses = new List<AppointClasses>();
        private List<Car> carsList = new List<Car>();
        private dynamic mymodel = new ExpandoObject();

        [Authorize(Roles = "Employee")]
        public ActionResult Bills()
        {
            appointClasses.Clear();
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


        public JsonResult CreateBill([FromBody] List<CarPart> partsIds)
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
                description = description + carPart.Name + ' ' + carPart.Price + '+' + carPart.LaborPrice + '@';
                price += Convert.ToInt32(carPart.Price) + Convert.ToInt32(carPart.LaborPrice);
            }
            // Generate Database entry
            Bill bill = CreateBillEntry(appointment,description,price);
            // return bill Id and name
            string date = appointment.Date.ToString("yyyyMMdd");
            string fileName = name + surname + "_" + date + ".pdf";
            return Json(new { data = bill.Id , name = fileName});
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
            appointment.Finished = true;
            db.Appointments.Update(appointment);
            db.SaveChanges();
            return bill;
        }

        // create bill based on bill id
        public ActionResult CreateDocument(Bill bill)
        {
            User user = db.Users.FirstOrDefault(x => x.Id == bill.UserId);
            string name = user.Name;
            string surname = user.Surname;
            string description = DescriptionDecode(bill.Description); 
            double finalPrice = bill.Price;

            //Get appointment related to the bill
            int appId = bill.AppointmentId;
            Appointment thisapp = db.Appointments.FirstOrDefault(x => x.Id == appId);

            //Load the pdf template from the project directory
            string root = Directory.GetCurrentDirectory();
            string fileName = root + "/wwwroot/docs/Bill_Example2.pdf";
            FileStream docStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(docStream);
            PdfLoadedForm form = loadedDocument.Form;
            //Edit each field using the fields' name (inspect the doc in browser)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            (form.Fields["name8[first]"] as PdfLoadedTextBoxField).Text = name;
            (form.Fields["name8[last]"] as PdfLoadedTextBoxField).Text = surname;
            (form.Fields["description6"] as PdfLoadedTextBoxField).Text = description;
            (form.Fields["finalPrice11"] as PdfLoadedTextBoxField).Text = finalPrice.ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            //Define the file name.
            string date = thisapp.Date.ToString("yyyyMMdd");
            fileName = name + surname + "_" + date + ".pdf";
            //Create the desired pdf file and return it to the user
            MemoryStream stream = new MemoryStream();
            loadedDocument.Save(stream);
            //If the position is not set to '0' then the PDF will be empty.
            stream.Position = 0;
            //Close the document.
            loadedDocument.Close(true);
            //Defining the ContentType for pdf file.
            string contentType = "application/pdf";
            //Creates a FileContentResult object by using the file contents, content type, and file name.
            stream.Position = 0;
            CreateMessageWithAttachment("smtp.gmail.com", "dragos.duma@gmail.com"); //TO DO: ADD FILE AS A PARAMETER HERE MAYBE OR FIND A WAY TO SEND IT TO THIS METHOD
            return File(stream, contentType, fileName);                             //Second TO DO: in loc de emailul meu trebuie sa luam emailul userului care a facut appointmentul pt factura asta
        }

        [Authorize (Roles = "Customer,Employee")]
        public IActionResult DownloadFile(int billId)
        {

            Bill bill = db.Bills.FirstOrDefault(x => x.Id == billId);
            User customer = null;
            Debug.WriteLine(billId + "bill ID");
            customer = db.Users.FirstOrDefault(x => x.Id == bill.UserId);
            if (customer != null || GetRole() == "Employee")
            {
                return CreateDocument(bill);
            }
            return RedirectToAction("Index","Home");
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
                bool found = false;
                appointClasses.Clear();
                foreach (Appointment app in appointments)
                { 
                    if (app.IdUser == u.Id && app.Finished == false)
                    {
                        found = true;
                        Car car = db.Cars.Single(model => model.Id == app.IdCar);
                        AppointClasses thisapp = new AppointClasses(app, car, u);
                        appointClasses.Add(thisapp);
                        Debug.Write("App gasit:" + thisapp.User.Name + " " + thisapp.Car.Brand + " " + thisapp.Appointment.Date);
                    }
                }
                if (found == true)
                    return Json(new { status = true });

            }
            return Json(new { status = false });
        }

        public static void CreateMessageWithAttachment(string server, string emailReceiver)
        {
            // Specify the file to be attached and sent.
            // This example assumes that a file named Data.xls exists in the
            // current working directory.
            // Create a message and set up the recipients.
            MailMessage message = new MailMessage(
                "automobilerepairshop1234@gmail.com",
                emailReceiver,
                "Your receipt has been emitted.",
                "See the attached pdf file.");

            // Add the file attachment to this email message.
            //var attachment = new Attachment(file, "report.pdf", MediaTypeNames.Application.Pdf); // AM INCERCAT SA IL PREIAU AICI CA SI FileStream dar nu o mers
            //message.Attachments.Add(attachment);

            //Send the message.


            // Add credentials if the SMTP server requires them.
            SmtpClient client = new SmtpClient(server, 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("automobilesrepairshop1234@gmail.com", "bkrnlpnxztyxqjxo");

            try
            {

                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateMessageWithAttachment(): {0}",
                    ex.ToString());
            }
            // Display the values in the ContentDisposition for the attachment.
            //ContentDisposition cd = attachment.ContentDisposition;
            //Console.WriteLine("Content disposition");
            //Console.WriteLine(cd.ToString());
            //Console.WriteLine("File {0}", cd.FileName);
            //Console.WriteLine("Size {0}", cd.Size);
            //Console.WriteLine("Creation {0}", cd.CreationDate);
            //Console.WriteLine("Modification {0}", cd.ModificationDate);
            //Console.WriteLine("Read {0}", cd.ReadDate);
            //Console.WriteLine("Inline {0}", cd.Inline);
            //Console.WriteLine("Parameters: {0}", cd.Parameters.Count);
            //foreach (DictionaryEntry d in cd.Parameters)
            //{
            //    Console.WriteLine("{0} = {1}", d.Key, d.Value);
            //}
            //attachment.Dispose();
        }
    }
}

