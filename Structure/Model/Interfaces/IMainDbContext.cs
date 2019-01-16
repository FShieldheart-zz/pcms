using SQLite;

namespace Structure.Model.Interfaces
{
    public interface IMainDbContext
    {
        SQLiteAsyncConnection GetCurrentContext();

        void Dispose();
    }
}