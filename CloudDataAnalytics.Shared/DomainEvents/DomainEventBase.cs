using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CloudDataAnalytics.Shared.DomainEvents
{
    [DataContract]
    public class DomainEventBase : IDomainEvent
    {
        [DataMember]
        public string Id
        {
            get;
            set;
        }
    }
}
