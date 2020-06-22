using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Kons.ShopCallpass.Parser;
using Kons.TsLibrary;
using Kons.Utility;

namespace Kons.ShopCallpass.Object
{
    public class ObjOrder
    {
        // ---------------------------------------------------------- define
        //
        public enum STATE_TYPE
        {
            ORDER_STATE_0 = 0,  // 대기
            ORDER_STATE_1,      // 배달요청
            ORDER_STATE_END
        }

        public enum CUST_PAY_TYPE
        {
            CASH = 0,               // 현금
            CARD,
            DEFERRED_PAYMENT,       // 후불
            DELIVERY_PAYMENT,       // 착불
            PRE_PAYMENT,            // 선불
            ACCOUNT_TRANS = 11,     // 계좌이체 - 2.0 %
            VACCOUNT = 12,          // 가상계좌 - 건당 300원
            NAVER_PAY = 21,         // 네이버페이 - 3.74 %
            CACAO_PAY = 22,         // 카카오페이 - 3.74%
            BAOKIM_PAY = 31,        // 바오킴페이 - 3.74%
            ERROR = 77,
            CUST_PAY_TYPE_END
        }

        public enum CUST_PAY_STEP
        {
            BANK_WAIT = 0,          // 입금확인중
            HOMEPAGE_BANK_WAIT,
            CARD_WAIT,
            PAY_OK = 9,             // 결제완료
            HOMEPAGE_BANK_OK,
            CARD_OK,
            HOMEPAGE_CARD_OK,
            PAY_CANCEL,
            PAY_DEFERRED = 21,      // 후불
            PAY_DELIVERY = 22,      // 착불
            ERROR = 77,
            ORDER_STATE_END
        }

        // ---------------------------------------------------------- basic object values
        // Order
        public String m_order_num;
        public Int32 m_order_biz_date;
        public Int32 m_order_type;
        public Int32 m_state_cd;

        public String m_call_num;

        public DateTime m_call_datetime;
        public DateTime m_date_1;
        public DateTime m_date_2;
        public DateTime m_date_3;
        public DateTime m_date_4;
        public DateTime m_date_5;
        public DateTime m_date_6;
        public DateTime m_date_7;

        // Shop
        public String m_shop_name;
        public Int32 m_shop_request_time;
        public Int32 m_shop_request_option;
        public Int32 m_shop_cost;

        // Customer
        public String m_customer_name;
        public Int32 m_customer_cost;
        public Int32 m_customer_additional_cost;
        public String m_customer_additional_cost_memo;

        public Int32 m_customer_pay_type_cd;
        public String m_customer_pay_type_memo;

        public Int32 m_customer_request_option;
        public Int32 m_customer_request_time;
        public String m_customer_request_memo;

        // Locate Arv (Shop Customer의 위치 및 주소정보)
        public String m_arv_locate_name;
        public String m_arv_locate_address;
        public String m_arv_locate_alternative_address; // 대체 가능한 주소 - 신주소
        public String m_arv_locate_memo;
        public String m_arv_person_tel_num;
        public String m_arv_person_memo;

        // 출력데이터
        public String m_print_port_num = "";
        public byte[] m_print_raw_data_buf = null;

        // ----------------------------------------------------------
        //
        public ObjOrder()
        {
            initObj();
        }

        public void initObj()
        {
            // Order
            m_order_num = "";
            m_order_biz_date = 0;
            m_order_type = 0;
            m_state_cd = 0;

            m_call_num = "";

            m_call_datetime = DateTime.MinValue;
            m_date_1 = DateTime.MinValue;
            m_date_2 = DateTime.MinValue;
            m_date_3 = DateTime.MinValue;
            m_date_4 = DateTime.MinValue;
            m_date_5 = DateTime.MinValue;
            m_date_6 = DateTime.MinValue;
            m_date_7 = DateTime.MinValue;

            // Shop
            m_shop_name = "";
            m_shop_request_time = 0;
            m_shop_request_option = 0;
            m_shop_cost = 0;

            // Customer
            m_customer_name = "";
            m_customer_cost = 0;
            m_customer_additional_cost = 0;
            m_customer_additional_cost_memo = "";

            m_customer_pay_type_cd = 0;
            m_customer_pay_type_memo = "";

            m_customer_request_option = 0;
            m_customer_request_time = 0;
            m_customer_request_memo = "";

            // Locate Arv
            m_arv_locate_name = "";
            m_arv_locate_address = "";
            m_arv_locate_alternative_address = "";
            m_arv_locate_memo = "";
            m_arv_person_tel_num = "";
            m_arv_person_memo = "";

            // pirnt info
            m_print_port_num = "";
            m_print_raw_data_buf = null;
        }

