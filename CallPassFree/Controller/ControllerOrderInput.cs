using Kons.ShopCallpass.Parser;
using Kons.ShopCallpass.Utility;
using Kons.ShopCallpass.Object;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using Kons.ShopCallpass.Model;
using Kons.ShopCallpass.AppMain;
using Kons.Utility;
using System.Diagnostics;

namespace Kons.ShopCallpass.Controller
{
    public class ControllerOrderInput : KnNotify
    {
        private static SerialPortOrderInput[] m_order_input_listen_serial_port_list;
        private Dictionary<String, SerialPortOrderInput> m_dic_order_input_listen_serial_port = new Dictionary<string, SerialPortOrderInput>();

        public int CONNECTED_INPUT_PORT_COUNT { get { return m_connected_input_port_count; } }
        private int m_connected_input_port_count = 0;

        // config
        private PoolConfigOrderInput m_pool_input_config = null;

        // pool
        private const int POOL_CAPACITY = 10;
        private KnPooledQueue<ObjOrderInputSerialRawDataBuf> m_pool_orderinput_rawdata;
        private ObjOrderInputSerialRawDataBuf m_read_rawdata = null;

        // ----------------------------------------------------------
        //
        public ControllerOrderInput()
        {
            m_pool_orderinput_rawdata = new KnPooledQueue<ObjOrderInputSerialRawDataBuf>(POOL_CAPACITY);
            m_read_rawdata = new ObjOrderInputSerialRawDataBuf();
        }

        public bool startListen()
        {
            Utility.Utility.LogWrite("startListen");
            // stop if already start
            stopListen();

            // start
            connectSerialPorts();
            
            // return
            return true;
        }

        public bool stopListen()
        {
            Utility.Utility.LogWrite("stopListen호출");
            // disconnect
            disconnectSerialPorts();

            Utility.Utility.LogWrite("stopListen");
            // clear pool
            while (m_pool_orderinput_rawdata.pop(ref m_read_rawdata))
            {
                // just pop
            }
            m_read_rawdata.initObj();

            // return
            return true;
        }

        public int getPoolHaveCount()
        {
            return m_pool_orderinput_rawdata.Count;
        }

        public int getPoolTotalCount()
        {
            return m_pool_orderinput_rawdata.TotalCreated;
        }

        // ----------------------------------------------------------
        //
        public void connectSerialPorts()
        {
            // ----------------------------------
            // disconnect
            disconnectSerialPorts();

            Utility.Utility.LogWrite("connectSerialPorts");
            // ----------------------------------
            // input config
            if (null == m_pool_input_config)
            {
                m_pool_input_config = new PoolConfigOrderInput(Kons.ShopCallpass.AppMain.AppCore.ORDER_INPUT_CONFIG_COUNT);
            }
            m_pool_input_config.loadObjectAll();

            // connect to order-input port
            m_order_input_listen_serial_port_list = new SerialPortOrderInput[m_pool_input_config.COUNT];
            for (int i = 0; i < m_order_input_listen_serial_port_list.Length; i++)
            {
                try
                {
                    ObjConfigOrderInput input_config = m_pool_input_config.getObject(i);
                    if (null != input_config && input_config.IS_USE)
                    {
                        int input_type = Kons.Utility.KnUtil.parseInt32(input_config.m_input_type);

                        Utility.Utility.LogWrite("input_type : " + input_type);

                        ParserOrderInputBase parser = null;
                        switch (ParserOrderInputBase.obtainParserType(input_type))
                        {
                            case ParserOrderInputBase.PARSER_TYPE.BAEMIN:
                                parser = new ParserOrderInputCommon();
                                break;
                            case ParserOrderInputBase.PARSER_TYPE.OKPOS:
                                parser = new ParserOrderInputCommon();
                                break;
                            case ParserOrderInputBase.PARSER_TYPE.DELGEN:
                                parser = new ParserOrderInputCommon();
                                break;
                            case ParserOrderInputBase.PARSER_TYPE.POSFEED:
                                parser = new ParserOrderInputCommon();
                                break;
                            case ParserOrderInputBase.PARSER_TYPE.EASYPOS:
                                parser = new ParserOrderInputCommon();
                                break;
                            case ParserOrderInputBase.PARSER_TYPE.YOGIYO:
                                parser = new ParserOrderInputCommon();
                                break;
                            case ParserOrderInputBase.PARSER_TYPE.BEDALTONG:
                                parser = new ParserOrderInputCommon();
                                break;
                            case ParserOrderInputBase.PARSER_TYPE.ETC:
                                parser = new ParserOrderInputCommon();
                                break;
                        }

                        // 입력포트 시작 - 읽기(listen) 대기 포트번호를 가지고 열어둔다
                        m_order_input_listen_serial_port_list[i] = new SerialPortOrderInput(input_config.m_listen_port_num, input_config.m_listen_port_baud, parser, input_config.m_conn_print_port_num);
                        Utility.Utility.LogWrite("parser : " + parser);
                        m_order_input_listen_serial_port_list[i].Notify += onNotifyOrderInputSerialPort;

                        m_order_input_listen_serial_port_list[i].connectSerialPort();
                        Utility.Utility.LogWrite("가즈아!!");
                        // dictionary
                        m_dic_order_input_listen_serial_port.Add(input_config.m_listen_port_num, m_order_input_listen_serial_port_list[i]);

                        // count
                        m_connected_input_port_count++;
                    }
                }
                catch (Exception err)
                {
                    Utility.Utility.LogWrite("connectSerialPorts여기서 예외 발생함 : " + err.Message);
                    ModelAppDevice device = new ModelAppDevice();
                    device.writeLog(err.Message);
                }
            }
        }

