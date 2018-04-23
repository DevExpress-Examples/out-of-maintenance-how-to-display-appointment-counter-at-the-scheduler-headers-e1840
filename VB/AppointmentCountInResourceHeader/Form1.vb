Imports Microsoft.VisualBasic
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
    Public Partial Class Form1
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
            If Not rh Is Nothing Then
            Dim visTime As TimeInterval = schedulerControl1.ActiveView.GetVisibleIntervals().Interval
            Dim appointments_Count As Integer = CountAppointmentsByCriteria(rh, visTime)
            rh.Caption = rh.Resource.Caption & " has " & appointments_Count.ToString() & " apts"

            End If
        End Sub

        Private Sub schedulerControl1_CustomDrawDayHeader(ByVal sender As Object, ByVal e As CustomDrawObjectEventArgs) Handles schedulerControl1.CustomDrawDayHeader
            Dim dh As DayHeader = TryCast(e.ObjectInfo, DayHeader)
            If Not dh Is Nothing Then
                Dim visTime As TimeInterval = dh.Interval
            Dim appointments_Count As Integer = CountAppointmentsByCriteria(dh, visTime)
                dh.Caption = String.Format("{0:MMM dd} has {1} apts", dh.Interval.Start.Date,appointments_Count)
            End If
        End Sub

        Private Function CountAppointmentsByCriteria(ByVal dh As SchedulerHeader, ByVal visTime As TimeInterval) As Integer
            Dim col As AppointmentBaseCollection = schedulerControl1.ActiveView.GetAppointments().FindAll(AddressOf New PredicateAppointment(dh, visTime).GetAppointmentsByCriteria)
            Return col.Count
        End Function

    End Class

    Class PredicateAppointment
        Private m_header As SchedulerHeader
        Private m_visible_time As TimeInterval

        Public Sub New(ByVal rh As SchedulerHeader, ByVal visTime As TimeInterval)
            m_header = rh
            m_visible_time = visTime
        End Sub

        Public Function GetAppointmentsByCriteria(ByVal apt As Appointment) As Boolean
            Return (apt.ResourceIds.Contains(m_header.Resource.Id) OrElse (apt.ResourceId Is Resource.Empty)) AndAlso (m_visible_time.Contains(New TimeInterval(apt.Start, apt.End)))
        End Function

    End Class

End Namespace