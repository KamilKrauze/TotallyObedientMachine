using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TotallyObedientMachine
{
    enum Instruction
    {
        // Memory instructions
        MOV, // Move - Copy value from src address to destination address.
        LDA, // Load - Load value to accumulator from address.
        STO, // Store - Store value in accumulator at address.
        ILD, // Indirect Load - Load from accumulator from address pointer.
        IST, // Indirect Store - Store value from accumulator at address pointer.

        // Logical Comparators
        CMP, // Compare values of 2 addresses
        
        // Jumps
        JMP, // Unconditional direct jump.
        IJP, // Unconditional indirect jump.
        JEQ, // Jump if accumulator equal to 0.
        JNE, // Jump if negative value in accumulator.
        JGE, // Jump if accumulator greater than 0.

        // Math Operations
        ADD, // Add value of 2 addresses or 1 address and 1 value.
        SUB, // Subtract value of 2 addresses or 1 address and 1 value.
        MPY, // Multiply address 1 by address 2, send result to address.
        DIV, // Divide address 1 by address 2, send result to address.
        INC, // Increment accumulator by value.
        DEC, // Decrement accumulator by value.

        HLT, // Halts execution
        NOP, // No operation
    }

    class Exectute
    {
        private Instruction instruction;
        int accumulator;
        int programCounter;
        TextBox[] instructions;
        TextBox[] operands;

        public Exectute(ref TextBox[] instructions, ref TextBox[] operands, ref string programCounter, ref string accumulator)
        {
            instruction = Instruction.NOP;

            this.instructions = instructions;
            this.operands = operands;

            this.programCounter = Int16.Parse(programCounter);
            this.accumulator = Int16.Parse(accumulator);
        }

        public Instruction Instruction { get => instruction; set => instruction = value; }

        public void MOV(string addressDest, string addressSrc)
        {
            if (addressDest == "" || addressSrc == "")
                return;

            int dest;
            int src;

            bool? result = int.TryParse(addressDest, out dest);
            bool? result1 = int.TryParse(addressSrc, out src);

            if (result == true && result1 == true)
            {
                if ((src >= 0 && src <= 255) && (dest >= 0 && dest <= 255))
                {
                    operands[dest].Text = operands[src].Text;
                }
            }
        }

        public void LDA(string address)
        {
            if (address == "")
                return;

            int addr;
            bool? result = int.TryParse(address, out addr);
            if(result == true)
            {
                if (operands[addr].Text == "")
                    return;

                
                if (addr >= 0 && addr <= 255)
                {
                    int.TryParse(operands[addr].Text, out accumulator);
                }
            }

            
        }

        public void STO(string address)
        {
            if (address == "")
                return;

            int addr;
            bool? result = int.TryParse(address, out addr);
            if(result == true)
            {
                if (addr >= 0 && addr <= 255)
                {
                    operands[addr].Text = accumulator.ToString();
                }
            }
        }
    
        public void ILD(string address)
        {
            if (address == "")
                return;
            
            int addr;
            bool? result = int.TryParse(address, out addr);
            if (result == true)
            {
                if (operands[addr].Text == "")
                    return;

                if (addr >= 0 && addr <= 255)
                {
                    int indirectAddress;
                    bool? result2 = int.TryParse(operands[addr].Text, out indirectAddress);
                    if (result2 == true)
                    {
                        if (indirectAddress >= 0 && indirectAddress <= 255)
                        {
                           int.TryParse(operands[indirectAddress].Text, out accumulator);
                        }
                    }
                }
            }
        }

        public void IST(string address)
        {
            if (address == "")
                return;

            int addr;

            bool? result = int.TryParse(address, out addr);
            if (result == true)
            {
                if (operands[addr].Text == "")
                    return;

                if (addr >= 0 || addr <= 255)
                {
                    int indirectAddress;
                    bool? result1 = int.TryParse(operands[addr].Text, out indirectAddress);
                    if (result1 == true)
                    {
                        if (indirectAddress >= 0 && indirectAddress <= 255)
                        {
                            operands[indirectAddress].Text = accumulator.ToString();
                        }
                    }
                }
            }
        }

        public void CMP(string address1, string address2)
        {
            if (address1 == "" || address2 == "")
                return;
            
            int addr1;
            int addr2;
            
            bool? result = int.TryParse(address1, out addr1);
            bool? result1 = int.TryParse(address2, out addr2);

            if(result == true & result1 == true)
            {
                if (addr1 < 0 || addr1 > 255)
                    return;
                else if (addr2 < 0 || addr2 > 255)
                    return;
                else if (addr1 == addr2)
                    return;

                if (operands[addr1].Text == operands[addr2].Text)
                    accumulator = 1;
                else
                    accumulator = 0;
            }
        }

        public void JMP(string address)
        {
            if (address == "")
            {
                return;
            }

            int newAddress = 0;
            bool? result = int.TryParse(address, out newAddress);
            if (result == true)
            {
                if (newAddress >= 0 && newAddress <= 255)
                {
                    programCounter = newAddress;
                }
            }
        }
        public void IJP(string address)
        {
            if (address == "")
                return;
            
            int newAddress = 0;
            bool? result = int.TryParse(address, out newAddress);
            if(result == true)
            {
                int indirectAddress = 0;
                bool? result1 = int.TryParse(operands[newAddress].Text, out indirectAddress);
                if (result1 == true)
                {
                    int value;
                    bool? result2 = int.TryParse(operands[indirectAddress].Text, out value);
                    if (result2 == true)
                    {
                        programCounter = value;
                    }
                }
            }
        }
        public void JEQ(string address)
        {
            if (accumulator == 0)
            {
                JMP(address);
            }
        }
        public void JNE(string address)
        {
            if(accumulator < 0)
            {
                JMP(address);
            }
        }
        public void JGE(string address)
        {
            if(accumulator > 0)
            {
                JMP(address);
            }
        }

        public void MATH(string address1, string address2)
        {
            if (address1 == "" && address2 == "")
                return;

            int addr1;
            int addr2;
            bool? result = int.TryParse(address1, out addr1);
            bool? result2 = int.TryParse(address2, out addr2);
            
            if(result == true && result2 == true)
            {
                int operand1;
                int operand2;
                bool? result3 = int.TryParse(operands[addr1].Text, out operand1);
                bool? result4 = int.TryParse(operands[addr2].Text, out operand2);

                if(result3 == true && result4 == true)
                {
                    if (instruction == Instruction.ADD)
                        operand1 += operand2;
                    else if (instruction == Instruction.SUB)
                        operand1 -= operand2;
                    else if (instruction == Instruction.MPY)
                        operand1 *= operand2;
                    else if (instruction == Instruction.DIV)
                        operand1 /= operand2;

                    operands[addr1].Text = operand1.ToString();
                }
            }
        }

        public void incrementOrDecrement(string address1)
        {
            if (address1 == "")
                return;

            int addr1;
            bool? result = int.TryParse(address1, out addr1);

            if (result == true)
            {
                int operand1;
                bool? result3 = int.TryParse(operands[addr1].Text, out operand1);

                if (result3 == true)
                {
                    if (instruction == Instruction.INC)
                        accumulator += operand1;
                    else if (instruction == Instruction.DEC)
                        accumulator -= operand1;
                }
            }
        }
    }
}
