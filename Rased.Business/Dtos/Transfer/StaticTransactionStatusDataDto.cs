using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Transfer
{

    public class AddStaticTransactionStatusDto
    {
        public string Name { get; set; }
    }

    public class UpdateStaticTransactionStatusDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ReadStaticTransactionStatusDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


}
