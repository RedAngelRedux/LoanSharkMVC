using LoanSharkMVC.Models;
using System.Security.Cryptography.X509Certificates;

namespace LoanSharkMVC.Helpers
{
    public class LoanHelper
    {
        public Loan GetPayments(Loan loan)
        {

            // Calculate Monthly Payment
            loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);

            // Create loop from 1 to term
            var balance = loan.Amount;
            var totalInterest = 0.00M;
            var monthlyInterest = 0.00M;
            var monthlyPrimcipal = 0.00M;
            var monthlyRate = CalcMonthlyRate(loan.Rate);

            for (int month = 0; month < loan.Term; month++)
            {   // Calculate payment scedule

                monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrimcipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrimcipal;

                LoanPayment loanPayment = new();

                loanPayment.Month = month;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyPrincipal = monthlyPrimcipal;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;

                // Push the payments into the loan
                loan.Payments.Add(loanPayment);

            }

            // Update loan object with total costs
            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + loan.TotalInterest;

            return loan;
        }

        private decimal CalcPayment(decimal amount, decimal rate, int term)
        {

            var rateD = Convert.ToDouble(CalcMonthlyRate(rate));
            var amountD = Convert.ToDouble(amount);
            var paymentD = (amountD * rateD) / (1 - Math.Pow(1 + rateD, -term));


            return Convert.ToDecimal(paymentD);
        }

        private decimal CalcMonthlyRate(decimal rate)
        {
            return rate / 1200;
        }
        private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;
        }
    }
}
