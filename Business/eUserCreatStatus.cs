using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp_net {
    public enum eUserCreatStatus {
        Success = 0,
        InvalidUserKey = 1, //用户键值格式错误，键可以是email，也可以是其他唯一性字符串
        InvalidPassword = 2,
        InvalidQuestion = 3,
        InvalidAnswer = 4,
        DuplicateUserName = 5,
        DuplicateUserKey = 6,
        InvalidProviderUserKey = 7,
        UserRejected = 8, //未知错误，请求被驳回
        WaitUnitwork = 9,
        ProviderError = 10
    }
}