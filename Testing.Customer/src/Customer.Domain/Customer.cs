using System;

namespace Customer.Domain
{
    public class Customer : Entity
    {
        public Customer(string fullName, string cityCode, DateTime birthDate)
        {
            SetFields(fullName, cityCode, birthDate);
        }

        public string FullName { get; private set; }
        public string CityCode { get; private set; }
        public DateTime BirthDate { get; private set; }


        public void SetFields(string fullName, string cityCode, DateTime birthDate)
        {
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(cityCode) ||
                birthDate.Date == DateTime.Today)
                throw new ArgumentException($"{fullName} || {cityCode} || {birthDate} is invalid");


            FullName = fullName;
            CityCode = cityCode;
            BirthDate = birthDate;
        }
    }
}