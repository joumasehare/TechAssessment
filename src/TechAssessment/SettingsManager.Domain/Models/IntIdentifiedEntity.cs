using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsManager.Domain.Models
{
    public abstract class IntIdentifiedEntity : IEntity<int>
    {
        public int Id { get; set; }
    }
}
