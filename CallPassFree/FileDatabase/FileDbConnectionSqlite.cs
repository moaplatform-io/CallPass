using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Kons.ShopCallpass.FileDatabase
{
    public class FileDbConnectionSqlite : FileDbConnectionBase
    {
        private String m_db_name = null;
        private String m_db_connection_string = null;
        //private SQLiteConnection m_db_connection = null; // 연결 커넥션

        // --------------------------------------
        //
        override public bool createDB(String _db_name, bool _is_force_create = false)
        {
            //m_db_name = _db_name;
            //m_db_connection_string = String.Format(@"Data Source = {0}", _db_name);
            //try
            //{
            //    if (_is_force_create || !File.Exists(_db_name))
            //    {
            //        SQLiteConnection.CreateFile(_db_name);
            //    }
            //    return true;
            //}
            //catch (Exception err)
            //{
            //    setLastErrorMessage(err.Message);
            //}
            return false;
        }

        override public bool openDB()
        {
            //try
            //{
            //    m_db_connection = new SQLiteConnection(m_db_connection_string);
            //    m_db_connection.Open();
            //    return true;
            //}
            //catch(Exception err)
            //{
            //    setLastErrorMessage(err.Message);
            //    m_db_connection = null;
            //}
            return false;
        }

        override public void closeDB()
        {
            //try
            //{
            //    m_db_connection.Close();
            //}
            //catch (Exception err)
            //{
            //    setLastErrorMessage(err.Message);
            //}
            //finally
            //{
            //    m_db_connection = null;
            //}
        }

        /*
        /// <summary>
        /// 연결 커넥션 반환
        /// </summary>
        /// <returns></returns>
        protected SQLiteConnection getConnection()
        {
            return m_db_connection;
        }

        protected SQLiteCommand getCommand(String _command_text)
        {
            return new SQLiteCommand(_command_text, getConnection());
        }

        /// <summary>
        /// 실행결과를 영향받은 행의 개수로 반환
        /// </summary>
        /// <param name="_query_text"></param>
        /// <returns></returns>
        override public int execNoneText(String _query_text)
        {
            if (null == m_db_connection)
            {
                setLastErrorMessage("Need a DataBase-Connection.");
                return 0;
            }

            int result = 0;
            try
            {
                SQLiteCommand command = new SQLiteCommand(_query_text, m_db_connection);
                result = command.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                setLastErrorMessage(err.Message);
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// 실행결과를 DataSet으로 반환
        /// </summary>
        /// <param name="_query_text"></param>
        /// <returns></returns>
        override public DataSet execDatasetText(String _query_text)
        {
            if (null == m_db_connection)
            {
                setLastErrorMessage("Need a DataBase-Connection.");
                return null;
            }

            DataSet ds = new DataSet();
            try
            {
                var adpt = new SQLiteDataAdapter(_query_text, m_db_connection);
                adpt.Fill(ds);
            }
            catch(Exception err)
            {
                setLastErrorMessage(err.Message);
                ds = null;
            }

            return ds;
        }
        */
    }
}
