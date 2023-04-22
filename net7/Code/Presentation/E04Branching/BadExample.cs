namespace Code.Presentation.E04Branching;

public sealed class BadExample
{
    public sealed class Service
    {
        public bool Process(int? p1, int p2, int p3)
        {
            if (p1 is not null)
            {
                ProcessA(p1.Value);
                if (p2 > 0 && p3 != p2)
                {
                    ProcessB(p2, p3);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void ProcessA(int p)
        {

        }

        private void ProcessB(int p1, int p2)
        {

        }

    }
}