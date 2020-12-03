using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.Exceptions
{
    public class BusinessLogic : Exception
    {
        public BusinessLogic()
        {

        }

        public BusinessLogic(string message):base(message)
        {

        }
    }
}
