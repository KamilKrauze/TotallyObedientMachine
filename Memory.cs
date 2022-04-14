using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotallyObedientMachine
{
    class Memory
    {
        private uint address;
        private string instruction;
        private string operand;

        public Memory(uint address, string instruction, string operand)
        {
            this.address = address;
            this.instruction = instruction;
            this.operand = operand;
        }
        public uint Address { get => address; set => address = value; }
        public string Instruction { get => instruction; set => instruction = value; }
        public string Operand { get => operand; set => operand = value; }
    }
}
