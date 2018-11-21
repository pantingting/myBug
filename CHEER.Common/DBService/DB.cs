namespace CHEER.Common.DBService
{
    public class DB
    {
        private string connection = null;
        private string dbType = null;

        public static DB Instance
        {
            get
            {
                return new DB(null, null);
            }
        }

        public static DB NewInstance(string connection = null, string dbType = null)
        {
            return new DB(connection, dbType);
        }

        public DB(string connection = null, string dbType = null)
        {
            this.connection = connection;
            this.dbType = dbType;
        }

        public Select Select
        {
            get
            {
                return new Select(connection, dbType);
            }
        }

        public Delete Delete
        {
            get
            {
                return new Delete(connection, dbType);
            }
        }

        public Insert Insert
        {
            get
            {
                return new Insert(connection, dbType);
            }
        }

        public Update Update
        {
            get
            {
                return new Update(connection, dbType);
            }
        }
    }
}
