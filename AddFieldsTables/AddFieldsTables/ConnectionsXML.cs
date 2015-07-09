using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
namespace AddFieldsTables
{
    public class XmlConnections
    {
        public XmlConnections()
        {

        }
        private static String filename = Application.StartupPath + "\\DBConnections.xml";
        private static XmlDocument doc;
        public static string GetConnection(string s)
        {
            if (!File.Exists(filename))
            {
                throw new Exception("���� � ������������� 'DBConnections.xml' �� ������.");
            }

            try
            {
                doc = new XmlDocument();
                doc.Load(filename);
            }
            catch
            {
                //MessageBox.Show(ex.Message);
                throw;
            }
            XmlNode node;
            try
            {
                node = doc.SelectSingleNode(s);
            }
            catch
            {
                throw new Exception("���� " + s + " �� ������ � ����� DBConnections.xml"); ;
            }

            return node.InnerText;
        }
    }

   
}
