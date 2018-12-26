using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using DNS.Protocol;
using DNS.Protocol.ResourceRecords;
using NetTools;
using Newtonsoft.Json;
using PacketDotNet;
using SharpPcap;
using SharpPcap.WinPcap;

namespace DnsPolygraph
{
    public partial class MainWin : Form
    {
        public static DataTable DtSales = new DataTable();

        private readonly Resolver _cloudFlare =
            new Resolver(1, "CloudFlare", "C", "https://cloudflare-dns.com/dns-query");

        private readonly Dictionary<int, Resolver> _dictResolvers = new Dictionary<int, Resolver>();

        private readonly Dictionary<int, Filter> _filters = new Dictionary<int, Filter>
        {
            {0, new Filter("0", "All", Color.FromArgb(255, 255, 255))},
            {1, new Filter("1", "Possible local DNS Spoof", Color.FromArgb(255, 0, 0))},
            {2, new Filter("2", "Same /16 Range", Color.FromArgb(218, 228, 255))},
            {3, new Filter("3", "Same /24 Range", Color.FromArgb(198, 255, 174))},
            {4, new Filter("4", "Local Resource", Color.FromArgb(0, 255, 255))},
            {5, new Filter("5", "Same Answer", Color.FromArgb(255, 255, 255))},
            {6, new Filter("6", "Unrelated", Color.FromArgb(208, 201, 202))},
            {7, new Filter("7", "Same L2 reverse IP domain", Color.FromArgb(255, 225, 193))},
            {8, new Filter("8", "Non-existing domain get public IP", Color.FromArgb(255, 189, 225))}
        };

        private readonly Resolver _google = new Resolver(0, "Google", "G", "https://dns.google.com/resolve");
        private ICaptureDevice _device;
        private WinPcapDeviceList _devices;

        private int _indexResolver;

        public MainWin()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            KeyPreview = true;
            KeyDown += MainForm_KeyDown;

            //Check if WinPcap installed and get interfaces
            if (!FillComboDevices())
                Application.Exit();

            //Flush DNS Cache
            DnsFlushResolverCache();

            //Add columns to the main DataGridView
            DtSales.Columns.Add("Date", typeof(string));
            DtSales.Columns.Add("Resolver", typeof(string));
            DtSales.Columns.Add("Domain", typeof(string));
            DtSales.Columns.Add("Response (Untrusted)", typeof(string));
            DtSales.Columns.Add("Response (DNS over HTTPS)", typeof(string));
            DtSales.Columns.Add("R", typeof(string));
            DtSales.Columns.Add("Info", typeof(string));
            DtSales.Columns.Add("Type", typeof(string));
            dgvMain.DataSource = DtSales;

            dgvMain.Columns[0].Width = 120;
            dgvMain.Columns[1].Width = 90;
            dgvMain.Columns[2].Width = 160;
            dgvMain.Columns[3].Width = 160;
            dgvMain.Columns[4].Width = 160;
            dgvMain.Columns[5].Width = 25;
            dgvMain.Columns[6].Width = 700;
            dgvMain.Columns[7].Visible = false;

            _dictResolvers.Add(_google.Id, _google);
            _dictResolvers.Add(_cloudFlare.Id, _cloudFlare);

            //Fill combobox with trusted resolvers
            cboResolvers.Items.Insert(_dictResolvers[0].Id, _dictResolvers[0].Name);
            cboResolvers.Items.Insert(_dictResolvers[1].Id, _dictResolvers[1].Name);
            cboResolvers.SelectedIndex = 0;
            _indexResolver = 0;

            cboResolvers.DropDownStyle = ComboBoxStyle.DropDownList;

            //Fill combobox with filters 
            foreach (var filter in _filters) cboFilters.Items.Insert(filter.Key, filter.Value.Description);

            //cboFilters.Items.AddRange(filtersNames);
            cboFilters.DrawMode = DrawMode.OwnerDrawFixed;
            cboFilters.DrawItem += CboFiltersDrawItem;
            cboFilters.SelectedIndexChanged += cboFilters_SelectedIndexChanged;

            //Reduce flickering
            dgvMain.DoubleBuffered(true);
        }


        [DllImport("dnsapi.dll", EntryPoint = "DnsFlushResolverCache")]
        private static extern uint DnsFlushResolverCache();

