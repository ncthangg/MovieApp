using MovieApp.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Service
{
    public class HelperWrapper
    {
        public IEmailHelper EmailHelper { get; set; }
        public ITokenHelper TokenHelper { get; set; }
        public IPasswordHelper PasswordHelper { get; set; }

        public HelperWrapper(
            IEmailHelper emailHelper,
            ITokenHelper tokenHelper,
            IPasswordHelper passwordHelper
            )
        {
            EmailHelper = emailHelper;
            TokenHelper = tokenHelper;
            PasswordHelper = passwordHelper;
        }
    }
}
