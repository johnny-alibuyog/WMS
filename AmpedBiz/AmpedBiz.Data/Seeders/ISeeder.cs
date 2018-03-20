namespace AmpedBiz.Data.Seeders
{
    public interface ISeeder
    {
        bool IsSourceExternalFile { get; }
        void Seed();
    }
}