        private void dgvMain_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvMain.ClearSelection();
        }

        private bool FillComboDevices()
        {
            try
            {
                _devices = WinPcapDeviceList.Instance;
                if (_devices.Count < 1)
                {
                    MessageBox.Show(@"No devices were found on this machine",
                        @"No devices found", MessageBoxButtons.OK);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"This app requires WinPcap. Please refer to: https://www.winpcap.org/install/",
                    @"WinPcap Not Found", MessageBoxButtons.OK);

                Debug.WriteLine("[-] Error WinPcap: " + ex.Message);
                return false;
            }

            foreach (ICaptureDevice device in _devices)
            {
                var pattern = "FriendlyName:.*\n";
                var matchDevice = Regex.Match(device.ToString(), pattern);
                var friendly = matchDevice.ToString().TrimEnd('\r', '\n');
                cboInterfaces.Items.Add(friendly.Split(' ')[1] + ": " + device.Description);
            }

            cboInterfaces.DropDownStyle = ComboBoxStyle.DropDownList;
            return true;
        }

        private void cboFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFilters.SelectedIndex == 0)
                DtSales.DefaultView.RowFilter = "[Date] IS NOT NULL";
            else
                DtSales.DefaultView.RowFilter = $"[Type] LIKE '%{_filters[cboFilters.SelectedIndex].id}%'";
        }

        // Useful: http://csharphelper.com/blog/2016/03/make-a-combobox-display-colors-or-images-in-c/
        private void CboFiltersDrawItem(object sender, DrawItemEventArgs e)
        {
            const int MarginWidth = 6;
            const int MarginHeight = 0;
            if (e.Index < 0) return;

            // Clear the background
            e.DrawBackground();

            // Draw the color sample.
            var hgt = e.Bounds.Height - 1 * MarginHeight;
            var rect = new Rectangle(e.Bounds.X + MarginWidth, e.Bounds.Y + MarginHeight, hgt, hgt);
            var cbo = sender as ComboBox;

            //Color color = (Color)cbo.Items[e.Index];
            using (var brush = new SolidBrush(_filters[e.Index].Color))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            // Draw the color's name to the right.
            using (var font = new Font(cbo.Font.FontFamily,
                cbo.Font.Size * 1f))
            {
                using (var sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    var x = hgt + 2 * MarginWidth;
                    var y = e.Bounds.Y + e.Bounds.Height / 2;
                    e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    e.Graphics.DrawString(cboFilters.Items[e.Index].ToString(), font,
                        Brushes.Black, x, y, sf);
                }
            }

            e.DrawFocusRectangle();
        }

        private void cboInterfaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboInterfaces.SelectedIndex > -1)
                btnCapture.Enabled = true;
        }

        // Open the interface to start capturing data using a BPF filter from sharppcap
        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (cboInterfaces.SelectedIndex > -1)
            {
                _device = _devices[cboInterfaces.SelectedIndex];
                _device.OnPacketArrival += device_OnPacketArrival;
                var readTimeoutMilliseconds = 1000;
                _device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
                var filter = "udp and src port 53";
                _device.Filter = filter;
                _device.StartCapture();
                btnCapture.Enabled = false;
                btnStop.Enabled = true;
                cboInterfaces.Enabled = false;
            }
        }

        private void UpdateRow(string time, string resolver, string domain, string resUntrusted, string restrusted,
            string ini, string info, string type = null)
        {
            if (dgvMain.InvokeRequired)
                dgvMain.Invoke(new MethodInvoker(delegate
                {
                    DtSales.Rows.Add(time, resolver, domain, resUntrusted, restrusted, ini, info, type);
                    if (cbScroll.Checked && dgvMain.RowCount > 0)
                        dgvMain.FirstDisplayedScrollingRowIndex = dgvMain.RowCount - 1;
                }));
        }

        // Get DNS response and parse fields with https://github.com/kapetan/dns/tree/master/DNS
        private void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            var packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
            var udp = (UdpPacket)packet.Extract(typeof(UdpPacket));
            if (udp == null) return;
            var payload = udp.PayloadData;
            var response = Response.FromArray(payload);
            if (response.AnswerRecords.Count <= 0) return;
            var p = IpPacket.GetEncapsulated(packet);

            var childThread = new Thread(() => UpdateListView(response, p.SourceAddress.ToString()));
            childThread.Start();
        }


        private void UpdateListView(Response response, string ipResolver)
        {
            // Get list of IP addresses returned by the untrusted resolver
            var untrustedAddresses = response.AnswerRecords
                .Where(r => r.Type == RecordType.A)
                .Cast<IPAddressResourceRecord>()
                .Select(r => r.IPAddress)
                .ToList();

            var domainReq = response.AnswerRecords[0].Name.ToString();
            var info = "";
            var type = "6";
            var time = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            var initial = "";

            // If passive mode 'on' ignore DNS over HTTPS; just records DNS responses
            if (cbPassive.Checked)
            {
                UpdateRow(time, ipResolver, domainReq, "", "", "", "Passive Mode", "0");
                return;
            }

            initial = _dictResolvers[_indexResolver].Ini;
            var objResolver = new TrustedResolver(domainReq, _dictResolvers[_indexResolver].Api, "A");
            var trusted = objResolver.DnsOverHttps();
            var untrusted = string.Join(",", untrustedAddresses);

            if (trusted != "")
            {
                if (trusted == untrusted)
                {
                    type = "5";
                }
                else
                {
                    if (untrustedAddresses.Count == 1)
                    {
                        if (trusted.Split(',').Length > 1)
                        {
                            info = "IP addresses do not match";
                            type = "6";
                        }
                        else if (IsPrivate(untrusted) && trusted != "NXDOMAIN")
                        {
                            type = "1";
                            info = "Public domain gets a private IP (possible DNS Spoof)";
                        }
                        else if (IsPrivate(untrusted))
                        {
                            type = "4";
                            info = "Local resource";
                        }
                        else if (trusted == "NXDOMAIN")
                        {
                            type = "8";
                            info = "Non-existent domain gets a public IP (possible DNS Spoof)";
                        }
                        else // At this point trusted and untrusted are public IP addresses
                        {
                            var rangeC = IPAddressRange.Parse(trusted + "/255.255.255.0");
                            var rangeB = IPAddressRange.Parse(trusted + "/255.255.0.0");
                            if (rangeC.Contains(IPAddress.Parse(untrusted)))
                            {
                                info = "Same /24 Domain: " + rangeC;
                                type = "3";
                            }
                            else if (rangeB.Contains(IPAddress.Parse(untrusted)))
                            {
                                info = "Same /16 Domain: " + rangeB;
                                type = "2";
                            }
                            else
                            {
                                var ptrUntrust = IpReverseLookup(untrusted);
                                var ptrTrust = IpReverseLookup(trusted);

                                if (CompareSubdomain(ptrUntrust, ptrTrust))
                                {
                                    info = $"Second-level domain in common: {ptrUntrust} / {ptrTrust}";
                                    type = "7";
                                }
                                else
                                {
                                    if (CompareSubdomain(ptrUntrust, domainReq))
                                    {
                                        info = "Second-level domain in common: " + ptrUntrust;
                                        type = "J";
                                    }
                                    else
                                    {
                                        if (cbWhois.Checked)
                                        {
                                            IPAddress address;
                                            if (IPAddress.TryParse(untrusted, out address))
                                            {
                                                var ipInfo = new IpInfo();
                                                info = WhoisIP(untrusted);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (untrustedAddresses.Count > 1 && trusted != "NXDOMAIN")
                    {
                        var trustedAddrSep = trusted.Split(',')
                            .Select(IPAddress.Parse)
                            .ToList();
                        if (trusted.Split(',').Length == untrustedAddresses.Count &&
                            untrustedAddresses.All(trustedAddrSep.Contains))
                        {
                            type = "5";
                        }
                        else
                        {
                            info = "IP addresses do not match";
                        }
                    }
                }
            }

            UpdateRow(time, ipResolver, domainReq, untrusted, trusted, initial, info, type);
        }

        private bool CompareSubdomain(string domainA, string domainB)
        {
            var tmpA = domainA.Split('.');
            var tmpB = domainB.Split('.');

            if (tmpA.Count() > 2 && tmpB.Count() > 2)
            {
                var sldDomainA = tmpA[tmpA.Count() - 3] + "." + tmpA[tmpA.Count() - 2];
                var sldDomainB = tmpB[tmpB.Count() - 3] + "." + tmpB[tmpB.Count() - 2];
                if (sldDomainA == sldDomainB)
                    return true;
            }

            return false;
        }

        private string IpReverseLookup(string ipAddress)
        {
            var reversePtr = string.Join(".", ipAddress.Split('.').Reverse()) + ".in-addr.arpa";
            var objResolver = new TrustedResolver(reversePtr, _dictResolvers[_indexResolver].Api, "PTR");
            var ptrValue = objResolver.DnsOverHttps();
            if (ptrValue != "" && ptrValue != "NXDOMAIN")
                return ptrValue;
            return "";
        }

        // Useful: https://stackoverflow.com/questions/8113546/how-to-determine-whether-an-ip-address-in-private
        private bool IsPrivate(string ipAddress)
        {
            var ipParts = ipAddress.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s)).ToArray();

            if (ipParts[0] == 10 ||
                ipParts[0] == 192 && ipParts[1] == 168 ||
                ipParts[0] == 172 && ipParts[1] >= 16 && ipParts[1] <= 31)
                return true;
            return false;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (btnCapture.Enabled == false)
            {
                _device.StopCapture();
                _device.Close();
                btnCapture.Enabled = true;
                btnStop.Enabled = false;
                cboInterfaces.Enabled = true;
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var opened = false;
            foreach (Form frm in Application.OpenForms)
                if (frm is about)
                {
                    frm.BringToFront();
                    opened = true;
                    break;
                }

            if (opened)
                return;

            var about = new about();
            about.Show();
        }


        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "F4")
            {
                var dialogResult = MessageBox.Show(@"Are you sure?", @"Delete data", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DtSales.Clear();
                    numRows.Text = "0";
                }
            }

            if (e.KeyCode.ToString() == "F5")
            {
                var result = DnsFlushResolverCache();
                if (result == 1)
                    MessageBox.Show(@"DNS cache cleared successfully");
            }

            if (e.KeyCode.ToString() == "F6") DomainCountToolStripMenuItem_Click(sender, e);

            if (e.Control && e.KeyCode == Keys.E) dgvMain.SelectAll();
        }

        private void MainWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.WindowsShutDown)
                if (btnStop.Enabled)
                {
                    _device.StopCapture();
                    _device.Close();
                }
        }

        private void ClearGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialogResult = MessageBox.Show(@"Are you sure?", @"Delete data", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes) DtSales.Clear();
        }

        private void TxtFilter_TextChanged(object sender, EventArgs e)
        {
            DtSales.DefaultView.RowFilter =
                $"[Date] LIKE '%{txtFilter.Text}%' or [Resolver] LIKE '%{txtFilter.Text}%' or [Domain] LIKE '%{txtFilter.Text}%' or [Response (Untrusted)] LIKE '%{txtFilter.Text}%' or [Response (DNS over HTTPS)] LIKE '%{txtFilter.Text}%'";
        }

        private void DgvMain_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            numRows.Text = (e.RowIndex + 1).ToString();
            if (e.RowIndex < 0) return;
            var type = dgvMain.Rows[e.RowIndex].Cells[7].Value.ToString();
            dgvMain.Rows[e.RowIndex].DefaultCellStyle.BackColor = _filters[int.Parse(type)].Color;

            if (e.RowIndex > 0)
            {
                var resolveOld = dgvMain.Rows[e.RowIndex - 1].Cells[1].Value;
                var resolveNew = dgvMain.Rows[e.RowIndex - 0].Cells[1].Value;

                if (resolveOld != null)
                    if (resolveOld.ToString() != resolveNew.ToString())
                    {
                        dgvMain.Rows[e.RowIndex].Cells[1].Style.ForeColor = Color.White;
                        dgvMain.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.FromArgb(72, 19, 246);
                    }
            }
        }


        private void FlushDNSCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = DnsFlushResolverCache();
            if (result == 1)
                MessageBox.Show(@"DNS cache cleared successfully");
            else
                MessageBox.Show(@"Unexpected error flushing the DNS cache", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DtSales.DefaultView.Count <= 0) return;
            try
            {
                using (var sfd = new SaveFileDialog { Filter = "CSV|*.csv", ValidateNames = true })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                        using (var sw = new StreamWriter(new FileStream(sfd.FileName, FileMode.Create),
                            Encoding.UTF8)
                        )
                        {
                            var sb = new StringBuilder();
                            foreach (DataRow drForm in DtSales.DefaultView.ToTable().Rows
                            ) //Falta Comprobar si están vacios los campos. Si no genera excepción
                                sb.AppendLine(
                                    $"{drForm[0]};{drForm[1]};{drForm[2]};{drForm[3]};{drForm[4]};{drForm[5]};{drForm[6]}");
                            sw.WriteLineAsync(sb.ToString());
                            MessageBox.Show(@"Data successfully exported", @"Info", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                }
            }
            catch (IOException)
            {
                MessageBox.Show(@"Error saving data. Please, check if the file is not already opened",
                    @"Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show(@"Unknown error",
                    @"Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtFilter_Click(object sender, EventArgs e)
        {
            txtFilter.Text = "";
        }

        // Useful: https://www.c-sharpcorner.com/article/working-with-menus-in-C-Sharp/
        private void DgvMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (dgvMain.GetCellCount(DataGridViewElementStates.Selected) <= 0) return;
            if (e.Button != MouseButtons.Right) return;
            var m = new ContextMenu();
            m.MenuItems.Add(new MenuItem("Copy Row/s", CopyRow_Click));
            m.MenuItems.Add(new MenuItem("Copy Item", CopyItem_Click));
            m.MenuItems.Add(new MenuItem("Delete Row/s", DeleteRow_Click));

            var potIp = dgvMain.CurrentCell.Value.ToString();
            if (dgvMain.CurrentRow != null)
            {
                var rowIndex = dgvMain.CurrentRow.Index;
                if (IPAddress.TryParse(potIp, out var address))
                    m.MenuItems.Add(new MenuItem("Get IP info: " + potIp,
                        delegate { WhoisInfo_Click(sender, e, potIp, rowIndex); }));
            }

            m.Show(dgvMain, new Point(e.X, e.Y));
        }

        private void DeleteRow_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow item in dgvMain.SelectedRows) dgvMain.Rows.RemoveAt(item.Index);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[-] Error deleting rows: " + ex.Message);
            }
        }

        private string WhoisIP(string untrusted)
        {
            try
            {
                var data = new WebClient().DownloadString("http://ip-api.com/json/" + untrusted);
                var ipInfo = JsonConvert.DeserializeObject<IpInfo>(data);
                return $"[{ipInfo.Query}] Org: {ipInfo.Org}, AS: {ipInfo.As}, ISP: {ipInfo.Isp}, " +
                       $"Country: {ipInfo.Country}, Region: {ipInfo.Region}";
            }
            catch (Exception)
            {
                return "Error getting data from ip-api.com";
            }
        }

        private void WhoisInfo_Click(object sender, EventArgs e, string potIP, int rowIndex)
        {
            var untrusted = potIP;
            ((DataRowView)dgvMain.Rows[rowIndex].DataBoundItem).Row["Info"] = WhoisIP(untrusted);
            dgvMain.Refresh();
        }

        private void CopyRow_Click(object sender, EventArgs e)
        {
            if (dgvMain.GetCellCount(DataGridViewElementStates.Selected) <= 0) return;
            try
            {
                Clipboard.SetDataObject(dgvMain.GetClipboardContent());
            }
            catch (ExternalException ex)
            {
                Debug.WriteLine("[-] Error Clipboard when copy row: " + ex.Message);
            }
        }

        private void CopyItem_Click(object sender, EventArgs e)
        {
            if (dgvMain.GetCellCount(DataGridViewElementStates.Selected) <= 0) return;
            try
            {
                Clipboard.SetDataObject(dgvMain.CurrentCell.Value.ToString());
            }
            catch (ExternalException ex)
            {
                Debug.WriteLine("[-] Error Clipboard when copy item: " + ex.Message);
            }
        }

        private void CbMatched_CheckedChanged(object sender, EventArgs e)
        {
            if (cbMatched.Checked)
            {
                DtSales.DefaultView.RowFilter = "[Type] NOT LIKE \'5\'";
                cboFilters.Enabled = false;
            }
            else
            {
                DtSales.DefaultView.RowFilter = "[Date] IS NOT NULL";
                cboFilters.Enabled = true;
                cboFilters.SelectedIndex = 0;
            }
        }

        private void DomainCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var opened = false;
            foreach (Form frm in Application.OpenForms)
            {
                if (!(frm is UniqueDomains)) continue;
                frm.BringToFront();
                opened = true;
                break;
            }

            if (opened)
                return;

            var formTotalDomains = new UniqueDomains();
            formTotalDomains.Show();
        }

        private void CboResolvers_SelectedIndexChanged(object sender, EventArgs e)
        {
            _indexResolver = cboResolvers.SelectedIndex;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }

    public class Resolver
    {
        public Resolver(int id, string name, string ini, string api)
        {
            Id = id;
            Name = name;
            Ini = ini;
            Api = api;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Ini { get; set; }
        public string Api { get; set; }
    }

    public class IpInfo
    {
        public string Region { get; set; } = "";
        public string Country { get; set; } = "";
        public string Org { get; set; } = "";
        public string Isp { get; set; } = "";
        public string As { get; set; } = "";
        public string Query { get; set; } = "";
    }

    public class Filter
    {
        public Filter(string id, string description, Color color)
        {
            this.id = id;
            Description = description;
            Color = color;
        }

        public string Description { get; set; }
        public Color Color { get; set; }
        public string id { get; set; }
    }

    public static class BufferMethod
    {
        public static void DoubleBuffered(this DataGridView dgv, bool value)
        {
            var dgvType = dgv.GetType();
            var info = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            if (info != null) info.SetValue(dgv, value, null);
        }
    }
}