        public void disconnectSerialPorts()
        {
            Utility.Utility.LogWrite("disconnectSerialPorts호출");
            // order-input port
            if (null != m_order_input_listen_serial_port_list)
            {
                for (int i = 0; i < m_order_input_listen_serial_port_list.Length; i++)
                {
                    if (null != m_order_input_listen_serial_port_list[i])
                    {
                        m_order_input_listen_serial_port_list[i].Notify -= onNotifyOrderInputSerialPort;
                        m_order_input_listen_serial_port_list[i].disconnectSerialPort();
                        m_order_input_listen_serial_port_list[i] = null;
                    }
                }
                m_order_input_listen_serial_port_list = null;
            }
            m_connected_input_port_count = 0;

            // clear dictionary
            if (null != m_dic_order_input_listen_serial_port)
            {
                m_dic_order_input_listen_serial_port.Clear();
            }
        }

        // ----------------------------------------------------------
        //
        public void procParsing()
        {
            Utility.Utility.LogWrite("procParsing");

            m_read_rawdata.initObj();
            while (m_pool_orderinput_rawdata.pop(ref m_read_rawdata)) // pop from pool list
            {
                Utility.Utility.LogWrite("m_read_rawdata : " + m_read_rawdata);
                // set ref
                ObjOrderInputSerialRawDataBuf buf_obj = m_read_rawdata;
                if (null != buf_obj)
                {
                    // notify
                    sendMyNotify(SerialPortOrderInput.SERIAL_PORT_NOTIFY_WHAT.RAW_DATA_STREAM, buf_obj);

                    // parsing
                    SerialPortOrderInput serial_port = m_read_rawdata.m_caller_serial_port;
                    if (null != serial_port)
                    {
                        ParserOrderInputBase parser = serial_port.getOrderParser();

                        Utility.Utility.LogWrite("procParsing parser : " + parser);

                        if (null != parser) // 파서가 지정되어 있을때만 파싱 요청한다
                        {
                            Utility.Utility.LogWrite("buf_obj.m_data_buf : " + System.Text.Encoding.Default.GetString(buf_obj.m_data_buf));
                            serial_port.saveRawDataToParsing(buf_obj.m_data_buf, 0, buf_obj.m_data_len);
                        }
                    }

                    // reset read object
                    buf_obj.initObj();
                }
            }
        }

        // ----------------------------------------------------------
        //
        private void onNotifyOrderInputSerialPort(object _sender, object _what, object _obj)
        {
            Utility.Utility.LogWrite("onNotifyOrderInputSerialPort");

            switch ((SerialPortOrderInput.SERIAL_PORT_NOTIFY_WHAT)_what)
            {
                case SerialPortOrderInput.SERIAL_PORT_NOTIFY_WHAT.RAW_DATA_STREAM:
                    {
                        Utility.Utility.LogWrite("case SerialPortOrderInput.SERIAL_PORT_NOTIFY_WHAT.RAW_DATA_STREAM:");
                        // 받은 데이터를 풀에 저장해 둔다
                        SerialPortOrderInput.SerialRawDataBufInfo src = (SerialPortOrderInput.SerialRawDataBufInfo)_obj;
                        int src_offset = src.offset;
                        int remain_len = src.len;
                        while (0 < remain_len)
                        {
                            Utility.Utility.LogWrite("while안");
                            ObjOrderInputSerialRawDataBuf des = m_pool_orderinput_rawdata.create(); // push to pool list
                            if (null != des)
                            {
                                Utility.Utility.LogWrite("if 안");
                                des.m_caller_serial_port = src.caller_serial;

                                int copy_len = (remain_len < des.m_data_buf.Length ? remain_len : des.m_data_buf.Length);
                                
                                if (0 < copy_len)
                                {
                                    Utility.Utility.LogWrite("copylen if 안");
                                    System.Array.Copy(src.buf, src_offset, des.m_data_buf, 0, copy_len);
                                }
                                des.m_data_len = copy_len;

                                // set remain length
                                src_offset += copy_len;
                                remain_len -= copy_len;
                            }
                        }
                    }
                    break;

                case SerialPortOrderInput.SERIAL_PORT_NOTIFY_WHAT.ORDER_OBJECT:

                    Utility.Utility.LogWrite("case SerialPortOrderInput.SERIAL_PORT_NOTIFY_WHAT.ORDER_OBJECT:");
                    sendMyNotify(_what, _obj);
                    break;

                default:
                    sendMyNotify(_what, _obj);
                    break;
            }
        }

        // ----------------------------------------------------------
        //
        public void sendTestData(String _input_port_num, byte[] _recv_buf, int _recv_offset, int _recv_length)
        {
            if (null != _input_port_num && m_dic_order_input_listen_serial_port.ContainsKey(_input_port_num))
            {
                SerialPortOrderInput sel_port = m_dic_order_input_listen_serial_port[_input_port_num];
                if (null != sel_port)
                {
                    sel_port.onReceivedData(_recv_buf, _recv_offset, _recv_length);
                }
            }
        }
    }
}
