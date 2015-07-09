using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AddFieldsTables
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class node
        {
            public int id;
            public int up;
            public int down;
            public int right;
            public int left;
            public string name;
            public node(){}
            public node(int up_, int down_, int right_, int left_, string name_, int id_)
            {
                this.id = id_;
                this.up = up_;
                this.down = down_;
                this.right = right_;
                this.left = left_;
                this.name = name_;
            }
            public static List<node> GetNodes(DataTable t)
            {
                List<node> res = new List<node>();
                foreach (DataRow r in t.Rows)
                {
                    res.Add(new node((int)r["VVERH"], (int)r["VNIZ"], (int)r["VPRAVO"], (int)r["VLEVO"], r["NAZV"].ToString(), (int)r["IDF"]));
                }
                return res;
            }
        }
        class VTreeNode : TreeNode
        {
            public node VNode;
            public void Add(string name,node VNode_)
            {
                
                this.VNode = VNode;

            }
        }
        public node FindRoot(List<node> Nodes_)
        {
            node res = Nodes_[0];

            foreach (node n in Nodes_)
            {
                
                if (n.up < res.up)
                {
                    res = n;
                }
            }
            //Nodes_.Remove(res);
            return res;
        }
        /*public List<node> FindChilds(List<node> Nodes_, node cur)
        {
            List<node> childs = new List<node>();
            TreeNodeCollection tnc;
            //tnc.Clear();
            foreach (node n in Nodes_)
            {
                if (n.up == cur.id)
                    childs.Add(n);
            }
            foreach(node n in childs)
            {
                //tnc.Add(n.name);
            }

            return childs;
        }*/

        public void FillRecursive(TreeNode trn, List<node> allNodes_)
        {
            foreach (node n in allNodes_)
            {
                if (n.up == int.Parse(trn.Text))
                {
                    trn.Nodes.Add(n.id.ToString());
                    //trn.Nodes[0].t
                }
            }
            foreach (TreeNode n in trn.Nodes)
            {
                FillRecursive(n, allNodes_);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection con =new SqlConnection(XmlConnections.GetConnection("/Connections/t1"));
            da.SelectCommand = new SqlCommand("select * from KOMPTEST..FIELDS where TIPTAB = 'DATA' order by VVERH asc",con);
            DataSet ds = new DataSet();
            int cnt = da.Fill(ds);
            string comm = "ALTER TABLE KOMPTEST..FIELDS ADD MyField Numeric";
            
            List<node> AllNodes = node.GetNodes(ds.Tables[0]);



            tr.BeginUpdate();
            tr.Nodes.Clear();
            node root = FindRoot(AllNodes);
            tr.Nodes.Add(root.id.ToString());
            //List<node> Childs = FindChilds(AllNodes, FindRoot(AllNodes));

            FillRecursive(tr.Nodes[0], AllNodes);
            


            


            tr.EndUpdate();
            
        }
    }
}
