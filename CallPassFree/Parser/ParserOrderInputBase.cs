using Kons.ShopCallpass.Model;
using Kons.ShopCallpass.Object;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Kons.ShopCallpass.Parser
{
    public class ParserOrderInputBase
    {
        public enum PARSER_TYPE
        {
            BAEMIN = 10,
            OKPOS = 21,
            DELGEN = 26,
            POSFEED = 27,
            EASYPOS = 31,
            YOGIYO = 32,
            BEDALTONG = 33,
            ETC = 98,
            UNKNOWN = 99,
            TYPE_END
        }

        static public PARSER_TYPE[] PARSER_TYPE_LIST = {
              
              ParserOrderInputBase.PARSER_TYPE.BAEMIN
            , ParserOrderInputBase.PARSER_TYPE.OKPOS
            , ParserOrderInputBase.PARSER_TYPE.DELGEN
            , ParserOrderInputBase.PARSER_TYPE.POSFEED
            , ParserOrderInputBase.PARSER_TYPE.EASYPOS
            , ParserOrderInputBase.PARSER_TYPE.YOGIYO
            , ParserOrderInputBase.PARSER_TYPE.BEDALTONG
            , ParserOrderInputBase.PARSER_TYPE.ETC
            , ParserOrderInputBase.PARSER_TYPE.UNKNOWN};

        static public String obtainParserName(PARSER_TYPE _parser_type)
        {
            switch (_parser_type)
            {
                case PARSER_TYPE.BAEMIN:
                    return "배달의민족";
                case PARSER_TYPE.OKPOS:
                    return "OKPOS(오케이포스)";
                case PARSER_TYPE.DELGEN:
                    return "배달천재";
                case PARSER_TYPE.POSFEED:
                    return "포스피드";
                case PARSER_TYPE.EASYPOS:
                    return "이지포스";
                case PARSER_TYPE.YOGIYO:
                    return "요기요";
                case PARSER_TYPE.BEDALTONG:
                    return "배달통";
                case PARSER_TYPE.ETC:
                    return "기타";
                case PARSER_TYPE.UNKNOWN:
                    return "[출력만사용]";
            }
            return "-";
        }

        static public PARSER_TYPE obtainParserType(int _parser_type)
        {
            for (int i = 0; i < ParserOrderInputBase.PARSER_TYPE_LIST.Length; i++)
            {
                ParserOrderInputBase.PARSER_TYPE sel_type = ParserOrderInputBase.PARSER_TYPE_LIST[i];
                if ((int)sel_type == _parser_type)
                {
                    return sel_type;
                }
            }
            return ParserOrderInputBase.PARSER_TYPE.UNKNOWN;
        }

        // --------------------------------------
        //
        private PARSER_TYPE m_parser_type = PARSER_TYPE.UNKNOWN;
        private String m_last_error_message = "";

        protected void setParserType(PARSER_TYPE _parser_type)
        {
            ModelAppDevice device = new ModelAppDevice();

            if (device != null)
            {
                string temp = device.readConfigString("TEMP_PORTNAME", "temp_portname");
                Debug.WriteLine("들어온 포트 temp : " + temp);

                for (int i = 0; i < 4; i++)
                {
                    if (temp == device.readConfigString("CONFIG_ORDER_INPUT", "listen_port_num_" + i.ToString()))
                    {
                        Debug.WriteLine("읽어온 포트 temp : " + device.readConfigString("CONFIG_ORDER_INPUT", "input_type_" + i.ToString()));
                        m_parser_type = (PARSER_TYPE)Int32.Parse(device.readConfigString("CONFIG_ORDER_INPUT", "input_type_" + i.ToString()));
                    }
                }
            }

            //m_parser_type = _parser_type;
        }

        public PARSER_TYPE getParserType()
        {
            return m_parser_type;
        }

        public void setLastErrorMessage(String _error_msg)
        {
            m_last_error_message = _error_msg;
        }

        public String getLastErrorMessage()
        {
            return m_last_error_message;
        }

        // -------------------------------------- virtual
        //
        /// <summary>
        /// 시작문자 : 보통 출력시작을 알린다
        /// </summary>
        /// <returns></returns>
        virtual public byte[] getInputData_SOD()
        {
            return null;
        }

        /// <summary>
        /// 종료문자 : 보통 용지 컷팅을 알린다. null 인경우는 다음 시작문자가 올때까지 찾는다.
        /// </summary>
        /// <returns></returns>
        virtual public byte[] getInputData_EOD()
        {
            return null;
        }

        /// <summary>
        /// 
        /// 파싱은 아래의 과정을 거치며 기본적으로 라인에서 해당 데이터를 뽑아내는 방식을 
        /// 사용하기 때문에 위에서 아래로 순차적으로 데이터를 뽑는다.
        /// 
        /// 1. 줄구분 문자로 줄 단위로 문자라인 배열을 만든다
        /// 2. 필요한 데이터가 들어 있는 라인이 나올때까지 라인을 증가시킴
        /// 3. 데이터가 들어 있는 라인을 찾았으면 데이터를 뽑아서 알맞는 형태로 변화 시킨다.
        /// 위에서 부터 순차적으로 찾기 때문에 다시 윗 라인으로 가서 데이터를 뽑지는 않는 점 유의할 것
        /// 
        /// </summary>
        /// <param name="_raw_data_buf"></param>
        /// <param name="_raw_data_len"></param>
        /// <returns></returns>
        virtual public ObjOrder parsingInputOrderRawData(byte[] _raw_data_buf, int _raw_data_len)
        {
            return null;
        }

        // 주문번호 앞에 여러 연동을 구분하기 위해 콜패스만의 고유값 붙일때 사용
        virtual public String getAttacthText_OrderNumHead()
        {
            return "";
        }
    }
}
