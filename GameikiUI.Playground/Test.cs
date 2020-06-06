using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameikiUI.Playground {
    public partial class Test : Form {
        public Test() {
            InitializeComponent();
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