        public void setRawDataForPrint(String _print_port_num, byte[] _recv_data_buf, int _recv_data_offset, int _recv_data_len)
        {
            m_print_port_num = _print_port_num;
            m_print_raw_data_buf = new byte[_recv_data_len];
            System.Array.Copy(_recv_data_buf, _recv_data_offset, m_print_raw_data_buf, 0, m_print_raw_data_buf.Length);
        }

        static public void makeTableSchema(ref DataTable _des_table)
        {
            if (null == _des_table)
            {
                return;
            }

            // Order
            KnUtil.insureTableColumn(ref _des_table, "order_num", typeof(string));
            KnUtil.insureTableColumn(ref _des_table, "order_biz_date", typeof(Int32));
            KnUtil.insureTableColumn(ref _des_table, "order_type", typeof(Int32));
            KnUtil.insureTableColumn(ref _des_table, "state_cd", typeof(Int32));

            KnUtil.insureTableColumn(ref _des_table, "call_num", typeof(string));

            KnUtil.insureTableColumn(ref _des_table, "call_datetime", typeof(DateTime));
            KnUtil.insureTableColumn(ref _des_table, "date_1", typeof(DateTime));
            KnUtil.insureTableColumn(ref _des_table, "date_2", typeof(DateTime));
            KnUtil.insureTableColumn(ref _des_table, "date_3", typeof(DateTime));
            KnUtil.insureTableColumn(ref _des_table, "date_4", typeof(DateTime));
            KnUtil.insureTableColumn(ref _des_table, "date_5", typeof(DateTime));
            KnUtil.insureTableColumn(ref _des_table, "date_6", typeof(DateTime));
            KnUtil.insureTableColumn(ref _des_table, "date_7", typeof(DateTime));

            // Shop
            KnUtil.insureTableColumn(ref _des_table, "shop_name", typeof(String));
            KnUtil.insureTableColumn(ref _des_table, "shop_request_time", typeof(Int32));
            KnUtil.insureTableColumn(ref _des_table, "shop_request_option", typeof(Int32));
            KnUtil.insureTableColumn(ref _des_table, "shop_cost", typeof(Int32));

            // Customer
            KnUtil.insureTableColumn(ref _des_table, "customer_name", typeof(String));
            KnUtil.insureTableColumn(ref _des_table, "customer_cost", typeof(Int32));
            KnUtil.insureTableColumn(ref _des_table, "customer_additional_cost", typeof(Int32));
            KnUtil.insureTableColumn(ref _des_table, "customer_additional_cost_memo", typeof(String));

            KnUtil.insureTableColumn(ref _des_table, "customer_pay_type_cd", typeof(Int32));
            KnUtil.insureTableColumn(ref _des_table, "customer_pay_type_memo", typeof(String));

            KnUtil.insureTableColumn(ref _des_table, "customer_request_option", typeof(Int32));
            KnUtil.insureTableColumn(ref _des_table, "customer_request_time", typeof(Int32));
            KnUtil.insureTableColumn(ref _des_table, "customer_request_memo", typeof(String));

            // Locate Arv
            KnUtil.insureTableColumn(ref _des_table, "arv_locate_name", typeof(string));
            KnUtil.insureTableColumn(ref _des_table, "arv_locate_address", typeof(string));
            KnUtil.insureTableColumn(ref _des_table, "arv_locate_alternative_address", typeof(string));
            KnUtil.insureTableColumn(ref _des_table, "arv_locate_memo", typeof(string));
            KnUtil.insureTableColumn(ref _des_table, "arv_person_tel_num", typeof(string));
            KnUtil.insureTableColumn(ref _des_table, "arv_person_memo", typeof(string));
        }

