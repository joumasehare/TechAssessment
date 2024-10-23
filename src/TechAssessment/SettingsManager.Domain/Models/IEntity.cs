using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsManager.Domain.Models
{
    public interface IEntity<TIdentifier>
    {
        public TIdentifier Id {get; set; }
    }
}
