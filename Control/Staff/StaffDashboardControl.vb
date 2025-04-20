Imports System.Data.SqlClient
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Drawing

Public Class StaffDashboardControl
    Inherits UserControl

    ' Chart controls
    Private ChartRevenue As New Chart()
    Private ChartBookings As New Chart()
    Private ChartCustomerTypes As New Chart()

    ' Modern color palette
    Private ReadOnly ColorPrimary As Color = Color.FromArgb(41, 128, 185)    ' Blue
    Private ReadOnly ColorSecondary As Color = Color.FromArgb(39, 174, 96)   ' Green
    Private ReadOnly ColorAccent As Color = Color.FromArgb(231, 76, 60)      ' Red
    Private ReadOnly ColorNeutral As Color = Color.FromArgb(52, 73, 94)      ' Dark Blue-Gray
    Private ReadOnly ColorBackground As Color = Color.FromArgb(245, 248, 250) ' Light Gray

    ' Chart color arrays for consistent styling
    Private ReadOnly PieChartColors As Color() = {
        Color.FromArgb(41, 128, 185),   ' Blue
        Color.FromArgb(39, 174, 96),    ' Green
        Color.FromArgb(231, 76, 60),    ' Red
        Color.FromArgb(241, 196, 15),   ' Yellow
        Color.FromArgb(142, 68, 173),   ' Purple
        Color.FromArgb(230, 126, 34)    ' Orange
    }

    Private Sub StaffDashboardControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = ColorBackground
        Me.Text = "Dashboard"
        Me.Size = New Size(1000, 750)

        ' Create a title label
        Dim lblDashboardTitle As New Label()
        With lblDashboardTitle
            .Text = "Revenue & Booking Analytics"
            .Font = New Font("Segoe UI", 18, FontStyle.Bold)
            .ForeColor = ColorNeutral
            .AutoSize = True
            .Location = New Point(20, 20)
        End With
        Me.Controls.Add(lblDashboardTitle)

        ' Create timestamp label
        Dim lblLastUpdated As New Label()
        With lblLastUpdated
            .Text = "Last updated: " & DateTime.Now.ToString("MMM dd, yyyy HH:mm")
            .Font = New Font("Segoe UI", 9)
            .ForeColor = ColorNeutral
            .AutoSize = True
            .Location = New Point(24, 55)
        End With
        Me.Controls.Add(lblLastUpdated)

        ' Create refresh button
        Dim btnRefresh As New Button()
        With btnRefresh
            .Text = "Refresh Data"
            .Font = New Font("Segoe UI", 9)
            .Size = New Size(100, 30)
            .Location = New Point(860, 45)
            .BackColor = ColorPrimary
            .ForeColor = Color.White
            .FlatStyle = FlatStyle.Flat
            .Cursor = Cursors.Hand
            .UseVisualStyleBackColor = False
        End With
        AddHandler btnRefresh.Click, AddressOf RefreshData
        Me.Controls.Add(btnRefresh)

        CreateCharts()
        LoadMonthlyRevenue()
        LoadBookingsOverTime()
        LoadCustomerTypes()
    End Sub

    Private Sub CreateCharts()
        ' Revenue Chart
        Dim revenuePanel As New Panel()
        With revenuePanel
            .Size = New Size(470, 320)
            .Location = New Point(20, 90)
            .BackColor = Color.White
            .BorderStyle = BorderStyle.None
        End With
        Me.Controls.Add(revenuePanel)

        ChartRevenue.Name = "ChartRevenue"
        ChartRevenue.Size = New Size(450, 290)
        ChartRevenue.Location = New Point(10, 20)
        revenuePanel.Controls.Add(ChartRevenue)

        Dim lblRevenue As New Label()
        With lblRevenue
            .Text = "Monthly Revenue"
            .Font = New Font("Segoe UI", 11, FontStyle.Bold)
            .ForeColor = ColorNeutral
            .AutoSize = True
            .Location = New Point(10, 0)
        End With
        revenuePanel.Controls.Add(lblRevenue)

        ' Customer Types Chart
        Dim customerPanel As New Panel()
        With customerPanel
            .Size = New Size(470, 320)
            .Location = New Point(510, 90)
            .BackColor = Color.White
            .BorderStyle = BorderStyle.None
        End With
        Me.Controls.Add(customerPanel)

        ChartCustomerTypes.Name = "ChartCustomerTypes"
        ChartCustomerTypes.Size = New Size(450, 290)
        ChartCustomerTypes.Location = New Point(10, 20)
        customerPanel.Controls.Add(ChartCustomerTypes)

        Dim lblCustomers As New Label()
        With lblCustomers
            .Text = "Customer Distribution"
            .Font = New Font("Segoe UI", 11, FontStyle.Bold)
            .ForeColor = ColorNeutral
            .AutoSize = True
            .Location = New Point(10, 0)
        End With
        customerPanel.Controls.Add(lblCustomers)

        ' Bookings Chart
        Dim bookingsPanel As New Panel()
        With bookingsPanel
            .Size = New Size(960, 320)
            .Location = New Point(20, 420)
            .BackColor = Color.White
            .BorderStyle = BorderStyle.None
        End With
        Me.Controls.Add(bookingsPanel)

        ChartBookings.Name = "ChartBookings"
        ChartBookings.Size = New Size(940, 290)
        ChartBookings.Location = New Point(10, 20)
        bookingsPanel.Controls.Add(ChartBookings)

        Dim lblBookings As New Label()
        With lblBookings
            .Text = "Bookings Trend"
            .Font = New Font("Segoe UI", 11, FontStyle.Bold)
            .ForeColor = ColorNeutral
            .AutoSize = True
            .Location = New Point(10, 0)
        End With
        bookingsPanel.Controls.Add(lblBookings)
    End Sub

    Private Sub LoadMonthlyRevenue()
        Try
            Dim query As String = "
            SELECT 
                DATE_FORMAT(PaymentDate, '%Y-%m') AS `Month`,
                SUM(AmountPaid) AS TotalRevenue
            FROM payment
            WHERE PaymentStatus IN ('Full', 'Partial', 'Overpaid')
            GROUP BY `Month`
            ORDER BY `Month`"

            Dim dt As DataTable = GetData(query)

            ' Safety check to ensure we have data
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                ' Generate sample data for testing
                dt = GenerateSampleRevenueData()
            End If

            With ChartRevenue
                .Series.Clear()
                .ChartAreas.Clear()

                ' Create and style chart area
                Dim chartArea As New ChartArea("Area1")
                chartArea.BackColor = Color.White
                chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(230, 230, 230)
                chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230)
                chartArea.AxisX.LabelStyle.Font = New Font("Segoe UI", 8)
                chartArea.AxisY.LabelStyle.Font = New Font("Segoe UI", 8)
                chartArea.AxisY.LabelStyle.Format = "${0:N0}"
                chartArea.AxisX.Title = "Month"
                chartArea.AxisY.Title = "Revenue ($)"
                chartArea.AxisX.TitleFont = New Font("Segoe UI", 9)
                chartArea.AxisY.TitleFont = New Font("Segoe UI", 9)
                .ChartAreas.Add(chartArea)

                ' Configure legends
                .Legends.Clear()
                Dim legend As New Legend("Legend1")
                legend.Font = New Font("Segoe UI", 9)
                legend.Docking = Docking.Bottom
                .Legends.Add(legend)

                ' Create and style series
                Dim series As New Series("Revenue")
                series.ChartType = SeriesChartType.Column
                series.Color = ColorPrimary
                series.BorderWidth = 0
                series.IsValueShownAsLabel = True
                series.LabelFormat = "${0:N0}"
                series.Font = New Font("Segoe UI", 8)
                .Series.Add(series)

                ' Add data points
                For Each row As DataRow In dt.Rows
                    Dim monthValue As String = row("Month").ToString()
                    Dim formattedMonth As String

                    ' Handle different date formats
                    If monthValue.Contains("-") Then
                        formattedMonth = Convert.ToDateTime(monthValue & "-01").ToString("MMM yy")
                    Else
                        formattedMonth = monthValue  ' Already formatted
                    End If

                    .Series("Revenue").Points.AddXY(formattedMonth, Convert.ToDecimal(row("TotalRevenue")))
                Next
            End With
        Catch ex As Exception
            MessageBox.Show("Error loading monthly revenue data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadBookingsOverTime()
        Try
            Dim query As String = "
                SELECT CAST(BookingDate AS DATE) AS BookingDay, 
                       COUNT(*) AS TotalBookings 
                FROM booking 
                GROUP BY CAST(BookingDate AS DATE) 
                ORDER BY BookingDay"

            Dim dt As DataTable = GetData(query)

            ' Safety check to ensure we have data
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                ' Generate sample data for testing
                dt = GenerateSampleBookingData()
            End If

            With ChartBookings
                .Series.Clear()
                .ChartAreas.Clear()

                ' Create and style chart area
                Dim chartArea As New ChartArea("Area1")
                chartArea.BackColor = Color.White
                chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(230, 230, 230)
                chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230)
                chartArea.AxisX.LabelStyle.Font = New Font("Segoe UI", 8)
                chartArea.AxisY.LabelStyle.Font = New Font("Segoe UI", 8)
                chartArea.AxisX.LabelStyle.Angle = -45
                chartArea.AxisX.Interval = 7
                chartArea.AxisX.Title = "Date"
                chartArea.AxisY.Title = "Number of Bookings"
                chartArea.AxisX.TitleFont = New Font("Segoe UI", 9)
                chartArea.AxisY.TitleFont = New Font("Segoe UI", 9)
                .ChartAreas.Add(chartArea)

                ' Configure legends
                .Legends.Clear()
                Dim legend As New Legend("Legend1")
                legend.Font = New Font("Segoe UI", 9)
                legend.Docking = Docking.Bottom
                .Legends.Add(legend)

                ' Create first series - line chart
                Dim linesSeries As New Series("Bookings")
                linesSeries.ChartType = SeriesChartType.Line
                linesSeries.Color = ColorSecondary
                linesSeries.BorderWidth = 3
                linesSeries.MarkerStyle = MarkerStyle.Circle
                linesSeries.MarkerSize = 6
                linesSeries.MarkerColor = ColorSecondary
                .Series.Add(linesSeries)

                ' Create second series - light area under the line
                Dim areaSeries As New Series("BookingsArea")
                areaSeries.ChartType = SeriesChartType.Area
                areaSeries.Color = Color.FromArgb(80, ColorSecondary)
                areaSeries.BorderWidth = 0
                .Series.Add(areaSeries)

                ' Add data points to both series
                For Each row As DataRow In dt.Rows
                    Dim bookingValue As Integer = Convert.ToInt32(row("TotalBookings"))
                    Dim dateValue As String

                    ' Handle different date formats
                    If TypeOf row("BookingDay") Is DateTime Then
                        dateValue = DirectCast(row("BookingDay"), DateTime).ToShortDateString()
                    Else
                        dateValue = row("BookingDay").ToString()
                    End If

                    .Series("Bookings").Points.AddXY(dateValue, bookingValue)
                    .Series("BookingsArea").Points.AddXY(dateValue, bookingValue)
                Next

                ' Set order of series (area in back, line in front)
                .Series("BookingsArea").SetCustomProperty("DrawingStyle", "Cylinder")
            End With
        Catch ex As Exception
            MessageBox.Show("Error loading booking data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadCustomerTypes()
        Try
            Dim query As String = "
                SELECT CustomerType, COUNT(*) AS Total 
                FROM customer 
                GROUP BY CustomerType"

            Dim dt As DataTable = GetData(query)

            ' Safety check to ensure we have data
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                ' Generate sample data for testing
                dt = GenerateSampleCustomerData()
            End If

            With ChartCustomerTypes
                .Series.Clear()
                .ChartAreas.Clear()
                .Annotations.Clear()

                ' Create and style chart area
                Dim chartArea As New ChartArea("Area1")
                chartArea.BackColor = Color.White
                .ChartAreas.Add(chartArea)

                ' Configure legends
                .Legends.Clear()
                Dim legend As New Legend("Legend1")
                legend.Font = New Font("Segoe UI", 9)
                legend.Docking = Docking.Right
                .Legends.Add(legend)

                ' Create and style series
                Dim series As New Series("CustomerTypes")
                series.ChartType = SeriesChartType.Doughnut
                series.BorderWidth = 1
                series.BorderColor = Color.White
                series.IsValueShownAsLabel = True
                series.LabelFormat = "{0:P1}"
                series.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                series.CustomProperties = "PieDrawingStyle=SoftEdge, DoughnutRadius=75"
                .Series.Add(series)

                ' Calculate total for percentage calculation
                Dim totalCustomers As Integer = 0
                For Each row As DataRow In dt.Rows
                    totalCustomers += Convert.ToInt32(row("Total"))
                Next

                ' Add data points with custom colors
                Dim colorIndex As Integer = 0
                For Each row As DataRow In dt.Rows
                    Dim dataPoint As New DataPoint()
                    dataPoint.SetValueXY(row("CustomerType").ToString(), Convert.ToInt32(row("Total")))
                    dataPoint.Color = PieChartColors(colorIndex Mod PieChartColors.Length)
                    dataPoint.LegendText = row("CustomerType").ToString() & " - " &
                        Convert.ToInt32(row("Total")) & " (" &
                        (Convert.ToInt32(row("Total")) / totalCustomers).ToString("P1") & ")"
                    series.Points.Add(dataPoint)
                    colorIndex += 1
                Next

                ' Only attempt to add center annotation if we have data points
                If series.Points.Count > 0 Then
                    ' Add a simple title annotation in the center instead of anchoring to a point
                    Dim annotation As New TextAnnotation()
                    With annotation
                        .Text = "Customer Types"
                        .ForeColor = ColorNeutral
                        .Font = New Font("Segoe UI", 10, FontStyle.Bold)
                        .X = 50  ' Use percentage of chart area (center)
                        .Y = 50  ' Use percentage of chart area (center)
                        .Width = 30
                        .Height = 10
                        .AnchorAlignment = ContentAlignment.MiddleCenter
                        .AllowMoving = False
                        .AllowSelecting = False
                    End With
                    ChartCustomerTypes.Annotations.Add(annotation)
                End If
            End With
        Catch ex As Exception
            MessageBox.Show("Error loading customer type data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RefreshData(sender As Object, e As EventArgs)
        Try
            LoadMonthlyRevenue()
            LoadBookingsOverTime()
            LoadCustomerTypes()

            ' Show loading indicator (simple implementation)
            Dim loadingLabel As New Label()
            With loadingLabel
                .Text = "Data refreshed!"
                .ForeColor = ColorSecondary
                .Font = New Font("Segoe UI", 9)
                .AutoSize = True
                .Location = New Point(770, 52)
            End With
            Me.Controls.Add(loadingLabel)

            ' Remove message after 2 seconds
            Dim timer As New Timer()
            timer.Interval = 2000
            AddHandler timer.Tick, Sub(s, args)
                                       Me.Controls.Remove(loadingLabel)
                                       loadingLabel.Dispose()
                                       timer.Stop()
                                       timer.Dispose()
                                   End Sub
            timer.Start()
        Catch ex As Exception
            MessageBox.Show("Error refreshing data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' This is a placeholder for the database connection method
    Private Function GetData(query As String) As DataTable
        ' In a real implementation, this would connect to the database
        ' For now, we return Nothing, and our sample data methods will handle it
        ' This would be replaced with actual database connection code
        Try
            ' Actual database connection code would go here
            ' For example:
            ' Using connection As New SqlConnection("Your connection string")
            '    Using adapter As New SqlDataAdapter(query, connection)
            '        Dim dt As New DataTable()
            '        adapter.Fill(dt)
            '        Return dt
            '    End Using
            ' End Using

            Return Nothing ' Return Nothing for now - sample data will be used
        Catch ex As Exception
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End Try
    End Function

    ' Generate sample data for testing purposes
    Private Function GenerateSampleRevenueData() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("Month", GetType(String))
        dt.Columns.Add("TotalRevenue", GetType(Decimal))

        ' Add sample monthly data for the last 6 months
        Dim currentDate As DateTime = DateTime.Now
        For i As Integer = 5 To 0 Step -1
            Dim monthDate As DateTime = currentDate.AddMonths(-i)
            Dim row As DataRow = dt.NewRow()
            row("Month") = monthDate.ToString("MMM yy")
            row("TotalRevenue") = 5000 + (i * 1200) + New Random().Next(-500, 1500)
            dt.Rows.Add(row)
        Next

        Return dt
    End Function

    Private Function GenerateSampleBookingData() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("BookingDay", GetType(String))
        dt.Columns.Add("TotalBookings", GetType(Integer))

        ' Add sample data for the last 30 days
        Dim currentDate As DateTime = DateTime.Now
        For i As Integer = 29 To 0 Step -1
            Dim day As DateTime = currentDate.AddDays(-i)
            Dim row As DataRow = dt.NewRow()
            row("BookingDay") = day.ToShortDateString()

            ' Create a pattern with weekend peaks
            Dim baseBookings As Integer = 10
            If day.DayOfWeek = DayOfWeek.Saturday OrElse day.DayOfWeek = DayOfWeek.Sunday Then
                baseBookings = 20
            End If

            row("TotalBookings") = baseBookings + New Random().Next(-3, 8)
            dt.Rows.Add(row)
        Next

        Return dt
    End Function

    Private Function GenerateSampleCustomerData() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("CustomerType", GetType(String))
        dt.Columns.Add("Total", GetType(Integer))

        ' Add sample customer types
        dt.Rows.Add("Individual", 356)
        dt.Rows.Add("Corporate", 212)
        dt.Rows.Add("Group", 154)
        dt.Rows.Add("VIP", 89)
        dt.Rows.Add("Partner", 47)

        Return dt
    End Function
End Class