using System;
using System.Collections.Generic;
using System.Text;

namespace DeweyDecimalTrainer.LinkedList
{
    public class Node
    {

        private DecimalClass _data;
        public DecimalClass Data
        {
            get { return _data; }
            set { _data = value; }
        }

        private Node _next;
        public Node Next
        {
            get { return _next; }
            set { _next = value; }
        }

        private Node _previous;
        public Node Previous
        {
            get { return _previous; }
            set { _previous = value; }
        }

        public Node(DecimalClass data)
        {
            Data = data;
        }

    }
}
