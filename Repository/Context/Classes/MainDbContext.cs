using SQLite;
using Structure.Domain.Classes;
using Structure.Model.Interfaces;
using System;

namespace Repository.Context.Classes
{
    public class MainDbContext : IDisposable, IMainDbContext
    {
        private readonly SQLiteAsyncConnection _connection;
        public MainDbContext(IDatabaseOptions databaseOptions)
        {
            _connection = new SQLiteAsyncConnection(databaseOptions.ConnectionString);
            ManualMigration();
        }

        public SQLiteAsyncConnection GetCurrentContext()
        {
            return _connection;
        }

        private void ManualMigration()
        {
            //_connection.DropTableAsync<Product>();
            //_connection.DropTableAsync<Campaign>();
            _connection.CreateTableAsync<Product>();
            _connection.CreateTableAsync<Campaign>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection.CloseAsync();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
