using System.Collections.Generic;

namespace ContactManager.Model
{
    class Countries
    {
        private static readonly List<string> _names;

        static Countries()
        {
            _names = new List<string>(50);

            _names.Add("Britain");
            _names.Add("China");
            _names.Add("German");
            _names.Add("Japan");
            _names.Add("Korea");
            _names.Add("United State");

        }

        public static IList<string> GetNames()
        {
            return _names;
        }
    }
}
