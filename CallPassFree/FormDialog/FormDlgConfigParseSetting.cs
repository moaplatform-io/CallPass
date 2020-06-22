using Kons.ShopCallpass.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kons.ShopCallpass.FormDialog
{
    
    public partial class FormDlgConfigParseSetting : FormDlgBase
    {
        string[] CONFIG_SECTION = { "UNWANTEDKEYWORD", "ORDER_NUM", "COST", "CALL_NUM", "ADDRESS",
                                            "PAYMENT_OPTION", "ORDER_DATE", "EXCLUSIVEKEYWORD", "ENDCHARACTER", "INTERVALREMOVEBINARYKEY"};

        string[] keyValues = { "unwanted_keyword", "order_num", "cost", "call_num", "address",
                                "payment_option", "order_date", "exclusive_keyword", "end_character", "interval_remove_binary_key"};

        string[] CONFIG_SECTION2 = { "BEMINENDCHAR", "YOGIYOENDCHAR", "EASYPOSENDCHAR", "DELGENENDCHAR", "OKPOSENDCHAR", "POSFEEDENDCHAR", "BEDALTONGENDCHAR", "ETCENDCHAR"};

        string[] keyValues2 = { "bemin_end_char", "yogiyo_end_char", "easypos_end_char", "delgen_end_char", "okpos_end_char", "posfeed_end_char", "bedaltong_end_char", "etc_end_char"};

        string[] CONFIG_SECTION3 = { "BEMINREQUESTLINE", "YOGIYOREQUESTLINE", "EASYPOSREQUESTLINE", "DELGENREQUESTLINE", "OKPOSREQUESTLINE", "POSFEEDREQUESTLINE", "BEDALTONGREQUESTLINE", "ETCREQUESTLINE" };

        string[] keyValues3 = { "bemin_request_line", "yogiyo_request_line", "easypos_request_line", "delgen_request_line", "okpos_request_line", "posfeed_request_line", "bedaltong_request_line", "etc_request_line"};

        string[] CONFIG_SECTION4 = { "BEMINENCODING", "YOGIYOENCODING", "EASYPOSENCODING", "DELGENENCODING", "OKPOSENCODING", "POSFEEDENCODING", "BEDALTONGENCODING", "ETCENCODING"};

        string[] keyValues4 = { "bemin_encoding", "yogiyo_encoding", "easypos_encoding", "delgen_encoding", "okpos_encoding", "posfeed_encoding", "bedaltong_encoding", "etc_encoding"};

        string[] keyValues5 = { "bemin_addr_end_char", "yogiyo_addr_end_char", "easypos_addr_end_char", "delgen_addr_end_char", "okpos_addr_end_char", "posfeed_addr_end_char", "bedaltong_addr_end_char", "etc_addr_end_char" };

        string[] keyValues6 = { "bemin_request_exclusive_key", "yogiyo_request_exclusive_key", "easypos_request_exclusive_key", "delgen_request_exclusive_key", "okpos_request_exclusive_key", "posfeed_request_exclusive_key", "bedaltong_request_exclusive_key", "etc_request_exclusive_key" };
        public FormDlgConfigParseSetting()
        {

        }

        public FormDlgConfigParseSetting(Form _parnet = null)
        {
            setBaseFormData(null, null, DLG_TYPE.TYPE_NORMAL);
            InitializeComponent();
            comboBox1.SelectedItem = comboBox1.Items[0];
            comboBox2.SelectedItem = comboBox2.Items[0];
            comboBox3.SelectedItem = comboBox3.Items[0];
            comboBox4.SelectedItem = comboBox4.Items[0];
            comboBox5.SelectedItem = comboBox5.Items[0];
            comboBox6.SelectedItem = comboBox6.Items[0];

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string tmp = device.readConfigString("UNWANTEDKEYWORD", "unwanted_keyword");
                    Debug.WriteLine("tmp : " + tmp);
                    string[] words = tmp.Split('+');

                    for (int i = 0; i < words.Length; i++)
                    {
                        Debug.WriteLine("words.Length : " + words.Length);
                        Debug.WriteLine("words[i] : " + words[i]);
                        listBox1.Items.Add(words[i]);
                    }

                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string tmp = device.readConfigString("EXCLUSIVEKEYWORD", "exclusive_keyword");
                    Debug.WriteLine("tmp : " + tmp);
                    string[] words = tmp.Split('+');

                    for (int i = 0; i < words.Length; i++)
                    {
                        Debug.WriteLine("words.Length : " + words.Length);
                        Debug.WriteLine("words[i] : " + words[i]);
                        listBox3.Items.Add(words[i]);
                    }

                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string tmp = device.readConfigString("INTERVALREMOVEBINARYKEY", "interval_remove_binary_key");
                    Debug.WriteLine("tmp : " + tmp);
                    string[] words = tmp.Split('+');

                    for (int i = 0; i < words.Length; i++)
                    {
                        Debug.WriteLine("words.Length : " + words.Length);
                        Debug.WriteLine("words[i] : " + words[i]);
                        listBox6.Items.Add(words[i]);
                    }

                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }

        public void saveDlgObjectData()
        {
            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string buf = null;

                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        buf += listBox1.Items[i].ToString();
                        if (i < listBox1.Items.Count - 1)
                        {
                            buf += "+";
                        }
                    }
                    if (listBox1.Items.Count == 0)
                    {
                        device.writeConfigString(CONFIG_SECTION[0], keyValues[0], "#");
                    }
                    else
                    {
                        device.writeConfigString(CONFIG_SECTION[0], keyValues[0], buf);
                    }
                    buf = null;

                    int index = 1;
                    switch (comboBox1.SelectedItem.ToString())
                    {
                        case "주문번호":
                            index = 1;
                            break;
                        case "가격":
                            index = 2;
                            break;
                        case "전화번호":
                            index = 3;
                            break;
                        case "주소":
                            index = 4;
                            break;
                        case "선불판단키워드":
                            index = 5;
                            break;
                        case "주문일시":
                            index = 6;
                            break;
                    }

                    for (int i = 0; i < listBox2.Items.Count; i++)
                    {
                         buf += listBox2.Items[i].ToString();
                         if (i < listBox2.Items.Count - 1)
                         {
                             buf += "+";
                         }
                    }
                    if (listBox2.Items.Count == 0)
                    {
                        device.writeConfigString(CONFIG_SECTION[index], keyValues[index], "#");
                    }
                    else
                    {
                        device.writeConfigString(CONFIG_SECTION[index], keyValues[index], buf);
                    }
                    buf = null;

                    for (int i = 0; i < listBox3.Items.Count; i++)
                    {
                        buf += listBox3.Items[i].ToString();
                        if (i < listBox3.Items.Count - 1)
                        {
                            buf += "+";
                        }
                    }
                    if (listBox3.Items.Count == 0)
                    {
                        device.writeConfigString(CONFIG_SECTION[7], keyValues[7], "#");
                    }
                    else
                    {
                        device.writeConfigString(CONFIG_SECTION[7], keyValues[7], buf);
                    }
                    buf = null;

                    index = 1;
                    switch (comboBox2.SelectedItem.ToString())
                    {
                        case "배달의민족":
                            index = 0;
                            break;
                        case "요기요":
                            index = 1;
                            break;
                        case "이지포스":
                            index = 2;
                            break;
                        case "배달천재":
                            index = 3;
                            break;
                        case "오케이포스":
                            index = 4;
                            break;
                        case "포스피드":
                            index = 5;
                            break;
                        case "배달통":
                            index = 6;
                            break;
                        case "기타":
                            index = 7;
                            break;
                    }

                    for (int i = 0; i < listBox4.Items.Count; i++)
                    {
                        buf += listBox4.Items[i].ToString();
                        if (i < listBox4.Items.Count - 1)
                        {
                            buf += "+";
                        }
                    }
                    if (listBox4.Items.Count == 0)
                    {
                        device.writeConfigString(CONFIG_SECTION2[index], keyValues2[index], "#");
                    }
                    else
                    {
                        device.writeConfigString(CONFIG_SECTION2[index], keyValues2[index], buf);
                    }
                    buf = null;

                    index = 1;
                    switch (comboBox4.SelectedItem.ToString())
                    {
                        case "배달의민족":
                            index = 0;
                            break;
                        case "요기요":
                            index = 1;
                            break;
                        case "이지포스":
                            index = 2;
                            break;
                        case "배달천재":
                            index = 3;
                            break;
                        case "오케이포스":
                            index = 4;
                            break;
                        case "포스피드":
                            index = 5;
                            break;
                        case "배달통":
                            index = 6;
                            break;
                        case "기타":
                            index = 7;
                            break;
                    }

                    for (int i = 0; i < listBox5.Items.Count; i++)
                    {
                        buf += listBox5.Items[i].ToString();
                        if (i < listBox5.Items.Count - 1)
                        {
                            buf += "+";
                        }
                    }
                    if (listBox5.Items.Count == 0)
                    {
                        device.writeConfigString(CONFIG_SECTION4[index], keyValues4[index], "#");
                    }
                    else
                    {
                        device.writeConfigString(CONFIG_SECTION4[index], keyValues4[index], buf);
                    }
                    buf = null;

                    for (int i = 0; i < listBox6.Items.Count; i++)
                    {
                        buf += listBox6.Items[i].ToString();
                        if (i < listBox6.Items.Count - 1)
                        {
                            buf += "+";
                        }
                    }
                    if (listBox6.Items.Count == 0)
                    {
                        device.writeConfigString(CONFIG_SECTION[9], keyValues[9], "#");
                    }
                    else
                    {
                        device.writeConfigString(CONFIG_SECTION[9], keyValues[9], buf);

                    }
                    buf = null;
                    index = 1;
                    switch (comboBox3.SelectedItem.ToString())
                    {
                        case "배달의민족":
                            index = 0;
                            break;
                        case "요기요":
                            index = 1;
                            break;
                        case "이지포스":
                            index = 2;
                            break;
                        case "배달천재":
                            index = 3;
                            break;
                        case "오케이포스":
                            index = 4;
                            break;
                        case "포스피드":
                            index = 5;
                            break;
                        case "배달통":
                            index = 6;
                            break;
                        case "기타":
                            index = 7;
                            break;
                    }
                    for (int i = 0; i < listBox7.Items.Count; i++)
                    {
                        buf += listBox7.Items[i].ToString();
                        if (i < listBox7.Items.Count - 1)
                        {
                            buf += "+";
                        }
                    }

                    if (listBox7.Items.Count == 0)
                    {
                        device.writeConfigString(CONFIG_SECTION3[index], keyValues3[index], "#");
                    }
                    else
                    {
                        device.writeConfigString(CONFIG_SECTION3[index], keyValues3[index], buf);
                    }
                    buf = null;

                    index = 1;
                    switch (comboBox5.SelectedItem.ToString())
                    {
                        case "배달의민족":
                            index = 0;
                            break;
                        case "요기요":
                            index = 1;
                            break;
                        case "이지포스":
                            index = 2;
                            break;
                        case "배달천재":
                            index = 3;
                            break;
                        case "오케이포스":
                            index = 4;
                            break;
                        case "포스피드":
                            index = 5;
                            break;
                        case "배달통":
                            index = 6;
                            break;
                        case "기타":
                            index = 7;
                            break;
                    }
                    for (int i = 0; i < listBox8.Items.Count; i++)
                    {
                        buf += listBox8.Items[i].ToString();
                        if (i < listBox8.Items.Count - 1)
                        {
                            buf += "+";
                        }
                    }
                    const string ADDRESSENDCHAR = "ADDRESSENDCHAR";

                    if (listBox8.Items.Count == 0)
                    {
                        device.writeConfigString(ADDRESSENDCHAR, keyValues5[index], "#");
                    }
                    else
                    {
                        device.writeConfigString(ADDRESSENDCHAR, keyValues5[index], buf);
                    }
                    buf = null;

                    index = 1;
                    switch (comboBox6.SelectedItem.ToString())
                    {
                        case "배달의민족":
                            index = 0;
                            break;
                        case "요기요":
                            index = 1;
                            break;
                        case "이지포스":
                            index = 2;
                            break;
                        case "배달천재":
                            index = 3;
                            break;
                        case "오케이포스":
                            index = 4;
                            break;
                        case "포스피드":
                            index = 5;
                            break;
                        case "배달통":
                            index = 6;
                            break;
                        case "기타":
                            index = 7;
                            break;
                    }
                    for (int i = 0; i < listBox9.Items.Count; i++)
                    {
                        buf += listBox9.Items[i].ToString();
                        if (i < listBox9.Items.Count - 1)
                        {
                            buf += "+";
                        }
                    }
                    const string REQUESTEXCLUSIVEKEY = "REQUESTEXCLUSIVEKEY";

                    if (listBox9.Items.Count == 0)
                    {
                        device.writeConfigString(REQUESTEXCLUSIVEKEY, keyValues6[index], "#");
                    }
                    else
                    {
                        device.writeConfigString(REQUESTEXCLUSIVEKEY, keyValues6[index], buf);
                    }
                    buf = null;
                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        void SelectSubjectButton(object sender, EventArgs e)
        {
            listBox2.Items.Clear();

            string sector = null;
            string key = null;

            switch (comboBox1.Text)
            {
                case "주문번호":
                    sector = "ORDER_NUM";
                    key = "order_num";
                    break;
                case "가격":
                    sector = "COST";
                    key = "cost";
                    break;
                case "전화번호":
                    sector = "CALL_NUM";
                    key = "call_num";
                    break;
                case "주소":
                    sector = "ADDRESS";
                    key = "address";
                    break;
                case "선불판단키워드":
                    sector = "PAYMENT_OPTION";
                    key = "payment_option";
                    break;
                case "주문일시":
                    sector = "ORDER_DATE";
                    key = "order_date";
                    break;
            }

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string tmp = device.readConfigString(sector, key);
                    Debug.WriteLine("tmp : " + tmp);
                    string[] words = tmp.Split('+');

                    for (int i = 0; i < words.Length; i++)
                    {
                        Debug.WriteLine("words.Length : " + words.Length);
                        Debug.WriteLine("words[i] : " + words[i]);
                        listBox2.Items.Add(words[i]);
                    }

                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }
        void EncodingSelectSubjectButton(object sender, EventArgs e)
        {
            listBox5.Items.Clear();

            string sector = null;
            string key = null;

            switch (comboBox4.Text)
            {
                case "배달의민족":
                    sector = "BEMINENCODING";
                    key = "bemin_encoding";
                    break;
                case "요기요":
                    sector = "YOGIYOENCODING";
                    key = "yogiyo_encoding";
                    break;
                case "이지포스":
                    sector = "EASYPOSENCODING";
                    key = "easypos_encoding";
                    break;
                case "배달천재":
                    sector = "DELGENENCODING";
                    key = "delgen_encoding";
                    break;
                case "오케이포스":
                    sector = "OKPOSENCODING";
                    key = "okpos_encoding";
                    break;
                case "포스피드":
                    sector = "POSFEEDENCODING";
                    key = "posfeed_encoding";
                    break;
                case "배달통":
                    sector = "BEDALTONGENCODING";
                    key = "bedaltong_encoding";
                    break;
                case "기타":
                    sector = "ETCENCODING";
                    key = "etc_encoding";
                    break;
            }

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string tmp = device.readConfigString(sector, key);
                    Debug.WriteLine("tmp : " + tmp);
                    string[] words = tmp.Split('+');

                    for (int i = 0; i < words.Length; i++)
                    {
                        Debug.WriteLine("words.Length : " + words.Length);
                        Debug.WriteLine("words[i] : " + words[i]);
                        listBox5.Items.Add(words[i]);
                    }

                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }

        void EndCharSelectSubjectButton(object sender, EventArgs e)
        {
            listBox4.Items.Clear();

            string sector = null;
            string key = null;

            switch (comboBox2.Text)
            {
                case "배달의민족":
                    sector = "BEMINENDCHAR";
                    key = "bemin_end_char";
                    break;
                case "요기요":
                    sector = "YOGIYOENDCHAR";
                    key = "yogiyo_end_char";
                    break;
                case "이지포스":
                    sector = "EASYPOSENDCHAR";
                    key = "easypos_end_char";
                    break;
                case "배달천재":
                    sector = "DELGENENDCHAR";
                    key = "delgen_end_char";
                    break;
                case "오케이포스":
                    sector = "OKPOSENDCHAR";
                    key = "okpos_end_char";
                    break;
                case "포스피드":
                    sector = "POSFEEDENDCHAR";
                    key = "posfeed_end_char";
                    break;
                case "배달통":
                    sector = "BEDALTONGENDCHAR";
                    key = "bedaltong_end_char";
                    break;
                case "기타":
                    sector = "ETCENDCHAR";
                    key = "etc_end_char";
                    break;
            }

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string tmp = device.readConfigString(sector, key);
                    Debug.WriteLine("tmp : " + tmp);
                    string[] words = tmp.Split('+');

                    for (int i = 0; i < words.Length; i++)
                    {
                        Debug.WriteLine("words.Length : " + words.Length);
                        Debug.WriteLine("words[i] : " + words[i]);
                        listBox4.Items.Add(words[i]);
                    }

                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }

        void RequestLineSelectSubjectButton(object sender, EventArgs e)
        {
            listBox7.Items.Clear();

            string sector = null;
            string key = null;

            switch (comboBox3.Text)
            {
                case "배달의민족":
                    sector = "BEMINREQUESTLINE";
                    key = "bemin_request_line";
                    break;
                case "요기요":
                    sector = "YOGIYOREQUESTLINE";
                    key = "yogiyo_request_line";
                    break;
                case "이지포스":
                    sector = "EASYPOSREQUESTLINE";
                    key = "easypos_request_line";
                    break;
                case "배달천재":
                    sector = "DELGENREQUESTLINE";
                    key = "delgen_request_line";
                    break;
                case "오케이포스":
                    sector = "OKPOSREQUESTLINE";
                    key = "okpos_request_line";
                    break;
                case "포스피드":
                    sector = "POSFEEDREQUESTLINE";
                    key = "posfeed_request_line";
                    break;
                case "배달통":
                    sector = "BEDALTONGREQUESTLINE";
                    key = "bedaltong_request_line";
                    break;
                case "기타":
                    sector = "ETCREQUESTLINE";
                    key = "etc_request_line";
                    break;
            }

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string tmp = device.readConfigString(sector, key);
                    Debug.WriteLine("tmp : " + tmp);
                    string[] words = tmp.Split('+');

                    for (int i = 0; i < words.Length; i++)
                    {
                        Debug.WriteLine("words.Length : " + words.Length);
                        Debug.WriteLine("words[i] : " + words[i]);
                        listBox7.Items.Add(words[i]);
                    }

                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }


        void AddExtractPatternButton(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
            {
                if (!CheckDuplicate(textBox2.Text))
                {
                    listBox2.Items.Add(textBox2.Text);

                    textBox2.Text = "";
                    textBox2.Focus();
                }
                else
                {
                    MessageBox.Show("중복");
                }
            }
            else
            {
                MessageBox.Show("빈칸");
            }
        }

        void RemoveExtractPatternButton(object sender, EventArgs e)
        {
            listBox2.Items.Remove(listBox2.SelectedItem);
        }

        bool CheckDuplicate(string sContent)
        {
            bool bDup = false;
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                if (Convert.ToString(listBox2.Items[i]).Equals(sContent))
                {
                    bDup = true;
                    break;
                }
            }

            return bDup;
        }

        private void FormDlgConfigParseSetting_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddExclusiveKeywordButton(object sender, EventArgs e)
        {
            if (textBox3.Text.Length > 0)
            {
                if (!CheckDuplicate(textBox3.Text))
                {
                    listBox3.Items.Add(textBox3.Text);

                    textBox3.Text = "";
                    textBox3.Focus();
                }
                else
                {
                    MessageBox.Show("중복");
                }
            }
            else
            {
                MessageBox.Show("빈칸");
            }
        }

        private void RemoveExclusiveKeywordButton(object sender, EventArgs e)
        {
            listBox3.Items.Remove(listBox3.SelectedItem);
        }

        private void AddLinkTargetButton(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                if (!CheckDuplicate(textBox1.Text))
                {
                    listBox1.Items.Add(textBox1.Text);

                    textBox1.Text = "";
                    textBox1.Focus();
                }
                else
                {
                    MessageBox.Show("중복");
                }
            }
            else
            {
                MessageBox.Show("빈칸");
            }
        }

        private void AddExclusiveBinaryButton(object sender, EventArgs e)
        {
            if (textBox5.Text.Length > 0)
            {
                if (!CheckDuplicate(textBox5.Text))
                {
                    listBox5.Items.Add(textBox5.Text);

                    textBox5.Text = "";
                    textBox5.Focus();
                }
                else
                {
                    MessageBox.Show("중복");
                }
            }
            else
            {
                MessageBox.Show("빈칸");
            }
        }

        private void AddIntervalRemoveBinaryKeyButton(object sender, EventArgs e)
        {
            if (textBox6.Text.Length > 0)
            {
                if (!CheckDuplicate(textBox6.Text))
                {
                    listBox6.Items.Add(textBox6.Text);

                    textBox6.Text = "";
                    textBox6.Focus();
                }
                else
                {
                    MessageBox.Show("중복");
                }
            }
            else
            {
                MessageBox.Show("빈칸");
            }
        }


        private void RemoveLinkTargetButton(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void RemoveExclusiveBinaryButton(object sender, EventArgs e)
        {
            listBox5.Items.Remove(listBox5.SelectedItem);
        }
        private void RemoveIntervalRemoveBinaryKeyButton(object sender, EventArgs e)
        {
            listBox6.Items.Remove(listBox6.SelectedItem);
        }


        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox4.Text.Length > 0)
            {
                if (!CheckDuplicate(textBox4.Text))
                {
                    listBox4.Items.Add(textBox4.Text);

                    textBox4.Text = "";
                    textBox4.Focus();
                }
                else
                {
                    MessageBox.Show("중복");
                }
            }
            else
            {
                MessageBox.Show("빈칸");
            }
        }
        private void button15_Click(object sender, EventArgs e)
        {
            if (textBox7.Text.Length > 0)
            {
                if (!CheckDuplicate(textBox7.Text))
                {
                    listBox7.Items.Add(textBox7.Text);

                    textBox7.Text = "";
                    textBox7.Focus();
                }
                else
                {
                    MessageBox.Show("중복");
                }
            }
            else
            {
                MessageBox.Show("빈칸");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox4.Items.Remove(listBox4.SelectedItem);
        }
        private void button16_Click(object sender, EventArgs e)
        {
            listBox7.Items.Remove(listBox7.SelectedItem);
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button21_Click(object sender, EventArgs e)
        {
            listBox8.Items.Clear();

            string sector = null;
            string key = null;

            const string ADDRESSENDCHAR = "ADDRESSENDCHAR";

            switch (comboBox5.Text)
            {
                case "배달의민족":
                    sector = ADDRESSENDCHAR;
                    key = "bemin_addr_end_char";
                    break;
                case "요기요":
                    sector = ADDRESSENDCHAR;
                    key = "yogiyo_addr_end_char";
                    break;
                case "이지포스":
                    sector = ADDRESSENDCHAR;
                    key = "easypos_addr_end_char";
                    break;
                case "배달천재":
                    sector = ADDRESSENDCHAR;
                    key = "delgen_addr_end_char";
                    break;
                case "오케이포스":
                    sector = ADDRESSENDCHAR;
                    key = "okpos_addr_end_char";
                    break;
                case "포스피드":
                    sector = ADDRESSENDCHAR;
                    key = "posfeed_addr_end_char";
                    break;
                case "배달통":
                    sector = ADDRESSENDCHAR;
                    key = "bedaltong_addr_end_char";
                    break;
                case "기타":
                    sector = ADDRESSENDCHAR;
                    key = "etc_addr_end_char";
                    break;
            }

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string tmp = device.readConfigString(sector, key);
                    Debug.WriteLine("tmp : " + tmp);
                    string[] words = tmp.Split('+');

                    for (int i = 0; i < words.Length; i++)
                    {
                        Debug.WriteLine("words.Length : " + words.Length);
                        Debug.WriteLine("words[i] : " + words[i]);
                        listBox8.Items.Add(words[i]);
                    }

                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox8.Items.Clear();

            string sector = null;
            string key = null;
            string select = comboBox5.SelectedItem.ToString();
            switch (select)
            {
                case "배달의민족":
                    sector = "ADDRESSENDCHAR";
                    key = "bemin_addr_end_char";
                    break;
                case "요기요":
                    sector = "ADDRESSENDCHAR";
                    key = "yogiyo_addr_end_char";
                    break;
                case "이지포스":
                    sector = "ADDRESSENDCHAR";
                    key = "easypos_addr_end_char";
                    break;
                case "배달천재":
                    sector = "ADDRESSENDCHAR";
                    key = "delgen_addr_end_char";
                    break;
                case "오케이포스":
                    sector = "ADDRESSENDCHAR";
                    key = "okpos_addr_end_char";
                    break;
                case "포스피드":
                    sector = "ADDRESSENDCHAR";
                    key = "posfeed_addr_end_char";
                    break;
                case "배달통":
                    sector = "ADDRESSENDCHAR";
                    key = "bedaltong_addr_end_char";
                    break;
                case "기타":
                    sector = "ADDRESSENDCHAR";
                    key = "etc_addr_end_char";
                    break;
            }

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string tmp = device.readConfigString(sector, key);
                    Debug.WriteLine("tmp : " + tmp);
                    string[] words = tmp.Split('+');

                    for (int i = 0; i < words.Length; i++)
                    {
                        Debug.WriteLine("words.Length : " + words.Length);
                        Debug.WriteLine("words[i] : " + words[i]);
                        listBox8.Items.Add(words[i]);
                    }

                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (textBox8.Text.Length > 0)
            {
                if (!CheckDuplicate(textBox8.Text))
                {
                    listBox8.Items.Add(textBox8.Text);

                    textBox8.Text = "";
                    textBox8.Focus();
                }
                else
                {
                    MessageBox.Show("중복");
                }
            }
            else
            {
                MessageBox.Show("빈칸");
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            listBox8.Items.Remove(listBox8.SelectedItem);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox7.Items.Clear();

            string sector = null;
            string key = null;
            string select = comboBox3.SelectedItem.ToString();
            switch (select)
            {
                case "배달의민족":
                    sector = "BEMINREQUESTLINE";
                    key = "bemin_request_line";
                    break;
                case "요기요":
                    sector = "YOGIYOREQUESTLINE";
                    key = "yogiyo_request_line";
                    break;
                case "이지포스":
                    sector = "EASYPOSREQUESTLINE";
                    key = "easypos_request_line";
                    break;
                case "배달천재":
                    sector = "DELGENREQUESTLINE";
                    key = "delgen_request_line";
                    break;
                case "오케이포스":
                    sector = "OKPOSREQUESTLINE";
                    key = "okpos_request_line";
                    break;
                case "포스피드":
                    sector = "POSFEEDREQUESTLINE";
                    key = "posfeed_request_line";
                    break;
                case "배달통":
                    sector = "BEDALTONGREQUESTLINE";
                    key = "bedaltong_request_line";
                    break;
                case "기타":
                    sector = "ETCREQUESTLINE";
                    key = "etc_request_line";
                    break;
            }

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string tmp = device.readConfigString(sector, key);
                    Debug.WriteLine("tmp : " + tmp);
                    string[] words = tmp.Split('+');

                    for (int i = 0; i < words.Length; i++)
                    {
                        Debug.WriteLine("words.Length : " + words.Length);
                        Debug.WriteLine("words[i] : " + words[i]);
                        listBox7.Items.Add(words[i]);
                    }

                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            listBox9.Items.Clear();

            string sector = null;
            string key = null;

            const string REQUESTEXCLUSIVEKEY = "REQUESTEXCLUSIVEKEY";

            switch (comboBox6.Text)
            {
                case "배달의민족":
                    sector = REQUESTEXCLUSIVEKEY;
                    key = "bemin_request_exclusive_key";
                    break;
                case "요기요":
                    sector = REQUESTEXCLUSIVEKEY;
                    key = "yogiyo_request_exclusive_key";
                    break;
                case "이지포스":
                    sector = REQUESTEXCLUSIVEKEY;
                    key = "easypos_request_exclusive_key";
                    break;
                case "배달천재":
                    sector = REQUESTEXCLUSIVEKEY;
                    key = "delgen_request_exclusive_key";
                    break;
                case "오케이포스":
                    sector = REQUESTEXCLUSIVEKEY;
                    key = "okpos_request_exclusive_key";
                    break;
                case "포스피드":
                    sector = REQUESTEXCLUSIVEKEY;
                    key = "posfeed_request_exclusive_key";
                    break;
                case "배달통":
                    sector = REQUESTEXCLUSIVEKEY;
                    key = "bedaltong_request_exclusive_key";
                    break;
                case "기타":
                    sector = REQUESTEXCLUSIVEKEY;
                    key = "etc_request_exclusive_key";
                    break;
            }

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string tmp = device.readConfigString(sector, key);
                    Debug.WriteLine("tmp : " + tmp);
                    string[] words = tmp.Split('+');

                    for (int i = 0; i < words.Length; i++)
                    {
                        Debug.WriteLine("words.Length : " + words.Length);
                        Debug.WriteLine("words[i] : " + words[i]);
                        listBox9.Items.Add(words[i]);
                    }

                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (textBox9.Text.Length > 0)
            {
                if (!CheckDuplicate(textBox9.Text))
                {
                    listBox9.Items.Add(textBox9.Text);

                    textBox9.Text = "";
                    textBox9.Focus();
                }
                else
                {
                    MessageBox.Show("중복");
                }
            }
            else
            {
                MessageBox.Show("빈칸");
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            listBox9.Items.Remove(listBox9.SelectedItem);
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox9.Items.Clear();

            string sector = null;
            string key = null;
            string select = comboBox6.SelectedItem.ToString();
            switch (select)
            {
                case "배달의민족":
                    sector = "REQUESTEXCLUSIVEKEY";
                    key = "bemin_request_exclusive_key";
                    break;
                case "요기요":
                    sector = "REQUESTEXCLUSIVEKEY";
                    key = "yogiyo_request_exclusive_key";
                    break;
                case "이지포스":
                    sector = "REQUESTEXCLUSIVEKEY";
                    key = "easypos_request_exclusive_key";
                    break;
                case "배달천재":
                    sector = "REQUESTEXCLUSIVEKEY";
                    key = "delgen_request_exclusive_key";
                    break;
                case "오케이포스":
                    sector = "REQUESTEXCLUSIVEKEY";
                    key = "okpos_request_exclusive_key";
                    break;
                case "포스피드":
                    sector = "REQUESTEXCLUSIVEKEY";
                    key = "posfeed_request_exclusive_key";
                    break;
                case "배달통":
                    sector = "REQUESTEXCLUSIVEKEY";
                    key = "bedaltong_request_exclusive_key";
                    break;
                case "기타":
                    sector = "REQUESTEXCLUSIVEKEY";
                    key = "etc_request_exclusive_key";
                    break;
            }

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string tmp = device.readConfigString(sector, key);
                    Debug.WriteLine("tmp : " + tmp);
                    string[] words = tmp.Split('+');

                    for (int i = 0; i < words.Length; i++)
                    {
                        Debug.WriteLine("words.Length : " + words.Length);
                        Debug.WriteLine("words[i] : " + words[i]);
                        listBox9.Items.Add(words[i]);
                    }

                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }

        private void listBox9_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            listBox4.Items.Clear();

            string sector = null;
            string key = null;
            string select = comboBox2.SelectedItem.ToString();
            switch (select)
            {
                case "배달의민족":
                    sector = "BEMINENDCHAR";
                    key = "bemin_end_char";
                    break;
                case "요기요":
                    sector = "YOGIYOENDCHAR";
                    key = "yogiyo_end_char";
                    break;
                case "이지포스":
                    sector = "EASYPOSENDCHAR";
                    key = "easypos_end_char";
                    break;
                case "배달천재":
                    sector = "DELGENENDCHAR";
                    key = "delgen_end_char";
                    break;
                case "오케이포스":
                    sector = "OKPOSENDCHAR";
                    key = "okpos_end_char";
                    break;
                case "포스피드":
                    sector = "POSFEEDENDCHAR";
                    key = "posfeed_end_char";
                    break;
                case "배달통":
                    sector = "BEDALTONGENDCHAR";
                    key = "bedaltong_end_char";
                    break;
                case "기타":
                    sector = "ETCENDCHAR";
                    key = "etc_end_char";
                    break;
            }

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string tmp = device.readConfigString(sector, key);
                    Debug.WriteLine("tmp : " + tmp);
                    string[] words = tmp.Split('+');

                    for (int i = 0; i < words.Length; i++)
                    {
                        Debug.WriteLine("words.Length : " + words.Length);
                        Debug.WriteLine("words[i] : " + words[i]);
                        listBox4.Items.Add(words[i]);
                    }

                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox5.Items.Clear();

            string sector = null;
            string key = null;
            string select = comboBox4.SelectedItem.ToString();
            switch (select)
            {
                case "배달의민족":
                    sector = "BEMINENCODING";
                    key = "bemin_encoding";
                    break;
                case "요기요":
                    sector = "YOGIYOENCODING";
                    key = "yogiyo_encoding";
                    break;
                case "이지포스":
                    sector = "EASYPOSENCODING";
                    key = "easypos_encoding";
                    break;
                case "배달천재":
                    sector = "DELGENENCODING";
                    key = "delgen_encoding";
                    break;
                case "오케이포스":
                    sector = "OKPOSENCODING";
                    key = "okpos_encoding";
                    break;
                case "포스피드":
                    sector = "POSFEEDENCODING";
                    key = "posfeed_encoding";
                    break;
                case "배달통":
                    sector = "BEDALTONGENCODING";
                    key = "bedaltong_encoding";
                    break;
                case "기타":
                    sector = "ETCENCODING";
                    key = "etc_encoding";
                    break;
            }

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string tmp = device.readConfigString(sector, key);
                    Debug.WriteLine("tmp : " + tmp);
                    string[] words = tmp.Split('+');

                    for (int i = 0; i < words.Length; i++)
                    {
                        Debug.WriteLine("words.Length : " + words.Length);
                        Debug.WriteLine("words[i] : " + words[i]);
                        listBox5.Items.Add(words[i]);
                    }

                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            listBox2.Items.Clear();

            string sector = null;
            string key = null;
            string select = comboBox1.SelectedItem.ToString();
            switch (select)
            {
                case "주문번호":
                    sector = "ORDER_NUM";
                    key = "order_num";
                    break;
                case "가격":
                    sector = "COST";
                    key = "cost";
                    break;
                case "전화번호":
                    sector = "CALL_NUM";
                    key = "call_num";
                    break;
                case "주소":
                    sector = "ADDRESS";
                    key = "address";
                    break;
                case "선불판단키워드":
                    sector = "PAYMENT_OPTION";
                    key = "payment_option";
                    break;
                case "주문일시":
                    sector = "ORDER_DATE";
                    key = "order_date";
                    break;
            }

            try
            {
                ModelAppDevice device = new ModelAppDevice();

                if (device != null)
                {
                    string tmp = device.readConfigString(sector, key);
                    Debug.WriteLine("tmp : " + tmp);
                    string[] words = tmp.Split('+');

                    for (int i = 0; i < words.Length; i++)
                    {
                        Debug.WriteLine("words.Length : " + words.Length);
                        Debug.WriteLine("words[i] : " + words[i]);
                        listBox2.Items.Add(words[i]);
                    }

                }
            }
            catch (Exception ex)
            {
                Kons.ShopCallpass.AppMain.AppCore.detectException(ex.Message);
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string endString = "";

            if (File.Exists(Environment.GetLogicalDrives().ElementAt(0) + @"\CallPass_log\CallpassLog.log"))
            {
                string[] lines = File.ReadAllLines(Environment.GetLogicalDrives().ElementAt(0) + @"\CallPass_log\CallpassLog.log");
                
                if (lines.Length > 0)
                {
                    const string key = "PROTOCOL_TMP:";
                    for (int i = lines.Length - 1; i >= 0; i--)
                    {
                        if (lines[i].Contains(key))
                        {
                            int keyIdx = lines[i].IndexOf(key) + key.Length;

                            while (lines[i].ElementAt(keyIdx) != ' ')
                            {
                                endString += lines[i].ElementAt(keyIdx++);
                            }
                            break;
                        }
                    }
                }
            }
            Utility.Utility.LogWrite("endString : " + endString);
            textBox4.Text = endString;
        }
    }
}
