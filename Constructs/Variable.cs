using System;
using System.Linq;

namespace HiSystems.Interpreter
{
    /// <summary>
    /// Represents a variable value that is resolved when first accessed.
    /// </summary>
    public class Variable : IConstruct
    {
        
        //public static List<Tuple<string, IConstruct>> _variableAliasList = new List<Tuple<string, IConstruct>>();
        private string name;
        public IConstruct Construct;

        public static int _current = 0;
        /// <summary>
        /// </summary>
        public Variable(string name)
            : this(name, null)
        {
        }

        /// <summary>
        /// </summary>
        public Variable(string name, IConstruct value)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("Name");

            this.name = name;
            this.Construct = value;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// The associated literal value that should be associated with this variable.
        /// This value should be set before the IConstruct.Transform() function is called.
        /// </summary>
        public IConstruct Value
        {
            get
            {
                return this.Construct;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                this.Construct = value;
            }
        }

        Literal IConstruct.Transform()
        {
            //foreach (var item in Program.Variables.ToList())
            //{
            //    if (item.Name == this.name.ToString())
            //    {
            //        //try
            //        //{
            //            _current++;

            //            var bit = item.Construct.Transform();
            //                return bit;
            //        //}
            //        //catch (Exception exc)
            //        //{
            //        //    return new Error(exc.Message);
            //        //}
            //    }

            //}

            if (this.Construct == null)
            {
                return new Text(this.name.ToString());
            }

            return this.Construct.Transform();

        }

        public override string ToString()
        {
            return this.name + ": " + (this.Construct == null ? String.Empty : this.Construct.ToString());
        }
    }
}