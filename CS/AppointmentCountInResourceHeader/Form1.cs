using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Drawing;

namespace WindowsApplication47 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            AddResources();
        }

        private void AddResources() {
            for(int i = 0; i < 3; i++) {
                Resource res = schedulerStorage1.CreateResource(i);
                res.Caption = "Resource " + i.ToString();
                schedulerStorage1.Resources.Add(res);
            }
            
        }

        private void schedulerControl1_CustomDrawResourceHeader(object sender, CustomDrawObjectEventArgs e) {
            ResourceHeader rh = e.ObjectInfo as ResourceHeader;
            if(rh != null) {
            TimeInterval visTime = schedulerControl1.ActiveView.GetVisibleIntervals().Interval;
            int appointments_Count = CountAppointmentsByCriteria(rh, visTime);
            rh.Caption = rh.Resource.Caption + " has " + appointments_Count.ToString() + " apts"; 
            }
        }

        private void schedulerControl1_CustomDrawDayHeader(object sender, CustomDrawObjectEventArgs e) {
            DayHeader dh = e.ObjectInfo as DayHeader;
            if(dh != null) {
                TimeInterval visTime = dh.Interval;
            int appointments_Count = CountAppointmentsByCriteria(dh, visTime);
                dh.Caption = String.Format("{0:MMM dd} has {1} apts", dh.Interval.Start.Date,appointments_Count );
            }
        }

        private int CountAppointmentsByCriteria(SchedulerHeader rh, TimeInterval visTime) {
            AppointmentBaseCollection col = schedulerControl1.ActiveView.GetAppointments().FindAll(delegate(Appointment apt) { return (apt.ResourceIds.Contains(rh.Resource.Id) || (Object.Equals(apt.ResourceId, ResourceEmpty.Id))) && (visTime.Contains(new TimeInterval(apt.Start, apt.End))); });
            return col.Count;
        }

    }
}