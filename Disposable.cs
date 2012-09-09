using System;

namespace sharp_net {

    public class Disposable : IDisposable {

        public void Dispose() {
            ClearUp(true);
            //使对象跳过Finalize()析构函数
            GC.SuppressFinalize(this);
        }

        //析构函数为Finalize，但override Finalize()无法编译
        //析构函数是在本对象真正被回收时调用，Dispose()只是能够触发析构函数，且支持using语法
        //如果对象未被手动调用Dispose()，则垃圾回收器会在回收时调用这个方法
        ~Disposable() {
            ClearUp(false);
        }

        private bool IsDisposed;

        private void ClearUp(bool isManual) {
            //保证只执行一次对象回收，防止用户手动调用多次Dispose()
            if (!IsDisposed) {
                if (isManual)
                    DisposeCustom();
                IsDisposed = true;
            }
        }

        /// <summary>
        /// 精确释放与本对象相关的资源，或执行相关任务
        /// </summary>
        protected virtual void DisposeCustom() {
        }
    }
}