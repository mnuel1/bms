Imports System.Data.SqlClient
Imports System.Windows.Forms.DataVisualization.Charting

Public Class StaffDashboardControl
    Inherits UserControl

    Private ChartRevenue As New Chart()
    Private ChartBookings As New Chart()
    Private ChartCustomerTypes As New Chart()

    Private Sub StaffDashboardControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Dashboard"
        Me.Size = New Size(1000, 700)
        CreateCharts()
        LoadMonthlyRevenue()
        LoadBookingsOverTime()
        LoadCustomerTypes()
    End Sub

    Private Sub CreateCharts()
        ' ChartRevenue - Monthly Revenue
        ChartRevenue.Name = "ChartRevenue"
        ChartRevenue.Size = New Size(450, 300)
        ChartRevenue.Location = New Point(20, 20)
        Me.Controls.Add(ChartRevenue)

        ' ChartCustomerTypes - Customer Type Pie Chart
        ChartCustomerTypes.Name = "ChartCustomerTypes"
        ChartCustomerTypes.Size = New Size(450, 300)
        ChartCustomerTypes.Location = New Point(500, 20)
        Me.Controls.Add(ChartCustomerTypes)

        ' ChartBookings - Bookings Over Time
        ChartBookings.Name = "ChartBookings"
        ChartBookings.Size = New Size(930, 300)
        ChartBookings.Location = New Point(20, 340)
        Me.Controls.Add(ChartBookings)
    End Sub

    Private Sub LoadMonthlyRevenue()
        Dim query As String = "
        SELECT 
            DATE_FORMAT(PaymentDate, '%Y-%m') AS `Month`,
            SUM(AmountPaid) AS TotalRevenue
        FROM payment
        WHERE PaymentStatus IN ('Full', 'Partial', 'Overpaid')
        GROUP BY `Month`
        ORDER BY `Month`"

        Dim dt As DataTable = GetData(query)

        With ChartRevenue
            .Series.Clear()
            .ChartAreas.Clear()
            .ChartAreas.Add(New ChartArea("Area1"))
            .Titles.Clear()
            .Titles.Add("Monthly Revenue")
            .Series.Add("Revenue")
            .Series("Revenue").ChartType = SeriesChartType.Column
            .Legends.Clear()
            .Legends.Add("Legend1")

            For Each row As DataRow In dt.Rows
                .Series("Revenue").Points.AddXY(
                row("Month").ToString(),
                Convert.ToDecimal(row("TotalRevenue"))
            )
            Next
        End With
    End Sub


    Private Sub LoadBookingsOverTime()
        Dim query As String = "
            SELECT CAST(BookingDate AS DATE) AS BookingDay, 
                   COUNT(*) AS TotalBookings 
            FROM booking 
            GROUP BY CAST(BookingDate AS DATE) 
            ORDER BY BookingDay"
        Dim dt As DataTable = GetData(query)

        With ChartBookings
            .Series.Clear()
            .ChartAreas.Clear()
            .ChartAreas.Add(New ChartArea("Area1"))
            .Titles.Clear()
            .Titles.Add("Bookings Over Time")
            .Legends.Clear()
            .Legends.Add("Legend1")
            .Legends("Legend1").Docking = Docking.Bottom
            .Series.Add("Bookings")
            .Series("Bookings").ChartType = SeriesChartType.Line
            .Series("Bookings").BorderWidth = 2


            For Each row As DataRow In dt.Rows
                .Series("Bookings").Points.AddXY(CDate(row("BookingDay")).ToShortDateString(), Convert.ToInt32(row("TotalBookings")))
            Next
        End With
    End Sub

    Private Sub LoadCustomerTypes()
        Dim query As String = "
            SELECT CustomerType, COUNT(*) AS Total 
            FROM customer 
            GROUP BY CustomerType"
        Dim dt As DataTable = GetData(query)

        With ChartCustomerTypes
            .Series.Clear()
            .ChartAreas.Clear()
            .ChartAreas.Add(New ChartArea("Area1"))
            .Titles.Clear()
            .Titles.Add("Customer Types")
            .Legends.Clear()
            .Legends.Add("Legend1")
            .Legends("Legend1").Docking = Docking.Bottom
            .Series.Add("CustomerTypes")
            .Series("CustomerTypes").ChartType = SeriesChartType.Pie
            .Series("CustomerTypes").IsValueShownAsLabel = True


            For Each row As DataRow In dt.Rows
                .Series("CustomerTypes").Points.AddXY(row("CustomerType").ToString(), Convert.ToInt32(row("Total")))
            Next
        End With
    End Sub

End Class
