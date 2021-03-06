using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HiSystems.Interpreter
{
    public class Bf : Command
    {
		private bool flipflop = true;

        public override string Name
        {
            get
            {
                return "Bf";
            }
        }

        public override string Description
        {
            get
            {
                return string.Format("Evaluate a Bf file, script text argument or convert the text to Bf.", '\t', Environment.NewLine);
            }
        }
                public override string Usage
        {
			get { return "{filename}/{sourcecode}/{text}"; }
        }

        public override Literal Execute(string[] args)
        {
			Programmer.Interpreter Process;
			int state = 0;
			int index = 0;
			string Source = string.Empty;

			bool text2bf = false;
			string Input = string.Empty;
			StringBuilder output = new StringBuilder();

            if (args.Length > 0)
            {
                if (!System.IO.File.Exists(Path.Execute(args[0])))
                {
                    var r = new Regex(@"^[+-<>.,\[\]]+$");

                    if (r.Matches(args[0]).Count > 0)
                    {
                        Source = args[0];
                    }
                    else
                    {
						text2bf = true;
						if (flipflop)
						{
							flipflop = false;
//                            Init();
//                            Next();
//
//                            for (int i = 1; i < args[0].Length; i++)
//                            {
//                                Print.Text(do_generate(args[0].Substring(0,i)));
//                            }
//							return new Text (string.Empty);
							Source = "+++++[>+++++++++<-],[[>--.++>+<<-]>+.->[<.>-]<<,]";
						}
						else
						{
							flipflop = true;
							Source = "++++++[>+++++++>+++++++++++++++>+++++++<<<-]>++++>+>+>,[[<.>-]<<<.>.>++.--<++.-->>,]";
						}
                    }
                }
                else
                {
                    Source = System.IO.File.ReadAllText(Path.Execute(args[0]));
                }

                Process = new Programmer.Interpreter(Source, () =>
                {
                    if (state == 0)
                    {
                        state++;
                        if (args.Length > 1)
                        {
                            Input = args[1];
                        }
						else if(text2bf)
						{
							Input = args[0];
							Output.Text(Input);
						}
                        else // Get input from the user
                        {
                            System.Console.WriteLine();
                            System.Console.Write(">: ");
                            Input = System.Console.ReadLine();
                        }

                        return (byte)Input[index++];
                    }
                    else if (state == 1)
                    {
                        // Send each character of the input
                        if (index < Input.Length)
                        {
                            return (byte)Input[index++];
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                },
                (b) =>
                {
                    output.Append((char)b);
                    output.Append((char)b);
                });

                Process.Run(0); // Max iterations a program may run before being killed (prevents infinite loops)

                return new Text(output.ToString());
            }

            return base.Error(1); //Wrong number of arguments
        }



		bool iter = true;
		int repeat = 2;
		//start;

		private string[,] map = new string[256,256];
		private string[] plus_map = new string[256];
		private string[] minus_map = new string[256];


		public int gcd(int a, int b)
		{
			if(b == 0)
			{
				return a;
			}
			else
			{
				return gcd(b, a % b);
			}
		}

		public void Next()
		{
			// keep applying rules until we can't find any more shortenings                             
			bool iter = false;

			// multiplication by n/d                                                                
			//console.time("1");
			for (var x = 0; x < 256; x++)
			{
				for (var d = 1; d < 40; d++)
				{
					var d_inv = inverse_mod(d, 256) & 255;

					for (var n = 1; n < 40; n++)
					{
						if(gcd(d, n) != 1)
						{
							continue;
						}

						int j = 0;
						int i = 0;

						if((d & 1) > 0) //&
						{
							j = 0;
							i = x * d_inv & 255;
						}
						else
						{
							j = x;
							for (i = 0; i < 256 && i < j; i++)
							{
								j = (j - d) & 255;
							}
						}

						if (j == 0)
						{
							var y = n * i & 255;

							if (d + n + 5 < map[x, y].Length)
							{
								map[x,y] = "[" + minus_map[d] + ">" + plus_map[n] + "<]>";
								//iter = true;
							}
						}

						if(d == 1) //&
						{
							j = 0;
							i = -x * d_inv & 255;
						}
						else
						{
							j = x;
							for (i = 0; i < 256 && i < j; i++)
							{
								j = (j + d) & 255;
							}
						}

						if (j == 0)
						{
							var y = -n * i & 255;

							if (d + n + 5 < map[x,y].Length)
							{
								map[x,y] = "[" + plus_map[d] + ">" + minus_map[n] + "<]>";
								//iter = true;
							}
						}

					}
				}
			}
			//console.timeEnd("1");

			// combine number schemes                                                               
			//console.time("2");
			string[] map_x = new string[256];
			string[] map_z = new string[256];
			string map_xz = string.Empty;

			for (var x = 0; x < 256; x++)
			{
				for (var c = 0; c < 256; c++)
				{
					map_x[c] = map[x,c];
				}

				for (var z = 0; z < 256; z++)
				{
					for (var c = 0; c < 256; c++)
					{
						map_z[c] = map[z,c];
					}

					for (var c = 0; c < 256; c++)
					{
						map_xz = map_x[z];
					}


					for (var y = 0; y < 256; y++)
					{
						if (map_xz.Length + map_z[y].Length < map_x[y].Length)
						{
							map_x[y] = map_xz + map_z[y];
							//iter = true;
						}
					}
				}
			}
			//console.timeEnd("2");

            repeat--;
            if (repeat > 0)
            {
                //info_head.textContent += ".";
                Next();
                //setTimeout(next, 0);
            }
            else
            {
                //info_head.textContent += ". done (" + ((new Date() - start) / 1000).toFixed(2) + " seconds).";
                return;
            }
		}

		public int inverse_mod(int n, int m)
		{
			int inv1 = 1;
			int inv2 = 0;
			int tmp = 0;

			while(m > 0)
			{
				tmp = inv1;
				inv1 = inv2;
				inv2 = tmp - inv2 * (n / m | 0);

				tmp = n;
				n = m;
				m = tmp % m;
			}

			return inv1;
		}

		public int shortest_str(string[] arr)
		{
			var min = 0;

			for(var i = 1; i < arr.Length; i++)
			{
				if(arr[i].Length < arr[min].Length)
				{
					min = i;
				}
			}

			return min;
		}



		public void Init()
		{


            plus_map[0] = "";
            minus_map[0] = "";

			for(var i = 1; i < 256; i++)
			{
				plus_map[i] = plus_map[i - 1] + "+";
				minus_map[i] = minus_map[i - 1] + "-";
			}

			// initial state for map[x][y]: go from x to y using +s or -s.                                
			for(var x = 0; x < 256; x++)
			{
				for(var y = 0; y < 256; y++)
				{
					var delta = y - x;

					if(delta > 128)
					{
						delta -= 256;
					}
					if(delta < -128)
					{
						delta += 256;
					}

					if(delta >= 0) 
					{
						map[x,y] = plus_map[delta];
					}
					else
					{
						map[x,y] = minus_map[-delta];
					}
				}
			}
		}



//		public string generate(string str)
//		{
//            Init();
//			int last = 0;
//			int len = str.Length;
//			//char_map,
//			string result = "";
//			//memory = [0],
//			int index = 0;
//
//			for(var i = 0; i < len; i++)
//			{
//				//var 
//					// unicode not supported  //.charCodeAt(i)
//				int chr = (byte)str [i] & 255; 
//				string[] options = new string[] {
//					">" + map[0,chr],
//					//map[memory[index]][chr],
//					map[last, chr]
//				};
//					//shortest;
//
//				//if(index > 0)
//				//{
//				//	options.push("[-]<" + map[memory[index - 1]][chr]);
//				//	//console.log(map[memory[index - 1]][chr].length - map[memory[index]][chr])
//				//}
//
//				int shortest = shortest_str(options);
//
//				result += options[shortest] + ".";
//
//				/*if(shortest === 0)
//					{
//						index++;
//						memory[index] = chr;
//					}
//					else if(shortest === 1)
//					{
//						memory[index] = chr;
//					}
//					else 
//					{
//						memory[index] = 0;
//						//memory.pop();
//						index--;
//						memory[index] = chr;
//					}*/
//
//				last = chr;
//			}
//
//			return result;
//		}
////
//        public string do_generate(string text)
//        {
//            if (repeat > 0)
//            {
//                return string.Empty;
//            }
//
//            //var x = 0;
//            //for(var i = 0; i < 256; i++)
//            //	for(var j = 0; j < 256; j++)
//            //		x += map[i][j].length;
//
//            //console.log(x);
//            //console.assert(x == 807481);
//
//            var code = generate(text);
//
//            return code;
//
//            //info.textContent =
//            //    "text length = " + text.length + " bytes\n" +
//            //        "code length = " + code.length + " bytes\n" +
//            //        "ratio = " + (code.length / (text.length || 1)).toFixed(2);
//        }
//
//


    }
}



//-------------------------------------------



namespace Programmer
{
    /// <summary>
    /// This is the bf interpreter.
    /// 
    /// > 	Increment the pointer.
    /// < 	Decrement the pointer.
    /// + 	Increment the byte at the pointer.
    /// - 	Decrement the byte at the pointer.
    /// . 	Output the byte at the pointer.
    /// , 	Input a byte and store it in the byte at the pointer.
    /// [ 	Jump forward past the matching ] if the byte at the pointer is zero.
    /// ] 	Jump backward to the matching [ unless the byte at the pointer is zero.
    /// 
    /// Extended commands, included in BrainPlus.
    /// @   Exits the program or if inside a function, return to last position in main program and restore state.
    /// $   Overwrites the byte in storage with the byte at the pointer.
    /// !   Overwrites the byte at the pointer with the byte in storage.
    /// a,b Call function a - z.
    /// 0-F Sets the value of the current memory pointer to a multiple of 16.
    /// </summary>
    public class Interpreter
    {
        /// <summary>
        /// Object used to swap state for a function call. This data is restored when the function terminates.
        /// </summary>
        public class FunctionCallObj
        {
            public int InstructionPointer { get; set; }
            public int DataPointer { get; set; }
            public int FunctionInputPointer { get; set; }
            public Stack<int> CallStack { get; set; }
            public bool ExitLoop { get; set; }
            public int ExitLoopInstructionPointer { get; set; }
            public int Ticks { get; set; }
        };

        /// <summary>
        /// The "call stack"
        /// </summary>
        private readonly Stack<int> m_CallStack = new Stack<int>();

        /// <summary>
        /// The input function
        /// </summary>
        private readonly Func<byte> m_Input;

        /// <summary>
        /// The instruction set
        /// </summary>
        private readonly IDictionary<char, Action> m_InstructionSet =
            new Dictionary<char, Action>();

        /// <summary>
        /// The memory of the program
        /// </summary>
        private readonly byte[] m_Memory = new byte[32768];
        //private readonly byte[] m_Memory = new byte[65535];
        /// <summary>
        /// The output function
        /// </summary>
        private readonly Action<byte> m_Output;

        /// <summary>
        /// The program code
        /// </summary>
        private readonly char[] m_Source;

        /// <summary>
        /// The data pointer
        /// </summary>
        private int m_DataPointer;

        /// <summary>
        /// The instruction pointer
        /// </summary>
        private int m_InstructionPointer;

        /// <summary>
        /// Boolean flag to indicate if we should skip the loop and continue execution at the next valid instruction. Used if the pointer is zero and a begin loop [ instruction is read, in which case we jump forward past the matching ].
        /// </summary>
        private bool m_ExitLoop;

        /// <summary>
        /// Holds the instruction pointer for the start of the loop. Used to bypass all inner-loops when searching for the end of the current loop.
        /// </summary>
        private int m_ExitLoopInstructionPointer;

        /// <summary>
        /// The list of functions and their starting instruction index.
        /// </summary>
        private readonly Dictionary<char, int> m_Functions = new Dictionary<char, int>();

        /// <summary>
        /// Identifier for next function. Will serve as the instruction to call this function.
        /// </summary>
        private char m_NextFunctionCharacter = 'a';

        /// <summary>
        /// The function "call stack".
        /// </summary>
        public readonly Stack<FunctionCallObj> m_FunctionCallStack = new Stack<FunctionCallObj>();

        /// <summary>
        /// Pointer to the current call stack (m_FunctionCallStack or m_CallStack).
        /// </summary>
        private Stack<int> m_CurrentCallStack;

        /// <summary>
        /// Pointer to a function's parent memory. When an input (,) command is executed from within a function, the function's current memory cell gets a copy of the value of the parent memory at this pointer. This allows passing multiple values as input to a function.
        /// For example: ++>++++>+<<a!.@,>,-[-<+>]<+$@
        /// Parent memory contains: 2, 4, 1. Function will contain: 2, 4 and store a value of 6 in storage. Resulting parent memory remains: 2, 4, 1. Upon next command !, parent memory will contain: 6, 4, 1. The value 6 is then displayed as output.
        /// </summary>
        private int m_FunctionInputPointer;

        /// <summary>
        /// Number of cells available to functions. When a function is executed, an array of cells are allocated in upper-addresses (eg., 1000-1999, 2000-2999, etc.) for usage.
        /// </summary>
        private const int _functionSize = 300;

        /// <summary>
        /// Storage memory value. Usually used to hold return values from function calls.
        /// </summary>
        private byte m_Storage;

        /// <summary>
        /// Number of instructions executed.
        /// </summary>
        public int m_Ticks;

        /// <summary>
        /// Flag to stop execution of the program.
        /// </summary>
        public bool m_Stop;

        /// <summary>
        /// Read-only access to the current data pointer index in memory.
        /// </summary>
        public int m_CurrentDataPointer { get { return m_DataPointer; } }

        /// <summary>
        /// Read-only access to the current instruction pointer index.
        /// </summary>
        public int m_CurrentInstructionPointer { get { return m_InstructionPointer; } }

        /// <summary>
        /// List of executed functions in the main program. Used for reference purposes by the GA to determine which functions were executed in the program (not functions calling other functions).
        /// </summary>
        public Dictionary<char, int> m_ExecutedFunctions = new Dictionary<char, int>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="programCode"></param>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public Interpreter(string programCode, Func<byte> input, Action<byte> output, Action<char> function = null)
        {
            // Save the program code
            this.m_Source = programCode.ToCharArray();

            // Store the i/o delegates
            this.m_Input = input;
            this.m_Output = output;
            
            m_CurrentCallStack = m_CallStack;

            // Create the instruction set for Basic Bf.
            this.m_InstructionSet.Add('+', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer]++; });
            this.m_InstructionSet.Add('-', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer]--; });

            ////todo arithmetic overflow
            //this.m_InstructionSet.Add('+', () => { if (!m_ExitLoop && this.m_Memory[this.m_DataPointer] < 255) this.m_Memory[this.m_DataPointer]++; });
            //this.m_InstructionSet.Add('-', () => { if (!m_ExitLoop && this.m_Memory[this.m_DataPointer] > 0) this.m_Memory[this.m_DataPointer]--; });


            this.m_InstructionSet.Add('>', () => { if (!m_ExitLoop) this.m_DataPointer++; });
            this.m_InstructionSet.Add('<', () => { if (!m_ExitLoop) this.m_DataPointer--; });

            this.m_InstructionSet.Add('.', () => { if (!m_ExitLoop) this.m_Output(this.m_Memory[this.m_DataPointer]); });

            // Prompt for input. If inside a function, pull input from parent memory, using the current FunctionInputPointer. Each call for input advances the parent memory cell that gets read from, allowing the passing of multiple values as input to a function.
            this.m_InstructionSet.Add(',', () => { if (!m_ExitLoop) m_Memory[this.m_DataPointer] = m_FunctionCallStack.Count == 0 ? this.m_Input() : this.m_Memory[this.m_FunctionInputPointer++]; });

            this.m_InstructionSet.Add('[', () =>
            {
                if (!m_ExitLoop && this.m_Memory[this.m_DataPointer] == 0)
                {
                    // Jump forward to the matching ] and exit this loop (skip over all inner loops).
                    m_ExitLoop = true;

                    // Remember this instruction pointer, so when we get past all inner loops and finally pop this one off the stack, we know we're done.
                    m_ExitLoopInstructionPointer = this.m_InstructionPointer;
                }

                this.m_CurrentCallStack.Push(this.m_InstructionPointer);
            });
            this.m_InstructionSet.Add(']', () =>
            {
                var temp = this.m_CurrentCallStack.Pop();

                if (!m_ExitLoop)
                {
                    this.m_InstructionPointer = this.m_Memory[this.m_DataPointer] != 0
                        ? temp - 1
                        : this.m_InstructionPointer;
                }
                else
                {
                    // Continue executing after loop.
                    if (temp == m_ExitLoopInstructionPointer)
                    {
                        // We've finally exited the loop.
                        m_ExitLoop = false;
                        m_ExitLoopInstructionPointer = 0;
                    }
                }
            });

            //// Create the instruction set for Bf Extended Type 3.
            //this.m_InstructionSet.Add('0', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 0; });
            //this.m_InstructionSet.Add('1', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 16; });
            //this.m_InstructionSet.Add('2', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 32; });
            //this.m_InstructionSet.Add('3', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 48; });
            //this.m_InstructionSet.Add('4', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 64; });
            //this.m_InstructionSet.Add('5', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 80; });
            //this.m_InstructionSet.Add('6', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 96; });
            //this.m_InstructionSet.Add('7', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 112; });
            //this.m_InstructionSet.Add('8', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 128; });
            //this.m_InstructionSet.Add('9', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 144; });
            //this.m_InstructionSet.Add('A', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 160; });
            //this.m_InstructionSet.Add('B', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 176; });
            //this.m_InstructionSet.Add('C', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 192; });
            //this.m_InstructionSet.Add('D', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 208; });
            //this.m_InstructionSet.Add('E', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 224; });
            //this.m_InstructionSet.Add('F', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = 240; });
            //this.m_InstructionSet.Add('@', () => 
            //{
            //    if (m_FunctionCallStack.Count > 0)
            //    {
            //        // Exit function.
            //        var temp = m_FunctionCallStack.Pop();

            //        // Restore the data pointer.
            //        this.m_DataPointer = temp.DataPointer;
            //        // Restore the call stack.
            //        this.m_CurrentCallStack = temp.CallStack;
            //        // Restore exit loop status.
            //        this.m_ExitLoop = temp.ExitLoop;
            //        // Restore exit loop instruction pointer.
            //        this.m_ExitLoopInstructionPointer = temp.ExitLoopInstructionPointer;
            //        // Restore ticks.
            //        this.m_Ticks = temp.Ticks;
            //        // Restore the instruction pointer.
            //        this.m_InstructionPointer = temp.InstructionPointer;
            //        // Restore function input pointer.
            //        this.m_FunctionInputPointer = temp.FunctionInputPointer;
            //    }
            //    else
            //    {
            //        // Exit program.
            //        this.m_Stop = true;
            //    }
            //});
            //this.m_InstructionSet.Add('$', () => { if (!m_ExitLoop) this.m_Storage = this.m_Memory[this.m_DataPointer]; });
            //this.m_InstructionSet.Add('!', () => { if (!m_ExitLoop) this.m_Memory[this.m_DataPointer] = this.m_Storage; });

            // Scan code for function definitions and store their starting memory addresses.
            ScanFunctions(programCode);

            // If we found any functions, create the instruction set for them.
            for (char inst = 'a'; inst < m_NextFunctionCharacter; inst++)
            {
                char instruction = inst; // closure
                this.m_InstructionSet.Add(instruction, () =>
                {
                    if (!m_ExitLoop)
                    {
                        // Record a list of executed function names from the main program (not a function calling another function).
                        if (m_FunctionCallStack.Count == 0)
                        {
                            if (m_ExecutedFunctions.ContainsKey(instruction))
                            {
                                m_ExecutedFunctions[instruction]++;
                            }
                            else
                            {
                                m_ExecutedFunctions.Add(instruction, 1);
                            }
                        }

                        if (function != null)
                        {
                            function(instruction);
                        }

                        // Store the current instruction pointer and data pointer before we move to the function.
                        var functionCallObj = new FunctionCallObj { InstructionPointer = this.m_InstructionPointer, DataPointer = this.m_DataPointer, FunctionInputPointer = this.m_FunctionInputPointer, CallStack = this.m_CurrentCallStack, ExitLoop = this.m_ExitLoop, ExitLoopInstructionPointer = this.m_ExitLoopInstructionPointer, Ticks = this.m_Ticks };
                        this.m_FunctionCallStack.Push(functionCallObj);

                        // Give the function a fresh call stack.
                        this.m_CurrentCallStack = new Stack<int>();
                        this.m_ExitLoop = false;
                        this.m_ExitLoopInstructionPointer = 0;

                        // Set the function input pointer to the parent's starting memory. Calls for input (,) from within the function will read from parent's memory, each call advances the parent memory cell that gets read from. This allows passing multiple values to a function.
                        this.m_FunctionInputPointer = this.m_DataPointer;

                        // Set the data pointer to the functions starting memory address.
                        this.m_DataPointer = _functionSize * (instruction - 96); // each function gets a space of 1000 memory slots.

                        // Clear function memory.
                        Array.Clear(this.m_Memory, this.m_DataPointer, _functionSize);

                        // Set the instruction pointer to the beginning of the function.
                        this.m_InstructionPointer = m_Functions[instruction];
                    }
                });
            }
        }

        /// <summary>
        /// Run the program
        /// </summary>
        public void Run(int maxInstructions = 0)
        {
            m_Ticks = 0;
            m_Stop = false;

            if (maxInstructions > 0)
            {
                RunLimited(maxInstructions);
            }
            else
            {
                RunUnlimited();
            }
        }

        /// <summary>
        /// Run the program with a maximum number of instructions before throwing an exception. Avoids infinite loops.
        /// </summary>
        /// <param name="maxInstructions">Max number of instructions to execute</param>
        private void RunLimited(int maxInstructions)
        {
            // Iterate through the whole program source
            while (this.m_InstructionPointer < this.m_Source.Length && !m_Stop)
            {
                // Fetch the next instruction
                char instruction = this.m_Source[this.m_InstructionPointer];

                // See if that IS an instruction and execute it if so
                Action action;
                if (this.m_InstructionSet.TryGetValue(instruction, out action))
                {
                    // Yes, it was - execute
                    action();
                }

                // Next instruction
                this.m_InstructionPointer++;

                // Have we exceeded the max instruction count?
                if (maxInstructions > 0 && m_Ticks >= maxInstructions)
                {
                    if (m_FunctionCallStack.Count > 0)
                    {
                        // We're inside a function, but ran out of instructions. Exit the function, but continue.
                        if (this.m_InstructionSet.TryGetValue('@', out action))
                        {
                            action();
                            this.m_InstructionPointer++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                m_Ticks++;
            }
        }

        /// <summary>
        /// Run the program
        /// </summary>
        private void RunUnlimited()
        {
            // Iterate through the whole program source
            while (this.m_InstructionPointer < this.m_Source.Length && !m_Stop)
            {
                // Fetch the next instruction
                char instruction = this.m_Source[this.m_InstructionPointer];

                // See if that IS an instruction and execute it if so
                Action action;
                if (this.m_InstructionSet.TryGetValue(instruction, out action))
                {
                    // Yes, it was - execute
                    action();
                }

                // Next instruction
                this.m_InstructionPointer++;

                m_Ticks++;
            }
        }

        /// <summary>
        /// Pre-scan the program code to record function instruction pointers.
        /// </summary>
        private void ScanFunctions(string source)
        {
            this.m_InstructionPointer = source.IndexOf('@');
            while (this.m_InstructionPointer > -1 && this.m_InstructionPointer < source.Length - 1 && !m_Stop)
            {
                // Store the function.
                m_Functions.Add(m_NextFunctionCharacter++, this.m_InstructionPointer);

                this.m_InstructionPointer = source.IndexOf('@', this.m_InstructionPointer + 1);
            }

            this.m_InstructionPointer = 0;
        }
    }
}
