using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotallyObedientMachine
{
    class Memory
    {
        private string instruction;
        private int operand;

        public Memory(string instruction, int operand)
        {
            this.instruction = instruction;
            this.operand = operand;
        }

        public string Instruction { get => instruction; set => instruction = value; }
        public int Operand { get => operand; set => operand = value; }
    }
}
