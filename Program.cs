/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading;

namespace HiSystems.Interpreter
{
    public class Program
    {
        #region Initialize

        public static Engine engine;

        public static readonly Dictionary<UInt32, Assembly> Assemblies = new Dictionary<UInt32, Assembly>();
        public static readonly List<Function> Functions = new List<Function>();
        public static readonly List<Command> Commands = new List<Command>();
        public static readonly List<Tuple<While, Thread>> Threads = new List<Tuple<While, Thread>>();
        public static readonly List<Action> Actions = new List<Action>();
        public static List<Variable> Variables = new List<Variable>();

        public static bool Waiting = true;
       
        public static Spinner spin;

        public static bool _Mono { get { return Type.GetType("Mono.Runtime") != null; } }

        #endregion
 
        private static int Main(string[] args)
        {
            #region Start Spinner

            spin = new Spinner();

            ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(delegate(object state)
            {
                while (Waiting)
                {
                    spin.Turn();
                    Sleep.Milliseconds(80);
                }
            }), null);

            #endregion

            #region Modify ThreadPool

            int minWorker, minIOC;
            // Get the current settings.
            ThreadPool.GetMinThreads(out minWorker, out minIOC);
            // Change the minimum number of worker threads to four, but
            // keep the old setting for minimum asynchronous I/O 
            // completion threads.
            if (ThreadPool.SetMinThreads(13, minIOC))
            {
                //Output.Text("The minimum number of threads was set successfully.");
            }
            else
            {
                Output.Text("The minimum number of threads was not changed.");
            }

            #endregion

            #region Load Assemblies & Register Types
            
            AppDomain _AppDomain = AppDomain.CurrentDomain;
            Assembly _Assembly = Assembly.GetExecutingAssembly();

            //set the assembly resolver
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            //add self
            Assemblies[Fasthash.Execute(_Assembly.GetName().FullName)] = _Assembly;

            //embeded assemblies
            foreach (var resourceName in _Assembly.GetManifestResourceNames().Where(x => x.EndsWith(".dll")))
            {
                Stream _Stream = _Assembly.GetManifestResourceStream(resourceName);
                byte[] _assembly = new byte[_Stream.Length];
                _Stream.Read(_assembly, 0, (int)_Stream.Length);
                //todo Assembly.Load(Deflate.Decompress(_assembly));
                Assemblies[Fasthash.Execute(resourceName)] = Assembly.Load(_assembly);
            }

            //file assemblies
            foreach (string dllPath in Directory.GetFiles(_AppDomain.BaseDirectory, "*.dll"))
            {
                var _AssemblyName = AssemblyName.GetAssemblyName(dllPath);

                Assemblies[Fasthash.Execute(_AssemblyName.Name)] = _AppDomain.Load(_AssemblyName);
            }

            //register types
            foreach (var assembly in Assemblies)
            {
                RegisterTypes(assembly.Value.GetTypes());
            }

            #endregion

            #region Start the Engine

            engine = new Engine();

            Parse.Execute("alias(ls, dir)");
            Parse.Execute("alias(rm, del)");
            Parse.Execute("alias(cp, copy)");
            Parse.Execute("alias(mv, rename)");             
            Parse.Execute("alias(\"_Input\", Log(KeyboardInput()))"); //KeyboardInput() is in the Console plugin so we're making the assumption...
            Parse.Execute("alias(\"_Parse\", Parse())"); //Parse() with no argument triggers Parse(_Input)
            Parse.Execute("Output(Notice(\"ready\"))");
            Parse.Execute("While(true, Parse())");
            
            #endregion

            Waiting = false; //stop the spinner
            Output.Text("\rNew Shell process 1");
            
            return 0; //startup complete
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly[] currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < currentAssemblies.Length; i++)
            {
                if (currentAssemblies[i].FullName == args.Name)
                {
                    return currentAssemblies[i];
                }
            }

            return null;
        }

        public static void RegisterTypes(Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                if (!types[i].IsAbstract && !types[i].IsInterface)
                {
                    switch (types[i].BaseType.Name)
                    {
                        case "Function":
                            var function = Functions.FirstOrDefault(x => x.Name == types[i].Name);
                            if (function != null)
                                Functions.Remove(function);

                            Functions.Add((Function)Activator.CreateInstance(types[i]));
                            break;
                        case "Command":
                            var command = Commands.FirstOrDefault(x => x.Name == types[i].Name);
                            if (command != null)
                                Commands.Remove(command);

                            Commands.Add((Command)Activator.CreateInstance(types[i]));
                            break;
                        case "Action":
                            var action = Actions.FirstOrDefault(x => x.Name == types[i].Name);
                            if (action != null)
                                Actions.Remove(action);

                            Actions.Add((Action)Activator.CreateInstance(types[i]));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

    }
}