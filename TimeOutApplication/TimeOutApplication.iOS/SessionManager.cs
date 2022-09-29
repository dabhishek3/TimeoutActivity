using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace TimeOutApplication.iOS
{
    public class SessionManagerApp : UIApplication
    {
        public SessionManagerApp() : base()
        {
        }
        public SessionManagerApp(IntPtr handle) : base(handle)
        {
        }
        public SessionManagerApp(Foundation.NSObjectFlag t) : base(t)
        {
        }
        public override void SendEvent(UIEvent uievent)
        {
            if (uievent.Type == UIEventType.Touches)
            {
                if (uievent.AllTouches.Cast<UITouch>().Any(t => t.Phase == UITouchPhase.Began))
                {
                    SessionManager.Instance.ExtendSession();
                    Console.WriteLine("Touch detected. Extending user session");
                }
            }
            base.SendEvent(uievent);
        }
    }
}