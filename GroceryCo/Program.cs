using System;
using System.Collections.Generic;
using GroceryCo.Models;
using GroceryCo.Repositories;
using GroceryCo.Services;

namespace GroceryCo
{
    class Program
    {
        static void Main(string[] args)
        {
            //To see sample data used here, see Repositories\FakeDataContext.cs

            var unitOfWork = new UnitOfWork();
            var salesService = new SalesService(unitOfWork);

            var sampleTransactions = new List<ISaleTransaction>();

            //sample 1: a sale with a product (id=4) which has a percent discount promotion (50% sale)
            sampleTransactions.Add(KioskUtilities.CreateSaleTransaction(new[] { 3, 1, 2, 1, 4, 3, 4 }));

            //sample 2: sale with a product (id=6) which has a bulk promotion (Buy 3 for $5)
            sampleTransactions.Add(KioskUtilities.CreateSaleTransaction(new[] { 1, 6, 3, 6, 6 }));

            //sample 3: sale with a product (id=7) which has a bogo promotion (Buy 2, get one for 50%) 
            sampleTransactions.Add(KioskUtilities.CreateSaleTransaction(new[] { 7, 1, 7, 3, 7, 1, 2, 3 }));

            //sample 4: sale with a product (id=8) which has two promotions (10% Student Discount, and Buy 3 for $5)
            sampleTransactions.Add(KioskUtilities.CreateSaleTransaction(new[] { 8, 1, 8, 3, 8, 1, 2, 3, 8 }));

            //sample 5: sale with multiple products on sale
            // - product 9 is on 3 promotions (50% Sale, 10% Student Discount, Buy 2, get one for 50%)
            // - product 6 is on 1 promotion (Buy 3 for $5)
            sampleTransactions.Add(KioskUtilities.CreateSaleTransaction(new[] { 9, 1, 6, 6, 9, 3, 9, 1, 6, 2, 3, 9, 2, 3, 1, 1 }));

            var count = 0;
            foreach(var transaction in sampleTransactions)
            {
                count++;

                salesService.Checkout(transaction);
                var regularTotal = transaction.GetRegularTotal();
                var effectiveTotal = transaction.GetEffectiveTotal();
                var savings = regularTotal - effectiveTotal;

                Console.WriteLine($"Sample {count}: Total = {effectiveTotal:c2}, Savings = {savings:c2}");
                Console.WriteLine(Environment.NewLine);
            }
                        
            Console.WriteLine("Hit enter to exit...");
            Console.ReadLine();
        }
    }
}