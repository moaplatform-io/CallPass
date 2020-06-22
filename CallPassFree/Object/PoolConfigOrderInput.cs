using Kons.ShopCallpass.Object;
using Kons.ShopCallpass.FormPopup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO.Ports;

namespace Kons.ShopCallpass.Object
{
    public class PoolConfigOrderInput
    {
        public ObjConfigOrderInput[] m_list = null;
        //public ObjConfigOrderInput[] m_prev_connect_list = null;
        public int COUNT { get { return (null == m_list ? 0 : m_list.Length); } }
        public bool IS_CHANGE { get; set; }

        // --------------------------------------
        //
        public PoolConfigOrderInput(int _count)
        {
            m_list = new ObjConfigOrderInput[_count];
            for (int i = 0; i < _count; i++)
            {
                m_list[i] = new ObjConfigOrderInput();
            }
        }

        public ObjConfigOrderInput getObject(int _index)
        {
            if (_index < m_list.Length)
            {
                return m_list[_index];
            }
            return null;
        }

        public void loadObjectAll()
        {
            for (int i = 0; i < m_list.Length; i++)
            {
                m_list[i].loadFromDevice(i);
            }
        }

        public void saveObjectAll()
        {
            for (int i = 0; i < m_list.Length; i++)
            {
                m_list[i].saveToDevice();
            }
        }
            

        public bool isVailedData()
        {
            Dictionary<String, ObjConfigOrderInput> dic = new Dictionary<string, ObjConfigOrderInput>();
            for (int i = 0; i < m_list.Length; i++)
            {
                ObjConfigOrderInput sel_obj = m_list[i];
                if (sel_obj.IS_USE)
                {
                    String sel_key = sel_obj.m_listen_port_num; // 실제로 오픈하는 것은 리슨 포트이므로 이것을 확인
                    if (dic.ContainsKey(sel_key))
                    {
                        dic.Clear();
                        return false;
                    }
                    dic.Add(sel_key, sel_obj);
                }
            }
            dic.Clear();
            return true;
        }

        public bool isRepeatValue()
        {
            List<string> selectPort = new List<string>();
            for (int i = 0; i < m_list.Length; i++)
            {
                if (m_list[i].IS_USE)
                {
                    selectPort.Add(m_list[i].m_input_port_num);
                    selectPort.Add(m_list[i].m_listen_port_num);
                }
            }
            for (int i = 0; i < selectPort.Count; i++)
            {
                for (int j = 0; j < selectPort.Count; j++)
                {
                    if (i != j && selectPort.ElementAt(i).Equals(selectPort.ElementAt(j)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool isOtherDeviceUse(PoolConfigOrderInput _prev_connect_list)
        {

            //for (int i = 0; i < _prev_connect_list.m_list.Length; i ++)
            //{
            //    Debug.WriteLine("이전 연결된 포트 : " + _prev_connect_list.m_list[i].m_input_port_num + " " + _prev_connect_list.m_list[i].m_listen_port_num);
            //    Debug.WriteLine("_prev_connect_list.m_list[i].m_is_install: " + _prev_connect_list.m_list[i].m_is_install);
            //    PcAppMain.Program.LogWrite("이전 연결된 포트 : " + _prev_connect_list.m_list[i].m_input_port_num + " " + _prev_connect_list.m_list[i].m_listen_port_num);
            //    PcAppMain.Program.LogWrite("_prev_connect_list.m_list[i].m_is_install: " + _prev_connect_list.m_list[i].m_is_install);
            //}
            string[] comlist = SerialPort.GetPortNames();

            //Debug.Write("다른 장치에서 사용중인 포트 목록 : ");
            //for (int i = 0; i < comlist.Length; i++)
            //{
            //    Debug.Write(comlist[i] + " ");
            //    PcAppMain.Program.LogWrite(comlist[i] + " ");
            //}
            //Debug.Write("\r\n");
            //PcAppMain.Program.LogWrite("\r\n");


            for (int i = 0; i < _prev_connect_list.m_list.Length; i++)
            {
                for (int j = 0; j < comlist.Length; j++)
                {
                    if (_prev_connect_list.m_list[i].IS_USE) // 바꾸고 테스팅 안해봄.
                    {
                        if (comlist[j].Contains(_prev_connect_list.m_list[i].m_input_port_num))
                        {
                            comlist[j] = "erase";
                        }
                        else if (comlist[j].Contains(_prev_connect_list.m_list[i].m_listen_port_num))
                        {
                            comlist[j] = "erase";
                        }
                    }
                }
            }

            Utility.Utility.LogWrite("다른 장치에서 사용중인 포트 목록 지운 후: ");
            for (int i = 0; i < comlist.Length; i++)
            {
                Utility.Utility.LogWrite(comlist[i] + " ");
            }
            Utility.Utility.LogWrite("\r\n");

            for (int i = 0; i < m_list.Length; i++)
            {
                for (int j = 0; j < comlist.Length; j++)
                {
                    if (m_list[i].IS_USE)
                    {
                        if (comlist[j].Contains(m_list[i].m_input_port_num) || comlist[j].Contains(m_list[i].m_listen_port_num))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public bool isValiedPort()
        {
            for (int i = 0; i < m_list.Length; i++)
            {
                if (m_list[i].m_input_port_num == "COM4" || m_list[i].m_listen_port_num == "COM4")
                {
                    return false;
                }
            }
            return true;
        }
    }
}
