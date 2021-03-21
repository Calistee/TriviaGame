using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Services.Model
{
    public class User
    {
        public int Id { get; set; }
        public int Score { get; set; }

    }
}
