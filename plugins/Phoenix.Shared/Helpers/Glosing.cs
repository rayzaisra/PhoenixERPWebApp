using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Shared.Helpers
{
    public class Glosing
    {
        private static Glosing _instance;

        private Glosing() { }

        public static Glosing Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Glosing();

                return _instance;
            }
        }

        public string Username { get; set; }
        public string EmployeeId { get; set; }
 
    }
}
