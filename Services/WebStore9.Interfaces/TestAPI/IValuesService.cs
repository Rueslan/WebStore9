namespace WebStore9.Interfaces.TestAPI
{
    public interface IValuesService
    {
        IEnumerable<string> GetAll();

        int Count();

        string GetById(int id);

        void Add(string value);

        void Edit(int id, string value);

        bool Delete(int id);
    }
}
