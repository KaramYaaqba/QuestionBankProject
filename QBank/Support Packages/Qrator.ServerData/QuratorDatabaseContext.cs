using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Qrator.ServerData.Entities
{
    public partial class QBankEntities : DbContext
    {
        public QBankEntities(string connString)
            : base(connString)
        {
        }
    }
}
