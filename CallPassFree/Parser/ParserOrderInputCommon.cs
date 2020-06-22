using Kons.ShopCallpass.Object;
using Kons.ShopCallpass.Parser;
using Kons.TsLibrary;
using Kons.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using Kons.ShopCallpass.Model;
using System.Text.RegularExpressions;
using System.IO;
using Kons.ShopCallpass.Controller;
using MySql.Data.MySqlClient;
using Renci.SshNet;
//using System.Net.Json;
using System.Net;

namespace Kons.ShopCallpass.Parser
{
    public class ParserOrderInputCommon : ParserOrderInputBase
    {
        static int orderCount = 1;
        public ParserOrderInputCommon()
        {
            setParserType(PARSER_TYPE.EASYPOS);
        }

        override public byte[] getInputData_SOD()
        {
            byte[] vals = {0x10, 0x04};
            return vals;
        }

        override public byte[] getInputData_EOD()
        {
            byte[] vals = {0x1B, 0x6D};
            return vals;
        }

        override public ObjOrder parsingInputOrderRawData(byte[] _raw_data_buf, int _raw_data_len)
        {
            ModelAppDevice device = new ModelAppDevice();
            //ByteArrayToFile(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\parsingbuf.txt", _raw_data_buf);
            Utility.Utility.LogWrite("parsingInputOrderRawData");

            string temp = device.readConfigString("INTERVALREMOVEBINARYKEY", "interval_remove_binary_key");
            string[] temps = temp.Split('+');
            for (int i = 0; i < temps.Length; i++)
            {  
                string[] points = temps[i].Split('~');
                Utility.Utility.LogWrite("a");
                if (points.Length == 2)
                {
                    Utility.Utility.LogWrite("points[1].Length : " + points[1].Length);
                    for (int j = 0; j < points[1].Length; j++)
                    {
                        Utility.Utility.LogWrite("point[i] : " + points[1].ElementAt(j));
                    }
                    int start;
                    while ((start = IndexOf(_raw_data_buf, ConvertByteArray(points[0]))) > -1)
                    {
                        Utility.Utility.LogWrite("b");
                        int end = -1;

                        end = IndexOf(_raw_data_buf, ConvertByteArray(points[1]));

                        while (end < start)
                        {
                            for (int j = end; j < end + ConvertByteArray(points[1]).Length; j++)
                            {
                                _raw_data_buf[j] = 0x00;
                            }
                            end = IndexOf(_raw_data_buf, ConvertByteArray(points[1]));
                            Utility.Utility.LogWrite("start : " + start + " end : " + end);
                        }

                        Utility.Utility.LogWrite("start : " + start);
                        Utility.Utility.LogWrite("end : " + end);
                        if (end == -1)
                        {
                            break;
                        }
                        for (int j = start; j < end + ConvertByteArray(points[1]).Length; j++)
                        {
                            Utility.Utility.LogWrite("erase");
                            _raw_data_buf[j] = 0x00;
                        }
                        Utility.Utility.LogWrite("여기는");
                    }
                }
            }
            MemoryStream osssut = new MemoryStream();
            for (int i = 0; i < _raw_data_len; i++)
            {
                //Utility.Utility.LogWrite("d");
                if (_raw_data_buf[i] >= 32 && _raw_data_buf[i] <= 126 || _raw_data_buf[i] >= 160 && _raw_data_buf[i] <= 255 && 
                    i > 0 && i + 1 < _raw_data_buf.Length && _raw_data_buf[i - 1] != 0x1B && _raw_data_buf[i - 1] != 0x1D && _raw_data_buf[i + 1] != 0x1B ||
                    _raw_data_buf[i] == 0x0A || _raw_data_buf[i] == 0x29)
                {
                    osssut.WriteByte(_raw_data_buf[i]);
                }
            }
            osssut.Flush();
            osssut.Close();
            Utility.Utility.LogWrite("tel");
            byte[] kym = osssut.ToArray();

            byte[] val = { 0x0A };
            byte[] val2 = { 0x20 };
            byte[] afterRemoive = ReplaceBytes(osssut.ToArray(), val, val2);
            Utility.Utility.LogWrite("와우 : " + EncodingToStringFitness(CheckSender(), kym));
            Utility.Utility.LogWrite("예스 :  " + EncodingToStringFitness(CheckSender(), afterRemoive));
            //ByteArrayToFile(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\remove0.txt", afterRemoive);
            ObjOrder recv_obj = new ObjOrder();
            //String recv_data_string = System.Text.Encoding.Default.GetString(_raw_data_buf, 0, _raw_data_len);
            String recv_data_string = EncodingToStringFitness(CheckSender(), afterRemoive);
            Utility.Utility.LogWrite("recv_data_string : " + recv_data_string);

            if (null != recv_data_string && 0 < recv_data_string.Length)
            {
                parsingOrderInputString(recv_data_string, kym, ref recv_obj);
            }
            return recv_obj;
        }

