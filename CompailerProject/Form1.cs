using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompailerProject
{
    public partial class Form1 : Form
    {
        LinkedList<string> stack;
        List<string> rulesUsed;
        JObject rules;
        public Form1()
        {
            InitializeComponent();
           
            string path = Environment.CurrentDirectory + "\\Rules.json";

            StreamReader sr = new StreamReader(path);
            string raw = sr.ReadToEnd();
            rules = JObject.Parse(raw);

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string result = convert(textBox1.Text);
            textBox2.Text = result;
            validate(result);
        }

        private string convert(string input) 
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i]))
                {
                    input = input.Replace(input[i], 'i');
                }
            }

            return input+"#";
        }

        private void validate(string input)
        {
            string currentState = "";
            int position = 0;
            richTextBox1.Text = "";
            stack = new LinkedList<string>();
            rulesUsed = new List<string>();

            stack.AddFirst("#");
            stack.AddFirst("E");

            while (currentState != "error")
            {
                currentState = string.Empty;
                string key = stack.First.Value + input[position].ToString();

                if (rules.ContainsKey(key))
                {
                    string[] rule = rules[key].ToString().Split(',');
                    stack.RemoveFirst();

                    if (rule[0].Equals("OK"))
                    {
                        currentState = "OK";
                        textBox3.Text = "OK";
                        break;
                    }
                    else if (rule[0].Equals("pop"))
                    {
                        position++;
                        if (position >= input.Length)
                        {
                            break;
                        }
                        stack.ToList().ForEach(n => currentState += n);
                        richTextBox1.AppendText("\n"+currentState+"--------------" + input[position]);
                        continue;
                    }
                    else if (rule[0].Equals("e"))
                    {
                        stack.ToList().ForEach(n => currentState += n);
                        rulesUsed.Add(rule[1]);
                        richTextBox1.AppendText("\n" + currentState + "--------------" + input[position]);
                        continue;
                    }

                    char[] ruleSplit =  rule[0].ToCharArray();
                    for (int i = ruleSplit.Length - 1; i >= 0; i--)
                    {
                        stack.AddFirst(ruleSplit[i].ToString());
                    }
                    rulesUsed.Add(rule[1]);
                    stack.ToList().ForEach(n => currentState += n);
                    richTextBox1.AppendText("\n" + currentState + "--------------" + input[position]);
                }
                else
                {
                    currentState = "error";
                    textBox3.Text = "ERROR";
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
