using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using Timer = System.Windows.Forms.Timer;

namespace ExchangeRates
{
    public partial class FrmExchangeRates : Form
    {
        public FrmExchangeRates()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        { TableCreate();
            dataGridView1.DataSource = Source();
            RefreshCheck();
        }
        DataTable dt = new DataTable();
        DataRow dr;
        public DataTable Source()
        {
            try
            {
                
               XmlTextReader rdr = new XmlTextReader("http://www.tcmb.gov.tr/kurlar/today.xml");
                // XmlTextReader nesnesini yaratıyoruz ve parametre olarak xml dokümanın urlsini veriyoruz
                // XmlTextReader urlsi belirtilen xml dokümanlarına hızlı ve forward-only giriş imkanı sağlar.
                XmlDocument myxml = new XmlDocument();
                // XmlDocument nesnesini yaratıyoruz.
                myxml.Load(rdr);
                // Load metodu ile xml yüklüyoruz
                XmlNode tarih = myxml.SelectSingleNode("/Tarih_Date/@Tarih");
                XmlNodeList mylist = myxml.SelectNodes("/Tarih_Date/Currency");
                XmlNodeList adi = myxml.SelectNodes("/Tarih_Date/Currency/Isim");
                XmlNodeList kod = myxml.SelectNodes("/Tarih_Date/Currency/@Kod");
                XmlNodeList doviz_alis = myxml.SelectNodes("/Tarih_Date/Currency/ForexBuying");
                XmlNodeList doviz_satis = myxml.SelectNodes("/Tarih_Date/Currency/ForexSelling");
                XmlNodeList efektif_alis = myxml.SelectNodes("/Tarih_Date/Currency/BanknoteBuying");
                XmlNodeList efektif_satis = myxml.SelectNodes("/Tarih_Date/Currency/BanknoteSelling");

                for (int i = 0; i < 4; i++)
                {
                    dr = dt.NewRow();
                    dr[0] = adi.Item(i).InnerText.ToString();
                    dr[1] = kod.Item(i).InnerText.ToString();
                    // Kod satırları
                    dr[2] = doviz_alis.Item(i).InnerText.ToString();
                    // Döviz Alış
                    dr[3] = doviz_satis.Item(i).InnerText.ToString();
                    // Döviz  Satış
                    dr[4] = efektif_alis.Item(i).InnerText.ToString();
                    // Efektif Alış
                    dr[5] = efektif_satis.Item(i).InnerText.ToString();
                    // Efektif Satış.
                    dt.Rows.Add(dr);
                    i = i + 2;
                }
                return dt;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }


        public void TableCreate()
        {

            // DataTable ın satırlarını tanımlıyoruz.
            dt.Columns.Add(new DataColumn("Adı", typeof(string)));
            dt.Columns.Add(new DataColumn("Kod", typeof(string)));
            dt.Columns.Add(new DataColumn("Döviz alış", typeof(string)));
            dt.Columns.Add(new DataColumn("Döviz satış", typeof(string)));
            dt.Columns.Add(new DataColumn("Efektif alış", typeof(string)));
            dt.Columns.Add(new DataColumn("Efektif Satış", typeof(string)));
           
        }

        private void tmr_Refresh_Tick(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Source();
            // MessageBox.Show("Değişti");
        }

        public void RefreshCheck()
        {
            try
            {
                Timer tmr = new Timer();
                tmr.Interval = (60 * 1 * 1000);//5 sec
                tmr.Tick += new EventHandler(tmr_Refresh_Tick);
                tmr.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

       
    }
}
