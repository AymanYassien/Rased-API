using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Transfer
{
    public class AddStaticReceiverTypeDataDto
    {
        public string ReceiverTypeName { get; set; }
    }

    public class UpdateStaticReceiverTypeDataDto
    {
        public int ReceiverTypeId { get; set; }
        public string ReceiverTypeName { get; set; }
    }

    public class ReadStaticReceiverTypeDataDto
    {
        public int ReceiverTypeId { get; set; }
        public string ReceiverTypeName { get; set; }
    }

}
