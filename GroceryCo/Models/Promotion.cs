using System;

namespace GroceryCo.Models
{
    /// <summary>
    /// Abstract class for modeling a promotion
    /// </summary>
    public abstract class Promotion
    {
        public int Id { get; set; }      

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public abstract string GetDescription();       

        public abstract decimal GetEffectivePrice(int quantity, decimal regularPrice);
    }
}
