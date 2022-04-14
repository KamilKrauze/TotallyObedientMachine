using System;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TotallyObedientMachine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const int addresses = 256;
        private int cols = 8;
        private int rows = 32;

        private string filepath = "";

        private List<Memory> memory;

        private StackPanel[] stackPanel = new StackPanel[8];
        private DockPanel[] dockPanel = new DockPanel[256];
        private TextBox[] txtbox_addressNo = new TextBox[addresses];
        private TextBox[] txtbox_instruction = new TextBox[addresses];
        private TextBox[] txtbox_operand = new TextBox[addresses];

        public MainWindow()
        {
            memory = new List<Memory>();

            // Initialize memory list
            for (uint i = 0; i < addresses; i++)
            {
                memory.Add(new Memory(i, "", ""));
            }

            InitializeComponent();

            this.Background = new SolidColorBrush(Color.FromRgb(50,50,50));

            for (int i=0; i<cols; i++)
            {
                stackPanel[i] = new StackPanel();
            }

            // Initialize textboxes and dock panel and add to window
            int count = 0;
            for (int i = 0; i < cols; i++)
            {
                Grid.SetColumn(stackPanel[i], i);

                for (int j = 0; j < rows; j++)
                {
                    //System.Diagnostics.Debug.WriteLine(count);

                    dockPanel[count] = new DockPanel();
                    txtbox_addressNo[count] = new TextBox();
                    txtbox_instruction[count] = new TextBox();
                    txtbox_operand[count] = new TextBox();

                    txtbox_addressNo[count].Width = 25;
                    txtbox_addressNo[count].Text = count.ToString();
                    txtbox_addressNo[count].Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    txtbox_addressNo[count].Background = new SolidColorBrush(Color.FromRgb(80, 80, 80));
                    txtbox_addressNo[count].IsReadOnly = true;
                    txtbox_addressNo[count].Margin = new Thickness(0, 2, 0, 0);

                    txtbox_instruction[count].Width = 43;
                    txtbox_instruction[count].Text = "";
                    txtbox_instruction[count].Foreground = new SolidColorBrush(Color.FromRgb(0, 240, 240));
                    txtbox_instruction[count].Background = new SolidColorBrush(Color.FromRgb(94, 94, 94));
                    txtbox_instruction[count].TextAlignment = TextAlignment.Center;
                    txtbox_instruction[count].Margin = new Thickness(0, 2, 0, 0);
                    txtbox_instruction[count].Tag = count;
                    txtbox_instruction[count].TextChanged += delegate (object sender, TextChangedEventArgs e)
                    {
                        updateInstructionInMemory(sender, e);
                    };

                    txtbox_operand[count].Width = 55;
                    txtbox_operand[count].Text = "";
                    txtbox_operand[count].Foreground = new SolidColorBrush(Color.FromRgb(240, 240, 0));
                    txtbox_operand[count].Background = new SolidColorBrush(Color.FromRgb(94, 94, 94));
                    txtbox_operand[count].TextAlignment = TextAlignment.Right;
                    txtbox_operand[count].Margin = new Thickness(0, 2, 0, 0);
                    txtbox_operand[count].Tag = count;
                    txtbox_operand[count].TextChanged += delegate (object sender, TextChangedEventArgs e)
                    {
                        updateOperandInMemory(sender, e);
                    };

                    dockPanel[count].LastChildFill = false;
                    dockPanel[count].Children.Add(txtbox_addressNo[count]);
                    dockPanel[count].Children.Add(txtbox_instruction[count]);
                    dockPanel[count].Children.Add(txtbox_operand[count]);

                    stackPanel[i].Children.Add(dockPanel[count]);

                    count++;

                }
                grid_memView.Children.Add(stackPanel[i]);
            }
        }
        private void updateGUIMemory()
        {
            Debug.WriteLine(memory[0].Instruction);
            for (int i=0; i<addresses; i++)
            {
                
                txtbox_instruction[i].Text = memory[i].Instruction;
                txtbox_operand[i].Text = memory[i].Operand;
            }
            Debug.WriteLine("Done...");
        }

        private void updateInstructionInMemory(object sender, TextChangedEventArgs e)
        {
            var item = (TextBox)sender;
            Debug.WriteLine(item.Tag);
            memory[((int)item.Tag)].Instruction = item.Text;
            Debug.WriteLine(memory[((int)item.Tag)].Instruction);
        }

        private void updateOperandInMemory(object sender, TextChangedEventArgs e)
        {
            var item = (TextBox)sender;
            Debug.WriteLine(item.Tag);
            memory[((int)item.Tag)].Operand = item.Text;
            Debug.WriteLine(memory[((int)item.Tag)].Operand);
        }

        private void OpenButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.DefaultExt = ".program";
                bool? result = openFile.ShowDialog();
                if(result == true)
                {
                    filepath = openFile.FileName;
                    using (StreamReader sr = File.OpenText(filepath))
                    {
                        string s;
                        int index = 0;
                        while((s = sr.ReadLine()) != null)
                        {
                            string[] tokens = s.Split(';');

                            memory[index].Instruction = tokens[1].Trim();
                            memory[index].Operand = tokens[2].Trim();

                            index++;
                        }
                        sr.Close();
                    }
                    updateGUIMemory();
                }

            }
        }
        
        private void SaveButton_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = ".program";

                bool? result = saveFile.ShowDialog();
                if (result == true)
                {
                    filepath = saveFile.FileName;

                    if (!File.Exists(filepath))
                    {
                        using (StreamWriter sw = File.CreateText(filepath))
                        {
                            for (int i = 0; i < addresses; i++)
                            {
                                sw.WriteLine(memory[i].Address.ToString() + ";" + memory[i].Instruction + ";" + memory[i].Operand);
                            }

                            sw.Close();
                        }

                        System.Diagnostics.Debug.WriteLine("Complete saving");
                    }
                    else
                    {
                        File.Delete(filepath);
                        
                        using (StreamWriter sw = File.CreateText(filepath))
                        {
                            for (int i = 0; i < addresses; i++)
                            {
                                sw.WriteLine(memory[i].Address.ToString() + "; " + memory[i].Instruction + "; " + memory[i].Operand);
                            }
                            sw.Flush();
                            sw.Close();
                        }
                    }
                }
            }
        }

    }
}
