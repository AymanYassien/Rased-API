using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Bills
{
    public class GeminiApiResponse
    {
        public List<Candidate>? Candidates { get; set; }
    }

    public class Candidate
    {
        public Content? Content { get; set; }
    }

    public class Content
    {
        public List<Part>? Parts { get; set; }
    }

    public class Part
    {
        public string? Text { get; set; }
    }

}
