using Dewey_Training.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dewey_Training.DoublyLinkedList
{
    public class Node
    {

        private DeweyDecimal _data;
        public DeweyDecimal Data
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

        public Node(DeweyDecimal data)
        {
            Data = data;
        }

    }
}
