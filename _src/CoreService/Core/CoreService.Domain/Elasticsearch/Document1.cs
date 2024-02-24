using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.Elasticsearch
{
    public class Document1
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Text]
        public string Name { get; set; } = string.Empty;
    }
}