        static public void setTableData(ref DataRow _des_row, ref ObjOrder _obj_order)
        {
            Utility.Utility.LogWrite("setTableData");
            // order 는 makeTableSchema 이후 ImportRow 통해서 넣는것이 아니라 한번에 넣기 때문에 
            // 프로시저에서 불러온 DataTable Schema 따라간다. 
            // 그래서 setDataRowData 함수 이용해서 없는것은 체크해서 넣어야 한다.
            try
            {
                _des_row.BeginEdit(); // start edit
                {
                    // Order
                    _des_row["order_num"] = _obj_order.m_order_num;
                    _des_row["order_biz_date"] = _obj_order.m_order_biz_date;
                    _des_row["order_type"] = _obj_order.m_order_type;
                    _des_row["state_cd"] = _obj_order.m_state_cd;

                    _des_row["call_num"] = _obj_order.m_call_num;

                    _des_row["call_datetime"] = _obj_order.m_call_datetime;
                    _des_row["date_1"] = _obj_order.m_date_1;
                    _des_row["date_2"] = _obj_order.m_date_2;
                    _des_row["date_3"] = _obj_order.m_date_3;
                    _des_row["date_4"] = _obj_order.m_date_4;
                    _des_row["date_5"] = _obj_order.m_date_5;
                    _des_row["date_6"] = _obj_order.m_date_6;
                    _des_row["date_7"] = _obj_order.m_date_7;

                    // Shop
                    _des_row["shop_name"] = _obj_order.m_shop_name;
                    _des_row["shop_request_time"] = _obj_order.m_shop_request_time;
                    _des_row["shop_request_option"] = _obj_order.m_shop_request_option;
                    _des_row["shop_cost"] = _obj_order.m_shop_cost;

                    // Customer
                    _des_row["customer_name"] = _obj_order.m_customer_name;
                    _des_row["customer_cost"] = _obj_order.m_customer_cost;
                    _des_row["customer_additional_cost"] = _obj_order.m_customer_additional_cost;
                    _des_row["customer_additional_cost_memo"] = _obj_order.m_customer_additional_cost_memo;

                    _des_row["customer_pay_type_cd"] = _obj_order.m_customer_pay_type_cd;
                    _des_row["customer_pay_type_memo"] = _obj_order.m_customer_pay_type_memo;

                    _des_row["customer_request_option"] = _obj_order.m_customer_request_option;
                    _des_row["customer_request_time"] = _obj_order.m_customer_request_time;
                    _des_row["customer_request_memo"] = _obj_order.m_customer_request_memo;

                    // Locate Arv
                    _des_row["arv_locate_name"] = _obj_order.m_arv_locate_name;
                    _des_row["arv_locate_address"] = _obj_order.m_arv_locate_address;
                    _des_row["arv_locate_alternative_address"] = _obj_order.m_arv_locate_alternative_address;
                    _des_row["arv_locate_memo"] = _obj_order.m_arv_locate_memo;
                    _des_row["arv_person_tel_num"] = _obj_order.m_arv_person_tel_num;
                    _des_row["arv_person_memo"] = _obj_order.m_arv_person_memo;
                }
            }
            catch (Exception ex)
            {
                TsLog.writeLog(ex.Message);
            }
            finally
            {
                _des_row.EndEdit(); // end edit
            }
        }

