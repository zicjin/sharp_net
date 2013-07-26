using MongoDB.Bson;
using System.Collections.Generic;
namespace sharp_net.Repositories {

    public enum eAct {
        Normal = 1,
        Delete = 2,
        Freeze = 3,
        unApproved = 4,
        Reported = 5
    }

    public static class ActHelp {

        public static bool CouldShow(eAct act) {
            return (act == eAct.Normal || act == eAct.Reported);
        }

        public static int[] CouldShowArray {
            get {
                return new [] { (int)eAct.Normal, (int)eAct.Reported };
            }
        }

    }
}