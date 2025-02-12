using LibreHardwareMonitor.Hardware;

namespace JiaoLongWMI.Models
{
    public class UpdateVisitor : IVisitor
    {

        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }

        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            IHardware[] subHardware = hardware.SubHardware;
            for (int i = 0; i < subHardware.Length; i++)
            {
                subHardware[i].Accept(this);
            }
        }
        
        public void VisitSensor(ISensor sensor)
        {
        }
        public void VisitParameter(IParameter parameter)
        {
        }
    }
}