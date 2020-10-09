using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DeweyDecimalTrainer.LinkedList
{
    public class DoublyLinkedList : IEnumerable<Node>
    {

        private Node head;
        public Node First
        {
            get { return head; }
        }

        private Node tail;
        public Node Last
        {
            get { return tail; }
        }

        public int Length { get; private set; }

        public IEnumerator<Node> GetEnumerator()
        {
            Node current = head;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Node> GetEnumeratorReverse()
        {
            Node current = tail;
            while (current != null)
            {
                yield return current;
                current = current.Previous;
            }
        }

        // AKA addLast
        public void Add(DecimalClass data)
        {

            Node newNode = new Node(data);
            if (tail == null)
            {
                head = newNode;
            }
            else
            {
                // Connect the final node
                newNode.Previous = tail;
                tail.Next = newNode;
            }
            // Set the new tail
            tail = newNode;
            Length++;

        }

        public bool Remove(DecimalClass value)
        {

            Node current = head;
            while (current != null)
            {

                if (current.Data == value)
                {

                    // End of the list
                    if (current.Next == null)
                    {
                        // Remove the last item in the list
                        tail = current.Previous;
                    }
                    else
                    {
                        current.Next.Previous = current.Previous;
                    }

                    // Start of the list
                    if (current.Previous == null)
                    {
                        head = current.Next;
                    }
                    else
                    {
                        // Tie the nodes together
                        current.Previous.Next = current.Next;
                    }

                    current = null;
                    Length--;
                    return true;

                }

                current = current.Next;

            }

            return false;

        }

        // Bubble sort the given linked list
        public Node CorrectSort()
        {
            int swapped;
            Node ptr1;
            Node lptr = null;

            // Checking for empty list  
            if (head == null)
                return null;

            do
            {
                swapped = 0;
                ptr1 = head;

                while (ptr1.Next != lptr)
                {
                    if (double.Parse(ptr1.Data.Decimal, CultureInfo.InvariantCulture) > double.Parse(ptr1.Next.Data.Decimal, CultureInfo.InvariantCulture))
                    {
                        DecimalClass t = ptr1.Data;
                        ptr1.Data = ptr1.Next.Data;
                        ptr1.Next.Data = t;
                        swapped = 1;
                    }
                    ptr1 = ptr1.Next;
                }
                lptr = ptr1;
            }
            while (swapped != 0);
            return head;
        }

    }
}
