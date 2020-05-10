using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameikiUI.Playground {
    public partial class Test : Form {
        public Test() {
            InitializeComponent();

            PrintColoredString(richTextBox1, "Hello, World!", Color.Brown); // Ispis inicijalnog teksta
            richTextBox1.AppendText(Environment.NewLine);

            richTextBox1.SelectionStart = 0; // Odabir riječi "Hello"
            richTextBox1.SelectionLength = 5;
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold); // Podebljanje riječi "Hello"

            // Indentacija teksta
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectedText = "Ovaj tekst je indentiran za 10 piksela";
            richTextBox1.SelectionIndent = 10; // Veličina indentacija u pikselima
            richTextBox1.AppendText($"{richTextBox1.SelectedText}");
        }

        private static void PrintColoredString(RichTextBox richTextBox, string str, Color color) {
            richTextBox.SelectionStart = richTextBox.TextLength; // Početak odabira (dodaje se na kraj trenutnog sadržaja)
            richTextBox.SelectionLength = 0; // Duljina odabira 

            richTextBox.SelectionColor = color; // Boja kojom će se tekst prebojati
            richTextBox.AppendText(str); // Ispis teksta
            richTextBox.SelectionColor = richTextBox.ForeColor;
        }
    }
}