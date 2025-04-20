Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Configuration

Public Class AccountantBookingControl
    Inherits UserControl

    ' UI Controls
    Private dgvBookings As DataGridView
    Private dgvPayments As DataGridView
    Private customerPanel As Panel
    Private mainContainer As TableLayoutPanel
    Private lblCustomerName, lblContact, lblEmail, lblAddress, lblCity, lblProvince As Label
    Private pnlHeader As Panel
    Private btnRefresh As Button
    Private lblNoBookings As Label
    Private lblNoPayments As Label

    ' Colors
    Private ReadOnly HeaderColor As Color = Color.FromArgb(143, 188, 219)      ' Soft Blue
    Private ReadOnly AccentColor As Color = Color.FromArgb(252, 186, 195)      ' Soft Pink
    Private ReadOnly BodyColor As Color = Color.FromArgb(245, 247, 250)        ' Light Gray-Blue
    Private ReadOnly TextColor As Color = Color.FromArgb(72, 84, 96)           ' Dark Gray-Blue
    Private ReadOnly GridHeaderColor As Color = Color.FromArgb(163, 213, 200)  ' Mint Green

    Public Sub New()
        InitializeComponent()
        Me.AutoScroll = True
        SetupLayout()
        LoadBookings()
        Me.BackColor = BodyColor
        Me.Font = New Font("Segoe UI", 9.5F)
        Me.ForeColor = TextColor
    End Sub

    Private Sub AccountantBookingControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Control loaded
    End Sub

    Private Sub SetupLayout()
        ' Main container with padding
        mainContainer = New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 5,
            .Padding = New Padding(15),
            .BackColor = BodyColor,
            .AutoScroll = True
        }
        Me.Controls.Add(mainContainer)

        ' Header with title and refresh button
        CreateHeader()

        ' Booking section
        CreateBookingsSection()

        ' Customer info section
        CreateCustomerSection()

        ' Payments section
        CreatePaymentsSection()
    End Sub

    Private Sub CreateHeader()
        pnlHeader = New Panel With {
            .Dock = DockStyle.Top,
            .Height = 50,
            .Padding = New Padding(5),
            .BackColor = HeaderColor
        }

        ' Title Label
        Dim lblTitle As New Label With {
            .Text = "Booking Management",
            .Font = New Font("Segoe UI Semibold", 14),
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(10, 15)
        }

        ' Refresh Button
        btnRefresh = New Button With {
            .Text = "↻ Refresh",
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.White,
            .ForeColor = HeaderColor,
            .Font = New Font("Segoe UI", 9.5F, FontStyle.Bold),
            .Size = New Size(90, 30),
            .Location = New Point(pnlHeader.Width - 105, 10),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
            .Cursor = Cursors.Hand
        }
        btnRefresh.FlatAppearance.BorderColor = Color.White
        AddHandler btnRefresh.Click, AddressOf RefreshData

        pnlHeader.Controls.Add(lblTitle)
        pnlHeader.Controls.Add(btnRefresh)
        mainContainer.Controls.Add(pnlHeader)
        mainContainer.SetRow(pnlHeader, 0)
    End Sub

    Private Sub CreateBookingsSection()
        ' Bookings Section
        Dim pnlBookings As New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.White,
            .Padding = New Padding(10),
            .Margin = New Padding(0, 10, 0, 5)
        }

        ' Section Label
        Dim lblBookingSection As New Label With {
            .Text = "🗓️ Bookings",
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .ForeColor = TextColor,
            .AutoSize = True,
            .Dock = DockStyle.Top,
            .Padding = New Padding(0, 0, 0, 5)
        }

        ' No bookings message
        lblNoBookings = New Label With {
            .Text = "No bookings found.",
            .Font = New Font("Segoe UI", 10),
            .ForeColor = Color.Gray,
            .TextAlign = ContentAlignment.MiddleCenter,
            .Dock = DockStyle.Fill,
            .Visible = False
        }

        ' Create rounded panel for the grid
        Dim bookingsContainer As New Panel With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(1),
            .BackColor = Color.LightGray
        }

        ' Setup the DataGridView
        dgvBookings = New DataGridView With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False,
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .RowHeadersVisible = False,
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            .ScrollBars = ScrollBars.Both,
            .AllowUserToResizeColumns = True,
            .AllowUserToResizeRows = False
        }
        dgvBookings.RowTemplate.Height = 35

        ' Style the DataGridView
        dgvBookings.EnableHeadersVisualStyles = False
        dgvBookings.ColumnHeadersDefaultCellStyle.BackColor = GridHeaderColor
        dgvBookings.ColumnHeadersDefaultCellStyle.ForeColor = TextColor
        dgvBookings.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
        dgvBookings.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dgvBookings.ColumnHeadersDefaultCellStyle.Padding = New Padding(5)
        dgvBookings.ColumnHeadersHeight = 40

        dgvBookings.DefaultCellStyle.BackColor = Color.White
        dgvBookings.DefaultCellStyle.ForeColor = TextColor
        dgvBookings.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 237, 246)
        dgvBookings.DefaultCellStyle.SelectionForeColor = TextColor
        dgvBookings.DefaultCellStyle.Padding = New Padding(3)

        dgvBookings.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 253)

        ' Add event handlers
        AddHandler dgvBookings.CellClick, AddressOf dgvBookings_CellClick
        AddHandler dgvBookings.CellFormatting, AddressOf dgvBookings_CellFormatting
        AddHandler dgvBookings.DataError, AddressOf dgvBookings_DataError

        bookingsContainer.Controls.Add(dgvBookings)
        bookingsContainer.Controls.Add(lblNoBookings)
        pnlBookings.Controls.Add(bookingsContainer)
        pnlBookings.Controls.Add(lblBookingSection)

        mainContainer.Controls.Add(pnlBookings)
        mainContainer.SetRow(pnlBookings, 1)
    End Sub

    Private Sub CreateCustomerSection()
        ' Customer Info Panel
        Dim pnlCustomerWrapper As New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.White,
            .Padding = New Padding(10),
            .Margin = New Padding(0, 5, 0, 5)
        }

        ' Section Label
        Dim lblCustomerSection As New Label With {
            .Text = "👤 Customer Details",
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .ForeColor = TextColor,
            .AutoSize = True,
            .Dock = DockStyle.Top,
            .Padding = New Padding(0, 0, 0, 5)
        }

        ' Customer Info Content
        customerPanel = New Panel With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(10),
            .BackColor = Color.FromArgb(252, 252, 252)
        }

        ' Create info cards layout
        Dim infoLayout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 3,
            .RowCount = 2,
            .Padding = New Padding(5),
            .CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        }

        ' Equal column and row styles
        For i As Integer = 0 To 2
            infoLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33F))
        Next
        infoLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 50))
        infoLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 50))

        ' Create customer info fields
        CreateInfoCard(infoLayout, "Name", lblCustomerName, 0, 0)
        CreateInfoCard(infoLayout, "Contact", lblContact, 1, 0)
        CreateInfoCard(infoLayout, "Email", lblEmail, 2, 0)
        CreateInfoCard(infoLayout, "Address", lblAddress, 0, 1)
        CreateInfoCard(infoLayout, "City", lblCity, 1, 1)
        CreateInfoCard(infoLayout, "Province", lblProvince, 2, 1)

        customerPanel.Controls.Add(infoLayout)
        pnlCustomerWrapper.Controls.Add(customerPanel)
        pnlCustomerWrapper.Controls.Add(lblCustomerSection)

        mainContainer.Controls.Add(pnlCustomerWrapper)
        mainContainer.SetRow(pnlCustomerWrapper, 2)
    End Sub

    Private Sub CreateInfoCard(container As TableLayoutPanel, title As String, ByRef valueLabel As Label, col As Integer, row As Integer)
        Dim cardPanel As New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.White,
            .Padding = New Padding(5)
        }

        Dim titleLabel As New Label With {
            .Text = title,
            .Font = New Font("Segoe UI", 9, FontStyle.Regular),
            .ForeColor = Color.Gray,
            .AutoSize = True,
            .Dock = DockStyle.Top
        }

        valueLabel = New Label With {
            .Text = "-",
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .ForeColor = TextColor,
            .AutoSize = False,
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleLeft
        }

        cardPanel.Controls.Add(valueLabel)
        cardPanel.Controls.Add(titleLabel)

        container.Controls.Add(cardPanel, col, row)
    End Sub

    Private Sub CreatePaymentsSection()
        ' Payments Section
        Dim pnlPayments As New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.White,
            .Padding = New Padding(10),
            .Margin = New Padding(0, 5, 0, 0)
        }

        ' Section Label
        Dim lblPaymentSection As New Label With {
            .Text = "💰 Payment History",
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .ForeColor = TextColor,
            .AutoSize = True,
            .Dock = DockStyle.Top,
            .Padding = New Padding(0, 0, 0, 5)
        }

        ' No payments message
        lblNoPayments = New Label With {
            .Text = "Select a booking to view payment history.",
            .Font = New Font("Segoe UI", 10),
            .ForeColor = Color.Gray,
            .TextAlign = ContentAlignment.MiddleCenter,
            .Dock = DockStyle.Fill,
            .Visible = True
        }

        ' Create rounded panel for the grid
        Dim paymentsContainer As New Panel With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(1),
            .BackColor = Color.LightGray,
            .AutoScroll = True
        }

        ' Setup the DataGridView
        dgvPayments = New DataGridView With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False,
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .RowHeadersVisible = False,
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            .Visible = False,
            .ScrollBars = ScrollBars.Both
        }
        dgvPayments.RowTemplate.Height = 35

        ' Style the DataGridView
        dgvPayments.EnableHeadersVisualStyles = False
        dgvPayments.ColumnHeadersDefaultCellStyle.BackColor = GridHeaderColor
        dgvPayments.ColumnHeadersDefaultCellStyle.ForeColor = TextColor
        dgvPayments.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
        dgvPayments.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dgvPayments.ColumnHeadersDefaultCellStyle.Padding = New Padding(5)
        dgvPayments.ColumnHeadersHeight = 40

        dgvPayments.DefaultCellStyle.BackColor = Color.White
        dgvPayments.DefaultCellStyle.ForeColor = TextColor
        dgvPayments.DefaultCellStyle.SelectionBackColor = Color.FromArgb(252, 236, 238)
        dgvPayments.DefaultCellStyle.SelectionForeColor = TextColor
        dgvPayments.DefaultCellStyle.Padding = New Padding(3)

        dgvPayments.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 248, 249)

        ' Add event handlers
        AddHandler dgvPayments.CellFormatting, AddressOf dgvPayments_CellFormatting
        AddHandler dgvPayments.DataError, AddressOf dgvPayments_DataError

        paymentsContainer.Controls.Add(dgvPayments)
        paymentsContainer.Controls.Add(lblNoPayments)
        pnlPayments.Controls.Add(paymentsContainer)
        pnlPayments.Controls.Add(lblPaymentSection)

        mainContainer.Controls.Add(pnlPayments)
        mainContainer.SetRow(pnlPayments, 3)
    End Sub

    Private Sub LoadBookings()
        Dim query As String = "SELECT BookingID, CustomerID, EventID, ServiceID, BookingDate, BookedBy, BookingStatus, TotalAmount, PaymentStatus, Remarks FROM booking"
        Dim dt As DataTable = GetData(query)

        dgvBookings.DataSource = dt

        ' Configure column formatting
        If dgvBookings.Columns.Count > 0 Then
            If dgvBookings.Columns.Contains("BookingDate") Then
                dgvBookings.Columns("BookingDate").DefaultCellStyle.Format = "MMM dd, yyyy"
            End If

            ' Set column headers with more user-friendly names
            Dim friendlyNames As New Dictionary(Of String, String) From {
                {"BookingID", "ID"},
                {"CustomerID", "Customer ID"},
                {"EventID", "Event ID"},
                {"ServiceID", "Service ID"},
                {"BookingDate", "Date"},
                {"BookedBy", "Booked By"},
                {"BookingStatus", "Status"},
                {"TotalAmount", "Amount"},
                {"PaymentStatus", "Payment Status"},
                {"Remarks", "Notes"}
            }

            For Each col As DataGridViewColumn In dgvBookings.Columns
                If friendlyNames.ContainsKey(col.Name) Then
                    col.HeaderText = friendlyNames(col.Name)
                End If
            Next

            ' Hide some columns to simplify the view
            If dgvBookings.Columns.Contains("EventID") Then dgvBookings.Columns("EventID").Visible = False
            If dgvBookings.Columns.Contains("ServiceID") Then dgvBookings.Columns("ServiceID").Visible = False
            If dgvBookings.Columns.Contains("CustomerID") Then dgvBookings.Columns("CustomerID").Visible = False
        End If

        ' Show message if no data
        lblNoBookings.Visible = (dt.Rows.Count = 0)
        dgvBookings.Visible = (dt.Rows.Count > 0)
        AdjustColumnWidths()
    End Sub

    Private Sub EnableMultilineText()
        If dgvBookings.Columns.Contains("Remarks") Then
            dgvBookings.Columns("Remarks").DefaultCellStyle.WrapMode = DataGridViewTriState.True
        End If

        If dgvPayments.Columns.Contains("Remarks") Then
            dgvPayments.Columns("Remarks").DefaultCellStyle.WrapMode = DataGridViewTriState.True
        End If
    End Sub

    Private Sub LoadPayments(bookingID As Integer)
        Dim query As String = "SELECT PaymentID, PaymentDate, AmountPaid, PaymentMethod, ReferenceNumber, PaymentStatus, ProcessedBy, PaymentTime, Balance, DiscountAmount, RefundedAmount, Remarks, ORNumber FROM payment WHERE BookingID = @BookingID"
        Dim parameters As New Dictionary(Of String, Object) From {
            {"@BookingID", bookingID}
        }

        Dim dt As DataTable = GetData(query, parameters)
        dgvPayments.DataSource = dt

        ' Configure column formatting
        If dgvPayments.Columns.Count > 0 Then
            If dgvPayments.Columns.Contains("PaymentDate") Then
                dgvPayments.Columns("PaymentDate").DefaultCellStyle.Format = "MMM dd, yyyy"
            End If

            If dgvPayments.Columns.Contains("PaymentTime") Then
                dgvPayments.Columns("PaymentTime").DefaultCellStyle.Format = "hh:mm tt"
            End If

            ' Set column headers with more user-friendly names
            Dim friendlyNames As New Dictionary(Of String, String) From {
                {"PaymentID", "ID"},
                {"PaymentDate", "Date"},
                {"AmountPaid", "Amount Paid"},
                {"PaymentMethod", "Method"},
                {"ReferenceNumber", "Reference #"},
                {"PaymentStatus", "Status"},
                {"ProcessedBy", "Processed By"},
                {"PaymentTime", "Time"},
                {"Balance", "Balance"},
                {"DiscountAmount", "Discount"},
                {"RefundedAmount", "Refunded"},
                {"Remarks", "Notes"},
                {"ORNumber", "OR Number"}
            }

            For Each col As DataGridViewColumn In dgvPayments.Columns
                If friendlyNames.ContainsKey(col.Name) Then
                    col.HeaderText = friendlyNames(col.Name)
                End If
            Next

            ' Hide some columns to simplify the view
            If dgvPayments.Columns.Contains("PaymentID") Then dgvPayments.Columns("PaymentID").Visible = False
        End If

        ' Show/hide appropriate elements
        lblNoPayments.Visible = (dt.Rows.Count = 0)
        dgvPayments.Visible = (dt.Rows.Count > 0)

        If dt.Rows.Count = 0 Then
            lblNoPayments.Text = "No payment records found for this booking."
        End If
    End Sub

    Private Sub LoadCustomer(customerID As Integer)
        Dim query As String = "SELECT FirstName, LastName, MiddleName, ContactNumber, Email, AddressLine, City, Province FROM customer WHERE CustomerID = @CustomerID"
        Dim parameters As New Dictionary(Of String, Object) From {
            {"@CustomerID", customerID}
        }

        Dim dt As DataTable = GetData(query, parameters)
        If dt.Rows.Count > 0 Then
            Dim row = dt.Rows(0)
            lblCustomerName.Text = $"{row("LastName")}, {row("FirstName")} {row("MiddleName")}".Trim()
            lblContact.Text = If(row("ContactNumber") Is DBNull.Value, "-", row("ContactNumber").ToString())
            lblEmail.Text = If(row("Email") Is DBNull.Value, "-", row("Email").ToString())
            lblAddress.Text = If(row("AddressLine") Is DBNull.Value, "-", row("AddressLine").ToString())
            lblCity.Text = If(row("City") Is DBNull.Value, "-", row("City").ToString())
            lblProvince.Text = If(row("Province") Is DBNull.Value, "-", row("Province").ToString())
        Else
            lblCustomerName.Text = "-"
            lblContact.Text = "-"
            lblEmail.Text = "-"
            lblAddress.Text = "-"
            lblCity.Text = "-"
            lblProvince.Text = "-"
        End If
    End Sub

    Private Sub dgvBookings_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvBookings.Rows(e.RowIndex)
            Dim bookingID As Integer = Convert.ToInt32(row.Cells("BookingID").Value)
            Dim customerID As Integer = Convert.ToInt32(row.Cells("CustomerID").Value)

            LoadPayments(bookingID)
            LoadCustomer(customerID)

            ' Change background color of selected row
            dgvBookings.DefaultCellStyle.SelectionBackColor = AccentColor
        End If
    End Sub

    Private Sub dgvBookings_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.ColumnIndex >= 0 AndAlso e.RowIndex >= 0 Then
            Dim columnName As String = dgvBookings.Columns(e.ColumnIndex).Name
            If columnName = "TotalAmount" Then
                If e.Value IsNot Nothing AndAlso Not IsDBNull(e.Value) Then
                    Try
                        Dim numericValue As Decimal = Convert.ToDecimal(e.Value)
                        e.Value = String.Format("₱{0:#,##0.00}", numericValue)
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        e.FormattingApplied = True
                    Catch ex As Exception
                        e.Value = "-"
                        e.FormattingApplied = True
                    End Try
                Else
                    e.Value = "-"
                    e.FormattingApplied = True
                End If
            End If
        End If
    End Sub

    Private Sub dgvPayments_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.ColumnIndex >= 0 AndAlso e.RowIndex >= 0 Then
            Dim columnName As String = dgvPayments.Columns(e.ColumnIndex).Name
            If columnName = "AmountPaid" OrElse columnName = "Balance" OrElse columnName = "DiscountAmount" OrElse columnName = "RefundedAmount" Then
                If e.Value IsNot Nothing AndAlso Not IsDBNull(e.Value) Then
                    Try
                        Dim numericValue As Decimal = Convert.ToDecimal(e.Value)
                        e.Value = String.Format("₱{0:#,##0.00}", numericValue)
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        e.FormattingApplied = True
                    Catch ex As Exception
                        e.Value = "-"
                        e.FormattingApplied = True
                    End Try
                Else
                    e.Value = "-"
                    e.FormattingApplied = True
                End If
            End If
        End If
    End Sub

    Private Sub AdjustColumnWidths()
        ' Notes column typically needs more width
        If dgvBookings.Columns.Contains("Remarks") Then
            dgvBookings.Columns("Remarks").MinimumWidth = 200
        End If

        ' Set last column to fill remaining space
        If dgvBookings.Columns.Count > 0 Then
            dgvBookings.Columns(dgvBookings.Columns.Count - 1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If

        ' Similar for payments grid
        If dgvPayments.Columns.Contains("Remarks") Then
            dgvPayments.Columns("Remarks").MinimumWidth = 200
        End If

        If dgvPayments.Columns.Count > 0 Then
            dgvPayments.Columns(dgvPayments.Columns.Count - 1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
    End Sub

    Private Sub dgvBookings_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        ' Suppress data errors (e.g., formatting issues)
        e.Cancel = True
    End Sub

    Private Sub dgvPayments_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        ' Suppress data errors (e.g., formatting issues)
        e.Cancel = True
    End Sub

    Private Sub RefreshData(sender As Object, e As EventArgs)
        ' Visual feedback for refresh button
        btnRefresh.BackColor = AccentColor
        btnRefresh.Text = "Refreshing..."
        Application.DoEvents()

        ' Reset customer info
        lblCustomerName.Text = "-"
        lblContact.Text = "-"
        lblEmail.Text = "-"
        lblAddress.Text = "-"
        lblCity.Text = "-"
        lblProvince.Text = "-"

        ' Clear payments and show message
        dgvPayments.DataSource = Nothing
        lblNoPayments.Text = "Select a booking to view payment history."
        lblNoPayments.Visible = True
        dgvPayments.Visible = False

        ' Reload booking data
        LoadBookings()

        ' Reset button after short delay
        System.Threading.Thread.Sleep(300)
        btnRefresh.BackColor = Color.White
        btnRefresh.Text = "↻ Refresh"
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If Not DesignMode Then
            If mainContainer IsNot Nothing Then
                ' Calculate total minimum height (should exceed the control's height to activate scrolling)
                Dim totalMinHeight As Integer = 50 + 250 + 220 + 300 + 30  ' Header + Bookings + Customer + Payments + padding

                ' Set the TableLayoutPanel to have a minimum size
                mainContainer.MinimumSize = New Size(Me.Width - 40, totalMinHeight)

                ' Set all rows to fixed height
                mainContainer.RowStyles.Clear()
                mainContainer.RowStyles.Add(New RowStyle(SizeType.Absolute, 50))     ' Header
                mainContainer.RowStyles.Add(New RowStyle(SizeType.Absolute, 250))    ' Bookings 
                mainContainer.RowStyles.Add(New RowStyle(SizeType.Absolute, 220))    ' Customer
                mainContainer.RowStyles.Add(New RowStyle(SizeType.Absolute, 300))    ' Payments
            End If
        End If
    End Sub
End Class