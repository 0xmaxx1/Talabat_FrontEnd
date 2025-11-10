using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models.Order_Aggregate
{
    public class Address
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }



        // Empty Parameterless Ctor -->
        // Entity Framework needs it to create an instance of the class when mapping the data from the database to the object.
        public Address()
        {
            
        }

        // Ctor with Parameters -->
        // This constructor is used to create an instance of the Address class with specific values for each property.
        public Address(string firstName, string lastName, string street, string city, string country)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            Country = country;
        }
    }
}
