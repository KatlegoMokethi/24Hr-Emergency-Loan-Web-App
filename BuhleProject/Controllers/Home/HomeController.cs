using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuhleProject.Models.Data;
using BuhleProject.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BuhleProject.Controllers
{
    public class HomeController : Controller
    {
        IRepositoryWrapper wrapper;
        public HomeController(IRepositoryWrapper _wrapper)
        {
            wrapper = _wrapper;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}