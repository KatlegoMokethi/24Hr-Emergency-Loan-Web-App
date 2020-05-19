using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BuhleProject.Models;
using BuhleProject.Models.Data;
using BuhleProject.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuhleProject.Controllers.Client
{
    [Authorize]
    public class ClientController : Controller
    {
        IRepositoryWrapper RepositoryWrapper;
        public ClientController(IRepositoryWrapper repositoryWrapper)
        {
            RepositoryWrapper = repositoryWrapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UploadDoc(String id)
        {

            return View("UploadDoc", id);
        }

        [HttpPost]
        public async Task<IActionResult> UploadDoc(string id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");
            else if (!file.FileName.ToLower().Contains(".jpg"))
            {
                if (!file.FileName.ToLower().Contains(".pdf"))
                    return Content("only pdf or picture(jpg) please");
            }

            var arr = file.FileName.Split('.');
            string ext;
            if (arr[arr.Length - 1].Contains("jpg"))
                ext = ".jpg";
            else
                ext = ".pdf";
            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), @"wwwroot\ClientDocuments",
                        id + User.Identity.Name + " " + ext);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var user = RepositoryWrapper.UserRepository.FindByCondition(s => s.Email == this.User.Identity.Name).FirstOrDefault();
            Document doc = new Document()
            {
                DateUploaded = DateTime.Now,
                filename = id + User.Identity.Name + " " + ext,
                UploaderId = user.Id,
                path = path,
                DocType = id
            };
            RepositoryWrapper.DocumentRepository.Create(doc);
            RepositoryWrapper.DocumentRepository.Save();
            if (id == "proof of address")
                user.pAddress = true;
            else if (id == "bank statement")
                user.pBank = true;
            else if (id == "proof of ID")
                user.pID = true;
            else if (id == "proof of registration")
                user.pReg = true;
            else
                user.pStdCard = true;
            ////////save to db later
            user.completed2 = user.pAddress && user.pBank && user.pID && user.pReg && user.pStdCard;
            RepositoryWrapper.UserRepository.Update(user);
            RepositoryWrapper.UserRepository.Save();
            return RedirectToAction("Loans", "Client");
        }

        [HttpGet]
        public IActionResult Documents()
        {
            var userId = RepositoryWrapper.UserRepository.FindByCondition(s => s.Email == this.User.Identity.Name).FirstOrDefault().Id;
            var Documents = RepositoryWrapper.DocumentRepository.FindByCondition(s => s.UploaderId == userId);

            return View(Documents);
        }


        [HttpGet]
        public IActionResult ApplyForLoan()
        {
            var user = RepositoryWrapper.UserRepository.FindByCondition(s => s.Email == this.User.Identity.Name).FirstOrDefault();
            if (!user.completed2)
            {
                return RedirectToAction("Loans", "Client");
            }
            else if (!user.completed3)
            {
                return Redirect("/Client/Terms");
            }
            else
            {
                LoanApplication loanApplication = new LoanApplication();
                loanApplication.estPayDate = loanApplication.applicationDate = DateTime.Now.AddDays(29);
                return View(loanApplication);
            }
        }

        [HttpPost]
        public IActionResult ApplyForLoan(LoanApplication loanApplication)
        {
            if (ModelState.IsValid && loanApplication.amount >= 150 && loanApplication.amount <= 500)
            {
                var userId = RepositoryWrapper.UserRepository.FindByCondition(s => s.Email == this.User.Identity.Name).FirstOrDefault();
                
                //Applicant format used:
                //loanApplication.applicant = userId.Name + " " + userId.Institution + " " + userId.Email + " " + "user id:" + " " + userId.Id;
                
                //Applicant format for develompent purposes only [to be deleted]:
                loanApplication.applicant = userId.Name + " " + userId.Surname + " | UserID:" + " " + userId.Id;
                
                loanApplication.applicationDate = DateTime.Now;
                loanApplication.approved = false;


                RepositoryWrapper.LoanApplicationRepository.Create(loanApplication);
                RepositoryWrapper.LoanApplicationRepository.Save();
                //Commented out email processing for development purposes only: 
                //Email.send(userId.Name, userId.Email, loanApplication.applicationDate.ToString(), loanApplication.amount.ToString("0:00"), "Loan application on " + DateTime.Now.ToShortTimeString(), " Greetings this is to inform you of the application for the loan of: "+loanApplication.amount).Wait();
                //Email.send("admin", "emergencystudentloan@gmail.com", loanApplication.applicationDate.ToString(), loanApplication.amount.ToString("0:00"), " new Loan application on " + DateTime.Now.ToShortTimeString(), " Greetings this is to inform you of the application for the loan of: " + loanApplication.amount+" by "+ userId.Name + " "+userId.Email).Wait();
                return RedirectToAction("Loans", "Client");
            }
            else if (loanApplication.amount < 150 || loanApplication.amount > 500)
            {
                ModelState.AddModelError(" ", "unsuccessful, ensure amount is R150 betwwen and R500");
            }
            else
                ModelState.AddModelError(" ", "unsuccessful, error!!");

            return View(loanApplication);

        }

        [HttpGet]
        public IActionResult Loans()
        {
            var user = RepositoryWrapper.UserRepository.FindByCondition(s => s.Email == this.User.Identity.Name).FirstOrDefault();
            user.completed2 = user.pAddress && user.pBank && user.pID && user.pReg && user.pStdCard;
            if (!user.completed2)
            {
                if (!user.pAddress)
                {
                    //{ "proof of registration", "ID", "student card",  "bank statement", "proof of address" }
                    return UploadDoc("proof of address");
                }
                else if (!user.pBank)
                {
                    return UploadDoc("bank statement");
                }
                else if (!user.pID)
                {
                    return UploadDoc("proof of ID");
                }
                else if (!user.pReg)
                {
                    return UploadDoc("proof of registration");
                }
                else
                {
                    //user.pStdCard
                    return UploadDoc("student card");
                }

            }
            else if (!user.completed3)
            {
                return Redirect("/Client/Terms");
            }
            else
            {
                try
                {
                    var id = user.Id;

                    var t = RepositoryWrapper.LoanRepository.FindByCondition(loan => loan.creditor.Contains(User.Identity.Name));
                    return View(t);
                }
                catch (NullReferenceException)
                {

                    return Redirect("/Client/Documents");
                }
            }

        }
        [AllowAnonymous]
        public IActionResult ViewTerms()
        {
            string path = Environment.CurrentDirectory + @"\wwwroot\final terms and conditions.pdf";
            return new PhysicalFileResult(path, "application/pdf");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {

            var v = RepositoryWrapper.DocumentRepository.GetById(id);
            return View(v);
        }

        [HttpPost]
        public IActionResult Delete(Document doc)
        {
            var user = RepositoryWrapper.UserRepository.FindByCondition(s => s.Email == User.Identity.Name).First();
            if (user.Id == doc.UploaderId)
            {
                RepositoryWrapper.DocumentRepository.Delete(doc);
                RepositoryWrapper.DocumentRepository.Save();
            }
            return View("Loans");
        }

        [HttpGet]
        public IActionResult Terms()
        {
            var user = RepositoryWrapper.UserRepository.FindByCondition(s => s.Email == User.Identity.Name).First();

            return View(user);
        }
        [HttpPost]
        public IActionResult Terms(User user)
        {
            if (ModelState.IsValid && user.Accepted == true)
            {
                user = RepositoryWrapper.UserRepository.FindByCondition(s => s.Email == User.Identity.Name).First();
                user.completed3 = true;
                RepositoryWrapper.UserRepository.Update(user);
                RepositoryWrapper.UserRepository.Save();
                return View("Loans");
            }
            ModelState.AddModelError("unsuccessful", "please read and accpect terms and conditions to continue");
            return View(user);


        }

    }
}