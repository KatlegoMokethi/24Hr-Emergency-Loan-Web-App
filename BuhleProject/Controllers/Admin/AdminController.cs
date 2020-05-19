using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuhleProject.Models;
using BuhleProject.Models.Data;
using BuhleProject.Models.Entities;
using ConvertApiDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuhleProject.Controllers.Admin
{
    [Authorize(Roles ="Admins")]
    public class AdminController : Controller
    {
        IRepositoryWrapper repositoryWrapper;
        public AdminController(IRepositoryWrapper _repositoryWrapper)
        {
            repositoryWrapper = _repositoryWrapper;
        }
        // GET: Admin
        [HttpGet]
        public ActionResult ApplicantDetails(string id)
        {
            var arr = id.Split(" ");
            var email = arr[arr.Length - 1];
            var v = repositoryWrapper.UserRepository.FindByCondition(s => s.Id == int.Parse(email)).First();
            return View(repositoryWrapper.DocumentRepository.FindByCondition(s => s.UploaderId == v.Id));
        }

        // GET: Admin/Details/5
        [HttpGet]
        public ActionResult Applications()
        {
            var v = repositoryWrapper.LoanApplicationRepository.FindByCondition(s=> !s.rejected && !s.approved ).OrderBy(s=>s.applicationDate).ThenByDescending(s => s.applicationDate);

            return View(v);
        }


        public ActionResult CurrentLoans()
        {
            var v = repositoryWrapper.LoanRepository.FindByCondition(s => s.paid == false).OrderBy(s=>s.payDate);
            return View(v);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var v = repositoryWrapper.LoanApplicationRepository.GetById(id);
            return View(v);
        }

        [HttpPost]
        public ActionResult Details(LoanApplication loan)
        {
            if (loan.approved)
            {
                Loan l = new Loan()
                {
                    amount = loan.amount,
                    paid = false,
                    payDate = DateTime.Now.AddDays(30),
                    creditor = loan.applicant

                };
                repositoryWrapper.LoanRepository.Create(l);
                repositoryWrapper.LoanRepository.Save();
                var arr = loan.applicant.Split(" ");
                var email = arr[arr.Length - 1];
                var v = repositoryWrapper.UserRepository.FindByCondition(s => s.Id == int.Parse(email)).First();
                //Temporarily turned off for development testing purposes only:
                //Email.send(v.Name + " " + v.Surname, v.Email, loan.applicationDate.ToString(), String.Format("{0:0.00}", loan.amount) ,"loan approved", "greetings " + v.Name + ",\n your application for the loan of the amount: " + loan.amount + " has been approved on the date: " + DateTime.Now + "\nThe expected repayment date is: " + l.payDate).Wait();
                return RedirectToAction("Applications", "Admin");

            }
            else
            {
                repositoryWrapper.LoanApplicationRepository.Update(loan);
                repositoryWrapper.LoanApplicationRepository.Save();
            }

            return RedirectToAction("Applications","Admin"); 
        }

        public ActionResult ViewDocument(int id)
        {
            var v = repositoryWrapper.DocumentRepository.GetById(id);
            string path = v.path;
            if (path.Contains("pdf"))
            {
                return new PhysicalFileResult(path, "application/pdf");
            }
            else
            {

                //var convertApi = new ConvertApi("q76StEnz8lzCQeWR");
                //convertApi.ConvertAsync("jpg", "pdf",
                //    new ConvertApiFileParam(@"C:\path\to\my_file.jpg")
                //).Result.SaveFilesAsync(@"C:\converted-files\");
                return View("openPic", v);
            }

        }
        // GET: Admin/Create

        public ActionResult Send()
        {
            Email.send();
            return View("Applications");
        }
        
        public ViewResult openPic(Document pic)
        { 
            return View(pic);
        }
    }
}