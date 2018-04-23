Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Drawing

Namespace WindowsApplication47
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
            AddResources()
        End Sub

        Private Sub AddResources()
            For i As Integer = 0 To 2
                Dim res As Resource = schedulerStorage1.CreateResource(i)
                res.Caption = "Resource " & i.ToString()
                schedulerStorage1.Resources.Add(res)
            Next i

        End Sub

        Private Sub schedulerControl1_CustomDrawResourceHeader(ByVal sender As Object, ByVal e As CustomDrawObjectEventArgs) Handles schedulerControl1.CustomDrawResourceHeader
            Dim rh As ResourceHeader = TryCast(e.ObjectInfo, ResourceHeader)
            If rh IsNot Nothing Then
            Dim visTime As TimeInterval = schedulerControl1.ActiveView.GetVisibleIntervals().Interval
            Dim appointments_Count As Integer = CountAppointmentsByCriteria(rh, visTime)
            rh.Caption = rh.Resource.Caption & " has " & appointments_Count.ToString() & " apts"
            End If
        End Sub

        Private Sub schedulerControl1_CustomDrawDayHeader(ByVal sender As Object, ByVal e As CustomDrawObjectEventArgs) Handles schedulerControl1.CustomDrawDayHeader
            Dim dh As DayHeader = TryCast(e.ObjectInfo, DayHeader)
            If dh IsNot Nothing Then
                Dim visTime As TimeInterval = dh.Interval
            Dim appointments_Count As Integer = CountAppointmentsByCriteria(dh, visTime)
                dh.Caption = String.Format("{0:MMM dd} has {1} apts", dh.Interval.Start.Date,appointments_Count)
            End If
        End Sub

        Private Function CountAppointmentsByCriteria(ByVal rh As SchedulerHeader, ByVal visTime As TimeInterval) As Integer
            Dim col As AppointmentBaseCollection = schedulerControl1.ActiveView.GetAppointments().FindAll(Function(apt As Appointment) (apt.ResourceIds.Contains(rh.Resource.Id) OrElse (Object.Equals(apt.ResourceId, ResourceEmpty.Id))) AndAlso (visTime.Contains(New TimeInterval(apt.Start, apt.End))))
            Return col.Count
        End Function

    End Class
End Namespace