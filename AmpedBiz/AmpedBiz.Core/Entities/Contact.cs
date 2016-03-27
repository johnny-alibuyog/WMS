namespace AmpedBiz.Core.Entities
{
    public class Contact : ValueObject<Contact>
    {
        public virtual string Email { get; set; }

        public virtual string Landline { get; set; }

        public virtual string Fax { get; set; }

        public virtual string Mobile { get; set; }

        public virtual string Web { get; set; }
    }
}