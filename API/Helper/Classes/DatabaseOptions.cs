using Structure.Model.Interfaces;

namespace API.Helper.Classes
{
    public class DatabaseOptions : IDatabaseOptions
    {
        public string ConnectionString
        {
            get
            {
                return Startup.ConnectionString;
            }
        }
    }
}