        override public string getAttacthText_OrderNumHead()
        {
            return "EP-";
        }
        public static int IndexOf(byte[] arrayToSearchThrough, byte[] patternToFind)
        {
            if (patternToFind.Length > arrayToSearchThrough.Length)
                return -1;
            for (int i = 0; i < arrayToSearchThrough.Length - patternToFind.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < patternToFind.Length; j++)
                {
                    if (arrayToSearchThrough[i + j] != patternToFind[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return i;
                }
            }
            return -1;
        }
 
        private bool parsingOrderInputString(String _recv_data_string, byte[] _onlyRequestByteArr, ref ObjOrder _des_obj/*,int strlenth*/)
        {
            Utility.Utility.LogWrite("CommonParsingOrderInputString");
            try
            {
                string onlyRequestByteArr = EncodingToStringFitness(CheckSender(), _onlyRequestByteArr);

                //_recv_data_string = _recv_data_string.Replace("\n","").Replace(")","-").Replace(" ", "");
                _recv_data_string = _recv_data_string.Replace(")", "-").Replace("(", "-");
                // 주문번호
                _des_obj.m_order_num = getParsingValue_order_num(_recv_data_string);
                Utility.Utility.LogWrite("_des_obj.m_order_num : "  + _des_obj.m_order_num);
                // 고객 합계금액
                _des_obj.m_customer_cost = getParsingValue_customer_cost(_recv_data_string);

                // 결제수단
                _des_obj.m_customer_pay_type_cd = getParsingValue_customer_pay_type_cd(_recv_data_string);
                
                // 고객주소
                _des_obj.m_arv_locate_address = getParsingValue_arv_locate_address(onlyRequestByteArr);
                _des_obj.m_arv_locate_memo = DetailAddress(_des_obj.m_arv_locate_address);
                Debug.WriteLine("_des_obj.m_arv_locate_memo : " + _des_obj.m_arv_locate_memo);
                
                // 고객전화
                _des_obj.m_call_num = getParsingValue_call_num(_recv_data_string);

                // 고객요청사항
                _des_obj.m_customer_request_memo = getParsingValue_customer_request_memo(onlyRequestByteArr);
                Utility.Utility.LogWrite("_des_obj.m_customer_request_memo : " + _des_obj.m_customer_request_memo);

                ModelAppDevice device = new ModelAppDevice();
                string val = device.readConfigString("TEMP_PORTNAME", "temp_portName");
                
                for (int i = 0; i < 4; i++)
                {
                    if (val.Equals(device.readConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString())))
                    {
                        _des_obj.m_order_type = Int32.Parse(device.readConfigString("CONFIG_ORDER_INPUT", "input_type_" + i.ToString()));
                    }
                }

                // 주문일시
                _des_obj.m_call_datetime = getParsingValue_call_datetime(_recv_data_string);
                _des_obj.m_order_biz_date = KnUtil.getIntDateYMD(_des_obj.m_call_datetime);

                // ########################################
                // 기타 없는 값 초기화
                
                // 도착메모
                //_des_obj.m_arv_locate_memo = "";

                _des_obj.m_customer_pay_type_memo = "";
                // order
                /*_des_obj.m_order_type = (int)getParserType();*/
                _des_obj.m_state_cd = (int)ObjOrder.STATE_TYPE.ORDER_STATE_0;

                //_des_obj.m_customer_request_memo = "";
                _des_obj.m_date_1 = DateTime.MinValue;
                _des_obj.m_date_2 = DateTime.MinValue;
                _des_obj.m_date_3 = DateTime.MinValue;
                _des_obj.m_date_4 = DateTime.MinValue;
                _des_obj.m_date_5 = DateTime.MinValue;
                _des_obj.m_date_6 = DateTime.MinValue;
                _des_obj.m_date_7 = DateTime.MinValue;

                // shop
                _des_obj.m_shop_name = "";
                _des_obj.m_shop_request_time = 0;
                _des_obj.m_shop_request_option = 0;
                _des_obj.m_shop_cost = 0;

                // customer
                _des_obj.m_customer_name = "";
                _des_obj.m_customer_additional_cost = 0;
                _des_obj.m_customer_additional_cost_memo = "";

                _des_obj.m_customer_request_option = 0;
                _des_obj.m_customer_request_time = 0;
                
                // ########################################
                // Locate Arv (Shop Customer의 위치 및 주소정보)
                _des_obj.m_arv_locate_name = "";
                _des_obj.m_arv_person_tel_num = "";
                _des_obj.m_arv_person_memo = "";

                //string order_type = "";

                //switch (_des_obj.m_order_type)
                //{
                //    case 10:
                //        order_type = "배달의민족";
                //        break;
                //    case 21:
                //        order_type = "오케이포스";
                //        break;
                //    case 26:
                //        order_type = "배달천재";
                //        break;
                //    case 27:
                //        order_type = "포스피드";
                //        break;
                //    case 31:
                //        order_type = "이지포스";
                //        break;
                //    case 32:
                //        order_type = "요기요";
                //        break;
                //    case 33:
                //        order_type = "배달통";
                //        break;
                //    case 98:
                //        order_type = "기타";
                //        break;
                //}

                //string pay_type = "";

                //switch (_des_obj.m_customer_pay_type_cd)
                //{
                //    case 0:
                //        pay_type = "현금";
                //        break;
                //    case 1:
                //        pay_type = "카드";
                //        break;
                //    case 4:
                //        pay_type = "선불";
                //        break;
                //    case 77:
                //        pay_type = "표시없음";
                //        break;
                //}

                //string store_id = "";
                //string store_num = "";
                //string store_name = "";
                //string store_pno = "";
                //string store_tel = "";
                //string store_addr = "";

                //if (device.readConfigLong("STOREMAPPINGCHECK", "store_mapping_check") == 1)
                //{
                //    string company_type = device.readConfigString("CONFIG_LAST_DELIVERY_REQUEST_INFO", "delivery_company_type");

                //    store_id = device.readConfigString("CONFIG_STORE_API_REG_INFO", "store_id_" + company_type);
                //    store_num = device.readConfigString("CONFIG_STORE_API_INFO", "store_num_" + company_type);
                //    store_name = device.readConfigString("CONFIG_STORE_API_INFO", "store_name_" + company_type);
                //    store_pno = device.readConfigString("CONFIG_STORE_API_INFO", "store_pno_" + company_type);
                //    store_tel = device.readConfigString("CONFIG_STORE_API_INFO", "store_tel_" + company_type);
                //    store_addr = device.readConfigString("CONFIG_STORE_API_INFO", "store_addr_" + company_type);
                //}

                //JsonObjectCollection obj = new JsonObjectCollection();
                //obj.Add(new JsonStringValue("dateTime", _des_obj.m_call_datetime.ToString()));

                //obj.Add(new JsonStringValue("storeId", store_id));
                //obj.Add(new JsonStringValue("storeName", store_name));
                //obj.Add(new JsonStringValue("storePno", store_pno));
                //obj.Add(new JsonStringValue("storeNum", store_num));
                //obj.Add(new JsonStringValue("storeTel", store_tel));
                //obj.Add(new JsonStringValue("storeAddr", store_addr));

                //obj.Add(new JsonStringValue("orderNum", _des_obj.m_order_num));
                //obj.Add(new JsonStringValue("orderType", order_type));
                //obj.Add(new JsonStringValue("payType", pay_type));
                //obj.Add(new JsonStringValue("cost", _des_obj.m_customer_cost.ToString()));
                //obj.Add(new JsonStringValue("customerRequestMemo", _des_obj.m_customer_request_memo));
                //obj.Add(new JsonStringValue("customerCallNum", _des_obj.m_call_num));
                //obj.Add(new JsonStringValue("customerAddress", _des_obj.m_arv_locate_address));

                //string sEntity = obj.ToString();
                //string url = "http://127.0.0.1:8080/bServer/moaBtwoB/DBInsert.json";
                //Utility.Utility.SendHTTP(sEntity,  url);

            }
            catch (Exception err)
            {
                Utility.Utility.LogWrite("check second recep::" + err.Message);
                TsLog.writeLog(err.Message);
                return false;
            }

            return true;
        }