        public void setObj(ObjOrder _obj_order)
        {
            Utility.Utility.LogWrite("setObj");
            // Order
            m_order_num = _obj_order.m_order_num;
            m_order_biz_date = _obj_order.m_order_biz_date;
            m_order_type = _obj_order.m_order_type;
            m_state_cd = _obj_order.m_state_cd;

            m_call_num = _obj_order.m_call_num;
            m_call_datetime = _obj_order.m_call_datetime;
            m_date_1 = _obj_order.m_date_1;
            m_date_2 = _obj_order.m_date_2;
            m_date_3 = _obj_order.m_date_3;
            m_date_4 = _obj_order.m_date_4;
            m_date_5 = _obj_order.m_date_5;
            m_date_6 = _obj_order.m_date_6;
            m_date_7 = _obj_order.m_date_7;

            // Shop
            m_shop_name = _obj_order.m_shop_name;
            m_shop_request_time = _obj_order.m_shop_request_time;
            m_shop_request_option = _obj_order.m_shop_request_option;
            m_shop_cost = _obj_order.m_shop_cost;

            // Customer
            m_customer_name = _obj_order.m_customer_name;
            m_customer_cost = _obj_order.m_customer_cost;
            m_customer_additional_cost = _obj_order.m_customer_additional_cost;
            m_customer_additional_cost_memo = _obj_order.m_customer_additional_cost_memo;

            m_customer_pay_type_cd = _obj_order.m_customer_pay_type_cd;
            m_customer_pay_type_memo = _obj_order.m_customer_pay_type_memo;

            m_customer_request_option = _obj_order.m_customer_request_option;
            m_customer_request_time = _obj_order.m_customer_request_time;
            m_customer_request_memo = _obj_order.m_customer_request_memo;

            // Locate Arv
            m_arv_locate_name = _obj_order.m_arv_locate_name;
            m_arv_locate_address = _obj_order.m_arv_locate_address;
            m_arv_locate_alternative_address = _obj_order.m_arv_locate_alternative_address;
            m_arv_locate_memo = _obj_order.m_arv_locate_memo;
            m_arv_person_tel_num = _obj_order.m_arv_person_tel_num;
            m_arv_person_memo = _obj_order.m_arv_person_memo;
        }

        public bool setObj(DataRow _dr)
        {
            if (null == _dr)
            {
                return false;
            }

            try
            {
                // Order
                m_order_num = _dr["order_num"].ToString();
                m_order_biz_date = KnUtil.parseInt32(_dr["order_biz_date"].ToString());
                m_order_type = KnUtil.parseInt32(_dr["order_type"].ToString());
                m_state_cd = KnUtil.parseInt32(_dr["state_cd"].ToString());

                m_call_num = _dr["call_num"].ToString();
                m_call_datetime = KnUtil.parseDateTime(_dr["call_datetime"].ToString());
                m_date_1 = KnUtil.parseDateTime(KnUtil.getDataRowString(_dr, "date_1"));
                m_date_2 = KnUtil.parseDateTime(KnUtil.getDataRowString(_dr, "date_2"));
                m_date_3 = KnUtil.parseDateTime(KnUtil.getDataRowString(_dr, "date_3"));
                m_date_4 = KnUtil.parseDateTime(KnUtil.getDataRowString(_dr, "date_4"));
                m_date_5 = KnUtil.parseDateTime(KnUtil.getDataRowString(_dr, "date_5"));
                m_date_6 = KnUtil.parseDateTime(KnUtil.getDataRowString(_dr, "date_6"));
                m_date_7 = KnUtil.parseDateTime(KnUtil.getDataRowString(_dr, "date_7"));

                // Shop
                m_shop_name = KnUtil.getDataRowString(_dr, "shop_name");
                m_shop_request_time = KnUtil.parseInt32(KnUtil.getDataRowString(_dr, "shop_request_time"));
                m_shop_request_option = KnUtil.parseInt32(KnUtil.getDataRowString(_dr, "shop_request_option"));
                m_shop_cost = KnUtil.parseInt32(KnUtil.getDataRowString(_dr, "shop_cost"));

                // Customer
                m_customer_name = KnUtil.getDataRowString(_dr, "customer_name");
                m_customer_cost = KnUtil.parseInt32(KnUtil.getDataRowString(_dr, "customer_cost"));
                m_customer_additional_cost = KnUtil.parseInt32(KnUtil.getDataRowString(_dr, "customer_additional_cost"));
                m_customer_additional_cost_memo = KnUtil.getDataRowString(_dr, "customer_additional_cost_memo");

                m_customer_pay_type_cd = KnUtil.parseInt32(KnUtil.getDataRowString(_dr, "customer_pay_type_cd"));
                m_customer_pay_type_memo = KnUtil.getDataRowString(_dr, "customer_pay_type_memo");

                m_customer_request_option = KnUtil.parseInt32(KnUtil.getDataRowString(_dr, "customer_request_option"));
                m_customer_request_time = KnUtil.parseInt32(KnUtil.getDataRowString(_dr, "customer_request_time"));
                m_customer_request_memo = KnUtil.getDataRowString(_dr, "customer_request_memo");

                // Locate Arv
                m_arv_locate_name = KnUtil.getDataRowString(_dr, "arv_locate_name");
                m_arv_locate_address = KnUtil.getDataRowString(_dr, "arv_locate_address");
                m_arv_locate_alternative_address = KnUtil.getDataRowString(_dr, "arv_locate_alternative_address");
                m_arv_locate_memo = KnUtil.getDataRowString(_dr, "arv_locate_memo");
                m_arv_person_tel_num = KnUtil.getDataRowString(_dr, "arv_person_tel_num");
                m_arv_person_memo = KnUtil.getDataRowString(_dr, "arv_person_memo");
            }
            catch (Exception ex)
            {
                TsLog.writeLog(ex.Message);
                return false;
            }
            return true;
        }

