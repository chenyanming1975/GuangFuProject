using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace DefectDetection
{
    public class QueList
    {
        // <summary>队列操作
        // <parament:"s">写入队列的字符串</parament>
        // </summary>
        Queue q = new Queue();
        textFile Txt = new textFile(Application.StartupPath + "\\log.txt");
        string[] qArray = new string[40];
        AutoResetEvent queueEvent = new AutoResetEvent(false);
        public void queueIn(string s)
        {
            try
            {
                if (s != "")
                {
                    if (q.Count < 40)
                    {
                        q.Enqueue(s);
                     
                    }
                    else
                    {
                        q.CopyTo(qArray, 0);
                        this.QueueOutAll();
                        q.Clear();
                        q.Enqueue(s);
                      
                    }
                  
                }
            }
            catch (Exception ecx)
            {
                q.Enqueue(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + ecx);
            }
        }

        public QueList()
        {
            q = new Queue();
        }

        public void QueueOutAll()
        {
            string x = "";
            Txt.txtfile(x);
            if (this.getCount() != 0)
            {
                foreach (string s in q)
                {
                    Txt.addTextfile(q.Dequeue().ToString());
                }
            }
        }
  
        public string queueOut()
        {
            try
            {
                string s;
                if (q.Count != 0)
                 {
                        s = q.Dequeue().ToString();
                        return s;
                  }
                  return "";
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.ToString() + "queueOut");
                return "";
            }
        }

        public int getCount()
        {
            return q.Count;
        }
        public void clear()
        {
            q.Clear();
        }
    }

    class textFile
    {
        string _path;
        public string path
        {
            get { return _path; }
            set { _path = value; }
        }

        public textFile(string path)
        {
            this.path = path;
        }

        public void txtfile(string s)
        {
            FileStream F = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(F);
            sw.WriteLine(s);
            sw.Close();
            F.Close();
        }
        public void addTextfile(string s)
        {
            FileStream F = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(F);
            sw.WriteLine(s);
            sw.Close();
            F.Close();
        }
    }
}

