using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class Min18YearsIfAMember : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //this is an object that gives us access to the containing class
            //so we cast it to Customer
            var customer = (Customer)validationContext.ObjectInstance; 

            //if their membership type is pay as you go, for which being 18 is not required, give success msg
            if (customer.MembershipTypeId == MembershipType.Unknown || customer.MembershipTypeId == MembershipType.PayAsYouGo)
                return ValidationResult.Success;

            //if the data was not entered - override the error message
            if (customer.Birthdate == null)
                return new ValidationResult("Birthdate is required.");

            //then calculate the age (this is not the real age calculation)
            var age = DateTime.Today.Year - customer.Birthdate.Value.Year;
            return (age >= 18)
                ? ValidationResult.Success
                : new ValidationResult("Customer should be at least 18 years old to have a membership");
        }
    }
}