using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Kons.ShopCallpass.FileDatabase
{
    public class FileDbConnectionBase : IDisposable
    {
        private String m_db_last_error = "";

        // ----------------------------------
        //
        ~FileDbConnectionBase()
        {
            Dispose(false);
            closeDB();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool _disposiong)
        {
            if (_disposiong)
            {
                closeDB();
            }
        }

        // --------------------------------------
        //
        virtual public bool openDB()
        {
            return false;
        }

        virtual public void closeDB()
        {
        }

        // --------------------------------------
        //
        virtual public bool createDB(String _db_name, bool _is_force_create = false)
        {
            return false;
        }

        /// <summary>
        /// 실행결과를 영향받은 행의 개수로 반환
        /// </summary>
        /// <param name="_query_text"></param>
        /// <returns></returns>
        virtual public int execNoneText(String _query_text)
        {
            return 0;
        }

        /// <summary>
        /// 실행결과를 DataSet으로 반환
        /// </summary>
        /// <param name="_query_text"></param>
        /// <returns></returns>
        virtual public DataSet execDatasetText(String _query_text)
        {
            return null;
        }

        // --------------------------------------
        //
        protected void setLastErrorMessage(String _error_message)
        {
            m_db_last_error = _error_message;
        }

        protected String getLastErrorMessage()
        {
            return m_db_last_error;
        }
    }
}
