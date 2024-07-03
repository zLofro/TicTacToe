using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.utils
{
    public class Pair<T, E>
    {

        private T firstElement;
        private E secondElement;

        public T FirstElement
        {
            get { return firstElement; }
            set { firstElement = value; } 
        }
        public E SecondElement
        {
            get { return secondElement; }
            set { secondElement = value; }
        }

        public Pair(T firstElement, E secondElement)
        {
            this.firstElement = firstElement;
            this.secondElement = secondElement;
        }

    }
}
