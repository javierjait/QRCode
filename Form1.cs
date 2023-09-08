using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using static QRCode.Utils;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace QRCode
{
    public partial class Form1 : Form
    {
        private readonly Stopwatch sw = new Stopwatch();
        private int totalCount = 0;
        private string carpeta;
        private CancellationTokenSource cts;
        private readonly ConcurrentDictionary<string, byte[]> qrs = new ConcurrentDictionary<string, byte[]>();
        private FileType fileType;
        private readonly string keyCarpeta = "carpeta";
        private readonly string keyUltimo = "ultimo";
        private readonly int maxCodes = 10000;

        public Form1()
        {
            InitializeComponent();

            // To report progress from the background worker we need to set this property
            backgroundWorker1.WorkerReportsProgress = true;
            // This event will be raised on the worker thread when the worker starts
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            // This event will be raised when we call ReportProgress
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);

            numericUpDownCantidad.Maximum = maxCodes;
            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Key";
            comboBox1.DataSource = EnumList.Of<FileType>();
            //comboBox1.DataSource = Enum.GetValues(typeof(FileType));
            comboBox1.Text = Enum.GetName(typeof(FileType), FileType.PNG);

            setCarpetaValue(Path.GetFullPath(GetSetting(keyCarpeta)));
        }

        #region form events
        private void buttonGenerar_Click(object sender, EventArgs e)
        {
            // Do form validation
            if (CheckErrors())
            {
                return;
            }

            // Check codes quantity 
            if (numericUpDownCantidad.Value == 0 || numericUpDownCantidad.Value > maxCodes) return;

            // Try to create folder
            try
            {
                Directory.CreateDirectory(labelCarpetaValue.Text);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            // Initializa count and dictionary
            totalCount = 0;
            qrs.Clear();
            DateTime fecha = dateTimePicker1.Value;
            for (int i = 0; i < numericUpDownCantidad.Value; i++)
            {
                qrs.TryAdd($"{fecha:yyyyMMddHHmmss}", null);
                fecha = fecha.AddSeconds(1);
            }

            // Start the background worker
            if (backgroundWorker1.IsBusy != true)
            {
                //fileType = (FileType)comboBox1.SelectedItem;
                KeyValuePair<FileType, string> kv = (KeyValuePair<FileType, string>)comboBox1.SelectedItem;
                fileType = kv.Key;
                setControlsEnabled(false);
                // Init progress bar
                progressBar1.Value = 0;
                progressBar1.Maximum = fileType == FileType.PDF_UNICO ? (int)numericUpDownCantidad.Value * 2
                    : (int)numericUpDownCantidad.Value;
                sw.Start();
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                cts?.Cancel();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            setControlsEnabled(true);
            string ultimo = GetSetting(keyUltimo);
            if (ultimo != null)
            {
                SetUltimo(ultimo);
            }
        }

        private void buttonCarpeta_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = carpeta;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (HasWritePermissionOnDir(dialog.SelectedPath))
                    {
                        setCarpetaValue(dialog.SelectedPath);
                    }
                    else
                    {
                        MessageBox.Show("La carpeta no tiene permiso de escritura", "Error");
                    }

                }
            }
        }

        private void numericUpDownCantidad_ValueChanged(object sender, EventArgs e)
        {
            bool buttonEnabled = (sender as NumericUpDown).Value != 0;

            buttonGenerar.Enabled = buttonEnabled;
            if (!buttonEnabled)
            {
                errorProvider1.Clear();
            }
        }
        #endregion

        #region form functions
        /// <summary>
        /// Set enabled/disabled controls and WaitCursor
        /// </summary>
        /// <param name="enabled">enabled value</param>
        private void setControlsEnabled(bool enabled)
        {
            SuspendLayout();
            Application.UseWaitCursor = !enabled;
            buttonGenerar.Enabled = enabled && numericUpDownCantidad.Value != 0;
            buttonCarpeta.Enabled = enabled;
            buttonCancel.Enabled = !enabled;
            buttonCancel.Cursor = Cursors.AppStarting;
            progressBar1.Enabled = !enabled;
            textBoxHTTP.Enabled = enabled;
            textBoxPrefijo.Enabled = enabled;
            numericUpDownCantidad.Enabled = enabled;
            dateTimePicker1.Enabled = enabled;
            ResumeLayout(false);
            PerformLayout();
            Application.DoEvents();
        }

        /// <summary>
        /// Do check validation
        /// </summary>
        /// <returns>true if there are errors. false is no errors</returns>
        private bool CheckErrors()
        {
            errorProvider1.Clear(); // Clear error provider

            /*if (string.IsNullOrWhiteSpace(textBoxHTTP.Text)) // Check empty http text
            {
                errorProvider1.SetError(textBoxHTTP, "Valor requerido");
            } else if (!Uri.IsWellFormedUriString(textBoxHTTP.Text, UriKind.Absolute)) { // Check valid URL
                errorProvider1.SetError(textBoxHTTP, "No es una URL válida");
            }*/

            if (string.IsNullOrEmpty(textBoxPrefijo.Text))
            {
                errorProvider1.SetError(textBoxPrefijo, "Valor requerido");
                return true;
            }

            return false;
            //return CountErrors(errorProvider1) > 0;
        }

        private void setCarpetaValue(string folder)
        {
            carpeta = folder;
            labelCarpetaValue.Text = folder;
        }
        #endregion


        #region backgroundWorker events
        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            cts = new CancellationTokenSource();
            ParallelOptions options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 2 * Environment.ProcessorCount,
                CancellationToken = cts.Token,
            };

            try
            {
                Parallel.ForEach(qrs, options, x =>
                {
                    byte[] qrCodeAsByteArr = GenerateQRCode($"{textBoxHTTP.Text.Trim()}{textBoxPrefijo.Text.Trim()}{x.Key}", fileType);
                    if (qrCodeAsByteArr != null)
                    {
                        try
                        {
                            if (fileType != FileType.PDF_UNICO)
                            {
                                File.WriteAllBytes($"{carpeta}\\{x.Key}.{fileType}", qrCodeAsByteArr);
                            }
                            qrs.TryUpdate(x.Key, qrCodeAsByteArr, null);
                        }
                        catch { }
                    }
                    backgroundWorker1.ReportProgress(Interlocked.Increment(ref totalCount));
                });

                if (fileType == FileType.PDF_UNICO)
                {
                    CreatePDFFile();
                }
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cts.Dispose();
            }
        }

        // Back on the 'UI' thread so we can update the progress bar
        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // The progress percentage is a property of e
            if (e.ProgressPercentage > progressBar1.Maximum)
            {
                progressBar1.Value = progressBar1.Maximum;
            }
            else
            {
                progressBar1.Value = e.ProgressPercentage;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = progressBar1.Maximum;
            setControlsEnabled(true);
            sw.Stop();

            try
            {
                string last = qrs.Where(x => x.Value != null).Select(x => x.Key).OrderBy(x => x).ToList().Last();
                SetUltimo(last);
                SetSetting(keyUltimo, last);
#if DEBUG
                MessageBox.Show(sw.Elapsed.ToString());
#endif
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
            finally
            {
                qrs.Clear();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Set último value
        /// </summary>
        /// <param name="ultimo"></param>
        private void SetUltimo(string ultimo)
        {
            labelUltimo.Text = $"Último generado: {ultimo}";
        }

        /// <summary>
        /// Count errors in errorProvider
        /// </summary>
        /// <returns>Count of errors</returns>
        private int CountErrors(ErrorProvider errorProvider)
        {
            int errors = 0;
            foreach (Control control in Controls)
            {
                if (!string.IsNullOrWhiteSpace(errorProvider.GetError(control)))
                {
                    errors++;
                }
            }

            return errors;
        }

        /// <summary>
        /// Get app settings
        /// </summary>
        /// <param name="key">Key to get setting</param>
        /// <returns>string if exists or null</returns>
        public string GetSetting(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif  
                return null;
            }
        }

        /// <summary>
        /// Set appsetting
        /// </summary>
        /// <param name="key">key to set setting</param>
        /// <param name="value">string value</param>
        public void SetSetting(string key, string value)
        {
            try
            {
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var keyValue = GetSetting(key);
                if (keyValue == null)
                {
                    configuration.AppSettings.Settings.Add(key, value);
                }
                else
                {
                    configuration.AppSettings.Settings[key].Value = value;
                }
                configuration.Save(ConfigurationSaveMode.Minimal, false);
                ConfigurationManager.RefreshSection(configuration.AppSettings.SectionInformation.Name);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }

        private void CreatePDFFile()
        {
            var images = qrs.Where(w => w.Value != null).OrderBy(o => o.Key).Select(s => s.Value).ToList();

            //Start creating PDF
            PdfDocument doc = new PdfDocument();
            int padding = 0;

            for (int i = 0; i < images.Count; i++)
            {
                //convert Image to XImage for PDF
                if (images[i] != null)
                {
                    using (MemoryStream stream = new MemoryStream(images[i]))
                    {
                        XGraphics xgr = null;
                        XImage img = null;
                        try
                        {
                            img = XImage.FromStream(stream);
                            PdfPage page = doc.AddPage();
                            page.Width = img.PointWidth + 2 * padding;
                            page.Height = img.PointHeight + 2 * padding;

                            xgr = XGraphics.FromPdfPage(page);
                            xgr.DrawImage(img, padding, padding);
                        }
                        finally
                        {

                            img?.Dispose();
                            xgr?.Dispose();
                        }
                    }
                    backgroundWorker1.ReportProgress(Interlocked.Increment(ref totalCount));
                }
            }

            try
            {
                doc.Save($"{carpeta}\\QR_{DateTime.Now:yyyyMMddHHmm}.pdf");
            }
            finally
            {
                doc.Close();
                doc?.Dispose();
            }
        }

        private void CreatePDFMultiFile()
        {
            var images = qrs.Where(w => w.Value != null).OrderBy(o => o.Key).Select(s => s.Value).ToList();

            //Start creating PDF
            PdfDocument doc = new PdfDocument();
            int padding = 0;
            int qrInPDF = 0;
            int pdfFile = 1;
            string pdfNameStart = $"QR_{DateTime.Now:yyyyMMddHHmm}";

            for (int i = 0; i < images.Count; i++)
            {
                //convert Image to XImage for PDF
                if (images[i] != null)
                {
                    using (MemoryStream stream = new MemoryStream(images[i]))
                    {
                        XImage img = XImage.FromStream(stream);
                        PdfPage page = doc.AddPage();
                        page.Width = img.PointWidth + 2 * padding;
                        page.Height = img.PointHeight + 2 * padding;

                        XGraphics xgr = XGraphics.FromPdfPage(page);
                        xgr.DrawImage(img, padding, padding);
                        img.Dispose();
                        xgr.Dispose();
                        qrInPDF++;
                        if (qrInPDF >= 10)
                        {
                            doc.Save($"{carpeta}\\{pdfNameStart}-{pdfFile}.pdf");
                            doc.Close();
                            doc.Dispose();
                            doc = new PdfDocument();
                            qrInPDF = 0;
                            pdfFile++;
                        }
                    }
                    backgroundWorker1.ReportProgress(Interlocked.Increment(ref totalCount));
                }
            }

            if (qrInPDF > 0)
            {
                doc.Save($"{carpeta}\\{pdfNameStart}-{pdfFile}.pdf");
            }
            doc.Close();
            doc.Dispose();
        }
    }
    #endregion
}
