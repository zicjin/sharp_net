using System;

namespace sharp_net {
    /// <summary>
    ///这个类的功能：Dispose()方法只会执行被override的DisposeCustom方法中的用户自定义的内容，而不会触发析构函数。
    ///析构函数在这里只会被垃圾回收器执行。另一个功能是保证DisposeCustom方法只被执行一次
    ///理论：托管资源是不需要实现IDisposable接口去手动清理的，CRL本身就能够用非常成熟的方式去释放托管资源，不会造成内存浪费。
    ///反而，在手动释放非托管资源时如果顺带清理了托管资源是一种性能浪费（C#-Dispose()默认既是如此处理）
    /// </summary>
    public class Disposable : IDisposable {

        //析构函数Finalize()是在对象真正被回收时调用，它无法被override
        //Dispose()用于手动触发析构函数，且支持using语法。
        //~Disposable用于垃圾回收器自动回收时触发（如果对象之前已被Dispose()回收过，则不会再被垃圾回收器处理）
        public void Dispose() {
            ClearUp(true);
            //使对象跳过Finalize()析构函数
            GC.SuppressFinalize(this);
        }

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