﻿using LoanSharkMVC.Models;
using LoanSharkMVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LoanSharkMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult App()
        {
            Loan loan = new()
            {
                Payment = 0.00M,
                TotalInterest = 0.00M,
                TotalCost = 0.00M,
                Rate = 3.50M,
                Amount = 150000.00M,
                Term = 60                
            };

            return View(loan);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult App(Loan loan)
        {
            // Calculate the Loan and Payment Schedule
            var loanHelper = new LoanHelper();

            Loan newLoan = loanHelper.GetPayments(loan);

            return View(newLoan);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}