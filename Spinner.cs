/*
 *
 * User: github.com/marc365
 * Created: 2016
 */


namespace HiSystems.Interpreter
{
    public class Spinner
    {
        int counter;
        public Spinner()
        {
            counter = 0;
        }
        public void Turn()
        {
            counter++;
            switch (counter % 4)
            {
                case 0:
                    System.Console.Write("\r/");
                    return;
                case 1:
                    System.Console.Write("\r-");
                    return;
                case 2:
                    System.Console.Write("\r\\");
                    return;
                case 3:
                    System.Console.Write("\r|");
                    return;
            }
        }
    }
}