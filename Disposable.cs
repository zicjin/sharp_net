using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zic_dotnet {
    public class Disposable : IDisposable {

        public void Dispose() {
            ClearUp(true);
            //使对象跳过Finalize()析构函数
            GC.SuppressFinalize(this);
        }

        //析构函数。使用override Finalize()无法编译
        //析构函数是在本对象真正被回收时调用，Dispose()仅仅是一个用户回收资源的约定方法名
        //如果对象未被调用Dispose()，则垃圾回收器会在回收时调用这个方法
        ~Disposable() {
            ClearUp(false);
        }

        private bool IsDisposed;

        private void ClearUp(bool isManual) {
            //报站只执行一次对象回收，防止用户调用多次Dispose()的资源消耗
            if (!IsDisposed) {
                if (isManual)
                    DisposeHostobj();
                DisposeDeHostobj();
            }
            IsDisposed = true;
        }

        /// <summary>
        /// 释放额外的托管资源，一般是本对象的内部字段
        /// </summary>
        protected virtual void DisposeHostobj() {

        }

        /// <summary>
        /// 释放非托管资源
        /// </summary>
        protected virtual void DisposeDeHostobj() {

        }

    }
}