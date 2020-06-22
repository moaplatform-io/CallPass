using Kons.ShopCallpass.FileDatabase;
using Kons.ShopCallpass.Object;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Kons.ShopCallpass.Model
{
    public class ModelFileDatabase
    {
        private const string DBFILE_DB_NAME = "CallpassData.db";

        // ---------------------------------------------------------- ModelDBFile
        //
        static private ModelFileDatabase m_this = null;
        private ModelFileDatabase() // private will be protect creator by new
        {
        }

        public static void init()
        {
            if (null != m_this)
            {
                deInit();
            }
            m_this = new ModelFileDatabase();
        }

        public static void deInit()
        {
            m_this = null;
        }

        public static MyFileDbConnection getConnection()
        {
            if (null == m_this)
            {
                ModelFileDatabase.init();
            }
            return (new ModelFileDatabase.MyFileDbConnection(DBFILE_DB_NAME));
        }

        // ---------------------------------------------------------- connection
        //
        public class MyFileDbConnection : MyFileDbConnectionCallpass
        {
            public MyFileDbConnection(String _db_name)
            {
                checkDB(_db_name);
            }
        }
    }
}