        public ObjProcedureResult saveToDB(int _reg_count_idx = 0, int _reg_count_tot = 0)
        {
            ObjProcedureResult proc_result = new ObjProcedureResult();
            try
            {
                //using (ModelDBServer.MyDbConnection my_db = ModelDBServer.getConnection())
                //{
                //    if (my_db.openDB())
                //    {
                //        my_db.addParameter("@_login_key", AppMain.AppCore.Instance.getLoginUserLoginKey());
                //        // order
                //        my_db.addParameter("@_state_cd", m_state_cd);
                //        my_db.addParameter("@_call_num", m_call_num);
                //        my_db.addParameter("@_caller_line", m_caller_line);
                //        my_db.addParameter("@_caller_id", m_caller_id);
                //        my_db.addParameter("@_memo", m_memo);
                //        // company
                //        my_db.addParameter("@_company_id", m_company_id);
                //        // shop
                //        my_db.addParameter("@_shop_id", m_shop_id);
                //        my_db.addParameter("@_shop_pay_type", m_shop_pay_type);
                //        my_db.addParameter("@_shop_pay_step", m_shop_pay_step);
                //        my_db.addParameter("@_shop_request_time", m_shop_request_time);
                //        my_db.addParameter("@_shop_request_option", m_shop_request_option);
                //        my_db.addParameter("@_shop_cost", m_shop_cost);
                //        // customer
                //        my_db.addParameter("@_customer_id", m_customer_id);
                //        my_db.addParameter("@_customer_cost", m_customer_cost);
                //        my_db.addParameter("@_customer_pay_type", m_customer_pay_type);
                //        my_db.addParameter("@_customer_pay_step", m_customer_pay_step);
                //        // driver
                //        my_db.addParameter("@_driver_id", m_driver_id);
                //        my_db.addParameter("@_driver_cost", m_driver_cost);
                //        // locate dpt
                //        my_db.addParameter("@_dpt_lev0_locate_code", m_dpt_lev0_locate_code);
                //        my_db.addParameter("@_dpt_lev1_locate_code", m_dpt_lev1_locate_code);
                //        my_db.addParameter("@_dpt_lev2_locate_code", m_dpt_lev2_locate_code);
                //        my_db.addParameter("@_dpt_locate_crypt_x", m_dpt_locate_crypt_x);
                //        my_db.addParameter("@_dpt_locate_crypt_y", m_dpt_locate_crypt_y);
                //        my_db.addParameter("@_dpt_locate_name", m_dpt_locate_name);
                //        my_db.addParameter("@_dpt_locate_address", m_dpt_locate_address);
                //        my_db.addParameter("@_dpt_locate_memo", m_dpt_locate_memo);
                //        my_db.addParameter("@_send_person_tel_num", m_send_person_tel_num);
                //        my_db.addParameter("@_send_person_memo", m_send_person_memo);
                //        // locate arv
                //        my_db.addParameter("@_arv_lev0_locate_code", m_arv_lev0_locate_code);
                //        my_db.addParameter("@_arv_lev1_locate_code", m_arv_lev1_locate_code);
                //        my_db.addParameter("@_arv_lev2_locate_code", m_arv_lev2_locate_code);
                //        my_db.addParameter("@_arv_locate_crypt_x", m_arv_locate_crypt_x);
                //        my_db.addParameter("@_arv_locate_crypt_y", m_arv_locate_crypt_y);
                //        my_db.addParameter("@_arv_locate_name", m_arv_locate_name);
                //        my_db.addParameter("@_arv_locate_address", m_arv_locate_address);
                //        my_db.addParameter("@_arv_locate_alternative_address", m_arv_locate_alternative_address);
                //        my_db.addParameter("@_arv_locate_memo", m_arv_locate_memo);
                //        my_db.addParameter("@_arv_person_tel_num", m_arv_person_tel_num);
                //        my_db.addParameter("@_arv_person_memo", m_arv_person_memo);
                //        // 복수콜
                //        my_db.addParameter("@_reg_count_idx", _reg_count_idx);
                //        my_db.addParameter("@_reg_count_tot", _reg_count_tot);

                //        DataTable result_dt = my_db.execDatatableProcedure("ccp_Order_ObjSave");
                //        if (!proc_result.setResult(result_dt))
                //        {
                //            String err_msg = (null == my_db.getLastError() ? "fail. set result data: " : my_db.getLastError());
                //            Debug.Assert(false, err_msg);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                TsLog.writeLog(ex.Message);
            }

            return proc_result;
        }

