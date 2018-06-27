using System.Windows.Shapes;
using System.Windows.Controls;

namespace Sorts
{
    public class Element
    {
       public Element(int i,Grid c,Rectangle r, string s)
        {
            value = i;
            container = c;
            rectangle = r;
            brush = s;
        }
        public Element()
        {

        }
        public int value;
        public Grid container;
        public Rectangle rectangle;
        public string brush;
    }
}
