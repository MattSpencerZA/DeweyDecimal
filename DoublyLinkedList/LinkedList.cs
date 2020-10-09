using Dewey_Training.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Dewey_Training.DoublyLinkedList
{
    public class LinkedList : IEnumerable<Node>
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
        public void Add(DeweyDecimal data)
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

        public void AddFirst(DeweyDecimal data)
        {

            Node newNode = new Node(data);
            if (head == null)
            {
                tail = newNode;
            }
            else
            {
                head.Previous = newNode;
            }

            head = newNode;
            Length++;

        }

        // Checks if the item exists in the list
        public bool Contains(DeweyDecimal value)
        {

            Node current = head;
            while (current != null)
            {

                if (current.Data == value)
                {
                    return true;
                }

                current = current.Next;

            }

            return false;

        }

        // Finds the node that contains the specified value
        public Node Find(DeweyDecimal value)
        {

            Node current = head;
            while (current != null)
            {

                if (current.Data == value)
                {
                    return current;
                }

                current = current.Next;

            }

            return null;

        }

        // Finds the last node that contains the specified value
        public Node FindLast(DeweyDecimal value)
        {

            Node current = tail;
            while (current != null)
            {

                if (current.Data == value)
                {
                    return current;
                }

                current = current.Previous;

            }

            return null;

        }

        public bool Remove(DeweyDecimal value)
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

        public void RemoveFirst()
        {

            if (head != null)
            {

                head = head.Next;

                // Empty list
                if (head == null)
                {
                    tail = null;
                }
                Length--;

            }

        }

        public void RemoveLast()
        {

            if (tail != null)
            {

                tail = tail.Previous;

                // Empty list
                if (tail == null)
                {
                    head = null;
                }
                Length--;

            }

        }

        // Bubble sort the given linked list
        public Node BubbleSort()
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
                    if (Double.Parse(ptr1.Data.Decimal) > Double.Parse(ptr1.Next.Data.Decimal))
                    {
                        DeweyDecimal t = ptr1.Data;
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
