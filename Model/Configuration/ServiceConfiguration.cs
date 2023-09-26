using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Configuration
{
    public class ServiceConfiguration : List<Service>
    {
        public Service? this[string name] => this.FirstOrDefault(e => e.Name == name);
    }
}
