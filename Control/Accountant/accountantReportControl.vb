Imports System.Windows.Forms
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D

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
    Private lblHeader As Label
    Private pnlHeader As Panel

    ' Colors
    Private ReadOnly headerColor As Color = Color.FromArgb(76, 175, 80)  ' Green
    Private ReadOnly accentColor As Color = Color.FromArgb(33, 150, 243)  ' Blue
    Private ReadOnly filterPanelColor As Color = Color.FromArgb(245, 245, 245)  ' Light Gray
    Private ReadOnly textColor As Color = Color.FromArgb(33, 33, 33)  ' Dark Gray
    Private ReadOnly buttonHoverColor As Color = Color.FromArgb(25, 118, 210)  ' Darker Blue

    Public Sub New()
        InitializeControls()
        ApplyStyles()
        SetupSalesReportControl()
    End Sub

    Private Sub InitializeControls()
        ' Instantiate all controls
        pnlHeader = New Panel()
        lblHeader = New Label()
        pnlSalesFilter = New Panel()
        cboSalesDateRange = New ComboBox()
        dtpSalesStartDate = New DateTimePicker()
        dtpSalesEndDate = New DateTimePicker()
        btnGenerateSalesReport = New Button()
        btnExportSales = New Button()
        lblTotalSales = New Label()
        dgvSalesReport = New DataGridView()
    End Sub

    Private Sub ApplyStyles()
        ' Overall control style
        Me.Font = New Font("Segoe UI", 9.5F, FontStyle.Regular)
        Me.ForeColor = textColor
        Me.BackColor = Color.White

        ' Apply styles to panels
        pnlHeader.BackColor = headerColor
        pnlSalesFilter.BackColor = filterPanelColor

        ' Style header label
        lblHeader.ForeColor = Color.White
        lblHeader.Font = New Font("Segoe UI", 14.0F, FontStyle.Bold)

        ' Style buttons
        StyleButton(btnGenerateSalesReport, accentColor, Color.White)
        StyleButton(btnExportSales, Color.FromArgb(220, 220, 220), textColor)

        ' Style total sales label
        lblTotalSales.ForeColor = headerColor
        lblTotalSales.Font = New Font("Segoe UI", 11.0F, FontStyle.Bold)

        ' Style data grid
        StyleDataGridView()
    End Sub

    Private Sub StyleButton(btn As Button, bgColor As Color, fgColor As Color)
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.BackColor = bgColor
        btn.ForeColor = fgColor
        btn.Font = New Font("Segoe UI", 9.5F, FontStyle.Regular)
        btn.Cursor = Cursors.Hand

        ' Use Tag to store original color for hover effect
        btn.Tag = bgColor

        ' Add hover effect handlers
        AddHandler btn.MouseEnter, AddressOf Button_MouseEnter
        AddHandler btn.MouseLeave, AddressOf Button_MouseLeave
    End Sub

    Private Sub Button_MouseEnter(sender As Object, e As EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        If btn.BackColor = accentColor Then
            btn.BackColor = buttonHoverColor
        Else
            btn.BackColor = Color.FromArgb(200, 200, 200)
        End If
    End Sub

    Private Sub Button_MouseLeave(sender As Object, e As EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        btn.BackColor = DirectCast(btn.Tag, Color)
    End Sub

    Private Sub StyleDataGridView()
        With dgvSalesReport
            .DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
            .ColumnHeadersHeight = 40
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
            .ColumnHeadersDefaultCellStyle.ForeColor = textColor
            .DefaultCellStyle.SelectionBackColor = accentColor
            .DefaultCellStyle.SelectionForeColor = Color.White
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248)
            .RowTemplate.Height = 35
            .GridColor = Color.FromArgb(230, 230, 230)
            .BorderStyle = BorderStyle.None
            .EnableHeadersVisualStyles = False
        End With
    End Sub

    Private Sub RoundCorners(control As Control, cornerRadius As Integer)
        Dim path As New GraphicsPath()
        path.AddArc(0, 0, cornerRadius * 2, cornerRadius * 2, 180, 90)
        path.AddArc(control.Width - cornerRadius * 2, 0, cornerRadius * 2, cornerRadius * 2, 270, 90)
        path.AddArc(control.Width - cornerRadius * 2, control.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90)
        path.AddArc(0, control.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90)
        path.CloseAllFigures()

        control.Region = New Region(path)
    End Sub

    Private Sub SetupSalesReportControl()
        ' Header Panel
        pnlHeader.Dock = DockStyle.Top
        pnlHeader.Height = 50

        ' Header Label
        lblHeader.Text = "Sales Report"
        lblHeader.Dock = DockStyle.Fill
        lblHeader.TextAlign = ContentAlignment.MiddleLeft
        lblHeader.Padding = New Padding(15, 0, 0, 0)

        ' Add controls to header panel
        pnlHeader.Controls.Add(lblHeader)

        ' Configure filter panel
        pnlSalesFilter.Dock = DockStyle.Top
        pnlSalesFilter.Height = 100
        pnlSalesFilter.Padding = New Padding(15, 0, 15, 0)

        ' Icon for date range (optional)
        Dim picDateIcon As New PictureBox()
        picDateIcon.Size = New Size(16, 16)
        picDateIcon.Location = New Point(15, 17)
        picDateIcon.BackColor = Color.Transparent

        ' Date range selector
        Dim lblSalesDateRange As New Label() With {
            .Text = "Date Range:",
            .Location = New Point(15, 15),
            .AutoSize = True,
            .Font = New Font("Segoe UI", 9.5F)
        }

        cboSalesDateRange.Items.AddRange({"Daily", "Weekly", "Monthly", "Yearly", "Custom"})
        cboSalesDateRange.SelectedIndex = 2 ' Monthly by default
        cboSalesDateRange.Location = New Point(100, 12)
        cboSalesDateRange.Width = 120
        cboSalesDateRange.FlatStyle = FlatStyle.Flat

        ' Start date
        Dim lblSalesStartDate As New Label() With {
            .Text = "Start Date:",
            .Location = New Point(240, 15),
            .AutoSize = True,
            .Font = New Font("Segoe UI", 9.5F)
        }

        dtpSalesStartDate.Format = DateTimePickerFormat.Short
        dtpSalesStartDate.Location = New Point(315, 12)
        dtpSalesStartDate.Width = 120

        ' End date
        Dim lblSalesEndDate As New Label() With {
            .Text = "End Date:",
            .Location = New Point(450, 15),
            .AutoSize = True,
            .Font = New Font("Segoe UI", 9.5F)
        }

        dtpSalesEndDate.Format = DateTimePickerFormat.Short
        dtpSalesEndDate.Location = New Point(520, 12)
        dtpSalesEndDate.Width = 120

        ' Generate report button
        btnGenerateSalesReport.Text = "Generate Report"
        btnGenerateSalesReport.Location = New Point(15, 55)
        btnGenerateSalesReport.Width = 150
        btnGenerateSalesReport.Height = 35
        AddHandler btnGenerateSalesReport.Paint, AddressOf RoundButton_Paint

        ' Export button
        btnExportSales.Text = "Export to CSV"
        btnExportSales.Location = New Point(180, 55)
        btnExportSales.Width = 150
        btnExportSales.Height = 35
        AddHandler btnExportSales.Paint, AddressOf RoundButton_Paint

        ' Total Sales label
        lblTotalSales.Text = "Total Sales: ₱0.00"
        lblTotalSales.Location = New Point(680, 55)
        lblTotalSales.AutoSize = True
        lblTotalSales.Anchor = AnchorStyles.Top Or AnchorStyles.Right

        ' Info label
        Dim lblInfo As New Label()
        lblInfo.Text = "Select date range and click Generate Report to view sales data"
        lblInfo.Location = New Point(350, 65)
        lblInfo.AutoSize = True
        lblInfo.ForeColor = Color.FromArgb(120, 120, 120)
        lblInfo.Font = New Font("Segoe UI", 8.0F, FontStyle.Italic)

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
        pnlSalesFilter.Controls.Add(lblInfo)

        ' Configure data grid
        dgvSalesReport.Dock = DockStyle.Fill
        dgvSalesReport.AllowUserToAddRows = False
        dgvSalesReport.AllowUserToDeleteRows = False
        dgvSalesReport.ReadOnly = True
        dgvSalesReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvSalesReport.BackgroundColor = Color.White
        dgvSalesReport.RowHeadersVisible = False

        ' Add divider between filter and grid
        Dim pnlDivider As New Panel()
        pnlDivider.Height = 1
        pnlDivider.Dock = DockStyle.Top
        pnlDivider.BackColor = Color.FromArgb(230, 230, 230)

        ' Add panels and grid to the user control
        Me.Controls.Add(dgvSalesReport)
        Me.Controls.Add(pnlDivider)
        Me.Controls.Add(pnlSalesFilter)
        Me.Controls.Add(pnlHeader)

        ' Set default dates
        Dim today As DateTime = DateTime.Today
        Dim firstDayOfMonth As New DateTime(today.Year, today.Month, 1)
        Dim lastDayOfMonth As DateTime = firstDayOfMonth.AddMonths(1).AddDays(-1)
        dtpSalesStartDate.Value = firstDayOfMonth
        dtpSalesEndDate.Value = lastDayOfMonth

        ' Generate initial report
        GenerateSalesReport()
    End Sub

    Private Sub RoundButton_Paint(sender As Object, e As PaintEventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim radius As Integer = 8

        Dim rect As New Rectangle(0, 0, btn.Width, btn.Height)
        Dim path As New GraphicsPath()

        path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90)
        path.AddArc(rect.Width - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90)
        path.AddArc(rect.Width - radius * 2, rect.Height - radius * 2, radius * 2, radius * 2, 0, 90)
        path.AddArc(rect.X, rect.Height - radius * 2, radius * 2, radius * 2, 90, 90)
        path.CloseAllFigures()

        btn.Region = New Region(path)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        ' Add slight shadow to filter panel
        Dim filterPanelRect As New Rectangle(0, pnlHeader.Height, Me.Width, pnlSalesFilter.Height)
        ControlPaint.DrawBorder(e.Graphics, filterPanelRect, Color.FromArgb(220, 220, 220), ButtonBorderStyle.Solid)
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
            ' Show loading indicator
            Cursor = Cursors.WaitCursor

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

            ' Format columns for better readability
            FormatDataGridColumns()

            ' Calculate and display total sales
            Dim totalSales As Decimal = 0
            For Each row As DataRow In salesData.Rows
                If Not IsDBNull(row("AmountPaid")) Then
                    totalSales += Convert.ToDecimal(row("AmountPaid"))
                End If
            Next

            ' Update total label
            lblTotalSales.Text = $"Total Sales: ₱{totalSales:N2}"

            ' Restore cursor
            Cursor = Cursors.Default
        Catch ex As Exception
            Cursor = Cursors.Default
            MessageBox.Show("Error generating sales report: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub FormatDataGridColumns()
        ' Format currency columns
        If dgvSalesReport.Columns.Contains("AmountPaid") Then
            dgvSalesReport.Columns("AmountPaid").DefaultCellStyle.Format = "₱#,##0.00"
            dgvSalesReport.Columns("AmountPaid").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End If

        ' Format date columns
        For Each colName As String In {"EventDate", "PaymentDate"}
            If dgvSalesReport.Columns.Contains(colName) Then
                dgvSalesReport.Columns(colName).DefaultCellStyle.Format = "MMM dd, yyyy"
            End If
        Next

        ' Color-code payment status
        If dgvSalesReport.Columns.Contains("PaymentStatus") Then
            For Each row As DataGridViewRow In dgvSalesReport.Rows
                Dim status As String = If(row.Cells("PaymentStatus").Value IsNot Nothing, row.Cells("PaymentStatus").Value.ToString(), "")
                Select Case status.ToLower()
                    Case "paid"
                        row.Cells("PaymentStatus").Style.ForeColor = Color.FromArgb(76, 175, 80)  ' Green
                    Case "pending"
                        row.Cells("PaymentStatus").Style.ForeColor = Color.FromArgb(255, 152, 0)  ' Orange
                    Case "cancelled", "refunded"
                        row.Cells("PaymentStatus").Style.ForeColor = Color.FromArgb(244, 67, 54)  ' Red
                End Select
            Next
        End If
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
                ' Show loading cursor
                Cursor = Cursors.WaitCursor

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

                ' Reset cursor
                Cursor = Cursors.Default

                ' Show cute export success notification
                Dim notification As New Form()
                notification.FormBorderStyle = FormBorderStyle.None
                notification.Size = New Size(300, 80)
                notification.StartPosition = FormStartPosition.CenterScreen
                notification.BackColor = Color.FromArgb(76, 175, 80)
                notification.Opacity = 0.9

                Dim lblNotification As New Label()
                lblNotification.Text = "🎉 Export Completed Successfully!"
                lblNotification.ForeColor = Color.White
                lblNotification.Font = New Font("Segoe UI", 12, FontStyle.Bold)
                lblNotification.Dock = DockStyle.Fill
                lblNotification.TextAlign = ContentAlignment.MiddleCenter

                notification.Controls.Add(lblNotification)

                ' Round corners
                Dim path As New GraphicsPath()
                path.AddArc(0, 0, 20, 20, 180, 90)
                path.AddArc(notification.Width - 20, 0, 20, 20, 270, 90)
                path.AddArc(notification.Width - 20, notification.Height - 20, 20, 20, 0, 90)
                path.AddArc(0, notification.Height - 20, 20, 20, 90, 90)
                path.CloseAllFigures()
                notification.Region = New Region(path)

                ' Show notification briefly then fade out
                notification.Show()
                Dim timer As New Timer()
                timer.Interval = 2000

                AddHandler timer.Tick, Sub(s, args)
                                           notification.Close()
                                           timer.Stop()
                                       End Sub
                timer.Start()
            End If
        Catch ex As Exception
            Cursor = Cursors.Default
            MessageBox.Show("Error exporting data: " & ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class