Imports System.Windows.Forms
Imports System.Data
Imports System.Drawing

Public Class accountantReportControl
    Inherits UserControl

    ' UI Controls
    Private WithEvents cboSalesDateRange As ComboBox
    Private dtpSalesStartDate As DateTimePicker
    Private dtpSalesEndDate As DateTimePicker
    Private WithEvents btnGenerateSalesReport As Button
    Private WithEvents btnExportSales As Button
    Private lblTotalSales As Label
    Private dgvSalesReport As DataGridView
    Private pnlSalesFilter As Panel

    Public Sub New()
        InitializeControls()
        SetupSalesReportControl()
    End Sub

    Private Sub InitializeControls()
        ' Instantiate all controls
        pnlSalesFilter = New Panel()
        cboSalesDateRange = New ComboBox()
        dtpSalesStartDate = New DateTimePicker()
        dtpSalesEndDate = New DateTimePicker()
        btnGenerateSalesReport = New Button()
        btnExportSales = New Button()
        lblTotalSales = New Label()
        dgvSalesReport = New DataGridView()
    End Sub

    Private Sub SetupSalesReportControl()
        ' Configure filter panel
        pnlSalesFilter.Dock = DockStyle.Top
        pnlSalesFilter.Height = 80
        pnlSalesFilter.BorderStyle = BorderStyle.FixedSingle

        ' Date range selector
        Dim lblSalesDateRange As New Label() With {.Text = "Date Range:", .Location = New Point(15, 15), .AutoSize = True}
        cboSalesDateRange.Items.AddRange({"Daily", "Weekly", "Monthly", "Yearly", "Custom"})
        cboSalesDateRange.SelectedIndex = 2 ' Monthly by default
        cboSalesDateRange.Location = New Point(100, 12)
        cboSalesDateRange.Width = 120

        ' Start date
        Dim lblSalesStartDate As New Label() With {.Text = "Start Date:", .Location = New Point(240, 15), .AutoSize = True}
        dtpSalesStartDate.Format = DateTimePickerFormat.Short
        dtpSalesStartDate.Location = New Point(315, 12)
        dtpSalesStartDate.Width = 100

        ' End date
        Dim lblSalesEndDate As New Label() With {.Text = "End Date:", .Location = New Point(430, 15), .AutoSize = True}
        dtpSalesEndDate.Format = DateTimePickerFormat.Short
        dtpSalesEndDate.Location = New Point(500, 12)
        dtpSalesEndDate.Width = 100

        ' Generate report button
        btnGenerateSalesReport.Text = "Generate Report"
        btnGenerateSalesReport.Location = New Point(620, 10)
        btnGenerateSalesReport.Width = 140
        btnGenerateSalesReport.Height = 30

        ' Export button
        btnExportSales.Text = "Export to CSV"
        btnExportSales.Location = New Point(620, 45)
        btnExportSales.Width = 140
        btnExportSales.Height = 30

        ' Total Sales label
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

        ' Configure data grid
        dgvSalesReport.Dock = DockStyle.Fill
        dgvSalesReport.AllowUserToAddRows = False
        dgvSalesReport.AllowUserToDeleteRows = False
        dgvSalesReport.ReadOnly = True
        dgvSalesReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvSalesReport.BackgroundColor = Color.White
        dgvSalesReport.RowHeadersVisible = False

        ' Add panels and grid to the user control
        Me.Controls.Add(dgvSalesReport)
        Me.Controls.Add(pnlSalesFilter)

        ' Set default dates
        Dim today As DateTime = DateTime.Today
        Dim firstDayOfMonth As New DateTime(today.Year, today.Month, 1)
        Dim lastDayOfMonth As DateTime = firstDayOfMonth.AddMonths(1).AddDays(-1)
        dtpSalesStartDate.Value = firstDayOfMonth
        dtpSalesEndDate.Value = lastDayOfMonth

        ' Generate initial report
        GenerateSalesReport()
    End Sub

    Private Sub CboSalesDateRange_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSalesDateRange.SelectedIndexChanged
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

        ' Enable date pickers only for custom range
        dtpSalesStartDate.Enabled = (cboSalesDateRange.SelectedIndex = 4)
        dtpSalesEndDate.Enabled = (cboSalesDateRange.SelectedIndex = 4)

        ' Generate report with new date range
        GenerateSalesReport()
    End Sub

    Private Sub BtnGenerateSalesReport_Click(sender As Object, e As EventArgs) Handles btnGenerateSalesReport.Click
        GenerateSalesReport()
    End Sub

    Private Sub BtnExportSales_Click(sender As Object, e As EventArgs) Handles btnExportSales.Click
        ExportToCSV()
    End Sub

    Private Sub GenerateSalesReport()
        Try
            ' Format dates for MySQL query
            Dim startDate As String = dtpSalesStartDate.Value.ToString("yyyy-MM-dd")
            Dim endDate As String = dtpSalesEndDate.Value.ToString("yyyy-MM-dd")

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

            ' Get data from database (Assuming DatabaseHelper is accessible)
            Dim salesData As DataTable = DatabaseHelper.GetData(query, parameters)

            ' Set data source
            dgvSalesReport.DataSource = salesData

            ' Calculate and display total sales
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

    Private Sub ExportToCSV()
        Try
            ' Create save file dialog
            Dim saveDialog As New SaveFileDialog()
            saveDialog.Filter = "CSV Files (*.csv)|*.csv"
            saveDialog.DefaultExt = "csv"
            saveDialog.AddExtension = True
            saveDialog.FileName = "Sales Report_" & DateTime.Now.ToString("yyyyMMdd")

            If saveDialog.ShowDialog() = DialogResult.OK Then
                ' Create stringbuilder for CSV data
                Dim sb As New System.Text.StringBuilder()

                ' Add column headers
                Dim columnHeaders As New List(Of String)
                For Each column As DataGridViewColumn In dgvSalesReport.Columns
                    Dim header As String = column.HeaderText
                    If header.Contains(",") Then
                        header = """" & header & """"
                    End If
                    columnHeaders.Add(header)
                Next
                sb.AppendLine(String.Join(",", columnHeaders))

                ' Add rows
                For Each row As DataGridViewRow In dgvSalesReport.Rows
                    Dim rowData As New List(Of String)
                    For Each cell As DataGridViewCell In row.Cells
                        Dim value As String = If(cell.Value IsNot Nothing, cell.Value.ToString(), "")
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