using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShare.Model
{
    struct TextViewSelection
    {
        public TextViewPosition StartPosition { get; set; }
        public TextViewPosition EndPosition { get; set; }
        public string Text { get; set; }
        public string Filename { get; set; }

        public TextViewSelection(TextViewPosition a, TextViewPosition b, string text, string filename)
        {
            StartPosition = TextViewPosition.Min(a, b);
            EndPosition = TextViewPosition.Max(a, b);
            Text = text;
            Filename = filename;
        }
    }
}
