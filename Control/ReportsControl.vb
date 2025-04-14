Imports System.Windows.Forms
Imports System.Data
Imports System.Drawing

Public Class ReportsControl
    ' Main controls
    Private tabReports As New TabControl()
    Private tabSalesReport As New TabPage("Sales Report")
    Private tabServiceUsageReport As New TabPage("Service Usage Report")
    Private tabEventSummaryReport As New TabPage("Event Summary Report")

    ' Report data grids
    Private dgvSalesReport As New DataGridView()
    Private dgvServiceUsageReport As New DataGridView()
    Private dgvEventSummaryReport As New DataGridView()

    ' Filter controls for Sales Report
    Private pnlSalesFilter As New Panel()
    Private lblSalesDateRange As New Label()
    Private cboSalesDateRange As New ComboBox()
    Private lblSalesStartDate As New Label()
    Private dtpSalesStartDate As New DateTimePicker()
    Private lblSalesEndDate As New Label()
    Private dtpSalesEndDate As New DateTimePicker()
    Private btnGenerateSalesReport As New Button()
    Private lblTotalSales As New Label()

    ' Filter controls for Service Usage Report
    Private pnlServiceFilter As New Panel()
    Private lblServiceStartDate As New Label()
    Private dtpServiceStartDate As New DateTimePicker()
    Private lblServiceEndDate As New Label()
    Private dtpServiceEndDate As New DateTimePicker()
    Private btnGenerateServiceReport As New Button()

    ' Filter controls for Event Summary Report
    Private pnlEventFilter As New Panel()
    Private lblEventStartDate As New Label()
    Private dtpEventStartDate As New DateTimePicker()
    Private lblEventEndDate As New Label()
    Private dtpEventEndDate As New DateTimePicker()
    Private cboEventStatus As New ComboBox()
    Private lblEventStatus As New Label()
    Private btnGenerateEventReport As New Button()

    ' Export controls
    Private btnExportSales As New Button()
    Private btnExportService As New Button()
    Private btnExportEvent As New Button()

    Public Sub New()
        SetupReportsControl()
    End Sub

    Private Sub SetupReportsControl()
        ' Configure main control
        Me.Dock = DockStyle.Fill
        Me.BackColor = Color.White

        ' Configure tab control
        tabReports.Dock = DockStyle.Fill
        tabReports.Font = New Font("Segoe UI", 10)
        Me.Controls.Add(tabReports)

        ' Configure tabs
        tabReports.TabPages.Add(tabSalesReport)
        tabReports.TabPages.Add(tabServiceUsageReport)
        tabReports.TabPages.Add(tabEventSummaryReport)

        ' Set up individual tabs
        SetupSalesReportTab()
        SetupServiceUsageReportTab()
        SetupEventSummaryReportTab()

        ' Set default dates
        Dim today As DateTime = DateTime.Today
        Dim firstDayOfMonth As New DateTime(today.Year, today.Month, 1)
        Dim lastDayOfMonth As DateTime = firstDayOfMonth.AddMonths(1).AddDays(-1)

        dtpSalesStartDate.Value = firstDayOfMonth
        dtpSalesEndDate.Value = lastDayOfMonth
        dtpServiceStartDate.Value = firstDayOfMonth
        dtpServiceEndDate.Value = lastDayOfMonth
        dtpEventStartDate.Value = firstDayOfMonth
        dtpEventEndDate.Value = lastDayOfMonth

        ' Generate initial reports
        GenerateSalesReport()
        GenerateServiceUsageReport()
        GenerateEventSummaryReport()
    End Sub

    Private Sub SetupSalesReportTab()
        ' Configure sales filter panel
        pnlSalesFilter = New Panel()
        pnlSalesFilter.Dock = DockStyle.Top
        pnlSalesFilter.Height = 80
        pnlSalesFilter.BorderStyle = BorderStyle.FixedSingle

        ' Date range selector
        lblSalesDateRange = New Label()
        lblSalesDateRange.Text = "Date Range:"
        lblSalesDateRange.Location = New Point(15, 15)
        lblSalesDateRange.AutoSize = True

        cboSalesDateRange = New ComboBox()
        cboSalesDateRange.Items.AddRange({"Daily", "Weekly", "Monthly", "Yearly", "Custom"})
        cboSalesDateRange.SelectedIndex = 2  ' Monthly by default
        cboSalesDateRange.Location = New Point(100, 12)
        cboSalesDateRange.Width = 120

        ' Start date
        lblSalesStartDate = New Label()
        lblSalesStartDate.Text = "Start Date:"
        lblSalesStartDate.Location = New Point(240, 15)
        lblSalesStartDate.AutoSize = True

        dtpSalesStartDate = New DateTimePicker()
        dtpSalesStartDate.Format = DateTimePickerFormat.Short
        dtpSalesStartDate.Location = New Point(315, 12)
        dtpSalesStartDate.Width = 100

        ' End date
        lblSalesEndDate = New Label()
        lblSalesEndDate.Text = "End Date:"
        lblSalesEndDate.Location = New Point(430, 15)
        lblSalesEndDate.AutoSize = True

        dtpSalesEndDate = New DateTimePicker()
        dtpSalesEndDate.Format = DateTimePickerFormat.Short
        dtpSalesEndDate.Location = New Point(500, 12)
        dtpSalesEndDate.Width = 100

        ' Generate report button
        btnGenerateSalesReport = New Button()
        btnGenerateSalesReport.Text = "Generate Report"
        btnGenerateSalesReport.Location = New Point(620, 10)
        btnGenerateSalesReport.Width = 140
        btnGenerateSalesReport.Height = 30
        AddHandler btnGenerateSalesReport.Click, AddressOf BtnGenerateSalesReport_Click

        ' Export button
        btnExportSales = New Button()
        btnExportSales.Text = "Export to Excel"
        btnExportSales.Location = New Point(620, 45)
        btnExportSales.Width = 140
        btnExportSales.Height = 30
        AddHandler btnExportSales.Click, AddressOf BtnExportSales_Click

        ' Total Sales label
        lblTotalSales = New Label()
        lblTotalSales.Text = "Total Sales: ₱0.00"
        lblTotalSales.Location = New Point(15, 50)
        lblTotalSales.AutoSize = True
        lblTotalSales.Font = New Font(lblTotalSales.Font, FontStyle.Bold)

        ' Add controls to filter panel
        pnlSalesFilter.Controls.Add(lblSalesDateRange)
        pnlSalesFilter.Controls.Add(cboSalesDateRange)
        pnlSalesFilter.Controls.Add(lblSalesStartDate)
        pnlSalesFilter.Controls.Add(dtpSalesStartDate)
        pnlSalesFilter.Controls.Add(lblSalesEndDate)
        pnlSalesFilter.Controls.Add(dtpSalesEndDate)
        pnlSalesFilter.Controls.Add(btnGenerateSalesReport)
        pnlSalesFilter.Controls.Add(btnExportSales)
        pnlSalesFilter.Controls.Add(lblTotalSales)

        ' Configure sales data grid
        dgvSalesReport = New DataGridView()
        dgvSalesReport.Dock = DockStyle.Fill
        dgvSalesReport.AllowUserToAddRows = False
        dgvSalesReport.AllowUserToDeleteRows = False
        dgvSalesReport.ReadOnly = True
        dgvSalesReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvSalesReport.BorderStyle = BorderStyle.None
        dgvSalesReport.BackgroundColor = Color.White
        dgvSalesReport.RowHeadersVisible = False
        dgvSalesReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvSalesReport.MultiSelect = False

        ' Add controls to tab
        tabSalesReport.Controls.Add(dgvSalesReport)
        tabSalesReport.Controls.Add(pnlSalesFilter)

        ' Add date range change handler
        AddHandler cboSalesDateRange.SelectedIndexChanged, AddressOf CboSalesDateRange_SelectedIndexChanged
    End Sub

    Private Sub SetupServiceUsageReportTab()
        ' Configure service filter panel
        pnlServiceFilter = New Panel()
        pnlServiceFilter.Dock = DockStyle.Top
        pnlServiceFilter.Height = 80
        pnlServiceFilter.BorderStyle = BorderStyle.FixedSingle

        ' Start date
        lblServiceStartDate = New Label()
        lblServiceStartDate.Text = "Start Date:"
        lblServiceStartDate.Location = New Point(15, 15)
        lblServiceStartDate.AutoSize = True

        dtpServiceStartDate = New DateTimePicker()
        dtpServiceStartDate.Format = DateTimePickerFormat.Short
        dtpServiceStartDate.Location = New Point(100, 12)
        dtpServiceStartDate.Width = 100

        ' End date
        lblServiceEndDate = New Label()
        lblServiceEndDate.Text = "End Date:"
        lblServiceEndDate.Location = New Point(220, 15)
        lblServiceEndDate.AutoSize = True

        dtpServiceEndDate = New DateTimePicker()
        dtpServiceEndDate.Format = DateTimePickerFormat.Short
        dtpServiceEndDate.Location = New Point(295, 12)
        dtpServiceEndDate.Width = 100

        ' Generate report button
        btnGenerateServiceReport = New Button()
        btnGenerateServiceReport.Text = "Generate Report"
        btnGenerateServiceReport.Location = New Point(415, 10)
        btnGenerateServiceReport.Width = 140
        btnGenerateServiceReport.Height = 30
        AddHandler btnGenerateServiceReport.Click, AddressOf BtnGenerateServiceReport_Click

        ' Export button
        btnExportService = New Button()
        btnExportService.Text = "Export to Excel"
        btnExportService.Location = New Point(415, 45)
        btnExportService.Width = 140
        btnExportService.Height = 30
        AddHandler btnExportService.Click, AddressOf BtnExportService_Click

        ' Add controls to filter panel
        pnlServiceFilter.Controls.Add(lblServiceStartDate)
        pnlServiceFilter.Controls.Add(dtpServiceStartDate)
        pnlServiceFilter.Controls.Add(lblServiceEndDate)
        pnlServiceFilter.Controls.Add(dtpServiceEndDate)
        pnlServiceFilter.Controls.Add(btnGenerateServiceReport)
        pnlServiceFilter.Controls.Add(btnExportService)

        ' Configure service usage data grid
        dgvServiceUsageReport = New DataGridView()
        dgvServiceUsageReport.Dock = DockStyle.Fill
        dgvServiceUsageReport.AllowUserToAddRows = False
        dgvServiceUsageReport.AllowUserToDeleteRows = False
        dgvServiceUsageReport.ReadOnly = True
        dgvServiceUsageReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvServiceUsageReport.BorderStyle = BorderStyle.None
        dgvServiceUsageReport.BackgroundColor = Color.White
        dgvServiceUsageReport.RowHeadersVisible = False
        dgvServiceUsageReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvServiceUsageReport.MultiSelect = False

        ' Add controls to tab
        tabServiceUsageReport.Controls.Add(dgvServiceUsageReport)
        tabServiceUsageReport.Controls.Add(pnlServiceFilter)
    End Sub

    Private Sub SetupEventSummaryReportTab()
        ' Configure event filter panel
        pnlEventFilter = New Panel()
        pnlEventFilter.Dock = DockStyle.Top
        pnlEventFilter.Height = 80
        pnlEventFilter.BorderStyle = BorderStyle.FixedSingle

        ' Start date
        lblEventStartDate = New Label()
        lblEventStartDate.Text = "Start Date:"
        lblEventStartDate.Location = New Point(15, 15)
        lblEventStartDate.AutoSize = True

        dtpEventStartDate = New DateTimePicker()
        dtpEventStartDate.Format = DateTimePickerFormat.Short
        dtpEventStartDate.Location = New Point(100, 12)
        dtpEventStartDate.Width = 100

        ' End date
        lblEventEndDate = New Label()
        lblEventEndDate.Text = "End Date:"
        lblEventEndDate.Location = New Point(220, 15)
        lblEventEndDate.AutoSize = True

        dtpEventEndDate = New DateTimePicker()
        dtpEventEndDate.Format = DateTimePickerFormat.Short
        dtpEventEndDate.Location = New Point(295, 12)
        dtpEventEndDate.Width = 100

        ' Event status filter
        lblEventStatus = New Label()
        lblEventStatus.Text = "Status:"
        lblEventStatus.Location = New Point(15, 45)
        lblEventStatus.AutoSize = True

        cboEventStatus = New ComboBox()
        cboEventStatus.Items.AddRange({"All", "Upcoming", "Cancelled", "Completed"})
        cboEventStatus.SelectedIndex = 0
        cboEventStatus.Location = New Point(100, 42)
        cboEventStatus.Width = 120

        ' Generate report button
        btnGenerateEventReport = New Button()
        btnGenerateEventReport.Text = "Generate Report"
        btnGenerateEventReport.Location = New Point(415, 10)
        btnGenerateEventReport.Width = 140
        btnGenerateEventReport.Height = 30
        AddHandler btnGenerateEventReport.Click, AddressOf BtnGenerateEventReport_Click

        ' Export button
        btnExportEvent = New Button()
        btnExportEvent.Text = "Export to Excel"
        btnExportEvent.Location = New Point(415, 45)
        btnExportEvent.Width = 140
        btnExportEvent.Height = 30
        AddHandler btnExportEvent.Click, AddressOf BtnExportEvent_Click

        ' Add controls to filter panel
        pnlEventFilter.Controls.Add(lblEventStartDate)
        pnlEventFilter.Controls.Add(dtpEventStartDate)
        pnlEventFilter.Controls.Add(lblEventEndDate)
        pnlEventFilter.Controls.Add(dtpEventEndDate)
        pnlEventFilter.Controls.Add(lblEventStatus)
        pnlEventFilter.Controls.Add(cboEventStatus)
        pnlEventFilter.Controls.Add(btnGenerateEventReport)
        pnlEventFilter.Controls.Add(btnExportEvent)

        ' Configure event summary data grid
        dgvEventSummaryReport = New DataGridView()
        dgvEventSummaryReport.Dock = DockStyle.Fill
        dgvEventSummaryReport.AllowUserToAddRows = False
        dgvEventSummaryReport.AllowUserToDeleteRows = False
        dgvEventSummaryReport.ReadOnly = True
        dgvEventSummaryReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvEventSummaryReport.BorderStyle = BorderStyle.None
        dgvEventSummaryReport.BackgroundColor = Color.White
        dgvEventSummaryReport.RowHeadersVisible = False
        dgvEventSummaryReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvEventSummaryReport.MultiSelect = False

        ' Add controls to tab
        tabEventSummaryReport.Controls.Add(dgvEventSummaryReport)
        tabEventSummaryReport.Controls.Add(pnlEventFilter)
    End Sub

    ' Event handlers
    Private Sub CboSalesDateRange_SelectedIndexChanged(sender As Object, e As EventArgs)
        ' Set date range based on selection
        Dim today As DateTime = DateTime.Today

        Select Case cboSalesDateRange.SelectedIndex
            Case 0 ' Daily
                dtpSalesStartDate.Value = today
                dtpSalesEndDate.Value = today
            Case 1 ' Weekly
                dtpSalesStartDate.Value = today.AddDays(-(CInt(today.DayOfWeek)))
                dtpSalesEndDate.Value = dtpSalesStartDate.Value.AddDays(6)
            Case 2 ' Monthly
                dtpSalesStartDate.Value = New DateTime(today.Year, today.Month, 1)
                dtpSalesEndDate.Value = dtpSalesStartDate.Value.AddMonths(1).AddDays(-1)
            Case 3 ' Yearly
                dtpSalesStartDate.Value = New DateTime(today.Year, 1, 1)
                dtpSalesEndDate.Value = New DateTime(today.Year, 12, 31)
            Case 4 ' Custom
                ' Leave dates as they are for manual selection
        End Select

        ' Update enabled state
        dtpSalesStartDate.Enabled = (cboSalesDateRange.SelectedIndex = 4)
        dtpSalesEndDate.Enabled = (cboSalesDateRange.SelectedIndex = 4)

        ' Generate report with new date range
        GenerateSalesReport()
    End Sub

    Private Sub BtnGenerateSalesReport_Click(sender As Object, e As EventArgs)
        GenerateSalesReport()
    End Sub

    Private Sub BtnGenerateServiceReport_Click(sender As Object, e As EventArgs)
        GenerateServiceUsageReport()
    End Sub

    Private Sub BtnGenerateEventReport_Click(sender As Object, e As EventArgs)
        GenerateEventSummaryReport()
    End Sub

    Private Sub BtnExportSales_Click(sender As Object, e As EventArgs)
        ExportToExcel(dgvSalesReport, "Sales Report")
    End Sub

    Private Sub BtnExportService_Click(sender As Object, e As EventArgs)
        ExportToExcel(dgvServiceUsageReport, "Service Usage Report")
    End Sub

    Private Sub BtnExportEvent_Click(sender As Object, e As EventArgs)
        ExportToExcel(dgvEventSummaryReport, "Event Summary Report")
    End Sub

    ' Report generation methods
    Private Sub GenerateSalesReport()
        Try
            ' Format dates for MySQL query
            Dim startDate As String = dtpSalesStartDate.Value.ToString("yyyy-MM-dd")
            Dim endDate As String = dtpSalesEndDate.Value.AddDays(1).ToString("yyyy-MM-dd")

            ' Query to get sales data from database
            Dim query As String = "
            SELECT 
                b.BookingID,
                e.EventName,
                c.CustomerID,
                CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName,
                e.EventDate,
                p.AmountPaid,
                p.PaymentDate,
                p.PaymentStatus,
                b.BookingStatus
            FROM 
                booking b
            JOIN 
                event e ON b.EventID = e.EventID
            JOIN 
                customer c ON b.CustomerID = c.CustomerID
            LEFT JOIN 
                payment p ON b.BookingID = p.BookingID
            WHERE 
                p.PaymentDate BETWEEN @startDate AND @endDate
            ORDER BY 
                p.PaymentDate DESC"

            ' Create parameters
            Dim parameters As New Dictionary(Of String, Object) From {
                {"@startDate", startDate},
                {"@endDate", endDate}
            }

            ' Get data from database
            Dim salesData As DataTable = DatabaseHelper.GetData(query, parameters)

            ' Set data source
            dgvSalesReport.DataSource = salesData

            ' Calculate and display total sales with correct column name
            Dim totalSales As Decimal = 0
            For Each row As DataRow In salesData.Rows
                If Not IsDBNull(row("AmountPaid")) Then
                    totalSales += Convert.ToDecimal(row("AmountPaid"))
                End If
            Next

            ' Update total label
            lblTotalSales.Text = $"Total Sales: ₱{totalSales:N2}"
        Catch ex As Exception
            MessageBox.Show("Error generating sales report: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub GenerateServiceUsageReport()
        Try
            ' Format dates for MySQL query
            Dim startDate As String = dtpServiceStartDate.Value.ToString("yyyy-MM-dd")
            Dim endDate As String = dtpServiceEndDate.Value.ToString("yyyy-MM-dd")

            ' Corrected query to get service usage data
            Dim query As String = "
            SELECT 
                s.ServiceID,
                s.ServiceName,
                s.Category,
                s.Price,
                s.Unit,
                SUM(CASE WHEN e.EventDate BETWEEN @startDate AND @endDate THEN 1 ELSE 0 END) AS UsageCount,
                SUM(CASE WHEN e.EventDate BETWEEN @startDate AND @endDate THEN s.Price ELSE 0 END) AS TotalRevenue
            FROM 
                service_availed s
            LEFT JOIN 
                booking b ON s.ServiceID = b.ServiceID
            LEFT JOIN 
                event e ON b.EventID = e.EventID
            GROUP BY 
                s.ServiceID, s.ServiceName, s.Category, s.Price, s.Unit
            ORDER BY 
                UsageCount DESC"

            ' Create parameters
            Dim parameters As New Dictionary(Of String, Object) From {
                {"@startDate", startDate},
                {"@endDate", endDate}
            }

            ' Get data from database
            Dim serviceData As DataTable = DatabaseHelper.GetData(query, parameters)

            ' Set data source
            dgvServiceUsageReport.DataSource = serviceData
        Catch ex As Exception
            MessageBox.Show("Error generating service usage report: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GenerateEventSummaryReport()
        Try
            ' Format dates for MySQL query
            Dim startDate As String = dtpEventStartDate.Value.ToString("yyyy-MM-dd")
            Dim endDate As String = dtpEventEndDate.Value.ToString("yyyy-MM-dd")

            ' Build query with parameterized status filter
            Dim query As String = "
            SELECT 
                e.EventID,
                e.EventName,
                e.EventType,
                e.EventDate,
                e.StartTime,
                e.EndTime,
                e.VenueLocation,
                e.GuestCount,
                e.Theme,
                c.CustomerID,
                CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName,
                b.BookingStatus,
                b.BookingDate
            FROM 
                event e
            JOIN 
                booking b ON e.EventID = b.EventID
            JOIN 
                customer c ON b.CustomerID = c.CustomerID
            WHERE 
                e.EventDate BETWEEN @startDate AND @endDate"

            ' Create parameters dictionary
            Dim parameters As New Dictionary(Of String, Object) From {
                {"@startDate", startDate},
                {"@endDate", endDate}
            }

            ' Add status filter if not "All"
            If cboEventStatus.SelectedIndex > 0 Then
                query &= " AND b.BookingStatus = @status"
                parameters.Add("@status", cboEventStatus.SelectedItem.ToString())
            End If

            query &= " ORDER BY e.EventDate"

            ' Get data from database
            Dim eventData As DataTable = DatabaseHelper.GetData(query, parameters)

            ' Set data source
            dgvEventSummaryReport.DataSource = eventData
        Catch ex As Exception
            MessageBox.Show("Error generating event summary report: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Helper methods
    Private Sub ExportToExcel(dgv As DataGridView, fileName As String)
        Try
            ' Create save file dialog
            Dim saveDialog As New SaveFileDialog()
            ' Set default filter to CSV only
            saveDialog.Filter = "CSV Files (*.csv)|*.csv"
            saveDialog.DefaultExt = "csv"
            saveDialog.AddExtension = True
            saveDialog.FileName = fileName & "_" & DateTime.Now.ToString("yyyyMMdd")

            If saveDialog.ShowDialog() = DialogResult.OK Then
                ' Create stringbuilder for CSV data
                Dim sb As New System.Text.StringBuilder()

                ' Add column headers
                Dim columnHeaders As New List(Of String)
                For Each column As DataGridViewColumn In dgv.Columns
                    ' Quote headers to handle commas
                    Dim header As String = column.HeaderText
                    If header.Contains(",") Then
                        header = """" & header & """"
                    End If
                    columnHeaders.Add(header)
                Next
                sb.AppendLine(String.Join(",", columnHeaders))

                ' Add rows
                For Each row As DataGridViewRow In dgv.Rows
                    Dim rowData As New List(Of String)
                    For Each cell As DataGridViewCell In row.Cells
                        ' Add cell value, handle nulls and special characters
                        Dim value As String = If(cell.Value IsNot Nothing, cell.Value.ToString(), "")
                        ' Quote values that contain commas or quotes, and escape quotes
                        If value.Contains(",") OrElse value.Contains("""") Then
                            value = """" & value.Replace("""", """""") & """"
                        End If
                        rowData.Add(value)
                    Next
                    sb.AppendLine(String.Join(",", rowData))
                Next

                ' Write to file
                System.IO.File.WriteAllText(saveDialog.FileName, sb.ToString())

                MessageBox.Show("Export completed successfully.", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Error exporting data: " & ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class