using Models.EF;

namespace Models.DAO
{
    public class CommonDAO
    {
        private WatchShopEntities database;

        private static CommonDAO instance;
        public static CommonDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommonDAO();
                }
                return instance;
            }
        }

        private CommonDAO()
        {
            database = new WatchShopEntities();
        }

        public WatchShopEntities Database
        {
            get
            {
                return database;
            }
        }
    }
}
