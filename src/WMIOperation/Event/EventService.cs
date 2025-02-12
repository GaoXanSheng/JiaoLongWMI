using System.Management;
using System.Timers;
using Timer = System.Threading.Timer;

namespace JiaoLongWMI.WMIOperation.Event;

public class EventService
  {
    private ManagementEventWatcher _watcher;
    private Timer _viewClock;
    private byte times;

    public event EventService.WMIEventArrivedEventHandler _WMIEventArrived;

    public bool InitAndStart(
      EventService.WMIEventArrivedEventHandler wMIEventArrived)
    {
      try
      {
        this._watcher = new ManagementEventWatcher("root\\WMI", "SELECT * FROM HID_EVENT20");
        Console.WriteLine("Waiting for an event...");
        this._watcher.EventArrived += new EventArrivedEventHandler(this.Watcher_EventArrived);
        this._WMIEventArrived += wMIEventArrived;
        this._watcher.Start();
        return true;
      }
      catch (ManagementException ex)
      {
        Console.WriteLine("An error occurred while trying to receive an event: " + ex.Message);
        return false;
      }
    }

    protected void viewTimer(object sender, ElapsedEventArgs e)
    {
      byte[] numArray = new byte[8];
      numArray[0] = (byte) 1;
      numArray[1] = (byte) 15;
      numArray[2] = this.times;
      EventType wMIEventType = (EventType) numArray[0];
      EventName wMIEventName = (EventName) numArray[1];
      object eVENTvalue = wMIEventName == EventName.CPUFanSpeed || wMIEventName == EventName.GPUFanSpeed ? (object) (((int) numArray[2] << 8) + (int) numArray[3]) : (object) numArray[2];
      Console.WriteLine("outdata:===============");
      for (int index = 0; index < numArray.Length; ++index)
        Console.WriteLine((int) numArray[index]);
      if (this._WMIEventArrived != null)
        this._WMIEventArrived(wMIEventType, wMIEventName, eVENTvalue);
      ++this.times;
      if (this.times != (byte) 3)
        return;
      this.times = (byte) 0;
    }

    public void Stop()
    {
      if (this._watcher != null)
      {
        this._watcher.Stop();
        this._watcher.Dispose();
      }
      this._watcher.EventArrived -= new EventArrivedEventHandler(this.Watcher_EventArrived);
      this._watcher.Dispose();
    }

    private void Watcher_EventArrived(object sender, EventArrivedEventArgs e)
    {
      Console.WriteLine((object) e.NewEvent);
      byte[] numArray = e.NewEvent["EventDetail"] as byte[];
      EventType wMIEventType = (EventType) numArray[0];
      EventName wMIEventName = (EventName) numArray[1];
      object eVENTvalue = wMIEventName == EventName.CPUFanSpeed || wMIEventName == EventName.GPUFanSpeed ? (object) (((int) numArray[2] << 8) + (int) numArray[3]) : (object) numArray[2];
      if (this._WMIEventArrived != null)
      {
        this._WMIEventArrived(wMIEventType, wMIEventName, eVENTvalue);
      }
    }

    public delegate void WMIEventArrivedEventHandler(
      EventType wMIEventType,
      EventName wMIEventName,
      object eVENTvalue);
  }