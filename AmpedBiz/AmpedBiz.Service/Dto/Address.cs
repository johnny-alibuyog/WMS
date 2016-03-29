namespace AmpedBiz.Service.Dto
{
    public class Address
    {
        public virtual string Street { get; set; }

        public virtual string Barangay { get; set; }

        public virtual string City { get; set; }

        public virtual string Province { get; set; }

        public virtual string Region { get; set; }

        public virtual string Country { get; set; }

        public virtual string ZipCode { get; set; }
    }
}