        public bool loadFromDB(String _order_num)
        {
            // 초기화
            initObj();

            // 읽어오기
            try
            {
                //using (ModelDBServer.MyDbConnection my_db = ModelDBServer.getConnection())
                //{
                //    if (my_db.openDB())
                //    {
                //        my_db.addParameter("@_login_key", AppMain.AppCore.Instance.getLoginUserLoginKey());
                //        my_db.addParameter("@_order_num", _order_num);
                //        {
                //            DataTable result_dt = my_db.execDatatableProcedure("ccp_Order_ObjLoad");
                //            if ((null != result_dt) && (0 < result_dt.Rows.Count))
                //            {
                //                setObj(result_dt.Rows[0]);
                //                Debug.Assert(_order_num == this.m_order_num);
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                TsLog.writeLog(ex.Message);
            }

            // 결과
            return (0 < this.m_order_num.Length);
        }

        // ---------------------------------------------------------- order_flag
        //

        // state
        static public Color getStateColor(int _state_cd)
        {
            switch ((ObjOrder.STATE_TYPE)_state_cd)
            {
                case ObjOrder.STATE_TYPE.ORDER_STATE_0:
                    return Color.FromArgb(255, 255, 238);
                case ObjOrder.STATE_TYPE.ORDER_STATE_1:
                    return Color.FromArgb(255, 255, 255);
                //case ObjOrder.STATE_TYPE.ORDER_STATE_0:
                //    return Color.FromArgb(255, 255, 238);
                //case ObjOrder.STATE_TYPE.ORDER_STATE_1:
                //    return Color.FromArgb(255, 242, 230);
                //case ObjOrder.STATE_TYPE.ORDER_STATE_2:
                //    return Color.FromArgb(255, 235, 255);
                //case ObjOrder.STATE_TYPE.ORDER_STATE_3:
                //    return Color.FromArgb(255, 242, 230);
                //case ObjOrder.STATE_TYPE.ORDER_STATE_4:
                //    return Color.FromArgb(235, 247, 255);
                //case ObjOrder.STATE_TYPE.ORDER_STATE_5:
                //    return Color.FromArgb(235, 247, 255);
                //case ObjOrder.STATE_TYPE.ORDER_STATE_6:
                //    return Color.FromArgb(255, 255, 255);
                //case ObjOrder.STATE_TYPE.ORDER_STATE_7:
                //    return Color.FromArgb(253, 253, 253);
                //case ObjOrder.STATE_TYPE.ORDER_STATE_8:
                //    return Color.FromArgb(253, 253, 253);
                //case ObjOrder.STATE_TYPE.ORDER_STATE_9:
                //    return Color.FromArgb(224, 255, 219);
            }

            return Color.Empty;
        }

