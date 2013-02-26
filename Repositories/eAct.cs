namespace sharp_net.Repositories {

    public enum eAct {
        Normal = 1,
        Delete = 2,
        Freeze = 3,
        unApproved = 4,
        Reported = 5
    }

    public class ActHelp{
        public static bool CouldShow(eAct act) {
            return (act == eAct.Normal || act == eAct.Reported);
        }
    }
}