        String getParsingValue_order_type()
        {
            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                string temp = device.readConfigString("TEMP_PORTNAME", "temp_portname");
                Utility.Utility.LogWrite("들어온 포트 temp : " + temp);

                for (int i = 0; i < 4; i++)
                {
                    if (temp == device.readConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString()))
                    {
                        Utility.Utility.LogWrite("읽어온 포트 temp : " + device.readConfigString("CONFIG_ORDER_INPUT", "selectedType_" + i.ToString()));
                        return device.readConfigString("CONFIG_ORDER_INPUT", "selectedType_" + i.ToString());
                    }
                }
            }
                return "";
        }
        private String getParsingValue_order_num(String _recv_data)
        {   
            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                string temp = device.readConfigString("EXCLUSIVEKEYWORD", "exclusive_keyword");
                string tmp = device.readConfigString("ORDER_NUM", "order_num");
                Utility.Utility.LogWrite("tmp : " + tmp);
                string[] patterns = tmp.Split('+');
                string[] exclusiveKeyword = temp.Split('+');

                Utility.Utility.LogWrite("words.Length : " + patterns.Length);
                for (int i = 0; i < patterns.Length; i++)
                {
                    Utility.Utility.LogWrite("words[i] : " + patterns[i]);
                }

                for (int i = 0; i < exclusiveKeyword.Length; i++)
                {
                    _recv_data = _recv_data.Replace(exclusiveKeyword[i], "");
                }

                for (int i = 0; i < patterns.Length; i++)
                {
                    string normalization = null;
                    for (int j = 0; j < patterns[i].Length; j++)
                    {
                        if (patterns[i].ElementAt(j) == 'n')
                        {
                            normalization += "[0-9]{1}";
                        }
                        else if (patterns[i].ElementAt(j) == 'a')
                        {
                            normalization += "[a-zA-Z]{1}";
                        }
                        else if (patterns[i].ElementAt(j) == '?')
                        {
                            normalization += "[a-zA-Z0-9]{1}";
                        }
                        else
                        {
                            normalization += patterns[i].ElementAt(j);
                        }
                    }

                    Utility.Utility.LogWrite("normalization : " + normalization);

                    normalization = normalization.Replace(")", "-");
                    Regex rgx = new Regex(normalization);

                    foreach (Match match in rgx.Matches(_recv_data))
                    {
                        if (match.Success)
                        {
                            return match.Value;
                        }
                    }
                }
            }
            return "absence" + (orderCount++).ToString();
        }

        // 주문시간
        private DateTime getParsingValue_call_datetime(String _recv_data)
        {
            Utility.Utility.LogWrite("getParsingValue_call_datetime");
            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                string temp = device.readConfigString("EXCLUSIVEKEYWORD", "exclusive_keyword");
                string tmp = device.readConfigString("ORDER_DATE", "order_date");
                Utility.Utility.LogWrite("tmp : " + tmp);
                string[] patterns = tmp.Split('+');
                string[] exclusiveKeyword = temp.Split('+');

                Utility.Utility.LogWrite("patterns.Length : " + patterns.Length);
                for (int i = 0; i < patterns.Length; i++)
                {
                    Utility.Utility.LogWrite("patterns[i] : " + patterns[i]);
                }

                for (int i = 0; i < exclusiveKeyword.Length; i++)
                {
                    _recv_data = _recv_data.Replace(exclusiveKeyword[i], "");
                }

                for (int i = 0; i < patterns.Length; i++)
                {
                    string normalization = null;
                    int[] YMDtms = new int[5];
                    int count = 1;

                    for (int j = 0; j < YMDtms.Length; j++)
                    {
                        YMDtms[j] = 0;
                    }

                    bool isfind = false;
                    for (int j = 0; j < patterns[i].Length; j++)
                    {
                        if (patterns[i].ElementAt(j) == 'y')
                        {
                            normalization += "[0-9]{4}";
                            YMDtms[0] = count++;
                        }
                        else if (patterns[i].ElementAt(j) == 'm')
                        {
                            normalization += "[0-9]{1,2}";
                            YMDtms[1] = count++;
                        }
                        else if (patterns[i].ElementAt(j) == 'd')
                        {
                            normalization += "[0-9]{1,2}";
                            YMDtms[2] = count++;
                        }
                        else if (patterns[i].ElementAt(j) == 'w')
                        {
                            normalization += "[가-힣]{1}";
                        }
                        else if (patterns[i].ElementAt(j) == 'T')
                        {
                            normalization += "[0-9]{1,2}";
                            YMDtms[3] = count++;
                        }
                        else if (patterns[i].ElementAt(j) == 'M')
                        {
                            normalization += "[0-9]{1,2}";
                            YMDtms[4] = count++;
                            isfind = true;
                        }
                        else
                        {
                            if (!isfind)
                            {
                                normalization += patterns[i].ElementAt(j);
                            }
                        }
                    }

                    Utility.Utility.LogWrite("normalization : " + normalization);
                    normalization = normalization.Replace(")", "-").Replace("(", "-");

                    Regex rgx = new Regex(normalization);

                    foreach (Match match in rgx.Matches(_recv_data))
                    {
                        if (match.Success)
                        {
                            Utility.Utility.LogWrite("match.Value 데이트타임값 : " + match.Value);

                            string[] numberCollections = new string[5];
                            for (int j = 0; j < numberCollections.Length; j++)
                            {
                                numberCollections[j] = "";
                            }
                            bool isValidCount = false;
                            int index = 0;
                            for (int j = 0; j < numberCollections.Length; j++)
                            {
                                for (int k = index; k < match.Value.Length; k++)
                                {
                                    if (match.Value[k] >= 48 && match.Value[k] <= 57)
                                    {
                                        isValidCount = true;
                                        numberCollections[j] += match.Value[k];
                                    }
                                    else
                                    {
                                        if (!isValidCount)
                                        {
                                            j--;
                                        }
                                        isValidCount = false;
                                        index = k + 1;
                                        break;
                                    }
                                }
                            }

                            string[] answer = new string[5];

                            for (int j = 0; j < answer.Length; j++)
                            {
                                answer[j] = "";
                            }

                            for (int j = 0; j < numberCollections.Length; j++)
                            {
                                Utility.Utility.LogWrite("numberCollections[] : " + numberCollections[j]);
                            }


                            for (int j = 0; j < numberCollections.Length; j++)
                            {
                                answer[j] = numberCollections[YMDtms[j] - 1];
                            }

                            if (match.Value.Contains("후") || match.Value.Contains("P") || match.Value.Contains("p"))
                            {
                                answer[3] = (Int32.Parse(answer[3]) + 12).ToString();
                            }

                            for (int j = 0; j < answer.Length; j++)
                            {
                                Utility.Utility.LogWrite("answer[] : " + answer[j]);
                            }

                            if (answer[1].Length == 1)
                            {
                                answer[1] = "0" + answer[1];
                            }
                            if (answer[2].Length == 1)
                            {
                                answer[2] = "0" + answer[2];
                            }
                            if (answer[3].Length == 1)
                            {
                                answer[3] = "0" + answer[3];
                            }
                            if (answer[4].Length == 1)
                            {
                                answer[4] = "0" + answer[4];
                            }

                            string returnValue = null;
                            returnValue = answer[0] + "-" + answer[1] + "-" + answer[2] + " " +answer[3] + ":" + answer[4];

                            Utility.Utility.LogWrite("returnValue : " + returnValue);
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            DateTime dateTime12 = DateTime.ParseExact(returnValue, "yyyy-MM-dd HH:mm", provider);
                            Utility.Utility.LogWrite("날짜 now: " + dateTime12);
                            Utility.Utility.LogWrite("dateTime12 : " + dateTime12);
                            return dateTime12;
                        }
                    }
                }
            }
            Utility.Utility.LogWrite("그냥now로빠짐");
            return DateTime.Now;
        }

        // 고객연락처
        private String getParsingValue_call_num(String _recv_data)
        {

            Utility.Utility.LogWrite("getParsingValue_call_num");
            Utility.Utility.LogWrite("_recv_data전화번호데이터 : " + _recv_data);

            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                string temp = device.readConfigString("EXCLUSIVEKEYWORD", "exclusive_keyword");
                string tmp = device.readConfigString("CALL_NUM", "call_num");
                Utility.Utility.LogWrite("tmp : " + tmp);
                string[] patterns = tmp.Split('+');
                string[] exclusiveKeyword = temp.Split('+');

                Utility.Utility.LogWrite("words.Length : " + patterns.Length);
                for (int i = 0; i < patterns.Length; i++)
                {
                    Utility.Utility.LogWrite("words[i] : " + patterns[i]);
                }

                for (int i = 0; i < exclusiveKeyword.Length; i++)
                {
                    _recv_data = _recv_data.Replace(exclusiveKeyword[i], "");
                }

                Utility.Utility.LogWrite("receivedata 가즈아 : " + _recv_data);
                for (int i = 0; i < patterns.Length; i++)
                {
                    string normalization = null;
                    for (int j = 0; j < patterns[i].Length; j++)
                    {
                        if (patterns[i].ElementAt(j) == 'n')
                        {
                            normalization += "[0-9]{1}";
                        }
                        else
                        {
                            normalization += patterns[i].ElementAt(j);
                        }
                    }

                    Utility.Utility.LogWrite("normalization : " + normalization);

                    normalization = normalization.Replace(")", "-");

                    Regex rgx = new Regex(normalization);
                    //Utility.Utility.LogWrite("_recievdata : " + _recv_data);

                    foreach (Match match in rgx.Matches(_recv_data))
                    {
                        Utility.Utility.LogWrite("짱짱짱");
                        if (match.Success)
                        {
                            Utility.Utility.LogWrite("들어옴");
                            //_recv_data = _recv_data.Replace(match.Value, "");

                            if (_recv_data[_recv_data.IndexOf(match.Value) - 1] >= 48 && _recv_data[_recv_data.IndexOf(match.Value) - 1] <= 57 ||
                                _recv_data[_recv_data.IndexOf(match.Value) + match.Value.Length] >= 48 && _recv_data[_recv_data.IndexOf(match.Value) + match.Value.Length] <= 57)
                            {
                                continue;
                            }
                            Utility.Utility.LogWrite("전화번호 match.Value :" + match.Value);
                            return match.Value;
                        }
                    }
                }
            }
            return "";
        }

        // 주소찾기
        private String getParsingValue_arv_locate_address(String _recv_data)
        {
            Utility.Utility.LogWrite("getParsingValue_arv_locate_address");


            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                string temp = device.readConfigString("EXCLUSIVEKEYWORD", "exclusive_keyword");
                string tmp = device.readConfigString("ADDRESS", "address");
                Utility.Utility.LogWrite("tmp : " + tmp);
                string[] patterns = tmp.Split('+');
                string[] exclusiveKeyword = temp.Split('+');

                Utility.Utility.LogWrite("patterns.Length : " + patterns.Length);
                for (int i = 0; i < patterns.Length; i++)
                {
                    Utility.Utility.LogWrite("patterns[i] : " + patterns[i]);
                }

                for (int i = 0; i < exclusiveKeyword.Length; i++)
                {
                    Utility.Utility.LogWrite("exclusiveKeyword 키워드: " + exclusiveKeyword[i]);
                    _recv_data = _recv_data.Replace(exclusiveKeyword[i], "");
                }
                string[] recvData = _recv_data.Split(new string[] { "\n" }, StringSplitOptions.None);

                int recognize = 0;
                string deviceValue = "";

                string val = device.readConfigString("TEMP_PORTNAME", "temp_portName");

                for (int i = 0; i < 4; i++)
                {
                    if (val.Equals(device.readConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString())))
                    {
                        recognize = Int32.Parse(device.readConfigString("CONFIG_ORDER_INPUT", "input_type_" + i.ToString()));
                    }
                }
                Utility.Utility.LogWrite("recognize : " + recognize);

                const string ADDRESSENDCHAR = "ADDRESSENDCHAR";

                switch (recognize)
                {
                    case 10:
                        deviceValue = device.readConfigString(ADDRESSENDCHAR, "bemin_addr_end_char");
                        break;
                    case 21:
                        deviceValue = device.readConfigString(ADDRESSENDCHAR, "okpos_addr_end_char");
                        break;
                    case 26:
                        deviceValue = device.readConfigString(ADDRESSENDCHAR, "delgen_addr_end_char");
                        break;
                    case 27:
                        deviceValue = device.readConfigString(ADDRESSENDCHAR, "posfeed_addr_end_char");
                        break;
                    case 31:
                        deviceValue = device.readConfigString(ADDRESSENDCHAR, "easypos_addr_end_char");
                        break;
                    case 32:
                        deviceValue = device.readConfigString(ADDRESSENDCHAR, "yogiyo_addr_end_char");
                        break;
                    case 33:
                        deviceValue = device.readConfigString(ADDRESSENDCHAR, "bedaltong_addr_end_char");
                        break;
                    case 98:
                        deviceValue = device.readConfigString(ADDRESSENDCHAR, "etc_addr_end_char");
                        break;
                }

                for (int i = 0; i < recvData.Length; i++)
                {
                    Utility.Utility.LogWrite("recvData[i]; : " + recvData[i]);
                }

                string[] startAddr = { "서울", "인천", "대전", "광주", "대구", "울산", "부산", 
                "경기", "강원", "충남", "충북", "전북", "전남", "경북", "경남", "제주", 
                "충청남도", "충청북도", "전라북도", "전라남도", "경상북도", "경상남도"};

                if (deviceValue == "#")
                {
                    Utility.Utility.LogWrite("#으로 빠짐");

                    for (int i = 0; i < patterns.Length; i++)
                    {
                        int idx = 0;

                        if ((idx = _recv_data.IndexOf(patterns[i])) > -1)
                        {
                            string returnValue = "";

                            int cnt = 0;
                            while (cnt < 2)
                            {
                                if (idx < _recv_data.Length)
                                {
                                    if (_recv_data[idx] == '\n')
                                    {
                                        cnt++;
                                        idx++;
                                    }
                                    else
                                    {
                                        returnValue += _recv_data[idx++];
                                    }
                                }
                            }

                            Utility.Utility.LogWrite("상세주소 제외전 : " + returnValue);

                            string removeKeyword = "[상세주소] ";
                            if (returnValue.Contains(removeKeyword))
                            {
                                returnValue = returnValue.Substring(returnValue.IndexOf(removeKeyword) + removeKeyword.Length);
                            }
                            Utility.Utility.LogWrite("#patterns주소 리턴 값 : " + returnValue);

                            returnValue = RemoveParenthesisInAddress(returnValue);
                            Utility.Utility.LogWrite("주소 괄호 삭제 후 리턴 값 : " + returnValue);

                            return returnValue;
                        }
                    }

                    for (int i = 0; i < startAddr.Length; i++)
                    {
                        int idx = 0;

                        if ((idx = _recv_data.IndexOf(startAddr[i])) > -1)
                        {
                            string returnValue = "";

                            int cnt = 0;
                            while (cnt < 2)
                            {
                                if (idx < _recv_data.Length)
                                {
                                    if (_recv_data[idx] == '\n')
                                    {
                                        cnt++;
                                        idx++;
                                    }
                                    else
                                    {
                                        returnValue += _recv_data[idx++];
                                    }
                                }
                            }
                            Utility.Utility.LogWrite("상세주소 제외전 : " + returnValue);

                            string removeKeyword = "[상세주소] ";
                            if (returnValue.Contains(removeKeyword))
                            {
                                returnValue = returnValue.Substring(returnValue.IndexOf(removeKeyword) + removeKeyword.Length);
                            }

                            Utility.Utility.LogWrite("#startAddr주소 리턴 값 : " + returnValue);

                            returnValue = RemoveParenthesisInAddress(returnValue);
                            Utility.Utility.LogWrite("주소 괄호 삭제 후 리턴 값 : " + returnValue);

                            return returnValue;
                        }
                    }
                }

                for (int i = 0; i < recvData.Length; i++)
                {
                    for (int j = 0; j < patterns.Length; j++)
                    {
                        int idx = 0;
                        if ((idx = recvData[i].IndexOf(patterns[j])) > -1)
                        {
                            string returnValue = recvData[i].Substring(idx);
                            int index = i + 1;
                            while (!recvData[index].Contains(deviceValue))
                            {
                                returnValue += recvData[index++];
                            }

                            Utility.Utility.LogWrite("상세주소 제외전 : " + returnValue);

                            string removeKeyword = "[상세주소] ";
                            if (returnValue.Contains(removeKeyword))
                            {
                                returnValue = returnValue.Substring(returnValue.IndexOf(removeKeyword) + removeKeyword.Length);
                            }

                            Utility.Utility.LogWrite("patterns주소 리턴 값 : " + returnValue);

                            returnValue = RemoveParenthesisInAddress(returnValue);
                            Utility.Utility.LogWrite("주소 괄호 삭제 후 리턴 값 : " + returnValue);

                            return returnValue;
                        }
                    }
                }

                for (int i = 0; i < recvData.Length; i++)
                {
                    for (int j = 0; j < startAddr.Length; j++)
                    {
                        int idx = 0;
                        if ((idx = recvData[i].IndexOf(startAddr[j])) > -1)
                        {
                            string returnValue = recvData[i].Substring(idx);
                            int index = i + 1;
                            while (index < recvData.Length && !recvData[index].Contains(deviceValue))
                            {
                                returnValue += recvData[index++];
                            }

                            Utility.Utility.LogWrite("상세주소 제외전 : " + returnValue);

                            string removeKeyword = "[상세주소] ";
                            if (returnValue.Contains(removeKeyword))
                            {
                                returnValue = returnValue.Substring(returnValue.IndexOf(removeKeyword) + removeKeyword.Length);
                            }

                            Utility.Utility.LogWrite("startAddr주소 리턴 값 : " + returnValue);

                            returnValue = RemoveParenthesisInAddress(returnValue);
                            Utility.Utility.LogWrite("주소 괄호 삭제 후 리턴 값 : " + returnValue);

                            return returnValue;
                        }
                    }
                }
            }
            return "못찾음";
        }

        // 요청메모
        private String getParsingValue_customer_request_memo(String _recv_data)
        {
            ModelAppDevice device = new ModelAppDevice();
            string temp = device.readConfigString("EXCLUSIVEKEYWORD", "exclusive_keyword");
            string[] exclusiveKeyword = temp.Split('+');
            for (int i = 0; i < exclusiveKeyword.Length; i++)
            {
                Utility.Utility.LogWrite("exclusiveKeyword 키워드: " + exclusiveKeyword[i]);
                _recv_data = _recv_data.Replace(exclusiveKeyword[i], "");
            }

            string[] recvData = _recv_data.Split(new string[] { "\n" }, StringSplitOptions.None);
            int recognize = 0;
            string deviceValue = "";
            string deviceRequestExclusiveKey = "";

            string val = device.readConfigString("TEMP_PORTNAME", "temp_portName");

            for (int i = 0; i < 4; i++)
            {
                if (val.Equals(device.readConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString())))
                {
                    recognize = Int32.Parse(device.readConfigString("CONFIG_ORDER_INPUT", "input_type_" + i.ToString()));
                }
            }
            Utility.Utility.LogWrite("recognize : " + recognize);

            switch (recognize)
            {
                case 10:
                    deviceValue = device.readConfigString("BEMINREQUESTLINE", "bemin_request_line");
                    break;
                case 21:
                    deviceValue = device.readConfigString("OKPOSREQUESTLINE", "okpos_request_line");
                    break;
                case 26:
                    deviceValue = device.readConfigString("DELGENREQUESTLINE", "delgen_request_line");
                    break;
                case 27:
                    deviceValue = device.readConfigString("POSFEEDREQUESTLINE", "posfeed_request_line");
                    break;
                case 31:
                    deviceValue = device.readConfigString("EASYPOSREQUESTLINE", "easypos_request_line");
                    break;
                case 32:
                    deviceValue = device.readConfigString("YOGIYOREQUESTLINE", "yogiyo_request_line");
                    break;
                case 33:
                    deviceValue = device.readConfigString("BEDALTONGREQUESTLINE", "bedaltong_request_line");
                    break;
                case 98:
                    deviceValue = device.readConfigString("ETCREQUESTLINE", "etc_request_line");
                    break;
            }

            const string REQUESTEXCLUSIVEKEY = "REQUESTEXCLUSIVEKEY";
            switch (recognize)
            {
                case 10:
                    deviceRequestExclusiveKey = device.readConfigString(REQUESTEXCLUSIVEKEY, "bemin_request_exclusive_key");
                    break;
                case 21:
                    deviceRequestExclusiveKey = device.readConfigString(REQUESTEXCLUSIVEKEY, "okpos_request_exclusive_key");
                    break;
                case 26:
                    deviceRequestExclusiveKey = device.readConfigString(REQUESTEXCLUSIVEKEY, "delgen_request_exclusive_key");
                    break;
                case 27:
                    deviceRequestExclusiveKey = device.readConfigString(REQUESTEXCLUSIVEKEY, "posfeed_request_exclusive_key");
                    break;
                case 31:
                    deviceRequestExclusiveKey = device.readConfigString(REQUESTEXCLUSIVEKEY, "easypos_request_exclusive_key");
                    break;
                case 32:
                    deviceRequestExclusiveKey = device.readConfigString(REQUESTEXCLUSIVEKEY, "yogiyo_request_exclusive_key");
                    break;
                case 33:
                    deviceRequestExclusiveKey = device.readConfigString(REQUESTEXCLUSIVEKEY, "bedaltong_request_exclusive_key");
                    break;
                case 98:
                    deviceRequestExclusiveKey = device.readConfigString(REQUESTEXCLUSIVEKEY, "etc_request_exclusive_key");
                    break;
            }
            string[] deviceRequestExclusiveKeys = deviceRequestExclusiveKey.Split('+');

            for (int i = 0; i < recvData.Length; i++)
            {
                Utility.Utility.LogWrite("recvData[i]; : " + recvData[i]);
            }
            string[] patterns = deviceValue.Split(',');

            if (patterns.Length == 3)
            {
                string value        = "";
                string keyword      = patterns[0];
                string direction    = patterns[1];
                int lineCount       = Int32.Parse(patterns[2]);

                Utility.Utility.LogWrite("keyword : " + keyword + " direction : " + direction + " lineCount : " + lineCount);
                
                for (int i = 0; i < recvData.Length; i++)
                {
                    if (recvData[i].Contains(keyword))
                    {
                        if (direction == "위")
                        {
                            int idx = i - lineCount;
                            if (idx < recvData.Length && idx > 0)
                            {
                                value = recvData[idx];
                            }
                            if (idx + 1 < recvData.Length && idx > 0)
                            {
                                value += recvData[idx + 1];
                            }
                            Utility.Utility.LogWrite("위 : " + value);

                            for (int j = 0; j < deviceRequestExclusiveKeys.Length; j++)
                            {
                                if (value.Contains(deviceRequestExclusiveKeys[j]))
                                {
                                    value = "요청없음";
                                }
                            }
                            
                            return value;
                        }
                        else
                        {
                            int idx = i + lineCount;
                            if (idx < recvData.Length && idx > 0)
                            {
                                value = recvData[idx];
                            }
                            if (idx + 1 < recvData.Length && idx > 0)
                            {
                                value += recvData[idx + 1];
                            }
                            Utility.Utility.LogWrite("밑 : " + value);

                            for (int j = 0; j < deviceRequestExclusiveKeys.Length; j++)
                            {
                                if (value.Contains(deviceRequestExclusiveKeys[j]))
                                {
                                    value = "요청없음";
                                }
                            }

                            return value;
                        }
                    }
                }
            }
            Utility.Utility.LogWrite("요청없음");
            return "요청없음";
        }
        String DetailAddress(string _value)
        {
            string[] val = _value.Split(' ');
            string returnValue = "";

            if (val.Length >= 3)
            {
                returnValue += val[val.Length - 3];
                returnValue += " ";
                returnValue += val[val.Length - 2];
                returnValue += " ";
                returnValue += val[val.Length - 1];
            }
            Utility.Utility.LogWrite("detail 주소 : " + returnValue);
            return returnValue;
        }

        // 결제수단
        private Int32 getParsingValue_customer_pay_type_cd(String _recv_data)
        {
            Utility.Utility.LogWrite("getParsingValue_customer_pay_type_cd");

            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                string temp = device.readConfigString("EXCLUSIVEKEYWORD", "exclusive_keyword");
                string tmp = device.readConfigString("PAYMENT_OPTION", "payment_option");
                Utility.Utility.LogWrite("tmp : " + tmp);
                string[] patterns = tmp.Split('+');
                string[] exclusiveKeyword = temp.Split('+');

                Utility.Utility.LogWrite("patterns.Length : " + patterns.Length);
                for (int i = 0; i < patterns.Length; i++)
                {
                    Utility.Utility.LogWrite("patterns[i] : " + patterns[i]);
                }

                for (int i = 0; i < exclusiveKeyword.Length; i++)
                {
                    _recv_data = _recv_data.Replace(exclusiveKeyword[i], "");
                }

                if (_recv_data.Contains("현금"))
                {
                    return 0;
                }
                else if (_recv_data.Contains("카드"))
                {
                    return 1;
                }

                for (int i = 0; i < patterns.Length; i++)
                {
                    if(_recv_data.Contains(patterns[i]))
                    {
                        return 4;
                    }
                }
            }
            return 77;
        }

        // 결제금액
        private Int32 getParsingValue_customer_cost(String _recv_data)
        {
            Utility.Utility.LogWrite("getParsingValue_customer_cost");

            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                string temp = device.readConfigString("EXCLUSIVEKEYWORD", "exclusive_keyword");
                string tmp = device.readConfigString("COST", "cost");
                Utility.Utility.LogWrite("tmp : " + tmp);
                string[] patterns = tmp.Split('+');
                string[] exclusiveKeyword = temp.Split('+');
                List<int> matchValueList = new List<int>();

                Utility.Utility.LogWrite("patterns.Length : " + patterns.Length);
                for (int i = 0; i < patterns.Length; i++)
                {
                    Utility.Utility.LogWrite("patterns[i] : " + patterns[i]);
                }
                for (int i = 0; i < exclusiveKeyword.Length; i++)
                {
                    _recv_data = _recv_data.Replace(exclusiveKeyword[i], "");
                }
                for (int i = 0; i < patterns.Length; i++)
                {
                    string normalization = null;
                    for (int j = 0; j < patterns[i].Length; j++)
                    {
                        if (patterns[i].ElementAt(j) == 'n')
                        {
                            normalization += "[0-9]{1}";
                        }
                        else
                        {
                            normalization += patterns[i].ElementAt(j);
                        }
                    }
                    Utility.Utility.LogWrite("normalization : " + normalization);

                    normalization = normalization.Replace(")", "-");

                    Regex rgx = new Regex(normalization);

                    foreach (Match match in rgx.Matches(_recv_data))
                    {
                        if (match.Success)
                        {
                            Utility.Utility.LogWrite("match.Value : " + match.Value);
                            
                            if (_recv_data.IndexOf(match.Value) + match.Value.Length + 1 < _recv_data.Length)
                            {
                                if (_recv_data[_recv_data.IndexOf(match.Value) + match.Value.Length] == '점' || _recv_data[_recv_data.IndexOf(match.Value) + match.Value.Length + 1] == '점')
                                {
                                    continue;
                                }
                            }
                            matchValueList.Add(Int32.Parse(match.Value.Replace(",", "")));
                        }
                    }
                    Utility.Utility.LogWrite("문제없음 4");
                    if (matchValueList.Count > 0)
                    {
                        for (int j = 0; j < matchValueList.Count - 1; j++)
                        {
                            if (matchValueList[j] > matchValueList[j + 1])
                            {
                                int t = 0;
                                t = matchValueList[j];
                                matchValueList[j] = matchValueList[j + 1];
                                matchValueList[j + 1] = t;
                            }
                        }
                        for (int j = 0; j < matchValueList.Count; j++)
                        {
                            Utility.Utility.LogWrite("matchValueList[i] : " + matchValueList[j]);
                        }
                        Utility.Utility.LogWrite("matchValueList.count : " + matchValueList.Count);
                    }
                }
                string value = null;
                if (matchValueList.Count > 0)
                {
                    value = matchValueList[matchValueList.Count - 1].ToString();
                }

                Utility.Utility.LogWrite("value : " + value);
                return KnUtil.formatInt32FromMoneyFormat(value);
            }
            return 0;
        }

        // 기타메모
        private String getParsingValue_arv_locate_memo(String[] _recv_data, ref int _line_offset)
        {
            ModelAppDevice device = new ModelAppDevice();

            for (int i = _line_offset; i < _recv_data.Length; i++)
            {
                // 대상라인 저장
                String line_str = _recv_data[i];

                // 데이터 뽑아서 파싱
                int idx_e = line_str.Length; // 끝 길이
                if (-1 < idx_e)
                {
                    _line_offset = i + 1; // 다음 찾기를 위해 오프셋 + 1

                    String fs = line_str.Substring(idx_e - 20, 20).Trim();

                    return getAttacthText_OrderNumHead() + fs;
                }
            }
            return "";
        }

        public byte[] ConvertByteArray(string strHex)
        {
            int iLength = strHex.Length;
            byte[] bytes = new byte[iLength / 2];
            for (int i = 0; i < iLength; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(strHex.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static int CheckSender()
        {
            ModelAppDevice device = new ModelAppDevice();

            string val = device.readConfigString("TEMP_PORTNAME", "temp_portName");
            int num = 0;

            for (int i = 0; i < 4; i++)
            {
                if (val.Equals(device.readConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString())))
                {
                    num = Int32.Parse(device.readConfigString("CONFIG_ORDER_INPUT", "input_type_" + i.ToString()));
                }
            }
            return num;
        }

        public static string EncodingToStringFitness(int _sender, byte[] _target)
        {
            ModelAppDevice device = new ModelAppDevice();
            string characterSet = "";
            string value = "";

            switch (_sender)
            {
                case 10:
                    characterSet = device.readConfigString("BEMINENCODING", "bemin_encoding");
                    break;
                case 21:
                    characterSet = device.readConfigString("OKPOSENCODING", "okpos_encoding");
                    break;
                case 26:
                    characterSet = device.readConfigString("DELGENENCODING", "delgen_encoding");
                    break;
                case 27:
                    characterSet = device.readConfigString("POSFEEDENCODING", "posfeed_encoding");
                    break;
                case 31:
                    characterSet = device.readConfigString("EASYPOSENCODING", "easypos_encoding");
                    break;
                case 32:
                    characterSet = device.readConfigString("YOGIYOENCODING", "yogiyo_encoding");
                    break;
                case 33:
                    characterSet = device.readConfigString("BEDALTONGENCODING", "bedaltong_encoding");
                    break;
                case 98:
                    characterSet = device.readConfigString("ETCENCODING", "etc_encoding");
                    break;
            }

            switch (characterSet)
            {
                case "EUC-KR":
                    value = Encoding.Default.GetString(_target);
                    break;
                case "UTF-8":
                    value = Encoding.UTF8.GetString(_target);
                    break;
            }
            return value;
        }

        public bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }

        public int FindBytes(byte[] src, byte[] find)
        {
            int index = -1;
            int matchIndex = 0;
            // handle the complete source array
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else if (src[i] == find[0])
                {
                    matchIndex = 1;
                }
                else
                {
                    matchIndex = 0;
                }
            }
            return index;
        }

        public byte[] ReplaceBytes(byte[] src, byte[] search, byte[] repl)
        {
            byte[] dst = null;
            int index = FindBytes(src, search);
            if (index >= 0)
            {
                dst = new byte[src.Length - search.Length + repl.Length];
                // before found array
                Buffer.BlockCopy(src, 0, dst, 0, index);
                // repl copy
                Buffer.BlockCopy(repl, 0, dst, index, repl.Length);
                // rest of src array
                Buffer.BlockCopy(
                    src,
                    index + search.Length,
                    dst,
                    index + repl.Length,
                    src.Length - (index + search.Length));
            }
            return dst;
        }

        public string RemoveParenthesisInAddress(string _address)
        {
            Utility.Utility.LogWrite("RemoveParenthesisInAddress");

            int startIndex, endIndex = 0;

            if ((startIndex = _address.IndexOf('(')) > -1)
            {
                if ((endIndex = _address.IndexOf(')')) > -1)
                {
                    if (startIndex < endIndex)
                    {
                        _address = _address.Remove(startIndex, endIndex - startIndex + 1);
                    }
                }
            }
            return _address;
        }
    }
}