        static public String getStateString(int _state_cd)
        {
            switch ((ObjOrder.STATE_TYPE)_state_cd)
            {
                case ObjOrder.STATE_TYPE.ORDER_STATE_0:
                    return "주문등록";
                case ObjOrder.STATE_TYPE.ORDER_STATE_1:   // shipment 로 짜여진 상태
                    return "배달요청";
            }
            return "";
        }

        static public Bitmap getStateImage(int _state_cd)
        {
            switch ((ObjOrder.STATE_TYPE)_state_cd)
            {
                case ObjOrder.STATE_TYPE.ORDER_STATE_0:
                    return Properties.Resources.order_state_2;
                case ObjOrder.STATE_TYPE.ORDER_STATE_1:
                    return Properties.Resources.order_state_4;
            }

            return null;
        }

        // pay type
        static public String getCustPayTypeString(int _pay_type)
        {
            switch (_pay_type)
            {
                case (int)CUST_PAY_TYPE.CASH:
                    return "현금";
                case (int)CUST_PAY_TYPE.CARD:
                    return "카드";
                case (int)CUST_PAY_TYPE.DEFERRED_PAYMENT:
                    return "후불";
                case (int)CUST_PAY_TYPE.DELIVERY_PAYMENT:
                    return "착불";
                case (int)CUST_PAY_TYPE.ACCOUNT_TRANS:
                    return "이체";
                case (int)CUST_PAY_TYPE.PRE_PAYMENT:
                    return "선불";
                case (int)CUST_PAY_TYPE.VACCOUNT:
                    return "가상";
                case (int)CUST_PAY_TYPE.NAVER_PAY:
                    return "네이버";
                case (int)CUST_PAY_TYPE.CACAO_PAY:
                    return "카카오";
                case (int)CUST_PAY_TYPE.BAOKIM_PAY:
                    return "BAOKIM"; //"바오킴";
                case (int)CUST_PAY_TYPE.ERROR:
                    return "표시없음";
            }
            return "-";
        }

        // pay step
        static public String getCustPayStepString(int _pay_step)
        {
            switch ((ObjOrder.CUST_PAY_STEP)_pay_step)
            {
                case ObjOrder.CUST_PAY_STEP.BANK_WAIT:
                    return "입금확인중";
                case ObjOrder.CUST_PAY_STEP.HOMEPAGE_BANK_WAIT:
                    return "홈입확인중";
                case ObjOrder.CUST_PAY_STEP.CARD_WAIT:
                    return "카결대기중";
                case ObjOrder.CUST_PAY_STEP.PAY_OK:
                    return "결제완료";
                case ObjOrder.CUST_PAY_STEP.HOMEPAGE_BANK_OK:
                    return "홈입금완료";
                case ObjOrder.CUST_PAY_STEP.CARD_OK:
                    return "카결제완료";
                case ObjOrder.CUST_PAY_STEP.HOMEPAGE_CARD_OK:
                    return "홈카드완료";
                case ObjOrder.CUST_PAY_STEP.PAY_DEFERRED:
                    return "후불";
                case ObjOrder.CUST_PAY_STEP.PAY_DELIVERY:
                    return "착불";
                case ObjOrder.CUST_PAY_STEP.PAY_CANCEL:
                    return "결제취소";
                case ObjOrder.CUST_PAY_STEP.ERROR:
                    return "표시오류";
            }
            return "";
        }

        // edit lock
        static public Bitmap getEditLockImage()
        {
            return Properties.Resources.bopermission_16x16;
        }

        static public Bitmap getCounselingMemoImage()
        {
            return Properties.Resources.checkbuttons_16x16;
        }

        // order type
        static public String getOrderTypeString(int _order_type)
        {
            return ParserOrderInputBase.obtainParserName(ParserOrderInputBase.obtainParserType(_order_type));
        }

        // ---------------------------------------------------------- order_flag
        //
        static public Bitmap getOrderFlagImage(int _order_flag)
        {
            //if (0 < (_order_flag & ORDER_INFO_FLAG_HOMEPAGE_ORDER))
            //{
            //    return Properties.Resources.order_state_9;
            //}
            //else if (0 < (_order_flag & ORDER_INFO_FLAG_MOBILE_APP_ORDER))
            //{
            //    return Properties.Resources.order_state_9;
            //}
            return null;
        }
    }
}