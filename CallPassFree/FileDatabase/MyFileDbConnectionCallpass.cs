using Kons.ShopCallpass.Object;
using Kons.TsLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Kons.ShopCallpass.FileDatabase
{
    public class MyFileDbConnectionCallpass : FileDbConnectionSqlite
    {
        // ----------------------------------------------------------
        //
        public bool checkDB(String _db_name)
        {
            if (null != _db_name && 0 < _db_name.Length)
            {
                return createDB(_db_name);
            }
            return false;
        }
        /*
        public void checkTableConfigOrderInput()
        {
            // check table
            String query = ObjConfigOrderInput.sqlite_getExistsTableString();
            SQLiteCommand cmd = getCommand(query);
            object ret = cmd.ExecuteScalar();
            if (null == ret)
            {
                try
                {
                    query = ObjConfigOrderInput.sqlite_getCreateTableString();
                    cmd = getCommand(query);
                    ret = cmd.ExecuteNonQuery();
                    if (null == ret)
                    {
                        TsLog.writeLog("Error: create table fail");
                    }
                }
                catch (Exception err)
                {
                    TsLog.writeLog(err.Message);
                }
            }
            cmd.Dispose();
        }

        // ----------------------------------------------------------
        //
        public DataTable loadConfigOrderInputObject(int _obj_key)
        {
            // check
            checkTableConfigOrderInput();

            // insert
            DataSet ds = new DataSet();
            try
            {
                String query = ObjConfigOrderInput.sqlite_getSelectRowString(_obj_key);
                SQLiteDataAdapter da = new SQLiteDataAdapter(query, getConnection());

                // fill
                da.Fill(ds);
            }
            catch (Exception err)
            {
                TsLog.writeLog(err.Message);
            }

            if (0 == ds.Tables.Count)
            {
                return null;
            }
            return ds.Tables[0];
        }

        public ObjProcedureResult saveConfigOrderInputObject(ObjConfigOrderInput _obj)
        {
            // check
            checkTableConfigOrderInput();

            // insert
            try
            {
                // update 
                String query = ObjConfigOrderInput.sqlite_getUpdateRowString(_obj);
                SQLiteCommand cmd = getCommand(query);
                int cnt = cmd.ExecuteNonQuery();

                // insert if update fail
                if (0 == cnt)
                {
                    query = ObjConfigOrderInput.sqlite_getInsertRowString(_obj);
                    cmd = getCommand(query);
                    cnt = cmd.ExecuteNonQuery();
                }
            }
            catch(Exception err)
            {
                TsLog.writeLog(err.Message);
            }

            return null;
        }

        // ----------------------------------------------------------
        //
        public ObjProcedureResult saveOrderObject(ObjOrder _order)
        {
            return null;
        }

        public DataTable loadOrderList(int _order_biz_date)
        {
            return null;
        }
        */
    }
}
