using LibreHardwareMonitor.Hardware;

namespace JiaoLongWMI.Models
{
    public class UpdateVisitor : IVisitor
    {
        // Token: 0x060000F3 RID: 243 RVA: 0x0000850B File Offset: 0x0000670B
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }

        // Token: 0x060000F4 RID: 244 RVA: 0x00008514 File Offset: 0x00006714
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            IHardware[] subHardware = hardware.SubHardware;
            for (int i = 0; i < subHardware.Length; i++)
            {
                subHardware[i].Accept(this);
            }
        }

        // Token: 0x060000F5 RID: 245 RVA: 0x00008545 File Offset: 0x00006745
        public void VisitSensor(ISensor sensor)
        {
        }

        // Token: 0x060000F6 RID: 246 RVA: 0x00008547 File Offset: 0x00006747
        public void VisitParameter(IParameter parameter)
        {
        }
    }
}