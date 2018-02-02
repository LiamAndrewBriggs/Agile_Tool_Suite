using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agile_Tool_Suite
{
    public class InputValidation
    {
        public static bool ValidatePassword (string p1, string p2)
        {
            bool pass = true;

            if(p1.Equals(p2) == false)
            {
                pass = false;
            }

            return pass;
        }

        public static bool ValidateName(String input)
        {
            bool pass = true;
            
            if(input.Trim().Length < 1)
            {
                pass = false;
            }

            return pass;
        }

        public static bool ValidateUserInput(String input)
        {
            bool pass = true;

            if (input.Trim().Length < 7 || input.Trim().Length > 17)
            {
                pass = false;
            }

            return pass;
        }

        public static bool ValidateEmail(String email)
        {
            bool pass = true;

            int index1 = email.IndexOf("@");
            int index2 = email.LastIndexOf("@");

            int num = email.Split('@').Length - 1;

            if (num > 1)
            {
                pass = false;
            }

            if (index1 != index2)
            {
                pass = false;
            }

            if (email.Trim() == "")
            {
                pass = false;
            }

            return pass;
        }

    }
}