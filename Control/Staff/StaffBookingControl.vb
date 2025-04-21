Imports System.Data
Imports MySql.Data.MySqlClient
Imports System.Drawing

Public Class StaffBookingControl
    Inherits UserControl

    Private dgvBookings As DataGridView
    Private txtAmount, txtRemarks, txtDiscount As TextBox
    Private cmbCustomerID, cmbEventID, cmbServiceID, cmbStatus, cmbPayment, cmbRefund As ComboBox
    Private dtpBookingDate, dtpEventDate, dtpTime As DateTimePicker
    Private currentBookingID As Integer = -1
    Private searchBox As TextBox

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        LoadBookings()
    End Sub

    Private Sub SetupLayout()
        ' Set control background
        Me.BackColor = Color.FromArgb(240, 240, 240)

        ' Header Panel with title and search
        Dim headerPanel As New Panel With {
            .Dock = DockStyle.Top,
            .Height = 60,
            .BackColor = Color.FromArgb(52, 152, 219) ' Blue header color
        }

        Dim lblTitle As New Label With {
            .Text = "Booking Management",
            .Font = New Font("Segoe UI", 16, FontStyle.Bold),
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(15, 15)
        }

        searchBox = New TextBox With {
            .Width = 250,
            .Height = 30,
            .Font = New Font("Segoe UI", 10),
            .Anchor = AnchorStyles.Right Or AnchorStyles.Top,
            .Location = New Point(headerPanel.Width - 270, 15)
        }

        AddHandler searchBox.TextChanged, AddressOf SearchBox_TextChanged

        Dim searchIcon As New Label With {
            .Text = "🔍",
            .Font = New Font("Segoe UI", 12),
            .ForeColor = Color.White,
            .AutoSize = True,
            .Anchor = AnchorStyles.Right Or AnchorStyles.Top,
            .Location = New Point(headerPanel.Width - 290, 18)
        }

        headerPanel.Controls.Add(lblTitle)
        headerPanel.Controls.Add(searchBox)
        headerPanel.Controls.Add(searchIcon)

        ' DataGridView with modern styling
        dgvBookings = New DataGridView With {
            .Dock = DockStyle.Fill,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .BorderStyle = BorderStyle.None,
            .BackgroundColor = Color.White,
            .GridColor = Color.FromArgb(224, 224, 224),
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False,
            .ReadOnly = True,
            .RowHeadersVisible = False
        }

        ' DataGridView Styling
        dgvBookings.DefaultCellStyle.SelectionBackColor = Color.FromArgb(87, 166, 245)
        dgvBookings.DefaultCellStyle.Font = New Font("Segoe UI", 9)
        dgvBookings.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvBookings.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        dgvBookings.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(70, 70, 70)
        dgvBookings.ColumnHeadersHeight = 40
        dgvBookings.RowTemplate.Height = 35

        AddHandler dgvBookings.CellClick, AddressOf dgvBookings_CellClick

        ' Input Controls Container Panel
        Dim panelControls As New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 320,
            .BackColor = Color.White,
            .Padding = New Padding(20)
        }

        ' Add shadow effect (border)
        AddHandler panelControls.Paint, Sub(sender As Object, e As PaintEventArgs)
                                            Dim topBorder As New Pen(Color.FromArgb(200, 200, 200))
                                            e.Graphics.DrawLine(topBorder, 0, 0, panelControls.Width, 0)
                                        End Sub

        ' Input Fields
        cmbCustomerID = CreateStyledComboBox("Select Customer")
        cmbEventID = CreateStyledComboBox("Select Event")
        cmbServiceID = CreateStyledComboBox("Select Service")

        txtAmount = CreateStyledTextBox("Total Amount")
        txtRemarks = CreateStyledTextBox("Remarks")
        txtDiscount = CreateStyledTextBox("Discount")

        cmbStatus = CreateStyledComboBox()
        cmbStatus.Items.AddRange({"Pending", "Confirmed", "Cancelled"})

        cmbPayment = CreateStyledComboBox()
        cmbPayment.Items.AddRange({"Paid", "Unpaid", "Partially Paid"})

        cmbRefund = CreateStyledComboBox()
        cmbRefund.Items.AddRange({"Refunded", "Not Refunded"})

        ' Date/Time pickers with custom styling
        dtpBookingDate = New DateTimePicker With {
            .Width = 250,
            .Font = New Font("Segoe UI", 10),
            .Format = DateTimePickerFormat.Short
        }

        dtpEventDate = New DateTimePicker With {
            .Width = 250,
            .Font = New Font("Segoe UI", 10),
            .Format = DateTimePickerFormat.Short
        }

        dtpTime = New DateTimePicker With {
            .Width = 250,
            .Font = New Font("Segoe UI", 10),
            .Format = DateTimePickerFormat.Time,
            .ShowUpDown = True
        }

        ' Button Panel
        Dim buttonPanel As New FlowLayoutPanel With {
            .FlowDirection = FlowDirection.RightToLeft,
            .Dock = DockStyle.Bottom,
            .Height = 50,
            .Padding = New Padding(0, 10, 20, 0)
        }

        Dim btnAdd As New Button With {
            .Text = "Create Booking",
            .Width = 140,
            .Height = 35,
            .Font = New Font("Segoe UI", 10),
            .BackColor = Color.FromArgb(52, 152, 219),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        btnAdd.FlatAppearance.BorderSize = 0
        AddHandler btnAdd.Click, AddressOf btnAdd_Click

        Dim btnUpdate As New Button With {
            .Text = "Update Booking",
            .Width = 140,
            .Height = 35,
            .Font = New Font("Segoe UI", 10),
            .BackColor = Color.FromArgb(46, 204, 113),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        btnUpdate.FlatAppearance.BorderSize = 0
        AddHandler btnUpdate.Click, AddressOf btnUpdate_Click

        Dim btnCancel As New Button With {
            .Text = "Cancel Booking",
            .Width = 140,
            .Height = 35,
            .Font = New Font("Segoe UI", 10),
            .BackColor = Color.FromArgb(231, 76, 60),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        btnCancel.FlatAppearance.BorderSize = 0
        AddHandler btnCancel.Click, AddressOf btnCancel_Click

        Dim btnClear As New Button With {
            .Text = "Clear Fields",
            .Width = 140,
            .Height = 35,
            .Font = New Font("Segoe UI", 10),
            .BackColor = Color.FromArgb(149, 165, 166),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        btnClear.FlatAppearance.BorderSize = 0
        AddHandler btnClear.Click, AddressOf btnClear_Click

        buttonPanel.Controls.AddRange({btnAdd, btnUpdate, btnCancel, btnClear})

        ' Layout structure - Left Column
        Dim leftColumn As New TableLayoutPanel With {
            .Width = 400,
            .Height = 230,
            .ColumnCount = 2,
            .RowCount = 5,
            .Location = New Point(20, 20)
        }
        leftColumn.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        leftColumn.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        leftColumn.Controls.Add(CreateFieldLabel("Customer:"), 0, 0)
        leftColumn.Controls.Add(cmbCustomerID, 1, 0)
        leftColumn.Controls.Add(CreateFieldLabel("Event:"), 0, 1)
        leftColumn.Controls.Add(cmbEventID, 1, 1)
        leftColumn.Controls.Add(CreateFieldLabel("Service:"), 0, 2)
        leftColumn.Controls.Add(cmbServiceID, 1, 2)
        leftColumn.Controls.Add(CreateFieldLabel("Amount:"), 0, 3)
        leftColumn.Controls.Add(txtAmount, 1, 3)
        leftColumn.Controls.Add(CreateFieldLabel("Discount:"), 0, 4)
        leftColumn.Controls.Add(txtDiscount, 1, 4)

        ' Layout structure - Right Column
        Dim rightColumn As New TableLayoutPanel With {
            .Width = 400,
            .Height = 230,
            .ColumnCount = 2,
            .RowCount = 5,
            .Location = New Point(450, 20)
        }
        rightColumn.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        rightColumn.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        rightColumn.Controls.Add(CreateFieldLabel("Booking Date:"), 0, 0)
        rightColumn.Controls.Add(dtpBookingDate, 1, 0)
        rightColumn.Controls.Add(CreateFieldLabel("Event Date:"), 0, 1)
        rightColumn.Controls.Add(dtpEventDate, 1, 1)
        rightColumn.Controls.Add(CreateFieldLabel("Time:"), 0, 2)
        rightColumn.Controls.Add(dtpTime, 1, 2)
        rightColumn.Controls.Add(CreateFieldLabel("Status:"), 0, 3)
        rightColumn.Controls.Add(cmbStatus, 1, 3)
        rightColumn.Controls.Add(CreateFieldLabel("Payment:"), 0, 4)
        rightColumn.Controls.Add(cmbPayment, 1, 4)

        ' Bottom row for remarks and refund status
        Dim bottomRow As New TableLayoutPanel With {
            .Width = 830,
            .Height = 40,
            .ColumnCount = 4,
            .RowCount = 1,
            .Location = New Point(20, 260)
        }
        bottomRow.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 70))
        bottomRow.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60))
        bottomRow.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120))
        bottomRow.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40))

        bottomRow.Controls.Add(CreateFieldLabel("Remarks:"), 0, 0)
        bottomRow.Controls.Add(txtRemarks, 1, 0)
        bottomRow.Controls.Add(CreateFieldLabel("Refund Status:"), 2, 0)
        bottomRow.Controls.Add(cmbRefund, 3, 0)

        panelControls.Controls.Add(leftColumn)
        panelControls.Controls.Add(rightColumn)
        panelControls.Controls.Add(bottomRow)

        ' Container for grid
        Dim gridContainer As New Panel With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(20, 10, 20, 10)
        }
        gridContainer.Controls.Add(dgvBookings)

        ' Main layout structure
        Me.Controls.Add(gridContainer)
        Me.Controls.Add(headerPanel)
        Me.Controls.Add(panelControls)
        Me.Controls.Add(buttonPanel)

        ' Populate data
        PopulateCustomerDropdown()
        PopulateEventsDropdown()
        PopulateServicesDropdown()
    End Sub

    Private Function CreateStyledTextBox(placeholder As String) As TextBox
        Dim txt As New TextBox With {
            .Width = 250,
            .Height = 30,
            .Font = New Font("Segoe UI", 10),
            .BorderStyle = BorderStyle.FixedSingle
        }

        ' Add placeholder text functionality
        Dim placeholderText As String = placeholder
        txt.Tag = placeholderText
        txt.ForeColor = Color.Gray
        txt.Text = placeholderText

        AddHandler txt.Enter, Sub(sender As Object, e As EventArgs)
                                  If txt.Text = txt.Tag.ToString() Then
                                      txt.Text = ""
                                      txt.ForeColor = Color.Black
                                  End If
                              End Sub

        AddHandler txt.Leave, Sub(sender As Object, e As EventArgs)
                                  If String.IsNullOrWhiteSpace(txt.Text) Then
                                      txt.Text = txt.Tag.ToString()
                                      txt.ForeColor = Color.Gray
                                  End If
                              End Sub

        Return txt
    End Function

    Private Function CreateStyledComboBox(Optional placeholder As String = "") As ComboBox
        Dim cmb As New ComboBox With {
            .Width = 250,
            .Height = 30,
            .Font = New Font("Segoe UI", 10),
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .FlatStyle = FlatStyle.Flat
        }

        Return cmb
    End Function

    Private Function CreateFieldLabel(text As String) As Label
        Return New Label With {
            .text = text,
            .Font = New Font("Segoe UI", 10),
            .AutoSize = True,
            .TextAlign = ContentAlignment.MiddleRight
        }
    End Function

    Private Sub SearchBox_TextChanged(sender As Object, e As EventArgs)
        Try
            If dgvBookings.DataSource IsNot Nothing AndAlso TypeOf dgvBookings.DataSource Is DataTable Then
                Dim dt As DataTable = DirectCast(dgvBookings.DataSource, DataTable)
                Dim dv As New DataView(dt)

                If Not String.IsNullOrWhiteSpace(searchBox.Text) Then
                    ' Search in multiple columns
                    dv.RowFilter = String.Format("BookingStatus LIKE '%{0}%' OR PaymentStatus LIKE '%{0}%' OR Remarks LIKE '%{0}%'",
                        searchBox.Text.Replace("'", "''"))
                End If

                dgvBookings.DataSource = dv.ToTable()
            End If
        Catch ex As Exception
            MessageBox.Show("Error filtering data: " & ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadBookings()
        Try
            Dim query As String = "SELECT * FROM booking"
            Dim dt As DataTable = GetData(query)
            dgvBookings.DataSource = dt

            ' Format the grid after loading data
            If dgvBookings.Columns.Count > 0 Then
                ' Optional: Hide unnecessary columns
                dgvBookings.Columns("BookingID").Visible = False

                ' Rename column headers
                dgvBookings.Columns("BookingStatus").HeaderText = "Status"
                dgvBookings.Columns("PaymentStatus").HeaderText = "Payment"
                dgvBookings.Columns("TotalAmount").HeaderText = "Amount"
                dgvBookings.Columns("BookingDate").HeaderText = "Booked On"
                dgvBookings.Columns("EventDate").HeaderText = "Event Date"

                ' Conditional formatting for Status column
                AddHandler dgvBookings.CellFormatting, Sub(sender As Object, e As DataGridViewCellFormattingEventArgs)
                                                           If dgvBookings.Columns(e.ColumnIndex).Name = "BookingStatus" Then
                                                               If e.Value IsNot Nothing Then
                                                                   Select Case e.Value.ToString()
                                                                       Case "Confirmed"
                                                                           e.CellStyle.ForeColor = Color.Green
                                                                           e.CellStyle.Font = New Font(dgvBookings.DefaultCellStyle.Font, FontStyle.Bold)
                                                                       Case "Cancelled"
                                                                           e.CellStyle.ForeColor = Color.Red
                                                                       Case "Pending"
                                                                           e.CellStyle.ForeColor = Color.Orange
                                                                   End Select
                                                               End If
                                                           ElseIf dgvBookings.Columns(e.ColumnIndex).Name = "PaymentStatus" Then
                                                               If e.Value IsNot Nothing Then
                                                                   Select Case e.Value.ToString()
                                                                       Case "Paid"
                                                                           e.CellStyle.ForeColor = Color.Green
                                                                       Case "Unpaid"
                                                                           e.CellStyle.ForeColor = Color.Red
                                                                       Case "Partially Paid"
                                                                           e.CellStyle.ForeColor = Color.Blue
                                                                   End Select
                                                               End If
                                                           End If
                                                       End Sub
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to load bookings: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
        ' Validate required fields
        If cmbCustomerID.SelectedIndex = -1 OrElse cmbEventID.SelectedIndex = -1 OrElse
           cmbServiceID.SelectedIndex = -1 OrElse cmbStatus.SelectedIndex = -1 OrElse
           cmbPayment.SelectedIndex = -1 OrElse
           String.IsNullOrWhiteSpace(txtAmount.Text) OrElse txtAmount.Text = txtAmount.Tag.ToString() Then

            MessageBox.Show("Please fill all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedCustomerID As Object = DirectCast(cmbCustomerID.SelectedItem, ComboItem).Value
        Dim selectedEventID As Object = DirectCast(cmbEventID.SelectedItem, ComboItem).Value
        Dim selectedServiceID As Object = DirectCast(cmbServiceID.SelectedItem, ComboItem).Value

        Dim query As String = "INSERT INTO booking (CustomerID, EventID, ServiceID, BookingDate, BookedBy, BookingStatus, BookingTime, EventDate, TotalAmount, PaymentStatus, Remarks, DiscountApplied, RefundStatus, CreatedAt)
                           VALUES (@CustomerID, @EventID, @ServiceID, @BookingDate, @BookedBy, @BookingStatus, @BookingTime, @EventDate, @TotalAmount, @PaymentStatus, @Remarks, @DiscountApplied, @RefundStatus, NOW())"

        Dim parameters As New Dictionary(Of String, Object) From {
            {"@CustomerID", selectedCustomerID},
            {"@EventID", selectedEventID},
            {"@ServiceID", selectedServiceID},
            {"@BookingDate", dtpBookingDate.Value},
            {"@BookedBy", SessionInfo.LoggedInUserFullName},
            {"@BookingStatus", cmbStatus.Text},
            {"@BookingTime", dtpTime.Value.ToString("HH:mm:ss")},
            {"@EventDate", dtpEventDate.Value},
            {"@TotalAmount", If(txtAmount.Text = txtAmount.Tag.ToString(), "0", txtAmount.Text)},
            {"@PaymentStatus", cmbPayment.Text},
            {"@Remarks", If(txtRemarks.Text = txtRemarks.Tag.ToString(), "", txtRemarks.Text)},
            {"@DiscountApplied", If(txtDiscount.Text = txtDiscount.Tag.ToString(), "0", txtDiscount.Text)},
            {"@RefundStatus", If(cmbRefund.SelectedIndex = -1, "Not Refunded", cmbRefund.Text)}
        }

        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("Booking created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadBookings()
            ClearFields()
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs)
        If currentBookingID = -1 Then
            MessageBox.Show("Please select a booking to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Validate required fields
        If cmbCustomerID.SelectedIndex = -1 OrElse cmbEventID.SelectedIndex = -1 OrElse
           cmbServiceID.SelectedIndex = -1 OrElse cmbStatus.SelectedIndex = -1 OrElse
           cmbPayment.SelectedIndex = -1 OrElse
           String.IsNullOrWhiteSpace(txtAmount.Text) OrElse txtAmount.Text = txtAmount.Tag.ToString() Then

            MessageBox.Show("Please fill all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedCustomerID As Object = DirectCast(cmbCustomerID.SelectedItem, ComboItem).Value
        Dim selectedEventID As Object = DirectCast(cmbEventID.SelectedItem, ComboItem).Value
        Dim selectedServiceID As Object = DirectCast(cmbServiceID.SelectedItem, ComboItem).Value

        Dim query As String = "UPDATE booking SET CustomerID=@CustomerID, EventID=@EventID, ServiceID=@ServiceID, BookingDate=@BookingDate,
                               BookingStatus=@BookingStatus, BookingTime=@BookingTime, EventDate=@EventDate,
                               TotalAmount=@TotalAmount, PaymentStatus=@PaymentStatus, Remarks=@Remarks,
                               DiscountApplied=@DiscountApplied, RefundStatus=@RefundStatus
                               WHERE BookingID=@BookingID"

        Dim parameters As New Dictionary(Of String, Object) From {
            {"@BookingID", currentBookingID},
            {"@CustomerID", selectedCustomerID},
            {"@EventID", selectedEventID},
            {"@ServiceID", selectedServiceID},
            {"@BookingDate", dtpBookingDate.Value},
            {"@BookingStatus", cmbStatus.Text},
            {"@BookingTime", dtpTime.Value.ToString("HH:mm:ss")},
            {"@EventDate", dtpEventDate.Value},
            {"@TotalAmount", If(txtAmount.Text = txtAmount.Tag.ToString(), "0", txtAmount.Text)},
            {"@PaymentStatus", cmbPayment.Text},
            {"@Remarks", If(txtRemarks.Text = txtRemarks.Tag.ToString(), "", txtRemarks.Text)},
            {"@DiscountApplied", If(txtDiscount.Text = txtDiscount.Tag.ToString(), "0", txtDiscount.Text)},
            {"@RefundStatus", If(cmbRefund.SelectedIndex = -1, "Not Refunded", cmbRefund.Text)}
        }

        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("Booking updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadBookings()
            ClearFields()
            currentBookingID = -1
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs)
        If currentBookingID = -1 Then
            MessageBox.Show("Please select a booking to cancel.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If MessageBox.Show("Are you sure you want to cancel this booking?", "Confirm Cancellation",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

            Dim query As String = "UPDATE booking SET BookingStatus = 'Cancelled' WHERE BookingID = @BookingID"
            Dim parameters As New Dictionary(Of String, Object) From {
                {"@BookingID", currentBookingID}
            }

            If ExecuteQuery(query, parameters) Then
                MessageBox.Show("Booking cancelled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadBookings()
                ClearFields()
                currentBookingID = -1
            End If
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs)
        ClearFields()
        currentBookingID = -1
    End Sub

    Private Sub dgvBookings_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvBookings.Rows(e.RowIndex)

            ' Store the current booking ID for update/cancel operations
            currentBookingID = Convert.ToInt32(row.Cells("BookingID").Value)

            ' Populate customer dropdown
            For i As Integer = 0 To cmbCustomerID.Items.Count - 1
                Dim item As ComboItem = DirectCast(cmbCustomerID.Items(i), ComboItem)
                If item.Value.ToString() = row.Cells("CustomerID").Value.ToString() Then
                    cmbCustomerID.SelectedIndex = i
                    Exit For
                End If
            Next

            ' Populate event dropdown
            For i As Integer = 0 To cmbEventID.Items.Count - 1
                Dim item As ComboItem = DirectCast(cmbEventID.Items(i), ComboItem)
                If item.Value.ToString() = row.Cells("EventID").Value.ToString() Then
                    cmbEventID.SelectedIndex = i
                    Exit For
                End If
            Next

            ' Populate service dropdown
            For i As Integer = 0 To cmbServiceID.Items.Count - 1
                Dim item As ComboItem = DirectCast(cmbServiceID.Items(i), ComboItem)
                If item.Value.ToString() = row.Cells("ServiceID").Value.ToString() Then
                    cmbServiceID.SelectedIndex = i
                    Exit For
                End If
            Next

            ' Set other fields
            dtpBookingDate.Value = Convert.ToDateTime(row.Cells("BookingDate").Value)
            dtpEventDate.Value = Convert.ToDateTime(row.Cells("EventDate").Value)

            If DateTime.TryParse(row.Cells("BookingTime").Value.ToString(), Nothing) Then
                dtpTime.Value = DateTime.Parse(row.Cells("BookingTime").Value.ToString())
            End If

            ' Set text fields with real values (not placeholders)
            txtAmount.Text = row.Cells("TotalAmount").Value.ToString()
            txtAmount.ForeColor = Color.Black

            txtRemarks.Text = row.Cells("Remarks").Value.ToString()
            txtRemarks.ForeColor = Color.Black

            txtDiscount.Text = row.Cells("DiscountApplied").Value.ToString()
            txtDiscount.ForeColor = Color.Black

            ' Set dropdown values
            cmbStatus.Text = row.Cells("BookingStatus").Value.ToString()
            cmbPayment.Text = row.Cells("PaymentStatus").Value.ToString()
            cmbRefund.Text = row.Cells("RefundStatus").Value.ToString()

            ' Highlight Update button
            For Each ctrl As Control In Me.Controls
                If TypeOf ctrl Is FlowLayoutPanel Then
                    For Each btn As Control In ctrl.Controls
                        If TypeOf btn Is Button AndAlso btn.Text = "Update Booking" Then
                            btn.BackColor = Color.FromArgb(46, 204, 113)
                        End If
                    Next
                End If
            Next
        End If
    End Sub

    Private Sub PopulateCustomerDropdown()
        Dim query As String = "SELECT CustomerID, FirstName, LastName, MiddleName FROM customer WHERE Status = 'Active'"
        Dim dt As DataTable = GetData(query)
        cmbCustomerID.Items.Clear()
        For Each row As DataRow In dt.Rows
            Dim fullName As String = $"{row("FirstName")} {row("MiddleName")} {row("LastName")}"
            cmbCustomerID.Items.Add(New ComboItem(fullName.Trim(), row("CustomerID")))
        Next
        If cmbCustomerID.Items.Count > 0 Then
            cmbCustomerID.SelectedIndex = 0
        End If
    End Sub

    Private Sub PopulateEventsDropdown()
        Dim query As String = "SELECT EventID, EventName FROM event WHERE Status = 'Upcoming'"
        Dim dt As DataTable = GetData(query)
        cmbEventID.Items.Clear()
        For Each row As DataRow In dt.Rows
            cmbEventID.Items.Add(New ComboItem(row("EventName").ToString(), row("EventID")))
        Next
        If cmbEventID.Items.Count > 0 Then
            cmbEventID.SelectedIndex = 0
        End If
    End Sub

    Private Sub PopulateServicesDropdown()
        Dim query As String = "SELECT ServiceID, ServiceName FROM service_availed WHERE Status = 'Active'"
        Dim dt As DataTable = GetData(query)
        cmbServiceID.Items.Clear()
        For Each row As DataRow In dt.Rows
            cmbServiceID.Items.Add(New ComboItem(row("ServiceName").ToString(), row("ServiceID")))
        Next
        If cmbServiceID.Items.Count > 0 Then
            cmbServiceID.SelectedIndex = 0
        End If
    End Sub

    Private Sub ClearFields()
        ' Reset comboboxes
        cmbCustomerID.SelectedIndex = -1
        cmbEventID.SelectedIndex = -1
        cmbServiceID.SelectedIndex = -1
        cmbStatus.SelectedIndex = -1
        cmbPayment.SelectedIndex = -1
        cmbRefund.SelectedIndex = -1

        ' Reset date pickers
        dtpBookingDate.Value = DateTime.Now
        dtpEventDate.Value = DateTime.Now
        dtpTime.Value = DateTime.Now

        ' Reset text fields to show placeholders
        txtAmount.Text = txtAmount.Tag.ToString()
        txtAmount.ForeColor = Color.Gray

        txtRemarks.Text = txtRemarks.Tag.ToString()
        txtRemarks.ForeColor = Color.Gray

        txtDiscount.Text = txtDiscount.Tag.ToString()
        txtDiscount.ForeColor = Color.Gray

    End Sub

End Class