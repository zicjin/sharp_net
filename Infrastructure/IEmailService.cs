using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp_net.Infrastructure {
    public interface IEmailService {

        void SendEmail(string toAddress, string subject, string content, bool isRich);
    }
}
