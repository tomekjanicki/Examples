namespace Code.Presentation.E04Branching;

public sealed class GoodExample
{
    public sealed class Service
    {
        public bool Process(int? p1, int p2, int p3)
        {
            if (p1 is null)
            {
                return false;
            }
            ProcessA(p1.Value);
            if (p2 <= 0 || p3 == p2)
            {
                return false;
            }
                
            ProcessB(p2, p3);

            return true;
        }

        private void ProcessA(int p)
        {

        }

        private void ProcessB(int p1, int p2)
        {

        }

    }
}