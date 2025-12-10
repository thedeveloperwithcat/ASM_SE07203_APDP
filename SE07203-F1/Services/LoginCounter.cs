namespace SE07203_F1.Services
{
    public class LoginCounter
    {
        private int _count = 0;

        public void Increase()
        {
            _count++;
        }

        public int GetCount()
        {
            return _count;
        }
    }
}
